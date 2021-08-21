// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Builder for card output.
    /// </summary>
    public class OutputBuilder
    {
        private readonly ILogger log;
        private readonly DeckBuildLogger buildLogger;

        private readonly List<IBuilderModule> modules = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputBuilder"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="buildLogger">Build logger.</param>
        public OutputBuilder(ILogger log, DeckBuildLogger buildLogger)
        {
            this.log = log;
            this.buildLogger = buildLogger;
        }

        /// <summary>
        /// Adds a <seealso cref="IBuilderModule"/> to <see cref="OutputBuilder"/>'s list of modules to use for building cards.
        /// </summary>
        /// <typeparam name="T"><seealso cref="IBuilderModule"/> to add.</typeparam>
        /// <returns>This <seealso cref="OutputBuilder"/> instance.</returns>
        public OutputBuilder AddModule<T>()
            where T : IBuilderModule, new()
        {
            var newModule = new T()
            {
                BuildLogger = this.buildLogger,
            };

            this.modules.Add(newModule);
            return this;
        }

        /// <summary>
        /// Builds output with the given modules.
        /// </summary>
        /// <param name="cards">Cards to build.</param>
        /// <param name="outputDir">Output directory.</param>
        public void BuildOutput(List<CardModel> cards, string outputDir)
        {
            foreach (var card in cards)
            {
                this.BuildCardOutput(card, outputDir);
            }
        }

        /// <summary>
        /// Builds the output of <paramref name="card"/> at <paramref name="outputDir"/>.
        /// </summary>
        /// <param name="card">Card to build.</param>
        /// <param name="outputDir">Directory to output built files.</param>
        private void BuildCardOutput(CardModel card, string outputDir)
        {
            HashSet<string> builtCardFiles = new();

            foreach (var module in this.modules)
            {
                module.BuildCard(card, outputDir, builtCardFiles);
            }

            this.log.LogDebug("Card {CardName} built with {NumFiles} files processed.", card.Name, builtCardFiles.Count);
        }
    }
}
