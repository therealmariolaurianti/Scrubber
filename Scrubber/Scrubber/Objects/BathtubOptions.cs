using System.Collections.Generic;

namespace Scrubber.Objects
{
    public class BathtubOptions
    {
        public BathtubOptions(string folderPath, bool clearComments, ICollection<InputAttribute> additionalInputAttributes,
            ICollection<InputAttribute> removalInputAttributes)
        {
            FolderPath = folderPath;
            ClearComments = clearComments;
            AdditionalInputAttributes = additionalInputAttributes;
            RemovalInputAttributes = removalInputAttributes;
        }

        public bool ClearComments { get; set; }
        public string FolderPath { get; set; }
        public ICollection<InputAttribute> AdditionalInputAttributes { get; set; }
        public ICollection<InputAttribute> RemovalInputAttributes { get; }
    }
}