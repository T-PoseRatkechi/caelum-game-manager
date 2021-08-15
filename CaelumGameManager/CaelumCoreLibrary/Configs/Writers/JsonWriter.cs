// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Configs.Writers
{
    using System.IO;
    using System.Text.Json;
    using CaelumCoreLibrary.Common;
    using Serilog;

    /// <summary>
    /// Json writer.
    /// </summary>
    public class JsonWriter : IWriter
    {
        private readonly ILogger log = Log.Logger.WithCallerSyntax();

        /// <inheritdoc/>
        public void WriteFile(string filePath, object obj)
        {
            this.log.Debug($"Writing {nameof(obj)} to {filePath}");
            var jsonText = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonText);
        }

        /// <inheritdoc/>
        public T ParseFile<T>(string filePath)
        {
            this.log.Debug($"Parsing {filePath} as {nameof(T)}");
            var fileText = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(fileText);
        }
    }
}
