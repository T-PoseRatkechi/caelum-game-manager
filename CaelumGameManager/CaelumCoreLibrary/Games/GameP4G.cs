// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// IGame implementation for P4G.
    /// </summary>
    public class GameP4G : IGame
    {
        private string gamePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameP4G"/> class.
        /// </summary>
        public GameP4G()
        {
            this.Name = "Persona 4 Golden";
            this.Init();
        }

        /// <inheritdoc/>
        public string Name { get; init; }

        /// <inheritdoc/>
        public ICard CreateCard(string name, CardType type)
        {
            switch (type)
            {
                case CardType.Folder:
                    var cardFolderPath = Path.Join(this.gamePath, name);
                    return new FolderCard(cardFolderPath);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Handles initializing game folders and files.
        /// </summary>
        private void Init()
        {
            var gameDir = Directory.CreateDirectory(Path.Join(Directory.GetCurrentDirectory(), this.Name));
            this.gamePath = gameDir.FullName;
        }
    }
}
