// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    /// <summary>
    /// DeckFactory interface.
    /// </summary>
    public interface IDeckFactory
    {
        /// <summary>
        /// Creates a new <seealso cref="IDeck"/> instance.
        /// </summary>
        /// <param name="cardsLoader"><seealso cref="ICardsLoader"/> to use for loading inital deck cards.</param>
        /// <returns>New <seealso cref="IDeck"/> instance.</returns>
        IDeck CreateDeck(ICardsLoader cardsLoader);
    }
}
