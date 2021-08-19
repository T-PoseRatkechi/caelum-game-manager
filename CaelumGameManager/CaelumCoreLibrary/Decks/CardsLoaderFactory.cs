// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Default implementation of <seealso cref="ICardsLoaderFactory"/>.
    /// </summary>
    public class CardsLoaderFactory : ICardsLoaderFactory
    {
        private readonly ILogger log;
        private readonly ICaelumConfig caelumConfig;
        private readonly ICardParser cardParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsLoaderFactory"/> class.
        /// </summary>
        /// <param name="caelumConfig">Caelum config.</param>
        /// <param name="cardParser">Card parser.</param>
        public CardsLoaderFactory(ILogger log, ICaelumConfig caelumConfig, ICardParser cardParser)
        {
            this.log = log;
            this.caelumConfig = caelumConfig;
            this.cardParser = cardParser;
        }

        /// <inheritdoc/>
        public ICardsLoader CreateCardsLoader(IGameInstall gameInstall)
        {
            return new CardsLoader(this.log, this.caelumConfig, this.cardParser, gameInstall);
        }
    }
}
