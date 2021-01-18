using System;

namespace Debwin.Core.MessageSources
{

    public interface ISupportsStartStop
    {
        void Start();

        void Stop();

        bool IsStopped { get; }
    }



    public interface IMessageSource : ISupportsStartStop, IDisposable
    {
        /// <summary>Event which is (only!) triggered when the CurrentProgress value changes.</summary>
        event EventHandler ProgressChanged;

        /// <summary>Returns the current progress of the message source as a value between 0 and 100 percent, or <see cref="Constants.MESSAGESOURCE_PROGRESS_MARQUEE"/> if no progress is available (i.e. remaining length of data is unknown)</summary>
        int CurrentProgress { get; }

        /// <summary>Returns a short name that represents the message source, e.g. a file name or UDP port.</summary>
        string GetName();

        /// <summary>Sets the observer which is called when a new (raw) log message is available.</summary>
        void SetMessageObserver(IMessageSourceObserver handler);

    }
}
