using System.Collections.Generic;
using Scrubber.Enums;
using Scrubber.Objects;

namespace Scrubber.Options
{
    public interface IScrubberOptions
    {
        bool ClearComments { get; }
        bool FormatFiles { get; }
        string Path { get; }
        ICollection<InputAttribute> ExistingAttributes { get; }
        ICollection<InputAttribute> InputAttributes { get; }
        FolderOrFile FolderOrFile { get; }
    }
}