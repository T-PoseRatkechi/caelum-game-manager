﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    /// <summary>
    /// Game config implementation.
    /// </summary>
    public class GameConfig : IGameConfig
    {

        /// <inheritdoc/>
        public string GameInstallPath { get; set; }

        /// <inheritdoc/>
        public string GameLauncher { get; set; }

        /// <inheritdoc/>
        public string GameTheme { get; set; }

        /// <inheritdoc/>
        public string OutputDirectory { get; set; }

        /// <inheritdoc/>
        public string DeckBuilderName { get; set; }
    }
}
