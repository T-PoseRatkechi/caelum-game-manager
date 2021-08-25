// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System;
    using System.IO;
    using System.Linq;
    using AtlusFileSystemLibrary.Common.IO;
    using AtlusFileSystemLibrary.FileSystems.PAK;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Converts Aemulus packages to Caelum Cards.
    /// </summary>
    public class AemulusPackageConverter
    {
        /// <summary>
        /// Possible archive types.
        /// </summary>
        private static readonly string[] ArchiveExts = new string[]
        {
            ".bin",
            ".arc",
            ".pak",
            ".pac",
            ".pack",
        };

        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="AemulusPackageConverter"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameInstall">Game installation.</param>
        public AemulusPackageConverter(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Converts the package at <paramref name="packageDir"/> to an Caelum card at <paramref name="outputDir"/>
        /// </summary>
        /// <param name="aemulusDir">Aemulus install directory.</param>
        /// <param name="packageDir">Package directory to convert.</param>
        /// <param name="outputDir">Output directory for new card.</param>
        public void Convert(string aemulusDir, string packageDir, string outputDir)
        {

        }

        /// <summary>
        /// Converts and imports all Aemulus packages as Caelum cards.
        /// </summary>
        /// <param name="aemulusDir">Aemulus install directory.</param>
        /// <param name="outputDir">Output directory to import the converted packages to.</param>
        public void Import(string aemulusDir, string outputDir)
        {
            // Path to P4G packages folder.
            var packagesFolder = Path.Join(aemulusDir, "Packages", "Persona 4 Golden");

            // Copy all packages to temp folder in output.
            var tempFolder = Path.Join(outputDir, "temp");
            Directory.CreateDirectory(tempFolder);
            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(packagesFolder, tempFolder, true);

            // Aemulus P4G original files.
            var originalFilesFolder = Path.Join(aemulusDir, "Original", "Persona 4 Golden", "data_e");

            // Fixed folder paths of packages in temp folder.
            foreach (var packageDir in Directory.GetDirectories(tempFolder, "*", SearchOption.TopDirectoryOnly))
            {
                string dataDir = null;
                if (Directory.Exists(Path.Join(packageDir, "data_e")))
                {
                    dataDir = Path.Join(packageDir, "data_e");
                }
                else if (Directory.Exists(Path.Join(packageDir, "data00004")))
                {
                    dataDir = Path.Join(packageDir, "data00004");
                }

                if (dataDir == null)
                {
                    continue;
                }

                var dataDirFolders = Directory.GetDirectories(dataDir, "*", SearchOption.TopDirectoryOnly);

                // Adjusts paths to match the "{originalFile}.bin_\" pattern for nested files.
                foreach (var dirFolder in dataDirFolders)
                {
                    this.RecursiveFixFolders(dirFolder, originalFilesFolder, dataDir);
                }

                // Move contents of package data folder to root package folder.
                this.MoveFolderContents(dataDir, packageDir);
            }
        }

        private void RecursiveFixFolders(string folder, string originalFilesFolder, string dataFolder)
        {
            var finalFolder = folder;

            var relativePath = folder.Replace(dataFolder, string.Empty);
            var allPossibleArchives = ArchiveExts.Select(x => Path.Join(originalFilesFolder, $"{relativePath}{x}")).ToArray();

            foreach (var possibleArc in allPossibleArchives)
            {
                if (File.Exists(possibleArc))
                {
                    var arcExt = Path.GetExtension(possibleArc);
                    var adjustedFolderPath = $"{folder}{arcExt}_";
                    Directory.Move(folder, adjustedFolderPath);
                    finalFolder = adjustedFolderPath;

                    // Unpack archive in case nested file of it is needed.

                    // Unpack like PakPack.
                    if (!PAKFileSystem.TryOpen(possibleArc, out var pak))
                    {
                        throw new ArgumentException($@"Invalid PAK file ""{possibleArc}"".");
                    }

                    // Unpack contents.
                    using (pak)
                    {
                        // this.log.LogDebug("PAK format version: {PakVersion}", pak.Version);
                        var outputDir = Path.Join(originalFilesFolder, adjustedFolderPath.Replace(dataFolder, string.Empty));

                        foreach (string file in pak.EnumerateFiles())
                        {
                            var normalizedFilePath = file.Replace("../", string.Empty); // Remove backwards relative path
                            using (var stream = FileUtils.Create(outputDir + Path.DirectorySeparatorChar + normalizedFilePath))
                            using (var inputStream = pak.OpenFile(file))
                            {
                                // Console.WriteLine($"Extracting {file}");
                                // this.log.LogDebug($"Extracting {file}");
                                inputStream.CopyTo(stream);
                            }
                        }
                    }

                    break;
                }
            }

            var directories = Directory.GetDirectories(finalFolder, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in directories)
            {
                this.RecursiveFixFolders(dir, originalFilesFolder, dataFolder);
            }
        }

        private void MoveFolderContents(string sourceFolder, string destFolder)
        {
            foreach (var file in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
            {
                var outputFile = Path.Join(destFolder, file.Replace(sourceFolder, string.Empty));
                Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                File.Copy(file, outputFile, true);
            }
        }
    }
}
