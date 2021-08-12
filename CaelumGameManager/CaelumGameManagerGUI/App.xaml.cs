// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using System.Windows.Threading;
    using Serilog;

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
        public static readonly string LogFilePath = $@"{Path.GetDirectoryName(AppPath)}\caelum.log";

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            // Configure logger.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Sink(AppLogSink)
                .WriteTo.File(LogFilePath, outputTemplate: "<ID:{ThreadId}> ({Timestamp:HH:mm:ss}) [{Level:u3}] {Exception}{Message:j}{NewLine}")
                .Enrich.WithThreadId()
                .CreateLogger();

            Log.Information("Caelum Game Manager starting");

            // Temp performance testing for GUI console log display.
            var dispatchTimer = new DispatcherTimer();
            int count = 0;
            dispatchTimer.Tick += (object sender, EventArgs args) =>
            {
                if (count < 5000)
                {
                    if (count > 40 && count < 44)
                    {
                        Log.Warning("Test warning {count}", count);
                    }
                    else if (count > 42 && count < 48)
                    {
                        Log.Error("Test error {count}", count);
                    }
                    else
                    {
                        Log.Information("Hello {count}", count);
                    }

                    try
                    {
                        if (count == 50)
                        {
                            int z = 0;
                            int test = 20 / z;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Exception caught");
                        dispatchTimer.Stop();
                    }

                    count++;
                }
            };

            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatchTimer.Start();
        }

        /// <summary>
        /// Gets the log sink for app GUI.
        /// </summary>
        public static CustomSink AppLogSink { get; } = new();
    }
}
