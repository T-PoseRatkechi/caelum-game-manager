// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching
{
    using System;
    using System.Dynamic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// File patch object.
    /// </summary>
    public class GamePatch
    {
        /// <summary>
        /// Gets or sets name of game patches are for.
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// Gets or sets the patches.
        /// </summary>
        [JsonConverter(typeof(PatchJsonConverter))]
        public IPatch[] Patches { get; set; }
    }
}
