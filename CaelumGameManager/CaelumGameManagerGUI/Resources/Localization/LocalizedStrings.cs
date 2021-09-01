// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Resources.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using WPFLocalizeExtension.Engine;

    /// <summary>
    /// Get localized strings from WPFLocalizeExtension resources.
    /// </summary>
    public class LocalizedStrings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedStrings"/> class.
        /// </summary>
        public LocalizedStrings()
        {
        }

        /// <summary>
        /// Gets localized strings instance.
        /// </summary>
        public static LocalizedStrings Instance { get; } = new();

        /// <summary>
        /// Gets the specified key string from WPFLocalizeExtension.
        /// </summary>
        /// <param name="key">The string's key.</param>
        /// <returns>The string key from localized resources.</returns>
        public string this[string key]
        {
            get
            {
                var result = LocalizeDictionary.Instance.GetLocalizedObject("CaelumGameManager", "Strings", key, LocalizeDictionary.Instance.Culture);
                return result as string;
            }
        }

        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <param name="cultureCode">Culture code to set to.</param>
        public void SetCulture(string cultureCode)
        {
            CultureInfo newCulture = new(cultureCode);
            LocalizeDictionary.Instance.Culture = newCulture;
        }
    }
}
