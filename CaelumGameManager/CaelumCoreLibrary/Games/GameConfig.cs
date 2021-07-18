// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    /// <summary>
    /// Game config class.
    /// </summary>
    public class GameConfig
    {
        /// <summary>
        /// Gets or sets the game config version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the game name.
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// Gets or sets the game install path.
        /// </summary>
        public string GameInstallPath { get; set; }

        /// <summary>
        /// Gets or sets the game launcher.
        /// </summary>
        public string GameLauncher { get; set; }

        /// <summary>
        /// Gets or sets the game's theme.
        /// </summary>
        public string GameTheme { get; set; }

        /// <summary>
        /// Gets or sets the game's output directory.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the game's deck.
        /// </summary>
        public string[] Deck { get; set; }
    }
}
