using System.Collections.Generic;
using Scrubber.Objects;

namespace Scrubber.Options
{
    public interface IScrubberOptions
    {
        bool ClearComments { get; }
        ICollection<InputAttribute> ExistingAttributes { get; }
        string FolderPath { get; }
        ICollection<InputAttribute> InputAttributes { get; }
    }
}