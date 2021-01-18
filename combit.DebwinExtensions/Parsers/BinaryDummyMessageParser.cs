using Debwin.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Debwin.Core;

namespace combit.DebwinExtensions.Parsers
{
    public class BinaryDummyMessageParser : IMessageParser
    {
        public LogMessage CreateMessageFrom(object rawMessage)
        {
            var data = rawMessage as byte[];

            return new LogMessage("received package, length: " + data.Length);
        }

        public IEnumerable<int> GetSupportedMessageTypeCodes()
        {
            return new int[] { LogMessage.TYPECODE_DEFAULT_MESSAGE };
        }
    }
}
