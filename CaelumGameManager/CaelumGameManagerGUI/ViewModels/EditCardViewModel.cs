// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Decks;
    using CaelumCoreLibrary.Games;
    using CaelumCoreLibrary.Utilities;
    using CaelumGameManagerGUI.Models;
    using Caliburn.Micro;

    /// <summary>
    /// EditCard VM.
    /// </summary>
    public class EditCardViewModel : Screen
    {
        private string selectedType = CardType.Mod.ToString();
        private BindableCollection<CardModel> cards;
        private IGame game;
        private string context;

        private string _cardId;
        private string _cardName;

        private string _authors;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCardViewModel"/> class.
        /// </summary>
        /// <param name="openContext">Context that VM was opened in: Create or Edit.</param>
        /// <param name="deckCards">Deck cards to add to or edit.</param>
        public EditCardViewModel(string openContext, IGame game, BindableCollection<CardModel> deckCards)
        {
            this.context = openContext;
            this.cards = deckCards;
            this.game = game;

            this.SetContextualNames();
        }

        /// <summary>
        /// Gets the card ID with validation.
        /// </summary>
        public string CardId
        {
            get
            {
                this._cardId = null;

                if (string.IsNullOrEmpty(this.CardName) || string.IsNullOrEmpty(this.Authors))
                {
                    return this._cardId;
                }

                try
                {
                    if (this.Authors.Length > 0)
                    {
                        var authorsList = this.Authors.Split(',');
                        if (authorsList.Length > 0)
                        {
                            var tempId = $"{authorsList[0]}_{this.CardName}".ToLower().Replace(" ", string.Empty);
                            if (CardUtils.IsValidId(tempId))
                            {
                                this._cardId = tempId;
                            }
                        }
                    }
                }
                catch (Exception e)
                {

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
        public string Authors
        {
            get
            {
                return this._authors;
            }

            set
            {
                this._authors = value;
                this.NotifyOfPropertyChange(() => this.Authors);
                this.NotifyOfPropertyChange(() => this.CardId);
            }
        }

        /// <summary>
        /// Gets or sets the card version.
        /// </summary>
        public string Version { get; set; }

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

        private string _cardImage = "/Resources/Images/missing-preview.png";

        public string CardImage
        {
            get { return _cardImage; }
            set { _cardImage = value; }
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
            if (this.context == "create")
            {
                try
                {
                    CardType cardType = (CardType)Enum.Parse(typeof(CardType), this.SelectedType);
                    string cardVersion = string.IsNullOrEmpty(this.Version) ? "1.0.0" : this.Version;
                    ICard newCard = this.game.CreateCard(this.CardId, this.CardName, cardType, this.Authors.Split(','), cardVersion);
                    this.CardImage = Path.Join(Directory.GetCurrentDirectory(), "test.png");

                    this.cards.Add(new CardModel(newCard));
                }
                catch (Exception e)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Sets text to match context.
        /// </summary>
        /// <param name="context">Context the VM was opened with.</param>
        private void SetContextualNames()
        {
            switch (this.context)
            {
                case "create":
                    this.DisplayName = "Create Card";
                    this.ConfirmText = "Create";
                    break;
                case "edit":
                    this.DisplayName = "Edit Card";
                    this.ConfirmText = "Confirm";
                    break;
                default:
                    this.DisplayName = "Edit Card";
                    this.ConfirmText = "Confirm";
                    break;
            }
        }
    }
}
