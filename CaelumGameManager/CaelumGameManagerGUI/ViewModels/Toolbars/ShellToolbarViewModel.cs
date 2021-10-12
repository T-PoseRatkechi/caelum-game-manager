// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.ViewModels.Toolbars
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Cards.Converters;
    using CaelumCoreLibrary.Cards.Converters.Aemulus;
    using CaelumCoreLibrary.Games;
    using CaelumCoreLibrary.Utilities;
    using CaelumGameManagerGUI.Cards;
    using CaelumGameManagerGUI.Common;
    using CaelumGameManagerGUI.Models;
    using CaelumGameManagerGUI.Resources.Localization;
    using CaelumGameManagerGUI.ViewModels.Configs;
    using Caliburn.Micro;
    using Newtonsoft.Json;
    using Serilog;

    public class ShellToolbarViewModel : Screen
    {
        private IGameInstance _gameInstance;
        private CardConverter _cardConverter;
        private BindableDeckModel _deck;
        private readonly IWindowManager _windowManager;

        public ShellToolbarViewModel(IGameInstance gameInstance, BindableDeckModel deck, CardConverter cardConverter, IWindowManager windowManager)
        {
            this._cardConverter = cardConverter;
            this._gameInstance = gameInstance;
            this._deck = deck;
            this._windowManager = windowManager;
        }

        public void OpenConfig()
        {
            this._windowManager.ShowDialogAsync(new MainConfigViewModel());
        }

        /// <summary>
        /// Import Aemulus packages and settings.
        /// </summary>
        /// <param name="sender">Sender.</param>
        public async void ImportAemulus()
        {
            var aemulusDir = SelectWindow.SelectFileWindow(LocalizedStrings.Instance["WindowSelectAemulusTitle"], "AemulusPackageManager.exe| *.exe");
            if (aemulusDir == null)
            {
                return;
            }
            else
            {
                aemulusDir = Path.GetDirectoryName(aemulusDir);
            }

            try
            {
                Log.Information(LocalizedStrings.Instance["ImportAemulusMessage"]);
                await Task.Run(() => this._cardConverter.AemulusConverter.Import(aemulusDir, this._gameInstance.GameInstall.CardsDirectory));

                Log.Debug("Loading Aemulus settings.");
                var p4gGamePackagesFile = Path.Join(aemulusDir, "Config", "Persona4GoldenPackages.xml");
                var p4gGamePackages = GamePackagesParser.ParseGamePackagesXml(p4gGamePackagesFile);

                // Set Card order to Aemulus order.
                var aemulusPackageOrder = p4gGamePackages.packages.Select(x => x.id).ToArray();
                this._gameInstance.GameConfig.Settings.Cards = aemulusPackageOrder;

                // Get P4G install from Aemulus config.
                var aemulusConfig = AemulusConfigParser.ParseAemulusConfig(Path.Join(aemulusDir, "Config", "Config.xml"));
                if (aemulusConfig.p4gConfig.exePath != null)
                {
                    this._gameInstance.GameConfig.Settings.GameInstallPath = aemulusConfig.p4gConfig.exePath;
                    this._gameInstance.GameConfig.Settings.OutputDirectory = aemulusConfig.p4gConfig.modDir;
                    this._gameInstance.GameConfig.Settings.OutputBuildOnly = true;

                    // Add Reloaded as game launcher.
                    if (!string.IsNullOrEmpty(aemulusConfig.p4gConfig.reloadedPath))
                    {
                        var launchersDir = this._gameInstance.GameInstall.LaunchersDirectory;
                        var reloadedCardId = "sewer56_reloadedii";
                        var reloadedCardDir = Path.Join(launchersDir, reloadedCardId);

                        // Copy reloaded to launchers dir as a card.
                        Directory.CreateDirectory(reloadedCardDir);

                        LauncherCardModel reloadedCard = new()
                        {
                            CardId = reloadedCardId,
                            Name = "Reloaded II",
                            Authors = null,
                            Games = new() { "Persona 4 Golden" },
                            IsEnabled = true,
                            Version = "1.0.0",
                            Description = "[Reloaded II] is a Universal DLL Injection based Mod Loader and Management System.",
                            Type = CardType.Launcher,
                            LauncherPath = "Reloaded-II.exe",
                            LauncherArgs = @"--launch ""${GameInstall}""",
                            InstallDirectory = reloadedCardDir,
                        };

                        // Write card file.
                        var cardText = JsonConvert.SerializeObject(reloadedCard, Formatting.Indented);
                        File.WriteAllText(Path.Join(reloadedCardDir, "card.json"), cardText);

                        CaelumFileIO.CopyFolder(Path.GetDirectoryName(aemulusConfig.p4gConfig.reloadedPath), Path.Join(reloadedCardDir, "Data"));
                        Log.Debug("Imported Reloaded II as a Launcher.");
                    }
                }

                Log.Debug("Aemulus settings loaded. Reloading cards.");
                this._gameInstance.Deck.LoadDeckCards();
                this._gameInstance.InitDeck();
                this._deck.Clear();
                this._deck.AddRange(this._gameInstance.Deck.Cards.ToObservableCards());

                // Set card enabled settings same as Aemulus.
                foreach (var gamePackage in p4gGamePackages.packages)
                {
                    var match = this._deck.FirstOrDefault(x => x.CardId == gamePackage.id);
                    if (match != null)
                    {
                        match.IsEnabled = gamePackage.enabled;
                    }
                }

                this._gameInstance.GameConfig.SaveGameConfig();

                // Annoying bug causes currently select item text to disappear after this.
                // Item is still default, just text missing. TODO: Code behind to fix?
                // this.NotifyOfPropertyChange(() => this.GameLauncher);

                Log.Information(LocalizedStrings.Instance["ImportAemulusSuccessMessage"]);
            }
            catch (Exception e)
            {
                Log.Error(e, LocalizedStrings.Instance["ErrorAemulusImportFailedMessage"]);
            }
        }
    }
}
