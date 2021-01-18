using System;

namespace Debwin.Core
{

    /// <summary>See <see cref="IControlMessage"/>. Extends the control message with a handler that may execute on certain components of the Debwin pipeline.</summary>
    /// <typeparam name="T">Component type that this control message applies to  (e.g. ILogController, ILogView, ...).</typeparam>
    public interface IExecutableControlMessage<T> : IControlMessage
    {
        void ExecuteOn(T component);
    }

    /// <summary>
    /// Defines a special kind of log message that is not intended to be displayed in a log view, but is passed through the Debwin pipeline to control components of it.
    /// </summary>
    public interface IControlMessage
    {
        LogMessage OriginalMessage { get; }
    }

    /// <summary>See <see cref="IControlMessage"/></summary>
    public class GenericControlMessage : IControlMessage
    {
        private readonly LogMessage _originalMessage;

        public LogMessage OriginalMessage { get { return _originalMessage; } }

        public GenericControlMessage(LogMessage originalMessage)
        {
            this._originalMessage = originalMessage;
        }
    }

    public static class ControlMessageFactory
    {

        /// <summary>See <see cref="IExecutableControlMessage{T}"/></summary>
        public static IExecutableControlMessage<ILogView> CreateForLogViews(LogMessage originalMessage, Action<ILogView> handler)
        {
            return new ExecutableControlMessage<ILogView>(originalMessage, handler);
        }

        /// <summary>See <see cref="IExecutableControlMessage{T}"/></summary>
        public static IExecutableControlMessage<ILogController> CreateForLogController(LogMessage originalMessage, Action<ILogController> handler)
        {
            return new ExecutableControlMessage<ILogController>(originalMessage, handler);
        }

        /// <summary>Generic control message (<see cref="IControlMessage"/>) that stores a handler that will be executed on all connected components of type T.</summary>
        private class ExecutableControlMessage<T> : GenericControlMessage, IExecutableControlMessage<T>
        {
            private readonly Action<T> _handler;

            public ExecutableControlMessage(LogMessage originalMessage, Action<T> handler)
            : base(originalMessage)
            {
                this._handler = handler;
            }

            public void ExecuteOn(T target)
            {
                _handler(target);
            }
        }

    }

}
