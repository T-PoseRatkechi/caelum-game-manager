﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Default <seealso cref="IDeckFactory"/> implementation.
    /// </summary>
    public class DeckFactory : IDeckFactory
    {
        /// <inheritdoc/>
        public IDeck CreateDeck(ICardsLoader cardsLoader)
        {
            return new Deck(cardsLoader);
        }
    }
}
