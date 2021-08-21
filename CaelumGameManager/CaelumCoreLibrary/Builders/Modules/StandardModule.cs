// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Simply copies and pastes card files to output.
    /// </summary>
    public class StandardModule : IBuilderModule
    {
        /// <inheritdoc/>
        public DeckBuildLogger BuildLogger { get; init; }

        /// <inheritdoc/>
        public void BuildCard(CardModel card, string outputDir, HashSet<string> builtFiles)
        {
            string cardDataDir = Path.Join(card.InstallDirectory, "Data");

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
                    this.BuildLogger.LogOutputFile(card, outputFilePath);
                }
            }
        }
    }
}
