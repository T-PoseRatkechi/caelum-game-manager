// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

using System.Diagnostics;

namespace CaelumCoreLibrary.Games.Launchers
{
    /// <summary>
    /// Game launcher model.
    /// </summary>
    public class GameLauncherModel
    {
        /// <summary>
        /// Gets or sets the path to game launcher.
        /// </summary>
        public string LauncherPath { get; set; }

        /// <summary>
        /// Gets or sets the arguements to open launcher with.
        /// </summary>
        public string LauncherArgs { get; set; }

        /// <summary>
        /// Starts the launcher with args.
        /// </summary>
        public void Start()
        {
            ProcessStartInfo startInfo = new(this.LauncherPath);
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = this.LauncherArgs != null ? this.LauncherArgs : string.Empty;
            var proc = Process.Start(startInfo);
            proc.WaitForExit();
        }
    }
}
