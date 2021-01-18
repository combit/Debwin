using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Debwin.Core.LogWriters
{

    /// <summary>
    /// Writes a log file as formatted text with fixed-sized columns for the message properties.
    /// </summary>
    public class BasicFormattedLogWriter : ILogWriter
    {

        private readonly IList<int> _propertiesToWrite;   // IDs of the the message properties to write (one column per property)
        private readonly TextWriter _writer;
        private bool _hasWrittenLines;

        /// <param name="propertiesToWrite">IDs of the the message properties to write (one column per property)</param>
        public BasicFormattedLogWriter(TextWriter writer, IList<int> propertiesToWrite)
        {
            if (propertiesToWrite == null || propertiesToWrite.Count == 0)
                throw new ArgumentException("No columns have been specified to write in the log file", nameof(propertiesToWrite));

            _writer = writer;
            _propertiesToWrite = propertiesToWrite;

            DateTimeFormat = "dd.MM.yyyy HH:mm:ss.fff";
        }

        public string DateTimeFormat { get; set; }

        public void WriteBeginLog() { }

        public void WriteEndLog() { }

        public void WriteLogEntry(LogMessage msg, CancellationToken cancelToken)
        {
            // Prepare list and metadata of columns to write
            List<LogFileColumn> columns = new List<LogFileColumn>(_propertiesToWrite.Count);
            foreach (int propertyToWrite in _propertiesToWrite)
            {
                // Get appropriate default column widths for the message properties
                var columnInfo = new LogFileColumn()
                {
                    MessagePropertyID = propertyToWrite,
                    DefaultLength = GetDefaultColumnWidthForProperty(propertyToWrite),
                };
                if (columnInfo.DefaultLength > 0)
                {
                    columnInfo.EmptyValue = new string('-', columnInfo.DefaultLength - 1);   // for performance, prepare a string of the default length in case the property is null
                }
                columns.Add(columnInfo);
            }

            cancelToken.ThrowIfCancellationRequested();

            if (_hasWrittenLines)
                _writer.Write(_writer.NewLine);

            // Write all requested properties of this message as columns in the written log file
            LogFileColumn currentColumn;
            object rawContent;
            string contentToWrite;
            for (int columnIndex = 0; columnIndex < columns.Count; columnIndex++)
            {
                currentColumn = columns[columnIndex];
                rawContent = msg.GetProperty(currentColumn.MessagePropertyID);

                if (columnIndex > 0)
                    _writer.Write(' ');

                // Format depending on property type. Order branches by probability of the value's type!
                if (currentColumn.MessagePropertyID == PropertyIdentifiers.PROPERTY_LEVEL)
                {
                    contentToWrite = LogLevel2String(msg.Level);
                }
                else if (rawContent is string)
                {
                    contentToWrite = rawContent as string;
                }
                else if (rawContent == null)
                {
                    contentToWrite = currentColumn.EmptyValue;
                }
                else if (rawContent is DateTime)
                {
                    contentToWrite = ((DateTime)rawContent).ToString(DateTimeFormat, CultureInfo.InvariantCulture);
                }
                else
                {
                    contentToWrite = rawContent.ToString();
                }

                if (currentColumn.DefaultLength > 0)
                {
                    contentToWrite = contentToWrite.PadRight(currentColumn.DefaultLength);
                }

                _writer.Write(contentToWrite);
            }

            _hasWrittenLines = true;
        }

        private string LogLevel2String(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return "DEBUG";
                case LogLevel.Error:
                    return "ERROR";
                case LogLevel.Info:
                    return "INFO";
                case LogLevel.Warning:
                    return "WARN";
                case LogLevel.UserComment:
                    return "USER";
                default:
                    return "UNKWN";
            }
        }

        /// <summary>
        /// Returns the default number of chars (for padding) for a specific message property of -1 if the property should not be padded.
        /// </summary>
        private int GetDefaultColumnWidthForProperty(int propertyID)
        {
            switch (propertyID)
            {
                case PropertyIdentifiers.PROPERTY_CPU_NR:
                    return 2;

                case PropertyIdentifiers.PROPERTY_LEVEL:
                    return 5;

                case PropertyIdentifiers.PROPERTY_LOGGER_NAME:
                    return 15;

                case PropertyIdentifiers.PROPERTY_MESSAGE:
                    return -1;

                case PropertyIdentifiers.PROPERTY_MODULE_NAME:
                    return 10;

                case PropertyIdentifiers.PROPERTY_THREAD:
                    return 8;

                case PropertyIdentifiers.PROPERTY_TIMESTAMP:
                    return DateTimeFormat.Length;

                case PropertyIdentifiers.PROPERTY_USER_PRINCIPAL:
                    return 15;

                default:
                    return 25;
            }
        }

        private class LogFileColumn
        {
            public int MessagePropertyID;   // ID of the property of the log message that should be written in this column
            public int DefaultLength;       // Minimum number of characters that this property has (for padding)
            public string EmptyValue;       // Value to write if the log message has no property value for this ID (usually an empty string of DefaultLength)
        }
    }
}
