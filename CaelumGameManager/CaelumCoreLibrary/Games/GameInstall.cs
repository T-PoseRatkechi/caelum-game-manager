// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Utilities;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Base implementation of <seealso cref="IGameInstall"/>.
    /// </summary>
    public class GameInstall : IGameInstall
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameInstall"/> class.
        /// </summary>
        /// <param name="name">Name of game.</param>
        /// <param name="directory">Directory to create game install in.</param>
        public GameInstall(string name, string directory)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            this.GameName = name;
            this.BaseDirectory = CaelumFileIO.BuildDirectory(Path.Join(directory, this.GameName));
            this.CardsDirectory = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDirectory, "Cards"));
            this.DownloadsDirectory = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDirectory, "Downloads"));
            this.BuildDirectory = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDirectory, "Build"));
            this.UnpackedDirectory = CaelumFileIO.BuildDirectory(Path.Join(this.BaseDirectory, "Unpacked"));
        }

        /// <inheritdoc/>
        public string GameName { get; init; }

        /// <inheritdoc/>
        public string BaseDirectory { get; init; }

        /// <inheritdoc/>
        public string CardsDirectory { get; init; }

        /// <inheritdoc/>
        public string DownloadsDirectory { get; init; }

        /// <inheritdoc/>
        public string BuildDirectory { get; init; }

        /// <inheritdoc/>
        public string UnpackedDirectory { get; init; }
    }
}
