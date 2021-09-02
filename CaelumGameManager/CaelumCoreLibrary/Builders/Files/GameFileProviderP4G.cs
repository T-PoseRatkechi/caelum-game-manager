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
    using CaelumCoreLibrary.Cards.Converters.Aemulus;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;
    using PreappPartnersLib.FileSystems;

    /// <summary>
    /// GameFileProvider for Persona 4 Golden.
    /// </summary>
    public class GameFileProviderP4G : IGameFileProvider
    {
        /// <summary>
        /// Character denoting the unpacked contents of a file.
        /// </summary>
        public const char UnpackedFolderChar = '_';

        // TODO: Make this controllable.
        private const string P4gDataName = "data_e";

        private readonly ILogger log;
        private readonly string unpackedDir;
        private readonly GameConfigModel gameConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameFileProviderP4G"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameConfig">Game config instance.</param>
        /// <param name="unpackedDir">Directory path of the game's unpacked folder.</param>
        public GameFileProviderP4G(ILogger log, GameConfigModel gameConfig, string unpackedDir)
        {
            this.log = log;
            this.unpackedDir = unpackedDir;
            this.gameConfig = gameConfig;
        }

        /// <summary>
        /// Gets the game install directory from the game config install path.
        /// </summary>
        private string GameInstallDirectory => Path.GetDirectoryName(this.gameConfig.GameInstallPath);

        /// <inheritdoc/>
        public string GetInstallGameFile(string relativeGameFile)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetUnpackedGameFile(string relativeGameFile)
        {
            var dataFilePath = Path.Join(this.GameInstallDirectory, $"{P4gDataName}.cpk");
            var dataUnpackedDir = Path.Join(this.unpackedDir, P4gDataName);

            // Expected location of the unpacked relativeGameFile in game's Unpacked folder.
            var expectedPath = Path.Join(this.unpackedDir, relativeGameFile);

            // Return immediately if file already exists.
            if (File.Exists(expectedPath))
            {
                return expectedPath;
            }

            // Check if relativeGameFile was unpacked from a game file.
            // Example: init_free.bin which is unpacked from data00004.
            if (this.IsRootFile(relativeGameFile))
            {
                // Unpack relativeGameFile from game file.
                this.UnpackCpk(dataFilePath, dataUnpackedDir, relativeGameFile);
                return expectedPath;
            }

            // relativeGameFile is a nested file.
            // It was unpacked from another file that was unpacked from a game file.
            // Example: ENCOUNT.tbl which is unpacked from init_free.bin unpacked from data00004.

            // Get path parts for unpacking nested files.
            var pathParts = relativeGameFile.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Unpack files as needed until relativeGameFile is met.
            for (int i = 0, total = pathParts.Length; i < total; i++)
            {
                // Current part of path.
                var currentPart = pathParts[i];

                // Current path part denotes the unpacked directory of a file.
                // Example: "init_free.bin_" -> unpacked contents of "init_free.bin".
                if (currentPart.EndsWith(UnpackedFolderChar))
                {
                    // The relative path to the source file the unpacked contents/folder came from.
                    var originalRelativeFile = string.Join(@"\", pathParts[0..(i + 1)]).TrimEnd(UnpackedFolderChar);

                    // The unpacked path of originalRelativeFile.
                    var unpackedRelativeFile = Path.Join(this.unpackedDir, originalRelativeFile);

                    // Current file came from unpacked game file.
                    if (this.IsRootFile(originalRelativeFile))
                    {
                        // Unpack originalRelativeFile from game file, if missing.
                        if (!File.Exists(unpackedRelativeFile))
                        {
                            this.UnpackCpk(dataFilePath, dataUnpackedDir, originalRelativeFile);
                        }

                        // Unpack like PakPack.
                        if (!PAKFileSystem.TryOpen(unpackedRelativeFile, out var pak))
                        {
                            throw new ArgumentException($@"Invalid PAK file ""{unpackedRelativeFile}"".");
                        }

                        // Unpacked directory of originalRelativeFile.
                        var unpackedFileDir = Path.Join(this.unpackedDir, $"{originalRelativeFile}_");
                        Directory.CreateDirectory(unpackedFileDir);

                        // Unpack contents of originalRelativeFile to its unpack directory.
                        using (pak)
                        {
                            this.log.LogDebug("PAK format version: {PakVersion}", pak.Version);

                            // TODO: Look into the SearchOption.AllDirectories, possibly able to replace
                            // nested files directly.
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

                    // Current file was unpacked from a file unpacked from a game file.
                    else
                    {
                        if (Path.GetExtension(unpackedRelativeFile) == ".spr")
                        {
                            // Unpacked directory of originalRelativeFile.
                            var unpackedFileDir = Path.Join(this.unpackedDir, $"{originalRelativeFile}{UnpackedFolderChar}");
                            Directory.CreateDirectory(unpackedFileDir);

                            SprUtils sprUtils = new(this.log);
                            sprUtils.ExtractTmx(unpackedRelativeFile, unpackedFileDir);
                        }
                        else
                        {
                            // Unpack like PakPack.
                            if (!PAKFileSystem.TryOpen(unpackedRelativeFile, out var pak))
                            {
                                throw new ArgumentException($@"Invalid PAK file ""{unpackedRelativeFile}"".");
                            }

                            // Unpacked directory of originalRelativeFile.
                            var unpackedFileDir = Path.Join(this.unpackedDir, $"{originalRelativeFile}{UnpackedFolderChar}");
                            Directory.CreateDirectory(unpackedFileDir);

                            // Unpack contents of originalRelativeFile to its unpack directory.
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
            }

            // Verify that expected file exists.
            if (!File.Exists(expectedPath))
            {
                throw new FileNotFoundException($@"Failed to find unpacked game file ""{relativeGameFile}"".", expectedPath);
            }

            return expectedPath;
        }

        // Praise be to TGE.

        /// <inheritdoc/>
        public void AppendArchive(string archiveName, string inputFolder, string newPacName = null)
        {
            var installArchivePath = Path.Join(this.GameInstallDirectory, $"{archiveName}.cpk");
            var backupInstallPath = Path.Join(this.unpackedDir, $"{archiveName}.cpk");

            // Make a backup of the original archive.
            if (!File.Exists(backupInstallPath))
            {
                File.Copy(installArchivePath, backupInstallPath);
            }

            // Overwrite the current install archive with the original backup.
            else
            {
                File.Copy(backupInstallPath, installArchivePath, true);
            }

            // Work on the archive in the install.
            CpkFile cpk = new(installArchivePath);

            var pacName = newPacName;
            var newPacIndex = newPacName == null ? 7 : Convert.ToInt32(Path.GetFileNameWithoutExtension(newPacName).Remove(0, 4).TrimStart('0'));

            if (pacName == null)
            {
                // Get highested pac index from install data pacs.
                newPacIndex = Directory.GetFiles(this.GameInstallDirectory, "data*****.pac", SearchOption.TopDirectoryOnly)
                    .Select(x =>
                    {
                        var result = Path.GetFileNameWithoutExtension(x).Remove(0, 4).TrimStart('0');
                        if (string.IsNullOrEmpty(result))
                        {
                            return 0;
                        }
                        else
                        {
                            return Convert.ToInt32(result);
                        }
                    }).Max() + 1;

                // Create name from pac index.
                pacName = this.FormatPacName($"{this.GetPacBaseNameFromCpkBaseName(installArchivePath, archiveName)}", newPacIndex);
            }

            var pacPath = Path.Join(this.GameInstallDirectory, pacName);

            this.log.LogDebug("Appending\nContents of: {FolderPath}\nAs: {NewPac}\nTo: {Archive}", inputFolder, pacName, archiveName);

            int filesAdded = 0;

            // Broken, doesn't close stream?
            // Think it's related to Parallel.ForEach in AddFiles.
            var pac = DwPackFile.Pack(inputFolder, newPacIndex, false, e =>
            {
                // Can cause GUI log window to crash.
                // TODO: Fix...
                // this.log.LogDebug($"Adding {e}");
                filesAdded++;
                return true;
            });

            this.log.LogDebug("Added {NumFiles} files to {PacName}", filesAdded, pacName);

            using var packFile = File.Create(pacPath);
            pac.Write(packFile, false, e => this.log.LogDebug("Writing {FilePath}", e.Path));

            foreach (var entry in pac.Entries)
            {
                entry.CloseStreams();
            }

            // Add entries to CPK.
            for (var i = 0; i < pac.Entries.Count; i++)
            {
                var entry = pac.Entries[i];
                cpk.Entries.Add(new CpkFileEntry(entry.Path, (short)i, (short)newPacIndex));
            }

            // Update data_x.cpk.
            var archiveStream = File.Create(installArchivePath);
            cpk.Write(archiveStream);

            packFile.Flush();
            packFile.Close();
            archiveStream.Flush();
            archiveStream.Close();
        }

        /// <summary>
        /// Unpacks cpk. Set <paramref name="relativePath"/> to unpack a specific file.
        /// </summary>
        /// <param name="inputFile">Input file.</param>
        /// <param name="outputDir">Where to output files.</param>
        /// <param name="relativePath">Specific file to unpack.</param>
        private void UnpackCpk(string inputFile, string outputDir, string relativePath = null)
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

            bool fileFound = false;
            var relativePathNoDataName = relativePath.Replace(P4gDataName, string.Empty).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            cpk.Unpack(packs, outputDir, e =>
            {
                // Wow, only getting the file needed is so fast!
                if (e.Path == relativePathNoDataName)
                {
                    // if (!ShouldUnpack(e.Path)) return false;
                    // this.log.LogDebug($"Extracting {e.Path} (pac: {e.PacIndex}, file: {e.FileIndex})"); // Will crash GUI.
                    this.log.LogDebug($"Extracting {e.Path} (pac: {e.PacIndex}, file: {e.FileIndex})");
                    fileFound = true;
                    return true;
                }

                return false;
            });

            if (!fileFound)
            {
                throw new ArgumentException($@"Root game file ""{relativePath}"" could not be found.");
            }
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
