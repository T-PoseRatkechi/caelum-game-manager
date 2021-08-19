// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using System.Collections.Generic;
    using CaelumCoreLibrary.Common;
    using Serilog;

    /// <summary>
    /// Base implementation for <seealso cref="IDeckBuilderFactory"/>.
    /// </summary>
    public class DeckBuilderFactory : IDeckBuilderFactory
    {
        private static readonly Dictionary<string, IDeckBuilder> DeckBuilders = new()
        {
            { "simple", new DeckBuilderSimple() },
            { "persona4golden", new DeckBuilderP4G() },
        };

        private ILogger log = Log.Logger.WithCallerSyntax();

        /// <inheritdoc/>
        public IDeckBuilder CreateDeckBuilderByName(string builderName)
        {
            if (string.IsNullOrEmpty(builderName))
            {
                return null;
            }

            if (DeckBuilders.ContainsKey(builderName))
            {
                this.log.Debug("Using deck builder {DeckBuilderName}", builderName);
                return DeckBuilders[builderName];
            }
            else
            {
                this.log.Error("Unknown deck builder {DeckBuilderName}", builderName);
                return null;
            }
        }
    }
}
