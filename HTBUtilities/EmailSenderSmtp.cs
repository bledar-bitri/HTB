using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;

namespace HTBUtilities
{
    public class EmailSenderSmtp : IEmailSender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string defaultFromEmail;
        private readonly string defaultFromName;

        private readonly string server;
        private readonly int port;
        private readonly string user;
        private readonly string password;

        public EmailSenderSmtp(string defaultFromEmail, string defaultFromName, string server, int port, string user, string password)
        {
            this.defaultFromEmail = defaultFromEmail;
            this.defaultFromName = defaultFromName;
            this.server = server;
            this.port = port;
            this.user = user;
            this.password = password;
        }

        public bool SendEmail(string from, IEnumerable<string> to, string replyTo, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments)
        {
            return SendEmail(from, to, null, null, replyTo, subject, body, ishtml, attachments);
        }
        public bool SendEmail(string from, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string replyTo, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments)
        {
            log.Info("Sending Generic Email");
            try
            {
                var mailMsg = new MailMessage();
                foreach (string toAddr in to)
                {
                    mailMsg.To.Add(toAddr);
                    log.Info("Generic Email TO: " + toAddr);
                }

                if (cc != null)
                {
                    foreach (string ccAddr in cc)
                    {
                        mailMsg.CC.Add(ccAddr);
                        log.Info("Generic Email CC [with attachments]: " + ccAddr);
                    }
                }

                if (bcc != null)
                {
                    foreach (string bccAddr in bcc)
                    {
                        mailMsg.Bcc.Add(bccAddr);
                        log.Info("Generic Email CC [with attachments]: " + bccAddr);
                    }
                }

                // From
                var mailAddress = new MailAddress(defaultFromName + " " + defaultFromEmail);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.IsBodyHtml = ishtml;
                mailMsg.Body = body;
                foreach (var attachment in attachments.Where(attachment => attachment != null))
                {
                    mailMsg.Attachments.Add(new Attachment(attachment.AttachmentStream, attachment.AttachmentStreamNamePrefix + attachment.AttachmentStreamName, attachment.AttachmentStreamMime));
                }
                if (from != null && HTBUtils.IsValidEmail(from))
                {
                    mailMsg.From = new MailAddress(from);
                }
                if (replyTo != string.Empty && HTBUtils.IsValidEmail(replyTo))
                {
                    mailMsg.ReplyToList.Add(new MailAddress(replyTo));
                }

                // Init SmtpClient and send
                var smtpClient = new SmtpClient(server);
                if (port > 0)
                {
                    smtpClient.Port = port;
                }
                var credentials = new System.Net.NetworkCredential(user, password);
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
                log.Info("SMTP EMail Sent!");
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    log.Error(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        log.Error(ex.InnerException.InnerException.Message);
                    }
                }
                return false;
            }
        }
    }
}
