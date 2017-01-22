using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapPoint
{
    public class MapPointConstants
    {
        public const int PORT = 8888;
        public const string REQUEST_END = "$";
        public const char DELIM = ';';
        public const string COMMAND_END = "~";
        public const string HOST = "127.0.0.1";
        public const int BUFFER_SIZE = 10025;
        public const string COMMAND_GetCalculatedRoute = "GetCalculatedRoute";

    }
}
