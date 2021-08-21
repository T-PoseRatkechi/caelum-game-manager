// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Addons
{
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateOutputBuilder
    {
        private readonly ILogger log;
        private readonly List<IBuilderAddon> builders = new();

        public CreateOutputBuilder(ILogger log)
        {
            this.log = log;
        }

        public CreateOutputBuilder UseAddon<T>()
            where T : IBuilderAddon, new()
        {

            this.builders.Add(new T());
            return this;
        }

        public void BuildCardOutput(CardModel card, string outputDir, Dictionary<string, List<string>> deckbuildLog)
        {
            HashSet<string> builtFiles = new();

            foreach (var builder in this.builders)
            {
                builder.BuildCard(card, outputDir, builtFiles, deckbuildLog);
            }

            this.log.LogDebug("Card {CardName} built with {NumFiles} files processed.", card.Name, builtFiles.Count);
        }
    }
}
