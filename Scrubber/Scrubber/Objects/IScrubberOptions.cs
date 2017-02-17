using System.Collections.Generic;

namespace Scrubber.Objects
{
    public interface IScrubberOptions
    {
        bool ClearComments { get; }
        ICollection<InputAttribute> ExistingAttributes { get; }
        string FolderPath { get; }
        ICollection<InputAttribute> InputAttributes { get; }
    }
}