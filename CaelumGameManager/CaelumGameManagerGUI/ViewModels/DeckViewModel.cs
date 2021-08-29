// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Data;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Cards.Converters;
    using CaelumCoreLibrary.Cards.Converters.Aemulus;
    using CaelumCoreLibrary.Games;
    using CaelumGameManagerGUI.Models;
    using CaelumGameManagerGUI.Resources.Localization;
    using CaelumGameManagerGUI.ViewModels.Cards;
    using Caliburn.Micro;
    using Microsoft.Win32;
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
        private BindableDeckModel _deck;

        private string selectedFilter = LocalizedStrings.Instance["AllText"];

        private ICollectionView filteredDeck;


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
                this.NotifyOfPropertyChange(() => this.FilteredDeck);
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
                    this.windowManager.ShowDialogAsync(new CreateCardViewModel(this.game, this._cardFactory, this._deck, selectedItem as ICardModel));
                    return;
                }

                this.windowManager.ShowDialogAsync(new CreateCardViewModel(this.game, this._cardFactory, this._deck));
            }
        }

        /// <summary>
        /// Import Aemulus packages and settings.
        /// </summary>
        /// <param name="sender">Sender.</param>
        public async void ImportAemulus(object sender)
        {
            if (sender != null)
            {
                string aemulusDir = null;

                OpenFileDialog openFileDialog = new();
                openFileDialog.Filter = $"AemulusPackageManager.exe| *.exe";
                openFileDialog.Title = "Select AemulusPackageManager.exe...";

                if (openFileDialog.ShowDialog() == true)
                {
                    // No file selected.
                    if (string.IsNullOrEmpty(openFileDialog.FileName))
                    {
                        return;
                    }

                    aemulusDir = Path.GetDirectoryName(openFileDialog.FileName);
                }
                else
                {
                    // Selection cancelled.
                    return;
                }

                try
                {
                    Log.Information("Importing Aemulus packages. This may take a while.");
                    await Task.Run(() => this._cardConverter.AemulusConverter.Import(aemulusDir, this.game.GameInstall.CardsDirectory));

                    Log.Information("Loading Aemulus settings.");
                    var p4gGamePackagesFile = Path.Join(aemulusDir, "Config", "Persona4GoldenPackages.xml");
                    var p4gGamePackages = GamePackagesParser.ParseGamePackagesXml(p4gGamePackagesFile);

                    // Set Card order to Aemulus order.
                    var aemulusPackageOrder = p4gGamePackages.packages.Select(x => x.id).ToArray();
                    this.game.GameConfig.Settings.Cards = aemulusPackageOrder;

                    // Get P4G install from Aemulus config.
                    var aemulusConfig = AemulusConfigParser.ParseAemulusConfig(Path.Join(aemulusDir, "Config", "Config.xml"));
                    if (aemulusConfig.p4gConfig.exePath != null)
                    {
                        this.game.GameConfig.Settings.GameInstallPath = Path.GetDirectoryName(aemulusConfig.p4gConfig.exePath);
                        this.game.GameConfig.Settings.OutputDirectory = aemulusConfig.p4gConfig.modDir;
                        this.game.GameConfig.Settings.GameLauncher = aemulusConfig.p4gConfig.reloadedPath;
                        this.game.GameConfig.Settings.OutputBuildOnly = true;
                    }

                    Log.Information("Aemulus settings loaded. Reloading cards.");
                    this.game.Deck.LoadDeckCards();
                    this.game.InitGame();
                    this._deck.Clear();
                    this._deck.AddRange(this.game.Deck.Cards);

                    // Set card enabled settings same as Aemulus.
                    foreach (var gamePackage in p4gGamePackages.packages)
                    {
                        var match = this._deck.FirstOrDefault(x => x.CardId == gamePackage.id);
                        if (match != null)
                        {
                            match.IsEnabled = gamePackage.enabled;
                        }
                    }

                    Log.Information("Cards reloaded. Aemulus successfully imported.");
                    this.game.GameConfig.SaveGameConfig();
                }
                catch (Exception e)
                {
                    Log.Error(e, "Problem encountered while importing Aemulus.");
                }
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
    }
}
