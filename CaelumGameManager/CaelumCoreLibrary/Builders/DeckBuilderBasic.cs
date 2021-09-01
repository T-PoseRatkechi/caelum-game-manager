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
    using CaelumCoreLibrary.Builders.Modules;
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Simple deck builder: copy and paste, file overwriting, patching, etc.
    /// </summary>
    public class DeckBuilderBasic : DeckBuilderBase
    {
        private readonly IGameFileProvider gameFileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBuilderBasic"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameFileProvider">Game file provider.</param>
        public DeckBuilderBasic(ILogger log, IGameFileProvider gameFileProvider)
            : base(log)
        {
            this.gameFileProvider = gameFileProvider;
        }

        /// <inheritdoc/>
        public override void Build(IList<ICardModel> deck, string outputDir)
        {
            this.Log.LogDebug("Building with Basic Deck Builder.");

            this.PrepareOutputFolder(outputDir);

            this.Log.LogDebug("Building cards.");

            IBuildLogger buildLogger = new BuildLogger();

            var outputBuilder = new OutputBuilder(this.Log, buildLogger, this.gameFileProvider)
                .AddModule(new PhosModule(this.Log, buildLogger))
                .AddModule(new StandardModule(this.Log, buildLogger));

            outputBuilder.BuildOutput(deck, outputDir);

            this.Log.LogDebug("Cards built.");

            // Write deck build log.
            buildLogger.WriteOutputLog(Path.Join(outputDir, "buildlog.txt"));
        }
    }
}
