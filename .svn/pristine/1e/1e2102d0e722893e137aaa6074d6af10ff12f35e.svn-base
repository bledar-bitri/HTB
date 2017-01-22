using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTB.v2.intranetx.glocalcode
{

    public class IncControls
    {
        private const string KT_relPath = "../../../";
        private static bool smartdate_jsscript_includes = false;
        private Browser browser;

        public IncControls(HttpRequest request)
        {
            smartdate_jsscript_includes = false;
            browser = new Browser(request);
        }

        public string SmartDate(string widget_name, string mask)
        {

            bool KT_smartdateSW = false;

            //browser dependent
            KT_smartdateSW = browser.Name.Equals("msie") || browser.Name.Equals("gecko") || browser.Name.Equals("safari");

            mask = mask.Replace("\\", "\\\\").Replace("'", "\'");
            string ret = "";
            if (KT_smartdateSW)
            {
                ret += "\" onblur=\"editDateBlur(this, '" + mask + "')\" ";
                //fix date format on focus, problem a first keypress and resulting date is not valid format -> value is deleted
                //ret = ret & " onfocus=""editDateBlur(this, '" & mask & "')"" "
                ret += " onkeypress=\"return editDatePre(this, '" + mask + "', event)\" ";
                ret += " onkeyup=\"return editDate(this, '" + mask + "', event);\" ";
                ret += " autocomplete=\"off\" ";
                //output include jscript only once and only if browser suports it
                if (!smartdate_jsscript_includes)
                {
                    smartdate_jsscript_includes = true;
                    ret += "/><script language=\"JavaScript}\" src=\"" + KT_relPath + "smartdate.js\"></script><input type=\"hidden\" b=\"";
                }
                else
                {
                    ret += "\"";
                }
            }
            return ret;
        }
    }
}