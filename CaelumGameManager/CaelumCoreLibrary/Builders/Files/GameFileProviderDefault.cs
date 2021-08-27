// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Files
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;

    public class GameFileProviderDefault : IGameFileProvider
    {
        private readonly ILogger log;
        private readonly string gameInstallPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameFileProviderDefault"/> class.
        /// </summary>
        /// <param name="log">Logger</param>
        /// <param name="gameInstallPath">Game install path.</param>
        public GameFileProviderDefault(ILogger log, string gameInstallPath)
        {
            this.log = log;
            this.gameInstallPath = gameInstallPath;
        }

        public void AppendArchive(string archiveName, string inputFolder)
        {
            throw new NotImplementedException();
        }

        public string GetGameFile(string gameFilePath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetInstallGameFile(string relativeGameFile)
        {
            return Path.Join(this.gameInstallPath, relativeGameFile);
        }

        /// <inheritdoc/>
        public string GetUnpackedGameFile(string relativeGameFile)
        {
            throw new NotImplementedException();
        }
    }
}
