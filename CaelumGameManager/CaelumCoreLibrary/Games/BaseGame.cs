﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using CaelumCoreLibrary.Decks;
    using CaelumCoreLibrary.Utilities;

    /// <summary>
    /// Base implementation of <seealso cref="IGame"/>.
    /// </summary>
    public abstract class BaseGame : IGame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGame"/> class.
        /// </summary>
        /// <param name="name">Name of game.</param>
        public BaseGame(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            this.Name = name;
            this.BaseDir = CaelumFileIO.BuildDirectory(Path.Join(Directory.GetCurrentDirectory(), $@"games{Path.DirectorySeparatorChar}{this.Name}"));
            this.ConfigPath = Path.Join(this.BaseDir, "game-config.json");
            this.CardsDir = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDir, "cards"));
            this.DownloadsDir = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDir, "downloads"));
            this.BuildDir = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDir, "build"));
            this.Deck = new List<ICard>();

            this.GameInit();
        }

        /// <inheritdoc/>
        public string Name { get; init; }

        /// <summary>
        /// Gets base directory of game.
        /// </summary>
        public string BaseDir { get; init; }

        /// <summary>
        /// Gets game's config path.
        /// </summary>
        public string ConfigPath { get; init; }

        /// <summary>
        /// Gets game's current config version.
        /// </summary>
        public int ConfigVersion { get; init; }

        /// <summary>
        /// Gets game's config path.
        /// </summary>
        public GameConfig Config { get; private set; }

        /// <summary>
        /// Gets game's cards directory.
        /// </summary>
        public string CardsDir { get; init; }

        /// <summary>
        /// Gets or sets game's deck.
        /// </summary>
        public List<ICard> Deck { get; set; }

        /// <summary>
        /// Gets game's downloads directory.
        /// </summary>
        public string DownloadsDir { get; init; }

        /// <summary>
        /// Gets game's build directory.
        /// </summary>
        public string BuildDir { get; init; }

        /// <inheritdoc/>
        public ICard CreateCard(string id, string name, CardType type, List<Author> authors, string description, string version)
        {
            switch (type)
            {
                case CardType.Folder:
                    {
                        var cardFolderPath = CaelumFileIO.BuildDirectory(Path.Join(this.CardsDir, id));
                        var newFolderCard = new FolderCard()
                        {
                            Id = id,
                            Name = name,
                            Authors = authors,
                            Description = description,
                            Game = this.Name,
                            Version = version,
                            Path = cardFolderPath,
                        };

                        var cardText = JsonSerializer.Serialize(newFolderCard, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(Path.Join(this.CardsDir, id, "card.json"), cardText);

                        return newFolderCard;
                    }

                case CardType.Tool:
                    {
                        var cardToolPath = CaelumFileIO.BuildDirectory(Path.Join(CaelumPaths.ToolsDir, id));
                        var newToolCard = new ToolCard()
                        {
                            Game = this.Name,
                            Id = id,
                            Name = name,
                            Authors = authors,
                            Description = description,
                            Version = version,
                            Path = cardToolPath,
                        };

                        var cardText = JsonSerializer.Serialize(newToolCard, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(Path.Join(CaelumPaths.ToolsDir, id, "card.json"), cardText);

                        return newToolCard;
                    }

                default:
                    return null;
            }
        }

        /// <summary>
        /// Loads the game config, or creates a default if none is found.
        /// </summary>
        public void WriteConfig()
        {
            this.Config.CardsList = this.Deck.Select(card => card.Id).ToArray();

            var configText = JsonSerializer.Serialize(this.Config, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(this.ConfigPath, configText);
        }

        /// <summary>
        /// Initializes core game data.
        /// </summary>
        protected virtual void GameInit()
        {
            this.LoadConfig();
            this.LoadDeck();
        }

        private void LoadDeck()
        {
            for (int i = 0, total = this.Config.CardsList.Length; i < total; i++)
            {
                var currentCard = this.Config.CardsList[i];
                this.Deck.Add(this.GetCard(currentCard));
            }
        }

        private ICard GetCard(string cardId)
        {
            // Card is mod type.
            var modCardFile = Path.Join(this.DownloadsDir, cardId);
            if (File.Exists(modCardFile))
            {
                // Mod card has data card.
                var modDataDir = Path.Join(this.CardsDir, cardId);
                if (Directory.Exists(modDataDir))
                {
                    // Add mod folder card.
                    var modDataCard = CardUtils.ParseCard<ModCard>(Path.Join(modDataDir, "card.json"));
                    return modDataCard;
                }
                else
                {
                    throw new ArgumentException($"{cardId} is missing its card data! Folder: {modDataDir}");
                }
            }


            // Card is folder type.
            var folderCardDir = Path.Join(this.CardsDir, cardId);
            if (Directory.Exists(folderCardDir))
            {
                var folderCardPath = Path.Join(folderCardDir, "card.json");
                var folderCard = CardUtils.ParseCard<FolderCard>(folderCardPath);
                return folderCard;
            }

            // Card is tool type.
            var toolCardDir = Path.Join(CaelumPaths.ToolsDir, cardId);
            if (Directory.Exists(toolCardDir))
            {
                var toolCardPath = Path.Join(toolCardDir, "card.json");
                var toolCard = CardUtils.ParseCard<ToolCard>(toolCardPath);
                return toolCard;
            }

            return null;
        }

        /// <summary>
        /// Loads the game config, or creates a default if none is found.
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(this.ConfigPath))
            {
                var configText = File.ReadAllText(this.ConfigPath);
                var config = JsonSerializer.Deserialize<GameConfig>(configText);
                this.Config = config;
            }
            else
            {
                var defaultConfig = new GameConfig()
                {
                    GameName = this.Name,
                    Version = this.ConfigVersion,
                    OutputDirectory = this.BuildDir,
                    CardsList = Array.Empty<string>(),
                };

                var defaultText = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions() { WriteIndented = true });
                File.WriteAllText(this.ConfigPath, defaultText);
                this.Config = defaultConfig;
            }
        }
    }
}
