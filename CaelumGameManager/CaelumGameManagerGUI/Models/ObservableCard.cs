// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using Caliburn.Micro;
    using Newtonsoft.Json;

    public class ObservableCard : PropertyChangedBase, ICardModel
    {
        protected ICardModel _card;

        public ObservableCard(ICardModel card)
        {
            this._card = card;

            this.PropertyChanged += this.SaveCardChanges;
        }

        /// <inheritdoc/>
        public string CardId
        {
            get
            {
                return this._card.CardId;
            }

            set
            {
                this._card.CardId = value;
                this.NotifyOfPropertyChange(() => this.CardId);
            }
        }

        /// <inheritdoc/>
        public bool IsEnabled
        {
            get
            {
                if (this.Type == CardType.None)
                {
                    return false;
                }

                return this._card.IsEnabled;
            }

            set
            {
                this._card.IsEnabled = value;
                this.NotifyOfPropertyChange(() => this.IsEnabled);
            }
        }

        /// <inheritdoc/>
        public bool IsHidden
        {
            get
            {
                return this._card.IsHidden;
            }

            set
            {
                this._card.IsHidden = value;
                this.NotifyOfPropertyChange(() => this.IsHidden);
            }
        }

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                return this._card.Name;
            }

            set
            {
                this._card.Name = value;
                this.NotifyOfPropertyChange(() => this.Name);
            }
        }

        /// <inheritdoc/>
        public List<string> Games { get; set; }

        /// <inheritdoc/>
        public List<Author> Authors { get; set; }

        /// <inheritdoc/>
        public string Description
        {
            get
            {
                return this._card.Description;
            }

            set
            {
                this._card.Description = value;
                this.NotifyOfPropertyChange(() => this.Description);
            }
        }

        /// <inheritdoc/>
        public string Version
        {
            get
            {
                return this._card.Version;
            }

            set
            {
                this._card.Version = value;
                this.NotifyOfPropertyChange(() => this.Version);
            }
        }

        /// <inheritdoc/>
        public CardType Type
        {
            get
            {
                return this._card.Type;
            }

            set
            {
                this._card.Type = value;
                this.NotifyOfPropertyChange(() => this.Type);
            }
        }

        /// <inheritdoc/>
        public string InstallDirectory
        {
            get
            {
                return this._card.InstallDirectory;
            }

            set
            {
                this._card.InstallDirectory = value;
            }
        }

        protected void SaveCardChanges(object sender, PropertyChangedEventArgs e)
        {
            var cardText = JsonConvert.SerializeObject(this._card, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            File.WriteAllText(Path.Join(this._card.InstallDirectory, "card.json"), cardText);
        }
    }
}
