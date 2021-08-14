// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Configs.Writers;

    /// <summary>
    /// Game instance for Kingdom Hearts 3.
    /// </summary>
    public class GameKH3 : IGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameKH3"/> class.
        /// </summary>
        public GameKH3()
        {
            this.Install = new GameInstall("Kingdom Hearts 3");
            this.Manager = new ConfigManager(new GameConfig(this.Install), new JsonWriter());
        }

        /// <inheritdoc/>
        public IGameInstall Install { get; init; }

        /// <inheritdoc/>
        public IConfigManager Manager { get; init; }

        /// <inheritdoc/>
        public List<ICard> Deck { get; init; } = new();
    }
}
