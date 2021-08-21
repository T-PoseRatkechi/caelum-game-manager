// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules
{
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Builds any .song presets present in cards.
    /// </summary>
    public class PhosModule : IBuilderModule
    {
        /// <summary>
        /// Gets game name.
        /// </summary>
        public string PhosGameName { get; init; } = "Persona 4 Golden";

        /// <inheritdoc/>
        public DeckBuildLogger BuildLogger { get; init; }

        /// <inheritdoc/>
        public void BuildCard(CardModel card, string outputDir, HashSet<string> builtFiles)
        {
            string cardDataDir = Path.Join(card.InstallDirectory, "Data");
            var songPresetFiles = Directory.GetFiles(cardDataDir, "*.songs", SearchOption.TopDirectoryOnly);

            if (songPresetFiles.Length > 0)
            {
                // Add card's song pack files to built files list.
                builtFiles.Add(songPresetFiles[0]);
                foreach (var presetSong in Directory.GetFiles(Path.Join(cardDataDir, "songs")))
                {
                    builtFiles.Add(presetSong);
                }

                // Load song pack music data.
                var presetMusicData = PhosLibrary.Common.MusicData.MusicDataParser.Parse(songPresetFiles[0]);

                // Adjust replacement file path for song pack songs.
                for (int i = 0, total = presetMusicData.songs.Length; i < total; i++)
                {
                    presetMusicData.songs[i].isEnabled = true;

                    var adjustedReplacementFile = Path.Join(
                        cardDataDir,
                        "songs",
                        presetMusicData.songs[i].replacementFilePath);

                    presetMusicData.songs[i].replacementFilePath = adjustedReplacementFile;

                    // Add output card files.
                    var expectedOutputFile = Path.Join(outputDir, presetMusicData.songs[i].outputFilePath);
                    this.BuildLogger.LogOutputFile(card, expectedOutputFile);
                }

                var phos = new PhosLibrary.Games.MusicP4G();
                phos.Build(presetMusicData, outputDir, false);
            }
        }
    }
}
