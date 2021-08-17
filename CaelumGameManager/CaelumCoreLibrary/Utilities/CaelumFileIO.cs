// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Utilities
{
    using System.IO;

    /// <summary>
    /// Utility functions related to File IO.
    /// </summary>
    public static class CaelumFileIO
    {
        /// <summary>
        /// Creates the directory given by <paramref name="dirPath"/> if it doesn't exist and returns the original
        /// path given.
        /// </summary>
        /// <param name="dirPath">Directory path to create.</param>
        /// <returns><paramref name="dirPath"/> if created or already exists.</returns>
        public static string BuildDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                return dirPath;
            }

            return dirPath;
        }
    }
}
