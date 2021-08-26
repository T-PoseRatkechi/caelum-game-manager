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
    public class ItemTblPatchFormat : IPatch
    {
        /// <inheritdoc/>
        public string Format { get; set; } = "itemtblpatch";

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
                byte[] numEntryBytes = new byte[2];
                writer.BaseStream.Read(numEntryBytes, 0, numEntryBytes.Length);

                const int entriesSize = 68;

                var numEntries = BitConverter.ToUInt16(numEntryBytes);
                var entriesEndOffset = (numEntries * entriesSize) + 2;

                if (this.Segment == 0)
                {
                    writer.BaseStream.Position = 0; // Does ITEMTBL patch offset start from file start??? They do...
                    writer.BaseStream.Seek(this.Offset, SeekOrigin.Current);
                    writer.Write(this.Data.ToByteArray());
                }
                else
                {
                    const int unknownChunk = 30;
                    writer.BaseStream.Position = entriesEndOffset + unknownChunk;

                    writer.Seek(this.Offset, SeekOrigin.Current);
                    writer.Write(this.Data.ToByteArray());
                }
            }
        }
    }
}
