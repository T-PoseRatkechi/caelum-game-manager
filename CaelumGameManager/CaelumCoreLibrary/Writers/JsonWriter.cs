// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Writers
{
    using System.IO;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Json writer.
    /// </summary>
    public class JsonWriter : IWriter
    {
        // private readonly ILogger log = Log.Logger.WithCallerSyntax();
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonWriter"/> class.
        /// </summary>
        /// <param name="log">Logger</param>
        public JsonWriter(ILogger log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public void WriteFile(string filePath, object obj)
        {
            this.log.LogDebug($"Writing {nameof(obj)} to {filePath}");
            var jsonText = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonText);
        }

        /// <inheritdoc/>
        public T ParseFile<T>(string filePath)
        {
            this.log.LogDebug($"Parsing {filePath} as {typeof(T)}");
            var fileText = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(fileText);
        }
    }
}
