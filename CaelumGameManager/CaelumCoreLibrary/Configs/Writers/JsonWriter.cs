// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs.Writers
{
    using System.IO;
    using System.Text.Json;

    /// <summary>
    /// Json writer.
    /// </summary>
    public class JsonWriter : IWriter
    {
        /// <inheritdoc/>
        public void WriteFile(string filePath, object obj)
        {
            var jsonText = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonText);
        }
    }
}
