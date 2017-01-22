using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HTB.Database;
using HTB.Database.Views;
using HTBUtilities;

namespace HTBReports
{
    public class MahnungManager
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly MahnungsListRecord _inCentroMahListRec = new MahnungsListRecord();
        readonly MahnungsListRecord _ecpMahListRec = new MahnungsListRecord();
        readonly ArrayList _processedList = new ArrayList();

        public void GenerateMahnungen (int userId) 
        {
            try
            {
                _inCentroMahListRec.MahnungsList.Clear();
                _ecpMahListRec.MahnungsList.Clear();

                ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblMahnung WHERE MahnungStatus = 0 ORDER BY MahnungRateID", typeof(tblMahnung));
                if (list.Count > 0)
                {
                    foreach (tblMahnung mahnung in list)
                    {
                        GenerateMahnungXML(mahnung);
                    }
                    string xmlFileName =  SaveMahnungXmlText();
                    // set mahnung status to 1 = sent
                    foreach (tblMahnung mahnung in list)
                    {
                        mahnung.MahnungXMLPath = xmlFileName;
                        mahnung.MahnungStatus = 1;
                        RecordSet.Update(mahnung);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            var pdfMahnunged = new MahnungPdfReportGenerator();
            pdfMahnunged.GenerateMahnungOrTerminverlust(_ecpMahListRec);
            foreach (Mahnung mah in _ecpMahListRec.MahnungsList)
            {
                GenerateDocumentRecord(mah, userId);
                GenerateActionRecord(mah, userId);
            }
        }

        public EcpMahnung GetMahnungForAkt(qryCustInkAkt akt)
        {
            var mahnung = new EcpMahnung();
            ArrayList invList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + akt.CustInkAktID, typeof(tblCustInkAktInvoice));
            if (akt != null)
            {
                mahnung.Number = HTBUtils.GetIntFromQuery("SELECT COUNT(*) IntValue FROM tblMahnung WHERE MahnungStatus >= 1 AND MahnungAktID = " + akt.CustInkAktID, typeof(SingleValue));
                if (mahnung.Number < 0)
                    mahnung.Number = 0;
                mahnung.Number++;

                mahnung.Assign(akt);
                mahnung.Faelligkeitsdatum = DateTime.Now.AddDays(akt.CustInkAktNextWFLStep.Subtract(DateTime.Now).Days - 1);
                TimeSpan daysDifference = mahnung.Faelligkeitsdatum.Subtract(DateTime.Now);
                if (daysDifference.TotalDays < 5)
                {
                    mahnung.Faelligkeitsdatum = DateTime.Now.AddDays(5);
                }

                AssignMahnungKosten(mahnung, invList);
                mahnung.CalculateTotal();
            }
            return mahnung;
        }

        public void GenerateDocumentRecord(Mahnung mah, int userId)
        {
            var doc = new tblDokument
                          {
                              // CollectionInvoice
                              DokDokType = 25,
                              DokCaption = mah is TerminverlustRec ? "Terminverlust: " + ((TerminverlustRec) mah).RateDate.ToShortDateString() : "Mahnung " + mah.Number,
                              DokInkAkt = mah.Aktenzahl,
                              DokCreator = userId,
                              DokAttachment = mah is TerminverlustRec ? "TVL_" + mah.Aktenzahl + "_" + ((TerminverlustRec) mah).RateID + ".pdf" : "Mahnung_" + mah.Aktenzahl + "_" + mah.Number + ".pdf",
                              DokCreationTimeStamp = DateTime.Now,
                              DokChangeDate = DateTime.Now
                          };
            
            RecordSet.Insert(doc);

            doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof(tblDokument));
            if (doc != null)
            {
                RecordSet.Insert(new tblAktenDokumente {ADAkt = mah.Aktenzahl, ADDok = doc.DokID, ADAkttyp = 1});
            }
        }

        private void GenerateActionRecord(Mahnung mah, int userId)
        {

        }

        private void GenerateMahnungXML(tblMahnung rec)
        {
            if (!IsAktProcessed(rec.MahnungAktID))
            {
                var ecpMahnung = new EcpMahnung();
                ecpMahnung.Assign(rec);
                if (rec.MahnungAktID > 0)
                {
                    ArrayList invList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + rec.MahnungAktID, typeof (tblCustInkAktInvoice));
                    var akt = (qryCustInkAkt) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + rec.MahnungAktID, typeof (qryCustInkAkt));
                    if (akt != null)
                    {
                        ecpMahnung.Number = GetNextMahnungNumber(rec.MahnungAktID);
                        ecpMahnung.Assign(akt);
                        // Fälligkeitsdatum Schuldner Mahnungsdok (WFL Datum -5 Tage)
                        ecpMahnung.Faelligkeitsdatum = DateTime.Now.AddDays(akt.CustInkAktNextWFLStep.Subtract(DateTime.Now).Days - 5);
                        TimeSpan daysDifference = ecpMahnung.Faelligkeitsdatum.Subtract(DateTime.Now);
                        if (daysDifference.TotalDays < 5)
                        {
                            ecpMahnung.Faelligkeitsdatum = DateTime.Now.AddDays(5);
                        }
                        AssignMahnungKosten(ecpMahnung, invList);
                        ecpMahnung.CalculateTotal();
                        if (rec.MahnungType == 1 && rec.MahnungRateID > 0)
                        {
                            var rate = (tblCustInkAktRate) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktRate WHERE CustInkAktRateID = " + rec.MahnungRateID, typeof (tblCustInkAktRate));
                            if (rate != null)
                            {
                                var tvl = new EcpTerminverlustRec();
                                tvl.Assign(ecpMahnung);
                                tvl.RateDate = rate.CustInkAktRateDueDate;
                                tvl.RateID = rate.CustInkAktRateID;
                                _inCentroMahListRec.MahnungsList.Add(new TerminverlustRec(tvl));
                                _ecpMahListRec.MahnungsList.Add(tvl);
                            }
                        }
                        else
                        {
                            _inCentroMahListRec.MahnungsList.Add(new Mahnung(ecpMahnung));
                            _ecpMahListRec.MahnungsList.Add(ecpMahnung);
                        }
                    }
                }
                _processedList.Add(rec.MahnungAktID);
            }
        }

        private string SaveMahnungXmlText()
        {
            /*
            using (StreamWriter outfile = new StreamWriter(HTBUtils.GetConfigValue("Mahnung_Text_File")+DateTime.Now+".txt"))
            {
                foreach (Mahnung m in mahListRec.MahnungsList)
                {
                    outfile.WriteLine("[Akt: " + m.Aktenzahl + "]  [Mahnung: " + m.ID + "]  [Number: " + m.Number+ "]  [Total: " + m.Forderung + "]");
                    outfile.WriteLine("========================================");
                    foreach (tblCustInkAktInvoice i in m.InvList)
                    {
                        outfile.WriteLine("\t"+i.ToTabString());
                    }
                    outfile.WriteLine("\r\n========================================");
                }
                outfile.Flush();
                outfile.Close();
            }
            */
            string fileName = HTBUtils.GetConfigValue("Mahnung_Text_File") + "_" + HTBUtils.GetPathTimestamp() + ".xml";
            using (var outfile = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write), Encoding.UTF8))
            {
                outfile.WriteLine(_inCentroMahListRec.ToXmlString());
                outfile.Flush();
                outfile.Close();
                outfile.Dispose();
            }

            // send to inCentro (SFTP)
            HTBSftp.SendFile(fileName);

//             send email to inCentro to let them know they can send this thing away
//            new HTBEmail().SendGenericEmail(
//                    new string[] { HTBUtils.GetConfigValue("Mahnung_FTP_Email"), HTBUtils.GetConfigValue("Mahnung_FTP_CC") },
//                    string.Format("Mahnung(en)  [ {0} ]  [ {1} ]", HTBUtils.GetJustFileName(fileName), _mahListRec.MahnungsList.Count),
//                    "Dateiname: " + HTBUtils.GetJustFileName(fileName) + "<br/>" + "Stückzahl: " + _mahListRec.MahnungsList.Count,
//                    true);

            return fileName;
        }

        private bool IsAktProcessed(int aktId)
        {
            return _processedList.Cast<int>().Any(id => id == aktId);
        }

        public void AssignMahnungKosten(EcpMahnung mahnung, ArrayList invList)
        {
            foreach (tblCustInkAktInvoice inv in invList)
            {
                if (inv.IsPayment())
                {
                    if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CREDIT)
                        mahnung.KostenReduktion += inv.InvoiceAmount;
                    else
                        mahnung.Bezahlt += inv.InvoiceAmount;
                }
                else if(inv.IsInterest() || inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST)
                {
                    mahnung.MahnspesenZinsen += inv.InvoiceAmountNetto;
                }
                else {
                    switch (inv.InvoiceType)
                    {
                        case tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL:
                            mahnung.Forderung += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_1:
                            mahnung.Mahnung1 += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_2:
                            mahnung.Mahnung2 += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_3:
                            mahnung.Mahnung3 += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_4:
                            mahnung.Mahnung4 += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_5:
                            mahnung.Mahnung5 += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_TELEFON:
                            mahnung.Telefonincasso += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_EVIDENZ:
                            mahnung.Evidenzhaltung += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_PERSONAL_INTERVENTION:
                            mahnung.InterventionWeg += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_RATE_ANGEBOT:
                            mahnung.Ratenangebot += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_OVERDUE_CHARGE:
                            mahnung.Terminverlust += inv.InvoiceAmountNetto;
                            break;
                        case tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MELDE:
                            mahnung.Meldeergebung += inv.InvoiceAmountNetto;
                            break;
                        default: 
                            mahnung.Bearbeitung += inv.InvoiceAmountNetto;
                            break;
                    }
                    mahnung.Steuer += inv.InvoiceTax;
                }
            }
            mahnung.CalculateTotal();
        }

        public static int GetNextMahnungNumber(int aktId)
        {
            int rett;
            try
            {
                rett = HTBUtils.GetIntFromQuery("SELECT COUNT(*) IntValue FROM tblMahnung WHERE  MahnungNr <> 0 AND MahnungStatus >= 1 AND MahnungAktID = " + aktId, typeof(SingleValue));
            }
            catch
            {
                rett = 0;
            }
            return rett + 1;
        }
    }
}
