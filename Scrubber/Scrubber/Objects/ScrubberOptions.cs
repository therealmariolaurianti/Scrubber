using System.Collections.Generic;

namespace Scrubber.Objects
{
    public class ScrubberOptions : IScrubberOptions
    {
        public ScrubberOptions(string folderPath, bool clearComments,
            ICollection<InputAttribute> additionalInputAttributes, ICollection<InputAttribute> removalInputAttributes)
        {
            FolderPath = folderPath;
            ClearComments = clearComments;
            InputAttributes = additionalInputAttributes;
            ExistingAttributes = removalInputAttributes;
        }
        public string FolderPath { get; set; }
        public bool ClearComments { get; set; }
        public ICollection<InputAttribute> InputAttributes { get; set; }
        public ICollection<InputAttribute> ExistingAttributes { get; set; }
    }
}