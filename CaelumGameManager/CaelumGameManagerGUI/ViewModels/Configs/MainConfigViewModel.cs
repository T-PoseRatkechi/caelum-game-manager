// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels.Configs
{
    using System.Windows.Input;

    /// <summary>
    /// MainConfigVM.
    /// </summary>
    public class MainConfigViewModel
    {
        public MainConfigViewModel()
        {
        }

        /// <summary>
        /// Gets a value indicating whether debug mode is enabled.
        /// </summary>
        public bool DebugEnabled
        {
            get
            {
                return App.LogLevelController.MinimumLevel == Serilog.Events.LogEventLevel.Debug;
            }
        }

        /// <summary>
        /// Gets command for enabling or disabling debug mode.
        /// </summary>
        public ICommand EnableDebug { get; } = new EnableDebugCommand();
    }
}
