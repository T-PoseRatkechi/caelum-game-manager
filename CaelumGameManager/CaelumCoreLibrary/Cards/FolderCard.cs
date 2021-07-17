// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    /// <summary>
    /// Folder card implementation.
    /// </summary>
    public class FolderCard : BaseCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderCard"/> class.
        /// </summary>
        /// <param name="cardPath">Path to card.</param>
        public FolderCard(string cardPath)
            : base(cardPath, CardType.Folder)
        {
        }
    }
}
