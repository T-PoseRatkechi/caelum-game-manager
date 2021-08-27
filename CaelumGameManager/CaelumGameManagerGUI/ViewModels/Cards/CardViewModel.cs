// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels.Cards
{
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using Caliburn.Micro;

    /// <summary>
    /// Card display.
    /// </summary>
    public class CardViewModel : Screen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardViewModel"/> class.
        /// </summary>
        /// <param name="card"><seealso cref="ICardModel"/> instance to dispaly.</param>
        public CardViewModel(ICardModel card = null)
        {
            this.Card = card;
        }

        /// <summary>
        /// Gets or sets card to display;
        /// </summary>
        public ICardModel Card { get; set; }

        /// <summary>
        /// Gets the card image source, if available. Else returns default missing-preview image.
        /// </summary>
        public string CardImage
        {
            get
            {
                if (this.Card is ICardModel installedCard)
                {
                    string cardImagePath = Path.Join(installedCard.InstallDirectory, "card.png");
                    if (File.Exists(cardImagePath))
                    {
                        return cardImagePath;
                    }
                    else
                    {
                        return null;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets top-right card icon.
        /// </summary>
        public string CardIcon
        {
            get
            {
                if (this.Card != null)
                {
                    switch (this.Card.Type)
                    {
                        case CardType.Folder:
                            return "FolderOpen";
                        case CardType.Tool:
                            return "Pickaxe";
                        case CardType.Update:
                            return "Update";
                        case CardType.Preset:
                            return "Cards";
                        case CardType.Mod:
                            return "Plus";
                        default:
                            return "HelpBox";
                    }
                }

                return "HelpBox";
            }
        }
    }
}
