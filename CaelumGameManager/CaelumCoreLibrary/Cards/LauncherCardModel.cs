﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using CaelumCoreLibrary.Common;
    using Newtonsoft.Json;

    /// <summary>
    /// Launcher card model.
    /// </summary>
    public class LauncherCardModel : ICardModel, ILauncherCardModel
    {
        /// <inheritdoc/>
        public CardMetadataModel Metadata { get; set; }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public bool Enabled { get; set; }

        /// <inheritdoc/>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets the path of the launcher exe to use, relative to the data folder.
        /// </summary>
        public string LauncherPath { get; set; }

        /// <summary>
        /// Gets or sets the launcher arguements to use when running the launcher path.
        /// </summary>
        public string LauncherArgs { get; set; }

        /// <inheritdoc/>
        public string InstallFolder { get; set; }

        /// <summary>
        /// Starts the launcher with args.
        /// </summary>
        /// <param name="gamePath">Game install path.</param>
        /// <returns>The started process.</returns>
        public Process Start(string gamePath)
        {
            string fullPath;
            if (this.LauncherPath == "${GameInstall}")
            {
                fullPath = gamePath;
            }
            else
            {
                fullPath = Path.Join(this.InstallFolder, "Data", this.LauncherPath);
            }

            ProcessStartInfo startInfo = new(fullPath);
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = this.LauncherArgs != null ? this.LauncherArgs.Replace("${GameInstall}", gamePath) : string.Empty;
            return Process.Start(startInfo);
        }
    }
}
