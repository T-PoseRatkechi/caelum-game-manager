// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    using System;
    using CaelumCoreLibrary.Configs.Writers;

    /// <summary>
    /// Base Config Manager implementation.
    /// </summary>
    public class ConfigManager : IConfigManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="config">Config instance to manage.</param>
        /// <param name="writer">Writer to use for writing <paramref name="config"/> to file.</param>
        public ConfigManager(IConfig config, IWriter writer)
        {
            this.Config = config;
            this.ConfigWriter = writer;
        }

        /// <inheritdoc/>
        public IConfig Config { get; init; }

        /// <inheritdoc/>
        public IWriter ConfigWriter { get; init; }

        /// <inheritdoc/>
        public void SaveConfig()
        {
            this.ConfigWriter.WriteFile(this.Config.ConfigFilePath, this.Config);
        }
    }
}
