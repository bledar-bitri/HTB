using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTBEmailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //string to = "office@interna.at";
            string to = "bledi1@yahoo.com";
            string subject = "Test";
            string message = "OK :)";

            if (args.Length > 0)
                to = args[0];

            new HTBUtilities.HTBEmail().SendGenericEmail(to, subject, message);
        }
    }
}
