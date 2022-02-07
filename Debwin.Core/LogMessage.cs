using System;

namespace Debwin.Core
{

    public class LogMessage
    {

        public const int TYPECODE_DEFAULT_MESSAGE = 1;

        public int LineNr = -1;

        public LogMessage() { }

        public LogMessage(string message)
        {
            this.Message = message;
        }

        /* For fast searches over millions of messages, we don`t want to have the overhead of calling a getter function for every message -> make the raw variables public */

        public DateTime Timestamp;

        public int RelativeTime = 0;

        public string Message;

        public LogLevel Level;

        public string Thread;

        public string LoggerName;

        public bool IsBookmark;

        /// <summary>
        /// Returns the message property associated with the specified property ID or null, if this message type does not have that property.
        /// </summary>
        public virtual object GetProperty(int propertyIdentifier)
        {
            switch (propertyIdentifier)
            {
                case PropertyIdentifiers.PROPERTY_TIMESTAMP:
                    return Timestamp;
                case PropertyIdentifiers.PROPERTY_MESSAGE:
                    return Message;
                case PropertyIdentifiers.PROPERTY_LEVEL:
                    return Level;
                case PropertyIdentifiers.PROPERTY_THREAD:
                    return Thread;
                case PropertyIdentifiers.PROPERTY_LOGGER_NAME:
                    return LoggerName;
                case PropertyIdentifiers.PROPERTY_LINE_NR:
                    return LineNr;

                    // New standard properties need to be added to GetStandardPropertyIDs() and GetPropertyMetadata() !
            }
            return null;
        }

        // See GetProperty().
        public virtual void SetProperty(int propertyIdentifier, object value) { }

        /// <summary>
        /// Gets the IDs of all custom properties available for this message type, so the log writers (<see cref="LogWriters.ILogWriter"/>) can add these
        /// properties when writing the captured messages to a log file.
        /// </summary>
        public virtual int[] GetCustomPropertyIDs()
        {
            return new int[0];
        }

        /// <summary>
        /// Returns the standard properties available for every message type. Combined with the return value of <see cref="GetCustomPropertyIDs"/>,
        /// you can get a list of all valid property identifier for <see cref="GetProperty(int)"/> and <see cref="SetProperty(int, object)"/>.
        /// </summary>
        public int[] GetStandardPropertyIDs()
        {
            return new int[] {
                PropertyIdentifiers.PROPERTY_TIMESTAMP,
                PropertyIdentifiers.PROPERTY_MESSAGE,
                PropertyIdentifiers.PROPERTY_LEVEL,
                PropertyIdentifiers.PROPERTY_THREAD,
                PropertyIdentifiers.PROPERTY_LOGGER_NAME,
                PropertyIdentifiers.PROPERTY_LINE_NR
            };
        }

        /// <summary>
        /// Returns a number that identifies the LogMessage type. This allows faster type checks than using Object.GetType()/Reflection when writing and parsing log messages.
        /// Child classes must override this with an own unique type code. New type codes should also be registered at the <see cref="LogMessageFactory"/>.
        /// </summary>
        public virtual int GetMessageTypeCode()
        {
            return TYPECODE_DEFAULT_MESSAGE;
        }

        /// <summary>Returns metadata for the supported properties of this message type, or null if the property ID is unknown.</summary>
        public virtual LogMessagePropertyInfo GetPropertyMetadata(int propertyID)
        {
            switch (propertyID)
            {
                case PropertyIdentifiers.PROPERTY_TIMESTAMP:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_TIMESTAMP, "Date/Time", typeof(DateTime));
                case PropertyIdentifiers.PROPERTY_MESSAGE:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_MESSAGE, "Message", typeof(string));
                case PropertyIdentifiers.PROPERTY_LEVEL:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_LEVEL, "Level", typeof(LogLevel));
                case PropertyIdentifiers.PROPERTY_THREAD:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_THREAD, "Thread", typeof(string));
                case PropertyIdentifiers.PROPERTY_LOGGER_NAME:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_LOGGER_NAME, "Logger", typeof(string));
                case PropertyIdentifiers.PROPERTY_LINE_NR:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_LINE_NR, "Line", typeof(int));
                default:
                    return null;
            }
        }

    }

    public class LogMessagePropertyInfo
    {
        public int PropertyID { get; private set; }
        public string UIName { get; private set; }
        public Type DataType { get; private set; }


        public LogMessagePropertyInfo(int propertyID, string uiName, Type dataType)
        {
            this.PropertyID = propertyID;
            this.UIName = uiName;
            this.DataType = dataType;
        }
    }

    public enum LogLevel
    {
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,

        /* Debwin4-Internal */
        UserComment = 100
    }

    /// <summary>
    /// IDs of known properties of log messages. Not each message type has all properties!
    /// </summary>
    public static class PropertyIdentifiers
    {
        // Default
        public const int PROPERTY_TIMESTAMP = 1;
        public const int PROPERTY_MESSAGE = 2;
        public const int PROPERTY_LEVEL = 3;
        public const int PROPERTY_THREAD = 4;
        public const int PROPERTY_LOGGER_NAME = 5;
        public const int PROPERTY_LINE_NR = 6;

        // Optional
        public const int PROPERTY_MODULE_NAME = 100;
        public const int PROPERTY_CPU_NR = 101;
        public const int PROPERTY_USER_PRINCIPAL = 102;
    }


}
