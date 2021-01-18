using System.Collections.Generic;

namespace Debwin.Core.Parsers
{
    public interface IMessageParser
    {

        ///// <summary>Returns the type of the LogMessage objects that this parser creates.</summary>
        //Type GetOutputType();

        ///// <summary>Returns the supported types that may be passed as rawMessage objects to <see cref="CreateMessageFrom(object)"/>.</summary>
        //Type[] GetSupportedInputTypes();

        /// <summary></summary>
        LogMessage CreateMessageFrom(object rawMessage);

        /// <summary>
        /// Returns a list of the LogMessage types that this parser might return, in form of the typecodes of those LogMessage sub types (<see cref="LogMessage.GetMessageTypeCode"/>).
        /// </summary>
        IEnumerable<int> GetSupportedMessageTypeCodes();

    }
}
