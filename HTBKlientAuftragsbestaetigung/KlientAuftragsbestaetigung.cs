using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using HTB.Database;
using HTB.Database.HTB.StoredProcs;
using HTBReports;
using HTBUtilities;
using System.Diagnostics;
using System.Threading;

namespace HTBKlientAuftragsbestaetigung
{
    public class KlientAuftragsbestaetigung
    {
        private static tblControl _control = HTBUtils.GetControlRecord();
        private static DateTime _startDate;
        private static string _aktIntTypeAuto;

        private KlientAuftragsbestaetigung(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            _control = HTBUtils.GetControlRecord();
            _startDate = GetDefaultDateIfConvertToDateError(HTBUtils.GetConfigValue("AuftragsbestaetigungsStartDate"));
            _aktIntTypeAuto = HTBUtils.GetConfigValue("AktIntTypeAuto");
            if (_startDate != HTBUtils.DefaultDate)
            {
                //SendClientAuftragReceipts();
                SendAgAuftragReceipts();
            }
        }

        static void SendClientAuftragReceipts()
        {
            String klientQuery = "SELECT DISTINCT k.klientid AS [IntValue] FROM tblKlient k inner join tblCustInkAkt a " +
                                " on k.KlientID = a.CustInkAktKlient " +
                                " WHERE a.CustInkAktEnterDate >= '" + _startDate + "' AND CustInkAktSendBericht = 1 AND CustInkAktIsAuftragsbestaetigungSent = 0";
            // test query
            //klientQuery = "SELECT DISTINCT k.klientid AS [IntValue] FROM tblKlient k inner join tblCustInkAkt a " +
            //                " on k.KlientID = a.CustInkAktKlient " +
            //                " WHERE KlientID = 9300";

            ArrayList klientsList = HTBUtils.GetSqlRecords(klientQuery, typeof(SingleValue));


            foreach (SingleValue klientId in klientsList)
            {
                var klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientId = " + klientId.IntValue, typeof(tblKlient));

                if (klient != null)
                {
                    ArrayList users = HTBUtils.GetSqlRecords("select * from tblUser where UserStatus = 1 and UserKlient = " + klientId.IntValue, typeof(tblUser));
                    if (users.Count > 0)
                    {
                        foreach (tblUser user in users)
                        {
                            GenerateAndSendClientReceipt(klientId.IntValue, user);
                        }
                    }
                    GenerateAndSendClientReceipt(klientId.IntValue);
                }
            }
        }

        private static void GenerateAndSendClientReceipt(int clientId, tblUser user = null)
        {
            var email = new HTBEmail();
            List<string> emailAddresses = null;
            var paramaters = new ReportParameters
            {
                StartKlient = clientId,
                EndKlient = clientId,
                StartDate = GetStartOrEndDateForKlient(clientId),
                EndDate = GetStartOrEndDateForKlient(clientId, false)
            };
            if (user != null)
            {
                paramaters.KlientSB = user.UserID;
                emailAddresses = HTBUtils.GetValidEmailAddressesFromStrings(new string[] { user.UserEMailOffice, user.UserEMailPrivate });
            }
            
            var receipt = new AuftragReceipt();
            if (!receipt.HasRecords(paramaters))
                return;
            
            using (var stream = new MemoryStream())
            {
                receipt.GenerateClientReceipt(paramaters, stream);
                var klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientId = " + clientId, typeof(tblKlient));
                if (klient != null)
                {
                    String body = GetKlientReceiptBody(klient, user);
                    if (body != null)
                    {
                        email.SendKlientReceipt(klient, emailAddresses, body, HTBUtils.ReopenMemoryStream(stream));
                        //SaveAndShowFile(stream, "c:/temp/Auftragsbestetigung.pdf"); // comment for production
                        SaveKlientAuftragReceipt(receipt.RecordsList, HTBUtils.ReopenMemoryStream(stream), clientId, (user != null ? user.UserID : -1));
                        UpdateInkassoAkts(receipt.RecordsList);
                        // insert log record so that we know when the receipt was sent
                        RecordSet.Insert(new tblCommunicationLog { ComLogKlientID = clientId, ComLogType = tblCommunicationLog.COMMUNICATION_TYPE_RECEIPT, ComLogDate = DateTime.Now });
                    }
                }

            }
        }

        static void SendAgAuftragReceipts()
        {
            String agQuery = "SELECT DISTINCT ag.AuftraggeberID AS [IntValue] FROM tblAuftraggeber ag inner join tblAktenInt a " +
                                 " on ag.AuftraggeberID = a.AktIntAuftraggeber " +
                                 " WHERE ag.AuftraggeberSendConfirmation = 1 AND a.AktIntDatum >= '" + _startDate + "' AND a.AktIntIsAuftragsbestaetigungSent = 0 AND A.AktIntAktType in (" + _aktIntTypeAuto + ")";

            ArrayList agList = HTBUtils.GetSqlRecords(agQuery, typeof(SingleValue));

            var email = new HTBEmail();

            foreach (SingleValue agId in agList)
            {

                using (var stream = new MemoryStream())
                {
                    var paramaters = new ReportParameters
                    {
                        StartAuftraggeber = agId.IntValue,
                        EndAuftraggeber = agId.IntValue,
                        StartDate = GetStartOrEndDateForAg(agId.IntValue),
                        EndDate = GetStartOrEndDateForAg(agId.IntValue, false)
                    };

                    var receipt = new AuftragReceipt();
                    receipt.GenerateAgReceipt(paramaters, stream);
                    var ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberId = " + agId.IntValue, typeof(tblAuftraggeber));
                    if (ag != null)
                    {
                        String body = GetAgReceiptBody();
                        if (body != null)
                        {
                            email.SendAgReceipt(ag, body, HTBUtils.ReopenMemoryStream(stream), false);
                            //SaveAndShowFile(stream, "c:/temp/Auftragsbestetigung2.pdf");
                            SaveAgAuftragReceipt(receipt.RecordsList, HTBUtils.ReopenMemoryStream(stream), agId.IntValue);
                            UpdateAgAkt(receipt.RecordsList);
                            // insert log record so that we know when the receipt was sent
                            RecordSet.Insert(new tblCommunicationLog { ComLogKlientID = agId.IntValue, ComLogType = tblCommunicationLog.COMMUNICATION_TYPE_RECEIPT, ComLogDate = DateTime.Now });
                        }
                    }
                }
            }
        }

        private static string GetKlientReceiptBody(tblKlient klient, tblUser user)
        {
            var sb = new StringBuilder();
            if (klient != null)
            {

                StreamReader re = File.OpenText(HTBUtils.GetConfigValue("Klient_Receipt_Text"));
                string input = null;
                while ((input = re.ReadLine()) != null)
                {
                    sb.Append(input);
                }
                re.Close();
                re.Dispose();
                if (user != null)
                {
                    if (user.UserSex == 1)
                    {
                        return sb.ToString().Replace("[name]", "r Herr " + user.UserNachname);
                    }
                    return sb.ToString().Replace("[name]", " Frau " + user.UserNachname);
                }

                var contact = (tblAnsprechpartner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAnsprechpartner WHERE AnsprechKlient = " + klient.KlientID, typeof(tblAnsprechpartner));
                if (contact != null)
                {
                    if (contact.AnsprechTitel.Trim().ToUpper() == "HERR")
                    {
                        return sb.ToString().Replace("[name]", "r " + contact.AnsprechTitel + " " + contact.AnsprechNachname);
                    }
                    return sb.ToString().Replace("[name]", " " + contact.AnsprechTitel + " " + contact.AnsprechNachname);
                }

                return sb.ToString().Replace("[name]", " Damen und Herren");
            }
            return null;
        }
        private static string GetAgReceiptBody()
        {
            var sb = new StringBuilder();
            StreamReader re = File.OpenText(HTBUtils.GetConfigValue("Klient_Receipt_Text"));
            string input = null;
            while ((input = re.ReadLine()) != null)
            {
                sb.Append(input);
            }
            re.Close();
            re.Dispose();
            return sb.ToString().Replace("[name]", " Damen und Herren");
        }

        private static void SaveKlientAuftragReceipt(ArrayList recordsList, MemoryStream stream, int klientId, int userId = -1)
        {
            var documentsFolder = HTBUtils.GetConfigValue("DocumentsFolder");
            var filename =
                $"{klientId}_{(userId >= 0 ? userId.ToString(CultureInfo.InvariantCulture) : "")}_AB_{HTBUtils.GetPathTimestamp()}.pdf";
            filename = HTBUtils.SanitizeFileName(filename);
            HTBUtils.SaveMemoryStream(stream, documentsFolder + filename);
            foreach (spAGReceipt rec in recordsList)
            {
                var doc = new tblDokument
                {
                    // CollectionInvoice
                    DokDokType = 25,
                    DokCaption = "Auftragsbestätigung",
                    DokInkAkt = rec.CustInkAktID,
                    DokCreator = _control.AutoUserId,
                    DokAttachment = filename,
                    DokCreationTimeStamp = DateTime.Now,
                    DokChangeDate = DateTime.Now
                };

                RecordSet.Insert(doc);

                doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof(tblDokument));
                if (doc != null)
                {
                    RecordSet.Insert(new tblAktenDokumente { ADAkt = rec.CustInkAktID, ADDok = doc.DokID, ADAkttyp = 1 });
                }
            }
        }
        private static void SaveAgAuftragReceipt(ArrayList recordsList, MemoryStream stream, int agId)
        {
            string documentsFolder = HTBUtils.GetConfigValue("DocumentsFolder");
            string filename = agId + "_AB_" + HTBUtils.GetPathTimestamp() + ".pdf";
            HTBUtils.SaveMemoryStream(stream, documentsFolder + filename);
            foreach (spAGReceipt rec in recordsList)
            {
                var doc = new tblDokument
                {
                    // Intervention
                    DokDokType = 26,
                    DokCaption = "Auftragsbestätigung",
                    DokInkAkt = rec.AktIntID,
                    DokCreator = _control.AutoUserId,
                    DokAttachment = filename,
                    DokCreationTimeStamp = DateTime.Now,
                    DokChangeDate = DateTime.Now
                };

                RecordSet.Insert(doc);

                doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof(tblDokument));
                if (doc != null)
                {
                    RecordSet.Insert(new tblAktenDokumente { ADAkt = rec.AktIntID, ADDok = doc.DokID, ADAkttyp = 3 });
                }
            }
        }


        private static void UpdateInkassoAkts(ArrayList recordsList)
        {
            var set = new RecordSet();
            try
            {
                foreach (spAGReceipt rec in recordsList)
                {
                    set.ExecuteNonQuery("UPDATE tblCustInkAkt set CustInkAktIsAuftragsbestaetigungSent = 1 WHERE CustInkAktID = " + rec.CustInkAktID);
                }
            }
            catch (Exception)
            {
                set.RollbackTransaction();
            }
        }
        private static void UpdateAgAkt(ArrayList recordsList)
        {
            var set = new RecordSet();
            try
            {
                foreach (spAGReceipt rec in recordsList)
                {
                    set.ExecuteNonQuery("UPDATE tblAktenInt set AktIntIsAuftragsbestaetigungSent = 1 WHERE AktIntID = " + rec.AktIntID);
                }
            }
            catch (Exception ex)
            {
                set.RollbackTransaction();
            }
        }

        private static DateTime GetDefaultDateIfConvertToDateError(string value)
        {
            DateTime rett;
            try
            {
                rett = Convert.ToDateTime(value);
            }
            catch
            {
                rett = new DateTime(1900, 1, 1);
            }
            return rett;
        }

        private static DateTime GetStartOrEndDateForKlient(int klientId, bool isStart = true)
        {
            string sqlFunctionName = isStart ? "MIN" : "MAX";
            var sb = new StringBuilder("SELECT ");
            sb.Append(sqlFunctionName);
            sb.Append(" (CustInkAktEnterDate) DateValue FROM tblCustInkAkt WHERE CustInkAktSendBericht = 1 AND CustInkAktIsAuftragsbestaetigungSent = 0 AND CustInkAktEnterDate >= '");
            sb.Append(_startDate.ToShortDateString());
            sb.Append("' AND CustInkAktKlient = ");
            sb.Append(klientId);
            var sv = (SingleValue)HTBUtils.GetSqlSingleRecord(sb.ToString(), typeof(SingleValue));
            return sv.DateValue;
        }
        private static DateTime GetStartOrEndDateForAg(int agId, bool isStart = true)
        {
            string sqlFunctionName = isStart ? "MIN" : "MAX";
            var sb = new StringBuilder("SELECT ");
            sb.Append(sqlFunctionName);
            sb.Append(" (AktIntDatum) DateValue FROM tblAktenint WHERE AktIntIsAuftragsbestaetigungSent = 0 AND AktIntDatum >= '");
            sb.Append(_startDate.ToShortDateString());
            sb.Append("' AND AktIntAktType in (");
            sb.Append(_aktIntTypeAuto);
            sb.Append(") AND AktIntAuftraggeber = ");
            sb.Append(agId);
            var sv = (SingleValue)HTBUtils.GetSqlSingleRecord(sb.ToString(), typeof(SingleValue));
            return sv.DateValue;
        }

        private static void SaveAndShowFile(MemoryStream stream, string fileName)
        {
            HTBUtils.SaveMemoryStream(HTBUtils.ReopenMemoryStream(stream), fileName);
            var proc = new Process {StartInfo = {FileName = fileName}};
            proc.Start();
        }
        static void Main(string[] args)
        {
            new KlientAuftragsbestaetigung(args);
        }
    }
}
