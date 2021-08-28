// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Cards.Converters.Aemulus
{
    using System;
    using System.IO;
    using System.Text;
    using CaelumCoreLibrary.Builders.Modules.FilePatching.Formats;

    /// <summary>
    /// Classic tblpatches parser. Converts tblpatches to binary patches format.
    /// </summary>
    public static class TblpatchesBinaryParser
    {
        public static BinaryPatchFormat GetConvertedTblpatch(string patchFile)
        {
            using (BinaryReader reader = new(File.OpenRead(patchFile)))
            {
                var patchTag = Encoding.ASCII.GetString(reader.ReadBytes(3));
                var offsetBytes = reader.ReadBytes(8);
                Array.Reverse(offsetBytes);
                var patchOffset = BitConverter.ToUInt64(offsetBytes);
                var data = reader.ReadBytes((int)(reader.BaseStream.Position - 10));

                var tblName = GetTblNameFromTag(patchTag);

                var newPatch = new BinaryPatchFormat()
                {
                    File = $@"${{UnpackedGameFiles}}\data_e\init_free.bin_\battle\{tblName}.TBL",
                    Comment = null,
                    Offset = (uint)patchOffset,
                    Data = ByteArrayToHexText(data),
                };

                return newPatch;
            }
        }

        private static string ByteArrayToHexText(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", " ");
        }

        private static string GetTblNameFromTag(string tag)
        {
            return tag switch
            {
                "SKL" => "SKILL",
                "UNT" => "UNIT",
                "MSG" => "MSG",
                "PSA" => "PERSONA",
                "ENC" => "ENCOUNT",
                "EFF" => "EFFECT",
                "MDL" => "MODEL",
                "AIC" => "AICALC",
                _ => null,
            };
        }
    }
}
