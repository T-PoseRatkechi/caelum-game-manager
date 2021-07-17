﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Deck instance.
    /// </summary>
    public class Deck : IDeck
    {
        /// <inheritdoc/>
        public string Game { get; set; }

        /// <inheritdoc/>
        public string Format { get; set; } = "1.0.0";

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public ICard[] Cards { get; set; }
    }
}
