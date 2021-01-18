using System;

namespace Debwin.Core.Metadata
{



    public interface IMessageSourceMetadataProvider
    {

        /// <summary>
        /// Describes the types of the raw message objects that a message parser can process.
        /// This information allows the UI to automatically build and show a list of suitable parsers for a message source.
        /// </summary>
        Type MessageOutputType { get; }


    }


    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class MessageSourceMetadataProviderAttribute : Attribute
    {
        private readonly Type _metadataProviderType;
        
        public MessageSourceMetadataProviderAttribute(Type metadataProviderType)
        {
            // we must not throw exceptions in the constructor of attributes => accept invalid possibly types for now
            _metadataProviderType = metadataProviderType;
        }

        public Type MetadataProviderType
        {
            get
            {
                if (_metadataProviderType != null && !typeof(IMessageSourceMetadataProvider).IsAssignableFrom(_metadataProviderType))
                {
                    throw new InvalidOperationException("The specified metadata provider of the message source does not implement " + nameof(IMessageSourceMetadataProvider));
                }
                return _metadataProviderType;
            }
        }
    }
}
