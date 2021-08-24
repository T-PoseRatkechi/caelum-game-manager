namespace CaelumCoreLibrary.Decks
{
    using System;
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Interface for decks.
    /// </summary>
    public interface IDeck
    {
        /// <summary>
        /// Deck changed handler.
        /// </summary>
        public event EventHandler OnDeckChanged;

        /// <summary>
        /// Gets or sets cards in deck.
        /// </summary>
        List<CardModel> Cards { get; set; }

        /// <summary>
        /// Loads deck cards.
        /// </summary>
        void LoadDeckCards();

        /// <summary>
        /// Adds <paramref name="cardToAdd"/> to deck.
        /// </summary>
        /// <param name="cardToAdd">Card to add.</param>

        void AddCard(CardModel cardToAdd);

        /// <summary>
        /// Hides the <paramref name="card"/> from deck.
        /// </summary>
        /// <param name="card">Card to hide.</param>

        void HideCard(CardModel card);

        /// <summary>
        /// Deletes the <paramref name="card"/> from deck and it's installation folder.
        /// </summary>
        /// <param name="card">Card to delete.</param>
        void DeleteCard(CardModel card);

        /// <summary>
        /// Notify that deck changed.
        /// </summary>
        void NotifyDeckChanged();
    }
}
