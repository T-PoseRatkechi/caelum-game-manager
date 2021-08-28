// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Aemulus game packages XML.
    /// </summary>
    [Serializable]
    [XmlRoot("Packages")]
    public class GamePackagesModel
    {
        public List<Package> packages { get; set; }
    }

    public class Package
    {
        public string path { get; set; }
        public bool enabled { get; set; }
        public string id { get; set; }
    }
}
