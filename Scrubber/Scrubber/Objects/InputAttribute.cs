using System.Diagnostics;
using System.Xml;
using Scrubber.Enums;
using Scrubber.Extensions;

namespace Scrubber.Objects
{
    [DebuggerDisplay("{Name}, {Value}")]
    public class InputAttribute
    {
        public InputAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public InputAttribute(XmlAttribute nodeAttribute)
        {
            Name = nodeAttribute.Name;
            Value = nodeAttribute.Value;
        }

        public InputAttribute(CommonAttributes attributeName, object value)
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