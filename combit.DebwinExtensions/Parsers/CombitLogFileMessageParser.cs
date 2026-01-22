using combit.DebwinExtensions.MessageTypes;
using Debwin.Core;
using Debwin.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;

namespace combit.DebwinExtensions.Parsers
{
    public class CombitLogFileMessageParser : IMessageParser
    {

        private LogMessage _lastCreatedMessage = null;
        private bool _isFirstMessage = true;
        private readonly ListLabelLogHelper _llLogHelper;

        public CombitLogFileMessageParser()
        {
            _llLogHelper = new Parsers.ListLabelLogHelper();
        }

        public IEnumerable<int> GetSupportedMessageTypeCodes()
        {
            return new int[] { ListLabelLogMessage.TYPECODE_LL_MESSAGE };
        }

        public IList<LogMessage> CreateMessageFrom(object rawMessage)
        {
            var msg = rawMessage as string;
            int msgType;

            // Ignore Debwin3 header:
            try
            {
                if (_isFirstMessage && msg.StartsWith("Welcome to DEBWIN V. 3"))
                    return null;
            }
            finally
            {
                _isFirstMessage = false;
            }

            // Ignore newlines (e. g. from stacktraces)
            if (String.IsNullOrEmpty(msg))
            {
                return null;
            }

            // Formats to expect:

            // (0) Log message with category and level (LL22 log written by LL)
            // CMLL22  : 11:20:31.117 0000101c/00 2 [L02 API]:   >LlGetOption(1,129)

            // (1) Log message with (text) category and optional level (LL22 log written by Debwin3)
            // CMLL22  : 09:15:34.222 00001bbc/00 6 [LIC] clsLicenseInfo::GetStateFromSerno()

            // (2) Other combit modules or LL versions < 22
            // CMLL21  : 09:14:32.122 00002328/00 2   User is Admin

            // (3) cRM Format
            // cRM:01:05:24.368 000016E8       CDARecord::ReadFieldsPhysicallyByPK(0)

            // (4) cRM Format (alternative, from cRMDebug.exe)
            // 14:57:27.517 00000B3C      >CDAFwCmnd::ExecuteSQLCmndParam(..., 0)

            // (5) New Util-format
            // CMLL31  : 2025-10-27 11:20:37.150 000{chk}thread 'CMLL31::LLXLoadProxy' was alive for 61 ms (UserMode: 0 ms, KernelMode: 0 ms)

            // (-1) Invalid Format / Wrapped lines of earlier messages:
            // /CMBTLE                 :          0, 8,


            // Detect format
            if (msg.Length > "CMLL22  : 09:15:34.222 00001bbc/00 6 [???]".Length && msg[8] == ':')   // cases (0), (1), (2) or (5)
            {
                if (msg.Length > "CMLL22  : 2025-10-27 09:15:34.222 000".Length && msg[14] == '-' && msg[17] == '-') // case (5)
                    msgType = 5;
                else if (msg.Length > "CMLL22  : 11:20:31.117 0000101c/00 2 [L02 API]".Length && msg[37] == '[' && msg[38] == 'L' && msg[45] == ']')  // has [L02 C01] block?  (level 2, category 1)
                    msgType = 0;
                else if (msg[37] == '[' && msg[41] == ']')  // has text category? E.g. "[EXP]"
                    msgType = 1;
                else
                    msgType = 2;
            }
            else if (msg.Length > 21 && msg[2] == ':' && msg[5] == ':' && msg[8] == '.' && msg[12] == ' ')  // case (4)
            {
                msgType = 4;
            }
            else if (msg.StartsWith("cRM:"))   // case (3)
            {
                msgType = 3;
            }
            else   // case (-1) unknown format 
            {
                msgType = -1;
            }

            List<LogMessage> logMessages = new List<LogMessage>();
            // Handle (-1) - Append to an earlier message or create unparsed message
            if (msgType == -1)
            {
                logMessages.Add(HandleNonStandardMessage(msg));
            }
            // Handle (1) and (2):
            else if (msgType == 0 || msgType == 1 || msgType == 2 || msgType == 5)
            {
                logMessages.Add(ParseLLMessage(msg, msgType));
            }
            else if (msgType == 3)
            {
                logMessages.Add(ParseCRMMessage(msg, true));
            }
            else if (msgType == 4)
            {
                logMessages.Add(ParseCRMMessage(msg, false));
            }

            if (logMessages.Count > 0 && logMessages[0] != null)
            {
                _lastCreatedMessage = logMessages[0];
                return logMessages;
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        private LogMessage ParseCRMMessage(string msg, bool hasCrmPrefix)
        {
            string module = "cRM";

            DateTime? timestamp = null;
            if (!ParseTimestamp(msg, hasCrmPrefix ? "cRM:".Length : 0, false, out timestamp))
                return HandleNonStandardMessage(msg);

            string thread = msg.Substring(hasCrmPrefix ? 17 : 13, 8);
            string text = text = msg.Substring(hasCrmPrefix ? 26 : 22);

            var result = new ListLabelLogMessage()
            {
                Message = text,
                ModuleName = module,
                Thread = thread,
                ProcessorNr = -1,
                Timestamp = timestamp.Value,
                Level = LogLevel.Debug
            };

            if (result.Message.StartsWith("WRN:"))
                result.Level = LogLevel.Warning;
            else if (result.Message.StartsWith("ERR:"))
                result.Level = LogLevel.Error;

            return result;
        }

        private LogMessage ParseLLMessage(string msg, int msgType)
        {
            string module = msg.Substring(0, 8).TrimEnd(' ');
            bool hasDate = (msgType == 5);

            DateTime? timestamp = null;
            if (!ParseTimestamp(msg, "CMLL22  : ".Length, hasDate, out timestamp))
                return HandleNonStandardMessage(msg);

            string thread = msg.Substring(hasDate ? 23+11 : 23, 8);
            string cpu = msg.Substring(hasDate ? 32+11 : 32, 2);

            string category;
            string text;
            LogLevel level = LogLevel.Info;

            if (msgType == 0)  // CMLL22  : 11:20:31.117 0000101c/00 2 [L02 API]:   >LlGetOption(1,129)
            {
                category = _llLogHelper.MapCategoryAbbreviationToName(msg.Substring(42, 3));
                level = (LogLevel)(msg[40] - '0');
                text = msg.Substring(47);
            }
            else if (msgType == 1)   // CMLL22  : 09:15:34.222 00001bbc/00 6 [LIC] WRN: clsLicenseInfo::GetStateFromSerno()
            {
                category = _llLogHelper.MapCategoryAbbreviationToName(msg.Substring(38, 3));
                text = msg.Substring(43);
            }
            else if (msgType == 2)   // CMLL21  : 09:14:32.122 00002328/00 2   User is Admin
            {
                category = "-";
                text = msg.Substring(37);   // starts at 38 in LL21, and 37 in LL22+
            }
            else if (msgType == 5) // CMLL31  : 2025-10-27 11:20:37.150 000{chk}thread 'CMLL31::LLXLoadProxy' was alive for 61 ms (UserMode: 0 ms, KernelMode: 0 ms)
            {
                category = "-";
                text = msg.Substring(48);
            }
            else
            {
                throw new NotImplementedException("Unknown msgType for ParseLLMessage()");
            }

            if (msgType != 0)  // Debwin3/LL21: Warnings and errors are prefixed with WRN/ERR
            {
                string trimmedText = text.TrimStart(' ');
                if (trimmedText.StartsWith("WRN"))
                    level = LogLevel.Warning;
                else if (trimmedText.StartsWith("ERR"))
                    level = LogLevel.Error;
            }

            return new ListLabelLogMessage()
            {
                Level = level,
                Message = text,
                LoggerName = category,
                ModuleName = module,
                Thread = thread,
                ProcessorNr = int.Parse(cpu, NumberStyles.HexNumber),
                Timestamp = timestamp.Value
            };
        }


        private LogMessage HandleNonStandardMessage(string msg)
        {
            if (_lastCreatedMessage != null)
            {
                return new LogMessage(_lastCreatedMessage)
                {
                    Message = msg,
                };
            }

            return null;
        }

        private bool ParseTimestamp(string msg, int offset, bool withDate, out DateTime? timestamp)
        {
            timestamp = null;
            int hour = 0, min = 0, sec = 0, msec = 0;
            int year = 1, month = 1, day = 1; // defaults if date not given

            if (withDate)
            {
                // Expected format: yyyy-MM-dd HH:mm:ss.fff
                if (msg.Length < offset + 23)
                    return false;

                if (!int.TryParse(msg.Substring(offset, 4), NumberStyles.None, CultureInfo.InvariantCulture, out year))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 5, 2), NumberStyles.None, CultureInfo.InvariantCulture, out month))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 8, 2), NumberStyles.None, CultureInfo.InvariantCulture, out day))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 11, 2), NumberStyles.None, CultureInfo.InvariantCulture, out hour))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 14, 2), NumberStyles.None, CultureInfo.InvariantCulture, out min))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 17, 2), NumberStyles.None, CultureInfo.InvariantCulture, out sec))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 20, 3), NumberStyles.None, CultureInfo.InvariantCulture, out msec))
                    return false;
            }
            else
            {
                // Expected format: HH:mm:ss.fff
                if (msg.Length < offset + 12)
                    return false;

                if (!int.TryParse(msg.Substring(offset, 2), NumberStyles.None, CultureInfo.InvariantCulture, out hour))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 3, 2), NumberStyles.None, CultureInfo.InvariantCulture, out min))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 6, 2), NumberStyles.None, CultureInfo.InvariantCulture, out sec))
                    return false;

                if (!int.TryParse(msg.Substring(offset + 9, 3), NumberStyles.None, CultureInfo.InvariantCulture, out msec))
                    return false;
            }

            try
            {
                timestamp = new DateTime(year, month, day, hour, min, sec, msec, DateTimeKind.Local);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class ListLabelLogHelper
    {
        private readonly Dictionary<string, string> _abbrevToCategoryNameMapping;
        private readonly string[] _indexToCategoryNameMapping;

        public ListLabelLogHelper()
        {
            _abbrevToCategoryNameMapping = new Dictionary<string, string>();
            _abbrevToCategoryNameMapping.Add("GEN", "LL.Generic");
            _abbrevToCategoryNameMapping.Add("TMP", "LL.Temporary");
            _abbrevToCategoryNameMapping.Add("API", "LL.API");
            _abbrevToCategoryNameMapping.Add("N2S", "LL.Native2SqlTranslation");
            _abbrevToCategoryNameMapping.Add("LIC", "LL.Licensing");
            _abbrevToCategoryNameMapping.Add("EXT", "LL.NetFX");
            _abbrevToCategoryNameMapping.Add("PRN", "LL.Printer");
            _abbrevToCategoryNameMapping.Add("EML", "LL.Mail");
            _abbrevToCategoryNameMapping.Add("NTF", "LL.Notification");
            _abbrevToCategoryNameMapping.Add("SYS", "LL.SystemInfo");
            _abbrevToCategoryNameMapping.Add("DOM", "LL.DOM");
            _abbrevToCategoryNameMapping.Add("DAT", "LL.DataProvider");
            _abbrevToCategoryNameMapping.Add("PRV", "LL.Storage");
            _abbrevToCategoryNameMapping.Add("OBJ", "LL.ObjectStates");
            _abbrevToCategoryNameMapping.Add("EXP", "LL.Export");
            _abbrevToCategoryNameMapping.Add("WDE", "LL.Webdesigner");

            _indexToCategoryNameMapping = new string[]
               {
                /* [0] */ "LL.Generic",
                /* [1] */ "LL.API",
                /* [2] */ "LL.Printer",
                /* [3] */ "LL.Licensing",
                /* [4] */ "LL.NetFX",
                /* [5] */ "LL.Mail",
                /* [6] */ "LL.Native2SqlTranslation",
                /* [7] */ "LL.Notification",
                /* [8] */ "LL.SystemInfo",
                /* [9] */ "LL.DOM",
                /* [10] */ "LL.DataProvider",
                /* [11] */ "LL.Storage",
                /* [12] */ "LL.ObjectStates",
                /* [13] */ "LL.Export",
                /* [14] */ "LL.Webdesigner",
                /* [15] */ "LL.Internal"
               };
        }

        public string MapCategoryAbbreviationToName(string abbrev)
        {
            string categoryName;
            if (_abbrevToCategoryNameMapping.TryGetValue(abbrev, out categoryName))
            {
                return categoryName;
            }

            return abbrev;
        }

        public string MapCategoryIndexToName(uint category)
        {
            if (category < 0 || category >= _indexToCategoryNameMapping.Length)
                category = 0;

            return _indexToCategoryNameMapping[category];
        }

    }

}
