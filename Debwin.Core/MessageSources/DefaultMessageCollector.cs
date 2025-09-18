using Debwin.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debwin.Core.MessageSources
{

    public class DefaultMessageCollector : IMessageCollector
    {

        private IMessageSource _messageSource;
        private IMessageCollectorObserver _observer;
        private int _currentLineNr = 0;

        public IMessageParser Parser { get; set; }

        public IMessageSource Source
        {
            get { return _messageSource; }
            set
            {
                _messageSource = value;

                if (_messageSource != null)
                    _messageSource.SetMessageObserver(this);
            }
        }

        protected IMessageCollectorObserver Observer { get { return _observer; } }

        private static string NormalizeNewlines(string input)
        {
            if (!input.Contains('\r'))
            {
                return input;
            }

            StringBuilder builder = new StringBuilder(input.Length);
            int length = input.Length;

            for (int i = 0; i < length; i++)
            {
                char current = input[i];

                if (current == '\r')
                {
                    builder.Append('\n');

                    // Skip the '\n' in a CRLF sequence
                    if (i + 1 < length && input[i + 1] == '\n')
                    {
                        i++;
                    }
                }
                else
                {
                    builder.Append(current);
                }
            }

            return builder.ToString();
        }

        public void NotifyNewRawMessage(object rawMessage)
        {
            if (_observer == null)
                return;   // no need to process logMessages if no observer is listening

            IList<LogMessage> logMessages = ParseRawMessage(rawMessage);

            if (logMessages == null)
            {
                // might be the header line of a log file
                return;
            }

            foreach (var message in logMessages)
            {
                if (message != null)   // notify controller that new logMessages is available
                {
                    message.Message = NormalizeNewlines(message.Message);

                    // Determine the lines to process: if there are no newline characters, use the original logMessages.
                    string[] lines = message.Message.IndexOf('\n') < 0
                        ? new[] { message.Message }
                        : message.Message.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length > 0)
                    {
                        // Use the original logMessages for the first line.
                        message.Message = lines[0];
                        _currentLineNr++;
                        message.LineNr = _currentLineNr;
                        _observer.NotifyNewLogMessage(this, message);

                        // For each additional line, create a copy via the Clone method.
                        for (int i = 1; i < lines.Length; i++)
                        {
                            // Assume Message has a copy constructor.
                            var newMessage = message.Clone();
                            newMessage.Message = lines[i];
                            _currentLineNr++;
                            newMessage.LineNr = _currentLineNr;
                            _observer.NotifyNewLogMessage(this, newMessage);
                        }
                    }
                }
            }
        }

        protected virtual IList<LogMessage> ParseRawMessage(object rawMessage)
        {
            if (Parser == null && !(rawMessage is LogMessage))
                throw new InvalidOperationException("ProcessRawMessage must not be called before setting the MessageParser.");

            IList<LogMessage> logMessages;

            try
            {
                logMessages = Parser.CreateMessageFrom(rawMessage);
            }
            catch (Exception e)
            {
                LogMessage errorMessage = new LogMessage("Error while parsing raw logMessages: " + e.ToString());
                errorMessage.Level = LogLevel.Error;
                logMessages = new List<LogMessage> { errorMessage };
            }

            return logMessages;
        }

        public void NotifyMessageSourceError(Exception e)
        {
            if (_observer == null)
                return;

            _observer.NotifyCollectorError(e);
        }


        public void SetObserver(IMessageCollectorObserver observer)
        {
            _observer = observer;
        }

        public virtual void Start()
        {
            Source.Start();

        }

        public virtual void Stop()
        {
            Source.Stop();
        }

        public bool IsStopped { get { return Source.IsStopped; } }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Source != null)
                    {
                        Source.SetMessageObserver(null);
                        Source.Dispose();
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
