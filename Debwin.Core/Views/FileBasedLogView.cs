using Debwin.Core.LogWriters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Debwin.Core.Views
{

    /// <summary>
    /// Implements a log view that stores the messages in a file on the disk and is not filterable.
    /// </summary>
    public class FileBasedLogView : ILogView, IDisposable
    {
        private ILogWriter _logWriter;
        private Stream _baseStream;
        private bool _isBufferingEnabled;
        private readonly string _filePath;
        private readonly object _fileLock = new object();

        public FileBasedLogView(string filePath, bool enableBuffering, bool appendMode)
        {
            _filePath = filePath;
            EnableBuffering = enableBuffering;
            InitializeFile(filePath, appendMode);
        }


        public string FilePath => _filePath;

        public bool EnableBuffering { get; set; }

        public bool IsOverflowFile { get; set; }

        public FilterDefinition FilterPredicate => null;

        public void AppendMessage(LogMessage message)
        {
            AppendMessageInternal(message, !_isBufferingEnabled);
        }

        public void AppendMessages(IEnumerable<LogMessage> messages)
        {
            foreach (var message in messages)
            {
                AppendMessageInternal(message, false);
            }
            if (!_isBufferingEnabled)
            {
                _baseStream.Flush();
            }
        }

        private void AppendMessageInternal(LogMessage message, bool flushAfterWrite)
        {
            //if (_filterPredicate != null && !_filterPredicate(message))
            //    return;

            lock (_fileLock)
            {
                _logWriter.WriteLogEntry(message, CancellationToken.None);
                if (flushAfterWrite)
                {
                    _baseStream.Flush();
                }
            }
        }

        public void TriggerFlush()
        {
            if (_baseStream == null)
                return;

            _baseStream.Flush();
        }

        private void InitializeFile(string filePath, bool append)
        {
            lock (_fileLock)
            {
                // close existing writer
                if (_baseStream != null)
                {
                    _logWriter.WriteEndLog();
                    _baseStream.Flush();
                    _baseStream.Dispose();
                }

                _baseStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

                if (append)
                {
                    _baseStream.Seek(0, SeekOrigin.End);
                }
                else
                {
                    _baseStream.SetLength(0);
                }

                bool isFirstFileEntry = _baseStream.Length == 0;

                if (EnableBuffering)
                {
                    _isBufferingEnabled = true;
                    _baseStream = new BufferedStream(_baseStream, 10000);
                }

                _logWriter = new Debwin4CsvLogWriter(new StreamWriter(_baseStream, Encoding.UTF8));

                if (isFirstFileEntry)
                    _logWriter.WriteBeginLog();
            }
        }

        public void ClearMessages()
        {
            InitializeFile(_filePath, false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_baseStream != null)
                    {
                        _baseStream.Flush();
                        _baseStream.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
