// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("AemulusConfig")]
    public class AemulusConfigModel
    {
        public string game { get; set; }
        public bool bottomUpPriority { get; set; }
        public bool updateAemulus { get; set; }
        public bool darkMode { get; set; }
        public P3FConfig p3fConfig { get; set; }
        public P4GConfig p4gConfig { get; set; }
        public P5Config p5Config { get; set; }
        public P5SConfig p5sConfig { get; set; }

    }

    public class P3FConfig
    {
        public bool buildWarning { get; set; }
        public bool buildFinished { get; set; }
        public bool updateConfirm { get; set; }
        public bool updateChangelog { get; set; }
        public bool advancedLauncherOptions { get; set; }
        public bool deleteOldVersions { get; set; }
        public bool updatesEnabled { get; set; }
    }

    public class P4GConfig
    {
        public string modDir { get; set; }
        public string exePath { get; set; }
        public string reloadedPath { get; set; }
        public bool emptySND { get; set; }
        public bool useCpk { get; set; }
        public string cpkLang { get; set; }
        public bool deleteOldVersions { get; set; }
        public bool buildWarning { get; set; }
        public bool buildFinished { get; set; }
        public bool updateConfirm { get; set; }
        public bool updateChangelog { get; set; }
        public bool updateAll { get; set; }
        public bool updatesEnabled { get; set; }
    }

    public class P5Config
    {
        public bool buildWarning { get; set; }
        public bool buildFinished { get; set; }
        public bool updateConfirm { get; set; }
        public bool updateChangelog { get; set; }
        public bool updateAll { get; set; }
        public bool deleteOldVersions { get; set; }
        public bool updatesEnabled { get; set; }
    }

    public class P5SConfig
    {
        public string modDir { get; set; }
        public bool buildWarning { get; set; }
        public bool buildFinished { get; set; }
        public bool updateConfirm { get; set; }
        public bool updateChangelog { get; set; }
        public bool updateAll { get; set; }
        public bool updatesEnabled { get; set; }
    }
}
