using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debwin.Core.Parsers
{
    public class BasicStringMessageParser : IMessageParser
    {
        public LogMessage CreateMessageFrom(object rawMessage)
        {
            return new LogMessage(rawMessage as string);
        }

        IEnumerable<int> IMessageParser.GetSupportedMessageTypeCodes()
        {
            return new int[] { LogMessage.TYPECODE_DEFAULT_MESSAGE };
        }
    }
}
