using HTBServices;
using HTBServices.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HTBEmailTest
{
    class Program
    {
        static void Mainx(string[] args)
        {

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            TestEmail(args);
            TestEmailWithAttachments(args);
            Console.WriteLine("Done!\n\tPress any key to continue!");
            Console.ReadLine();
        }

        static void TestEmail(string[] args)
        {
            
            //string to = "office@interna.at";
            string to = "bledi1@yahoo.com";
            string subject = "Test";
            string message = "OK :)";

            if (args.Length > 0)
                to = args[0];
            Console.WriteLine($"Testing email [to: {to}]");

            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(to, subject, message);
        }

        static void TestEmailWithAttachments(string[] args)
        {
            //string to = "office@interna.at";
            string to = "bledi1@yahoo.com";
            string subject = "Test";
            string message = "OK :)";

            if (args.Length > 0)
                to = args[0];
            var attachments = new List<HTBEmailAttachment>{ new HTBEmailAttachment(new MemoryStream(Encoding.Default.GetBytes("Test File ...")), "Test File.txt", "")};
            Console.WriteLine($"Testing email with attachment [to: {to}]");
            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail("office@ecp.or.at", new List<string>{to}, subject, message, true, attachments, 0,0);
        }


    }
}
