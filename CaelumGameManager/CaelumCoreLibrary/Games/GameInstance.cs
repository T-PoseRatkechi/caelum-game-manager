// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Builders;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Writers;
    using Serilog;

    /// <summary>
    /// Base class for game instances.
    /// </summary>
    public class GameInstance : IGameInstance
    {
        private ILogger log = Log.Logger.WithCallerSyntax();
        private IWriter writer;
        private string configFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstance"/> class.
        /// </summary>
        /// <param name="gameName">Name of game.</param>
        /// <param name="writer">Writer to use.</param>
        /// <param name="gameInstallFactory">Game install factory.</param>
        /// <param name="deckBuilderFactory">Deck builder factory.</param>
        public GameInstance(string gameName, IWriter writer, IGameInstallFactory gameInstallFactory, IDeckBuilderFactory deckBuilderFactory)
        {
            this.writer = writer;
            this.GameInstall = gameInstallFactory.GetGameInstall(gameName);

            this.configFilePath = Path.Join(this.GameInstall.BaseDirectory, "game-config.json");

            this.LoadGameConfig();

            this.DeckBuilder = deckBuilderFactory.GetDeckBuilderByName(this.GameConfig.DeckBuilderName);
        }

        /// <inheritdoc/>
        public IGameInstall GameInstall { get; init; }

        /// <inheritdoc/>
        public IGameConfig GameConfig { get; private set; }

        /// <inheritdoc/>
        public List<IInstallableCard> Deck { get; init; } = new();

        private IDeckBuilder DeckBuilder { get; set; }

        /// <inheritdoc/>
        public void LoadGameConfig()
        {
            if (File.Exists(this.configFilePath))
            {
                this.GameConfig = this.writer.ParseFile<GameConfig>(this.configFilePath);
            }
            else
            {
                this.GameConfig = new GameConfig();
                this.SaveGameConfig();
            }
        }

        /// <inheritdoc/>
        public void SaveGameConfig()
        {
            this.writer.WriteFile(this.configFilePath, this.GameConfig);
        }

        /// <inheritdoc/>
        public void BuildDeck()
        {
            this.DeckBuilder.Build(this.Deck.ToArray(), this.GameConfig.OutputDirectory);
        }

        /// <inheritdoc/>
        public ICard CreateCard(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.Mod:
                case CardType.Tool:
                case CardType.Launcher:
                    return new InstallableCard() { Type = cardType };
                default:
                    return null;
            }
        }

        private void LoadDeck()
        {
            string[] gameCardsList = Directory.GetDirectories(this.GameInstall.CardsDirectory);

            foreach (var gameCardDir in gameCardsList)
            {
                var gameCardFile = Path.Join(gameCardDir, "card.json");
                var gameCard = this.writer.ParseFile<InstallableCard>(gameCardFile);
                gameCard.InstallPath = gameCardDir;
                this.Deck.Add(gameCard);
                this.log.Debug("Loaded card {CardName}", gameCard.Name);
            }
        }
    }
}
