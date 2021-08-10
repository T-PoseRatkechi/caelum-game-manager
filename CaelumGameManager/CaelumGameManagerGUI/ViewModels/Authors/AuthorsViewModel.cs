// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels.Authors
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Common;
    using CaelumGameManagerGUI.ViewModels;
    using Caliburn.Micro;

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
            AuthorsList = AuthorUtils.AvailableAuthors();
            AuthorDisplay = new(_selectedAuthor);
            this.cardAuthors = cardAuthors;
        }

        /// <summary>
        /// Gets or sets the selected author.
        /// </summary>
        public Author SelectedAuthor
        {
            get
            {
                return _selectedAuthor;
            }

            set
            {
                _selectedAuthor = value;
                if (_selectedAuthor != null)
                {
                    AuthorDisplay = new AuthorProfileViewModel(_selectedAuthor);
                    NotifyOfPropertyChange(() => AuthorDisplay);
                }

                NotifyOfPropertyChange(() => CanAddAuthor);
                NotifyOfPropertyChange(() => AuthorDescription);
            }
        }

        /// <summary>
        /// Gets a value indicating whether add author button is enabled.
        /// </summary>
        public bool CanAddAuthor
        {
            get
            {
                return SelectedAuthor != null && cardAuthors != null && cardAuthors.FindIndex(a => a.Name.Equals(SelectedAuthor.Name)) == -1;
            }
        }

        public string AuthorDescription
        {
            get
            {
                if (SelectedAuthor != null)
                {
                    return SelectedAuthor.Description;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (SelectedAuthor != null)
                {
                    SelectedAuthor.Description = value;
                }
            }
        }

        /// <summary>
        /// Adds selected author to card.
        /// </summary>
        public void AddAuthor()
        {
            cardAuthors.Add(SelectedAuthor);
            NotifyOfPropertyChange(() => CanAddAuthor);
        }

        /// <summary>
        /// Opens Create Author VM.
        /// </summary>
        public void CreateAuthor()
        {
            windowManager.ShowDialogAsync(new CreateAuthorViewModel());
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
