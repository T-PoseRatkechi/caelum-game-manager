// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System;

    /// <summary>
    /// Interface that all package types implement.
    /// </summary>
    public interface ICard
    {
        /// <summary>
        /// Gets or sets a value indicating whether the card is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets card id that must be unique in the given game deck. Only exception are Update Cards.
        /// Should typically be: {author_name}.{card_name}
        /// Update Cards must have the same id as the card they are updating.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets name of card.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets name(s) of authors that created the card.
        /// </summary>
        public string[] Authors { get; set; }

        /// <summary>
        /// Gets or sets name of game this card is for.
        /// </summary>
        public string Game { get; set; }

        /// <summary>
        /// Gets or sets version of card.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets card data, such as card type and path to data.
        /// </summary>
        public CardData Data { get; }
    }
}
