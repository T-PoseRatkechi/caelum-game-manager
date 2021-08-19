﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
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
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base class for game instances.
    /// </summary>
    public class GameInstance : IGameInstance
    {
        private readonly ILogger log;
        private readonly IDeckBuilder deckBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstance"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="deckBuilder">Deck builder to use for building output.</param>
        public GameInstance(ILogger log, IDeckBuilder deckBuilder)
        {
            this.deckBuilder = deckBuilder;
        }

        /// <inheritdoc/>
        public IGameInstall GameInstall { get; init; }

        /// <inheritdoc/>
        public IGameConfigManager GameConfig { get; init; }

        /// <inheritdoc/>
        public IDeck Deck { get; init; }

        /// <inheritdoc/>
        public void BuildDeck()
        {
            this.log.LogInformation("Building deck");
            this.deckBuilder.Build(this.Deck.Cards.ToArray(), this.GameInstall.BuildDirectory);
        }

        /// <inheritdoc/>
        public void CreateCard(CardModel newCard)
        {
            this.log.LogDebug("Creating card");
        }
    }
}
