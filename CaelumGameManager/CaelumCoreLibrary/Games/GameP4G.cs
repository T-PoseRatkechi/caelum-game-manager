// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Configs.Writers;

    /// <summary>
    /// Game instance for Persona 4 Golden.
    /// </summary>
    public class GameP4G : IGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameP4G"/> class.
        /// </summary>
        public GameP4G()
        {
            this.Install = new GameInstall("Persona 4 Golden");

            this.Manager = new ConfigManager(new GameConfig(this.Install), new JsonWriter());
        }

        /// <inheritdoc/>
        public IGameInstall Install { get; init; }

        /// <inheritdoc/>
        public IConfigManager Manager { get; init; }
    }
}
