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

        public string Name => this.card.Name;
        public string Authors => string.Join(", ", card.Authors);
        public string Game => this.card.Game;
        public string PackagePath => this.card.Data.Path;
        public string PackageType => this.card.Data.Type.ToString();
    }
}
