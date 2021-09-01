// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Data;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Cards.Converters;
    using CaelumCoreLibrary.Cards.Converters.Aemulus;
    using CaelumCoreLibrary.Games;
    using CaelumCoreLibrary.Utilities;
    using CaelumGameManagerGUI.Cards;
    using CaelumGameManagerGUI.Models;
    using CaelumGameManagerGUI.Resources.Localization;
    using CaelumGameManagerGUI.ViewModels.Cards;
    using Caliburn.Micro;
    using Microsoft.Win32;
    using Newtonsoft.Json;
    using Serilog;

    /// <summary>
    /// Deck VM.
    /// </summary>
    public class DeckViewModel : Screen
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
        private CardConverter _cardConverter;

        private readonly IWindowManager windowManager = new WindowManager();

        private bool isBuildingEnabled = true;

        private string selectedFilter = LocalizedStrings.Instance["AllText"];

        private BindableDeckModel _deck;
        private ICollectionView _filteredDeck;
        private ILauncherCardModel _selectedGameLauncher;
        private ICardModel _selectedCard;


        /// <summary>
        /// Initializes a new instance of the <see cref="DeckViewModel"/> class.
        /// </summary>
        public DeckViewModel(IGameInstance game, ICardFactory cardFactory, CardConverter cardConverter, BindableDeckModel deck)
        {
            this.game = game;
            this._deck = deck;
            this._cardFactory = cardFactory;
            this._cardConverter = cardConverter;

            this.FilteredDeck = CollectionViewSource.GetDefaultView(this._deck);
            this.SetInitialLauncher();
        }

        /// <summary>
        /// Gets or sets the deck with a filter.
        /// </summary>
        public ICollectionView FilteredDeck
        {
            get
            {
                return this._filteredDeck;
            }

            set
            {
                this._filteredDeck = value;
                this.NotifyOfPropertyChange(() => this.FilteredDeck);
            }
        }

        /// <summary>
        /// Gets a list of filters by name.
        /// </summary>
        public List<string> FilterKeys { get; } = new(Filters.Keys);

        /// <summary>
        /// Gets the game's available game launchers.
        /// </summary>
        public List<ICardModel> GameLauncher => this._deck.Where(card => card.Type == CardType.Launcher).ToList();

        /// <summary>
        /// Gets or sets the index of currently selected game launcher.
        /// </summary>
        public ILauncherCardModel SelectedGameLauncher
        {
            get
            {
                return this._selectedGameLauncher;
            }

            set
            {
                this._selectedGameLauncher = value;

                if (this._selectedGameLauncher == null)
                {
                    this._selectedGameLauncher = (ILauncherCardModel)this.GameLauncher[0];
                }

                this.game.GameConfig.Settings.DefaultGameLauncher = this._selectedGameLauncher.CardId;
                this.game.GameConfig.SaveGameConfig();
            }
        }

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
        /// Gets or sets selected card.
        /// </summary>
        public ICardModel SelectedCard
        {
            get
            {
                return this._selectedCard;
            }

            set
            {
                this._selectedCard = value;
                this.NotifyOfPropertyChange(() => this.IsCardSelected);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a card is selected.
        /// </summary>
        public bool IsCardSelected => this.SelectedCard != null;

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
                    this.windowManager.ShowDialogAsync(new CreateCardViewModel(this.game, this._cardFactory, this._deck, selectedItem as ICardModel));
                    return;
                }

                this.windowManager.ShowDialogAsync(new CreateCardViewModel(this.game, this._cardFactory, this._deck));
            }
        }

        /// <summary>
        /// Card filter to apply to deck and only display cards of <paramref name="type"/>.
        /// </summary>
        /// <param name="item">Card item.</param>
        /// <param name="type">Card type to display.</param>
        /// <returns>Whether card passes filter.</returns>
        private static bool FilterCardsByType(object item, CardType type)
        {
            if (item is ICardModel cardModel)
            {
                if (cardModel.Type == type)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetInitialLauncher()
        {
            // Set default launcher.
            this._selectedGameLauncher = (ILauncherCardModel)this._deck.FirstOrDefault(card => card.CardId == this.game.GameConfig.Settings.DefaultGameLauncher);
            if (this._selectedGameLauncher == null)
            {
                this._selectedGameLauncher = (ILauncherCardModel)this.GameLauncher[0];
            }
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
                await Task.Run(() => this.game.BuildDeck(this._deck));
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

        public void OpenCardFolder()
        {
            if (this.SelectedCard != null && this.SelectedCard.InstallDirectory != null)
            {
                Process.Start("explorer.exe", this.SelectedCard.InstallDirectory);
            }
        }

        public void StartGame()
        {
            try
            {
                this.CanBuildGameDeck = false;
                this.game.StartGame(this.SelectedGameLauncher);
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
