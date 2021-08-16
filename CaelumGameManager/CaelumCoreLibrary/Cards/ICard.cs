﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Common;

    /// <summary>
    /// Base interface for all cards.
    /// </summary>
    public interface ICard
    {
        /// <summary>
        /// Gets or sets card ID. Must be unique with the deck with only Update Card being the exception.
        /// Should typically be: {author_name}-{card_name}
        /// Update Cards must have the same id as the card they are updating.
        /// </summary>
        string CardId { get; set; }

        /// <summary>
        /// Gets or sets name of card.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets games card is for.
        /// </summary>
        string[] Games { get; set; }

        /// <summary>
        /// Gets or sets authors of the card.
        /// </summary>
        List<Author> Authors { get; set; }

        /// <summary>
        /// Gets or sets description of card.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets version of card.
        /// </summary>
        string Version { get; set; }

        /// <summary>
        /// Gets or sets card type.
        /// </summary>
        CardType Type { get; set; }
    }
}
