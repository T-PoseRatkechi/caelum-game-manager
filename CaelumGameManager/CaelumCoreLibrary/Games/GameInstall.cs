// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Utilities;

    /// <summary>
    /// Base implementation of <seealso cref="IGameInstall"/>.
    /// </summary>
    public class GameInstall : IGameInstall
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstall"/> class.
        /// </summary>
        /// <param name="name">Name of game.</param>
        /// <param name="directory">Directory to create game install in.</param>
        public GameInstall(string name, string directory)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            this.GameName = name;
            this.BaseDirectory = CaelumFileIO.BuildDirectory(Path.Join(directory, this.GameName));
            this.CardsDirectory = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDirectory, "Cards"));
            this.DownloadsDirectory = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDirectory, "Downloads"));
            this.BuildDirectory = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDirectory, "Build"));
        }

        /// <inheritdoc/>
        public string GameName { get; }

        /// <inheritdoc/>
        public string BaseDirectory { get; }

        /// <inheritdoc/>
        public string CardsDirectory { get; }

        /// <summary>
        /// Gets game's downloads directory.
        /// </summary>
        public string DownloadsDirectory { get; }

        /// <summary>
        /// Gets game's build directory.
        /// </summary>
        public string BuildDirectory { get; }

        //public ICard CreateCard(string id, string name, CardType type, List<Author> authors, string description, string version)
        //{
        //    switch (type)
        //    {
        //        case CardType.Folder:
        //            {
        //                var cardFolderPath = CaelumFileIO.BuildDirectory(Path.Join(this.CardsDir, id));
        //                var newFolderCard = new InstallableCard()
        //                {
        //                    CardId = id,
        //                    Name = name,
        //                    Authors = authors,
        //                    Description = description,
        //                    Game = this.Name,
        //                    Version = version,
        //                    Type = CardType.Folder,
        //                    InstallPath = cardFolderPath,
        //                };

        //                var cardText = JsonSerializer.Serialize(newFolderCard, new JsonSerializerOptions { WriteIndented = true });
        //                File.WriteAllText(Path.Join(this.CardsDir, id, "card.json"), cardText);

        //                foreach (var author in authors)
        //                {
        //                    AuthorUtils.WriteAuthor(author, Path.Join(this.CardsDir, id, "authors", $"{author.Name.GetHashCode()}.author"));
        //                }

        //                return newFolderCard;
        //            }

        //        case CardType.Tool:
        //            {
        //                var cardToolPath = CaelumFileIO.BuildDirectory(Path.Join(CaelumPaths.ToolsDir, id));
        //                var newToolCard = new InstallableCard()
        //                {
        //                    Game = this.Name,
        //                    CardId = id,
        //                    Name = name,
        //                    Authors = authors,
        //                    Description = description,
        //                    Version = version,
        //                    Type = CardType.Tool,
        //                    InstallPath = cardToolPath,
        //                };

        //                var cardText = JsonSerializer.Serialize(newToolCard, new JsonSerializerOptions { WriteIndented = true });
        //                File.WriteAllText(Path.Join(CaelumPaths.ToolsDir, id, "card.json"), cardText);
        //                File.WriteAllText(Path.Join(this.CardsDir, id, "card.json"), cardText);

        //                foreach (var author in authors)
        //                {
        //                    AuthorUtils.WriteAuthor(author, Path.Join(this.CardsDir, id, "authors", $"{author.Name.GetHashCode()}.author"));
        //                }

        //                return newToolCard;
        //            }

        //        default:
        //            return null;
        //    }
        //}

        /// <summary>
        /// Loads the game config, or creates a default if none is found.
        /// </summary>
        //public void WriteConfig()
        //{
        //    this.Config.CardsList = this.Deck.Select(card => card.CardId).ToArray();

        //    var configText = JsonSerializer.Serialize(this.Config, new JsonSerializerOptions() { WriteIndented = true });
        //    File.WriteAllText(this.ConfigPath, configText);
        //}

        //private void LoadDeck()
        //{
        //    for (int i = 0, total = this.Config.CardsList.Length; i < total; i++)
        //    {
        //        var currentCard = this.Config.CardsList[i];
        //        this.Deck.Add(this.GetCard(currentCard));
        //    }
        //}

        //private ICard GetCard(string cardId)
        //{
        //    // Card is mod type.
        //    var modCardFile = Path.Join(this.DownloadsDir, cardId);
        //    if (File.Exists(modCardFile))
        //    {
        //        // Mod card has data card.
        //        var modDataDir = Path.Join(this.CardsDir, cardId);
        //        if (Directory.Exists(modDataDir))
        //        {
        //            // Add mod folder card.
        //            var modDataCard = CardUtils.ParseCard<ModCard>(Path.Join(modDataDir, "card.json"));
        //            return modDataCard;
        //        }
        //        else
        //        {
        //            throw new ArgumentException($"{cardId} is missing its card data! Folder: {modDataDir}");
        //        }
        //    }

        //    // Card is folder type.
        //    var folderCardDir = Path.Join(this.CardsDir, cardId);
        //    if (Directory.Exists(folderCardDir))
        //    {
        //        var folderCardPath = Path.Join(folderCardDir, "card.json");
        //        var folderCard = CardUtils.ParseCard<FolderCard>(folderCardPath);
        //        return folderCard;
        //    }

        //    // Card is tool type.
        //    var toolCardDir = Path.Join(CaelumPaths.ToolsDir, cardId);
        //    if (Directory.Exists(toolCardDir))
        //    {
        //        var toolCardPath = Path.Join(toolCardDir, "card.json");
        //        var toolCard = CardUtils.ParseCard<ToolCard>(toolCardPath);
        //        return toolCard;
        //    }

        //    return null;
        //}

        //private GameConfig GetConfig()
        //{
        //    if (File.Exists(this.ConfigPath))
        //    {
        //        return CaelumParsing.ParseGameConfig(this.ConfigPath);
        //    }
        //    else
        //    {
        //        var defaultConfig = new GameConfig()
        //        {
        //            GameName = this.Name,
        //            OutputDirectory = this.BuildDir,
        //            CardsList = Array.Empty<string>(),
        //        };

        //        var defaultText = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions() { WriteIndented = true });
        //        File.WriteAllText(this.ConfigPath, defaultText);
        //        return defaultConfig;
        //    }
        //}
    }
}
