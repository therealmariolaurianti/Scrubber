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
    }
}