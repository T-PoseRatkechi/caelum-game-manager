using CaelumCoreLibrary.Cards;
using CaelumGameManagerGUI.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaelumGameManagerGUI.ViewModels
{
    public class DeckViewModel : Screen
    {
        private BindableCollection<CardModel> deck = new();

        public DeckViewModel()
        {
            deck.Add(new CardModel(new ModCard("test/path/bgme a") 
            {
                IsEnabled=true,
                Id="tpose-ratkechi.bgmea",
                Name="BGME:A",
                Authors=new string[] { "T-Pose Ratkechi" },
                Game="Persona 4 Golden",
                Version="1.0.0",
            }));;

            deck.Add(new CardModel(new FolderCard("test/path/randoimzed encounters")
            {
                IsEnabled = false,
                Id = "tpose-ratkechi.randomizedencounters",
                Name = "Randomized Encounters",
                Authors = new string[] { "T-Pose Ratkechi" },
                Game = "Persona 4 Golden",
                Version = "0.1.2",
            }));

            deck.Add(new CardModel(new ToolCard("test/path/caelum music manager")
            {
                IsEnabled = true,
                Id = "tpose-ratkechi.caelummusicmanager",
                Name = "Caelum Music Manager",
                Authors = new string[] { "T-Pose Ratkechi" },
                Game = "Persona 4 Golden",
                Version = "1.2.489",
            }));
        }


        public BindableCollection<CardModel> Deck
        {
            get { return deck; }
            set { deck = value; }
        }
    }
}
