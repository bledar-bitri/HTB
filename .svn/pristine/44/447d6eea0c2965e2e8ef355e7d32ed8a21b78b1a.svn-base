using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapPoint
{
    public class MapPointUtils
    {
        public static string GetStreetName(string street)
        {
            street = RemoveCharsAfter(street, "/");
            street = RemoveCharsAfter(street, "\\");
            street = RemoveCharsAfter(street, " Top");

            street = RemoveAllSpecialChars(street);
            return street;
        }

        public static string RemoveCharsAfter(string mainText, string identifier)
        {
            int idx = mainText.IndexOf(identifier);
            if (idx > 0)
            {
                return mainText.Substring(0, idx);
            }
            else
            {
                return mainText;
            }
        }
        public static string RemoveAllSpecialChars(string str)
        {
            return str.
                Replace("!", "").
                Replace("@", "").
                Replace("$", "").
                Replace("%", "").
                Replace("^", "").
                Replace("&", "").
                Replace("*", "").
                Replace("(", "").
                Replace(")", "").
                Replace("_", "").
                Replace("-", "").
                Replace("+", "").
                Replace("=", "").
                Replace("\\", "").
                Replace("|", "").
                Replace("{", "").
                Replace("}", "").
                Replace("[", "").
                Replace("]", "").
                Replace("?", "").
                Replace("/", "").
                Replace(".", "").
                Replace(",", "").
                Replace(">", "").
                Replace("<", "").
                Replace("~", "").
                Replace("`", "");
        }
    }
}
