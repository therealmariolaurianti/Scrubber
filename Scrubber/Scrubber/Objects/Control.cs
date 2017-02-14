using System.Collections.Generic;

namespace Scrubber.Objects
{
    public class Control
    {
        public Control(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public List<ControlAttribute> ControlAttributes { get; set; }
    }

    public class ControlAttribute
    {
        public string Name { get; set; }
    }
}