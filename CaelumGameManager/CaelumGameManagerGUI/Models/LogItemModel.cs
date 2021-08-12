// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI.Models
{
    using Serilog.Events;

    /// <summary>
    /// Contains a log's formatted string and it's level.
    /// </summary>
    public class LogItemModel
    {
        /// <summary>
        /// Gets log message.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Gets log level.
        /// </summary>
        public LogEventLevel Level { get; init; }
    }
}
