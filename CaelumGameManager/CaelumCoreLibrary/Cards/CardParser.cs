// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using Newtonsoft.Json;
    using System;
    using System.IO;

    /// <summary>
    /// Base implementation of <see cref="ICardParser"/>.
    /// </summary>
    public class CardParser : ICardParser
    {
        /// <inheritdoc/>
        public ICardModel ParseCard<T>(string cardFile)
            where T : ICardModel
        {
            if (string.IsNullOrWhiteSpace(cardFile))
            {
                throw new ArgumentException($"'{nameof(cardFile)}' cannot be null or whitespace.", nameof(cardFile));
            }

            string cardText = File.ReadAllText(cardFile);
            var card = JsonConvert.DeserializeObject<T>(cardText);

            card.InstallFolder = Path.GetDirectoryName(cardFile);

            // Load author files.
            var authorsDir = Path.Join(card.InstallFolder, "Authors");
            if (Directory.Exists(authorsDir))
            {
                foreach (var file in Directory.GetFiles(authorsDir))
                {
                    // TODO: Load authors.
                }
            }

            return card;
        }
    }
}
