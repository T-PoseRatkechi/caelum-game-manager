// Copyright (c) T-Pose Ratkechi. All rights reserved.
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
    using CaelumGameManagerGUI.Models;
    using Caliburn.Micro;
    using Serilog;

    /// <summary>
    /// Shell VM.
    /// </summary>
    public class ShellViewModel : Conductor<Screen>
    {
        private ILogger logger = Serilog.Log.Logger;
        private IGameInstance _currentGame;
        private ICardFactory _cardFactory;

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
                this._cardFactory = scope.Resolve<ICardFactory>();
            }

            App.loggingLevelSwitch.MinimumLevel = this._currentGame.GameConfig.Settings.ShowDebugMessages ? Serilog.Events.LogEventLevel.Information : Serilog.Events.LogEventLevel.Debug;
            Log.Verbose("Test");
            this.gameDeck = new BindableDeckModel(this.CurrentGame.Deck, this.CurrentGame.GameConfig.Settings.ShowDebugMessages);

            this.ActivateItemAsync(new DeckViewModel(this.CurrentGame, this._cardFactory, this.gameDeck));

            try
            {
                this.CurrentGame.BuildDeck();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Gets or sets the current <seealso cref="IGameInstance"/>.
        /// </summary>
        public IGameInstance CurrentGame
        {
            get { return this._currentGame; }
            set { this._currentGame = value; }
        }

        /// <summary>
        /// Gets the current game's name.
        /// </summary>
        public string GameName { get => this.CurrentGame.GameInstall.GameName; }

        /// <summary>
        /// Gets the <seealso cref="LogViewModel"/>.
        /// </summary>
        public LogViewModel LogVM { get; } = new();


        /// <inheritdoc/>
        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            this.logger.Information("Caelum Game Manager ready");
        }

        /// <inheritdoc/>
        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            return base.OnDeactivateAsync(close, cancellationToken);
        }
    }
}
