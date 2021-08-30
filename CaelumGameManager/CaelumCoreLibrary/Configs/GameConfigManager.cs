// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    using System.IO;
    using System.Timers;
    using CaelumCoreLibrary.Games.Launchers;
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

            this.ValidateConfig();
        }

        /// <inheritdoc/>
        public void SaveGameConfig()
        {
            this.saveTimer.Stop();
            this.saveTimer.Start();
        }

        /// <summary>
        /// Validates and corrects any issues with the config.
        /// </summary>
        private void ValidateConfig()
        {
            // Set game launchers prop to empty list if null.
            if (this.Settings.GameLaunchers == null)
            {
                this.Settings.GameLaunchers = new();
            }

            // Add default game launcher if missing.
            if (this.Settings.GameLaunchers.Count == 0)
            {
                this.Settings.GameLaunchers.Add(new GameLauncherModel()
                {
                    LauncherName = "Default",
                    LauncherPath = "${GameInstall}",
                    LauncherArgs = null,
                });
            }

            // Set default game launcher to first launcher if exceeds available launchers.
            if (this.Settings.DefaultGameLauncher > this.Settings.GameLaunchers.Count)
            {
                this.Settings.DefaultGameLauncher = 0;
                // this.log
            }
        }

        /// <summary>
        /// Initializes the save timer.
        /// </summary>
        private void InitSaveTimer()
        {
            // Save only every 1.5 seconds.
            this.saveTimer.Interval = 1000;
            this.saveTimer.AutoReset = false;

            this.saveTimer.Elapsed += this.SaveTimer_Elapsed;
        }

        // Possibly should be a front-end thing?
        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.writer.WriteFile(this.configFilePath, this.Settings);
        }
    }
}
