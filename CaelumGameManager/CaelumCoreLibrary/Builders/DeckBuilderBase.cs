// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Deck builder base.
    /// </summary>
    public abstract class DeckBuilderBase : IDeckBuilder
    {
        private const int MaxFilesDeleted = DeckBuilderUtilities.MaxFilesAllowedForDeleting;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBuilderBase"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        public DeckBuilderBase(ILogger log)
        {
            this.Log = log;
        }

        /// <summary>
        /// Gets or sets logger.
        /// </summary>
        protected ILogger Log { get; set; }

        /// <inheritdoc/>
        public abstract void Build(IList<ICardModel> deck, string outputDir);

        /// <summary>
        /// Prepares output folder for building.
        /// </summary>
        /// <param name="outputDir">Output folder.</param>
        /// <param name="ignoreMaxFilesWarning">Flag indicating whether to ignore the maximum allowed files for deleting.</param>
        protected void PrepareOutputFolder(string outputDir, bool ignoreMaxFilesWarning = false)
        {
            if (string.IsNullOrWhiteSpace(outputDir))
            {
                throw new ArgumentException($"'{nameof(outputDir)}' cannot be null or whitespace.", nameof(outputDir));
            }

            // TODO: Adjust for optimized building.

            // Disallow potentially unwanted output folders.
            if (!DeckBuilderUtilities.IsValidOutputDirectory(outputDir))
            {
                throw new ArgumentException($"!!!DISALLOWED OUTPUT DIRECTORY SET!!! CHANGE ASAP!!! OUTPUT DIRECTORY: {outputDir}", nameof(outputDir));
            }

            string[] outputDirFiles = Directory.GetFiles(outputDir, "*.*", SearchOption.AllDirectories);

            // Check number files that will be deleted exceeds max limit.
            // This check can be skipped by setting ignoreMaxFilesWarning to true.
            if (outputDirFiles.Length > MaxFilesDeleted && !ignoreMaxFilesWarning)
            {
                throw new ArgumentException($"!!!OUTPUT DIRECTORY CONTAINS MORE FILES THAN ALLOWED TO DELETE {MaxFilesDeleted}!!! DELETE MANUALLY OR CHANGE THIS SETTING!!! OUTPUT DIRECTORY: {outputDir}", nameof(outputDir));
            }

            this.Log.LogInformation("Preparing output folder.");

            this.Log.LogDebug("Deleting {NumFiles} files", outputDirFiles.Length);
            foreach (var file in outputDirFiles)
            {
                File.Delete(file);
                this.Log.LogTrace("Deleted file: {FilePath}.", file);
            }

            this.Log.LogInformation("Output folder prepared.");
        }
    }
}
