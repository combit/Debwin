using Debwin.Core.Parsers;
using System;

namespace Debwin.Core.MessageSources
{

    public interface IMessageSourceObserver
    {

        void NotifyNewRawMessage(object rawMessage);

        void NotifyMessageSourceError(Exception e);
    }

    public interface IMessageCollectorObserver
    {

        void NotifyNewLogMessage(IMessageCollector sender, LogMessage logMessage);

        /// <summary>
        /// Notifies of special log messages that control components (of type T) in the Debwin pipeline and execute handlers on that component.
        /// </summary>
        void NotifyControlMessage(IControlMessage controlMessage);

        void NotifyCollectorError(Exception e);

    }

    public interface IMessageCollector : ISupportsStartStop, IMessageSourceObserver, IDisposable
    {

        IMessageSource Source { get; }

        IMessageParser Parser { get; }


        void SetObserver(IMessageCollectorObserver observer);

    }
}
