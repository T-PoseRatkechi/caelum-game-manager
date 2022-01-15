// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Cards;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// File patching module.
    /// </summary>
    public class FilePatchingModule : IBuilderModule
    {
        private const string UnpackedGameFilesString = "${UnpackedGameFiles}";
        private const string GameInstallString = "${GameInstall}";

        private readonly ILogger log;
        private readonly IBuildLogger buildLogger;
        private readonly IGameFileProvider gameFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePatchingModule"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="buildLogger">Build logger.</param>
        public FilePatchingModule(ILogger log, IBuildLogger buildLogger, IGameFileProvider gameFile)
        {
            this.log = log;
            this.buildLogger = buildLogger;
            this.gameFile = gameFile;
        }

        /// <inheritdoc/>
        public void BuildCard(ICardModel card, string outputDir, HashSet<string> builtCardFiles)
        {
            // Card patches dir.
            string cardPatchesFolder = Path.Join(card.InstallFolder, "Data", "Patches");

            if (Directory.Exists(cardPatchesFolder))
            {
                // Get all patches files.
                var patchFiles = Directory.GetFiles(cardPatchesFolder, "*.json", SearchOption.AllDirectories);
                foreach (var patchFile in patchFiles)
                {
                    // Add patch file to built files.
                    builtCardFiles.Add(patchFile);

                    // Parse patch file.
                    var gamePatch = JsonConvert.DeserializeObject<GamePatch>(File.ReadAllText(patchFile));

                    // Apply patches.
                    foreach (var patch in gamePatch.Patches)
                    {
                        // The relative file path, sans variable
                        string relativeGameFile = null;

                        // The expected path to its output equivalent.
                        string expectedOutputFile = null;

                        // The path to the actual local file.
                        string actualGameFile = null;

                        // Set the relative and actual paths to file.
                        if (patch.File.StartsWith(UnpackedGameFilesString))
                        {
                            relativeGameFile = patch.File.Replace(UnpackedGameFilesString, string.Empty);
                            actualGameFile = this.gameFile.GetUnpackedGameFile(relativeGameFile);
                        }
                        else if (patch.File.StartsWith(GameInstallString))
                        {
                            relativeGameFile = patch.File.Replace(GameInstallString, string.Empty);
                            actualGameFile = this.gameFile.GetInstallGameFile(relativeGameFile);
                        }
                        else
                        {
                            // Require that the path include one of the variables.
                            throw new ArgumentException($@"Unrecognized file path ""{patch.File}"" in patch file ""{patchFile}"".");
                        }

                        // Set the expected output path.
                        expectedOutputFile = Path.Join(outputDir, relativeGameFile);

                        // Copy actual file to output if missing.
                        if (!File.Exists(expectedOutputFile))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(expectedOutputFile));
                            File.Copy(actualGameFile, expectedOutputFile);
                        }

                        // TODO: Possibly improve performance by grouping patches per file
                        // and opening and closing streams once per file/card instead per each patch.
                        patch.ApplyPatch(expectedOutputFile);

                        this.buildLogger.LogOutputFile(card, expectedOutputFile);
                        // this.log.LogDebug("Patch applied.\nFormat: {PatchType}\nFile: {File}\nComment: {PatchComment}", patch.Format, patch.File, patch.Comment);
                    }
                }

                this.log.LogDebug("Loaded {NumPatches} file patch(es) from card {CardName}", patchFiles.Length, card.Metadata.Name);
            }
        }
    }
}
