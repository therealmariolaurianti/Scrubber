using System.Diagnostics;
using Ninject;
using Scrubber.Extensions;

namespace Scrubber.Objects
{
    [DebuggerDisplay("{ControlName}, {AttributeName}, {AttributeValue}")]
    public class InputAttribute
    {
        [Inject]
        public InputAttribute()
        {
            
        }

        public InputAttribute(string controlName, string attributeName, object attributeValue)
        {
            ControlName = controlName;
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }

        public InputAttribute(string controlName, string attributeName)
        {
            ControlName = controlName;
            AttributeName = attributeName;
            AttributeValue = null;
        }

        public bool IsDesignTimeAttribute => AttributeValue.ToString().Contains("d:");
        public string NamespaceXmnlsCharacter => IsDesignTimeAttribute ? AttributeName?[0].ToLowerCaseString() : string.Empty;
        public string ControlName { get; set; }
        public string AttributeName { get; set; }
        public object AttributeValue { get; set; }
    }
}