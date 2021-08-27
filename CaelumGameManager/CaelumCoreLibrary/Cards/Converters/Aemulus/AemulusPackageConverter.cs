// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using AtlusFileSystemLibrary.Common.IO;
    using AtlusFileSystemLibrary.FileSystems.PAK;
    using CaelumCoreLibrary.Builders.Modules.FilePatching;
    using CaelumCoreLibrary.Builders.Modules.FilePatching.Formats;
    using CaelumCoreLibrary.Writers;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

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
            ".spr",
        };

        private readonly ILogger log;
        private readonly IWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AemulusPackageConverter"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        /// <param name="gameInstall">Game installation.</param>
        public AemulusPackageConverter(ILogger log, IWriter writer)
        {
            this.log = log;
            this.writer = writer;
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
                var packageXmlFile = Path.Join(packageDir, "Package.xml");

                // Create card json file from package xml.
                using (StringReader reader = new(File.ReadAllText(packageXmlFile)))
                {
                    AemulusPackageModel packageXml = new XmlSerializer(typeof(AemulusPackageModel)).Deserialize(reader) as AemulusPackageModel;

                    CardModel card = new()
                    {
                        Name = packageXml.name,
                        CardId = packageXml.id,
                        Authors = new(),
                        Description = packageXml.description,
                        Type = CardType.Folder,
                        Version = packageXml.version,
                    };

                    this.writer.WriteFile(Path.Join(packageDir, "card.json"), card);
                }

                // Move SND folder if preset to card data folder.
                var sndFolder = Path.Join(packageDir, "SND");
                if (Directory.Exists(sndFolder))
                {
                    this.CopyFolder(sndFolder, Path.Join(packageDir, "Data", "SND"));
                }

                string dataDir = null;
                if (Directory.Exists(Path.Join(packageDir, "data_e")))
                {
                    dataDir = Path.Join(packageDir, "data_e");
                }
                else if (Directory.Exists(Path.Join(packageDir, "data00004")))
                {
                    dataDir = Path.Join(packageDir, "data00004");
                }

                if (dataDir != null)
                {
                    var dataDirFolders = Directory.GetDirectories(dataDir, "*", SearchOption.TopDirectoryOnly);

                    // Adjusts paths to match the "{originalFile}.bin_\" pattern for nested files.
                    foreach (var dirFolder in dataDirFolders)
                    {
                        this.RecursiveFixFolders(dirFolder, originalFilesFolder, dataDir);
                    }

                    // Move contents of package data folder to root package folder.
                    this.CopyFolder(dataDir, Path.Join(packageDir, "Data", "data_e"));
                    Directory.Delete(dataDir, true);
                }

                // Delete xml and original data dir.
                File.Delete(packageXmlFile);

                var dataFolder = Path.Join(packageDir, "Data");
                Directory.CreateDirectory(dataFolder);

                // Convert tbl patches.
                var tblpatchesDir = Path.Join(packageDir, "tblpatches");
                if (Directory.Exists(tblpatchesDir))
                {
                    List<IPatch> convertedPatches = new();

                    foreach (var tbpPatchFile in Directory.GetFiles(tblpatchesDir, "*.tbp", SearchOption.AllDirectories))
                    {
                        var tbpFileText = File.ReadAllText(tbpPatchFile);
                        var tbpPatch = JsonConvert.DeserializeObject<TbpPatchFormat>(tbpFileText);

                        foreach (var patch in tbpPatch.Patches)
                        {
                            IPatch newPatch;

                            if (patch.tbl == "ITEMTBL")
                            {
                                newPatch = new ItemTblPatchFormat()
                                {
                                    File = $@"${{UnpackedGameFiles}}\data_e\init_free.bin_\init\itemtbl.bin",
                                    Comment = patch.comment,
                                    Segment = patch.section,
                                    Offset = patch.offset,
                                    Data = patch.data,
                                };
                            }
                            else
                            {
                                newPatch = new TblPatchFormat()
                                {
                                    File = $@"${{UnpackedGameFiles}}\data_e\init_free.bin_\battle\{patch.tbl}.TBL",
                                    Comment = patch.comment,
                                    Segment = patch.section,
                                    Offset = patch.offset,
                                    Data = patch.data,
                                };
                            }

                            convertedPatches.Add(newPatch);
                        }
                    }

                    var gamePatch = new GamePatch()
                    {
                        GameName = "Persona 4 Golden",
                        Patches = convertedPatches.ToArray(),
                    };

                    var gamePatchString = JsonConvert.SerializeObject(gamePatch, Formatting.Indented);
                    var gamePatchFile = Path.Join(packageDir, "Data", "Patches", "Converted TBP Patches.json");
                    Directory.CreateDirectory(Path.GetDirectoryName(gamePatchFile));
                    File.WriteAllText(gamePatchFile, gamePatchString);

                    this.log.LogDebug("Converted {NumPatches} for package {PackageName}", convertedPatches.Count, Path.GetDirectoryName(packageDir));
                }

                // Copy over preappfile append files.
                var packageAppendFolder = Path.Join(packageDir, "preappfile", "data_e");
                if (Directory.Exists(packageAppendFolder))
                {
                    var caelumAppendFolder = Path.Join(dataFolder, "data_e", "append");
                    this.CopyFolder(packageAppendFolder, caelumAppendFolder);
                    Directory.Delete(packageAppendFolder, true);
                }
            }

            this.CopyFolder(tempFolder, outputDir);
            Directory.Delete(tempFolder, true);
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

                    var outputDir = Path.Join(originalFilesFolder, adjustedFolderPath.Replace(dataFolder, string.Empty));

                    // Unpack archive in case nested file of it is needed.
                    if (possibleArc.EndsWith(".spr"))
                    {
                        this.log.LogDebug("Found spr file, must unpack");
                        var sprUtils = new SprUtils(this.log);
                        sprUtils.ExtractTmx(possibleArc, outputDir);
                    }
                    else
                    {
                        // Unpack like PakPack.
                        if (!PAKFileSystem.TryOpen(possibleArc, out var pak))
                        {
                            throw new ArgumentException($@"Invalid PAK file ""{possibleArc}"".");
                        }

                        // Unpack contents.
                        using (pak)
                        {
                            // this.log.LogDebug("PAK format version: {PakVersion}", pak.Version);
                            foreach (string file in pak.EnumerateFiles())
                            {
                                var normalizedFilePath = file.Replace("../", string.Empty); // Remove backwards relative path
                                using (var stream = FileUtils.Create(outputDir + Path.DirectorySeparatorChar + normalizedFilePath))
                                using (var inputStream = pak.OpenFile(file))
                                {
                                    // Console.WriteLine($"Extracting {file}");
                                    this.log.LogDebug($"Extracting {file}");
                                    inputStream.CopyTo(stream);
                                }
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

        private void CopyFolder(string sourceFolder, string destFolder)
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
