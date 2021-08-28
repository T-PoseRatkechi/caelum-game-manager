// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.Models
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Decks;
    using Caliburn.Micro;

    /// <summary>
    /// Wrapper for <seealso cref="IDeck"/> to keep the bindable collection and deck cards in-sync.
    /// </summary>
    public class BindableDeckModel : BindableCollection<ICardModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindableDeckModel"/> class.
        /// </summary>
        /// <param name="cards">Cards.</param>
        public BindableDeckModel(IEnumerable<ICardModel> cards)
            : base(cards)
        {
        }
    }
}
