using Debwin.Core.Parsers;
using System;
using System.Text;
using Debwin.Core;
using System.Runtime.InteropServices;
using combit.DebwinExtensions.MessageTypes;
using System.Collections.Generic;

namespace combit.DebwinExtensions.Parsers
{
    public class ListLabelBinaryMessageParser : IMessageParser
    {

        private readonly int _expectedHeaderSize;
        private readonly ListLabelLogHelper _llLogHelper;

        private int _currentIndent;
        private string _currentIndentString;
        public ListLabelBinaryMessageParser()
        {
            _expectedHeaderSize = Marshal.SizeOf(typeof(Debwin4DataPacket));
            _llLogHelper = new Parsers.ListLabelLogHelper();
            _currentIndent = 0;
            _currentIndentString = string.Empty;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct Debwin4DataPacket   // scDEBWIN4DataPacket in LL Core
        {
            public UInt32 HeaderSize;
            public SYSTEMTIME Timestamp;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string ModuleName;
            public UInt32 ThreadID;
            public UInt32 Processor;
            public UInt32 MessageNr;
            public Int32 IndentDelta;
            public UInt32 Severity;
            public UInt32 Category;
            public UInt32 DataLength;

            /* rest of packet is byte[] of payload in unicode-encoding */
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        public IEnumerable<int> GetSupportedMessageTypeCodes()
        {
            return new int[] { ListLabelLogMessage.TYPECODE_LL_MESSAGE };
        }

        public virtual LogMessage CreateMessageFrom(object rawMessage)
        {
            try
            {
                var data = rawMessage as byte[];
                var header = GCHandle.Alloc(data, GCHandleType.Pinned);
                Debwin4DataPacket packet = (Debwin4DataPacket)Marshal.PtrToStructure(header.AddrOfPinnedObject(), typeof(Debwin4DataPacket));
                header.Free();

                if (packet.HeaderSize != _expectedHeaderSize)
                {
                    throw new ArgumentException("Header size of packet does not match size of " + nameof(Debwin4DataPacket) + " structure!");
                }

                //var message = Encoding.Unicode.GetString(data, _expectedHeaderSize, data.Length - _expectedHeaderSize);

                if (packet.IndentDelta < 0 && _currentIndent > 0)   // decrease affects current message
                {
                    _currentIndent += packet.IndentDelta;
                    AdaptIndentation();
                }

                var offset = _expectedHeaderSize; //+ 30 * sizeof(char);
                var message = string.Concat(_currentIndentString, Encoding.Unicode.GetString(data, offset, data.Length - offset));

                if (packet.IndentDelta > 0)   // increase affects next message
                {
                    _currentIndent += packet.IndentDelta;
                    AdaptIndentation();
                }

                return new ListLabelLogMessage()
                {
                    Message = message,
                    Thread = packet.ThreadID.ToString("X"),
                    Level = (LogLevel)(int)Math.Max(1, packet.Severity),
                    Timestamp = new DateTime(packet.Timestamp.wYear, packet.Timestamp.wMonth, packet.Timestamp.wDay, packet.Timestamp.wHour, packet.Timestamp.wMinute, packet.Timestamp.wSecond, packet.Timestamp.wMilliseconds, DateTimeKind.Local),
                    ModuleName = string.Intern(packet.ModuleName),   // as we can have millions of messages, but only a few different module names, use string interning to save memory
                    ProcessorNr = (int)packet.Processor,
                    LoggerName = _llLogHelper.MapCategoryIndexToName(packet.Category)
                };
            }
            catch (Exception e)
            {
                return new LogMessage("Debwin-Error - Could not parse message: " + e.ToString()) { Level = LogLevel.Error };
            }
        }

        private void AdaptIndentation()
        {
            _currentIndentString = new string(' ', _currentIndent);
        }
        public Type GetOutputType()
        {
            return typeof(ListLabelLogMessage);
        }

        public Type[] GetSupportedInputTypes()
        {
            return new Type[] { typeof(byte[]) };
        }
    }


}
