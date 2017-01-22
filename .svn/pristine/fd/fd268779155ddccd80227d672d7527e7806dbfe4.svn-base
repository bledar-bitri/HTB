using System;
using System.Net;
using System.Net.Mail;
using HTB.Database;
using HTBUtilities;

namespace HTB.v2.intranetx.util
{
    public class EMailSender
    {
        private tblServerSettings settings;

        public EMailSender()
        {
            settings = (tblServerSettings)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblServerSettings", typeof(tblServerSettings));
        }

        public void sendEmail(string to, string subject, string body)
        {
            try
            {
                SmtpClient sMail = new SmtpClient(settings.ServerSMTP);//exchange or smtp server goes here.
                sMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                sMail.Credentials = new NetworkCredential(settings.ServerSMTPUser,settings.ServerSMTPPW);
                sMail.Send(settings.ServerSystemMail, to, subject, body);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}