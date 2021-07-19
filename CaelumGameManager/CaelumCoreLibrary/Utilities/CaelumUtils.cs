// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Utilities
{
    using System;

    /// <summary>
    /// Misc. utility functions for Caelum Library.
    /// </summary>
    public class CaelumUtils
    {
        /// <summary>
        /// Checks whether <paramref name="url"/> is a valid URL.
        /// </summary>
        /// <param name="url">URL to check.</param>
        /// <returns>True if <paramref name="url"/> is a valid URL, else false.</returns>
        public static bool IsValidUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var uri = new Uri(url);
                if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
