// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI
{
    using System;
    using System.IO;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Utilities;

    /// <summary>
    /// CaemlumGameManagerGUI implementation of <seealso cref="ICaelumConfig"/>.
    /// </summary>
    internal class CaelumConfig : ICaelumConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaelumConfig"/> class.
        /// </summary>
        public CaelumConfig()
        {
            var appDir = AppDomain.CurrentDomain.BaseDirectory;

            this.ToolsDirectory = CaelumFileIO.BuildDirectory(Path.Join(appDir, "Tools"));
            this.GamesDirectory = CaelumFileIO.BuildDirectory(Path.Join(appDir, "Games"));
            this.ConfigDirectory = CaelumFileIO.BuildDirectory(Path.Join(appDir, "Configs"));
            this.AuthorsDirectory = CaelumFileIO.BuildDirectory(Path.Join(appDir, "Authors"));
        }

        /// <inheritdoc/>
        public string ToolsDirectory { get; init; }

        /// <inheritdoc/>
        public string GamesDirectory { get; init; }

        /// <inheritdoc/>
        public string ConfigDirectory { get; init; }

        /// <inheritdoc/>
        public string AuthorsDirectory { get; init; }
    }
}
