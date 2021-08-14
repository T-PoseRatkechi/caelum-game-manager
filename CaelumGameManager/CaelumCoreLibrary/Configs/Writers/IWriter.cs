// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs.Writers
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
    }
}
