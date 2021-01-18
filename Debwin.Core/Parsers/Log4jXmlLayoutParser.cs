using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Debwin.Core.Parsers
{
    public class Log4jXmlLayoutParser : IMessageParser
    {

        private readonly XmlReaderSettings _readerSettings;
        private readonly XmlParserContext _parserContext;

        public Log4jXmlLayoutParser()
        {
            _readerSettings = new XmlReaderSettings { NameTable = new NameTable() };
            var xmlns = new XmlNamespaceManager(_readerSettings.NameTable);
            xmlns.AddNamespace("log4j", "http://jakarta.apache.org/log4j/");

            _parserContext = new XmlParserContext(null, xmlns, "", XmlSpace.Default);
        }

        IEnumerable<int> IMessageParser.GetSupportedMessageTypeCodes()
        {
            return new int[] { LogMessage.TYPECODE_DEFAULT_MESSAGE };
        }

        public LogMessage CreateMessageFrom(object rawMessage)
        {
            byte[] data = rawMessage as byte[];
            //int packageNr = (int)(rawMessage as object[])[0];
            string xml = Encoding.UTF8.GetString(data);

            LogMessage result = new LogMessage();
            /* Sample Message:

            <log4j:event 
                logger="RS.UI.DashboardController" 
                level="INFO" 
                timestamp="1462870563451" 
                thread="57">
    
                <log4j:message>Hello World</log4j:message>
                <log4j:properties>
                    <log4j:data name="log4japp" value="/LM/W3SVC/1/ROOT/rs3-2-131073433398896052(8764)" />
                    <log4j:data name="log4jmachinename" value="CMBTLE" />
                </log4j:properties>
            </log4j:event>

            */

            using (var textReader = new StringReader(xml))
            {
                using (XmlReader reader = XmlReader.Create(textReader, _readerSettings, _parserContext))
                {
                    var namespaceManager = new XmlNamespaceManager(reader.NameTable);

                    reader.Read();  // read <log4j:event>

                    // Read arguments of <log4j:event>
                    while (reader.MoveToNextAttribute())
                    {
                        var attributeName = reader.LocalName;
                        switch (attributeName)
                        {
                            case "logger":
                                result.LoggerName = reader.ReadContentAsString();
                                break;
                            case "level":
                                result.Level = MapLevelStringToLevel(reader.ReadContentAsString());
                                break;
                            case "timestamp":
                                result.Timestamp = UnixTimeStampToDateTime(reader.ReadContentAsDouble());
                                break;
                            case "thread":
                                result.Thread = reader.ReadContentAsString();
                                break;
                        }
                    }

                    while (reader.Read())
                    {
                        if (reader.LocalName == "message")
                        {
                            result.Message = reader.ReadElementContentAsString().Replace("\n", Environment.NewLine);   // NLog replaces Windows-style line breaks with \n
                        }
                        else if (reader.LocalName == "properties")
                        {
                            ReadPropertiesNode(reader, result);
                        }
                    }

                    return result;
                }
            }
        }

        private void ReadPropertiesNode(XmlReader reader, LogMessage message)
        {

        }


        private LogLevel MapLevelStringToLevel(string level)
        {
            if (level == "INFO")
                return LogLevel.Info;
            else if (level == "WARN")
                return LogLevel.Warning;
            else if (level == "ERROR")
                return LogLevel.Error;

            return LogLevel.Debug;
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp / 1000).ToLocalTime();
            return dtDateTime;
        }

    }
}
