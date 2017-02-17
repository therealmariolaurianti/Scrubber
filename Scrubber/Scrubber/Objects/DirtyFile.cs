using System.Diagnostics;
using Scrubber.Extensions;

namespace Scrubber.Objects
{
    [DebuggerDisplay("{" + nameof(FileName) + "}")]
    public class DirtyFile
    {
        public DirtyFile(string file)
        {
            FilePath = file;
        }

        public string FilePath { get; set; }

        public string FileContent { get; set; }

        public string FileName => FilePath.GetFileName();

        public bool IsClean { get; set; }
    }
}