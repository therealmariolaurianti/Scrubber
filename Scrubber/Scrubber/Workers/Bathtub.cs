using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Scrubber.Helpers;
using Scrubber.Interfaces;
using Scrubber.Objects;

namespace Scrubber.Workers
{
    public class Bathtub
    {
        private readonly Soap _soap;
        private readonly IOptions _options;

        private List<DirtyFile> DirtyFiles { get; } = new List<DirtyFile>();
        public string FolderPath => _options.FolderPath;
        
        public Bathtub(Soap soap, IOptions options)
        {
            _soap = soap;
            _options = options;
        }

        public Result<List<DirtyFile>> Drain()
        {
            if(DirtyFiles.Any(df => !df.IsClean))
                Result<List<DirtyFile>>.CreateFail(DirtyFiles.Where(df => !df.IsClean).ToList());
            return Result<List<DirtyFile>>.CreateSuccess(DirtyFiles);
        }

        public void Fill() => FolderPath.FileByExtenstion("*.xaml").ForEach(file => DirtyFiles.Add(new DirtyFile(file)));

        public void Rinse() => DirtyFiles.ForEach(dirtyFile => _soap.Scrub(dirtyFile));
    }
}