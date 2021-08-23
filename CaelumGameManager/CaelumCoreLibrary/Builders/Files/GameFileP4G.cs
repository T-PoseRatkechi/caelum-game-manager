// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Files
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameFileP4G : IGameFile
    {
        private readonly ILogger log;
        private readonly string unpackedDir;
        private readonly string gameInstallPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameFileP4G"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="unpackedDir">Directory path of the game's unpacked folder.</param>
        public GameFileP4G(ILogger log, string gameInstallPath, string unpackedDir)
        {
            this.log = log;
            this.unpackedDir = unpackedDir;
            this.gameInstallPath = gameInstallPath;
        }

        public string GetInstallGameFile(string relativeGameFile)
        {
            throw new NotImplementedException();
        }

        public string GetUnpackedGameFile(string relativeGameFile)
        {
            return Path.Join(this.unpackedDir, relativeGameFile);
        }
    }
}
