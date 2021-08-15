// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Common;

    /// <summary>
    /// Game interface.
    /// </summary>
    public interface IGameInstall
    {
        /// <summary>
        /// Gets game's name.
        /// </summary>
        string GameName { get; }

        /// <summary>
        /// Gets game's base directory.
        /// </summary>
        string BaseDirectory { get; }

        /// <summary>
        /// Gets game's build directory.
        /// </summary>
        string BuildDirectory { get; }

        /// <summary>
        /// Gets game's card directory.
        /// </summary>
        string CardsDirectory { get; }

        /// <summary>
        /// Gets game's downloads dir.
        /// </summary>
        string DownloadsDirectory { get; }
    }
}