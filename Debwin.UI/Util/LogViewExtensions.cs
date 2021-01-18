using Debwin.Core;
using Debwin.Core.Views;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Debwin.UI.Util
{
    public static class LogViewExtensions
    {

        public static void EnableLogToFileOnOverflow(this ISupportsMaximumMessageCount logView, ILogController logController, string filePath)
        {
            if (!(logView is IQueryableLogView))
                throw new NotSupportedException("logView must implement IQueryableLogView for this feature.");

            bool isFirstReachOfMaximum = true;
            logView.HasReachedMaximumMessageCount += (sender, e) =>
            {
                // If the log was cleared in the meantime, we must not add another file writer if the message is limit is reached again
                if (!isFirstReachOfMaximum)
                    return;
                isFirstReachOfMaximum = true;

                IQueryableLogView overflowedLogView = (IQueryableLogView)sender;

                try
                {
                    // Do the expensive file initialization within the message loop (this thread has a reader lock), so we miss as few messages as possible while preparing the file logging
                    FileBasedLogView fileLogView;
                    try
                    {
                        bool appendMode = File.Exists(filePath) && DateTime.Now.Subtract(File.GetLastWriteTime(filePath)) < TimeSpan.FromHours(1);
                        fileLogView = new FileBasedLogView(filePath, true, appendMode);
                    }
                    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                    {
                        string alternativePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), ".log4"));
                        fileLogView = new FileBasedLogView(alternativePath, true, false);
                    }
                    fileLogView.IsOverflowFile = true;   // other handling in UI than regular (user-intended) logging to file (warning icon)

                    // As we are within a call from the LogController, we must not access it's methods in the same thread (would cause recursive locks in the log controller)
                    // => Do the lock-requiring operations in a separate thread:
                    new Thread(new ThreadStart(() =>
                    {
                        try
                        {
                            // cRM attaches a file writer via a special control message -> no need to attach a second file
                            if (logController.GetLogViews().Any(view => view is FileBasedLogView))
                                return;

                            overflowedLogView.AppendMessage(new LogMessage("Debwin4: Maximum message buffer size was reached. Creating log backup at: " + filePath) { Level = LogLevel.UserComment, LoggerName = "[DEBWIN4]" });

                            overflowedLogView.CopyMessagesTo(fileLogView, null);
                            logController.AddView(fileLogView);
                        }
                        catch (Exception) {  /* swallow all remaing exceptions as we don't want Debwin to crash in case of an unhandled exception in this background thread */ }
                    }))
                    { Priority = ThreadPriority.Highest }.Start();  // minimize the number of missed messages between the buffer full event and the beginning of the file logging
                    Thread.Yield();

                }
                catch (Exception ex)
                {
                    overflowedLogView.AppendMessage(new LogMessage("Debwin4-Error: Failed to add a file writer for the overflowing log: " + ex.Message) { Level = LogLevel.Error, LoggerName = "[DEBWIN4]" });
                }
            };
        }

    }

}
