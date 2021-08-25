// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI
{
    using System.IO.Abstractions;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using CaelumCoreLibrary.Builders;
    using CaelumCoreLibrary.Cards;
    using CaelumCoreLibrary.Cards.Converters;
    using CaelumCoreLibrary.Cards.Converters.Aemulus;
    using CaelumCoreLibrary.Configs;
    using CaelumCoreLibrary.Writers;
    using CaelumGameManagerGUI.Models;
    using CaelumGameManagerGUI.ViewModels;
    using Caliburn.Micro;
    using Microsoft.Extensions.Logging;
    using Serilog;

    /// <summary>
    /// AutoFac container.
    /// </summary>
    public static class ContainerConfig
    {
        /// <summary>
        /// Configures container.
        /// </summary>
        /// <returns>Built container.</returns>
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            var caelumLibrary = Assembly.Load(nameof(CaelumCoreLibrary));

            // Factories as single instance.
            builder.RegisterAssemblyTypes(caelumLibrary)
                .Where(t => t.Namespace.Contains("Configs") ||
                            t.Namespace.Contains("Games") ||
                            t.Namespace.Contains("Decks"))
                .Where(t => t.Name.Contains("Factory"))
                .AsImplementedInterfaces()
                .SingleInstance();

            // Writer.
            builder.RegisterType<JsonWriter>().As<IWriter>().SingleInstance();
            builder.RegisterType<CaelumConfig>().As<ICaelumConfig>().SingleInstance();

            // Cards.
            builder.RegisterType<CardParser>().As<ICardParser>().SingleInstance();
            builder.RegisterType<CardFactory>().As<ICardFactory>().SingleInstance();

            // Deck builders.
            builder.RegisterType<DeckBuilderFactory>().As<IDeckBuilderFactory>().SingleInstance();

            // Converters
            builder.RegisterType<CardConverter>().SingleInstance();

            // Logging.
            var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);
            var logger = loggerFactory.CreateLogger("Logger");
            builder.RegisterInstance(logger).As<Microsoft.Extensions.Logging.ILogger>();

            // Filesystem.
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();

            // Caelum Core.
            builder.RegisterType<CaelumCore>().SingleInstance();

            // VMs.
            builder.RegisterType<ShellViewModel>().SingleInstance();
            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();

            return builder.Build();
        }
    }
}
