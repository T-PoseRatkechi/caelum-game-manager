// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

// preappfile and PreappPartnersLib by TGEnigma.
// https://github.com/TGEnigma/preappfile
// Literally solo-carring Persona modding.
namespace CaelumCoreLibrary.Builders.Files
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using PreappPartnersLib.FileSystems;

    /// <summary>
    /// GameFileProvider for Persona 4 Golden.
    /// </summary>
    public class GameFileProviderP4G : IGameFileProvider
    {
        private readonly ILogger log;
        private readonly string unpackedDir;
        private readonly string gameInstallPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameFileProviderP4G"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="unpackedDir">Directory path of the game's unpacked folder.</param>
        public GameFileProviderP4G(ILogger log, string gameInstallPath, string unpackedDir)
        {
            this.log = log;
            this.unpackedDir = unpackedDir;
            this.gameInstallPath = gameInstallPath;
        }

        /// <inheritdoc/>
        public string GetInstallGameFile(string relativeGameFile)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetUnpackedGameFile(string relativeGameFile)
        {
            // Expected path of the unpacked file.
            var expectedPath = Path.Join(this.unpackedDir, relativeGameFile);

            // Unpack file if missing.
            if (!File.Exists(expectedPath))
            {
                var dataE = Path.Join(this.gameInstallPath, "data_e.cpk");
                this.UnpackCpk(dataE, relativeGameFile);
            }

            return expectedPath;
        }

        // Praise be to TGE.
        private void UnpackCpk(string inputFile, string relativePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(inputFile);
            var dir = Path.GetDirectoryName(inputFile);

            // Try to detect pac base name
            var pacBaseName = this.GetPacBaseNameFromCpkBaseName(dir, fileName);
            var cpk = new CpkFile(inputFile);

            // Load needed pacs
            var packs = new List<DwPackFile>();
            foreach (var pacIdx in cpk.Entries.Select(x => x.PacIndex).Distinct().OrderBy(x => x))
            {
                var pacName = $"{pacBaseName}{pacIdx:D5}.pac";
                var pacPath = Path.Combine(dir, pacName);

                if (!File.Exists(pacPath))
                {
                    throw new FileNotFoundException("Failed to unpack {InputFile} because {PacName} is missing.", pacPath);
                }

                var pac = new DwPackFile(pacPath);
                var refFileCount = cpk.Entries.Where(x => x.PacIndex == pacIdx)
                    .Select(x => x.FileIndex)
                    .Max() + 1;

                if (refFileCount > pac.Entries.Count)
                {
                    throw new ArgumentException($"Failed to unpack: CPK references {refFileCount} in {pacName} but only {pac.Entries.Count} exist.");
                }

                packs.Add(pac);
            }

            cpk.Unpack(packs, this.unpackedDir, e =>
            {
                // Wow, only getting the file needed is so fast!
                if (e.Path == relativePath)
                {
                    // if (!ShouldUnpack(e.Path)) return false;
                    // this.log.LogDebug($"Extracting {e.Path} (pac: {e.PacIndex}, file: {e.FileIndex})"); // Will crash GUI.
                    this.log.LogDebug($"Extracting {e.Path} (pac: {e.PacIndex}, file: {e.FileIndex})");
                    return true;
                }

                return false;
            });
        }

        private string GetPacBaseNameFromCpkBaseName(string dir, string baseName)
        {
            var pacBaseName = baseName;
            if (!File.Exists(Path.Combine(dir, this.FormatPacName(pacBaseName, 0))))
            {
                // Trim language suffix: _e, _c, _k
                var start = pacBaseName.IndexOf('_');
                if (start != -1)
                {
                    pacBaseName = pacBaseName.Substring(0, start);
                }
            }

            return pacBaseName;
        }

        private string FormatPacName(string baseName, int index)
        {
            return $"{baseName}{index:D5}.pac";
        }
    }
}
