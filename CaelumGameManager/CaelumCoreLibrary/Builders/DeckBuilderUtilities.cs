// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System;
    using System.IO;
    using CaelumCoreLibrary.Utilities;

    /// <summary>
    /// Utility functions related to <seealso cref="IDeckBuilder"/>.
    /// </summary>
    public static class DeckBuilderUtilities
    {
        /// <summary>
        /// Maximum allowed amount of files to exist in an output directory prior to deleting.
        /// </summary>
        public const int MaxFilesAllowedForDeleting = 512;

        /// <summary>
        /// List of known invalid output directories.
        /// </summary>
        private static readonly string[] InvalidOutputDirectories = new string[]
        {
            Path.GetFullPath("c:").ToLower(),
            Path.Join("c:", "users"),
            Path.Join("c:", "program files"),
            Path.Join("c:", "program files (x86)"),
            Path.Join("c:", "windows"),
            Path.Join("c:", "program data"),
        };

        /// <summary>
        /// Indicates whether <paramref name="directory"/> is a valid, and safe, output directory.
        /// </summary>
        /// <param name="directory">Output directory to check.</param>
        /// <returns>Whether <paramref name="directory"/> is a valid output directory.</returns>
        public static bool IsValidOutputDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentException($"'{nameof(directory)}' cannot be null or whitespace.", nameof(directory));
            }

            var normalizedPath = directory.AsNormalizedPath().ToLower();

            // Check output dir is not a disallowed directory.
            if (Array.IndexOf(InvalidOutputDirectories, normalizedPath) > -1)
            {
                return false;
            }

            // Check output dir is not the root of a users folder.
            var userTextIndex = normalizedPath.IndexOf("users");
            if (userTextIndex > -1)
            {
                var isRootOfUserFolder = true;

                // Check if there is a subdirectory after Users folder.
                for (int indexInString = userTextIndex + "users".Length + 1, total = normalizedPath.Length; indexInString < total; indexInString++)
                {
                    char currentChar = normalizedPath[indexInString];

                    if (currentChar == Path.DirectorySeparatorChar || currentChar == Path.AltDirectorySeparatorChar)
                    {
                        isRootOfUserFolder = false;
                        break;
                    }
                }

                if (isRootOfUserFolder)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
