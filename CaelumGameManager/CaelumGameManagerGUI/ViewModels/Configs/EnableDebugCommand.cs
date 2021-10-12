// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels.Configs
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Command for enabling debug mode.
    /// </summary>
    public class EnableDebugCommand : ICommand
    {
        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            App.LogLevelController.MinimumLevel = (App.LogLevelController.MinimumLevel == Serilog.Events.LogEventLevel.Debug) ? Serilog.Events.LogEventLevel.Information : Serilog.Events.LogEventLevel.Debug;
        }
    }
}
