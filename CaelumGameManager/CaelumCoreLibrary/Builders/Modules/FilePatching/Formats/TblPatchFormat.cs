// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching.Formats
{
    using System;
    using System.IO;
    using CaelumCoreLibrary.Builders.Modules.FilePatching.Utilities;

    /// <summary>
    /// TBL patch format.
    /// </summary>
    public class TblPatchFormat : IPatch
    {
        /// <inheritdoc/>
        public string Format { get; set; } = "tblpatch";

        /// <inheritdoc/>
        public string File { get; set; }

        /// <inheritdoc/>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the segment in TBL to apply patch to.
        /// </summary>
        public int Segment { get; set; }

        /// <summary>
        /// Gets or sets the offset in segment.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the patch data.
        /// </summary>
        public string Data { get; set; }

        /// <inheritdoc/>
        public void ApplyPatch(string filePath)
        {
            using (BinaryWriter writer = new(System.IO.File.Open(filePath, FileMode.Open)))
            {
                byte[] sectionSizeBytes = new byte[4];
                writer.BaseStream.Read(sectionSizeBytes, 0, sectionSizeBytes.Length);

                for (int currentSegment = 0; currentSegment <= this.Segment; currentSegment++)
                {
                    // In correct segement then write data bytes.
                    if (currentSegment == this.Segment)
                    {
                        writer.BaseStream.Seek(this.Offset, SeekOrigin.Current);
                        var patchBytes = this.Data.ToByteArray();

                        writer.Write(patchBytes);
                        break;
                    }

                    // Seek to next segment start.
                    else
                    {

                        var sectionSize = BitConverter.ToUInt32(sectionSizeBytes);
                        writer.Seek((int)sectionSize, SeekOrigin.Current);
                    }
                }
            }
        }
    }
}
