namespace CaelumCoreLibrary.Decks
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Interface for decks.
    /// </summary>
    public interface IDeck
    {
        /// <summary>
        /// Gets cards in deck.
        /// </summary>
        List<CardModel> Cards { get; }

        /// <summary>
        /// Loads deck cards.
        /// </summary>
        void LoadDeck();

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
    }
}
