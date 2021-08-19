// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Models
{
    using System;
    using System.Linq;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Decks;
    using Caliburn.Micro;
    using Serilog;

    /// <summary>
    /// Wrapper for <seealso cref="IDeck"/> to keep the bindable collection and deck cards in-sync.
    /// </summary>
    public class BindableDeckModel : BindableCollection<CardModel>
    {
        private readonly IDeck deck;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableDeckModel"/> class.
        /// </summary>
        /// <param name="deck">Deck.</param>
        public BindableDeckModel(IDeck deck)
            : base(deck.Cards)
        {
            this.deck = deck;
        }

        /// <inheritdoc/>
        protected override void InsertItemBase(int index, CardModel item)
        {
            try
            {
                // Adding new card to end of list.
                if (index == this.deck.Cards.Count)
                {
                    this.deck.AddCard(item);
                }

                // DragDrop moving card.
                else
                {
                    this.deck.Cards.Insert(index, item);
                }

                base.InsertItemBase(index, item);
                this.CheckListSync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        private void CheckListSync()
        {
            for (int i = 0, total = this.deck.Cards.Count; i < total; i++)
            {
                if (total != this.Count)
                {
                    Log.Error(
                        "Game deck mismatches bindable deck card count! Game Deck: {GameDeckTotalCards} Bindable Deck: {BindableDeckTotalCards} Restart recommended!",
                        this.deck.Cards.Count,
                        this.Count);
                    break;
                }

                if (this.deck.Cards[i].CardId != this[i].CardId)
                {
                    Log.Error("Game deck and bindable deck are out of sync! Restart recommended!");
                }
            }
        }
    }
}
