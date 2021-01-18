using System;

namespace Debwin.Core.Metadata
{

    /// <summary>
    /// Describes the properties of the log messages of a message type.
    /// This information allows the UI to automatically create suitable columns and filters for the messages of a certain type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class LogMessageMetadataProviderAttribute : Attribute
    {
        private readonly Type _rawMessageOutputType;

        public LogMessageMetadataProviderAttribute(Type rawMessageOutputType)
        {
            _rawMessageOutputType = rawMessageOutputType;
        }

        public Type RawMessageOutputType
        {
            get
            {
                return _rawMessageOutputType;
            }
        }
    }
}
