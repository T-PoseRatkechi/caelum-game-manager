// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Common;
    using Newtonsoft.Json;

    /// <summary>
    /// CardModel implementation.
    /// </summary>
    public class CardModel : ICardModel
    {
        /// <inheritdoc/>
        public CardMetadataModel Metadata { get; set; }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public bool Enabled { get; set; }

        /// <inheritdoc/>
        public bool Hidden { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public string InstallFolder { get; set; }
    }
}
