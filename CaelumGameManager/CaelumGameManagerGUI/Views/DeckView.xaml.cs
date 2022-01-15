// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Views
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using CaelumGameManagerGUI.Resources.Localization;

    /// <summary>
    /// Interaction logic for DeckView.xaml
    /// </summary>
    public partial class DeckView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckView"/> class.
        /// </summary>
        public DeckView()
        {
            this.InitializeComponent();
            this.SetColumnWidth();

            this.OpenEditCard.Loaded += (sender, evt) =>
            {
                if (sender != null)
                {
                    var menuItem = sender as MenuItem;
                    var contextMenu = menuItem.Parent as ContextMenu;
                    var item = contextMenu.PlacementTarget as DataGrid;
                    var selectedItem = item.SelectedItem;

                    if (selectedItem != null)
                    {
                        this.OpenEditCard.Header = LocalizedStrings.Instance["EditCardText"];
                    }
                    else
                    {
                        this.OpenEditCard.Header = LocalizedStrings.Instance["CreateCardText"];
                    }
                }
            };
        }

        /// <summary>
        /// Sets column widths according to the following:
        /// 0. XAML sets inital widths to Size to Header.
        /// 1. First column (Enabled) is set to disable resize.
        /// 2. All columns between first and last have their min width set to current width (Size to Header).
        ///    then have their current widths set to fit their content.
        /// 3. The last column has their width set to fill remaining space.
        /// </summary>
        private void SetColumnWidth()
        {
            this.FilteredDeck.Loaded += (sender, evt) =>
            {
                for (int i = 0, total = this.FilteredDeck.Columns.Count; i < total; i++)
                {
                    // Set min width to Header size (XAML has Column Width = SizeToHeader)
                    this.FilteredDeck.Columns[i].MinWidth = this.FilteredDeck.Columns[i].ActualWidth;

                    // Set first column (Enabled) to disable resize.
                    if (i == 0)
                    {
                        this.FilteredDeck.Columns[i].CanUserResize = false;
                    }

                    // Set column widths to fit cells content.
                    // Set last column to fill remaining space to remove empty column area.
                    this.FilteredDeck.Columns[i].Width = (i != total - 1) ? new DataGridLength(1, DataGridLengthUnitType.SizeToCells) : new DataGridLength(1, DataGridLengthUnitType.Star);
                }
            };
        }

        /// <summary>
        /// Diselect any items when mouse clicks in empty space.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        private void FilteredDeck_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;

                if (grid != null && grid.SelectedItem != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow row = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;

                    row.IsSelected = row.IsMouseOver;
                }
            }
        }
    }

    /// <summary>
    /// Converts card type to localized string.
    /// </summary>
    public class TypeToString : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CardType type)
            {
                return type switch
                {
                    CardType.Folder => LocalizedStrings.Instance["FolderText"],
                    CardType.Mod => LocalizedStrings.Instance["ModText"],
                    CardType.Launcher => LocalizedStrings.Instance["LauncherText"],
                    _ => string.Empty,
                };
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// For converting authors array to single string.
    /// </summary>
    public class AuthorsToString : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Author[])
            {
                var list = value as Author[];
                if (list.Length < 1)
                {
                    return LocalizedStrings.Instance["UnknownText"];
                }

                if (list.Length == 1)
                {
                    return list[0].Name;
                }

                return $"{list[0].Name} +{list.Length - 1} {LocalizedStrings.Instance["OthersText"]}";
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
