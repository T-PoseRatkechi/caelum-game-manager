// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
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

        private string cardId;

        public string CardId
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CardName) && !string.IsNullOrEmpty(this.Authors))
                {
                    try
                    {
                        if (this.Authors.Length > 0)
                        {
                            var authorsList = this.Authors.Split(',');
                            if (authorsList.Length > 0)
                            {
                                var tempId = $"{this.CardName}-{authorsList[0]}".ToLower();
                                if (CardUtils.IsValidId(tempId))
                                {
                                    this.cardId = tempId;
                                }
                                else
                                {
                                    this.cardId = null;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this.cardId = this.CardName;
                    }
                }

                return this.cardId;
            }
        }

        private string cardName;

        public string CardName
        {
            get
            {
                return this.cardName;
            }

            set
            {
                this.cardName = value;
                this.NotifyOfPropertyChange(() => this.CardName);
                this.NotifyOfPropertyChange(() => this.CardId);
            }
        }


        private string _authors;

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

        public bool CanConfirmCard()
        {
            if (string.IsNullOrEmpty(this.CardId))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Confirm button action.
        /// </summary>
        public void ConfirmCard()
        {
            if (this.context == "create")
            {
                try
                {
                    CardType cardType = (CardType)Enum.Parse(typeof(CardType), this.SelectedType);
                    var newCard = this.game.CreateCard(this.CardName, cardType);
                }
                catch (Exception e)
                {

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
