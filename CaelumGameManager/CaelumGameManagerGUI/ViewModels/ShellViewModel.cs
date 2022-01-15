// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CaelumCoreLibrary.Cards.Converters;
    using CaelumCoreLibrary.Games;
    using CaelumGameManagerGUI.Cards;
    using CaelumGameManagerGUI.Models;
    using CaelumGameManagerGUI.ViewModels.Toolbars;
    using Caliburn.Micro;
    using Serilog;

    /// <summary>
    /// Shell VM.
    /// </summary>
    public class ShellViewModel : Conductor<Screen>
    {
        private IGameInstance currentGame;
        private BindableDeckModel gameDeck;
        private readonly IWindowManager windowManager = new WindowManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="caelumCore">Caelum core.</param>
        /// <param name="cardConverter">Card converter.</param>
        public ShellViewModel(CaelumCore caelumCore, CardConverter cardConverter)
        {
            try
            {
                this.InitGameInstance(caelumCore, cardConverter);
                var rand = new Random();

                if (rand.Next(0, 3) == 0)
                {
                    Log.Information("Shuba shuba shuba!");
                }
                else
                {
                    Log.Information("Caelum Game Manager is ready.");
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to initialize game instance.");
            }
        }

        /// <summary>
        /// Gets or sets the current <seealso cref="IGameInstance"/>.
        /// </summary>
        public IGameInstance CurrentGame
        {
            get { return this.currentGame; }
            set { this.currentGame = value; }
        }

        public ShellToolbarViewModel ShellToolbar { get; set; }

        /// <summary>
        /// Gets the current game's name.
        /// </summary>
        public string GameName { get => this.CurrentGame.GameInstall.GameName; }

        /// <summary>
        /// Gets the <seealso cref="LogViewModel"/>.
        /// </summary>
        public LogViewModel LogVM { get; } = new();

        /// <inheritdoc/>
        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        private void GameDeck_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.currentGame.GameConfig.Settings.Cards = this.gameDeck.Select(x => x.Id).ToArray();
            this.currentGame.GameConfig.SaveGameConfig();
        }

        /// <summary>
        /// Initializes a game instance.
        /// </summary>
        /// <param name="caelumCore">Caelum core.</param>
        /// <param name="cardConverter">Card converter.</param>
        private void InitGameInstance(CaelumCore caelumCore, CardConverter cardConverter)
        {
            this.currentGame = caelumCore.GetGameInstance();
            this.currentGame.InitDeck();

            if (!this.currentGame.GameConfig.Settings.ShowDebugMessages)
            {
                // Clear any earlier debug messages from log window.
                this.LogVM.Log.Clear();

                // Set min level to info.
                App.LogLevelController.MinimumLevel = Serilog.Events.LogEventLevel.Information;
            }

            this.gameDeck = new BindableDeckModel(this.currentGame.Deck.Cards.ToObservableCards());

            this.ActivateItemAsync(new DeckViewModel(this.currentGame, caelumCore.CardFactory, cardConverter, this.gameDeck));
            this.ShellToolbar = new(this.currentGame, this.gameDeck, cardConverter, this.windowManager);
        }
    }
}
