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
    public class ShellViewModel : Screen
    {
        private BindableCollection<CardModel> deck = new();

        public ShellViewModel()
        {

        }

        public BindableCollection<CardModel> Deck
        {
            get { return deck; }
            set { deck = value; }
        }
    }
}
