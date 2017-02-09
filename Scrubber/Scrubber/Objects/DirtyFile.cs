using System.Diagnostics;

namespace Scrubber
{
    [DebuggerDisplay("{" + nameof(FileName) + "}")]
    public class DirtyFile
    {
        public DirtyFile(string file)
        {
            File = file;
        }

        public string File { get; set; }

        public string FileName => File.GetFileName();

        public bool IsClean { get; set; }
    }
}