// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using System.Collections.Generic;

    /// <summary>
    /// Metadata model.
    /// </summary>
    public class MetadataModel
    {
        /// <summary>
        /// Gets or sets card name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets card description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets authors' names list.
        /// </summary>
        public List<string> Authors { get; set; }

        /// <summary>
        /// Gets or sets card version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets url to card page.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets card type.
        /// </summary>
        public CardType Type { get; set; }

        /// <summary>
        /// Gets or sets list of supported games.
        /// </summary>
        public List<string> Games { get; set; }
    }
}
