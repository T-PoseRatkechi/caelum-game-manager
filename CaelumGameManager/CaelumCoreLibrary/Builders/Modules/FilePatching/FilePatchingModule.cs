// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.IO;
    using System.Text.Json;
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// File patching module.
    /// </summary>
    public class FilePatchingModule : IBuilderModule
    {
        private readonly ILogger log;
        private readonly IBuildLogger buildLogger;
        private readonly IGameFile unpacker;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePatchingModule"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="buildLogger">Build logger.</param>
        public FilePatchingModule(ILogger log, IBuildLogger buildLogger, IGameFile unpacker)
        {
            this.log = log;
            this.buildLogger = buildLogger;
            this.unpacker = unpacker;
        }

        /// <inheritdoc/>
        public void BuildCard(CardModel card, string outputDir, HashSet<string> builtCardFiles)
        {
            // Card patches dir.
            string cardPatchesFolder = Path.Join(card.InstallDirectory, "Data", "Patches");

            if (Directory.Exists(cardPatchesFolder))
            {
                // Get all patches files.
                var patchFiles = Directory.GetFiles(cardPatchesFolder, "*.json", SearchOption.AllDirectories);
                foreach (var patchFile in patchFiles)
                {
                    builtCardFiles.Add(patchFile);

                    var gamePatch = JsonSerializer.Deserialize<GamePatch>(File.ReadAllText(patchFile));

                    foreach (var patch in gamePatch.Patches)
                    {
                        if (patch is BinaryPatchFormat binaryPatch)
                        {
                            this.log.LogInformation($"{binaryPatch.File}\n{binaryPatch.Format}\n{binaryPatch.Offset}\n{binaryPatch.Data}");

                            var unpackedFile = this.unpacker.GetUnpackedGameFile(patch.File.Replace("{UnpackedGameFiles}", string.Empty));

                            using (BinaryWriter writer = new(File.Open(unpackedFile, FileMode.Open)))
                            {
                                writer.BaseStream.Seek(binaryPatch.Offset, SeekOrigin.Begin);

                                var patchBytes = ConvertHexStringToByteArray(binaryPatch.Data);

                                writer.Write(patchBytes);
                            }
                        }
                        else
                        {
                            this.log.LogInformation("Unknown patch.");
                        }
                    }
                }

                this.log.LogDebug("Loaded {NumPatches} file patch(es) from card {CardName}", patchFiles.Length, card.Name);
            }
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            var fixedString = hexString.Replace(" ", string.Empty);
            if (fixedString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", fixedString));
            }

            byte[] data = new byte[fixedString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = fixedString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
    }
}
