// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels.Configs
{
    using System;
    using System.Windows.Input;
    using CaelumCoreLibrary.Configs;

    /// <summary>
    /// Command for enabling debug mode.
    /// </summary>
    public class EnableDebugCommand : ICommand
    {
        private readonly IGameConfigManager gameConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnableDebugCommand"/> class.
        /// </summary>
        /// <param name="gameConfig">Game config manager.</param>
        public EnableDebugCommand(IGameConfigManager gameConfig)
        {
            this.gameConfig = gameConfig;
        }

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
            // Toggle debug mode.
            App.LogLevelController.MinimumLevel = this.gameConfig.Settings.ShowDebugMessages ? Serilog.Events.LogEventLevel.Information : Serilog.Events.LogEventLevel.Debug;

            // Toggle debug mode in game config.
            this.gameConfig.Settings.ShowDebugMessages = !this.gameConfig.Settings.ShowDebugMessages;

            // Save changes to game config file.
            this.gameConfig.SaveGameConfig();
        }
    }
}
