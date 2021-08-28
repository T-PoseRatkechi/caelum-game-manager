// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Base interface for building cards.
    /// </summary>
    public interface IDeckBuilder
    {
        /// <summary>
        /// Builds output from cards in deck.
        /// </summary>
        /// <param name="deck">Deck of cards to build.</param>
        /// <param name="outputDir">Directory to build card at.</param>
        void Build(IList<ICardModel> deck, string outputDir);
    }
}
