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
    using CaelumGameManagerGUI.Resources.Localization;
    using CaelumGameManagerGUI.ViewModels.Authors;
    using Caliburn.Micro;
    using Serilog;

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
            cards = deckCards;
            this.game = game;
            this.card = card;

            if (card != null)
            {
                _cardId = card.Id;
                CardName = card.Name;
                Authors = card.Authors;
                SelectedType = card.Type.ToString();
                Description = card.Description;

                CardDisplay = new(card);
            }

            SetContextualNames();
        }

        /// <summary>
        /// Gets the card ID with validation.
        /// </summary>
        public string CardId
        {
            get
            {
                if (string.IsNullOrEmpty(CardName) || Authors.Count < 1)
                {
                    _cardId = null;
                }
                else
                {
                    var tempId = $"{Authors[0].Name}_{CardName}".ToLower().Replace(" ", string.Empty);
                    if (CardUtils.IsValidId(tempId))
                    {
                        _cardId = tempId;
                    }
                }

                return _cardId;
            }
        }

        /// <summary>
        /// Gets or sets the card name.
        /// </summary>
        public string CardName
        {
            get
            {
                return _cardName;
            }

            set
            {
                _cardName = value;
                NotifyOfPropertyChange(() => CardName);
                NotifyOfPropertyChange(() => CardId);
            }
        }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        public List<Author> Authors
        {
            get
            {
                return _authors;
            }

            set
            {
                _authors = value;
                NotifyOfPropertyChange(() => Authors);
                NotifyOfPropertyChange(() => CardId);
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
            get { return selectedType; }
            set { selectedType = value; }
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
            if (card == null)
            {
                try
                {
                    CardType cardType = (CardType)Enum.Parse(typeof(CardType), SelectedType);
                    string cardVersion = string.IsNullOrEmpty(Version) ? "1.0.0" : Version;
                    ICard newCard = game.CreateCard(CardId, CardName, cardType, Authors, Description, cardVersion);
                    CardImage = Path.Join(Directory.GetCurrentDirectory(), "test.png");

                    CardDisplay = new(newCard);
                    NotifyOfPropertyChange(() => CardDisplay);

                    cards.Add(newCard);
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
            windowManager.ShowDialogAsync(new AuthorsViewModel(Authors));
            NotifyOfPropertyChange(() => AuthorText);
            NotifyOfPropertyChange(() => CardId);
        }

        public string AuthorText
        {
            get
            {
                if (Authors.Count == 0)
                {
                    return null;
                }
                else if (Authors.Count == 1)
                {
                    return Authors[0].Name;
                }
                else
                {
                    return $"{Authors[0].Name} +{Authors.Count - 1} other(s)";
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
                Log.Debug("Opening Create Card window");
                this.DisplayName = LocalizedStrings.Instance["CreateCardText"];
                this.ConfirmText = LocalizedStrings.Instance["CreateText"];
            }
            else
            {
                Log.Debug("Opening Edit Card window");
                this.DisplayName = LocalizedStrings.Instance["EditCardText"];
                this.ConfirmText = LocalizedStrings.Instance["ConfirmText"];
            }
        }
    }
}
