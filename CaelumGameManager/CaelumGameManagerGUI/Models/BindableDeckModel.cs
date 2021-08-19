// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Decks;
    using Caliburn.Micro;
    using Serilog;

    public class BindableDeckModel : BindableCollection<CardModel>
    {
        private readonly IDeck deck;

        public BindableDeckModel(IDeck deck)
            : base(deck.Cards)
        {
            this.deck = deck;
        }

        protected override void InsertItemBase(int index, CardModel item)
        {
            try
            {
                this.deck.AddCard(item);
                base.InsertItemBase(index, item);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
