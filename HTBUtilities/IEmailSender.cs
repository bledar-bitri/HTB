using System.Collections.Generic;

namespace HTBUtilities
{
    public interface IEmailSender
    {
        bool SendEmail(string from, IEnumerable<string> to, string replyTo, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments);
        bool SendEmail(string from, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string replyTo, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments);

    }
}
