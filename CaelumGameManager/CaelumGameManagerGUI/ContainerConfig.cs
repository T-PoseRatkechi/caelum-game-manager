// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using CaelumCoreLibrary.Writers;

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

            builder.RegisterAssemblyTypes(Assembly.Load(nameof(CaelumCoreLibrary)))
                .Where(t => t.Namespace.Contains("Configs") || t.Namespace.Contains("Games"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterType<JsonWriter>().As<IWriter>();

            return builder.Build();
        }
    }
}
