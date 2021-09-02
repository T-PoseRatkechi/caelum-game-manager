// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System;
    using System.Collections.Generic;
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;

    public class DeckBuilderP5 : DeckBuilderBase
    {
        private readonly IGameFileProvider gameFileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBuilderP5"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameInstall">Game install.</param>
        /// <param name="gameConfig">Game config.</param>
        public DeckBuilderP5(ILogger log, IGameInstall gameInstall, GameConfigModel gameConfig)
            : base(log)
        {
            this.gameFileProvider = new GameFileProviderP5(log, gameConfig, gameInstall.UnpackedDirectory);
        }

        public override void Build(IList<ICardModel> deck, string outputDir)
        {
            throw new NotImplementedException();
        }
    }
}
