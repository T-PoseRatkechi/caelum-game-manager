// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using Caliburn.Micro;
    using Serilog;

    /// <summary>
    /// Log VM.
    /// </summary>
    public class LogViewModel : Screen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogViewModel"/> class.
        /// </summary>
        public LogViewModel()
        {
            App.AppLogSink.LogReceived += (object sender, string s) =>
            {
                this.LogLines.Add(s);
                if (this.LogLines.Count > 500)
                {
                    this.LogLines.RemoveAt(0);
                }
            };
        }

        /// <summary>
        /// Gets or sets the currently selected log line.
        /// </summary>
        public string SelectedLine { get; set; }

        /// <summary>
        /// Gets log lines.
        /// </summary>
        public BindableCollection<string> LogLines { get; } = new();

        /// <summary>
        /// Log context menu.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="command">String command.</param>
        public void OpenLogContext(object sender, string command = null)
        {
            try
            {
                switch (command)
                {
                    case "openLog":
                        var proc = new System.Diagnostics.Process();
                        proc.StartInfo.FileName = App.LogFilePath;
                        proc.StartInfo.UseShellExecute = true;
                        proc.Start();
                        break;
                    case "copy":
                        if (this.SelectedLine != null)
                        {
                            Clipboard.SetText(this.SelectedLine);
                        }

                        break;
                    default:
                        Log.Warning("Log context menu received unknown command! Command: {command}", command);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Log context menu command failed! Command: {command}", command);
            }
        }
    }
}
