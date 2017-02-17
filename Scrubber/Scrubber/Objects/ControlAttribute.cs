using System.Collections.Generic;
using Scrubber.Helpers;

namespace Scrubber.Objects
{
    public class ControlAttribute
    {
        public ControlAttribute(string name, string controlName)
        {
            Name = name;
            ControlName = controlName;
        }
        
        public List<AttributeValue> AttributeValues => ControlHelper.GetValuesByAttribute(Name, ControlName);

        public string ControlName { get; }
        public string Name { get; set; }
    }
}