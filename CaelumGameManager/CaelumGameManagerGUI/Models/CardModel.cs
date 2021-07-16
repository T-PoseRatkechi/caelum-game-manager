using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaelumCoreLibrary.Cards;

namespace CaelumGameManagerGUI.Models
{
    public class CardModel
    {
        private readonly ICard card;

        public CardModel(ICard card)
        {
            this.card = card;
        }

        public ICard Card { get => this.card; }

        public string Authors => string.Join(", ", card.Authors);
    }
}
