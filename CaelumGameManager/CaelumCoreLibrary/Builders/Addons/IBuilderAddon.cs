// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Addons
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Cards;

    public interface IBuilderAddon
    {
        /// <summary>
        /// Builds <paramref name="card"/> at <paramref name="outputDir"/>.
        /// </summary>
        /// <param name="card">Card to build.</param>
        /// <param name="outputDir">Directory to output card build at.</param>
        /// <param name="builtFiles">List of files that have been built.</param>
        void BuildCard(CardModel card, string outputDir, HashSet<string> builtFiles, Dictionary<string, List<string>> deckbuildLog);
    }
}
