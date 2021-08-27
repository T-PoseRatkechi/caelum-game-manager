// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

/// <summary>
/// PreappAppend as a card builder module. Unused in favor of a quicker post-build step.
/// Might be useful with have multiple new pacs.
/// </summary>
namespace CaelumCoreLibrary.Builders.Modules
{
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Build card files that require preappfile append.
    /// </summary>
    public class PreappAppendModule : IBuilderModule
    {
        private readonly ILogger log;
        private readonly IBuildLogger buildLogger;
        private readonly IGameFileProvider gameFileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreappAppendModule"/> class.
        /// </summary>
        /// <param name="log">Logger</param>
        /// <param name="buildLogger">Build logger.</param>
        public PreappAppendModule(ILogger log, IBuildLogger buildLogger, IGameFileProvider gameFileProvider)
        {
            this.log = log;
            this.buildLogger = buildLogger;
            this.gameFileProvider = gameFileProvider;
        }

        /// <inheritdoc/>
        public void BuildCard(CardModel card, string outputDir, HashSet<string> builtCardFiles)
        {
            var dataFolders = Directory.GetDirectories(Path.Join(card.InstallDirectory, "Data"), "data_*", SearchOption.TopDirectoryOnly);

            /*
            // Only allow one data_x card.
            if (dataDirs.Length > 1)
            {
                throw new ArgumentException($@"More than one data_x folder was found in card ""{card.CardId}"".");
            }
            */

            foreach (var dataDir in dataFolders)
            {
                var appendDir = Path.Join(dataDir, "append");
                if (!Directory.Exists(appendDir))
                {
                    continue;
                }

                var archiveToAppend = Path.GetFileName(dataDir);

                this.gameFileProvider.AppendArchive(archiveToAppend, appendDir);

                foreach (var file in Directory.GetFiles(appendDir, "*", SearchOption.AllDirectories))
                {
                    builtCardFiles.Add(file);
                }
            }
        }
    }
}
