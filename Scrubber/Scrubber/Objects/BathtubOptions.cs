using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Scrubber.Objects
{
    public class BathtubOptions
    {
        public BathtubOptions(string folderPath, bool clearComments,
            ICollection<InputAttribute> inputAttributes)
        {
            FolderPath = folderPath;
            ClearComments = clearComments;
            InputAttributes = inputAttributes;
        }

        public bool ClearComments { get; set; }

        public string FolderPath { get; set; }
        public ICollection<InputAttribute> InputAttributes { get; set; }
    }
}