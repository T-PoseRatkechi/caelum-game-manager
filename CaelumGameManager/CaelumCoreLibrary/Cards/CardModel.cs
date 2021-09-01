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
        public string CardId { get; set; }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public bool IsHidden { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public List<string> Games { get; set; }

        /// <inheritdoc/>
        public List<Author> Authors { get; set; }

        /// <inheritdoc/>
        public string Description { get; set; }

        /// <inheritdoc/>
        public string Version { get; set; }

        /// <inheritdoc/>
        public CardType Type { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public string InstallDirectory { get; set; }
    }
}
