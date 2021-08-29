// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

using CaelumCoreLibrary.Games.Launchers;
using System.Collections.Generic;

namespace CaelumCoreLibrary.Configs
{
    /// <summary>
    /// Interface for game configs.
    /// </summary>
    public class GameConfigModel
    {
        /// <summary>
        /// Gets or sets the game install path.
        /// </summary>
        public string GameInstallPath { get; set; }

        /// <summary>
        /// Gets or sets the default game launcher (index) to use.
        /// </summary>
        public int DefaultGameLauncher { get; set; }

        /// <summary>
        /// Gets or sets the game launcher.
        /// </summary>
        public List<GameLauncherModel> GameLaunchers { get; set; }

        /// <summary>
        /// Gets or sets the game's theme.
        /// </summary>
        public string GameTheme { get; set; }

        /// <summary>
        /// Gets or sets the game's output directory.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to build directly in the output directory, skipping
        /// requiring a successful build first.
        /// </summary>
        public bool OutputBuildOnly { get; set; }

        /// <summary>
        /// Gets or sets game cards by IDs. Used for preserving building priority.
        /// </summary>
        public string[] Cards { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display debug messages.
        /// </summary>
        public bool ShowDebugMessages { get; set; }
    }
}