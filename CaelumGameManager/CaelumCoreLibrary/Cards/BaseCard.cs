using CaelumCoreLibrary.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.Cards
{
    public abstract class BaseCard : ICard
    {
        private CardData data;

        public BaseCard(string path, CardType type)
        {
            Data = new() { Path = path, Type = type };
        }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string[] Authors { get; set; }

        /// <inheritdoc/>
        public string Game { get; set; }

        /// <inheritdoc/>
        public string Version { get; set; }

        /// <inheritdoc/>
        public CardData Data
        {
            get { return data; }
            private init { data = value; }
        }
    }
}
