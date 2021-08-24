// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching.Formats
{
    /// <summary>
    /// Patch interface.
    /// </summary>
    public interface IPatch
    {
        /// <summary>
        /// Gets or sets patch format name.
        /// </summary>
        string Format { get; set; }

        /// <summary>
        /// Gets or sets the file the patch is for.
        /// </summary>
        string File { get; set; }

        /// <summary>
        /// Gets or sets the comment for patch.
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// Applies patch to file.
        /// </summary>
        /// <param name="filePath">File to apply patch to.</param>
        void ApplyPatch(string filePath);
    }
}
