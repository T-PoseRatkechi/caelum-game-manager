// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.PostBuild
{
    using System;
    using System.IO;
    using AtlusFileSystemLibrary;
    using AtlusFileSystemLibrary.FileSystems.PAK;
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Cards.Converters.Aemulus;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Post builder for P4G output.
    /// </summary>
    public class PostBuildP4G : IPostBuild
    {
        /// <summary>
        /// The character representing a folder is the unpacked contants of a file.
        /// </summary>
        private const char UnpackedFolderChar = GameFileProviderP4G.UnpackedFolderChar;

        private readonly ILogger log;
        private readonly IBuildLogger buildLogger;
        private readonly IGameFileProvider gameFileProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostBuildP4G"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="buildLogger">Build logger.</param>
        /// <param name="gameFileProvider">Game file provider.</param>
        public PostBuildP4G(ILogger log, IBuildLogger buildLogger, IGameFileProvider gameFileProvider)
        {
            this.log = log;
            this.buildLogger = buildLogger;
            this.gameFileProvider = gameFileProvider;
        }

        /// <inheritdoc/>
        public void FinalizeBuild(string buildDir)
        {
            this.Merge(buildDir);
        }

        /// <summary>
        /// File merging.
        /// </summary>
        private void Merge(string buildDir)
        {
            this.log.LogDebug("Running post build merge.");
            var unpackedDirs = Directory.GetDirectories(buildDir, $"*{UnpackedFolderChar}", SearchOption.AllDirectories);

            // Copy required files for merging.
            // WARNING: This assumes any folder with a name ending in an underscore should be
            // the contents of an unpacked game file.
            foreach (var unpackedDir in unpackedDirs)
            {
                var buildFilePath = unpackedDir.TrimEnd(UnpackedFolderChar);
                var relativeFilePath = buildFilePath.Replace(buildDir, string.Empty).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var unpackedFile = this.gameFileProvider.GetUnpackedGameFile(relativeFilePath);

                // Copy file from unpacked files to build dir if a copy doesn't already exists in build dir.
                if (!File.Exists(buildFilePath))
                {
                    File.Copy(unpackedFile, buildFilePath);
                }
            }

            this.ProcessAppendFolder(buildDir);

            // Merge files from unpacked dirs into their source file
            // from lowest layer until the root layer.
            this.RecursiveFileMerge(buildDir, buildDir);

            // Clean up empty folders.
            this.ProcessDirectory(buildDir);
        }

        /// <summary>
        /// Process then deletes the preappfile append folder.
        /// </summary>
        /// <param name="buildDir">Build directory.</param>
        private void ProcessAppendFolder(string buildDir)
        {
            const string newPacName = "data00007.pac";

            var dataFolders = Directory.GetDirectories(buildDir, "data_*", SearchOption.TopDirectoryOnly);

            /*
            // Only allow one data_x card.
            if (dataDirs.Length > 1)
            {
                throw new ArgumentException($@"More than one data_x folder was found in card ""{card.CardId}"".");
            }
            */

            foreach (var dataDir in dataFolders)
            {
                var appendDir = Path.Join(dataDir, "append");
                if (!Directory.Exists(appendDir))
                {
                    continue;
                }

                var archiveToAppend = Path.GetFileName(dataDir);

                this.gameFileProvider.AppendArchive(archiveToAppend, appendDir, newPacName);

                // preappfile's Pack is still using files even though seemingly finished Pack
                // Stream not getting closed or weirdness with Parallel.ForEach?
                Directory.Delete(appendDir, true);
            }
        }

        /// <summary>
        /// Deletes any empty folder located in <paramref name="startLocation"/>.
        /// </summary>
        /// <param name="startLocation">Starting folder.</param>
        private void ProcessDirectory(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                this.ProcessDirectory(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        /// <summary>
        /// Recursively merges files in unpacked folders into their source file.
        /// Might stack overflow if nested too much; hope it doesn't though...
        /// </summary>
        /// <param name="folder">Current folder in merge.</param>
        /// <param name="buildDir">Final build directory.</param>
        private void RecursiveFileMerge(string folder, string buildDir)
        {
            // Gets if any folders or sub-folders are unpacked contents of files that will need merging.
            string[] unpackedFolders = Directory.GetDirectories(folder, $"*{UnpackedFolderChar}", SearchOption.AllDirectories);

            if (unpackedFolders.Length > 0)
            {
                // Run recursively BUT only in the top directories to keep from duplicate folder merges.
                foreach (var subfolder in Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly))
                {
                    this.RecursiveFileMerge(subfolder, buildDir);
                }
            }

            // Currently in a files unpacked folder.
            if (folder.EndsWith(UnpackedFolderChar))
            {
                // The source file for this folder is folder sans the last character.
                // Example: Folder "./Build Directory/init.bin_" has a source file of "./Build Directory/init.bin"
                var sourceFile = folder.TrimEnd(UnpackedFolderChar);

                // Path to save edited file.
                var tempSavePath = $"{sourceFile}.temp";

                // Merge files with SprUtils Insert.
                if (Path.GetExtension(sourceFile) == ".spr")
                {
                    // Merge all tmx files in folder into source spr file.
                    foreach (var file in Directory.GetFiles(folder, "*.tmx", SearchOption.TopDirectoryOnly))
                    {
                        SprUtils sprUtils = new(this.log);

                        sprUtils.InsertTmxFast(sourceFile, file, tempSavePath);

                        // Overwrite original source file with merged temp file.
                        File.Move(tempSavePath, sourceFile, true);

                        this.log.LogDebug("Merged {TmxName} into {SprName}", Path.GetFileName(file), Path.GetFileName(sourceFile));
                    }
                }

                // Handle merging non-spr files.
                else
                {
                    // Open file.
                    if (!PAKFileSystem.TryOpen(sourceFile, out var pak))
                    {
                        throw new ArgumentException($@"Invalid PAK file ""{sourceFile}"".");
                    }

                    // Merge any files in current folder into the source file.
                    using (pak)
                    {
                        foreach (var file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
                        {
                            var partialPath = file.Replace(folder, string.Empty)
                                .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                                .Replace(Path.DirectorySeparatorChar, '/');

                            if (pak.Exists(partialPath))
                            {
                                pak.AddFile(partialPath, file, ConflictPolicy.Replace);
                                this.log.LogDebug("Merged a File\nFile Merged: {MergedFIle}\nInto Source File: {SourceFile}", file.Replace(buildDir, string.Empty), sourceFile.Replace(buildDir, string.Empty));
                            }
                            else
                            {
                                this.log.LogWarning("Expected file {RelativePath} was not found in build file {SourceFile}.", partialPath.Replace(buildDir, string.Empty), sourceFile.Replace(buildDir, string.Empty));
                            }
                        }

                        // Save stream? to temp path.
                        pak.Save(tempSavePath);
                    }

                    // Overwrite original source file with merged temp file.
                    File.Move(tempSavePath, sourceFile, true);
                }

                // Clean up merged files.
                foreach (var file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
