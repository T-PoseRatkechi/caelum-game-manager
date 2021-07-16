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

        public string Authors
        {
            get
            {
                if (Card.Authors == null)
                {
                    return "Unknown";
                }

                if (Card.Authors.Length == 1)
                {
                    return Card.Authors[0];
                }

                if (Card.Authors.Length > 1)
                {
                    return $"{Card.Authors[0]} +{Card.Authors.Length} other(s)";
                }

                return "Unknown";
            }
        }
    }
}
