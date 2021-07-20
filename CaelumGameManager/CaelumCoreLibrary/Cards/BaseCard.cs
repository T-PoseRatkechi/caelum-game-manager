// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using CaelumCoreLibrary.Common;

    /// <summary>
    /// Base card implementation.
    /// </summary>
    public abstract class BaseCard : ICard
    {
        /// <inheritdoc/>
        public string Game { get; set; }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public List<Author> Authors { get; set; }

        /// <inheritdoc/>
        public string Description { get; set; }

        /// <inheritdoc/>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the card's type.
        /// </summary>

        public CardType Type { get; set; }

        /// <summary>
        /// Gets or sets card's data path.
        /// </summary>
        [JsonIgnore]
        public string Path { get; set; }
    }
}
