// Copyright (c) T-Pose Ratkechi. All rights reserved.
// Licensed under the GNU GPLv3 license. See LICENSE file in the project root for full license information.
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

namespace CaelumGameManagerGUI
{
    using System;
    using System.IO;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Formatting.Display;

    /// <summary>
    /// Sink to log Serilog events to GUI console log.
    /// </summary>
    public class CustomSink : ILogEventSink
    {
        /// <summary>
        /// Log event text formatter.
        /// </summary>
        private readonly ITextFormatter textFormatter = new MessageTemplateTextFormatter("<ID:{ThreadId}> ({Timestamp:HH:mm:ss}) [{Level:u3}] {Exception}{Message:j}");

        private readonly StringWriter stringWriter = new();

        /// <summary>
        /// Log event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="s">Formatted log event as string.</param>
        public delegate void LogEventHandler(object sender, string s);

        /// <summary>
        /// Log received event.
        /// </summary>
        public event LogEventHandler LogReceived;

        /// <inheritdoc/>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            this.stringWriter.GetStringBuilder().Clear();
            this.textFormatter.Format(logEvent, this.stringWriter);
            this.LogReceived?.Invoke(this, this.stringWriter.ToString());
        }
    }
}
