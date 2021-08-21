﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Text;
    using CaelumCoreLibrary.Builders.Modules;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Utilities;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Simple deck builder: copy and paste, file overwriting, patching, etc.
    /// </summary>
    public class DeckBuilderBasic : IDeckBuilder
    {
        private const int MaxFilesDeleted = DeckBuilderUtilities.MaxFilesAllowedForDeleting;

        private readonly ILogger log;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBuilderBasic"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="fileSystem">File system.</param>
        public DeckBuilderBasic(ILogger log, IFileSystem fileSystem)
        {
            this.log = log;
            this.fileSystem = fileSystem;
        }

        /// <inheritdoc/>
        public void Build(List<CardModel> deck, string outputDir)
        {
            this.log.LogDebug("Building with Basic Deck Builder.");

            this.PrepareOutputFolder(outputDir);

            this.log.LogDebug("Building cards.");

            DeckBuildLogger buildLogger = new();

            var outputBuilder = new OutputBuilder(this.log, buildLogger)
                .AddModule<PhosModule>()
                .AddModule<StandardModule>();

            outputBuilder.BuildOutput(deck, outputDir);

            this.log.LogDebug("Cards built.");

            // Write deck build log.
            buildLogger.WriteOutputLog(Path.Join(outputDir, "buildlog.txt"));
        }

        /// <summary>
        /// Prepares output folder for building.
        /// </summary>
        /// <param name="outputDir">Output folder.</param>
        /// <param name="ignoreMaxFilesWarning">Flag indicating whether to ignore the maximum allowed files for deleting.</param>
        public void PrepareOutputFolder(string outputDir, bool ignoreMaxFilesWarning = false)
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

            string[] outputDirFiles = this.fileSystem.Directory.GetFiles(outputDir, "*.*", SearchOption.AllDirectories);

            // Check number files that will be deleted exceeds max limit.
            // This check can be skipped by setting ignoreMaxFilesWarning to true.
            if (outputDirFiles.Length > MaxFilesDeleted && !ignoreMaxFilesWarning)
            {
                throw new ArgumentException($"!!!OUTPUT DIRECTORY CONTAINS MORE FILES THAN ALLOWED TO DELETE {MaxFilesDeleted}!!! DELETE MANUALLY OR CHANGE THIS SETTING!!! OUTPUT DIRECTORY: {outputDir}", nameof(outputDir));
            }

            this.log.LogInformation("Preparing output folder");

            this.log.LogDebug("Deleting {NumFiles} files", outputDirFiles.Length);
            foreach (var file in outputDirFiles)
            {
                File.Delete(file);
                this.log.LogTrace("Deleted file: {FilePath}", file);
            }

            this.log.LogInformation("Output folder prepared");
        }
    }
}
