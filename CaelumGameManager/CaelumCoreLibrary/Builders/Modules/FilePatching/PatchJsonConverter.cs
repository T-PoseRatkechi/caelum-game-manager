// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Converter for <see cref="IPatch"/> arrays.
    /// </summary>
    public class PatchJsonConverter : JsonConverter<IPatch[]>
    {
        /// <inheritdoc/>
        public override IPatch[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<IPatch> patchList = new();

            this.ParsePatchObjects(patchList, ref reader);

            return patchList.ToArray();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, IPatch[] value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses and adds <see cref="IPatch"/> objects in JSON patches array to returning array.
        /// </summary>
        /// <param name="patches">Patches list to add patches to.</param>
        /// <param name="reader">Reader.</param>
        private void ParsePatchObjects(List<IPatch> patches, ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return;
                }

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    // Skip StartObject token.
                    reader.Read();

                    // Skip format prop name.
                    reader.Read();

                    // Format prop value.
                    var formatValue = reader.GetString().ToLower();
                    reader.Read();

                    // New patch container.
                    IPatch newPatch;

                    // Set patch instance.
                    switch (formatValue)
                    {
                        case "binary":
                            newPatch = new BinaryPatchFormat();
                            break;
                        default:
                            throw new JsonException($@"Unknown file patch format ""{formatValue}"".");
                    }

                    newPatch.Format = formatValue;

                    var patchType = newPatch.GetType();

                    // Set patch object props.
                    while (reader.TokenType != JsonTokenType.EndObject)
                    {
                        // Read JSON element prop name.
                        var propName = reader.GetString();
                        reader.Read();

                        // Get property info from newPatch that matches to propName.
                        var property = patchType.GetProperty(propName);

                        // Read the correct type for property value then set
                        // newPatches property to the value read in.
                        if (property.PropertyType == typeof(string))
                        {
                            var propValue = reader.GetString();
                            reader.Read();

                            property.SetValue(newPatch, propValue);
                        }
                        else if (property.PropertyType == typeof(uint))
                        {
                            var propValue = reader.GetUInt32();
                            reader.Read();

                            property.SetValue(newPatch, propValue);
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            var propValue = reader.GetInt32();
                            reader.Read();

                            property.SetValue(newPatch, propValue);
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            var propValue = reader.GetBoolean();
                            reader.Read();

                            property.SetValue(newPatch, propValue);
                        }
                        else
                        {
                            throw new JsonException($@"Unsupported property ""{property.Name}"" of type ""{property.PropertyType.Name}"" for patch format ""{formatValue}"".");
                        }
                    }

                    // Add new patch to patches list.
                    patches.Add(newPatch);
                }
            }
        }
    }
}
