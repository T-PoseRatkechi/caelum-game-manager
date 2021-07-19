// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;

    /// <summary>
    /// Game interface for managing deck and card creation.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets game name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets game deck.
        /// </summary>
        public List<ICard> Deck { get; set; }

        /// <summary>
        /// Handles creating a new card with the given name and <seealso cref="CardType"/>.
        /// </summary>
        /// <param name="id">Card ID.</param>
        /// <param name="name">Name of card.</param>
        /// <param name="type">Card type.</param>
        /// <param name="authors">Authors of card.</param>
        /// <param name="description">Description of card.</param>
        /// <param name="version">Card version.</param>
        /// <returns>New <seealso cref="ICard"/> instance.</returns>
        public ICard CreateCard(string id, string name, CardType type, Author[] authors = null, string description = null, string version = null);

        public void WriteConfig();
    }
}
