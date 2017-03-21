using System.Collections.Generic;
using System.Linq;
using Scrubber.Extensions;
using Scrubber.Factories;
using Scrubber.Helpers;
using Scrubber.Objects;
using Scrubber.Options;

namespace Scrubber.Workers
{
    public class Bathtub
    {
        private readonly IScrubberOptions _bathtubOptions;
        private readonly ISoapFactory _soapFactory;

        public Bathtub(IScrubberOptions bathtubOptions, ISoapFactory soapFactory)
        {
            _bathtubOptions = bathtubOptions;
            _soapFactory = soapFactory;
        }

        private List<DirtyFile> DirtyFiles { get; } = new List<DirtyFile>();

        public Result<Dictionary<bool, List<DirtyFile>>> Drain()
        {
            var result = DirtyFiles.GroupBy(item => item.IsClean)
                .ToDictionary(x => x.Key, x => x.ToList());

            return DirtyFiles.Any(df => !df.IsClean)
                ? Result<Dictionary<bool, List<DirtyFile>>>.CreateFail(result)
                : Result<Dictionary<bool, List<DirtyFile>>>.CreateSuccess(result);
        }

        public void FillAndRinse()
        {
            _bathtubOptions.FolderPath.GetFilesByExtenstion("xaml").ForEach(file =>
            {
                var dirtyFile = new DirtyFile(file);
                DirtyFiles.Add(dirtyFile);
            });

            Rinse();
        }

        public void Rinse()
        {
            var soap = _soapFactory.Create(_bathtubOptions);
            DirtyFiles.ForEach(dirtyFile => soap.Scrub(dirtyFile));
        }
    }
}