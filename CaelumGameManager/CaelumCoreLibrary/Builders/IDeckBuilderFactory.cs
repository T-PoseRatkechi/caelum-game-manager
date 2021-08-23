// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;

    /// <summary>
    /// DeckBuilderFactory interface.
    /// </summary>
    public interface IDeckBuilderFactory
    {
        /// <summary>
        /// Gets the <see cref="IDeckBuilder"/> for the given game.
        /// </summary>
        /// <param name="gameInstall">Game install.</param>
        /// <param name="gameConfig">Game config.</param>
        /// <returns>The game deck builder.</returns>
        IDeckBuilder GetGameDeckBuilder(IGameInstall gameInstall, GameConfigModel gameConfig);
    }
}