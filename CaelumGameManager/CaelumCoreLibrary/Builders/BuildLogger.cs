// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Responsible for logging a deck's output build.
    /// </summary>
    public class BuildLogger : IBuildLogger
    {
        private readonly Dictionary<string, List<string>> outputFilesLog = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildLogger"/> class.
        /// </summary>
        public BuildLogger()
        {
        }

        /// <summary>
        /// Adds to output log that <paramref name="card"/> outputted <paramref name="file"/>.
        /// </summary>
        /// <param name="card">Card that outputted file.</param>
        /// <param name="outputFile">File that was outputted.</param>
        public void LogOutputFile(CardModel card, string outputFile)
        {
            if (this.outputFilesLog.ContainsKey(outputFile))
            {
                this.outputFilesLog[outputFile].Add(card.CardId);
            }
            else
            {
                this.outputFilesLog.Add(outputFile, new() { card.CardId });
            }
        }

        /// <summary>
        /// Writes the output build log to file.
        /// </summary>
        /// <param name="outputFile">Log output file.</param>
        public void WriteOutputLog(string outputFile)
        {
            // Output build log.
            StringBuilder sb = new();
            foreach (var entry in this.outputFilesLog)
            {
                sb.AppendLine($"File: {entry.Key}\nCards: {string.Join(", ", entry.Value)}\n");
            }

            File.WriteAllText(outputFile, sb.ToString());
        }
    }
}
