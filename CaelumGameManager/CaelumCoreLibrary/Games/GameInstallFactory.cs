// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using CaelumCoreLibrary.Configs;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base implementation of <seealso cref="IGameInstallFactory"/>.
    /// </summary>
    public class GameInstallFactory : IGameInstallFactory
    {
        private readonly ILogger log;
        private readonly ICaelumConfig caelumConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstallFactory"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="caelumConfig">Caelum config.</param>
        public GameInstallFactory(ILogger log, ICaelumConfig caelumConfig)
        {
            this.log = log;
            this.caelumConfig = caelumConfig;
        }

        /// <inheritdoc/>
        public IGameInstall CreateGameInstall(string name)
        {
            this.log.LogDebug("Creating game install with name {GameName}", name);
            return new GameInstall(name, this.caelumConfig.GamesDirectory);
        }
    }
}
