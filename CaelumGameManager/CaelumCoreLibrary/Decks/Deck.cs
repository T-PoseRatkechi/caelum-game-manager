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

    /// <summary>
    /// Base implementation of IDeck.
    /// </summary>
    public class Deck : IDeck
    {
        private readonly ICardsLoader cardsLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="cardsLoader">Cards loader to use.</param>
        /// <param name="gameConfigManager">Game config manager.</param>
        public Deck(ICardsLoader cardsLoader)
        {
            this.cardsLoader = cardsLoader;

            // Load initial cards.
            this.Cards = cardsLoader.GetInstalledCards();
        }

        /// <inheritdoc/>
        public event EventHandler OnDeckChanged;

        /// <inheritdoc/>
        public List<ICardModel> Cards { get; set; }

        /// <inheritdoc/>
        public void LoadDeckCards()
        {
            // Load installed cards.
            this.Cards = this.cardsLoader.GetInstalledCards();
            this.NotifyDeckChanged();
        }

        /// <inheritdoc/>
        public void AddCard(ICardModel card)
        {
            // Don't add duplicate card instances.
            if (this.Cards.Contains(card))
            {
                throw new ArgumentException("Cannot add duplicate card instance to deck.", nameof(card));
            }

            // Don't add cards if one already exists with the same card id.
            else if (this.Cards.FindIndex(cardInDeck => cardInDeck.CardId == card.CardId) > -1)
            {
                throw new ArgumentException($"Cannot add card because a card already exists with the same id {card.CardId}.", nameof(card));
            }
            else
            {
                this.Cards.Add(card);
            }

            this.NotifyDeckChanged();
        }

        /// <inheritdoc/>
        public void DeleteCard(ICardModel card)
        {
            // Could not remove card from deck.
            if (!this.Cards.Remove(card))
            {
                throw new ArgumentException($"Could not remove card {card.CardId} from deck.", nameof(card));
            }

            // Delete card installation folder.
            Directory.Delete(card.InstallDirectory, true);

            this.NotifyDeckChanged();
        }

        /// <inheritdoc/>
        public void HideCard(ICardModel card)
        {
            // Don't allow hiding cards that are already hidden.
            if (card.IsHidden)
            {
                throw new ArgumentException($"Could not set card {card.CardId} to hidden because card is already hidden!", nameof(card));
            }

            card.IsHidden = true;

            this.NotifyDeckChanged();
        }

        /// <inheritdoc/>
        public void NotifyDeckChanged()
        {
            this.OnDeckChanged?.Invoke(this, null);
        }
    }
}
