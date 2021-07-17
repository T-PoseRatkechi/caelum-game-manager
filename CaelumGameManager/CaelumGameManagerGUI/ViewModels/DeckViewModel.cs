using CaelumCoreLibrary.Cards;
using CaelumGameManagerGUI.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CaelumGameManagerGUI.ViewModels
{
    public class DeckViewModel : Screen
    {
        IWindowManager WindowManager { get; set; } = new WindowManager();

        private BindableCollection<CardModel> deck = new();

        private ICollectionView filteredDeck;

        public ICollectionView FilteredDeck
        {
            get { return filteredDeck; }
            set 
            { 
                filteredDeck = value;
                NotifyOfPropertyChange("FilteredDeck");
            }
        }


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
                Authors = new string[] { "T-Pose Ratkechi", "T-Pose Ratkechi", "T-Pose Ratkechi", "T-Pose Ratkechi", "T-Pose Ratkechi" },
                Game = "Persona 4 Golden",
                Version = "0.1.2",
            }));

            deck.Add(new CardModel(new ToolCard("test/path/caelum music manager")
            {
                IsEnabled = true,
                Id = "tpose-ratkechi.caelummusicmanager",
                Name = "Caelum Music Manager",
                Authors = new string[] { "T-Pose Ratkechi", "T-Pose Ratkechi", "T-Pose Ratkechi" },
                Game = "Persona 4 Golden",
                Version = "1.2.489",
            }));

            FilteredDeck = CollectionViewSource.GetDefaultView(deck);
        }

        private bool FilterCardsByType(object item, CardType type)
        {
            CardModel cardModel = item as CardModel;
            if (cardModel != null)
            {
                if (cardModel.Card.Data.Type == type)
                {
                    return true;
                }
            }

            return false;
        }

        public ICollectionView Deck { get => this.FilteredDeck; }

        private string selectedFilter = "All";

        public string SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                switch (value)
                {
                    case "Mods":
                        this.FilteredDeck.Filter = (obj) => FilterCardsByType(obj, CardType.Mod);
                        break;
                    case "Tools":
                        this.FilteredDeck.Filter = (obj) => FilterCardsByType(obj, CardType.Tool);
                        break;
                    case "Folder":
                        this.FilteredDeck.Filter = (obj) => FilterCardsByType(obj, CardType.Folder);
                        break;
                    case "All":
                    default:
                        this.FilteredDeck.Filter = null;
                        break;
                }
            }
        }

        public void OpenEditCard(string context)
        {
            this.WindowManager.ShowWindowAsync(new EditCardViewModel(context));
        }

        public string[] Filters { get; } = new string[] { "All", "Mods", "Tools", "Folder" };
    }
}
