// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System;
    using System.IO;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Utilities;
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
            this.log.LogDebug("Using basic deck builder");

            foreach (var card in deck)
            {
                string cardDataDir = Path.Join(card.InstallDirectory, "Data");

                foreach (var dataFile in Directory.GetFiles(cardDataDir, "*.*", SearchOption.AllDirectories))
                {
                    var outputFilePath = dataFile.Replace(cardDataDir, outputDir);
                    File.Copy(dataFile, outputFilePath);
                }
            }
        }

        /// <summary>
        /// Prepares output folder for building.
        /// </summary>
        /// <param name="outputDir">Output folder.</param>
        public void PrepareOutputFolder(string outputDir, bool ignoreFileCountWarning = false)
        {
            if (string.IsNullOrWhiteSpace(outputDir))
            {
                throw new ArgumentException($"'{nameof(outputDir)}' cannot be null or whitespace.", nameof(outputDir));
            }

            // TODO: Adjust for optimized building.

            // Disallow potentially unwanted output folders.
            if (!DeckBuilderUtilities.IsValidOutputDirectory(outputDir))
            {
                throw new ArgumentException($"!!!DISALLOWED OUTPUT DIRECTORY SET!!! OUTPUT DIRECTORY: {outputDir}", nameof(outputDir));
            }
        }
    }
}
