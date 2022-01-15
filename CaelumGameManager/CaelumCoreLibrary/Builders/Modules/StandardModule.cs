// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules
{
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Simply copies and pastes card files to output.
    /// </summary>
    public class StandardModule : IBuilderModule
    {
        private readonly ILogger log;
        private readonly IBuildLogger buildLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardModule"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="buildLogger">Build logger.</param>
        public StandardModule(ILogger log, IBuildLogger buildLogger)
        {
            this.log = log;
            this.buildLogger = buildLogger;
        }

        /// <inheritdoc/>
        public void BuildCard(ICardModel card, string outputDir, HashSet<string> builtFiles)
        {
            string cardDataDir = Path.Join(card.InstallFolder, "Data");

            foreach (var dataFile in Directory.GetFiles(cardDataDir, "*.*", SearchOption.AllDirectories))
            {
                if (!builtFiles.Contains(dataFile))
                {
                    var outputFilePath = dataFile.Replace(cardDataDir, outputDir);
                    if (!Directory.Exists(Path.GetDirectoryName(outputFilePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                    }

                    File.Copy(dataFile, outputFilePath, true);

                    // Add to output lists.
                    builtFiles.Add(dataFile);
                    this.buildLogger.LogOutputFile(card, outputFilePath);
                }
            }
        }
    }
}
