// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;

    /// <summary>
    /// ObservableCardModel implementation.
    /// </summary>
    public class ObservableCardModel : Caliburn.Micro.PropertyChangedBase, ICardModel
    {
        public ObservableCardModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCardModel"/> class.
        /// </summary>
        /// <param name="card">Source card.</param>
        public ObservableCardModel(ICardModel card)
        {
            this.CardId = card.CardId;
            this.IsEnabled = card.IsEnabled;
            this.IsHidden = card.IsHidden;
            this.Name = card.Name;
            this.Games = card.Games;
            this.Authors = card.Authors;
            this.Description = card.Description;
            this.Version = card.Version;
            this.Type = card.Type;
            this.InstallDirectory = card.InstallDirectory;
        }

        private string _cardId;

        /// <inheritdoc/>
        public string CardId
        {
            get
            {
                return this._cardId;
            }

            set
            {
                this._cardId = value;
                this.NotifyOfPropertyChange(() => this.IsEnabled);
            }
        }

        private bool _isEnabled;

        /// <inheritdoc/>
        public bool IsEnabled
        {
            get
            {
                return this._isEnabled;
            }

            set
            {
                this._isEnabled = value;
                this.NotifyOfPropertyChange(() => this.IsEnabled);
            }
        }

        /// <inheritdoc/>
        public bool IsHidden { get; set; }

        private string _name;

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
                this.NotifyOfPropertyChange(() => this.Name);
            }
        }

        /// <inheritdoc/>
        public string[] Games { get; set; }

        /// <inheritdoc/>
        public List<Author> Authors { get; set; }

        /// <inheritdoc/>
        public string Description { get; set; }

        /// <inheritdoc/>
        public string Version { get; set; }

        /// <inheritdoc/>
        public CardType Type { get; set; }

        /// <inheritdoc/>
        public string InstallDirectory { get; set; }
    }
}
