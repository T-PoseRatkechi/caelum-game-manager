// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using System.Collections.Generic;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Controls;
    using System.Windows.Data;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Games;
    using CaelumGameManagerGUI.Resources.Localization;
    using CaelumGameManagerGUI.ViewModels.Cards;
    using Caliburn.Micro;
    using GongSolutions.Wpf.DragDrop;
    using Serilog;
    using System;
    using System.Threading.Tasks;
    using CaelumCoreLibrary.Cards.Converters.Aemulus;
    using Microsoft.Win32;
    using System.IO;

    /// <summary>
    /// Deck VM.
    /// </summary>
    public class DeckViewModel : Screen, IDropTarget
    {
        /// <summary>
        /// Gets list of available filters.
        /// </summary>
        private static readonly Dictionary<string, CardType> Filters = new()
        {
            // No filter uses the card type Update since Update cards are never displayed in deck.
            // They're only used overwrite another card's files.
            { LocalizedStrings.Instance["AllText"], CardType.Update },
            { LocalizedStrings.Instance["ModsText"], CardType.Mod },
            { LocalizedStrings.Instance["ToolsText"], CardType.Tool },
            { LocalizedStrings.Instance["FoldersText"], CardType.Folder },
        };

        private IGameInstance game;
        private ICardFactory _cardFactory;

        private readonly IWindowManager windowManager = new WindowManager();

        private bool isBuildingEnabled = true;
        private BindableCollection<CardModel> _deck;

        private string selectedFilter = LocalizedStrings.Instance["AllText"];

        private ICollectionView filteredDeck;


        /// <summary>
        /// Initializes a new instance of the <see cref="DeckViewModel"/> class.
        /// </summary>
        public DeckViewModel(IGameInstance game, ICardFactory cardFactory, BindableCollection<CardModel> deck)
        {
            this.game = game;
            this._deck = deck;
            this._cardFactory = cardFactory;

            this.FilteredDeck = CollectionViewSource.GetDefaultView(this._deck);
        }

        /// <summary>
        /// Gets or sets the deck with a filter.
        /// </summary>
        public ICollectionView FilteredDeck
        {
            get
            {
                return this.filteredDeck;
            }

            set
            {
                this.filteredDeck = value;
                this.NotifyOfPropertyChange(nameof(this.FilteredDeck));
            }
        }

        /// <summary>
        /// Gets a list of filters by name.
        /// </summary>
        public List<string> FilterKeys { get; } = new(Filters.Keys);

        /// <summary>
        /// Gets or sets the selected filter on deck.
        /// </summary>
        public string SelectedFilter
        {
            get
            {
                return this.selectedFilter;
            }

            set
            {
                this.selectedFilter = value;
                if (Filters.ContainsKey(this.selectedFilter))
                {
                    var cardType = Filters[this.selectedFilter];
                    if (cardType == CardType.Update)
                    {
                        this.FilteredDeck.Filter = null;
                    }
                    else
                    {
                        this.FilteredDeck.Filter = (obj) => FilterCardsByType(obj, cardType);
                    }
                }
                else
                {
                    Log.Warning("Unrecognized card filter! Filter name: {filter}", this.selectedFilter);
                }
            }
        }

        /// <summary>
        /// Open the Create/Edit Card window.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        public void OpenEditCard(object sender)
        {
            if (sender != null)
            {
                var menuItem = sender as MenuItem;
                var contextMenu = menuItem.Parent as ContextMenu;
                var item = contextMenu.PlacementTarget as DataGrid;
                var selectedItem = item.SelectedItem;

                if (selectedItem != null)
                {
                    this.windowManager.ShowDialogAsync(new CreateCardViewModel(this.game, this._cardFactory, this._deck, selectedItem as CardModel));
                    return;
                }

                this.windowManager.ShowDialogAsync(new CreateCardViewModel(this.game, this._cardFactory, this._deck));
            }
        }

        public void ConvertAemulus(object sender)
        {
            if (sender != null)
            {
                var converter = new AemulusPackageConverter(null, this.game.GameInstall);
                string aemulusDir = null;

                OpenFileDialog openFileDialog = new();
                openFileDialog.Filter = $"AemulusPackageManager.exe| *.exe";
                openFileDialog.Title = "Select Aemulus exe...";
                if (openFileDialog.ShowDialog() == true)
                {
                    aemulusDir = Path.GetDirectoryName(openFileDialog.FileName);
                }

                converter.ConvertAemulusPackages(aemulusDir);
            }
        }

        /// <inheritdoc/>
        public void DragOver(IDropInfo dropInfo)
        {
            // Call default DragOver method, cause most stuff should work by default.
            DragDrop.DefaultDropHandler.DragOver(dropInfo);
        }

        /// <inheritdoc/>
        public void Drop(IDropInfo dropInfo)
        {
            // DragDrop drop event removes all selected cards at start from BindableCollection.
            // The following will also remove the selected cards from GameInstance's deck.
            // They will then be added back through the BindableCollection's InsertItem method.

            // Kinda iffy solution. DropInfo.Data should be a typed enumerable
            // but I don't think it is or I'm misunderstanding something...
            if (dropInfo.Data is IEnumerable groupedCards)
            {
                // Remove cards from group drop.
                foreach (CardModel card in groupedCards)
                {
                    var deckCardIndex = this.game.Deck.Cards.FindIndex(c => c.CardId == card.CardId);

                    if (deckCardIndex < 0)
                    {
                        Log.Fatal("Failed to remove a card in group drop event! Unexpected behaviour likely to occur!");
                        Log.Fatal(LocalizedStrings.Instance["ErrorRecommendRestartMessage"]);
                    }
                    else
                    {
                        this.game.Deck.Cards.RemoveAt(deckCardIndex);
                        Log.Verbose("Removed card with id {CardId}", card.CardId);
                    }
                }
            }
            else
            {
                // Remove card from solo drop.
                // Strange this didn't break like the group drop...
                var card = (CardModel)dropInfo.Data;
                if (!this.game.Deck.Cards.Remove(card))
                {
                    Log.Fatal("Failed to remove a card in drop event! Unexpect behaviour likely to occur!");
                    Log.Fatal(LocalizedStrings.Instance["ErrorRecommendRestartMessage"]);
                }
                else
                {
                    Log.Verbose("Removed card with id {CardId}", card.CardId);
                }
            }

            // Default drop event.
            DragDrop.DefaultDropHandler.Drop(dropInfo);
        }

        /// <summary>
        /// Card filter to apply to deck and only display cards of <paramref name="type"/>.
        /// </summary>
        /// <param name="item">Card item.</param>
        /// <param name="type">Card type to display.</param>
        /// <returns>Whether card passes filter.</returns>
        private static bool FilterCardsByType(object item, CardType type)
        {
            CardModel cardModel = item as CardModel;
            if (cardModel != null)
            {
                if (cardModel.Type == type)
                {
                    return true;
                }
            }

            return false;
        }

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1201 // Elements should appear in the correct order
        public bool CanBuildGameDeck
        {
            get
            {
                return this.isBuildingEnabled;
            }

            set
            {
                this.isBuildingEnabled = value;
                this.NotifyOfPropertyChange(() => this.CanBuildGameDeck);
            }
        }

        public async void BuildGameDeck()
        {
            try
            {
                this.CanBuildGameDeck = false;
                await Task.Run(() => this.game.BuildDeck());
            }
            catch (Exception e)
            {
                Log.Error(e, LocalizedStrings.Instance["ErrorDeckBuildFailedMessage"]);
            }
            finally
            {
                this.CanBuildGameDeck = true;
            }
        }
    }
}
