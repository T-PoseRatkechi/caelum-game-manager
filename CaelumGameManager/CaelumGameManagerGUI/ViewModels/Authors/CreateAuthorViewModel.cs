// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels.Authors
{
    using System;
    using System.IO;
    using CaelumCoreLibrary.Common;
    using CaelumCoreLibrary.Utilities;
    using Caliburn.Micro;
    using Microsoft.Win32;

    /// <summary>
    /// Create Author VM.
    /// </summary>
    public class CreateAuthorViewModel : Screen
    {
        // Backing fields.
        private string _name;
        private string _twitterUrl;
        private string _githubUrl;
        private string _donateUrl;
        private string _miscUrl;
        private string _about;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAuthorViewModel"/> class.
        /// </summary>
        public CreateAuthorViewModel()
        {
            NewAuthor = new();
            AuthorDisplay = new(NewAuthor);
        }

        /// <summary>
        /// Gets author profile VM.
        /// </summary>
        public AuthorProfileViewModel AuthorDisplay { get; init; }

        /// <summary>
        /// Gets or sets new author object.
        /// </summary>
        public Author NewAuthor { get; set; }

        /// <summary>
        /// Gets or sets author name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                NewAuthor.Name = value;

                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.Profile);
            }
        }

        /// <summary>
        /// Gets or sets github url.
        /// </summary>
        public string GithubUrl
        {
            get
            {
                return _githubUrl;
            }

            set
            {
                _githubUrl = value;

                if (CaelumUtils.IsValidUrl(value))
                {
                    Uri url = new(value);
                    if (url.Host.Equals("github.com") || url.Host.Equals("www.github.com"))
                    {
                        NewAuthor.GithubUrl = value;
                    }
                    else
                    {
                        NewAuthor.GithubUrl = null;
                    }
                }
                else
                {
                    NewAuthor.GithubUrl = null;
                }

                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.Profile);
                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.CanGithubLink);
            }
        }

        /// <summary>
        /// Gets or sets twitter url.
        /// </summary>
        public string TwitterUrl
        {
            get
            {
                return _twitterUrl;
            }

            set
            {
                _twitterUrl = value;

                if (CaelumUtils.IsValidUrl(value))
                {
                    Uri url = new(value);
                    if (url.Host.Equals("twitter.com") || url.Host.Equals("www.twitter.com"))
                    {
                        NewAuthor.TwitterUrl = value;
                    }
                    else
                    {
                        NewAuthor.TwitterUrl = null;
                    }
                }
                else
                {
                    NewAuthor.TwitterUrl = null;
                }

                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.Profile);
                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.CanTwitterLink);
            }
        }

        /// <summary>
        /// Gets or sets the donate url.
        /// </summary>
        public string DonateUrl
        {
            get
            {
                return _donateUrl;
            }

            set
            {
                _donateUrl = value;

                if (CaelumUtils.IsValidUrl(value))
                {
                    NewAuthor.DonationUrl = value;
                }
                else
                {
                    NewAuthor.DonationUrl = null;
                }

                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.Profile);
                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.CanDonateLink);
            }
        }

        /// <summary>
        /// Gets or sets misc. url.
        /// </summary>
        public string MiscUrl
        {
            get
            {
                return _miscUrl;
            }

            set
            {
                _miscUrl = value;

                if (CaelumUtils.IsValidUrl(value))
                {
                    NewAuthor.MiscUrl = value;
                }
                else
                {
                    NewAuthor.MiscUrl = null;
                }

                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.Profile);
                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.CanOtherLink);
            }
        }

        /// <summary>
        /// Gets or sets author about text.
        /// </summary>
        public string About
        {
            get
            {
                return _about;
            }

            set
            {
                _about = value;
                NewAuthor.About = value;
                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.Profile);
            }
        }

        /// <summary>
        /// Opens a file select to select the author avatar image.
        /// </summary>
        public void SelectAvatar()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Images files|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                var imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                NewAuthor.AvatarBytes = imageBytes;
                AuthorDisplay.NotifyOfPropertyChange(() => AuthorDisplay.AuthorAvatar);
            }
        }

        /// <summary>
        /// Writes author to author file.
        /// </summary>
        public void CreateButton()
        {
            AuthorUtils.WriteAuthor(NewAuthor);
        }
    }
}
