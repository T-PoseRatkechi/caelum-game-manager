using CaelumCoreLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumCoreLibrary.Cards
{
    public class InstallableCard : IInstallableCard
    {
        public string InstallPath { get; set; }
        public bool IsEnabled { get; set; }
        public string CardId { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
        public List<Author> Authors { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public CardType Type { get; set;  }
    }
}
