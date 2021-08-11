// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Games;
    using Caliburn.Micro;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Shell VM.
    /// </summary>
    public class ShellViewModel : Conductor<Screen>
    {
        private IGame _currentGame = new GameP4G();
        private BindableCollection<ICard> gameDeck;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        public ShellViewModel()
        {
            this.gameDeck = new BindableCollection<ICard>(this.CurrentGame.Deck);
            this.gameDeck.CollectionChanged += this.OnDeckChange;

            this.ActivateItemAsync(new DeckViewModel(this.CurrentGame, this.gameDeck));
        }

        private void OnDeckChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            this.CurrentGame.Deck = new List<ICard>(this.gameDeck);
            this.CurrentGame.WriteConfig();
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public IGame CurrentGame
        {
            get { return _currentGame; }
            set { _currentGame = value; }
        }

        public LogViewModel Log { get; } = new();
    }
}
