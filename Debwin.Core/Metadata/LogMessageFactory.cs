using System;
using System.Collections.Generic;

namespace Debwin.Core.Metadata
{

    /// <summary>
    /// Maps the type codes of log messages to factory functions that create instances of these LogMessage types.
    /// New LogMessage subtypes may be registered by the GUI, this allows to parse Debwin4 log files which have LogMessage subtypes that are not part of Debwin.Core.
    /// </summary>
    public static class LogMessageFactory
    {

        private static readonly Dictionary<int, Func<LogMessage>> _logMessageFactories;

        static LogMessageFactory()
        {
            _logMessageFactories = new Dictionary<int, Func<LogMessage>>();
            _logMessageFactories.Add(LogMessage.TYPECODE_DEFAULT_MESSAGE, () => new LogMessage());
        }


        public static Func<LogMessage> GetFactoryForTypeCode(int typeCode)
        {
            Func<LogMessage> result;
            if (!_logMessageFactories.TryGetValue(typeCode, out result))
            {
                result = _logMessageFactories[LogMessage.TYPECODE_DEFAULT_MESSAGE];
            }

            return result;
        }

        /// <summary>
        /// Registers a constructor that creates LogMessage objects of the specified type (see <see cref="LogMessage.GetMessageTypeCode"/>).
        /// </summary>
        public static void RegisterFactory(int logMessageTypeCode, Func<LogMessage> factory)
        {
            _logMessageFactories.Add(logMessageTypeCode, factory);
        }

        public static IEnumerable<int> GetKnownMessageTypeCodes()
        {
            return _logMessageFactories.Keys;
        }
    }
}
