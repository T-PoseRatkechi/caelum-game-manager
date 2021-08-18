// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Interface for loading decks.
    /// </summary>
    public interface ICardsLoader
    {
        /// <summary>
        /// Loads cards into a deck.
        /// </summary>
        /// <param name="cardsList">List to add loaded cards into.</param>
        /// <returns>List of installed cards.</returns>
        List<CardModel> GetInstalledCards();
    }
}
