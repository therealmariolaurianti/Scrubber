using System.Collections.Generic;
using System.Linq;
using Scrubber.Enums;

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
        
    }
}