// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Builders;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Utilities;
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
            this.LoadDeck();

            this.DeckBuilder = deckBuilderFactory.GetDeckBuilderByName(this.GameConfig.DeckBuilderName);
        }

        /// <inheritdoc/>
        public IGameInstall GameInstall { get; init; }

        /// <inheritdoc/>
        public IGameConfig GameConfig { get; private set; }

        /// <inheritdoc/>
        public List<IInstallableCard> Deck { get; } = new();

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
        public void CreateCard(ICard newCard)
        {
            string newCardDir;
            switch (newCard.Type)
            {
                case CardType.Mod:
                case CardType.Launcher:
                case CardType.Empty:
                case CardType.Preset:
                    {
                        newCardDir = CaelumFileIO.BuildDirectory(Path.Join(this.GameInstall.CardsDirectory, newCard.CardId));
                        break;
                    }

                case CardType.Tool:
                    {
                        newCardDir = CaelumFileIO.BuildDirectory(Path.Join(CaelumPaths.ToolsDir, newCard.CardId));
                        break;
                    }

                default:
                    throw new NotImplementedException($@"Unknown card type ""{newCard}"" could not created");
            }

            // Create card Data folder.
            CaelumFileIO.BuildDirectory(Path.Join(newCardDir, "Data"));

            // Set path to card file and authors folder.
            var newCardFile = Path.Join(newCardDir, "card.json");
            var authorsDir = CaelumFileIO.BuildDirectory(Path.Join(newCardDir, "Authors"));

            // Write card file.
            this.writer.WriteFile(newCardFile, newCard);

            // Write card author files.
            foreach (var author in newCard.Authors)
            {
                AuthorUtils.WriteAuthor(author, Path.Join(authorsDir, author.Name.GetHashCode().ToString()));
            }

            this.log.Information("Created new {CardType} card {CardName} with ID {CardId}", newCard.Type.ToString(), newCard.Name, newCard.CardId);
        }

        private void LoadDeck()
        {
            string[] gameCardsList = Directory.GetDirectories(this.GameInstall.CardsDirectory);

            foreach (var gameCardDir in gameCardsList)
            {
                var gameCardFile = Path.Join(gameCardDir, "card.json");
                var gameCard = this.writer.ParseFile<InstallableCard>(gameCardFile);
                gameCard.InstallDirectory = gameCardDir;

                // Load authors.
                foreach (var authorFile in Directory.GetFiles(Path.Join(gameCardDir, "Authors")))
                {
                    gameCard.Authors.Add(AuthorUtils.ParseAuthor(authorFile));
                }

                this.Deck.Add(gameCard);
                this.log.Debug("Loaded card {CardName}", gameCard.Name);
            }

            string[] toolCardsList = Directory.GetDirectories(CaelumPaths.ToolsDir);

            foreach (var toolCardDir in toolCardsList)
            {
                var toolCardFile = Path.Join(toolCardDir, "card.json");
                var toolCard = this.writer.ParseFile<InstallableCard>(toolCardFile);
                toolCard.InstallDirectory = toolCardDir;

                // Load authors.
                foreach (var authorFile in Directory.GetFiles(Path.Join(toolCardDir, "Authors")))
                {
                    toolCard.Authors.Add(AuthorUtils.ParseAuthor(authorFile));
                }

                this.Deck.Add(toolCard);
                this.log.Debug("Loaded card {CardName}", toolCard.Name);
            }
        }
    }
}
