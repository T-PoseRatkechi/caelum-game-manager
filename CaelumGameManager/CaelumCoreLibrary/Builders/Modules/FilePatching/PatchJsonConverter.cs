// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders.Modules.FilePatching
{
    using System;
    using System.Collections.Generic;
    using CaelumCoreLibrary.Builders.Modules.FilePatching.Formats;
    using Newtonsoft.Json;

    /// <summary>
    /// Converter for <see cref="IPatch"/> arrays.
    /// </summary>
    public class PatchJsonConverter : JsonConverter<IPatch[]>
    {
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, IPatch[] value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override IPatch[] ReadJson(JsonReader reader, Type objectType, IPatch[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<IPatch> patchList = new();

            this.ParsePatchObjects(patchList, reader);

            return patchList.ToArray();
        }

        /// <summary>
        /// Parses and adds <see cref="IPatch"/> objects in JSON patches array to returning array.
        /// </summary>
        /// <param name="patches">Patches list to add patches to.</param>
        /// <param name="reader">Reader.</param>
        private void ParsePatchObjects(List<IPatch> patches, JsonReader reader)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray)
                {
                    return;
                }

                if (reader.TokenType == JsonToken.StartObject)
                {
                    // Read start object token.
                    reader.Read();
                    reader.Read();

                    string formatValue = (string)reader.Value;

                    // New patch container.
                    IPatch newPatch = this.GetPatchInstance(formatValue);
                    var patchType = newPatch.GetType();

                    // Set patch object props.
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                        {
                            break;
                        }

                        // Read JSON element prop name.
                        var propName = reader.Value;

                        // Get property info from newPatch that matches to propName.
                        var property = patchType.GetProperty((string)propName);

                        if (property == null)
                        {
                            throw new JsonException($@"JSON property ""{propName}"" was not found in patch format ""{patchType.Name}"".");
                        }

                        reader.Read();

                        // Read the correct type for property value then set
                        // newPatches property to the value read in.
                        if (property.PropertyType == typeof(string))
                        {
                            string propValue = (string)reader.Value;

                            property.SetValue(newPatch, propValue);
                        }
                        else if (property.PropertyType == typeof(uint))
                        {
                            uint propValue = Convert.ToUInt32(reader.Value); // No readAsUInt.

                            property.SetValue(newPatch, propValue);
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            int propValue = Convert.ToInt32(reader.Value);

                            property.SetValue(newPatch, propValue);
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            bool propValue = (bool)reader.Value;

                            property.SetValue(newPatch, propValue);
                        }
                        else
                        {
                            throw new JsonException($@"Unsupported type ""{property.PropertyType.Name}"" for property ""{property.Name}"" in patch format ""{patchType.Name}"".");
                        }
                    }

                    // Add new patch to patches list.
                    patches.Add(newPatch);
                }
            }
        }

        private IPatch GetPatchInstance(string format)
        {
            switch (format)
            {
                case "binary":
                    return new BinaryPatchFormat();
                case "tblpatch":
                    return new TblPatchFormat();
                default:
                    throw new JsonException($@"Unknown file patch format ""{format}"".");
            }
        }
    }
}
