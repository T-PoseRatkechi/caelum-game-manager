// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Views.Cards
{
    using CaelumCoreLibrary.Common;
    using Caliburn.Micro;
    using System.Dynamic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for CardView.xaml.
    /// </summary>
    public partial class CardView : UserControl
    {
        ViewModels.Authors.AuthorProfileViewModel popupAuthor = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardView"/> class.
        /// </summary>
        public CardView()
        {
            this.InitializeComponent();
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock item = e.OriginalSource as TextBlock;
            WindowManager windowManager = new();
            this.popupAuthor = new(item.DataContext as Author);
            dynamic settings = new ExpandoObject();
            settings.Width = 400;
            settings.Height = 500;
            settings.Placement = PlacementMode.Mouse;

            windowManager.ShowPopupAsync(this.popupAuthor, null, settings);
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.popupAuthor != null)
            {
                this.popupAuthor.TryCloseAsync();
                this.popupAuthor = null;
            }
        }
    }
}
