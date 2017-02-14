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

        public string Name { get; set; }
        public List<ControlAttribute> ControlAttributes => ControlHelper.GetControlAttributesByName(Name);
    }

    public class ControlAttribute
    {
        public ControlAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}