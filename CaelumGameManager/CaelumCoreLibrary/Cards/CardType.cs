// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    /// <summary>
    /// Available card types.
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// Mods are cards that were installed from an archive.
        /// </summary>
        Mod = 0,

        /// <summary>
        /// Folders are cards that have not installed archive and exist directyly in the cards folder. Usually WIPs mods.
        /// </summary>
        Folder = 1,

        /// <summary>
        /// Tools are installed app wide and can be used by any game.
        /// </summary>
        Tool = 2,

        /// <summary>
        /// Update cards share an id with another card. When installed they overwrite the files of the card with the matching id.
        /// </summary>
        Update = 3,

        /// <summary>
        /// Preset cards are a collection of the other cards.
        /// </summary>
        Preset = 4,
    }
}
