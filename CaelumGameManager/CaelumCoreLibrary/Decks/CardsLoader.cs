// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Loads cards from tools and game install folder.
    /// </summary>
    public class CardsLoader : ICardsLoader
    {
        private readonly ILogger log;
        private readonly ICardParser cardParser;
        private readonly string toolsDir;
        private readonly string gameCardsDir;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsLoader"/> class.
        /// </summary>
        /// <param name="caelumConfig">Caelum config to use for directories.</param>
        /// <param name="gameInstall">Game install instance.</param>
        /// <param name="cardParser">Card parser to use.</param>
        public CardsLoader(ILogger log, ICaelumConfig caelumConfig, ICardParser cardParser, IGameInstall gameInstall)
        {
            this.log = log;
            this.toolsDir = caelumConfig.ToolsDirectory;
            this.gameCardsDir = gameInstall.CardsDirectory;
            this.cardParser = cardParser;
        }

        /// <inheritdoc/>
        public List<CardModel> GetInstalledCards()
        {
            this.log.LogDebug("Loading installed cards");

            List<CardModel> installedCards = new();

            this.AddCardsFromFolder(installedCards, this.toolsDir);
            this.AddCardsFromFolder(installedCards, this.gameCardsDir);

            this.log.LogInformation("Loaded {InstalledCardsTotal} total cards", installedCards.Count);

            return installedCards;
        }

        /// <summary>
        /// Adds every card found in <paramref name="folder"/> to <paramref name="cardsList"/>.
        /// </summary>
        /// <param name="cardsList">List to add new cards to.</param>
        /// <param name="folder">Folder to seach for cards in.</param>
        private void AddCardsFromFolder(List<CardModel> cardsList, string folder)
        {
            this.log.LogDebug("Loading cards in folder {Folder}", folder);

            foreach (string cardFolder in Directory.GetDirectories(folder))
            {
                // Parse card.
                var card = this.cardParser.ParseCardFile(Path.Join(cardFolder, "card.json"));

                // Validate card.
                // Don't allow duplicate cards to be added.
                if (cardsList.FindIndex(x => x.CardId == card.CardId) > -1)
                {
                    throw new InvalidOperationException($@"Cannot add card because a card already exists with the id ""{card.CardId}""! File: {cardFolder}");
                }

                // Add card to list.
                cardsList.Add(card);
                this.log.LogDebug("Loaded card {CardName} with Card ID {CardId}", card.Name, card.CardId);
            }
        }
    }
}
