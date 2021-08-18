// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;

    /// <summary>
    /// Base implementation of IDeck.
    /// </summary>
    public class Deck : IDeck
    {
        private readonly ICaelumConfig caelumConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="caelumConfig">Caelum config to ues for directories.</param>
        public Deck(ICaelumConfig caelumConfig = null)
        {
            this.caelumConfig = caelumConfig;
        }

        /// <inheritdoc/>
        public List<CardModel> Cards { get; set; } = new();

        /// <inheritdoc/>
        public void AddCard(CardModel card)
        {
            // Don't add duplicate card instances.
            if (this.Cards.Contains(card))
            {
                throw new ArgumentException("Cannot add duplicate card instance to deck!", nameof(card));
            }

            // Don't add cards if one already exists with the same card id.
            else if (this.Cards.FindIndex(cardInDeck => cardInDeck.CardId == card.CardId) > -1)
            {
                throw new ArgumentException($"Cannot add card because a card already exists with the same id! Card ID: {card.CardId}", nameof(card));
            }
            else
            {
                this.Cards.Add(card);
            }
        }

        /// <inheritdoc/>
        public void DeleteCard(CardModel card)
        {
            // Could not remove card from deck.
            if (!this.Cards.Remove(card))
            {
                throw new ArgumentException($"Could not remove card from deck! Card ID: {card.CardId}", nameof(card));
            }

            // Delete card installation folder.
            Directory.Delete(card.InstallDirectory, true);
        }

        /// <inheritdoc/>
        public void HideCard(CardModel card)
        {
            // Don't allow hiding cards that are already hidden.
            if (card.IsHidden)
            {
                throw new ArgumentException($"Could not set card to hidden because card is already hidden! Card ID: {card.CardId}", nameof(card));
            }

            card.IsHidden = true;
        }
    }
}
