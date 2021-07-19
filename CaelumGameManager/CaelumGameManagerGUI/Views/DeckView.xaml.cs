namespace CaelumGameManagerGUI.Views
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

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
                        this.OpenEditCard.Header = "Edit Card";
                    }
                    else
                    {
                        this.OpenEditCard.Header = "Create Card";
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
        /// 4. The last column has their width set to fill remaining space.
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
    /// For converting authors array to single string.
    /// </summary>
    public class ListToString : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string[])
            {
                var list = value as string[];
                if (list.Length < 1)
                {
                    return "Unknown";
                }

                if (list.Length == 1)
                {
                    return list[0];
                }

                return $"{list[0]} +{list.Length - 1} other(s)";
            }

            return "Unknown";
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
