// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Diagnostics;
    using System.Linq;
    using System.Timers;
    using CaelumCoreLibrary.Builders;
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

        // Save timer for saving config.
        private readonly Timer saveTimer = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstance"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="deckBuilder">Deck builder to use for building output.</param>
        public GameInstance(ILogger log, IDeckBuilder deckBuilder)
        {
            this.log = log;
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
            Stopwatch watch = new();

            watch.Start();
            this.deckBuilder.Build(this.Deck.Cards, this.GameInstall.BuildDirectory);
            watch.Stop();

            this.log.LogInformation("Deck built successfully in {TimeElapsed} ms.", watch.ElapsedMilliseconds);
        }

        /// <inheritdoc/>
        public void InitGame()
        {
            // Sort a list from another list IDs
            // https://stackoverflow.com/a/55650341
            // CC BY-SA 4.0
            // Kladzey

            if (this.GameConfig.Settings.Cards != null)
            {
                // Set inital order of cards based on config.
                var originalCards = this.Deck.Cards;
                this.Deck.Cards = this.GameConfig.Settings.Cards.Join(this.Deck.Cards, i => i, d => d.CardId, (i, d) => d).ToList();

                // Add back any cards that were removed for not existing
                if (originalCards.Count != this.Deck.Cards.Count)
                {
                    foreach (var originalCard in originalCards)
                    {
                        if (!this.Deck.Cards.Contains(originalCard))
                        {
                            this.Deck.Cards.Add(originalCard);
                        }
                    }
                }
            }

            this.saveTimer.AutoReset = false;
            this.saveTimer.Enabled = false;
            this.saveTimer.Interval = 1000;

            this.saveTimer.Elapsed += (sender, e) =>
            {
                var cardOrder = this.Deck.Cards.Select(x => x.CardId).ToArray();
                this.GameConfig.Settings.Cards = cardOrder;
                this.GameConfig.SaveGameConfig();
            };

            this.Deck.OnDeckChanged += (sender, e) =>
            {
                this.saveTimer.Enabled = true;
                this.saveTimer.Stop();
                this.saveTimer.Start();
            };
        }
    }
}
