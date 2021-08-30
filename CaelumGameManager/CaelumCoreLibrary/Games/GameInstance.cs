﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Timers;
    using CaelumCoreLibrary.Builders;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Decks;
    using CaelumCoreLibrary.Games.Launchers;
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
        public void StartGame(GameLauncherModel gameLauncher = null)
        {
            // Start game with default launcher.
            if (gameLauncher == null)
            {
                // Start game with a launcher if one exists in config.
                if (this.GameConfig.Settings.GameLaunchers != null && this.GameConfig.Settings.GameLaunchers.Count > 0)
                {
                    // Start game with default launcher if settings valid.
                    if (this.GameConfig.Settings.DefaultGameLauncher < this.GameConfig.Settings.GameLaunchers.Count)
                    {
                        var defaultLauncher = this.GameConfig.Settings.GameLaunchers[this.GameConfig.Settings.DefaultGameLauncher];
                        this.log.LogDebug(
                            "Launching game with default launcher.\nPath: {LauncherPath}\nArguments: {LauncherArgs}",
                            defaultLauncher.LauncherPath,
                            defaultLauncher.LauncherArgs);
                        defaultLauncher.Start(this.GameConfig.Settings.GameInstallPath);
                    }

                    // Start game with first launcher if settings invalid.
                    else
                    {
                        this.log.LogError(
                            "Default launcher setting {DefaultLauncherIndex} does not exist.\nStarting game with first game launcher.",
                            this.GameConfig.Settings.DefaultGameLauncher);
                        var launcher = this.GameConfig.Settings.GameLaunchers[0];
                        this.log.LogDebug(
                            "Launching game with default launcher.\nPath: {LauncherPath}\nArguments: {LauncherArgs}",
                            launcher.LauncherPath,
                            launcher.LauncherArgs);
                        launcher.Start(this.GameConfig.Settings.GameInstallPath);
                    }
                }
                else
                {
                    this.log.LogError("Game {GameName} has no launchers to start game.", this.GameInstall.GameName);
                }
            }
            else
            {
                this.log.LogDebug(
                    "Launching game with given game launcher.\nPath: {LauncherPath}\nArguments: {LauncherArgs}",
                    gameLauncher.LauncherPath,
                    gameLauncher.LauncherArgs);
                gameLauncher.Start(this.GameConfig.Settings.GameInstallPath);
            }
        }

        /// <inheritdoc/>
        public void BuildDeck(IList<ICardModel> cards)
        {
            this.log.LogInformation("Building deck.");
            Stopwatch watch = new();

            watch.Start();
            if (this.GameConfig.Settings.OutputBuildOnly && this.GameConfig.Settings.OutputDirectory != null)
            {
                this.deckBuilder.Build(cards, this.GameConfig.Settings.OutputDirectory);
            }
            else
            {
                this.deckBuilder.Build(cards, this.GameInstall.BuildDirectory);
                if (this.GameConfig.Settings.OutputDirectory != null)
                {
                    // TODO: Copy built to output directory.
                }
            }

            watch.Stop();

            this.log.LogInformation("Deck built successfully in {TimeElapsed} ms.", watch.ElapsedMilliseconds);
        }

        /// <inheritdoc/>
        public void InitDeck()
        {
            // Sort a list from another list IDs
            // https://stackoverflow.com/a/55650341
            // CC BY-SA 4.0
            // Kladzey
            if (this.GameConfig.Settings.Cards != null)
            {
                // Set inital order of cards based on config.
                var originalCards = this.Deck.Cards;
                this.Deck.Cards = this.GameConfig.Settings.Cards.Join(this.Deck.Cards, i => i, d => d.CardId, (i, d) => d).Distinct().ToList();

                // Add back any cards that were removed for not existing previously.
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
        }
    }
}
