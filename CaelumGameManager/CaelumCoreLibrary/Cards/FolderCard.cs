using CaelumCoreLibrary.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.Cards
{
    public class FolderCard : BaseCard
    {
        public FolderCard(string path) : base(path, CardType.Folder)
        {
        }
    }
}
