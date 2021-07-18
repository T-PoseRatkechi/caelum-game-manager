// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System;
    using System.IO;
    using System.Text.Json;
    using CaelumCoreLibrary.Cards;
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
        /// Gets game's config path.
        /// </summary>
        public GameConfig Config { get; private set; }

        /// <summary>
        /// Gets game's current config version.
        /// </summary>
        public int ConfigVersion { get; init; }

        /// <summary>
        /// Gets game's cards directory.
        /// </summary>
        public string CardsDir { get; init; }

        /// <summary>
        /// Gets game's downloads directory.
        /// </summary>
        public string DownloadsDir { get; init; }

        /// <summary>
        /// Gets game's build directory.
        /// </summary>
        public string BuildDir { get; init; }

        /// <inheritdoc/>
        public ICard CreateCard(string id, string name, CardType type, string[] authors, string version)
        {
            switch (type)
            {
                case CardType.Folder:
                    var cardFolderPath = Path.Join(this.CardsDir, id);
                    var newFolderCard = new FolderCard(cardFolderPath)
                    {
                        Id = id,
                        Name = name,
                        Authors = authors,
                        Game = this.Name,
                        Version = version,
                    };

                    return newFolderCard;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Initializes core game data.
        /// </summary>
        protected virtual void GameInit()
        {
            this.LoadConfig();
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
                    Deck = Array.Empty<string>(),
                };

                var defaultText = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions() { WriteIndented = true });
                File.WriteAllText(this.ConfigPath, defaultText);
                this.Config = defaultConfig;
            }
        }
    }
}
