using Debwin.Core.LogWriters;
using Debwin.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Debwin.Core.Parsers
{

    /// <summary>
    /// Parses log files written by <see cref="Debwin4CsvLogWriter"/>.
    /// </summary>
    public class Debwin4CsvParser : IMessageParser
    {

        private LogMessage _lastCreatedMessage;
        private string _lastMessageTypeCode;
        private Func<LogMessage> _currentLogMessageFactory;
        private bool _processingDisabled;
        private int messageNr;

        public Debwin4CsvParser()
        {
            LogFormatVersion = -1;
        }

        /// <summary>
        /// If true, the Debwin4-CSV header line is expected (<see cref="Debwin4CsvLogWriter.V1_COLUMN_HEADERS"/>).
        /// This should not be set if the parser is started AFTER the logging has begun.
        /// </summary>
        public bool CheckForMissingVersionHeader { get; set; }

        /// <summary>
        /// If <see cref="CheckForMissingVersionHeader"/> is false, the log message format to expect must be set manually via this property.
        /// </summary>
        public int LogFormatVersion { get; set; }    // saves the version read from the first line  or a manually defined value


        IEnumerable<int> IMessageParser.GetSupportedMessageTypeCodes()
        {
            return LogMessageFactory.GetKnownMessageTypeCodes();
        }

        public LogMessage CreateMessageFrom(object rawMessage)
        {
            messageNr++;

            if (_processingDisabled)
                return null;

            string csvLine;

            if (rawMessage != null && rawMessage is byte[])
            {
                csvLine = Encoding.UTF8.GetString(rawMessage as byte[]);
            }
            else
            {
                csvLine = rawMessage as string;
            }

            if (string.IsNullOrEmpty(csvLine))
                return null;

            // For the first read message, try to read the .log4 file header to determine the file format version
            if (messageNr == 1 && csvLine == Debwin4CsvLogWriter.V1_COLUMN_HEADERS) {
                LogFormatVersion = 1;
                return null;
            }

            if (CheckForMissingVersionHeader && LogFormatVersion == -1)
            {
                _processingDisabled = true;   // stop the parsing, as all coming lines of the log would throw the exception again  ->  may take a long time until the invalid log is processed
                throw new Exception("No log format version was specified and auto-detection is turned off");
            }
            

            // After we checked the version info, we can assume that the log file has a valid format and skip extra checks for performance reasons

            // Version 1: Columns are defined in Debwin4CsvLogWriter.V1_COLUMN_HEADERS  ("[Dummmy];Timestamp;Level;Logger;Thread;Properties;Message")
            if (LogFormatVersion == 1 || !CheckForMissingVersionHeader)
            {
                return ParseV1FormattedLine(csvLine);
            }

            throw new NotImplementedException("Tried to read Debwin-CSV file of unknown version!");
        }

        private LogMessage ParseV1FormattedLine(string csvLine)
        {
            // The dummy column (=  '▪' char) at the begin of each line works as a marker: 
            // If it exists, it is a new row of the CSV file, if it does not exist it probably is a line break within the original log message. 
            // In the seconds case, the line belongs to the previously created log message.
            if (!csvLine.StartsWith(Debwin4CsvLogWriter.V1_ROW_PREFIX) && _lastCreatedMessage != null)
            {
                _lastCreatedMessage.Message += csvLine;
                return null;
            }

            string[] parts = csvLine.Split(new char[] { ';' }, Debwin4CsvLogWriter.V1_COLUMN_COUNT);
            if (parts.Length != Debwin4CsvLogWriter.V1_COLUMN_COUNT)
            {
                return new LogMessage("[DebwinCsvParser: Invalid Format] " + csvLine) { Level = LogLevel.Warning };
            }

            // For faster deserialization, we don`t write the full typename to the log, but only
            // an integer ID (type code) that is mapped to a LogMessage factory which creates objects for that type code.
            string currentTypeCode = parts[1];
            if (currentTypeCode != _lastMessageTypeCode)
            {
                _lastMessageTypeCode = currentTypeCode;
                _currentLogMessageFactory = LogMessageFactory.GetFactoryForTypeCode(int.Parse(currentTypeCode));
            }


            LogMessage msg = _currentLogMessageFactory.Invoke();
            _lastCreatedMessage = msg;

            msg.Timestamp = DateTime.ParseExact(parts[2], Debwin4CsvLogWriter.V1_TIMESTAMP_FORMAT, CultureInfo.InvariantCulture);
            msg.Level = (LogLevel)int.Parse(parts[3]);
            msg.LoggerName = parts[4];
            msg.Thread = parts[5];
            msg.Message = parts[7];

            // Parse the variable properties:
            if (parts[6].Length != 0)
            {
                if (!ParseAndSetPropertiesV1(msg, parts[6], csvLine))
                    return msg;  // no further processing of message on error
            }

            // handle cases where LL.net uses LlDebugOutput where the original level is always 'Debug' even for error messages.
            if (msg.Message.StartsWith("WRN:"))
                msg.Level = LogLevel.Warning;
            else if (msg.Message.StartsWith("ERR:"))
                msg.Level = LogLevel.Error;

            return msg;
        }


        /// <summary>
        /// Parses the custom properties string of a message, returning true if the properties were valid.
        /// </summary>
        private bool ParseAndSetPropertiesV1(LogMessage msg, string propertiesColumns, string csvLine)
        {
            string[] properties = propertiesColumns.Split(Debwin4CsvLogWriter.V1_PROPERTY_SEPARATOR);
            for (int i = 0; i < properties.Length; ++i)
            {
                // Properties Format usually is:    [Property1]♦[Property2]♦...
                // Property Format usually is:   [PropID]:[PropType]=[PropValue]
                int typeSeparatorIndex = properties[i].IndexOf(Debwin4CsvLogWriter.V1_PROPERTY_TYPE_SEPARATOR);
                if (typeSeparatorIndex == -1)
                {
                    msg.Message = "[DebinCsvParser: Invalid Property Format] " + csvLine;
                    return false;
                }

                int propertyID;
                if (!int.TryParse(properties[i].Substring(0, typeSeparatorIndex), out propertyID))
                {
                    msg.Message = "[DebinCsvParser: Invalid Property Name] " + csvLine;
                    return false;
                }

                int valueSeparatorIndex = properties[i].IndexOf(Debwin4CsvLogWriter.V1_PROPERTY_VALUE_SEPARATOR, typeSeparatorIndex + 1);
                string propertyType = properties[i].Substring(typeSeparatorIndex + 1, valueSeparatorIndex - typeSeparatorIndex - 1);
                string propValueStr = properties[i].Substring(valueSeparatorIndex + 1);

                object propValue;
                if (propertyType == Debwin4CsvLogWriter.V1_PROPERTYTYPE_NULL)
                {
                    propValue = null;
                }
                else if (propertyType == Debwin4CsvLogWriter.V1_PROPERTYTYPE_INT32)
                {
                    propValue = int.Parse(propValueStr);
                }
                else if (propertyType == Debwin4CsvLogWriter.V1_PROPERTYTYPE_STRING)
                {
                    propValue = propValueStr;
                }
                else if (propertyType == Debwin4CsvLogWriter.V1_PROPERTYTYPE_DATETIME)
                {
                    propValue = DateTime.ParseExact(propValueStr, Debwin4CsvLogWriter.V1_TIMESTAMP_FORMAT, CultureInfo.InvariantCulture);
                }
                else
                {
                    msg.Message = "[DebinCsvParser: Invalid Property Type/Value] " + csvLine;
                    return false;
                }

                msg.SetProperty(propertyID, propValue);
            }
            return true;
        }

    }

}
