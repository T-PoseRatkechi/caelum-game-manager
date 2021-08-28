// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    using System;
    using System.IO;
    using System.Timers;
    using CaelumCoreLibrary.Writers;

    /// <summary>
    /// Manages reading, editing, and writing a <seealso cref="GameConfigModel"/> instance to file.
    /// </summary>
    public class GameConfigManager : IGameConfigManager
    {
        private readonly IWriter writer;
        private readonly string configFilePath;
        private readonly Timer saveTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConfigManager"/> class.
        /// </summary>
        /// <param name="configFilePath">Path to config file.</param>
        /// <param name="writer">Writer to use for writing and reading config.</param>
        public GameConfigManager(IWriter writer, string configFilePath)
        {
            this.writer = writer;
            this.configFilePath = configFilePath;
            this.saveTimer = new();

            this.LoadGameConfig();
            this.InitSaveTimer();
        }

        /// <inheritdoc/>
        public GameConfigModel Settings { get; private set; }

        /// <inheritdoc/>
        public void LoadGameConfig()
        {
            if (File.Exists(this.configFilePath))
            {
                this.Settings = this.writer.ParseFile<GameConfigModel>(this.configFilePath);
            }
            else
            {
                this.Settings = new GameConfigModel();
                this.SaveGameConfig();
            }
        }

        /// <inheritdoc/>
        public void SaveGameConfig()
        {
            this.saveTimer.Stop();
            this.saveTimer.Start();
        }

        private void InitSaveTimer()
        {
            // Save only every 1.5 seconds.
            this.saveTimer.Interval = 1000;
            this.saveTimer.AutoReset = false;

            this.saveTimer.Elapsed += this.SaveTimer_Elapsed;
        }

        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.writer.WriteFile(this.configFilePath, this.Settings);
        }
    }
}
