using CaelumCoreLibrary.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.Decks
{
    public class DeckModel
    {
        public string Game { get; set; }
        public string Format { get; set; }
        public string Name { get; set; }
        public ICard[] Cards { get; set; }
    }
}
