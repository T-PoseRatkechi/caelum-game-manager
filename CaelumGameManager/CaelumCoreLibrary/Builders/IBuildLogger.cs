// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Builders
{
    using CaelumCoreLibrary.Cards;

    /// <summary>
    /// BuildLogger interface.
    /// </summary>
    public interface IBuildLogger
    {
        /// <summary>
        /// Logs that <paramref name="card"/> interacted with <paramref name="outputFile"/>.
        /// </summary>
        /// <param name="card">Card.</param>
        /// <param name="outputFile">Output file card interacted with.</param>
        void LogOutputFile(ICardModel card, string outputFile);

        /// <summary>
        /// Writes build log to file.
        /// </summary>
        /// <param name="outputFile">Log output file.</param>
        void WriteOutputLog(string outputFile);
    }
}