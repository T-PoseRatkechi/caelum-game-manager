// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Utilities
{
    using System.Text.RegularExpressions;

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
    }
}
