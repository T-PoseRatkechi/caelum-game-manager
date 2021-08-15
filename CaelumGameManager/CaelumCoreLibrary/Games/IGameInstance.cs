// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;

    /// <summary>
    /// Game instance interface.
    /// </summary>
    public interface IGameInstance
    {
        /// <summary>
        /// Gets game install.
        /// </summary>
        IGameInstall Install { get; }

        /// <summary>
        /// Gets game config.
        /// </summary>
        IGameConfig GameConfig { get; }

        /// <summary>
        /// Gets game's deck of cards.
        /// </summary>
        List<ICard> Deck { get; }

        /// <summary>
        /// Save config to file.
        /// </summary>
        void SaveGameConfig();

        /// <summary>
        /// Load config from file.
        /// </summary>
        void LoadGameConfig();
    }
}