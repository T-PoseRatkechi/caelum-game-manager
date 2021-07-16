using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.Cards
{
    /// <summary>
    /// Interface that all package types implement.
    /// </summary>
    public interface ICard
    {
        /// <summary>
        /// Toggle whether the card is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Card id that must be unique in the given game deck. Only exception are Update Cards.
        /// Should typically be: {author_name}.{card_name}
        /// Update Cards must have the same id as the card they are updating.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of card.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name(s) of authors that created the card.
        /// </summary>
        public string[] Authors { get; set; }

        /// <summary>
        /// Name of game this card is for.
        /// </summary>
        public string Game { get; set; }

        /// <summary>
        /// Version of card.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Card data, such as card type and path to data.
        /// </summary>
        public CardData Data { get; }
    }
}
