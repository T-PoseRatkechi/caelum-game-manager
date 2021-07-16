using CaelumCoreLibrary.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.Cards
{
    public class ToolCard : BaseCard
    {
        /// <summary>
        /// Initializes a new tool card.
        /// </summary>
        /// <param name="path">Path to tool card data.</param>
        public ToolCard(string path) : base(path, CardType.Tool)
        {
        }
    }
}
