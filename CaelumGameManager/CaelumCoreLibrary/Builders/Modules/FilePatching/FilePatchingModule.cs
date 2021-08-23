// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Text.Json;
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// File patching module.
    /// </summary>
    public class FilePatchingModule : IBuilderModule
    {
        private readonly ILogger log;
        private readonly IBuildLogger buildLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePatchingModule"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="buildLogger">Build logger.</param>
        public FilePatchingModule(ILogger log, IBuildLogger buildLogger)
        {
            this.log = log;
            this.buildLogger = buildLogger;
        }

        /// <summary>
        /// File patches.
        /// </summary>
        public List<GamePatch> Patches { get; } = new();

        /// <inheritdoc/>
        public void BuildCard(CardModel card, string outputDir, HashSet<string> builtCardFiles)
        {
            string cardPatchesFolder = Path.Join(card.InstallDirectory, "Data", "Patches");

            if (Directory.Exists(cardPatchesFolder))
            {
                var patchFiles = Directory.GetFiles(cardPatchesFolder, "*.json", SearchOption.AllDirectories);
                foreach (var patchFile in patchFiles)
                {
                    builtCardFiles.Add(patchFile);

                    var gamePatch = JsonSerializer.Deserialize<GamePatch>(File.ReadAllText(patchFile));

                    foreach (var patch in gamePatch.Patches)
                    {
                        if (patch is BinaryPatchFormat binaryPatch)
                        {
                            this.log.LogInformation($"{binaryPatch.File}\n{binaryPatch.Format}\n{binaryPatch.Offset}\n{binaryPatch.Data}");
                        }
                        else
                        {
                            this.log.LogInformation("Unknown patch");
                        }
                    }

                    this.Patches.Add(gamePatch);
                }

                this.log.LogDebug("Loaded {NumPatches} file patch(es) from card {CardName}", patchFiles.Length, card.Name);
            }
        }
    }
}
