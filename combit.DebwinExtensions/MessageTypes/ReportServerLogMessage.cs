using Debwin.Core;

namespace combit.DebwinExtensions.MessageTypes
{
    /// <summary>
    /// Subtype of LogMessage with Report Server-specific properties like the active user name.
    /// </summary>
    public class ReportServerLogMessage : LogMessage
    {

        public const int TYPECODE_RS_MESSAGE = 2000;
        private static readonly int[] CUSTOM_PROPERTIES = new int[] { PropertyIdentifiers.PROPERTY_USER_PRINCIPAL, PropertyIdentifiers.PROPERTY_MODULE_NAME };

        public string UserName;
        public string ModuleName;

        public override object GetProperty(int propertyIdentifier)
        {
            switch (propertyIdentifier)
            {
                case PropertyIdentifiers.PROPERTY_USER_PRINCIPAL:
                    return UserName;

                case PropertyIdentifiers.PROPERTY_MODULE_NAME:
                    return ModuleName;

                    // switch branches must match the list in SetProperty() and GetPropertyMetadata()!
            }

            return base.GetProperty(propertyIdentifier);
        }

        public override int[] GetCustomPropertyIDs()
        {
            return CUSTOM_PROPERTIES;
        }

        public override int GetMessageTypeCode()
        {
            return TYPECODE_RS_MESSAGE;
        }

        public override void SetProperty(int propertyIdentifier, object value)
        {
            switch (propertyIdentifier)
            {
                case PropertyIdentifiers.PROPERTY_USER_PRINCIPAL:
                    UserName = value as string;
                    return;

                case PropertyIdentifiers.PROPERTY_MODULE_NAME:
                    ModuleName = value as string;
                    return;

                    // switch branches must match the list in GetProperty() and GetPropertyMetadata()!
            }
            base.SetProperty(propertyIdentifier, value);
        }

        public override LogMessagePropertyInfo GetPropertyMetadata(int propertyID)
        {
            // Provide metadata for the custom properties of this message type
            switch (propertyID)
            {
                case PropertyIdentifiers.PROPERTY_USER_PRINCIPAL:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_USER_PRINCIPAL, "User", typeof(string));
                case PropertyIdentifiers.PROPERTY_MODULE_NAME:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_MODULE_NAME, "Module", typeof(string));
            }
            return base.GetPropertyMetadata(propertyID);
        }

    }
}
