using System.Collections.ObjectModel;

namespace Scrubber.Objects
{
    public class BathtubOptions
    {
        public BathtubOptions(string folderPath, bool clearComments,
            ObservableCollection<InputAttribute> inputAttributes)
        {
            FolderPath = folderPath;
            ClearComments = clearComments;
            InputAttributes = inputAttributes;
        }

        public bool ClearComments { get; set; }

        public string FolderPath { get; set; }
        public ObservableCollection<InputAttribute> InputAttributes { get; set; }
    }
}