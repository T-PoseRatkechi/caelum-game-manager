// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Common
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Author object.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Gets or sets author's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets author's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets author's twitter url.
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// Gets or sets author's Github url.
        /// </summary>

        public string GithubUrl { get; set; }

        /// <summary>
        /// Gets or sets the author's donation url.
        /// </summary>

        public string DonationUrl { get; set; }

        /// <summary>
        /// Gets or sets the author's misc. url.
        /// </summary>

        public string MiscUrl { get; set; }

        /// <summary>
        /// Gets or sets the author's about me text.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Gets or sets the author's avatar bytes.
        /// </summary>
        [JsonIgnore]
        public byte[] AvatarBytes { get; set; } = Array.Empty<byte>();
    }
}
