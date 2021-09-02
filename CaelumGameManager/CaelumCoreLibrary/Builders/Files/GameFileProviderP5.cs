// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Files
{
    using CaelumCoreLibrary.Configs;
    using Microsoft.Extensions.Logging;
    using System;

    public class GameFileProviderP5 : IGameFileProvider
    {
        private readonly ILogger log;
        private readonly string unpackedDir;
        private GameConfigModel gameConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameFileProviderP5"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameConfig">Game config.</param>
        /// <param name="unpackedDir">Unpacked directory.</param>
        public GameFileProviderP5(ILogger log, GameConfigModel gameConfig, string unpackedDir)
        {
            this.log = log;
            this.unpackedDir = unpackedDir;
            this.gameConfig = gameConfig;
        }

        public void AppendArchive(string archiveName, string inputFolder, string newPacName = null)
        {
            throw new NotImplementedException();
        }

        public string GetInstallGameFile(string relativeGameFile)
        {
            throw new NotImplementedException();
        }

        public string GetUnpackedGameFile(string relativeGameFile)
        {
            throw new NotImplementedException();
        }
    }
}
