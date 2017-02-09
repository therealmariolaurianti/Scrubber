using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Scrubber
{
    public static class Extenstions
    {
        public static List<string> FileByExtenstion(this string path, string extenstion,
            SearchOption searchOption = SearchOption.AllDirectories) =>
            Directory.GetFiles(path, extenstion, searchOption).ToList();

        public static string GetFileName(this string file) =>
            Path.GetFileName(file);
    }
}