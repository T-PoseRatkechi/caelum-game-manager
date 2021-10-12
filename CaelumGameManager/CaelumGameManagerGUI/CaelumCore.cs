// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI
{
    using CaelumCoreLibrary.Builders;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Games;

    /// <summary>
    /// Caelum core.
    /// </summary>
    public class CaelumCore
    {
        private readonly IGameInstanceFactory gameInstanceFactory;
        private readonly ICardFactory cardFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaelumCore"/> class.
        /// </summary>
        /// <param name="gameInstanceFactory">Game instance factory.</param>
        /// <param name="deckBuilder">Deck builder.</param>
        /// <param name="cardFactory">Card factory.</param>
        public CaelumCore(IGameInstanceFactory gameInstanceFactory, ICardFactory cardFactory)
        {
            this.cardFactory = cardFactory;
            this.gameInstanceFactory = gameInstanceFactory;
        }

        /// <summary>
        /// Gets card factory.
        /// </summary>
        public ICardFactory CardFactory => this.cardFactory;

        /// <summary>
        /// Gets a game instance.
        /// </summary>
        /// <returns>Game instance.</returns>
        public IGameInstance GetGameInstance()
        {
            return this.gameInstanceFactory.CreateGameInstance("Persona 4 Golden");
            // return this.gameInstanceFactory.CreateGameInstance("Persona 5");
        }
    }
}
