// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Addons
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Simply copies and pastes card files to output.
    /// </summary>
    public class BasicBuild : IBuilderAddon
    {
        /// <inheritdoc/>
        public void BuildCard(CardModel card, string outputDir, HashSet<string> builtFiles, Dictionary<string, List<string>> deckbuildLog)
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

                    // Added output files to list.
                    if (deckbuildLog.ContainsKey(outputFilePath))
                    {
                        deckbuildLog[outputFilePath].Add(card.CardId);
                    }
                    else
                    {
                        deckbuildLog.Add(outputFilePath, new());
                        deckbuildLog[outputFilePath].Add(card.CardId);
                    }
                }
            }
        }

    }
}
