using CommandLine;
using Scrubber.Interfaces;

namespace Scrubber.Helpers
{
    public class Options : IOptions
    {
        [Option('f', "folderPath", HelpText = "FolderPath", Required = true)]
        public string FolderPath { get; set; }
    }
}