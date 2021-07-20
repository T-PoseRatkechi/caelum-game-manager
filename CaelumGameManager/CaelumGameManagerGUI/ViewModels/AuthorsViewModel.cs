// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using CaelumCoreLibrary.Common;
    using Caliburn.Micro;

    /// <summary>
    /// Authors VM.
    /// </summary>
    public class AuthorsViewModel : Screen
    {
        private Author _selectedAuthor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorsViewModel"/> class.
        /// </summary>
        public AuthorsViewModel()
        {
            this.AuthorsList = AuthorUtils.AvailableAuthors();
        }

        /// <summary>
        /// Gets or sets the selected author.
        /// </summary>
        public Author SelectedAuthor
        {
            get
            {
                return this._selectedAuthor;
            }

            set
            {
                this._selectedAuthor = value;
                if (this._selectedAuthor != null)
                {
                    this.AuthorDisplay = new AuthorProfileViewModel(this._selectedAuthor);
                    this.NotifyOfPropertyChange(() => this.AuthorDisplay);
                }
            }
        }

        /// <summary>
        /// Gets or sets list of available authors.
        /// </summary>
        public Author[] AuthorsList { get; set; }

        /// <summary>
        /// Gets or sets the author display.
        /// </summary>
        public AuthorProfileViewModel AuthorDisplay { get; set; }
    }
}
