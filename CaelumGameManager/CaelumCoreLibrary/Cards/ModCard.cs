using CaelumCoreLibrary.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.Cards
{
    public class ModCard : BaseCard
    {
        /// <summary>
        /// Initializes a new mod package.
        /// </summary>
        /// <param name="path">Path to mod package data.</param>
        public ModCard(string path) : base(path, CardType.Mod)
        {
        }
    }
}
