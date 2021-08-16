// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Builders;
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
        IGameInstall GameInstall { get; }

        /// <summary>
        /// Gets game config.
        /// </summary>
        IGameConfig GameConfig { get; }

        /// <summary>
        /// Gets game's deck of cards.
        /// </summary>
        List<IInstallableCard> Deck { get; }

        /// <summary>
        /// Save config to file.
        /// </summary>
        void SaveGameConfig();

        /// <summary>
        /// Load config from file.
        /// </summary>
        void LoadGameConfig();

        /// <summary>
        /// Builds current card in deck to output.
        /// </summary>
        void BuildDeck();

        /// <summary>
        /// Creates a new card.
        /// </summary>
        /// <param name="cardType">Type of card to create.</param>
        /// <returns>The created card.</returns>
        ICard CreateCard(CardType cardType);
    }
}