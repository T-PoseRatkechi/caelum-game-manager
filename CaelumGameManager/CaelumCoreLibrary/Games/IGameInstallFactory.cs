// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Games
{
    /// <summary>
    /// Interface for create game installs.
    /// </summary>
    public interface IGameInstallFactory
    {
        /// <summary>
        /// Creates a new <seealso cref="IGameInstall"/> instance.
        /// </summary>
        /// <param name="name">Game name.</param>
        /// <returns>New <seealso cref="IGameInstall"/> instance.</returns>
        IGameInstall CreateGameInstall(string name);
    }
}