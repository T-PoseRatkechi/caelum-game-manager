// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Builders.Modules;
    using CaelumCoreLibrary.Builders.Modules.PostBuild;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Deck builder for Persona 4 Golden.
    /// </summary>
    public class DeckBuilderP4G : DeckBuilderBase
    {
        private readonly IGameFileProvider gameFileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBuilderP4G"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameInstall">Game install.</param>
        /// <param name="gameConfig">Game config.</param>
        public DeckBuilderP4G(ILogger log, IGameInstall gameInstall, GameConfigModel gameConfig)
            : base(log)
        {
            this.gameFileProvider = new GameFileProviderP4G(log, gameConfig, gameInstall.UnpackedDirectory);
        }

        /// <inheritdoc/>
        public override void Build(IList<ICardModel> deck, string outputDir)
        {
            this.Log.LogInformation("Using Persona 4 Golden Deck Builder.");
            this.PrepareOutputFolder(outputDir);

            this.Log.LogDebug("Building cards.");

            IBuildLogger buildLogger = new BuildLogger();

            var outputBuilder = new OutputBuilder(this.Log, buildLogger, this.gameFileProvider)
                .AddModule(new PhosModule(this.Log, buildLogger))
                .AddModule(new StandardModule(this.Log, buildLogger))
                .PostBuild(new PostBuildP4G(this.Log, buildLogger, this.gameFileProvider));

            outputBuilder.BuildOutput(deck, outputDir);

            this.Log.LogDebug("Cards built.");

            // Write deck build log.
            buildLogger.WriteOutputLog(Path.Join(outputDir, "buildlog.txt"));
        }
    }
}
