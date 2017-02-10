using CommandLine;
using Scrubber.Interfaces;

namespace Scrubber.Helpers
{
    public class Options : IOptions
    {
        [Option('f', "folderPath", HelpText = "FolderPath", Required = true)]
        public string FolderPath { get; set; }

        [Option('c', "clearComments", HelpText = "ClearComments", Required = true)]
        public bool ClearComments { get; set; }
    }
}