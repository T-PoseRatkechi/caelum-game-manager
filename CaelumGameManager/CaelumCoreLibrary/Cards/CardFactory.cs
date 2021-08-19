// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards
{
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Games;
    using CaelumCoreLibrary.Writers;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.IO.Abstractions;

    public class CardFactory : ICardFactory
    {
        private readonly ILogger log;
        private readonly IFileSystem fileSystem;
        private readonly IWriter writer;
        private readonly ICaelumConfig caelumConfig;

        public CardFactory(ILogger log, IFileSystem fileSystem, IWriter writer, ICaelumConfig caelumConfig)
        {
            this.log = log;
            this.fileSystem = fileSystem;
            this.writer = writer;
            this.caelumConfig = caelumConfig;
        }

        public void CreateCard(IGameInstall gameInstall, CardModel card)
        {
            switch (card.Type)
            {
                case CardType.Folder:
                    var cardInstallDir = Path.Join(gameInstall.CardsDirectory, card.CardId);
                    if (this.fileSystem.Directory.Exists(cardInstallDir))
                    {
                        throw new ArgumentException($"Could not create card because an installation folder for it already exists. Directory: {cardInstallDir}", nameof(card));
                    }

                    this.fileSystem.Directory.CreateDirectory(cardInstallDir);
                    this.writer.WriteFile(Path.Join(cardInstallDir, "card.json"), card);
                    card.InstallDirectory = cardInstallDir;

                    break;
                default:
                    throw new NotSupportedException($@"Creating cards of card type ""{card.Type}"" is not supported.");
            }
        }
    }
}
