﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using CaelumCoreLibrary.Common;
    using CaelumCoreLibrary.Utilities;
    using Caliburn.Micro;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAuthorViewModel"/> class.
        /// </summary>
        public CreateAuthorViewModel()
        {
            this.NewAuthor = new();
            this.AuthorDisplay = new(this.NewAuthor);
        }

        public Author NewAuthor { get; set; }

        /// <summary>
        /// Gets or sets author name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
                this.NewAuthor.Name = value;

                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.Profile);
            }
        }


        /// <summary>
        /// Gets or sets github url.
        /// </summary>
        public string GithubUrl
        {
            get
            {
                return this._githubUrl;
            }

            set
            {
                this._githubUrl = value;

                if (CaelumUtils.IsValidUrl(value))
                {
                    Uri url = new(value);
                    if (url.Host.Equals("github.com") || url.Host.Equals("www.github.com"))
                    {
                        this.NewAuthor.GithubUrl = value;
                    }
                    else
                    {
                        this.NewAuthor.GithubUrl = null;
                    }
                }
                else
                {
                    this.NewAuthor.GithubUrl = null;
                }

                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.Profile);
                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.CanGithubLink);
            }
        }

        /// <summary>
        /// Gets or sets twitter url.
        /// </summary>
        public string TwitterUrl
        {
            get
            {
                return this._twitterUrl;
            }

            set
            {
                this._twitterUrl = value;

                if (CaelumUtils.IsValidUrl(value))
                {
                    Uri url = new(value);
                    if (url.Host.Equals("twitter.com") || url.Host.Equals("www.twitter.com"))
                    {
                        this.NewAuthor.TwitterUrl = value;
                    }
                    else
                    {
                        this.NewAuthor.TwitterUrl = null;
                    }
                }
                else
                {
                    this.NewAuthor.TwitterUrl = null;
                }

                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.Profile);
                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.CanTwitterLink);
            }
        }

        /// <summary>
        /// Gets or sets the donate url.
        /// </summary>
        public string DonateUrl
        {
            get
            {
                return this._donateUrl;
            }

            set
            {
                this._donateUrl = value;

                if (CaelumUtils.IsValidUrl(value))
                {
                    this.NewAuthor.DonationUrl = value;
                }
                else
                {
                    this.NewAuthor.DonationUrl = null;
                }

                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.Profile);
                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.CanDonateLink);
            }
        }

        /// <summary>
        /// Gets or sets misc. url.
        /// </summary>
        public string MiscUrl
        {
            get
            {
                return this._miscUrl;
            }

            set
            {
                this._miscUrl = value;

                if (CaelumUtils.IsValidUrl(value))
                {
                    this.NewAuthor.MiscUrl = value;
                }
                else
                {
                    this.NewAuthor.MiscUrl = null;
                }

                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.Profile);
                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.CanOtherLink);
            }
        }

        /// <summary>
        /// Gets or sets author about text.
        /// </summary>
        private string _about;

        public string About
        {
            get
            {
                return this._about;
            }

            set
            {
                this._about = value;
                this.NewAuthor.About = value;
                this.AuthorDisplay.NotifyOfPropertyChange(() => this.AuthorDisplay.Profile);
            }
        }


        /// <summary>
        /// Gets or sets author display.
        /// </summary>
        public AuthorProfileViewModel AuthorDisplay { get; set; }
    }
}
