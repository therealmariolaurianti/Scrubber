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
        private readonly BathtubOptions _bathtubOptions;

        public Bathtub(Soap soap, BathtubOptions bathtubOptions)
        {
            _bathtubOptions = bathtubOptions;
            _soap = soap;
        }

        private List<DirtyFile> DirtyFiles { get; } = new List<DirtyFile>();

        public string FolderPath => _bathtubOptions.FolderPath;
        public bool ClearComments => _bathtubOptions.ClearComments;
        public ICollection<InputAttribute> InputAttributes => _bathtubOptions.InputAttributes;

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

        public void Rinse()
        {
            _soap.ClearComments = ClearComments;
            _soap.InputAttributes = InputAttributes;

            DirtyFiles.ForEach(dirtyFile => _soap.Scrub(dirtyFile));
        }
    }
}