// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching.Utilities
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Extension method for converting a string of hex bytes to a byte array.
    /// </summary>
    public static class HexStringToByteArray
    {
        /// <summary>
        /// Converts string of hex bytes into a byte array.
        /// </summary>
        /// <param name="str">String of bytes.</param>
        /// <returns><paramref name="str"/> as a byte array.</returns>
        public static byte[] ToByteArray(this string str)
        {
            var fixedString = str.Replace(" ", string.Empty);
            if (fixedString.Length % 2 != 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", fixedString));
            }

            byte[] data = new byte[fixedString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = fixedString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
    }
}
