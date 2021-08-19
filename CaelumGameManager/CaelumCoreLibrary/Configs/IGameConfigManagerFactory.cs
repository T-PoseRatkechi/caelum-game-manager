// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    /// <summary>
    /// IGameConfigManagerFactory interface.
    /// </summary>
    public interface IGameConfigManagerFactory
    {
        /// <summary>
        /// Creates and returns a new instance of <seealso cref="IGameConfigManager"/>.
        /// </summary>
        /// <param name="configFilePath">Path of config file.</param>
        /// <returns>New instance of <seealso cref="IGameConfigManager"/>.</returns>
        IGameConfigManager CreateGameConfigManager(string configFilePath);
    }
}
