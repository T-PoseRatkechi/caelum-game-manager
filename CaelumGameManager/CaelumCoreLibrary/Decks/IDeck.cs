﻿namespace CaelumCoreLibrary.Decks
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Interface for decks.
    /// </summary>
    public interface IDeck
    {
        /// <summary>
        /// Gets or sets cards in deck.
        /// </summary>
        List<InstallableCardModel> Cards { get; set; }

        /// <summary>
        /// Adds <paramref name="cardToAdd"/> to deck.
        /// </summary>
        /// <param name="cardToAdd">Card to add to deck.</param>

        void AddCard(InstallableCardModel cardToAdd);

        /// <summary>
        /// Hides the card at <paramref name="card"/> from the deck.
        /// </summary>
        /// <param name="card">Card to hide.</param>

        void HideCard(InstallableCardModel card);

        /// <summary>
        /// Deletes the card at <paramref name="card"/> from deck and it's installation.
        /// </summary>
        /// <param name="card">Card to delete.</param>

        void DeleteCard(InstallableCardModel card);
    }
}
