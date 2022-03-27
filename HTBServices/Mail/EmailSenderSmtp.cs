using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using HTBUtilities;

namespace HTBServices.Mail
{
    public class EmailSenderSmtp : IEmailSender
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

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
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => sslPolicyErrors == SslPolicyErrors.None;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public bool SendEmail(string from, IEnumerable<string> to, string replyTo, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments)
        {
            return SendEmail(from, to, null, null, replyTo, subject, body, ishtml, attachments);
        }
        public bool SendEmail(string from, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string replyTo, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments)
        {
            Log.Info("Sending Generic Email");
            try
            {
                var mailMsg = new MailMessage();
                foreach (string toAddr in to)
                {
                    mailMsg.To.Add(toAddr);
                    Log.Info("Generic Email TO: " + toAddr);
                }

                if (cc != null)
                {
                    foreach (string ccAddr in cc)
                    {
                        mailMsg.CC.Add(ccAddr);
                        Log.Info("Generic Email CC [with attachments]: " + ccAddr);
                    }
                }

                if (bcc != null)
                {
                    foreach (string bccAddr in bcc)
                    {
                        mailMsg.Bcc.Add(bccAddr);
                        Log.Info("Generic Email CC [with attachments]: " + bccAddr);
                    }
                }

                // From
                var mailAddress = new MailAddress(defaultFromName + " " + defaultFromEmail);
                mailMsg.From = mailAddress;
                
                Log.Info("Generic Email From: " + mailAddress);

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.IsBodyHtml = ishtml;
                mailMsg.Body = body;
                Log.Info("Generic Adding Attachments: ");
                var attachmentsToSend = attachments?.Where(attachment => attachment != null);


                if (attachmentsToSend != null)
                {
                    var htbEmailAttachments = attachmentsToSend as HTBEmailAttachment[] ?? attachmentsToSend.ToArray();
                    foreach (var attachment in htbEmailAttachments)
                    {
                        Log.Debug($"Adding Attachment: {attachment.AttachmentStreamName} ");
                        mailMsg.Attachments.Add(new Attachment(attachment.AttachmentStream,
                            attachment.AttachmentStreamNamePrefix + attachment.AttachmentStreamName,
                            attachment.AttachmentStreamMime));
                    }
                }

                if (from != null && HTBUtils.IsValidEmail(from))
                {
                    mailMsg.From = new MailAddress(from);
                }
                if (replyTo != string.Empty && HTBUtils.IsValidEmail(replyTo))
                {
                    mailMsg.ReplyToList.Add(new MailAddress(replyTo));
                }
                
                Log.Debug($"Creating SMTP Client [server: {server}]");

                // Init SmtpClient and send
                var smtpClient = new SmtpClient(server);
                if (port > 0)
                {
                    Log.Debug($"SMTP Setting Port {port}");
                    smtpClient.Port = port;
                }
                var credentials = new NetworkCredential(user, password);
                Log.Debug($"SMTP Credentials [{user}][{password}]");

                smtpClient.EnableSsl = true;
                smtpClient.Credentials = credentials;

                Log.Info("Sending Mail");
                smtpClient.Send(mailMsg);

                Log.Info("SMTP EMail Sent!");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Log.Error(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        Log.Error(ex.InnerException.InnerException.Message);
                    }
                }
                return false;
            }
        }
    }
}
