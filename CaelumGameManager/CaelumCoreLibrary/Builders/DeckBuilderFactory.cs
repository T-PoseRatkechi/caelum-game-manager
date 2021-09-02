// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Default <see cref="IDeckBuilderFactory"/> implementation.
    /// </summary>
    public class DeckBuilderFactory : IDeckBuilderFactory
    {
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckBuilderFactory"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        public DeckBuilderFactory(ILogger log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public IDeckBuilder GetGameDeckBuilder(IGameInstall gameInstall, GameConfigModel gameConfig)
        {
            switch (gameInstall.GameName)
            {
                case "Persona 4 Golden":
                    return new DeckBuilderP4G(this.log, gameInstall, gameConfig);
                case "Persona 5":
                    return new DeckBuilderP5(this.log, gameInstall, gameConfig);
                default:
                    return new DeckBuilderBasic(this.log, new GameFileProviderDefault(this.log, gameConfig.GameInstallPath));
            }
        }
    }
}
