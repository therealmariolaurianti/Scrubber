using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Scrubber.Extensions;
using Scrubber.Helpers;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class Bathtub
    {
        private readonly Soap _soap;

        public Bathtub(Soap soap, string folderPath, bool clearComments)
        {
            FolderPath = folderPath;
            ClearComments = clearComments;
            _soap = soap;
        }

        private List<DirtyFile> DirtyFiles { get; } = new List<DirtyFile>();
        public string FolderPath { get; set; }
        public bool ClearComments { get; set; }

        public Result<Dictionary<bool, List<DirtyFile>>> Drain()
        {
            var result = DirtyFiles.GroupBy(item => item.IsClean)
                .ToDictionary(x => x.Key, x => x.ToList());

            return DirtyFiles.Any(df => !df.IsClean)
                ? Result<Dictionary<bool, List<DirtyFile>>>.CreateFail(result)
                : Result<Dictionary<bool, List<DirtyFile>>>.CreateSuccess(result);
        }

        public void Fill()
        {
            FolderPath.GetFilesByExtenstion("*.xaml").ForEach(file =>
            {
                var dirtyFile = new DirtyFile(file);
                DirtyFiles.Add(dirtyFile);
            });
        }

        public ObservableCollection<InputAttribute> InputAttributes { get; set; }

        public void Rinse()
        {
            _soap.ClearComments = ClearComments;
            _soap.InputAttributes = InputAttributes;

            DirtyFiles.ForEach(dirtyFile => _soap.Scrub(dirtyFile));
        }
    }
}