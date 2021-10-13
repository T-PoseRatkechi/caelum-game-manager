// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Serilog;
    using Serilog.Core;
    using WPFLocalizeExtension.Engine;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Path to app exe.
        /// </summary>
        public static readonly string AppPath = Process.GetCurrentProcess().MainModule.FileName;

        /// <summary>
        /// Path to app log file.
        /// </summary>
        public static readonly string LogFilePath = Path.Join(Path.GetDirectoryName(AppPath), "Logs", $"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.json");

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            /*
            LocalizeDictionary.Instance.Culture = CultureInfo.CurrentCulture;
            */

            // LocalizeDictionary.Instance.Culture = new CultureInfo("es-ES");
            LocalizeDictionary.Instance.MissingKeyEvent += (object sender, MissingKeyEventArgs evt) =>
            {
                Log.Warning("String key missing! Key: {key}, Culture: {culture}", evt.Key, LocalizeDictionary.Instance.Culture.Name);
            };

            this.ConfigureLogger();
        }

        /// <summary>
        /// Gets log level controller.
        /// </summary>
        public static LoggingLevelSwitch LogLevelController { get; private set; } = new();

        /// <summary>
        /// Gets the log sink for app GUI.
        /// </summary>
        public static CustomSink AppLogSink { get; } = new();

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        /// <summary>
        /// Configures global Serilog logger.
        /// </summary>
        private void ConfigureLogger()
        {
            // Configure logger.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LogLevelController)
                .WriteTo.Sink(AppLogSink)
                .WriteTo.File(LogFilePath, outputTemplate: "<ID:{ThreadId}> ({Timestamp:HH:mm:ss}) [{Level:u3}] {Exception}{Message:j}{NewLine}{Properties}{NewLine}")
                .Enrich.WithThreadId()
                .CreateLogger();

            LogLevelController.MinimumLevel = Serilog.Events.LogEventLevel.Debug;

            var cmdArgs = Environment.GetCommandLineArgs();

            // App launched with verbose mode.
            if (cmdArgs.Contains("-verbose"))
            {
                // Set min level to verbose.
                LogLevelController.MinimumLevel = Serilog.Events.LogEventLevel.Verbose;

                // Set log level controller to new instance so logger's level can no longer be changed.
                LogLevelController = new();
            }

            Log.Debug("Logger ready");
        }
    }
}
