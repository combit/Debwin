using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debwin.Core.Parsers
{
    public class BasicStringMessageParser : IMessageParser
    {
        public IList<LogMessage> CreateMessageFrom(object rawMessage)
        {
            return new List<LogMessage>() { new LogMessage(rawMessage as string) };
        }

        IEnumerable<int> IMessageParser.GetSupportedMessageTypeCodes()
        {
            return new int[] { LogMessage.TYPECODE_DEFAULT_MESSAGE };
        }
    }
}
