using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTB.v2.intranetx
{
    public class KTpublic
    {
        public static string KT_escapeJS(string val)
        {
            return val;
        }

        public static string KT_addcslashes(string str, string charlist)
        {
            int i;
            string outstr;
            outstr = str;
            for (i = 1; i < charlist.Length; i++)
            {
                string ch = charlist.Substring(i, 1);
                outstr = outstr.Replace(ch, "\\" + ch);
            }
            return outstr;
        }
    }
}
	