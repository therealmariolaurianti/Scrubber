using System.Collections.Generic;
using Scrubber.Enums;
using Scrubber.Objects;

namespace Scrubber.Options
{
    public class ScrubberOptions : IScrubberOptions
    {
        public ScrubberOptions(string folderPath, bool clearComments, bool formatFiles, 
            ICollection<InputAttribute> additionalInputAttributes, ICollection<InputAttribute> removalInputAttributes, 
            FolderOrFile folderOrFile)
        {
            Path = folderPath;
            ClearComments = clearComments;
            FormatFiles = formatFiles;
            InputAttributes = additionalInputAttributes;
            ExistingAttributes = removalInputAttributes;
            FolderOrFile = folderOrFile;
        }

        public string Path { get; }
        public bool ClearComments { get; }
        public bool FormatFiles { get; }
        public ICollection<InputAttribute> InputAttributes { get; }
        public FolderOrFile FolderOrFile { get; }
        public ICollection<InputAttribute> ExistingAttributes { get; }
    }
}