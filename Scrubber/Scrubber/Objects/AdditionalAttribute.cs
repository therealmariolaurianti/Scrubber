using System.Diagnostics;
using System.Xml;
using Scrubber.Enums;
using Scrubber.Helpers;
using Scrubber.Workers;

namespace Scrubber.Objects
{
    [DebuggerDisplay("{Name}, {Value}")]
    public class AdditionalAttribute
    {
        public AdditionalAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public AdditionalAttribute(XmlAttribute nodeAttribute)
        {
            Name = nodeAttribute.Name;
            Value = nodeAttribute.Value;
        }

        public AdditionalAttribute(CommonAttributes attributeName, object value)
        {
            Name = attributeName.ToString();
            Value = value;
        }

        public bool IsDesignTimeAttribute => Value.ToString().Contains("d:");

        public string Name { get; set; }
        public string NamespaceXmnlsCharacter => IsDesignTimeAttribute ? Name[0].ToLowerCaseString() : string.Empty;
        public object Value { get; set; }
    }
}