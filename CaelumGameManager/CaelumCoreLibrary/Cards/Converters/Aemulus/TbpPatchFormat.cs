// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TbpPatchFormat
    {
        public int Version { get; set; }
        public TbpPatch[] Patches { get; set; }
    }

    public class TbpPatch
    {
        public string comment { get; set; }
        public string tbl { get; set; }
        public int section { get; set; }
        public int offset { get; set; }
        public string data { get; set; }
    }
}
