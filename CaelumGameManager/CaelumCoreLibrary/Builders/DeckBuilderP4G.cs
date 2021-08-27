// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Deck builder for Persona 4 Golden.
    /// </summary>
    public class DeckBuilderP4G : IDeckBuilder
    {
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBuilderP4G"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        public DeckBuilderP4G(ILogger log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public void Build(List<ICardModel> deck, string outputDir)
        {
            this.log.LogInformation($"Building deck for P4G. Total Cards: {deck.Count} - Output: {outputDir}");
        }
    }
}
