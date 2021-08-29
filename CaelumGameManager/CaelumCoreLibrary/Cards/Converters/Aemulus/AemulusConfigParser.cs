// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Peraser for Aemulus Config XML file.
    /// </summary>
    public static class AemulusConfigParser
    {
        public static AemulusConfigModel ParseAemulusConfig(string filepath)
        {
            using StringReader reader = new(File.ReadAllText(filepath));
            AemulusConfigModel aemulusConfig = new XmlSerializer(typeof(AemulusConfigModel)).Deserialize(reader) as AemulusConfigModel;

            return aemulusConfig;
        }
    }
}
