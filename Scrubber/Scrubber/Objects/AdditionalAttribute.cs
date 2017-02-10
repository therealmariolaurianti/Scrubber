using System.Diagnostics;

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

        public string Name { get; set; }
        public object Value { get; set; }
    }
}