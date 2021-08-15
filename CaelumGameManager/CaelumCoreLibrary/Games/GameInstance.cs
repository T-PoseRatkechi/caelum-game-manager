// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Configs.Writers;

    /// <summary>
    /// Base class for game instances.
    /// </summary>
    public class GameInstance : IGameInstance
    {
        private IWriter writer;

        private string configFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstance"/> class.
        /// </summary>
        /// <param name="gameName">Name of game.</param>
        /// <param name="writer">Writer to use.</param>
        /// <param name="gameInstallFactory">Game install factory.</param>
        public GameInstance(string gameName, IWriter writer, IGameInstallFactory gameInstallFactory)
        {
            this.writer = writer;

            this.Install = gameInstallFactory.GetGameInstall(gameName);
            this.configFilePath = Path.Join(this.Install.BaseDirectory, "game-config.json");
        }

        /// <inheritdoc/>
        public IGameInstall Install { get; init; }

        /// <inheritdoc/>
        public IGameConfig GameConfig { get; private set; }

        /// <inheritdoc/>
        public List<ICard> Deck { get; init; } = new();

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
    }
}
