// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

using CaelumCoreLibrary.Common;
using System.Collections.Generic;

namespace CaelumCoreLibrary.Cards
{
    /// <summary>
    /// Interface for cards that are installable (has an installation folder somewhere).
    /// </summary>
    public class CardModel
    {
        /// <summary>
        /// Gets or sets card ID. Must be unique with the deck with only Update Card being the exception.
        /// Should typically be: {author_name}-{card_name}
        /// Update Cards must have the same id as the card they are updating.
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the card is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the card is hidden in the deck.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets name of card.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets games card is for.
        /// </summary>
        public string[] Games { get; set; }

        /// <summary>
        /// Gets or sets authors of the card.
        /// </summary>
        public List<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets description of card.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets version of card.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets card type.
        /// </summary>
        public CardType Type { get; set; }

        /// <summary>
        /// Gets or sets the install path of the card.
        /// </summary>
        public string InstallDirectory { get; set; }
    }
}
