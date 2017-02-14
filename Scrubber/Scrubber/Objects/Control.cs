using System.Collections.Generic;
using Scrubber.Extensions;

namespace Scrubber.Objects
{
    public class Control
    {
        public Control(string name)
        {
            Name = name;
        }

        public List<ControlAttribute> ControlAttributes => ControlHelper.GetControlAttributesByName(Name);

        public string Name { get; set; }
    }

    public class ControlAttribute
    {
        public ControlAttribute(string name)
        {
            Name = name;
        }

        public List<AttributeValue> AttributeValues
            => new List<AttributeValue> {new AttributeValue("True"), new AttributeValue("False")};

        public string Name { get; set; }
    }

    public class AttributeValue
    {
        public AttributeValue(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}