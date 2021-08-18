// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;

    /// <summary>
    /// Loads cards from tools and game install folder.
    /// </summary>
    public class CardsLoader : ICardsLoader
    {
        private readonly string toolsDir;
        private readonly string gameCardDir;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardsLoader"/> class.
        /// </summary>
        /// <param name="caelumConfig">Caelum config to use for directories.</param>
        /// <param name="gameInstall">Game install instance.</param>
        public CardsLoader(ICaelumConfig caelumConfig, IGameInstall gameInstall)
        {
            this.toolsDir = caelumConfig.ToolsDirectory;
            this.gameCardDir = gameInstall.CardsDirectory;
        }

        /// <inheritdoc/>
        public void LoadCards(List<CardModel> cardsList)
        {
            string[] allToolCardDirs = Directory.GetDirectories(this.toolsDir);
            foreach (string toolCardDir in allToolCardDirs)
            {
                // Parse card.

                // Validate card.

                // Add card to list.
            }

            string[] allGameCardDirs = Directory.GetDirectories(this.gameCardDir);
            foreach (string gameCardDir in allGameCardDirs)
            {
                // Parse card.

                // Validate card.

                // Add card to list.
            }
        }

        /// <summary>
        /// Adds every card found in <paramref name="folder"/> to <paramref name="cardsList"/>.
        /// </summary>
        /// <param name="folder">Folder to seach for cards in.</param>
        private void AddCardsFromFolder(string folder, List<CardModel> cardsList)
        {
            foreach (string file in Directory.GetDirectories(folder))
            {
                // Parse card.

                // Validate card.

                // Add card to list.
            }
        }
    }
}
