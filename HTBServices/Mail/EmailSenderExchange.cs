using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Net;
using HTBUtilities;
using Microsoft.Exchange.WebServices.Data;

namespace HTBServices.Mail
{
    public class EmailSenderExchange : IEmailSender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        private readonly string defaultFromEmail;
        private readonly string defaultFromName;

        private readonly string user;
        private readonly string password;

        public EmailSenderExchange(string defaultFromEmail, string defaultFromName, string user, string password)
        {
            this.defaultFromEmail = defaultFromEmail;
            this.defaultFromName = defaultFromName;
            this.user = user;
            this.password = password;
        }

        private bool RedirectionCallback(string url)
        {
            return url.ToLower().StartsWith("https://");
        }

        public bool SendEmail(string from, IEnumerable<string> to, string replyTo, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments)
        {
            return SendEmail(from, to, null, null, replyTo, subject, body, ishtml, attachments);
        }
        public bool SendEmail(string from, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string replyTo, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments)
        {
            var service = new ExchangeService
            {
                // Set specific credentials.
                Credentials = new NetworkCredential(user, password)
            };

            // Look up the user's EWS endpoint by using Autodiscover.
            service.AutodiscoverUrl(user, RedirectionCallback);

            log.Info("Sending Generic Email");
            try
            {
                var mailMsg = new EmailMessage(service);
                foreach (string toAddr in to)
                {
                    mailMsg.ToRecipients.Add(toAddr);
                    log.Info("Generic Email TO: " + toAddr);
                }

                if (cc != null)
                {
                    foreach (string ccAddr in cc)
                    {
                        mailMsg.CcRecipients.Add(ccAddr);
                        log.Info("Generic Email CC [with attachments]: " + ccAddr);
                    }
                }

                if (bcc != null)
                {
                    foreach (string bccAddr in bcc)
                    {
                        mailMsg.BccRecipients.Add(bccAddr);
                        log.Info("Generic Email BCC [with attachments]: " + bccAddr);
                    }
                }
                // From
                mailMsg.From = defaultFromName + " <" + defaultFromEmail + ">";

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;
                if (from != null && HTBUtils.IsValidEmail(from))
                {
                    mailMsg.From = from;
                }
                if (replyTo != string.Empty && HTBUtils.IsValidEmail(replyTo))
                {
                    mailMsg.ReplyTo.Add(replyTo);
                }
                if (attachments != null)
                {
                    foreach (var attachment in attachments.Where(attachment => attachment != null))
                    {
                        mailMsg.Attachments.AddFileAttachment(
                            attachment.AttachmentStreamNamePrefix + attachment.AttachmentStreamName,
                            attachment.AttachmentStream);
                    }
                }
                mailMsg.Send();
                log.Info("Exchange EMail Sent!");
                //EmailSentSuccessfully(mailMsg.Subject, to, null, inkAktId, intAktId);
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
                //EmailSentError(subject, to, inkAktId, intAktId, ex);
                return false;
            }
        }
    }
}
