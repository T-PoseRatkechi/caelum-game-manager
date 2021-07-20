// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels
{
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using CaelumGameManagerGUI.Views;
    using Caliburn.Micro;
    using System.Collections.Generic;

    /// <summary>
    /// Authors VM.
    /// </summary>
    public class AuthorsViewModel : Screen
    {
        private WindowManager windowManager = new();
        private List<Author> cardAuthors;

        private Author _selectedAuthor = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorsViewModel"/> class.
        /// </summary>
        public AuthorsViewModel(List<Author> cardAuthors = null)
        {
            this.AuthorsList = AuthorUtils.AvailableAuthors();
            this.AuthorDisplay = new(this._selectedAuthor);
            this.cardAuthors = cardAuthors;
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

                this.NotifyOfPropertyChange(() => this.CanAddAuthor);
            }
        }

        /// <summary>
        /// Gets a value indicating whether add author button is enabled.
        /// </summary>
        public bool CanAddAuthor
        {
            get
            {
                return this.SelectedAuthor != null && this.cardAuthors != null;
            }
        }

        /// <summary>
        /// Adds selected author to card.
        /// </summary>
        public void AddAuthor()
        {
            this.cardAuthors.Add(this.SelectedAuthor);
        }

        /// <summary>
        /// Opens Create Author VM.
        /// </summary>
        public void CreateAuthor()
        {
            this.windowManager.ShowDialogAsync(new CreateAuthorViewModel());
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
