// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.IO;
    using CaelumCoreLibrary.Builders;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Decks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Default implementation of <seealso cref="IGameInstanceFactory"/>.
    /// </summary>
    public class GameInstanceFactory : IGameInstanceFactory
    {
        private readonly ILogger log;
        private readonly IGameInstallFactory gameInstallFactory;
        private readonly IGameConfigManagerFactory gameConfigManagerFactory;
        private readonly ICardsLoaderFactory cardsLoaderFactory;
        private readonly IDeckFactory deckFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstanceFactory"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameInstallFactory">Game install factory.</param>
        /// <param name="gameConfigManagerFactory">Game config manager factory.</param>
        /// <param name="cardsLoaderFactory">Cards loader factory.</param>
        /// <param name="deckFactory">Deck factory.</param>
        public GameInstanceFactory(
            ILogger log,
            IGameInstallFactory gameInstallFactory,
            IGameConfigManagerFactory gameConfigManagerFactory,
            ICardsLoaderFactory cardsLoaderFactory,
            IDeckFactory deckFactory)
        {
            this.log = log;
            this.cardsLoaderFactory = cardsLoaderFactory;
            this.gameInstallFactory = gameInstallFactory;
            this.gameConfigManagerFactory = gameConfigManagerFactory;
            this.deckFactory = deckFactory;
        }

        /// <inheritdoc/>
        public IGameInstance CreateGameInstance(string gameName, IDeckBuilder deckBuilder)
        {
            this.log.LogDebug("Building game instance with name {GameName}", gameName);

            var gameInstall = this.gameInstallFactory.CreateGameInstall(gameName);
            var cardsLoader = this.cardsLoaderFactory.CreateCardsLoader(gameInstall);

            IGameInstance gameInstance = new GameInstance(this.log, deckBuilder)
            {
                GameInstall = gameInstall,
                GameConfig = this.gameConfigManagerFactory.CreateGameConfigManager(Path.Join(gameInstall.BaseDirectory, "game-config.json")),
                Deck = this.deckFactory.CreateDeck(cardsLoader),
            };

            return gameInstance;
        }
    }
}
