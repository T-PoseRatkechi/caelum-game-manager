// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Decks
{
    using CaelumCoreLibrary.Games;

    /// <summary>
    /// CardsLoaderFactory interface.
    /// </summary>
    public interface ICardsLoaderFactory
    {
        /// <summary>
        /// Creates and returns a cards loader.
        /// </summary>
        /// <param name="gameInstall">Game install.</param>
        /// <returns><seealso cref="ICardsLoader"/> instance.</returns>
        ICardsLoader CreateCardsLoader(IGameInstall gameInstall);
    }
}