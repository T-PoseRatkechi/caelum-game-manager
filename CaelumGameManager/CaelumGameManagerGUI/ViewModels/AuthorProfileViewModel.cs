// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using CaelumCoreLibrary.Common;

    /// <summary>
    /// Author Profile VM.
    /// </summary>
    public class AuthorProfileViewModel
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorProfileViewModel"/> class.
        /// </summary>
        /// <param name="author">Author to display.</param>
        public AuthorProfileViewModel(Author author)
        {
            this.AuthorProfile = author;
        }

        /// <summary>
        /// Gets or sets the author's profile.
        /// </summary>
        public Author AuthorProfile { get; set; }

        /// <summary>
        /// Gets the author's avatar.
        /// </summary>
        public ImageSource AuthorAvatar
        {
            get
            {
                using (MemoryStream ms = new(this.AuthorProfile.AvatarBytes))
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
        }

        public bool CanTwitterLink()
        {
            return !string.IsNullOrEmpty(this.AuthorProfile.TwitterUrl);
        }

        public void TwitterLink()
        {
            var open = new ProcessStartInfo()
            {
                FileName = this.AuthorProfile.TwitterUrl,
                UseShellExecute = true,
            };

            Process.Start(open);
        }

        public bool CanGithubLink()
        {
            return !string.IsNullOrEmpty(this.AuthorProfile.GithubUrl);
        }

        public void GithubLink()
        {
            var open = new ProcessStartInfo()
            {
                FileName = this.AuthorProfile.GithubUrl,
                UseShellExecute = true,
            };

            Process.Start(open);
        }

        public bool CanDonateLink()
        {
            return !string.IsNullOrEmpty(this.AuthorProfile.DonationUrl);
        }

        public void DonateLink()
        {
            var open = new ProcessStartInfo()
            {
                FileName = this.AuthorProfile.DonationUrl,
                UseShellExecute = true,
            };

            Process.Start(open);
        }

        public bool CanOtherLink()
        {
            return !string.IsNullOrEmpty(this.AuthorProfile.MiscUrl);
        }

        public void OtherLink()
        {
            var open = new ProcessStartInfo()
            {
                FileName = this.AuthorProfile.MiscUrl,
                UseShellExecute = true,
            };

            Process.Start(open);
        }
    }
}
