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

        public BindableCollection<string> LogLines { get; } = new();
    }
}
