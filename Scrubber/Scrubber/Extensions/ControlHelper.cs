using System.Collections.Generic;
using Scrubber.Objects;

namespace Scrubber.Extensions
{
    public static class ControlHelper
    {
        //reflection to get types and attributes
        public static readonly List<Control> Controls = new List<Control>
        {
            new Control("Grid"),
            new Control("DockPanel"),
            new Control("StackPanel"),
            new Control("Expander"),
            new Control("Button"),
            new Control("TextBox"),
            new Control("GroupBox")
        };

        public static List<ControlAttribute> GetControlAttributesByName(string name)
        {
            return new List<ControlAttribute>{ new ControlAttribute("IsEnabled")};
        }
    }
}