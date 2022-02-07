using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Debwin.Core.Views
{
    /// <summary>
    /// Represents a view to a collection of log messages. While a root view lists all available messages, filtered views usually contain a subset of an other view.
    /// </summary>
    public class MemoryBasedLogView : IQueryableLogView, ISupportsMaximumMessageCount
    {

        public event EventHandler HasReachedMaximumMessageCount;

        private readonly Func<LogMessage, bool> _filterPredicate;
        private readonly IMemoryBasedList<LogMessage> _messages;
        private readonly object _messagesLock = new object();
        private readonly int _maximumMessageCount;
        private Dictionary<int, LogMessage> _messageDictionary = new Dictionary<int, LogMessage>();


        /// <param name="maximumMessageCount">-1 to allow an unlimited number of messages in this view, >= 0 for a ring-buffer behaviour where old message are dropped when the limit is reached.</param>
        public MemoryBasedLogView(int maximumMessageCount)
        {
            const int initialCapacity = 10000;
            if (maximumMessageCount != -1)
            {
                _maximumMessageCount = maximumMessageCount;
                _messages = new RingBufferList<LogMessage>(initialCapacity, maximumMessageCount);
                (_messages as RingBufferList<LogMessage>).HasReachedMaximumMessageCount += new EventHandler((sender, e) => HasReachedMaximumMessageCount?.Invoke(this, e));
            }
            else
            {
                _maximumMessageCount = -1;
                _messages = new UnlimitedList<LogMessage>(initialCapacity);
            }
            _messagesLock = new object();
        }

        public FilterDefinition FilterPredicate { get; }

        public MemoryBasedLogView(IQueryableLogView logViewToClone, FilterDefinition filterDefinition)
            : this(logViewToClone is MemoryBasedLogView ? ((MemoryBasedLogView)logViewToClone)._maximumMessageCount : -1)
        {
            FilterPredicate = filterDefinition;
            if (filterDefinition != null)
                _filterPredicate = filterDefinition.BuildPredicate();

            logViewToClone.CopyMessagesTo(this, null);
        }

        public void CopyMessagesTo(ILogView targetView)
        {
            CopyMessagesTo(targetView, null);
        }


        public void CopyMessagesTo(ILogView targetView, IEnumerable<int> selectedIndices)
        {
            lock (this._messagesLock)
            {
                if (selectedIndices == null)
                {
                    targetView.AppendMessages(this._messages);
                }
                else
                {
                    foreach (var selectedIndex in selectedIndices)
                    {
                        targetView.AppendMessage(this._messages[selectedIndex]);
                    }
                }
            }
        }

        public void CopyMessagesListTo(ILogView targetView, IEnumerable<LogMessage> selectedLogMessages)
        {
            lock (this._messagesLock)
            {
                if (selectedLogMessages == null)
                {
                    targetView.AppendMessages(this._messages);
                }
                else
                {
                    foreach (var selectedLogMessage in selectedLogMessages)
                    {
                        targetView.AppendMessage(selectedLogMessage);
                    }
                }
            }
        }


        private void AppendMessageInternal(LogMessage message)
        {
            _messages.Add(message);
            if (message.Level != LogLevel.UserComment)
            {
                _messageDictionary.Add(message.LineNr, message);
            }
        }


        public void AppendMessages(IEnumerable<LogMessage> messages)
        {
            if (_filterPredicate != null)
            {
                messages = messages.AsParallel().AsOrdered().Where(_filterPredicate);
            }

            lock (_messagesLock)
            {
                foreach (var newMsg in messages)
                {
                    AppendMessageInternal(newMsg);
                }
            }

            //if (_observer != null)
            //    _observer.NotifyMessagesChanged();
        }

        public void AppendMessage(LogMessage message)
        {
            if (_filterPredicate != null && !_filterPredicate(message))
                return;

            lock (_messagesLock)
            {
                AppendMessageInternal(message);
            }

            //if (_observer != null)
            //    _observer.NotifyMessagesChanged();
        }

        public int FindIndexOfMessage(bool findLastIndex, int startIndex, Predicate<LogMessage> predicate)
        {
            lock (_messagesLock)
            {
                if (findLastIndex)
                {
                    return _messages.FindLastIndex(startIndex, predicate);
                }
                else
                {
                    return _messages.FindIndex(startIndex, predicate);
                }
            }
        }

        public int MessageCount
        {
            get
            {
                return _messages.Count;
            }
        }

        public LogMessage GetMessage(int index)
        {
            lock (_messagesLock)
            {
                if (index < 0 || index >= _messages.Count)
                    return null;

                return _messages[index];
            }
        }

        public List<LogMessage> GetMessages(int[] indexes)
        {
            lock (_messagesLock)
            {
                List<LogMessage> selectedMessages = new List<LogMessage>();
                foreach (int index in indexes)
                {
                    if (index < 0 || index >= _messages.Count)
                        return null;

                    selectedMessages.Add(_messages[index]);
                }
                return selectedMessages;
            }
        }

        public LogMessage GetPreviousMessage(LogMessage logMessage)
        {
            lock (_messagesLock)
            {
                if (logMessage.LineNr == 1)
                {
                    return logMessage;
                }
                else
                {
                    return _messageDictionary[logMessage.LineNr - 1];
                }
            }
        }

        public void ClearMessages()
        {
            lock (_messagesLock)
            {
                _messages.Clear();
            }

            //if (_observer != null)
            //    _observer.NotifyMessagesChanged();
        }

        public bool IsRingBuffering => _messages.IsRingBuffering;


        private interface IMemoryBasedList<T> : IList<T>
        {
            int FindLastIndex(int startIndex, Predicate<T> predicate);

            int FindIndex(int startIndex, Predicate<T> predicate);

            bool IsRingBuffering { get; }
        }


        /// <summary>
        /// Helper class to wrap List<T> with ISearchableList interface.
        /// Needs external locking for multi-threaded use!
        /// </summary>
        private class UnlimitedList<T> : List<T>, IMemoryBasedList<T>
        {
            public UnlimitedList(int initialSize)
                : base(initialSize) { }

            public bool IsRingBuffering => false;
        }

        /// <summary>
        /// Array-based collection class that implements a ring-buffer behaviour. Note that some modifying operations are not supported.
        /// (When the list has grown to the maximum item count, the oldest item is removed before appending a new item.)
        /// Needs external locking for multi-threaded use!
        /// </summary>
        private class RingBufferList<T> : IMemoryBasedList<T>, ISupportsMaximumMessageCount
        {
            private readonly List<T> _list;
            private int _currentStartOfList;
            private int _maxSize;
            private bool _hasReachedMaxMessageCount;

            public event EventHandler HasReachedMaximumMessageCount;

            public RingBufferList(int initialSize, int maxSize)
            {
                _currentStartOfList = 0;
                _maxSize = maxSize;
                _list = new List<T>(initialSize);
            }

            private int MapVirtualToPhysicalIndex(int virtualIndex)
            {
                if (virtualIndex < 0 || virtualIndex >= _list.Count)
                    throw new IndexOutOfRangeException();

                return (_currentStartOfList + virtualIndex) % _maxSize;
            }

            private int MapPhysicalToVirtualIndex(int baseIndex)
            {
                if (baseIndex >= _currentStartOfList)
                {
                    return baseIndex - _currentStartOfList;
                }
                else
                {
                    return _list.Count - _currentStartOfList + baseIndex;
                }
            }

            public void Add(T item)
            {
                // handle maxSize = 0  (HasReachedMaximumMessageCount event should still be raised on first message)
                if (_maxSize == 0)
                {  
                    if (!_hasReachedMaxMessageCount && HasReachedMaximumMessageCount != null)  // needs to be separate condition!  if(maxSize==0) must not have other conditions so the else blocks are never entered for maxSize=0
                    {
                        _hasReachedMaxMessageCount = true;
                        HasReachedMaximumMessageCount(this, new EventArgs());
                    }
                }

                // adding with remaining places
                else if (_list.Count < _maxSize)
                {
                    _list.Add(item);

                    // Notify parent of the full log, can get special handling by the controller/UI
                    if (_list.Count == _maxSize && !_hasReachedMaxMessageCount && HasReachedMaximumMessageCount != null)
                    {
                        _hasReachedMaxMessageCount = true;
                        HasReachedMaximumMessageCount(this, new EventArgs());
                    }
                }

                // adding to full list -> replace oldest item
                else
                {
                    _list[_currentStartOfList] = item;   // Override the item right before the new first item of the list
                    _currentStartOfList = (_currentStartOfList + 1) % _maxSize;
                }
            }

            public T this[int index]
            {
                get { return _list[MapVirtualToPhysicalIndex(index)]; }
                set { throw new NotSupportedException(); }
            }


            public void Clear()
            {
                _currentStartOfList = 0;
                _list.Clear();
            }

            public int Count => _list.Count;

            public bool IsReadOnly => false;

            public bool Contains(T item)
            {
                return _list.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    array[arrayIndex + i] = _list[MapVirtualToPhysicalIndex(i)];
                }
            }

            public int IndexOf(T item)
            {
                return FindIndex(0, candidate => candidate.Equals(item));
            }

            public int FindLastIndex(int startIndex, Predicate<T> predicate)
            {
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException();

                if (_list.Count == 0)
                    return -1;

                if (startIndex >= _list.Count)
                    throw new ArgumentOutOfRangeException();

                for (int i = startIndex; i >= 0; i--)
                {
                    if (predicate(_list[MapVirtualToPhysicalIndex(i)]))
                        return i;
                }
                return -1;
            }

            public int FindIndex(int startIndex, Predicate<T> predicate)
            {
                if (startIndex < 0 || startIndex > _list.Count)
                    throw new ArgumentOutOfRangeException();

                for (int i = startIndex; i < _list.Count; i++)
                {
                    if (predicate(_list[MapVirtualToPhysicalIndex(i)]))
                        return i;
                }
                return -1;
            }

            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    yield return _list[MapVirtualToPhysicalIndex(i)];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public bool IsRingBuffering => (this._maxSize == _list.Count);

            #region "Unsupported Operations"

            public void Insert(int index, T item)
            {
                throw new NotSupportedException();
            }

            public bool Remove(T item)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            #endregion

        }
    }
}