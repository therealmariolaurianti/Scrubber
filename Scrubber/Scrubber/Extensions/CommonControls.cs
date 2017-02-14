using System.Collections.Generic;
using Scrubber.Objects;

namespace Scrubber.Extensions
{
    public static class CommonControls
    {
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
    }
}