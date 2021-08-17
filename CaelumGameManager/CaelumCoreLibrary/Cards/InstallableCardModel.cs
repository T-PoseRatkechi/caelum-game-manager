﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    /// <summary>
    /// Interface for cards that are installable (has an installation folder somewhere).
    /// </summary>
    public class InstallableCardModel : CardModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the card is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the card is hidden in the deck.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the install path of the card.
        /// </summary>
        public string InstallDirectory { get; set; }
    }
}
