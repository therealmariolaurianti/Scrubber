using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Scrubber.Helpers
{
    public static class Extenstions
    {
        public static List<string> FileByExtenstion(this string path, string extenstion,
            SearchOption searchOption = SearchOption.AllDirectories)
        {
            return Directory.GetFiles(path, extenstion, searchOption).ToList();
        }

        public static string GetFileName(this string file)
        {
            return Path.GetFileName(file);
        }

        public static bool ContainsAny(this string s, List<string> collection)
        {
            return collection.Any(s.Contains);
        }
    }
}