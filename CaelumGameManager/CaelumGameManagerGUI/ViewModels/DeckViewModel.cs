// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using System.ComponentModel;
    using System.Windows.Controls;
    using System.Windows.Data;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Games;
    using CaelumGameManagerGUI.Models;
    using Caliburn.Micro;

    /// <summary>
    /// Deck VM.
    /// </summary>
    public class DeckViewModel : Screen
    {
        private readonly IWindowManager windowManager = new WindowManager();

        private BindableCollection<CardModel> deck = new();

        private string selectedFilter = "All";

        private ICollectionView filteredDeck;


        /// <summary>
        /// Initializes a new instance of the <see cref="DeckViewModel"/> class.
        /// </summary>
        public DeckViewModel()
        {
            this.FilteredDeck = CollectionViewSource.GetDefaultView(this.deck);
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
        /// Gets list of available filters.
        /// </summary>
        public string[] Filters { get; } = new string[] { "All", "Mods", "Tools", "Folder" };

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
                    this.windowManager.ShowDialogAsync(new EditCardViewModel("edit", new GameP4G(), this.deck));
                    return;
                }

                this.windowManager.ShowDialogAsync(new EditCardViewModel("create", new GameP4G(), this.deck));
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
    }
}
