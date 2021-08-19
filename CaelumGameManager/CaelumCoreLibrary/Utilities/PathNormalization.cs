// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Utilities
{
    using System;
    using System.IO;

    /// <summary>
    /// Extension method for normalizing paths.
    ///
    /// How can one get an absolute or normalized file path in .NET?
    /// https://stackoverflow.com/a/21058121
    /// CC BY-SA 3.0.
    /// nawfal.
    /// </summary>
    public static class PathNormalization
    {
        /// <summary>
        /// Returns <paramref name="str"/> as a normalized path with no trailing directory seperator.
        /// </summary>
        /// <param name="str">String.</param>
        /// <returns>Normalized path with no trailing directory seperator.</returns>
        public static string AsNormalizedPath(this string str)
        {
            return Path.TrimEndingDirectorySeparator(Path.GetFullPath(new Uri(str).LocalPath));
        }
    }
}
