// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Files
{
    /// <summary>
    /// Unpacker interface.
    /// </summary>
    public interface IGameFile
    {
        /// <summary>
        /// Returns the local unpacked file path of <paramref name="relativeGameFile"/>, unpacking from source game files if missing.
        /// </summary>
        /// <param name="relativeGameFile">The relative file path of the game file.</param>
        /// <returns>The unpacked local copy of <paramref name="relativeGameFile"/>.</returns>
        string GetUnpackedGameFile(string relativeGameFile);

        /// <summary>
        /// Returns the local game installation file path of <paramref name="relativeGameFile"/>, throwing an exception if missing.
        /// </summary>
        /// <param name="relativeGameFile">The relative file path of the game file.</param>
        /// <returns>The local copy of <paramref name="relativeGameFile"/>.</returns>
        string GetInstallGameFile(string relativeGameFile);
    }
}