using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTB.v2.intranetx.glocalcode
{
    public class Browser
    {

        public string Name, Platform;
        public int Version = -1;

        private HttpRequest request;

        public Browser(HttpRequest request)
        {
            this.request = request;
            Init();
        }

        public void Init()
        {
            string HTTP_USER_AGENT = request.ServerVariables["HTTP_USER_AGENT"];
            Name = "unknown";
            Version = -1;
            Platform = "unknown";

            string ua = HTTP_USER_AGENT.ToLower();

            if (ua.IndexOf("opera") >= 0)
            {
                Name = "Opera";
                Version = Convert.ToInt32(ua.Substring(ua.IndexOf("opera") + 6, 1));
            }
            else if (ua.IndexOf("msie") >= 0)
            {
                Name = "msie";
                Version = Convert.ToInt32(ua.Substring(ua.IndexOf("msie") + 5, 1));
            }
            else if (ua.IndexOf("safari") >= 0)
            {
                Name = "safari";
            }
            else if (ua.IndexOf("gecko") >= 0)
            {
                Name = "gecko";
            }
            else if (ua.IndexOf("konqueror") >= 0)
            {
                Name = "konqueror";
            }

            if (ua.IndexOf("windows") >= 0)
            {
                Platform = "windows";
            }
            else if (ua.IndexOf("linux") >= 0)
            {
                Platform = "linux";
            }
            else if (ua.IndexOf("mac") >= 0)
            {
                Platform = "mac";
            }
            else if (ua.IndexOf("unix") >= 0)
            {
                Platform = "unix";
            }
        }
    }
}