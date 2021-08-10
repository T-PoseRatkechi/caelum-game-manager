// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;
    using CaelumCoreLibrary.Decks;
    using CaelumCoreLibrary.Games;
    using CaelumCoreLibrary.Utilities;
    using CaelumGameManagerGUI.ViewModels.Authors;
    using Caliburn.Micro;

    /// <summary>
    /// EditCard VM.
    /// </summary>
    public class EditCardViewModel : Screen
    {
        private WindowManager windowManager = new WindowManager();

        private string selectedType = CardType.Mod.ToString();
        private BindableCollection<ICard> cards;
        private ICard card;
        private IGame game;

        private string _cardId;
        private string _cardName;

        private List<Author> _authors = new();

        public CardViewModel CardDisplay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCardViewModel"/> class.
        /// </summary>
        /// <param name="openContext">Context that VM was opened in: Create or Edit.</param>
        /// <param name="deckCards">Deck cards to add to or edit.</param>
        public EditCardViewModel(IGame game, BindableCollection<ICard> deckCards, ICard card = null)
        {
            this.cards = deckCards;
            this.game = game;
            this.card = card;

            if (card != null)
            {
                this._cardId = card.Id;
                this.CardName = card.Name;
                this.Authors = card.Authors;
                this.SelectedType = card.Type.ToString();
                this.Description = card.Description;

                this.CardDisplay = new(card);
            }

            this.SetContextualNames();
        }

        /// <summary>
        /// Gets the card ID with validation.
        /// </summary>
        public string CardId
        {
            get
            {
                if (string.IsNullOrEmpty(this.CardName) || this.Authors.Count < 1)
                {
                    this._cardId = null;
                }
                else
                {
                    var tempId = $"{this.Authors[0].Name}_{this.CardName}".ToLower().Replace(" ", string.Empty);
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
        public List<Author> Authors
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

        /// <summary>
        /// Gets or sets the card's preview image.
        /// </summary>
        public string CardImage { get; set; } = "/Resources/Images/missing-preview.png";

        /// <summary>
        /// Gets or sets the card's description.
        /// </summary>
        public string Description { get; set; }


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
                    string cardVersion = string.IsNullOrEmpty(this.Version) ? "1.0.0" : this.Version;
                    ICard newCard = this.game.CreateCard(this.CardId, this.CardName, cardType, this.Authors, this.Description, cardVersion);
                    this.CardImage = Path.Join(Directory.GetCurrentDirectory(), "test.png");

                    this.CardDisplay = new(newCard);
                    this.NotifyOfPropertyChange(() => this.CardDisplay);

                    this.cards.Add(newCard);
                }
                catch (Exception e)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Opens Available Authors view.
        /// </summary>
        public void OpenAuthors()
        {
            this.windowManager.ShowDialogAsync(new AuthorsViewModel(this.Authors));
            this.NotifyOfPropertyChange(() => this.AuthorText);
            this.NotifyOfPropertyChange(() => this.CardId);
        }

        public string AuthorText
        {
            get
            {
                if (this.Authors.Count == 0)
                {
                    return null;
                }
                else if (this.Authors.Count == 1)
                {
                    return this.Authors[0].Name;
                }
                else
                {
                    return $"{this.Authors[0].Name} +{this.Authors.Count - 1} other(s)";
                }
            }
        }

        /// <summary>
        /// Sets text to match context.
        /// </summary>
        /// <param name="context">Context the VM was opened with.</param>
        private void SetContextualNames()
        {
            if (this.card == null)
            {
                this.DisplayName = "Create Card";
                this.ConfirmText = "Create";
            }
            else
            {
                this.DisplayName = "Edit Card";
                this.ConfirmText = "Confirm";
            }
        }
    }
}
