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
        /// Mods are archives that are installed to the game's <c>Cards</c> folder.
        /// They are backed by a hidden Folder card representing it's data in the <c>cards</c> folder.
        /// Mod and Folder Cards can be converted between the two.
        /// </summary>
        Mod = 0,

        /// <summary>
        /// Folders exist only as a folder in the game's <c>Cards</c> folder.
        /// Folders Cards are meant for WIP mods whose data is likely to change.
        /// Mod and Folder Cards can be converted between the two.
        /// </summary>
        Folder = 1,

        /// <summary>
        /// Tools are external programs, usually that help with mod development or mod an unsupported part
        /// of the game. They are installed in the app's <c>Tools</c> folder and can be viewed by any game.
        /// This is changable in the card's settings.
        /// </summary>
        Tool = 2,

        /// <summary>
        /// Updates contain updated files meant to overwrite an existing installation of another card.
        /// Helps to reduce the size of updates with larger mods.
        /// Required to have the same <c>Id</c> as an existing card in the game's deck.
        /// </summary>
        Update = 3,

        /// <summary>
        /// Presets are simply collections of every other type of card.
        /// </summary>
        Preset = 4,

        /// <summary>
        /// Launchers are alternative programs to start games, usually to add new functionality related to modding.
        /// Launchers are installed in the game's <c>Launchers</c> folder.
        /// </summary>
        Launcher = 5,

        /// <summary>
        /// Dependencies are programs or files required by Caelum to perform functions.
        /// They are installed the app's <c>Dependencies</c> folder.
        /// </summary>
        Dependency = 6,

        /// <summary>
        /// Simply a placeholder. Placeholder cards don't contain any data outside of a name.
        /// Installed in the game's <c>cards</c> folder.
        /// </summary>
        Empty = 7,
    }
}
