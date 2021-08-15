// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    /// <summary>
    /// Interface for game configs.
    /// </summary>
    public interface IGameConfig
    {
        /// <summary>
        /// Gets or sets the game install path.
        /// </summary>
        string GameInstallPath { get; set; }

        /// <summary>
        /// Gets or sets the game launcher.
        /// </summary>
        string GameLauncher { get; set; }

        /// <summary>
        /// Gets or sets the game's theme.
        /// </summary>
        string GameTheme { get; set; }

        /// <summary>
        /// Gets or sets the game's output directory.
        /// </summary>
        string OutputDirectory { get; set; }
    }
}