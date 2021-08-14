﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using CaelumCoreLibrary.Configs;

    /// <summary>
    /// Game instance interface.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets game install.
        /// </summary>
        IGameInstall Install { get; init; }

        /// <summary>
        /// Gets game config manager.
        /// </summary>
        IConfigManager Manager { get; init; }

        // TODO: Add deck prop?
    }
}