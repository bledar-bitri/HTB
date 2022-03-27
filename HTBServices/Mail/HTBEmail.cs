using HTB.Database;
using HTB.Database.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HTBUtilities;
using Tamir.SharpSsh.java.io;

namespace HTBServices.Mail
{
    public class HTBEmail : IHTBEmail
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        private static readonly tblServerSettings serverSettings = (tblServerSettings)HTBUtils.GetSqlSingleRecord("Select * from tblServerSettings", typeof(tblServerSettings));
        private static readonly tblControl control = HTBUtils.GetControlRecord();

        private readonly int _intAktEmailSentActionTypeId = HTBUtils.GetIntConfigValue("AktInt_EMail_Sent_Action_Type");
        private readonly int _intAktEmailNotSentActionTypeId = HTBUtils.GetIntConfigValue("AktInt_EMail_NOT_Sent_Action_Type");
        private readonly string _fromOfficeName = HTBUtils.GetConfigValue("From_Office_Name");

        private readonly IEmailSender _emailSender;

        private string _replyTo = string.Empty;
        public string ReplyTo
        {
            get { return _replyTo; }
            set { _replyTo = value; }
        }
        
        public HTBEmail(IEmailSender emailSender)
        {
            _emailSender = emailSender;
            Log.Debug("Created New HTBEmail");
        }

        #region Lawyer
        public bool SendLawyerPackage(IEnumerable<string> to, qryCustInkAkt akt, IEnumerable<HTBEmailAttachment> attachments, string body = "")
        {
            try
            {
                if (!to.Any()) return false;

                var subject = control.LawyerEmailSubject.Replace("<akt>", akt.CustInkAktID.ToString()).Replace("<gegner>", akt.GegnerName1 + " " + akt.GegnerName2);
                if (attachments != null)
                {
                    foreach (var attachment in attachments.Where(attachment => attachment != null))
                    {
                        attachment.AttachmentStreamNamePrefix = akt.GegnerName1 + " " + akt.GegnerName2 + "_";
                    }
                }
                return  _emailSender.SendEmail(_fromOfficeName + " " + serverSettings.ServerSystemMail, to, ReplyTo, subject, body, true, attachments);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }
        public bool SendLawyerReminder(qryCustInkAkt akt)
        {
            try
            {
                var lawyer = (tblLawyer)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblLawyer WHERE LawyerId = " + akt.CustInkAktLawyerId, typeof(tblLawyer));

                var to = HTBUtils.GetValidEmailAddressesFromStrings(new[]
                {
                    lawyer.LawyerEmail,HTBUtils.GetConfigValue("Office_Email")
                    
                });
                if (!to.Any()) return false;
                var subject = control.LawyerEmailSubject.Replace("<akt>", akt.CustInkAktID.ToString() + " ERINERUNG").Replace("<gegner>", akt.GegnerName1 + " " + akt.GegnerName2);
                
                return _emailSender.SendEmail(_fromOfficeName + " " + serverSettings.ServerSystemMail, to, ReplyTo, subject, GetLawyerReminderEMailBody(akt, lawyer), true, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return false;
            }
        }

        private string GetLawyerReminderEMailBody(qryCustInkAkt akt, tblLawyer lawyer)
        {

            string text = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Lawyer_Reminder_EMail_Text"));
            if (lawyer != null)
            {
                var sb = new StringBuilder();
                if (lawyer.LawyerSex == 1)
                    sb.Append("r " + lawyer.LawyerAnrede + " " + lawyer.LawyerName1);
                else
                    sb.Append(" " + lawyer.LawyerAnrede + " " + lawyer.LawyerName1);
                text = text.Replace("[Recipient]", sb.ToString());
            }
            text = text.Replace("[Gegner]", akt.GegnerName1 + " " + akt.GegnerName2);
            text = text.Replace("[Akt]", akt.CustInkAktID.ToString());

            return text;
        }

        #endregion

        #region Melde Research
        public bool SendMeldeResearchNotice(tblAktMelde melde)
        {
            Log.Info("Sending Research Notice");
            try
            {
                var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + control.MeldeResearchSB, typeof(tblUser));
                var to = new string[]
                {
                    user.UserEMailOffice

                };
                if (!to.Any()) return false;
                var subject = control.MeldeEmailSubject.Replace("<akt>", melde.AMNr);
                var body = "More Research Needed for this Melde Akt: " + melde.AMNr;
                return _emailSender.SendEmail(_fromOfficeName + " " + serverSettings.ServerSystemMail, to, ReplyTo, subject, body, true, null);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return false;
            }
        } 

        #endregion

        #region Mahnung
        public bool SendMahnung(string body, string attachment, string subject = null, string attachmentName = null)
        {
            Log.Info("Sending Mahnung");
            try
            {
                Log.Info("Mahnung Email TO: " + control.MahnungEmail);

                var to = new List<string>{ control.MahnungEmail };
                subject = subject ?? control.MahnungEmailSubject;
                var attachments = new List<HTBEmailAttachment>();
                if (attachment != null && attachment.Trim() != string.Empty)
                {
                    byte[] byteArray = Encoding.Default.GetBytes(attachment);
                    attachments.Add(new HTBEmailAttachment(new MemoryStream(byteArray), (attachmentName ?? "Mahnung.pdf"), "text/xml"));

                }
                return _emailSender.SendEmail(_fromOfficeName + " " + serverSettings.ServerSystemMail, to, ReplyTo, subject, body, true, attachments);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Log.Error("Inner: "+ex.InnerException.Message);
                }
                return false;
            }
        }
        #endregion

        #region Klient Receipt
        public bool SendKlientReceipt(tblKlient klient, List<string> toEmailAddresses, string body, Stream attachment, bool includeAgb = true)
        {
            Log.Error("Sending Klient Receipt");
            try
            {
                var to = toEmailAddresses;
                if (to == null || to.Count == 0)
                    to = HTBUtils.GetKlientEmailAddresses(klient.KlientID, klient.KlientEMail);

                if (HTBUtils.IsTestEnvironment)
                    to.Clear();

                var sb = new StringBuilder(body);

                var subject = control.KlientReceiptEmailSubject.Replace("<date>", DateTime.Now.ToShortDateString());
                var subject2 = HTBUtils.GetKlientNotifiationEmailSubject(klient, sb, to);

                
                
                if (!string.IsNullOrEmpty(subject2))
                {
                    subject += " " + subject2;
                    to.Add(HTBUtils.GetConfigValue("Office_Email")); // send copy to office
                }
                to.Add(HTBUtils.GetConfigValue("Default_EMail_Addr")); // send copy to default email for backup purposes

                var attachFile = new HTBEmailAttachment(attachment, "Auftragsbestätigung_" + DateTime.Now.ToShortDateString() + ".pdf", "application/pdf");
                var attachments = new List<HTBEmailAttachment>{attachFile};
                if (includeAgb)
                    attachments.Add(new HTBEmailAttachment(new FileInputStream(HTBUtils.GetConfigValue("AGB_File")), "ECP_AGB.pdf", "application/pdf"));

                var ok = _emailSender.SendEmail(_fromOfficeName + " " + serverSettings.ServerSystemMail, to, ReplyTo, subject, sb.ToString(), true, attachments);

                Log.Info(ok
                    ? $"Klient [ID: {klient.KlientID}] [Name: {klient.KlientName1}] Receipt sent to: [{string.Join("  ", to)}]"
                    : $"Could not send email to klient [ID: {klient.KlientID}] [Name: {klient.KlientName1}] Receipt sent to: [{string.Join("  ", to)}]");
                return ok;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Log.Error("Inner: " + ex.InnerException.Message);
                }
                return false;
            }
        }
        public bool SendAgReceipt(tblAuftraggeber ag, String body, Stream attachment, bool includeAgb = true)
        {
            Log.Info("Sending Klient Receipt");
            try
            {
                // To
                var sb = new StringBuilder(body);

                var receipients = HTBUtils.GetValidEmailAddressesFromString(ag.AuftraggeberEMail);
                if (HTBUtils.IsTestEnvironment)
                    receipients.Clear();

                var subject = control.KlientReceiptEmailSubject.Replace("<date>", DateTime.Now.ToShortDateString());
                receipients.Add(HTBUtils.GetConfigValue("Default_EMail_Addr")); // send copy to default email for backup purposes

                var to = new List<string>();
                foreach (string toAdr in receipients)
                {
                    if(HTBUtils.IsValidEmail(toAdr))
                        to.Add(toAdr);
                }

                var attachFile = new HTBEmailAttachment(attachment, "Auftragsbestätigung_" + DateTime.Now.ToShortDateString() + ".pdf", "application/pdf");
                var attachments = new List<HTBEmailAttachment> { attachFile };
                if (includeAgb)
                    attachments.Add(new HTBEmailAttachment(new FileInputStream(HTBUtils.GetConfigValue("AGB_File")), "ECP_AGB.pdf", "application/pdf"));

                return _emailSender.SendEmail(_fromOfficeName + " " + serverSettings.ServerSystemMail, to, ReplyTo, subject, sb.ToString(), true, attachments);
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Log.Error("Inner: " + ex.InnerException.Message);
                }
                return false;
            }
        }
        #endregion

        private String GetHtmlTableHeader(string[] header)
        {
            var sb = new StringBuilder("<TABLE>");
            sb.Append("<TR>");
            for (int i = 0; i < header.Length; i++)
            {
                sb.Append("<TH>");
                sb.Append(header[i]);
                sb.Append("</TH>");
            }
            sb.Append("</TR>");
            return sb.ToString();
        }

        #region Generic Email

        public bool SendGenericEmail(string to, string subject, string body, bool ishtml = true, int inkAktId = 0)
        {
            return SendGenericEmail(new List<string> { to }, subject, body, ishtml, inkAktId);
        }

        public bool SendGenericEmail(IEnumerable<string> to, string subject, string body, bool ishtml = true, int inkAktId = 0, int intAktId = 0)
        {
            return SendGenericEmail(null, to, subject, body, ishtml, inkAktId, intAktId);
        }
        public bool SendGenericEmail(string from, IEnumerable<string> to, string subject, string body, bool ishtml, int inkAktId, int intAktId)
        {
            try
            {
                Log.Info("Sending Generic Email");
                var ok = _emailSender.SendEmail(from, to, ReplyTo, subject, body, ishtml, null);
                Log.Info("Email was sent");
                if (ok)
                    EmailSentSuccessfully(subject, to, null, inkAktId, intAktId);
                
                Log.Info("Email success OK");

                return ok;
            }
            catch (Exception ex)
            {
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
                EmailSentError(subject, to, inkAktId, intAktId, ex);
                return false;
            }
        }

        public bool SendGenericEmail(IEnumerable<string> to, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments, int inkAktId, int intAktId)
        {
            return SendGenericEmail(null, to, subject, body, ishtml, attachments, inkAktId, intAktId);
        }

        public bool SendGenericEmail(string from, IEnumerable<string> to, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments, int inkAktId, int intAktId)
        {
            return SendGenericEmail(from, to, null, null, subject, body, ishtml, attachments, inkAktId, intAktId);
        }
        public bool SendGenericEmail(string from, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments, int inkAktId, int intAktId)
        {
            try
            {
                var sender = _fromOfficeName + " " + serverSettings.ServerSystemMail;
                if (from != null && HTBUtils.IsValidEmail(from))
                {
                    sender = from;
                }
                var attachmentsToDispose = new List<Stream>();
                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        attachmentsToDispose.Add(attachment.AttachmentStream);
                    }
                }
                var ok = _emailSender.SendEmail(sender, to, cc, bcc, ReplyTo, subject, body, ishtml, attachments);

                if (ok)
                    EmailSentSuccessfully(subject, to, null, inkAktId, intAktId);
                foreach (var s in attachmentsToDispose)
                {
                    try
                    {
                        s.Close();
                        s.Dispose();
                    }
                    catch
                    {
                    }
                }
                return ok;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                EmailSentError(subject, to, inkAktId, intAktId, ex);
                return false;
            }
        }
        #endregion

        #region Private Helper Methods
        private void EmailSentSuccessfully(string subject, IEnumerable<string> to, List<string> cc, int inkAktId, int intAktId)
        {
            if(inkAktId > 0)
                AddInkassoAction(subject, to, cc, inkAktId, "EMail Geschickt!");
            if(intAktId > 0)
                AddInterventionAction(subject, to, cc, intAktId, _intAktEmailSentActionTypeId);
        }
        private void EmailSentError(string subject, IEnumerable<string> to, int inkAktId, int intAktId, Exception ex)
        {
            if (inkAktId > 0)
                AddInkassoAction(subject, to, null, inkAktId, string.Format("EMail Wurde NICHT Geschickt! []" + ex.Message));
            if(intAktId > 0)
                AddInterventionAction(subject, to, null, intAktId, _intAktEmailNotSentActionTypeId, string.Format("EMail Wurde NICHT Geschickt! []" + ex.Message));
        }
        private void AddInkassoAction(string subject, IEnumerable<string> to, List<string> cc, int aktId, string action)
        {
            if (aktId <= 0) return;
            
            var sb = new StringBuilder("Betref: ");
            sb.Append(subject);
            sb.Append("\nAN: ");
            sb.Append(string.Join("  ", to));
            if (cc != null && cc.Count > 0)
            {
                sb.Append("\nCC: ");
                sb.Append(string.Join("  ", cc));
            }
            HTBUtils.AddInkassoAction(aktId, action, sb.ToString(), control.DefaultSB);       
        }

        private void AddInterventionAction(string subject, IEnumerable<string> to, List<string> cc, int aktId, int actionTypeId, string errorMessage = null)
        {
            if (aktId <= 0) return;
            

            var sb = new StringBuilder();
            if (errorMessage != null)
            {
                sb.Append(errorMessage);
                sb.Append(" \n");
            }

            sb.Append("Betref: ");
            sb.Append(subject);
            sb.Append("\nAN: ");
            sb.Append(string.Join("  ", to));
            if (cc != null && cc.Count > 0)
            {
                sb.Append("\nCC: ");
                sb.Append(string.Join("  ", cc));
            }
            HTBUtils.AddInterventionAction(aktId, actionTypeId, sb.ToString(), control.DefaultSB);
        }

        string IHTBEmail.GetLawyerReminderEMailBody(qryCustInkAkt akt, tblLawyer lawyer)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
