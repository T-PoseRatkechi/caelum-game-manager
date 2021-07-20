// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Common
{
    /// <summary>
    /// Author file structure.
    /// </summary>
    public class AuthorFile
    {
        public byte[] header { get; set; } = new byte[] { 0x53, 0x55, 0x43, 0x44 };
        public uint size { get; set; }
        public uint avatarSize { get; set; }
        public byte[] avatarBytes { get; set; }
        public uint authorSize { get; set; }
        public byte[] authorBytes { get; set; }
    }
}
