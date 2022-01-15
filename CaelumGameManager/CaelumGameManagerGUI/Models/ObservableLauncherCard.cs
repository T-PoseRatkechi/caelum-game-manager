// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Models
{
    using System;
    using System.Diagnostics;
    using CaelumCoreLibrary.Cards;

    public class ObservableLauncherCard : ObservableCard, ILauncherCardModel
    {
        private ILauncherCardModel _cardLauncher;

        public ObservableLauncherCard(ICardModel card)
            : base(card)
        {
            if (card.Metadata.Type != CardType.Launcher)
            {
                throw new ArgumentException("Card is a not a game launcher.");
            }

            this._cardLauncher = card as ILauncherCardModel;
        }

        public string LauncherArgs
        {
            get
            {
                return this._cardLauncher.LauncherArgs;
            }

            set
            {
                this._cardLauncher.LauncherArgs = value;
                this.NotifyOfPropertyChange(() => this.LauncherArgs);
            }
        }

        public string LauncherPath
        {
            get
            {
                return this._cardLauncher.LauncherPath;
            }

            set
            {
                this._cardLauncher.LauncherPath = value;
                this.NotifyOfPropertyChange(() => this.LauncherPath);
            }
        }

        public Process Start(string gamePath)
        {
            return this._cardLauncher.Start(gamePath);
        }
    }
}
