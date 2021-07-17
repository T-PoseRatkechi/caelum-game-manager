namespace CaelumGameManagerGUI.Views
{
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for DeckView.xaml
    /// </summary>
    public partial class DeckView : UserControl
    {

        public DeckView()
        {
            InitializeComponent();
            SetColumnWidth();

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
            FilteredDeck.Loaded += (sender, evt) =>
            {
                for (int i = 0, total = FilteredDeck.Columns.Count; i < total; i++)
                {
                    // Set min width to Header size (XAML has Column Width = SizeToHeader)
                    FilteredDeck.Columns[i].MinWidth = FilteredDeck.Columns[i].ActualWidth;

                    // Set first column (Enabled) to disable resize.
                    if (i == 0)
                    {
                        FilteredDeck.Columns[i].CanUserResize = false;
                    }

                    // Set column widths to fit cells content.
                    // Set last column to fill remaining space to remove empty column area.
                    FilteredDeck.Columns[i].Width = (i != total - 1) ? new DataGridLength(1, DataGridLengthUnitType.SizeToCells) : new DataGridLength(1, DataGridLengthUnitType.Star);
                }
            };
        }

        /// <summary>
        /// Diselect any items when mouse clicks in empty space.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
}
