using System.Collections.Generic;
using Scrubber.Helpers;

namespace Scrubber.Objects
{
    public class Control
    {
        public Control(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public List<ControlAttribute> ControlAttributes => ControlHelper.GetFieldsByControlType(Name);
    }
}