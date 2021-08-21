// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// File patching module.
    /// </summary>
    public class FilePatchingModule : IBuilderModule
    {
        /// <inheritdoc/>
        public DeckBuildLogger BuildLogger { get; init; }

        /// <inheritdoc/>
        public void BuildCard(CardModel card, string outputDir, HashSet<string> builtCardFiles)
        {

        }
    }
}
