// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System.IO;

    /// <summary>
    /// Base card implementation.
    /// </summary>
    public abstract class BaseCard : ICard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCard"/> class.
        /// </summary>
        /// <param name="cardPath">Path to card.</param>
        /// <param name="type">Card type.</param>
        public BaseCard(string cardPath, CardType type)
        {
            // Create card directory.
            Directory.CreateDirectory(cardPath);

            // Create and set data directory.
            string cardDataPath = Path.Join(cardPath, "data");
            Directory.CreateDirectory(cardDataPath);

            this.Data = new() { Path = cardDataPath, Type = type };
        }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string[] Authors { get; set; }

        /// <inheritdoc/>
        public string Game { get; set; }

        /// <inheritdoc/>
        public string Version { get; set; }

        /// <inheritdoc/>
        public CardData Data { get; init; }
    }
}
