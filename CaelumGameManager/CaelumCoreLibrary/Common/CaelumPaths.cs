// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Common
{
    using System.IO;
    using CaelumCoreLibrary.Utilities;

    /// <summary>
    /// Contains paths don't relate to single game.
    /// </summary>
    public class CaelumPaths
    {
        /// <summary>
        /// Gets path of tools directory.
        /// </summary>
        public static string ToolsDir => CaelumFileIO.BuildDirectory(Path.Join(Directory.GetCurrentDirectory(), "Tools"));
    }
}
