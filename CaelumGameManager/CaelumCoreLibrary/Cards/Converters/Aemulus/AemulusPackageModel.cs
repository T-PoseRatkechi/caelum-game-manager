// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable SA1600 // Elements should be documented

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Aemulus Package XML.
    /// </summary>
    [Serializable]
    [XmlRoot("Metadata")]
    public class AemulusPackageModel
    {
        public string name { get; set; }

        public string id { get; set; }

        public string author { get; set; }

        public string version { get; set; }

        public string description { get; set; }
    }
}
