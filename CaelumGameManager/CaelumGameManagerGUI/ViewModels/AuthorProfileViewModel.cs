// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using CaelumCoreLibrary.Common;
    using CaelumGameManagerGUI.Models;
    using Caliburn.Micro;

    /// <summary>
    /// Author Profile VM.
    /// </summary>
    public class AuthorProfileViewModel : Screen
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorProfileViewModel"/> class.
        /// </summary>
        /// <param name="author">Author to display.</param>
        public AuthorProfileViewModel(Author author)
        {
            this.Profile = author;
        }

        /// <summary>
        /// Gets or sets the author's profile.
        /// </summary>
        public Author Profile { get; set; }

        /// <summary>
        /// Gets the author's avatar.
        /// </summary>
        public ImageSource AuthorAvatar
        {
            get
            {
                if (this.Profile?.AvatarBytes?.Length > 0)
                {
                    using (MemoryStream ms = new(this.Profile.AvatarBytes))
                    {
                        BitmapImage image = new();
                        ms.Position = 0;
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                        return image;
                    }
                }

                return null;
            }
        }

        public bool CanTwitterLink
        {
            get
            {
                return !string.IsNullOrEmpty(this.Profile.TwitterUrl);
            }
        }

        public void TwitterLink()
        {
            var open = new ProcessStartInfo()
            {
                FileName = this.Profile.TwitterUrl,
                UseShellExecute = true,
            };

            Process.Start(open);
        }

        public bool CanGithubLink
        {
            get
            {
                return !string.IsNullOrEmpty(this.Profile.GithubUrl);
            }
        }

        public void GithubLink()
        {
            var open = new ProcessStartInfo()
            {
                FileName = this.Profile.GithubUrl,
                UseShellExecute = true,
            };

            Process.Start(open);
        }

        public bool CanDonateLink
        {
            get
            {
                return !string.IsNullOrEmpty(this.Profile.DonationUrl);
            }
        }

        public void DonateLink()
        {
            var open = new ProcessStartInfo()
            {
                FileName = this.Profile.DonationUrl,
                UseShellExecute = true,
            };

            Process.Start(open);
        }

        public bool CanOtherLink
        {
            get
            {
                return !string.IsNullOrEmpty(this.Profile.MiscUrl);
            }
        }

        public void OtherLink()
        {
            var open = new ProcessStartInfo()
            {
                FileName = this.Profile.MiscUrl,
                UseShellExecute = true,
            };

            Process.Start(open);
        }
    }
}
