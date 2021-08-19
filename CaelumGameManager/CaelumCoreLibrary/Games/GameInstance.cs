// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System;
    using CaelumCoreLibrary.Builders;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Decks;

    /// <summary>
    /// Base class for game instances.
    /// </summary>
    public class GameInstance : IGameInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstance"/> class.
        /// </summary>
        public GameInstance()
        {
        }

        /// <inheritdoc/>
        public IGameInstall GameInstall { get; init; }

        /// <inheritdoc/>
        public IGameConfigManager GameConfig { get; init; }

        /// <inheritdoc/>
        public IDeck Deck { get; init; }

        private IDeckBuilder DeckBuilder { get; set; }

        /// <inheritdoc/>
        public void BuildDeck()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void CreateCard(CardModel newCard)
        {
            throw new NotImplementedException();
        }
    }
}
