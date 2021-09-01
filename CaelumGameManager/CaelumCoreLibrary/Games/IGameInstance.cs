// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Decks;

    /// <summary>
    /// Game instance interface.
    /// </summary>
    public interface IGameInstance
    {
        /// <summary>
        /// Gets game install.
        /// </summary>
        IGameInstall GameInstall { get; }

        /// <summary>
        /// Gets game config.
        /// </summary>
        IGameConfigManager GameConfig { get; }

        /// <summary>
        /// Gets game's deck of cards.
        /// </summary>
        IDeck Deck { get; }

        /// <summary>
        /// Builds current card in deck to output.
        /// </summary>
        void BuildDeck(IList<ICardModel> cards);

        /// <summary>
        /// Starts the game with the given launcher or the default launcher if none given.
        /// </summary>
        /// <param name="gameLauncher">Game launcher to use to start game.</param>
        void StartGame(ILauncherCardModel gameLauncher = null);

        /// <summary>
        /// Initializes game instance.
        /// </summary>
        void InitDeck();
    }
}