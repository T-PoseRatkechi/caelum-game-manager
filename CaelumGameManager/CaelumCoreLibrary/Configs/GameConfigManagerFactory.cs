// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    using CaelumCoreLibrary.Writers;

    /// <summary>
    /// Default implementatino of <seealso cref="IGameConfigManagerFactory"/>.
    /// </summary>
    public class GameConfigManagerFactory : IGameConfigManagerFactory
    {
        private readonly IWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConfigManagerFactory"/> class.
        /// </summary>
        /// <param name="writer">Writer to use for files..</param>
        public GameConfigManagerFactory(IWriter writer)
        {
            this.writer = writer;
        }

        /// <inheritdoc/>
        public IGameConfigManager CreateGameConfigManager(string configFilePath)
        {
            return new GameConfigManager(this.writer, configFilePath);
        }
    }
}
