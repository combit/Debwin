using System;
using System.Threading;

namespace Debwin.Core.LogWriters
{
    public interface ILogWriter
    {
        void WriteBeginLog();

        void WriteEndLog();

        void WriteLogEntry(LogMessage message, CancellationToken cancelToken);

    }


    public static class LogWriterHelper
    {

        public static void WriteTo(this IQueryableLogView logView, ILogWriter writer, CancellationToken cancelToken, Action<int> progressChangedCallback)
        {
            int processedMessages = 0;
            int lastProgress = 0;

            writer.WriteBeginLog();

            for (int i = 0; i < logView.MessageCount; i++)
            {
                if (cancelToken.IsCancellationRequested)
                    break;

                LogMessage msg = logView.GetMessage(i);

                writer.WriteLogEntry(msg, cancelToken);

                // Calculate progress
                processedMessages++;
                if (logView.MessageCount > 0 && progressChangedCallback != null)
                {
                    int progress = (100 * processedMessages / logView.MessageCount);
                    if (progress != lastProgress)
                    {
                        progressChangedCallback(progress);
                    }
                    lastProgress = progress;
                }
            }

            writer.WriteEndLog();
        }
    }
}
