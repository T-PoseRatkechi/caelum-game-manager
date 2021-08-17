// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs
{
    /// <summary>
    /// Interface for CaelumConfig.
    /// </summary>
    public interface ICaelumConfig
    {
        /// <summary>
        /// Gets the tool directory.
        /// </summary>
        string ToolsDirectory { get; }

        /// <summary>
        /// Gets the games directory.
        /// </summary>
        string GamesDirectory { get; }

        /// <summary>
        /// Gets the config directory.
        /// </summary>
        public string ConfigDirectory { get; }

        /// <summary>
        /// Gets the authors directory.
        /// </summary>
        public string AuthorsDirectory { get; }
    }
}
