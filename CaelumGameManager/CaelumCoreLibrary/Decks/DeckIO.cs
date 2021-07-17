// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// Utility functions for reading and writing decks from and to file.
    /// </summary>
    public class DeckIO
    {
        /// <summary>
        /// Parses the JSON file given by <paramref name="deckPath"/> as a <seealso cref="IDeck"/>.
        /// </summary>
        /// <param name="deckPath">File path of deck.</param>
        /// <returns><paramref name="deckPath"/> parsed as <seealso cref="IDeck"/>.</returns>
        public static IDeck ParseDeck(string deckPath)
        {
            var deckText = File.ReadAllText(deckPath);
            var deckJson = JsonSerializer.Deserialize<IDeck>(deckText);

            return deckJson;
        }

        /// <summary>
        /// Writes to <paramref name="deck"/> the given <paramref name="deck"/> as a JSON.
        /// </summary>
        /// <param name="deckPath">File path to write to.</param>
        /// <param name="deck">Deck to write as JSON.</param>
        public static void WriteDeck(string deckPath, IDeck deck)
        {
            var deckString = JsonSerializer.Serialize(deck);
            File.WriteAllText(deckPath, deckString);
        }
    }
}
