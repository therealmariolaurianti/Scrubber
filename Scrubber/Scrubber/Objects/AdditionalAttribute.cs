using System.Diagnostics;
using System.Xml;
using Scrubber.Helpers;

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

        public string Name { get; set; }
        public object Value { get; set; }
        public bool IsDesignTimeAttribute => Value.ToString().Contains("d:");
        public string NamespaceXmnlsCharacter => IsDesignTimeAttribute ? Name[0].ToLowerCaseString() : string.Empty;
    }
}