using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Scrubber.Objects;

namespace Scrubber.Helpers
{
    public static class Extenstions
    {
        public static List<string> GetFilesByExtenstion(this string path, string extenstion,
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

        public static string ToLowerCaseString(this object s)
        {
            return s.ToString().ToLower();
        }

        public static void DisplayResult(this Result<Dictionary<bool, List<DirtyFile>>> result)
        {
            string messageText;
            var cleaned = result.ResultValue.Any(r => r.Key)
                ? result.ResultValue[true].Count
                : 0;

            if (!result.Success)
            {
                var dirty = result.ResultValue[false].Count;

                messageText = $"Operation Completed With Errors. {cleaned} Cleaned. {dirty} Failed.";
            }
            else
            {
                messageText = $"Operation Completed. {cleaned} Cleaned. 0 Failed.";
            }

            MessageBox.Show(messageText);
        }
    }
}