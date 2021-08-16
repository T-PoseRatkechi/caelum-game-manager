// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using CaelumCoreLibrary.Cards;
    using Serilog;

    /// <summary>
    /// Simple deck builder: copy and paste, file overwriting, patching, etc.
    /// </summary>
    public class DeckBuilderSimple : IDeckBuilder
    {
        /// <inheritdoc/>
        public void Build(IInstallableCard[] deck, string outputDir)
        {
            Log.Information("Building with simple deck builder");
        }
    }
}
