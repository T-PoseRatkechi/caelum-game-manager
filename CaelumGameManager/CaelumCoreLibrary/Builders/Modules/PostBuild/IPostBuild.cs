// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.PostBuild
{
    /// <summary>
    /// PostBuild interface.
    /// </summary>
    public interface IPostBuild
    {
        /// <summary>
        /// Finalizes the build located in <paramref name="buildDir"/>.
        /// </summary>
        /// <param name="buildDir">Path to where the build is.</param>
        void FinalizeBuild(string buildDir);
    }
}