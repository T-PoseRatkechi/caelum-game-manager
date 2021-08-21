// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Addons
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// Builds any .song presets present in cards.
    /// </summary>
    public class PhosSupport : IBuilderAddon
    {
        public string PhosGameName { get; init; } = "Persona 4 Golden";

        public void BuildCard(CardModel card, string outputDir, HashSet<string> builtFilesList, Dictionary<string, List<string>> deckbuildLog)
        {
            var defaultMusicDataPath = Path.Join(
                AppDomain.CurrentDomain.BaseDirectory,
                "Dependencies",
                "PhosLibrary",
                this.PhosGameName,
                "default-music-data.json");

            string cardDataDir = Path.Join(card.InstallDirectory, "Data");
            var songPresetFiles = Directory.GetFiles(cardDataDir, "*.songs", SearchOption.TopDirectoryOnly);

            if (songPresetFiles.Length > 0)
            {
                builtFilesList.Add(songPresetFiles[0]);
                foreach (var presetSong in Directory.GetFiles(Path.Join(cardDataDir, "songs")))
                {
                    // Add source card data files.
                    builtFilesList.Add(presetSong);
                }

                var musicData = PhosLibrary.Common.MusicData.MusicDataParser.Parse(defaultMusicDataPath);
                var presetMusicData = PhosLibrary.Common.MusicData.MusicDataParser.Parse(songPresetFiles[0]);

                foreach (var song in presetMusicData.songs)
                {
                    var index = Array.FindIndex(musicData.songs, x => x.outputFilePath == song.outputFilePath);
                    musicData.songs[index].isEnabled = true;
                    musicData.songs[index].replacementFilePath = Path.Join(cardDataDir, "songs", song.replacementFilePath);
                    musicData.songs[index].loopStartSample = song.loopStartSample;
                    musicData.songs[index].loopEndSample = song.loopEndSample;

                    // Add output card files.
                    var expectedOutput = Path.Join(outputDir, musicData.songs[index].outputFilePath);
                    if (deckbuildLog.ContainsKey(expectedOutput))
                    {
                        deckbuildLog[expectedOutput].Add(card.CardId);
                    }
                    else
                    {
                        deckbuildLog.Add(expectedOutput, new());
                        deckbuildLog[expectedOutput].Add(card.CardId);
                    }
                }

                var phos = new PhosLibrary.Games.MusicP4G();
                phos.Build(musicData, outputDir, false);
            }
        }
    }
}
