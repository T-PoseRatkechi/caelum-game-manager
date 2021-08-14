// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    using CaelumCoreLibrary.Configs.Writers;

    /// <summary>
    /// Base interface for config files.
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// Gets config the manager controls.
        /// </summary>
        IConfig Config { get; init; }

        /// <summary>
        /// Gets <seealso cref="IWriter"/> to use for writing config.
        /// </summary>
        IWriter ConfigWriter { get; init; }

        /// <summary>
        /// Saves <see cref="Config"/>.
        /// </summary>
        void SaveConfig();
    }
}
