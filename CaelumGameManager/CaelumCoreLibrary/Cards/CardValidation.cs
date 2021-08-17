// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Functions related to validating card properties.
    /// </summary>
    public static class CardValidation
    {
        private const string InvalidIdCharacters = @"([^A-z0-9]|[\^\\])";
        private static readonly Regex InvalidIdCharactersReg = new(InvalidIdCharacters);

        /// <summary>
        /// Combines <paramref name="authorName"/> and <paramref name="cardName"/> into a valid card ID.
        /// Filters out any non-ASCII characters from <paramref name="authorName"/> and <paramref name="cardName"/>.
        /// </summary>
        /// <param name="authorName">Author name.</param>
        /// <param name="cardName">Card name.</param>
        /// <returns>Valid Card ID, <c>null</c> if <paramref name="authorName"/> or <paramref name="cardName"/> becomes empty after filtering.</returns>
        public static string GetValidCardId(string authorName, string cardName)
        {
            if (string.IsNullOrWhiteSpace(authorName))
            {
                throw new ArgumentException($"'{nameof(authorName)}' cannot be null or whitespace.", nameof(authorName));
            }

            if (string.IsNullOrWhiteSpace(cardName))
            {
                throw new ArgumentException($"'{nameof(cardName)}' cannot be null or whitespace.", nameof(cardName));
            }

            // Filter out any non-ASCII characters from author and card names.
            var parsedAuthor = InvalidIdCharactersReg.Replace(authorName, string.Empty);
            var parsedCard = InvalidIdCharactersReg.Replace(cardName, string.Empty);

            // Return if either author or card name no longer has any characters.
            if (string.IsNullOrWhiteSpace(parsedAuthor) || string.IsNullOrWhiteSpace(parsedCard))
            {
                return null;
            }

            // Final card id.
            var finalCardId = $"{parsedAuthor}_{parsedCard}".ToLower();

            return finalCardId;
        }
    }
}
