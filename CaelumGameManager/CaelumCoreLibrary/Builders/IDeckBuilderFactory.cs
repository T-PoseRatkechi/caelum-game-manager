// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    /// <summary>
    /// Interface for DeckBuilderFactory.
    /// </summary>
    public interface IDeckBuilderFactory
    {
        /// <summary>
        /// Gets deck builder by builder name.
        /// </summary>
        /// <param name="builderName">Builder name.</param>
        /// <returns>Deck builder.</returns>
        public IDeckBuilder GetDeckBuilderByName(string builderName);
    }
}
