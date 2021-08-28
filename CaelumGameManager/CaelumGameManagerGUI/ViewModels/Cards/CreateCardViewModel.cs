// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels.Cards
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using CaelumCoreLibrary.Games;
    using CaelumCoreLibrary.Utilities;
    using CaelumGameManagerGUI.Models;
    using CaelumGameManagerGUI.Resources.Localization;
    using CaelumGameManagerGUI.ViewModels.Authors;
    using Caliburn.Micro;
    using Serilog;

    /// <summary>
    /// CreateCard VM.
    /// </summary>
    public class CreateCardViewModel : Screen
    {
        private WindowManager windowManager = new WindowManager();

        private string selectedType = CardType.Mod.ToString();
        private BindableCollection<ObservableCardModel> cards;
        private ICardModel card;
        private IGameInstance game;
        private ICardFactory cardFactory;

        private string _cardId;
        private string _cardName;

        private List<Author> _authors = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCardViewModel"/> class.
        /// </summary>
        /// <param name="openContext">Context that VM was opened in: Create or Edit.</param>
        /// <param name="deckCards">Deck cards to add to or edit.</param>
        public CreateCardViewModel(IGameInstance game, ICardFactory cardFactory, BindableCollection<ObservableCardModel> deckCards, ICardModel card = null)
        {
            this.cards = deckCards;
            this.cardFactory = cardFactory;
            this.game = game;
            this.card = card;

            if (card != null)
            {
                this._cardId = card.CardId;
                this.CardName = card.Name;
                this.CardAuthors = card.Authors;
                this.SelectedType = card.Type.ToString();
                this.CardDescription = card.Description;

                this.CardDisplay = new(card);
            }

            this.SetContextualNames();
        }

        /// <summary>
        /// Gets or sets card display VM.
        /// </summary>
        public CardViewModel CardDisplay { get; set; }

        /// <summary>
        /// Gets the card ID with validation.
        /// </summary>
        public string CardId
        {
            get
            {
                if (string.IsNullOrEmpty(this.CardName) || this.CardAuthors.Count < 1)
                {
                    this._cardId = null;
                }
                else
                {
                    var tempId = $"{this.CardAuthors[0].Name}_{this.CardName}".ToLower().Replace(" ", string.Empty);
                    if (CardUtils.IsValidId(tempId))
                    {
                        this._cardId = tempId;
                    }
                }

                return this._cardId;
            }
        }

        /// <summary>
        /// Gets or sets the card name.
        /// </summary>
        public string CardName
        {
            get
            {
                return this._cardName;
            }

            set
            {
                this._cardName = value;
                this.NotifyOfPropertyChange(() => this.CardName);
                this.NotifyOfPropertyChange(() => this.CardId);
            }
        }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        public List<Author> CardAuthors
        {
            get
            {
                return this._authors;
            }

            set
            {
                this._authors = value;
                this.NotifyOfPropertyChange(() => this.CardAuthors);
                this.NotifyOfPropertyChange(() => this.CardId);
            }
        }

        /// <summary>
        /// Gets or sets the card version.
        /// </summary>
        public string CardVersion { get; set; }

        /// <summary>
        /// Gets Confirm button text.
        /// </summary>
        public string ConfirmText { get; private set; }

        /// <summary>
        /// Gets a list of available card types.
        /// </summary>
        public string[] CardTypes { get; } = Enum.GetNames(typeof(CardType));

        /// <summary>
        /// Gets or sets the currently selected type for card.
        /// </summary>
        public string SelectedType
        {
            get { return this.selectedType; }
            set { this.selectedType = value; }
        }

        /// <summary>
        /// Gets or sets the card's preview image.
        /// </summary>
        public string CardImage { get; set; } = "/Resources/Images/missing-preview.png";

        /// <summary>
        /// Gets or sets the card's description.
        /// </summary>
        public string CardDescription { get; set; }

        /// <summary>
        /// Gets text to display for authors.
        /// </summary>
        public string AuthorText
        {
            get
            {
                if (this.CardAuthors.Count == 0)
                {
                    return null;
                }
                else if (this.CardAuthors.Count == 1)
                {
                    return this.CardAuthors[0].Name;
                }
                else
                {
                    return $"{this.CardAuthors[0].Name} +{this.CardAuthors.Count - 1} other(s)";
                }
            }
        }


        /// <summary>
        /// Validates card properties are valid and enables/disables the confirm button.
        /// </summary>
        /// <param name="cardId">Card ID.</param>
        /// <returns>Whether card has valid properties.</returns>
        public bool CanConfirmCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Confirm button action.
        /// </summary>
        /// <param name="cardId">Card ID.</param>
#pragma warning disable IDE0060 // Remove unused parameter
        public void ConfirmCard(string cardId)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (this.card == null)
            {
                try
                {
                    CardType cardType = (CardType)Enum.Parse(typeof(CardType), this.SelectedType);

                    string cardVersion = string.IsNullOrEmpty(this.CardVersion) ? "1.0.0" : this.CardVersion;

                    ObservableCardModel newCard = new();
                    newCard.CardId = this.CardId;
                    newCard.Name = this.CardName;
                    newCard.Description = this.CardDescription;
                    newCard.Authors = this.CardAuthors;
                    newCard.Version = cardVersion;
                    newCard.Type = cardType;

                    this.cardFactory.CreateCard(this.game.GameInstall, newCard);
                    this.cards.Add(newCard);

                    this.CardDisplay = new CardViewModel(newCard);

                    this.NotifyOfPropertyChange(() => this.CardDisplay);
                }
                catch (Exception e)
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                this.card.CardId = this.CardId;
                this.card.Name = this.CardName;
                this.card.Description = this.CardDescription;
                this.card.Authors = this.CardAuthors;
                // this.card.Version = this.cardVersion;
            }
        }

        /// <summary>
        /// Opens Available Authors view.
        /// </summary>
        public void OpenAuthors()
        {
            this.windowManager.ShowDialogAsync(new AuthorsViewModel(this.CardAuthors));
            this.NotifyOfPropertyChange(() => this.AuthorText);
            this.NotifyOfPropertyChange(() => this.CardId);
        }

        /// <summary>
        /// Sets text to match context.
        /// </summary>
        /// <param name="context">Context the VM was opened with.</param>
        private void SetContextualNames()
        {
            if (this.card == null)
            {
                Log.Verbose("Opening Create Card window");
                this.DisplayName = LocalizedStrings.Instance["CreateCardText"];
                this.ConfirmText = LocalizedStrings.Instance["CreateText"];
            }
            else
            {
                Log.Verbose("Opening Edit Card window");
                this.DisplayName = LocalizedStrings.Instance["EditCardText"];
                this.ConfirmText = LocalizedStrings.Instance["ConfirmText"];
            }
        }
    }
}
