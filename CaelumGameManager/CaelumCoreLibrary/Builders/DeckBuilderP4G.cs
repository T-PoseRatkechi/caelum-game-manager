// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using CaelumCoreLibrary.Cards;
    using Serilog;

    /// <summary>
    /// Deck builder for Persona 4 Golden.
    /// </summary>
    public class DeckBuilderP4G : IDeckBuilder
    {
        /// <inheritdoc/>
        public void Build(InstallableCardModel[] deck, string outputDir)
        {
            Log.Information($"Building deck for P4G. Total Cards: {deck.Length} - Output: {outputDir}");
        }
    }
}
