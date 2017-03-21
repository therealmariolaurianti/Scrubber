using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Scrubber.Extensions
{
    public static class FileExtenstions
    {
        public static bool DirectoryExists(this string path)
        {
            return Directory.Exists(path);
        }
        public static List<string> GetFilesByExtenstion(this string path, string extenstion,
            SearchOption searchOption = SearchOption.AllDirectories)
        {
            return Directory.GetFiles(path, $"*.{extenstion}", searchOption).ToList();
        }

        public static string GetFileName(this string file)
        {
            return Path.GetFileName(file);
        }
    }
}