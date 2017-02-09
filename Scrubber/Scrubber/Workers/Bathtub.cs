using System.Collections.Generic;
using System.Linq;

namespace Scrubber
{
    public class Bathtub
    {
        private readonly Soap _soap;
        private readonly Options _options;

        private List<DirtyFile> DirtyFiles { get; } = new List<DirtyFile>();
        public string FolderPath => _options.FolderPath;
        
        public Bathtub(Soap soap, Options options)
        {
            _soap = soap;
            _options = options;
        }

        public Result<List<DirtyFile>> Drain() => DirtyFiles.All(df => df.IsClean) 
            ? Result<List<DirtyFile>>.CreateSuccess(DirtyFiles) 
            : Result<List<DirtyFile>>.CreateFail(DirtyFiles);

        public void Fill() => FolderPath.FileByExtenstion("*.xaml").ForEach(file => DirtyFiles.Add(new DirtyFile(file)));

        public void Rinse() => DirtyFiles.ForEach(dirtyFile => _soap.Scrub(dirtyFile));
    }
}