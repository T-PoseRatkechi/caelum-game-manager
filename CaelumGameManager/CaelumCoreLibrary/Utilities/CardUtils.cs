// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Utilities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;

    /// <summary>
    /// Utility functions related to Cards.
    /// </summary>
    public class CardUtils
    {
        private const string InvalidIdCharacters = @"([^A-z0-9-_]|[\^\\])";

        /// <summary>
        /// Returns whether <paramref name="s"/> is valid as an ID. Only A-z and - characters.
        /// </summary>
        /// <param name="s">String to test.</param>
        /// <returns>If <paramref name="s"/> is a valid <seealso cref="Cards.ICard"/> ID.</returns>
        public static bool IsValidId(string s)
        {
            Regex reg = new(InvalidIdCharacters);

            if (reg.IsMatch(s))
            {
                return false;
            }

            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            if (s.Length > 64)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parses the card at <paramref name="cardFilePath"/> and returns it.
        /// </summary>
        /// <param name="cardFilePath">Path to card.</param>
        /// <returns><paramref name="cardFilePath"/> parsed as a new <seealso cref="ICard"/>.</returns>
        public static ICard ParseCard<T>(string cardFilePath)
        {
            var cardText = File.ReadAllText(cardFilePath);
            ICard card = (ICard)JsonSerializer.Deserialize<T>(cardText);

            // Set card path.
            // card.Path = Path.GetDirectoryName(cardFilePath);

            // Load authors.
            // var cardAuthorsPath = Path.Join(card.Path, "authors");
            // var cardAuthors = AuthorUtils.GetAllAuthors(cardAuthorsPath);
           //  card.Authors = new List<Author>(cardAuthors);

            return card;
        }
    }
}
