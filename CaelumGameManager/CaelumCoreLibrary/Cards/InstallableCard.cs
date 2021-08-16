﻿// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using CaelumCoreLibrary.Common;

    /// <summary>
    /// Base implementation for installable cards.
    /// </summary>
    public class InstallableCard : IInstallableCard
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public string InstallDirectory { get; set; }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public string CardId { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string[] Games { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public List<Author> Authors { get; set; } = new();

        /// <inheritdoc/>
        public string Description { get; set; }

        /// <inheritdoc/>
        public string Version { get; set; }

        /// <inheritdoc/>
        public CardType Type { get; set;  }
    }
}
