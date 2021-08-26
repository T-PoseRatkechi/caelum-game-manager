// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Utility functions related spr files.
    /// </summary>
    public class SprUtils
    {
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="SprUtils"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        public SprUtils(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Extracts all TMX files from <paramref name="sprFile"/> to <paramref name="outputDir"/>.
        /// </summary>
        /// <param name="sprFile">SPR files to extract from.</param>
        /// <param name="outputDir">Output directory.</param>
        public void ExtractTmx(string sprFile, string outputDir)
        {
            int numExtractedFiles = 0;
            using BinaryReader reader = new(File.OpenRead(sprFile));

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                // TMX0
                if (reader.ReadUInt32() == 0x30584d54)
                {
                    var tmxStartPostition = reader.BaseStream.Position - 12;

                    // Go back and get size of tmx;
                    reader.BaseStream.Seek(-8, SeekOrigin.Current);
                    var tmxSize = reader.ReadInt32();

                    // Seek to start of texture name bytes.
                    reader.BaseStream.Position += 28;
                    var nameBytes = reader.ReadBytes(28);
                    var name = Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');

                    reader.BaseStream.Position = tmxStartPostition;

                    var tmxBytes = reader.ReadBytes(tmxSize);

                    Directory.CreateDirectory(outputDir);
                    File.WriteAllBytes(Path.Join(outputDir, $"{name}.tmx"), tmxBytes);
                    numExtractedFiles++;

                    this.log.LogDebug("Extracted {FileName}.tmx", name);
                }
            }

            this.log.LogDebug("Extracted {NumExtracted} TMX files from {SprFile}.", numExtractedFiles, sprFile);
        }

        /// <summary>
        /// Creates a new file at <paramref name="outputFile"/> with <paramref name="inputTmxFile"/> inserted into <paramref name="sprFile"/>.
        /// </summary>
        /// <param name="sprFile">SPR files to insert TMX to.</param>
        /// <param name="inputTmxFile">TMX file to insert.</param>
        /// <param name="outputFile">Path of to output new file.</param>
        public void InsertTmxFast(string sprFile, string inputTmxFile, string outputFile)
        {
            var inputTmxName = Path.GetFileNameWithoutExtension(inputTmxFile);

            FileStream fs = File.OpenRead(sprFile);
            var tmxOffsetsList = this.GetTmxOffsetList(fs);

            int inputTmxOffset = -1;
            int inputTmxIndex = -1;

            for (int i = 0, total = tmxOffsetsList.Count; i < total; i++)
            {
                var currentTmxName = this.GetTmxNameByOffset(fs, tmxOffsetsList[i]);

                if (currentTmxName == inputTmxName)
                {
                    inputTmxOffset = tmxOffsetsList[i];
                    inputTmxIndex = i;
                    break;
                }
            }

            if (inputTmxOffset < 0)
            {
                throw new ArgumentException($@"Input TMX file ""{inputTmxName}"" was not found in SPR file ""{Path.GetFileName(sprFile)}"".");
            }

            FileStream outputfs = File.OpenWrite(outputFile);
            fs.Position = 0;

            // fs.CopyTo(outputfs, 0, inputTmxOffset);
            var originalBuffer = new byte[inputTmxOffset];
            fs.Read(originalBuffer);
            outputfs.Write(originalBuffer);

            fs.Position = inputTmxOffset + 4;
            byte[] originalSizeBuffer = new byte[4];
            fs.Read(originalSizeBuffer, 0, 4);
            var originalSize = BitConverter.ToInt32(originalSizeBuffer);

            var inputBytes = File.ReadAllBytes(inputTmxFile);
            outputfs.Write(inputBytes);

            // Copy like normal if offsets are normal.
            if (inputTmxIndex < tmxOffsetsList.Count - 1 && tmxOffsetsList[inputTmxIndex + 1] > inputTmxOffset)
            {
                fs.Position = tmxOffsetsList[inputTmxIndex + 1];
                fs.CopyTo(outputfs);
            }

            // Fix offsets for tmx after the one that was inserted.
            var adjustment = inputBytes.Length - originalSize;
            for (int i = inputTmxIndex + 1, total = tmxOffsetsList.Count; i < total; i++)
            {
                tmxOffsetsList[i] = tmxOffsetsList[i] + adjustment - 16;
            }

            // Write fixed offsets.
            outputfs.Position = 32;
            for (int i = 0, total = tmxOffsetsList.Count; i < total; i++)
            {
                outputfs.Position += 4;
                outputfs.Write(BitConverter.GetBytes(tmxOffsetsList[i]));
            }

            // Close streams.
            fs.Close();
            outputfs.Flush();
            outputfs.Close();
        }

        private string GetTmxNameByOffset(Stream stream, int offset)
        {
            stream.Position = offset + 36;

            var nameBytesBuffer = new byte[28];
            stream.Read(nameBytesBuffer, 0, 28);

            var name = Encoding.ASCII.GetString(nameBytesBuffer).TrimEnd('\0');

            return name;
        }

        private List<int> GetTmxOffsetList(Stream stream)
        {
            var tmxOffsetList = new List<int>();
            const int numTmxEntriesOffset = 20;
            const int tmxEntriesOffset = 32;

            using (BinaryReader reader = new(stream, Encoding.Default, true))
            {
                reader.BaseStream.Position = numTmxEntriesOffset;

                var numEntries = reader.ReadInt16();

                reader.BaseStream.Position = tmxEntriesOffset;
                for (int i = 0; i < numEntries; i++)
                {
                    reader.BaseStream.Position += 4;
                    tmxOffsetList.Add(reader.ReadInt32());
                }
            }

            return tmxOffsetList;
        }

        private Dictionary<string, int> GetTmxOffsetTable(Stream stream)
        {
            var tmxTable = new Dictionary<string, int>();
            const int numTmxEntriesOffset = 24;
            const int tmxEntriesOffset = 32;

            using (BinaryReader reader = new(stream))
            {
                reader.BaseStream.Position = numTmxEntriesOffset;

                var numEntries = reader.ReadInt32();

                reader.BaseStream.Position = tmxEntriesOffset;
                for (int i = 0; i < numEntries; i++)
                {
                    reader.BaseStream.Position += 4;
                    var currentTmxOffset = reader.ReadInt32();

                    var currentPosition = reader.BaseStream.Position;

                    // Get tmx entry name.
                    reader.BaseStream.Position = currentTmxOffset + 36;
                    var nameBytes = reader.ReadBytes(28);
                    var name = Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');

                    // Add entry.
                    tmxTable.Add(name, currentTmxOffset);

                    // Reset position in stream.
                    reader.BaseStream.Position = currentPosition;
                }
            }

            return tmxTable;
        }
    }
}
