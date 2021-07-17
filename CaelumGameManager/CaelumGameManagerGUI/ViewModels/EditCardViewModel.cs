// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using CaelumCoreLibrary.Cards;
    using Caliburn.Micro;

    /// <summary>
    /// EditCard VM.
    /// </summary>
    public class EditCardViewModel : Screen
    {
        private string selectedType = CardType.Mod.ToString();

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCardViewModel"/> class.
        /// </summary>
        /// <param name="openContext">Context that VM was opened in: Create or Edit.</param>
        public EditCardViewModel(string openContext)
        {
            this.SetContextualNames(openContext);
        }

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
        /// Confirm button action.
        /// </summary>
        public void ConfirmButton()
        {

        }

        /// <summary>
        /// Sets text to match context.
        /// </summary>
        /// <param name="context">Context the VM was opened with.</param>
        private void SetContextualNames(string context)
        {
            switch (context)
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
