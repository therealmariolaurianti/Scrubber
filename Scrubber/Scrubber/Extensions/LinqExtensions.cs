using System;
using System.Collections.Generic;
using System.Linq;

namespace Scrubber.Extensions
{
    public static class LinqExtensions
    {
        public static bool ContainsAny(this string s, List<string> collection)
        {
            return collection.Any(s.Contains);
        }

        public static string ToLowerCaseString(this object s)
        {
            return s.ToString().ToLower();
        }

        public static string FormatAttribute(this string s)
        {
            var formatAttribute = s.Replace("Property", string.Empty).Trim();
            return formatAttribute;
        }

        public static string SplitOnCapitals(this string str)
        {
            var newString = string.Join("", str.Select((c, i) => i != 0 && char.IsUpper(c) ? " " + c : c.ToString()));
            return newString;
        }

        public static List<T2> ToListOfType<T,T2>(this IEnumerable<T> list)
        {
            return list.OfType<T2>().ToList();
        }

        public static List<List<float[]>> SplitList(List<float[]> locations, int nSize)
        {
            var list = new List<List<float[]>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }
    }
}