using Debwin.Core.MessageSources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace Debwin.Core
{

    public interface ILogController : IDisposable, INotifyPropertyChanged
    {

        event EventHandler<AddOrRemoveMessageCollectorEventArgs> AddedMessageCollector;
        event EventHandler<AddOrRemoveMessageCollectorEventArgs> RemovedMessageCollector;
        event EventHandler<MessageSourceErrorEventArgs> MessageSourceError;
        event EventHandler<ReceivedControlMessageEventArgs> ReceivedControlMessage;

        string Name { get; set; }

        void AddMessageCollector(IMessageCollector collector);

        ReadOnlyCollection<IMessageCollector> GetMessageCollectors();

        void AddView(ILogView view);

        IEnumerable<ILogView> GetLogViews();

        void RemoveLogView(ILogView log);

        long TotalReceivedMessages { get; }
    }

    public class AddOrRemoveMessageCollectorEventArgs : EventArgs
    {
        public IMessageCollector MessageCollector { get; set; }
    }

    public class MessageSourceErrorEventArgs : EventArgs
    {
        public Exception MessageSourceError { get; set; }
    }

    public class ReceivedControlMessageEventArgs : EventArgs
    {
        public IControlMessage ControlMessage { get; set; }
    }


    public class LogController : ILogController, IMessageCollectorObserver
    {

        public event EventHandler<AddOrRemoveMessageCollectorEventArgs> AddedMessageCollector;
        public event EventHandler<AddOrRemoveMessageCollectorEventArgs> RemovedMessageCollector;
        public event EventHandler<MessageSourceErrorEventArgs> MessageSourceError;
        public event EventHandler<ReceivedControlMessageEventArgs> ReceivedControlMessage;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly List<IMessageCollector> _messageCollectors;
        private readonly List<ILogView> _logViews;
        private readonly ReaderWriterLockSlim _logViewsCollectionLock;
        private long _totalReceivedMessages;

        private string _controllerName;

        public LogController()
        {
            _messageCollectors = new List<IMessageCollector>(1);
            _logViewsCollectionLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            _logViews = new List<ILogView>(1);
            _totalReceivedMessages = 0;
        }

        public string Name
        {
            get
            {
                return _controllerName;
            }
            set
            {
                _controllerName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public void AddMessageCollector(IMessageCollector collector)
        {
            collector.SetObserver(this);
            _messageCollectors.Add(collector);

            if (AddedMessageCollector != null)
                AddedMessageCollector(this, new AddOrRemoveMessageCollectorEventArgs() { MessageCollector = collector });
        }


        public ReadOnlyCollection<IMessageCollector> GetMessageCollectors()
        {
            return _messageCollectors.AsReadOnly();
        }

        void IMessageCollectorObserver.NotifyNewLogMessage(IMessageCollector sender, LogMessage logMessage)
        {
            try
            {
                // NullReferneceException without locking: Although writes to the _logViews list don`t appear often, with the frequency of some message sources,
                // there was a high probability that List.Count was increased before the item was available in the list`s internal array (see .NET reference source)
                _logViewsCollectionLock.EnterReadLock();
                _totalReceivedMessages++;

                for (int i = 0; i < _logViews.Count; i++)
                {
                    ILogView currentView = _logViews[i];

                    currentView.AppendMessage(logMessage);
                }
            }
            finally
            {
                _logViewsCollectionLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Handles special log messages that control Debwin or contain other commands.
        /// </summary>
        public void NotifyControlMessage(IControlMessage controlMessage)
        {
            if (controlMessage is IExecutableControlMessage<ILogView> logViewCtlMessage)   // control message for ILogViews
            {
                try
                {
                    _logViewsCollectionLock.EnterReadLock();

                    foreach (ILogView logView in _logViews)
                    {
                        logViewCtlMessage.ExecuteOn(logView);
                    }
                }
                finally
                {
                    _logViewsCollectionLock.ExitReadLock();
                }
            }
            else if (controlMessage is IExecutableControlMessage<ILogController> logControllerCtlMessage)   // control message for ILogController
            {
                logControllerCtlMessage.ExecuteOn(this);
            }
            ReceivedControlMessage?.Invoke(this, new ReceivedControlMessageEventArgs() { ControlMessage = controlMessage });
        }

        public void NotifyCollectorError(Exception e)
        {
            MessageSourceError?.Invoke(this, new MessageSourceErrorEventArgs() { MessageSourceError = e });
        }

        public long TotalReceivedMessages => _totalReceivedMessages;

        public void AddView(ILogView logView)
        {
            try
            {
                _logViewsCollectionLock.EnterWriteLock();
                _logViews.Add(logView);
            }
            finally
            {
                _logViewsCollectionLock.ExitWriteLock();
            }
        }

        public IEnumerable<ILogView> GetLogViews()
        {
            try
            {
                _logViewsCollectionLock.EnterReadLock();

                List<ILogView> collectionClone = new List<ILogView>(_logViews);  // create a clone, so the returned object can be used outside of the lock
                return collectionClone;
            }
            finally
            {
                _logViewsCollectionLock.ExitReadLock();
            }
        }


        public void RemoveLogView(ILogView log)
        {
            //log.SetObserver(null);
            try
            {
                _logViewsCollectionLock.EnterWriteLock();
                _logViews.Remove(log);
            }
            finally
            {
                _logViewsCollectionLock.ExitWriteLock();
            }
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var collector in _messageCollectors)
                    {
                        this.RemovedMessageCollector?.Invoke(this, new Core.AddOrRemoveMessageCollectorEventArgs() { MessageCollector = collector });
                        collector.Dispose();
                    }
                    _messageCollectors.Clear();

                    foreach (var logView in _logViews)
                    {
                        if (logView is IDisposable disposable)
                            disposable.Dispose();
                    }
                    _logViews.Clear();
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
