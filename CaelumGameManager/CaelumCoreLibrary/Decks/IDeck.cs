// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Deck interface.
    /// </summary>
    public interface IDeck
    {
        /// <summary>
        /// Gets or sets the game the deck is for.
        /// </summary>
        public string Game { get; set; }

        /// <summary>
        /// Gets or sets the game the deck's format version.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the deck's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the deck's cards.
        /// </summary>
        public ICard[] Cards { get; set; }
    }
}
