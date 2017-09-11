using System;

namespace ServiceManager.Util
{
    public static class StringExt
    {
        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
