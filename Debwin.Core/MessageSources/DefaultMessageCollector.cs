using Debwin.Core.Parsers;
using System;

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

        public void NotifyNewRawMessage(object rawMessage)
        {
            if (_observer == null)
                return;   // no need to process message if no observer is listening

            LogMessage message = ParseRawMessage(rawMessage);

            if (message != null)   // notify controller that new message is available
            {
                _currentLineNr++;
                message.LineNr = _currentLineNr;
                _observer.NotifyNewLogMessage(this, message);
            }
        }

        protected virtual LogMessage ParseRawMessage(object rawMessage)
        {
            if (Parser == null && !(rawMessage is LogMessage))
                throw new InvalidOperationException("ProcessRawMessage must not be called before setting the MessageParser.");

            LogMessage message;

            try
            {
                message = Parser.CreateMessageFrom(rawMessage);
            }
            catch (Exception e)
            {
                message = new LogMessage("Error while parsing raw message: " + e.ToString());
                message.Level = LogLevel.Error;
            }

            return message;
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
