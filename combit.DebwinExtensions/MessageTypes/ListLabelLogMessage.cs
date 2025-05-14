using Debwin.Core;

namespace combit.DebwinExtensions.MessageTypes
{
    /// <summary>
    /// Subtype of LogMessage with ListLabel-specific properties like module name and CPU nr.
    /// </summary>
    public class ListLabelLogMessage : LogMessage
    {
        public const int TYPECODE_LL_MESSAGE = 1000;
        public string ModuleName;
        public int ProcessorNr;
        private static readonly int[] CUSTOM_PROPERTIES = new int[] { PropertyIdentifiers.PROPERTY_MODULE_NAME, PropertyIdentifiers.PROPERTY_CPU_NR };

        public static int[] CUSTOM_PROPERTIES1
        {
            get
            {
                return CUSTOM_PROPERTIES;
            }
        }

        public ListLabelLogMessage()
        {
            // Default constructor
        }

        public ListLabelLogMessage(ListLabelLogMessage other) : base(other)
        {
            this.ModuleName = other.ModuleName;
            this.ProcessorNr = other.ProcessorNr;
        }

        public override LogMessage Clone()
        {
            return new ListLabelLogMessage(this);
        }

        public override int[] GetCustomPropertyIDs()
        {
            return CUSTOM_PROPERTIES1;
        }

        public override int GetMessageTypeCode()
        {
            return TYPECODE_LL_MESSAGE;
        }

        public override object GetProperty(int propertyIdentifier)
        {
            switch (propertyIdentifier)
            {
                case PropertyIdentifiers.PROPERTY_MODULE_NAME:
                    return ModuleName;

                case PropertyIdentifiers.PROPERTY_CPU_NR:
                    return ProcessorNr;

                    // switch branches must match the list in SetProperty() and GetPropertyMetadata()!
            }

            return base.GetProperty(propertyIdentifier);
        }

        public override LogMessagePropertyInfo GetPropertyMetadata(int propertyID)
        {
            // Provide metadata for the custom properties of this message type
            switch (propertyID)
            {
                case PropertyIdentifiers.PROPERTY_MODULE_NAME:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_MODULE_NAME, "Module", typeof(string));

                case PropertyIdentifiers.PROPERTY_CPU_NR:
                    return new LogMessagePropertyInfo(PropertyIdentifiers.PROPERTY_CPU_NR, "CPU", typeof(int));
            }
            return base.GetPropertyMetadata(propertyID);
        }

        public override void SetProperty(int propertyIdentifier, object value)
        {
            switch (propertyIdentifier)
            {
                case PropertyIdentifiers.PROPERTY_MODULE_NAME:
                    ModuleName = value as string;
                    return;

                case PropertyIdentifiers.PROPERTY_CPU_NR:
                    ProcessorNr = (int)value;
                    return;

                    // switch branches must match the list in GetProperty() and GetPropertyMetadata()!
            }
            base.SetProperty(propertyIdentifier, value);
        }
    }
}