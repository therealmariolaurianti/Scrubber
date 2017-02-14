using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup.Primitives;
using Scrubber.Helpers;
using Scrubber.Objects;

namespace Scrubber.Extensions
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
    }
}