// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using AtlusFileSystemLibrary.Common.IO;
    using AtlusFileSystemLibrary.FileSystems.PAK;
    using CaelumCoreLibrary.Builders.Files;
    using CaelumCoreLibrary.Games;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AemulusPackageConverter
    {
        private static readonly string[] ArchiveExts = new string[]
        {
            ".bin",
            ".arc",
            ".pak",
            ".pac",
            ".pack",
        };

        private readonly ILogger log;
        private readonly IGameInstall gameInstall;

        public AemulusPackageConverter(ILogger log, IGameInstall gameInstall)
        {
            this.log = log;
            this.gameInstall = gameInstall;
        }

        public void ConvertAemulusPackages(string aemulusDir)
        {
            var originalFilesFolder = Path.Join(aemulusDir, "Original", "Persona 4 Golden", "data_e");
            var allOriginalFiles = Directory.GetFiles(originalFilesFolder, "*", SearchOption.AllDirectories);

            var packagesFolder = Path.Join(aemulusDir, "Packages", "Persona 4 Golden");

            // Iterate over packages.
            foreach (var packageDir in Directory.GetDirectories(packagesFolder, "*", SearchOption.TopDirectoryOnly))
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
                    // throw new ArgumentException($@"Failed to find a data directory for Aemulus package ""{Path.GetDirectoryName(packageDir)}"".");
                    continue;
                }

                var dataDirFolders = Directory.GetDirectories(dataDir, "*", SearchOption.TopDirectoryOnly);

                foreach (var dirFolder in dataDirFolders)
                {
                    this.RecursiveFixFolders(dirFolder, originalFilesFolder, dataDir);

                    //StringBuilder fixedPathBuilder = new();

                    //var relativePath = dirFolder.Replace(dataDir, string.Empty).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    //var pathParts = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                    //for (int i = 0, total = pathParts.Length; i < total; i++)
                    //{
                    //    var currentPart = pathParts[i];
                    //    var pathToTest = string.Join(Path.DirectorySeparatorChar, pathParts[0..(i + 1)]);

                    //    var allPossibleArchives = ArchiveExts.Select(x => Path.Join(originalFilesFolder, $"{pathToTest}{x}")).ToArray();

                    //    var arcMatchFound = false;
                    //    foreach (var possibleArc in allPossibleArchives)
                    //    {
                    //        if (File.Exists(possibleArc))
                    //        {
                    //            fixedPathBuilder.Append($"{Path.GetFileName(possibleArc)}_{Path.DirectorySeparatorChar}");
                    //            arcMatchFound = true;
                    //            break;
                    //        }
                    //    }

                    //    if (!arcMatchFound)
                    //    {
                    //        fixedPathBuilder.Append(currentPart);
                    //    }
                    //}

                    //var fixedPath = Path.Join(dataDir, fixedPathBuilder.ToString());
                }
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

                        // TODO: Look into the SearchOption.AllDirectories, possibly able to replace
                        // nested files directly.
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

                    if (finalFolder.EndsWith("\\data_e\\init\\event"))
                    {
                        var x = 10;
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
    }
}
