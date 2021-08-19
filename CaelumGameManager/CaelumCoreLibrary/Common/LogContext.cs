// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumCoreLibrary.Common
{
    using System.IO;
    using System.Runtime.CompilerServices;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// https://stackoverflow.com/a/57615023
    /// CC BY-SA 4.0
    /// rob.earwaker.
    /// </summary>
    public static class LogContext
    {
        /*
        /// <summary>
        /// Adds caller syntax to logger.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="memberName">Member name.</param>
        /// <param name="filePath">File path.</param>
        /// <param name="lineNumber">Line number.</param>
        /// <returns>Logger with caller syntax.</returns>
        public static ILogger WithCallerSyntax(
        this ILogger logger,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        {
            return logger.ForContext("MemberName", memberName)
                .ForContext("FilePath", filePath)
                .ForContext("FileName", Path.GetFileNameWithoutExtension(filePath))
                .ForContext("LineNumber", lineNumber);
        }
        */
    }
}
