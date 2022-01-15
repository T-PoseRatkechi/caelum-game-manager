// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using CaelumGameManagerGUI.Resources.Localization;
    using Caliburn.Micro;
    using Newtonsoft.Json;

    /// <summary>
    /// ObservableCardModel implementation.
    /// </summary>
    public class ObservableCardModel : PropertyChangedBase, ICardModel
    {
        private CardMetadataModel _metadata;
        private string _id;
        private bool _enabled;
        private bool _hidden;
        private string _installFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCardModel"/> class.
        /// </summary>
        public ObservableCardModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCardModel"/> class.
        /// </summary>
        /// <param name="card">Source card.</param>
        public ObservableCardModel(ICardModel card)
        {
            this._id = card.Id;
            this._enabled = card.Enabled;
            this._hidden = card.Hidden;
            this.InstallFolder = card.InstallFolder;

            this.PropertyChanged += this.SaveCardChanges;
        }

        /// <inheritdoc/>
        public CardMetadataModel Metadata
        {
            get
            {
                return this._metadata;
            }

            set
            {
                this._metadata = value;
                this.NotifyOfPropertyChange(() => this.Metadata);
            }
        }

        /// <inheritdoc/>
        public string Id
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
                this.NotifyOfPropertyChange(() => this.Enabled);
            }
        }

        /// <inheritdoc/>
        public bool Enabled
        {
            get
            {
                if (this.Metadata.Type == CardType.None)
                {
                    return false;
                }

                return this._enabled;
            }

            set
            {
                this._enabled = value;
                this.NotifyOfPropertyChange(() => this.Enabled);
            }
        }

        /// <inheritdoc/>
        public bool Hidden
        {
            get
            {
                return this._hidden;
            }

            set
            {
                this._hidden = value;
                this.NotifyOfPropertyChange(() => this.Hidden);
            }
        }

        /// <summary>
        /// Gets card type as a localized string.
        /// Currently does not update correctly after importing Aemulus packages.
        /// </summary>
        public string TypeString
        {
            get
            {
                return this.Metadata.Type switch
                {
                    CardType.Folder => LocalizedStrings.Instance["FolderText"],
                    CardType.Mod => LocalizedStrings.Instance["ModText"],
                    CardType.Launcher => LocalizedStrings.Instance["LauncherText"],
                    _ => string.Empty,
                };
            }
        }

        /// <inheritdoc/>
        public string InstallFolder { get; set; }

        private void SaveCardChanges(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ICardModel card)
            {
                /*
                var cardModel = new CardModel()
                {
                    Id = this.Id,
                    Enabled = this.Enabled,
                    Hidden = this.Hidden,
                    Name = this.Name,
                    Games = this.Games,
                    Authors = this.Authors,
                    Description = this.Description,
                    Version = this.Version,
                    Type = this.Type,
                };
                */

                // Update card metadata.
                var newMetadata = new CardMetadataModel()
                {
                    Name = this.Metadata.Name,
                    Games = this.Metadata.Games,
                    Authors = this.Metadata.Authors,
                    Description = this.Metadata.Description,
                    Version = this.Metadata.Version,
                    Type = this.Metadata.Type,
                };

                var cardText = JsonConvert.SerializeObject(newMetadata, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                File.WriteAllText(Path.Join(card.InstallFolder, "card.json"), cardText);
            }
        }
    }
}
