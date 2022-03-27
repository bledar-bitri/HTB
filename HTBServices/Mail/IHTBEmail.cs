using System;
using System.Collections.Generic;
using HTB.Database;
using System.IO;
using HTB.Database.Views;

namespace HTBServices.Mail
{
    public interface IHTBEmail
    {
       
        #region Lawyer

        bool SendLawyerPackage(IEnumerable<string> to, qryCustInkAkt akt, IEnumerable<HTBEmailAttachment> attachments, string body = "");

        bool SendLawyerReminder(qryCustInkAkt akt);
        string GetLawyerReminderEMailBody(qryCustInkAkt akt, tblLawyer lawyer);

        #endregion

        #region Melde Research

        bool SendMeldeResearchNotice(tblAktMelde melde);

        #endregion

        #region Mahnung

        bool SendMahnung(string body, string attachment, string subject = null, string attachmentName = null);
        #endregion

        #region Klient Receipt

        bool SendKlientReceipt(tblKlient klient, List<string> toEmailAddresses, string body, Stream attachment, bool includeAgb = true);

        bool SendAgReceipt(tblAuftraggeber ag, String body, Stream attachment, bool includeAgb = true);
        #endregion

        #region Generic Email

        bool SendGenericEmail(string to, string subject, string body, bool ishtml = true, int inkAktId = 0);

        bool SendGenericEmail(IEnumerable<string> to, string subject, string body, bool ishtml = true, int inkAktId = 0, int intAktId = 0);

        bool SendGenericEmail(string from, IEnumerable<string> to, string subject, string body, bool ishtml,
            int inkAktId, int intAktId);

        bool SendGenericEmail(IEnumerable<string> to, string subject, string body, bool ishtml,
            IEnumerable<HTBEmailAttachment> attachments, int inkAktId, int intAktId);

        bool SendGenericEmail(string from, IEnumerable<string> to, string subject, string body, bool ishtml,
            IEnumerable<HTBEmailAttachment> attachments, int inkAktId, int intAktId);

        bool SendGenericEmail(string from, IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc,
            string subject, string body, bool ishtml, IEnumerable<HTBEmailAttachment> attachments, int inkAktId,
            int intAktId);
        
        #endregion
    }
}
