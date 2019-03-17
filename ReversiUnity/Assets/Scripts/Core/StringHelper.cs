using System;
using System.Linq;

namespace Core
{
    public static class StringHelper
    {
        public static string[] ToLinedString(this string str)
        {
            var linedStrings = str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return linedStrings.Select(linedString => linedString.Trim()).ToArray();
        }
    }
}
