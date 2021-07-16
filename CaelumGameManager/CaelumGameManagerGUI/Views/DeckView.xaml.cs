using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaelumGameManagerGUI.Views
{
    /// <summary>
    /// Interaction logic for DeckView.xaml
    /// </summary>
    public partial class DeckView : UserControl
    {
        public DeckView()
        {
            InitializeComponent();
            SetColumnWidth();
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
            Deck.Loaded += (sender, evt) => {
                for (int i = 0, total = Deck.Columns.Count; i < total; i++)
                {
                    // Set min width to Header size (XAML has Column Width = SizeToHeader)
                    Deck.Columns[i].MinWidth = Deck.Columns[i].ActualWidth;

                    // Set first column (Enabled) to disable resize.
                    if (i == 0)
                    {
                        Deck.Columns[i].CanUserResize = false;
                    }

                    // Set column widths to fit cells content.
                    // Set last column to fill remaining space to remove empty column area.
                    Deck.Columns[i].Width = (i != total - 1) ?  new DataGridLength(1, DataGridLengthUnitType.SizeToCells) : new DataGridLength(1, DataGridLengthUnitType.Star);
                }
            };
        }
    }
}
