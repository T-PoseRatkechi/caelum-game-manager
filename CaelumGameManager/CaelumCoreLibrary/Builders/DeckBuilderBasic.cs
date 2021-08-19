// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Simple deck builder: copy and paste, file overwriting, patching, etc.
    /// </summary>
    public class DeckBuilderBasic : IDeckBuilder
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBuilderBasic"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        public DeckBuilderBasic(ILogger log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public void Build(CardModel[] deck, string outputDir)
        {
            this.log.LogInformation("Using basic deck builder");
        }
    }
}
