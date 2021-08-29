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
        private string _cardId;
        private bool _isEnabled;
        private string _name;
        private string _description;
        private string _version;
        private CardType _type;

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

            this.PropertyChanged += this.SaveCardChanges;
        }

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

        /// <inheritdoc/>
        public bool IsEnabled
        {
            get
            {
                if (this.Type == CardType.None)
                {
                    return false;
                }

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
        public string Description
        {
            get
            {
                return this._description;
            }

            set
            {
                this._description = value;
                this.NotifyOfPropertyChange(() => this.Description);
            }
        }

        /// <inheritdoc/>
        public string Version
        {
            get
            {
                return this._version;
            }

            set
            {
                this._version = value;
                this.NotifyOfPropertyChange(() => this.Version);
            }
        }

        /// <inheritdoc/>
        public CardType Type
        {
            get
            {
                return this._type;
            }

            set
            {
                this._type = value;
                this.NotifyOfPropertyChange(() => this.Type);
                this.NotifyOfPropertyChange(() => this.TypeString);
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
                return this.Type switch
                {
                    CardType.Folder => LocalizedStrings.Instance["FolderText"],
                    CardType.Mod => LocalizedStrings.Instance["ModText"],
                    CardType.Launcher => LocalizedStrings.Instance["LauncherText"],
                    _ => string.Empty,
                };
            }
        }

        /// <inheritdoc/>
        public string InstallDirectory { get; set; }

        private void SaveCardChanges(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ICardModel card)
            {
                var cardModel = new CardModel()
                {
                    CardId = this.CardId,
                    IsEnabled = this.IsEnabled,
                    IsHidden = this.IsHidden,
                    Name = this.Name,
                    Games = this.Games,
                    Authors = this.Authors,
                    Description = this.Description,
                    Version = this.Version,
                    Type = this.Type,
                };

                var cardText = JsonConvert.SerializeObject(cardModel, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                File.WriteAllText(Path.Join(card.InstallDirectory, "card.json"), cardText);
            }
        }
    }
}
