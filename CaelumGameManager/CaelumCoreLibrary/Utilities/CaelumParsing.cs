// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Utilities
{
    using System;
    using System.IO;
    using System.Text.Json;
    using CaelumCoreLibrary.Games;

    /// <summary>
    /// Functions related to parsing files related to Caelum library.
    /// </summary>
    public class CaelumParsing
    {
        /// <summary>
        /// Returns <paramref name="configFile"/> parsed as <seealso cref="GameConfig"/>.
        /// </summary>
        /// <param name="configFile">File to parse.</param>
        /// <returns><paramref name="configFile"/> as <seealso cref="GameConfig"/>.</returns>
        public static GameConfig ParseGameConfig(string configFile)
        {
            var configText = File.ReadAllText(configFile);
            var config = JsonSerializer.Deserialize<GameConfig>(configText);
            return config;
        }
    }
}
