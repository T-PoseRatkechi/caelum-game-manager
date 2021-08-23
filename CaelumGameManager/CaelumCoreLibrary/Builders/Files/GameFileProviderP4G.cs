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
    using AtlusFileSystemLibrary.Common.IO;
    using AtlusFileSystemLibrary.FileSystems.PAK;
    using Microsoft.Extensions.Logging;
    using PreappPartnersLib.FileSystems;

    /// <summary>
    /// GameFileProvider for Persona 4 Golden.
    /// </summary>
    public class GameFileProviderP4G : IGameFileProvider
    {
        private const char UnpackedFolderChar = '_';

        private readonly ILogger log;
        private readonly string unpackedDir;
        private readonly string gameInstallPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameFileProviderP4G"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameInstallPath">Game isntall path.</param>
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
            var dataE = Path.Join(this.gameInstallPath, "data_e.cpk");

            // Expected path of the unpacked file.
            var expectedPath = Path.Join(this.unpackedDir, relativeGameFile);

            if (File.Exists(expectedPath))
            {
                return expectedPath;
            }

            if (this.IsRootFile(relativeGameFile))
            {
                if (!File.Exists(expectedPath))
                {
                    this.UnpackCpk(dataE, relativeGameFile);
                }

                return expectedPath;
            }

            // Handle nested files, unpacking as needed.
            var pathParts = relativeGameFile.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            for (int i = 0, total = pathParts.Length; i < total; i++)
            {
                var currentPart = pathParts[i];

                // Path denotes the unpacked directory of a file.
                if (currentPart.EndsWith('_'))
                {
                    var originalRelativeFile = string.Join(@"\", pathParts[0..(i + 1)]).TrimEnd('_');
                    var unpackedRelativeFile = Path.Join(this.unpackedDir, originalRelativeFile);

                    // Current file came from unpacked game file.
                    if (this.IsRootFile(originalRelativeFile))
                    {
                        if (!File.Exists(unpackedRelativeFile))
                        {
                            this.UnpackCpk(dataE, originalRelativeFile);
                        }

                        // Unpack like PakPack
                        if (!PAKFileSystem.TryOpen(unpackedRelativeFile, out var pak))
                        {
                            throw new ArgumentException($@"Invalid PAK file ""{unpackedRelativeFile}"".");
                        }

                        var unpackedFileDir = Path.Join(this.unpackedDir, $"{originalRelativeFile}_");
                        Directory.CreateDirectory(unpackedFileDir);

                        using (pak)
                        {
                            this.log.LogDebug("PAK format version: {PakVersion}", pak.Version);
                            foreach (string file in pak.EnumerateFiles())
                            {
                                var normalizedFilePath = file.Replace("../", string.Empty); // Remove backwards relative path
                                using (var stream = FileUtils.Create(unpackedFileDir + Path.DirectorySeparatorChar + normalizedFilePath))
                                using (var inputStream = pak.OpenFile(file))
                                {
                                    // Console.WriteLine($"Extracting {file}");
                                    this.log.LogDebug($"Extracting {file}");
                                    inputStream.CopyTo(stream);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Unpack like PakPack
                        if (!PAKFileSystem.TryOpen(unpackedRelativeFile, out var pak))
                        {
                            throw new ArgumentException($@"Invalid PAK file ""{unpackedRelativeFile}"".");
                        }

                        var unpackedFileDir = Path.Join(this.unpackedDir, $"{originalRelativeFile}_");

                        using (pak)
                        {
                            this.log.LogDebug("PAK format version: {PakVersion}", pak.Version);
                            foreach (string file in pak.EnumerateFiles())
                            {
                                var normalizedFilePath = file.Replace("../", string.Empty); // Remove backwards relative path
                                using (var stream = FileUtils.Create(unpackedFileDir + Path.DirectorySeparatorChar + normalizedFilePath))
                                using (var inputStream = pak.OpenFile(file))
                                {
                                    // Console.WriteLine($"Extracting {file}");
                                    this.log.LogDebug($"Extracting {file}");
                                    inputStream.CopyTo(stream);
                                }
                            }
                        }
                    }
                }
            }

            // Unpack file if missing.
            if (!File.Exists(expectedPath))
            {
                this.UnpackCpk(dataE, relativeGameFile);
            }

            return expectedPath;
        }

        // Praise be to TGE.

        /// <summary>
        /// Unpacks cpk. Set <paramref name="relativePath"/> to unpack a specific file.
        /// </summary>
        /// <param name="inputFile">Input file.</param>
        /// <param name="relativePath">Specific file to unpack.</param>
        private void UnpackCpk(string inputFile, string relativePath = null)
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

        /// <summary>
        /// Indicates whether <paramref name="file"/> is a root file (unpacked directly from game file).
        /// </summary>
        /// <param name="file">File to check.</param>
        /// <returns>Whether <paramref name="file"/> is root file.</returns>
        private bool IsRootFile(string file)
        {
            var fileParts = file.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            foreach (var part in fileParts)
            {
                if (part.EndsWith(UnpackedFolderChar))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
