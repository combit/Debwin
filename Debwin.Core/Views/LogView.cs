using System;
using System.Collections.Generic;

namespace Debwin.Core
{

    /// <summary>An optionally filtered view onto a log (stores [references to] all log messages that meet the conditions of the view.</summary>
    public interface ILogView
    {
        FilterDefinition FilterPredicate { get; }

        void AppendMessage(LogMessage message);

        void AppendMessages(IEnumerable<LogMessage> messages);

        void ClearMessages();

    }

    /// <summary>A log view (<see cref="ILogView"/>) with additional support for retrieving and searching in the contained log messages.</summary>
    public interface IQueryableLogView : ILogView
    {

        int MessageCount { get; }

        LogMessage GetMessage(int index);

        List<LogMessage> GetMessages(int[] indexes);
        LogMessage GetPreviousMessage(LogMessage logMessage);

        /// <summary>Copies the log message with the indices specified in 'selectedIndices' to the target log view.</summary>
        /// <param name="selectedIndices">Limit to the specified indices. If null, all messages are copied.</param>
        void CopyMessagesTo(ILogView targetView, IEnumerable<int> selectedIndices);

        void CopyMessagesListTo(ILogView targetView, IEnumerable<LogMessage> selectedLogMessages);

        int FindIndexOfMessage(bool findLastIndex, int startIndex, Predicate<LogMessage> predicate);

    }

}

namespace Debwin.Core.Views
{
    /// <summary>For log views (<see cref="ILogView"/>) that may limit the number of stored log messages.</summary>
    public interface ISupportsMaximumMessageCount
    {

        /// <summary>Raised when a log view with limited capacity reaches the maximum message count (before removing any obsolete messages).</summary>
        event EventHandler HasReachedMaximumMessageCount;

        /// <summary>Returns true if the view is currently ring-buffering, i.e. dropping old messages when appending new ones.</summary>
        bool IsRingBuffering { get; }
    }


}