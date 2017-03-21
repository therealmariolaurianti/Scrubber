using System.Collections.Generic;
using Scrubber.Objects;

namespace Scrubber.Options
{
    public interface IScrubberOptions
    {
        bool ClearComments { get; }
        bool FormatFiles { get; set; }
        string FolderPath { get; }
        ICollection<InputAttribute> ExistingAttributes { get; }
        ICollection<InputAttribute> InputAttributes { get; }
    }
}