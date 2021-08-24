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
    /// Binary patch format.
    /// </summary>
    public class BinaryPatchFormat : IPatch
    {
        /// <inheritdoc/>
        public string Format { get; set; } = "binary";

        /// <inheritdoc/>
        public string File { get; set; }

        /// <inheritdoc/>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the offset to apply patch to.
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// Gets or sets the data of the binary patch.
        /// </summary>
        public string Data { get; set; }

        /// <inheritdoc/>
        public void ApplyPatch(string filePath)
        {
            using (BinaryWriter writer = new(System.IO.File.Open(filePath, FileMode.Open)))
            {
                writer.BaseStream.Seek(this.Offset, SeekOrigin.Begin);

                var patchBytes = this.Data.ToByteArray();

                writer.Write(patchBytes);
            }
        }
    }
}
