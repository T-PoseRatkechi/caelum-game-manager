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

        /// <summary>
        /// Copies the contents of <paramref name="sourceFolder"/> to <paramref name="destFolder"/>, overwriting any existing files.
        /// </summary>
        /// <param name="sourceFolder">Source folder to copy.</param>
        /// <param name="destFolder">Destination folder.</param>
        public static void CopyFolder(string sourceFolder, string destFolder)
        {
            foreach (var file in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
            {
                var outputFile = Path.Join(destFolder, file.Replace(sourceFolder, string.Empty));
                Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                File.Copy(file, outputFile, true);
            }
        }
    }
}
