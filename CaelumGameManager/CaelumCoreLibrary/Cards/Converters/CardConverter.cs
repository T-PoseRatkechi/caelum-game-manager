// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CaelumCoreLibrary.Cards.Converters.Aemulus;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Card converters.
    /// </summary>
    public class CardConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardConverter"/> class.
        /// </summary>
        /// <param name="log">Logger.</param>
        public CardConverter(ILogger log)
        {
            this.AemulusConverter = new(log);
        }

        public AemulusPackageConverter AemulusConverter { get; }
    }
}
