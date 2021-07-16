// using CaelumGameManagerGUI.Models;
using CaelumCoreLibrary.Cards;
using CaelumGameManagerGUI.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumGameManagerGUI.ViewModels
{
    public class ShellViewModel : Conductor<Screen>
    {
        public ShellViewModel()
        {
            ActivateItemAsync(new DeckViewModel());
        }
    }
}
