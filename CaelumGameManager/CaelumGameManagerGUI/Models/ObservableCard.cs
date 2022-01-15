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
        /// <summary>
        /// Original card.
        /// </summary>
        protected ICardModel _card;

        public ObservableCard(ICardModel card)
        {
            this._card = card;

            this.PropertyChanged += this.SaveCardChanges;
        }

        /// <inheritdoc/>
        public CardMetadataModel Metadata
        {
            get
            {
                return this._card.Metadata;
            }

            set
            {
                this._card.Metadata = value;
                this.NotifyOfPropertyChange(() => this._card.Metadata);
            }
        }

        /// <inheritdoc/>
        public string Id
        {
            get
            {
                return this._card.Id;
            }

            set
            {
                this._card.Id = value;
                this.NotifyOfPropertyChange(() => this.Id);
            }
        }

        /// <inheritdoc/>
        public bool Enabled
        {
            get
            {
                if (this._card.Metadata.Type == CardType.None)
                {
                    return false;
                }

                return this._card.Enabled;
            }

            set
            {
                this._card.Enabled = value;
                this.NotifyOfPropertyChange(() => this.Enabled);
            }
        }

        /// <inheritdoc/>
        public bool Hidden
        {
            get
            {
                return this._card.Hidden;
            }

            set
            {
                this._card.Hidden = value;
                this.NotifyOfPropertyChange(() => this.Hidden);
            }
        }

        /// <inheritdoc/>
        public string InstallFolder
        {
            get
            {
                return this._card.InstallFolder;
            }

            set
            {
                this._card.InstallFolder = value;
            }
        }

        /// <summary>
        /// Saves card changes to file.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event.</param>
        protected void SaveCardChanges(object sender, PropertyChangedEventArgs e)
        {
            var cardText = JsonConvert.SerializeObject(this._card, new JsonSerializerSettings() { Formatting = Formatting.Indented });
            File.WriteAllText(Path.Join(this._card.InstallFolder, "card.json"), cardText);
        }
    }
}
