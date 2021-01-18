using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Debwin.Core.LogWriters
{

    public class Debwin4CsvLogWriter : ILogWriter
    {

        public const string V1_TIMESTAMP_FORMAT = "dd.MM.yyyy HH:mm:ss.fff";
        public const string V1_PROPERTYTYPE_NULL = "0";
        public const string V1_PROPERTYTYPE_INT32 = "1";
        public const string V1_PROPERTYTYPE_STRING = "2";
        public const string V1_PROPERTYTYPE_DATETIME = "3";
        public const string V1_ROW_PREFIX = "▪";
        public const char V1_PROPERTY_SEPARATOR = '♦';
        public const char V1_PROPERTY_TYPE_SEPARATOR = ':';
        public const char V1_PROPERTY_VALUE_SEPARATOR = '=';
        // this header will allow us to choose the right parser when reading the file and at the same time provide column headers when opening the file with a CSV viewer like Excel:
        public const string V1_COLUMN_HEADERS = "Debwin4::CSV::V1;TypeCode;Timestamp;Level;Logger;Thread;Properties;Message";
        public const int V1_COLUMN_COUNT = 8;

        private readonly TextWriter _writer;

        private int lastMessageTypeCode = -1;
        private int[] customPropertyIDs = null;

        public Debwin4CsvLogWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteBeginLog()
        {
            _writer.WriteLine(V1_COLUMN_HEADERS);
            _writer.Flush();
        }

        public void WriteEndLog() { }

        public void WriteLogEntry(LogMessage msg, CancellationToken cancelToken)
        {

            cancelToken.ThrowIfCancellationRequested();

            if (lastMessageTypeCode != msg.GetMessageTypeCode())
            {
                lastMessageTypeCode = msg.GetMessageTypeCode();
                customPropertyIDs = msg.GetCustomPropertyIDs();
            }

            _writer.Write(V1_ROW_PREFIX);   // the '◗' in the first column works as a marker char which allows to distinguish real new rows of the CSV files from line breaks within the message content
            _writer.Write(";");

            // 0) Message Type - Required to restore the correct LogMessage subtype on deserialization
            _writer.Write(lastMessageTypeCode);
            _writer.Write(';');

            // 1) Timestamp
            _writer.Write(msg.Timestamp.ToString(V1_TIMESTAMP_FORMAT, CultureInfo.InvariantCulture));
            _writer.Write(";");

            // 2) Level
            _writer.Write((int)msg.Level);
            _writer.Write(";");

            // 3) Logger
            _writer.Write(msg.LoggerName);
            _writer.Write(";");

            // 4) Thread
            _writer.Write(msg.Thread);
            _writer.Write(";");

            // 5) Properties
            if (customPropertyIDs != null)
            {
                for (int i = 0; i < customPropertyIDs.Length; ++i)   // Save as    [propID]=[value]♦[propID]=[value]♦...
                {
                    if (i > 0)
                    {
                        _writer.Write(V1_PROPERTY_SEPARATOR);   // '♦'
                    }

                    WriteProperty(_writer, msg, customPropertyIDs[i]);
                }
            }
            _writer.Write(";");

            // 6) Message  (should always be the last column, because it may contain line breaks)
            _writer.WriteLine(msg.Message);
            _writer.Flush();
        }

        private void WriteProperty(TextWriter writer, LogMessage msg, int propertyID)
        {
            // Write [PropID]=[PropType]:[PropValue], e.g.   100:2=CMBTLL22   -> Property 100 (Module) has type 2 (String) and value "CMBTLL22"

            _writer.Write(propertyID);
            _writer.Write(V1_PROPERTY_TYPE_SEPARATOR);  // ':'

            object propValue = msg.GetProperty(propertyID);

            if (propValue == null)
            {
                _writer.Write(V1_PROPERTYTYPE_NULL);
                _writer.Write(V1_PROPERTY_VALUE_SEPARATOR);
                _writer.Write("-");   // write at least one char, makes parser code easier
            }
            else if (propValue is DateTime)
            {
                _writer.Write(V1_PROPERTYTYPE_DATETIME);
                _writer.Write(V1_PROPERTY_VALUE_SEPARATOR);
                _writer.Write(((DateTime)propValue).ToString(V1_TIMESTAMP_FORMAT, CultureInfo.InvariantCulture));
            }
            else if (propValue is int)
            {
                _writer.Write(V1_PROPERTYTYPE_INT32);
                _writer.Write(V1_PROPERTY_VALUE_SEPARATOR);
                _writer.Write(propValue.ToString());
            }
            else if (propValue is string)
            {
                _writer.Write(V1_PROPERTYTYPE_STRING);
                _writer.Write(V1_PROPERTY_VALUE_SEPARATOR);
                _writer.Write(propValue.ToString());
            }
            else
            {
                throw new NotImplementedException("Cannot write property type: " + propValue.GetType().FullName);
            }
        }


    }

}