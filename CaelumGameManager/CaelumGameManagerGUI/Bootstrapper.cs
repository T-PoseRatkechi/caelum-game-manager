// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Autofac;
    using CaelumGameManagerGUI.ViewModels;
    using Caliburn.Micro;

    /// <summary>
    /// Bootstrapper for Caliburn.
    ///
    /// Setting up Autofac with Caliburn Micro
    /// Grant Byrne
    /// https://medium.com/@holymoo/setting-up-autfac-with-caliburn-micro-v3-2-0-b033487ae3b0
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {
        private static IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this.Initialize();
        }

        /// <inheritdoc/>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<ShellViewModel>();
        }

        /// <inheritdoc/>
        protected override void Configure()
        {
            container = ContainerConfig.Configure();
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        /// <inheritdoc/>
        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (container.IsRegistered(service))
                {
                    return container.Resolve(service);
                }
            }
            else
            {
                if (container.IsRegisteredWithKey(key, service))
                {
                    return container.ResolveKeyed(key, service);
                }
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        /// <inheritdoc/>
        protected override void BuildUp(object instance)
        {
            container.InjectProperties(instance);
        }
    }
}
