// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1309 // Field names should not begin with underscore

namespace CaelumGameManagerGUI.ViewModels
{
    using CaelumCoreLibrary.Cards;
    using Caliburn.Micro;
    using System.IO;

    /// <summary>
    /// Card display.
    /// </summary>
    public class CardViewModel : Screen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardViewModel"/> class.
        /// </summary>
        /// <param name="card"><seealso cref="ICard"/> instance to dispaly.</param>
        public CardViewModel(ICard card = null)
        {
            this.Card = card;
        }

        /// <summary>
        /// Gets or sets card to display;
        /// </summary>
        public ICard Card { get; set; }

        /// <summary>
        /// Gets the card image source, if available. Else returns default missing-preview image.
        /// </summary>
        public string CardImage
        {
            get
            {
                if (this.Card == null)
                {
                    return "/Resources/Images/missing-preview.png";
                }

                string cardImagePath = Path.Join(this.Card.Data.Path, "card.png");
                if (File.Exists(cardImagePath))
                {
                    return cardImagePath;
                }
                else
                {
                    return "/Resources/Images/missing-preview.png";
                }
            }
        }

        public string CardIcon
        {
            get
            {
                if (this.Card != null)
                {
                    switch (this.Card.Data.Type)
                    {
                        case CardType.Folder:
                            return "FolderOpen";
                        case CardType.Tool:
                            return "Pickaxe";
                        case CardType.Update:
                            return "Update";
                        case CardType.Preset:
                            return "Cards";
                        default:
                            return "Help";
                    }
                }

                return "Help";
            }
        }
    }
}
