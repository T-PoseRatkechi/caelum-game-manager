// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Writers
{
    /// <summary>
    /// Base writer interface.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Writes <paramref name="obj"/> to <paramref name="filePath"/>.
        /// </summary>
        /// <param name="filePath">Output file path.</param>
        /// <param name="obj">Object to write.</param>
        void WriteFile(string filePath, object obj);

        /// <summary>
        /// Reads and parses <paramref name="filePath"/> as <typeparamref name="T"/> from a file written in the same format as <see cref="WriteFile(string, object)"/>.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="filePath">File to parse.</param>
        /// <returns><paramref name="filePath"/> parsed as <typeparamref name="T"/>.</returns>
        T ParseFile<T>(string filePath);
    }
}
