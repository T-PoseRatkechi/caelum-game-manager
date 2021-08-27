// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.Models
{
    using System;
    using System.Linq;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Decks;
    using CaelumGameManagerGUI.Resources.Localization;
    using Caliburn.Micro;
    using Serilog;

    /// <summary>
    /// Wrapper for <seealso cref="IDeck"/> to keep the bindable collection and deck cards in-sync.
    /// </summary>
    public class BindableDeckModel : BindableCollection<ObservableCardModel>
    {
        private readonly IDeck _deck;

        /// <summary>
        /// Flag indiciating whether to check if deck cards and BindableDeck list are in sync.
        /// Only enable if app confg debug is enabled.
        /// </summary>
        private readonly bool _checkSyncStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableDeckModel"/> class.
        /// </summary>
        /// <param name="deck">Deck.</param>
        /// <param name="checkSyncStatus">Flag indicating whether to check if lists are in-sync.</param>
        public BindableDeckModel(IDeck deck, bool checkSyncStatus)
            : base(deck.Cards.Select(x => new ObservableCardModel(x)))
        {
            this._deck = deck;
            this._checkSyncStatus = checkSyncStatus;

            this.CollectionChanged += (sender, e) =>
            {
                Log.Information("Item change");
            };
        }

        /// <inheritdoc/>
        protected override void InsertItemBase(int index, ObservableCardModel item)
        {
            try
            {
                this._deck.Cards.Insert(index, item);
                this._deck.NotifyDeckChanged();

                base.InsertItemBase(index, item);

                if (this._checkSyncStatus)
                {
                    this.CheckListSync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Iterates over GameInstance's deck cards and this BindableDeck's cards are in the same order.
        /// If not, logs an error and recommends a restart.
        /// </summary>
        private void CheckListSync()
        {
            for (int i = 0, total = this._deck.Cards.Count; i < total; i++)
            {
                if (total != this.Count)
                {
                    Log.Error(
                        "Game deck mismatches bindable deck card count! Game Deck: {GameDeckTotalCards} Bindable Deck: {BindableDeckTotalCards}",
                        this._deck.Cards.Count,
                        this.Count);
                    Log.Error(LocalizedStrings.Instance["ErrorRecommendRestartMessage"]);
                    break;
                }

                if (this._deck.Cards[i].CardId != this[i].CardId)
                {
                    Log.Error("Game deck and bindable deck are out of sync!");
                    Log.Error(LocalizedStrings.Instance["ErrorRecommendRestartMessage"]);
                    break;
                }
            }
        }
    }
}
