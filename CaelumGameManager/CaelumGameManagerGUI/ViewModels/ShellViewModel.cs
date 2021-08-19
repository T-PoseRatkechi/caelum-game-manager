﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using System.Collections.Specialized;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using CaelumCoreLibrary.Builders;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Games;
    using Caliburn.Micro;
    using Serilog;

    /// <summary>
    /// Shell VM.
    /// </summary>
    public class ShellViewModel : Conductor<Screen>
    {
        private ILogger logger = Serilog.Log.Logger;
        private IGameInstance _currentGame;

        private BindableCollection<CardModel> gameDeck;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        public ShellViewModel()
        {
            this.logger.Information("Caelum Game Manager starting");

            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var gameInstanceFactory = scope.Resolve<IGameInstanceFactory>();
                var deckBuilderBasic = scope.Resolve<IDeckBuilder>();

                this._currentGame = gameInstanceFactory.CreateGameInstance("Persona 4 Golden", deckBuilderBasic);
            }

            this.gameDeck = new BindableCollection<CardModel>(this.CurrentGame.Deck.Cards);
            this.gameDeck.CollectionChanged += this.OnDeckChange;

            this.ActivateItemAsync(new DeckViewModel(this.CurrentGame, this.gameDeck));

            try
            {
                this.CurrentGame.BuildDeck();
            }
            catch (Exception e)
            {

            }
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            this.logger.Information("Caelum Game Manager ready");
        }

        private void OnDeckChange(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            // this.CurrentGame.Deck = new List<ICard>(this.gameDeck);
            // this.CurrentGame.WriteConfig();
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public IGameInstance CurrentGame
        {
            get { return _currentGame; }
            set { _currentGame = value; }
        }

        public string GameName { get => this.CurrentGame.GameInstall.GameName; }

        public LogViewModel Log { get; } = new();
    }
}
