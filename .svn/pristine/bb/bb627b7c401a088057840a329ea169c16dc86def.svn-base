using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using HTB.Database;
using System.IO;
using HTBUtilities;
using HTBInvoiceManager;
using HTBAktLayer;
using System.Collections;
using System.Threading;

namespace DataImport
{
    class InkassoAktImporter
    {
        static string strAktConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\temp\\imports\\sn_a.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        static string strBerichtConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\temp\\imports\\sn_b.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        static string strPayConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\temp\\imports\\payments.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        static InvoiceManager invMgr = new InvoiceManager();

        public InkassoAktImporter()
        {
            ImportAkts();
            ImportPayments();
            //Console.Read();
        }
        public void ImportActions()
        {
            OleDbConnection con = new OleDbConnection(strBerichtConn);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [bericht$]", con);
            DataTable tables = GetDatabaseTables_SQL();
            DataTable dt = new DataTable();
            da.Fill(dt);
            int count = 1;
            RecordSet set = new RecordSet();
            Console.WriteLine("IMPORTING BERICHTEN");
            using (StreamWriter outfile = new StreamWriter(@"c:\temp\imports\Bericht.txt"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (!row["AUFTRAG"].ToString().Trim().Equals("NO_SAVE"))
                    {
                        //Console.WriteLine("\tROW: " + (count++) + "\tID: " + row["AUFTRAG"] + "  TEXT: " + row["DESCRIPTION"]);
                        tblCustInkAkt akt = (tblCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAkt WHERE CustInkAktAZ = " + NormalizeString(row["AUFTRAG"]), typeof(tblCustInkAkt));
                        if (akt != null)
                        {
                            string status = NormalizeString(row["STATUS"]) + " " + NormalizeString(row["ABSCHLUSS"]) + " " + NormalizeString(row["BEARBEITER"]) + " " + NormalizeString(row["EF_STATUS"]);
                            string iinfo = NormalizeString(row["IINFO"]);

                            if (status.Trim() != string.Empty)
                                CreateAction(set, akt.CustInkAktID, row, "Status: " + status);
                            if (iinfo.Trim() != string.Empty)
                                CreateAction(set, akt.CustInkAktID, row, "Interninfo: " + iinfo);

                            object[] actions = GetActions(NormalizeString(row["DESCRIPTION"]));
                            foreach (object objAction in actions)
                            {
                                string strAction = objAction.ToString();
                                outfile.WriteLine(NormalizeString(row["AUFTRAG"]) + "\t" + strAction);
                                Console.WriteLine("\tROW: " + (count++) + "\tID: " + row["AUFTRAG"]);

                                var action = new tblCustInkAktAktion();
                                action.CustInkAktAktionAktID = akt.CustInkAktID;
                                action.CustInkAktAktionDate = Convert.ToDateTime(strAction.Substring(0, 10));
                                action.CustInkAktAktionEditDate = action.CustInkAktAktionDate;
                                action.CustInkAktAktionMemo = strAction.Substring(11).Replace("\n", "");
                                set.InsertRecord(action);
                                if (NormalizeString(row["Status"]).Trim().Equals("I"))
                                {
                                    akt.CustInkAktStatus = 1; // mahnwesen
                                    akt.CustInkAktCurStatus = 1; // erste mahnung
                                    set.UpdateRecord(akt);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Could not find akt for auftrag " + NormalizeString(row["AUFTRAG"]));
                        }
                    }
                }
            }
            //Console.Read();
        }

        public void ImportPayments()
        {
            OleDbConnection con = new OleDbConnection(strPayConn);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [pay$]", con);
            DataTable tables = GetDatabaseTables_SQL();
            DataTable dt = new DataTable();
            da.Fill(dt);
            int count = 1;
            RecordSet set = new RecordSet();
            Console.WriteLine("IMPORTING PAYMENTS");
            
            foreach (DataRow row in dt.Rows)
            {
                if (!row["uakaktenzahl"].ToString().Trim().Equals("NO_SAVE"))
                {
                    //Console.WriteLine("\tROW: " + (count++) + "\tID: " + row["AUFTRAG"] + "  TEXT: " + row["DESCRIPTION"]);
                    tblCustInkAkt akt = (tblCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAkt WHERE CustInkAktAZ = '" + NormalizeString(row["uakaktenzahl"])+"'", typeof(tblCustInkAkt));
                    if (akt != null)
                    {
                        CreatePayment(akt, row, NormalizeString(row["uakklient"]), NormalizeString(row["uakecp"]));
                    }
                    else
                    {
                        Console.WriteLine("Could not find akt for auftrag " + NormalizeString(row["uakaktenzahl"]));
                    }
                }
            }
            
            //Console.Read();
        }

        public void ImportAkts() {
            OleDbConnection con = new OleDbConnection(strAktConn);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [INKA$]", con);
            DataTable tables = GetDatabaseTables_SQL();
            DataTable dt = new DataTable();
            da.Fill(dt);
            int count = 1;
            foreach (DataRow row in dt.Rows)
            {
                //if (count >= 1632)
                {
                    tblCustInkAkt akt = GetInkassoAkt(row);
                    Console.WriteLine("\tROW: " + count + "\tID: " + NormalizeString(row["AUFTRAG"]) + "  DATE: " + akt.CustInkAktEnterDate);
                    //SaveAktText(akt);
                    RecordSet.Insert(akt);
                    akt = (tblCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAkt ORDER BY CustInkAktID DESC", typeof(tblCustInkAkt));
                    CreateInvoice(akt, tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL, NormalizeString(row["KAPITALUG"]), "Kapital - Forderung", false);
                    CreateInvoice(akt, tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_COLECTION, NormalizeString(row["ZINSEN_OFF"]), "Zinsen", false);
                    //CreateInvoice(akt, tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_INITIAL_PAYMENT, row["KAPITALBEZ"], "Zinsen", false);
                    CreateInvoice(akt, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE, NormalizeString(row["SPESEN_OFF"]), "Bearbeitunsgebühren", true);
                    CreateInvoice(akt, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE, NormalizeString(row["KOSTEN_OFF"]), "1.Mahnung+Ratenangebot", true);
                    CreateInvoice(akt, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MELDE, NormalizeString(row["ADRHEB"]), "Meldeerhebung", true);
                    CreateInvoice(akt, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE, NormalizeString(row["EVIDENZG"]), "Evidenzhaltung", true);
                    CreateInvoice(akt, tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE, NormalizeString(row["WEGGEB"]), "Persönliche Intervention Weggebühr", true);
                    CreateTax(akt, NormalizeString(row["MWST"]));
                    //CreateForderung(akt, NormalizeString(row["KAPITALUG"]));
                    //CreatePayment(akt, NormalizeString(row["SALDO_GES"]));

                    akt.CustInkAktIsPartial = false;

                    RecordSet.Update(akt);
                    ImportActions(row, akt.CustInkAktID);
                }
                count++;
            }
            //Console.Read();
        }

        public void ImportActions(DataRow row, int aktId)
        {

            object[] actions = GetActions(NormalizeString(row["INFO"]));
            int count = 1;
            RecordSet set = new RecordSet();
            
            tblCustInkAkt akt = (tblCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAkt WHERE CustInkAktID = " + aktId, typeof(tblCustInkAkt));
            if (akt != null)
            {
                string status = NormalizeString(row["STATUS"]) + " " + NormalizeString(row["ABSCHLUSS"]) + " " + NormalizeString(row["BEARBEITER"]) + " " + NormalizeString(row["EF_STATUS"]);
                string iinfo = NormalizeString(row["IINFO"]);

                if (status.Trim() != string.Empty)
                    CreateAction(set, akt.CustInkAktID, row, "Status: " + status);
                if (iinfo.Trim() != string.Empty)
                    CreateAction(set, akt.CustInkAktID, row, "Interninfo: " + iinfo);

                foreach (object objAction in actions)
                {
                    string strAction = objAction.ToString();
                    if (strAction.Length > 10)
                    {
                        Console.WriteLine("\tROW: " + (count++) + "\tID: " + aktId);

                        tblCustInkAktAktion action = new tblCustInkAktAktion();
                        action.CustInkAktAktionAktID = akt.CustInkAktID;
                        try
                        {
                            action.CustInkAktAktionDate = Convert.ToDateTime(strAction.Substring(0, 10));
                        }
                        catch
                        {
                            action.CustInkAktAktionDate = DateTime.Now;
                        }
                        action.CustInkAktAktionEditDate = action.CustInkAktAktionDate;
                        action.CustInkAktAktionMemo = strAction.Substring(11).Replace("\n", "");
                        set.InsertRecord(action);
                        if (NormalizeString(row["Status"]).Trim().Equals("I"))
                        {
                            akt.CustInkAktStatus = 1; // mahnwesen
                            akt.CustInkAktCurStatus = 1; // erste mahnung
                            set.UpdateRecord(akt);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Could not find akt for auftrag " + NormalizeString(row["AUFTRAG"]));
            }
        }

        private tblCustInkAkt GetInkassoAkt(DataRow r)
        {
            tblCustInkAkt akt = new tblCustInkAkt();
            
            akt.CustInkAktAZ = NormalizeString(r["AUFTRAG"]);
            akt.CustInkAktOldID = akt.CustInkAktAZ;
            akt.CustInkAktCurrentStep = -1;
            akt.CustInkAktAuftraggeber = 41; // ECP
            akt.CustInkAktInvoiceDate = GetDate(NormalizeString(r["RECHNDATUM"]));
            akt.CustInkAktEnterDate = GetDate(NormalizeString(r["ERSTELLT"]));
            akt.CustInkAktGEName = NormalizeString(r["NAME1"]);
            akt.CustInkAktGEOrt = NormalizeString(r["ORT"]);
            akt.CustInkAktGEStrasse = NormalizeString(r["STRASSE"]);
            akt.CustInkAktGEZIP = NormalizeString(r["PLZ"]);
            akt.CustInkAktLastChange = DateTime.Now;
            akt.CustInkAktNextWFLStep = DateTime.Now;
            //akt.CustInkAktKunde = r["KUNDE"].ToString();
            akt.CustInkAktStatus = 5; // storno
            akt.CustInkAktCurStatus = 63; // new status 'storno nadine'
            //akt.CustInkAktGothiaNr = r["KUNDENNR"].ToString();
            akt.CustInkAktKunde = NormalizeString(r["KUNDENNR"]);
            akt.CustInkAktForderung = GetAmount(NormalizeString(r["KAPITALUG"]));
            tblGegner g = GetGegner(r);
            if (g == null)
            {
                throw new Exception("COULD NOT CREATE GEGNER !!!!");
            }
            /*
            tblKlient k = GetKlient(r);
            if (k == null)
            {
                throw new Exception("COULD NOT FIND KLIENT !!!!");
            } 
             */
            //akt.CustInkAktKlient = k.KlientID;
            akt.CustInkAktGegner = g.GegnerID;
            akt.CustInkAktKlient = Convert.ToInt32(NormalizeString(r["KLIENTID"]));
            akt.CustInkAktIsPartial = true;
            akt.CustInkAktSkipInitialInvoices = true;
            return akt;
        }

        private tblGegner GetGegner(DataRow r)
        {
            string sql = "SELECT * FROM tblGegner WHERE "+
                                "RTRIM(LTRIM(UPPER(GegnerLastName1))) = '" + NormalizeString(r["NAME1"]).ToUpper() +
                                "' AND RTRIM(LTRIM(UPPER(GegnerLastName2))) = '" + NormalizeString(r["NAME2"]).ToUpper() +
                                "' AND RTRIM(LTRIM(UPPER(GegnerLastStrasse))) = '" + NormalizeString(r["STRASSE"]).ToUpper() +
                                "' AND GegnerLastZIP = '" + NormalizeString(r["PLZ"]) + "'";

            tblGegner gegner = (tblGegner) HTBUtils.GetSqlSingleRecord(sql, typeof(tblGegner));

            if (gegner != null)
            {

                gegner.GegnerName1 = NormalizeString(r["NAME1"]);
                gegner.GegnerName2 = NormalizeString(r["NAME2"]);
                gegner.GegnerName3 = NormalizeString(r["NAME3"]);
                if (gegner.GegnerName1.Length > 60)
                    gegner.GegnerName1 = gegner.GegnerName1.Substring(0, 60);
                if (gegner.GegnerName2.Length > 60)
                    gegner.GegnerName2 = gegner.GegnerName2.Substring(0, 60);
                if (gegner.GegnerName3.Length > 60)
                    gegner.GegnerName3 = gegner.GegnerName3.Substring(0, 60);
    
                gegner.GegnerStrasse = NormalizeString(r["STRASSE"]);
                gegner.GegnerZipPrefix = "A";
                gegner.GegnerZip = NormalizeString(r["PLZ"]);
                gegner.GegnerOrt = NormalizeString(r["ORT"]);
                gegner.GegnerAnrede = NormalizeString(r["ANREDE"]);
                if (gegner.GegnerAnrede.ToLower().Trim() == "firma")
                    gegner.GegnerType = 0;
                else if (gegner.GegnerAnrede.ToLower().Trim() == "herr")
                    gegner.GegnerType = 1;
                else if (gegner.GegnerAnrede.ToLower().Trim() == "frau")
                    gegner.GegnerType = 2;
                RecordSet.Update(gegner);
                return gegner;
            }
            else
            {
                gegner = new tblGegner();
                gegner.GegnerName1 = NormalizeString(r["NAME1"]);
                gegner.GegnerName2 = NormalizeString(r["NAME2"]);
                gegner.GegnerName3 = NormalizeString(r["NAME3"]);
                if (gegner.GegnerName1.Length > 60)
                    gegner.GegnerName1 = gegner.GegnerName1.Substring(0, 60);
                if (gegner.GegnerName2.Length > 60)
                    gegner.GegnerName2 = gegner.GegnerName2.Substring(0, 60);
                if (gegner.GegnerName3.Length > 60)
                    gegner.GegnerName3 = gegner.GegnerName3.Substring(0, 60);
                gegner.GegnerStrasse = NormalizeString(r["STRASSE"]);
                gegner.GegnerZipPrefix = "A";
                gegner.GegnerZip = NormalizeString(r["PLZ"]);
                gegner.GegnerOrt = NormalizeString(r["ORT"]);
                gegner.GegnerAnrede = NormalizeString(r["ANREDE"]);
                if (gegner.GegnerAnrede.ToLower().Trim() == "firma")
                    gegner.GegnerType = 0;
                else if (gegner.GegnerAnrede.ToLower().Trim() == "herr")
                    gegner.GegnerType = 1;
                else if (gegner.GegnerAnrede.ToLower().Trim() == "frau")
                    gegner.GegnerType = 2;
                gegner.GegnerCreateDate = DateTime.Now;
                gegner.GegnerCreateSB = 99; // Nadine Auer
                gegner.GegnerVVZDate = DateTime.Now;
                gegner.GegnerVVZEnterDate = DateTime.Now;
                gegner.GegnerGebDat = new DateTime(1900, 1, 1);
                gegner.GegnerOldID = " ";
                RecordSet.Insert(gegner);
                gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblGegner ORDER BY GegnerID DESC", typeof(tblGegner));
                if (gegner != null)
                {
                    gegner.GegnerOldID = "1000" + gegner.GegnerID + ".002";
                    RecordSet.Update(gegner);
                }
                return gegner;
            }
        }

        private tblKlient GetKlient(DataRow r)
        {
            string sql = "SELECT * FROM tblKlient WHERE "+
                                "RTRIM(LTRIM(UPPER(KlientStrasse))) = '" + NormalizeString(r["AG_STRASSE"]).ToUpper() +
                                "' AND RTRIM(LTRIM(UPPER(KlientOrt))) = '" +NormalizeString( r["AG_ORT"]).ToUpper() +
                                "' AND KlientPLZ = '" + NormalizeString(r["AG_PLZ"]) + "'" +
                                " ORDER BY  KlientID DESC";
            return (tblKlient)HTBUtils.GetSqlSingleRecord(sql, typeof(tblKlient));
            
        }

        public void CreateAction(RecordSet set, int aktId, DataRow row, string text)
        {
            tblCustInkAktAktion action = new tblCustInkAktAktion();
            action.CustInkAktAktionAktID = aktId;

            //action.CustInkAktAktionDate = Convert.ToDateTime(NormalizeString(row["ABSCHLUSS"]));
            //if (action.CustInkAktAktionDate.ToShortDateString() == "01.01.1900")
            {
                action.CustInkAktAktionDate = DateTime.Now;
            }
            action.CustInkAktAktionEditDate = action.CustInkAktAktionDate;
            action.CustInkAktAktionMemo = text.Replace("\n", "");
            set.InsertRecord(action);
        }

        private void CreateInvoice(tblCustInkAkt akt, int type, object amt, string desc, bool isTaxable)
        {
            double amount = GetAmount(amt);
            if (amount > 0) {
                int invId = invMgr.CreateAndSaveInvoice(akt.CustInkAktID, type, amount, desc, isTaxable);
                if (invId > 0)
                {
                    tblCustInkAktInvoice inv = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + invId, typeof(tblCustInkAktInvoice));
                    inv.InvoiceDate = akt.CustInkAktEnterDate;
                    //inv.InvoiceLastInterestDate = new DateTime(2011, 5, 13); // change this date every time we import new cases
                    inv.InvoiceLastInterestDate = DateTime.Now; // fuck it! [not a big deal of money]
                    RecordSet.Update(inv);
                }
            }
        }

        private void CreateTax(tblCustInkAkt akt, object amt)
        {
            AktUtils aktUtils = new AktUtils(akt.CustInkAktID);
            double tax = aktUtils.GetAktTotalTax();
            double amount = GetAmount(amt);
            if (amount > 0 && amount != tax)
            {
                double diff = amount - tax;
                tblCustInkAktInvoice inv = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + akt.CustInkAktID + " AND InvoiceTax > 0 ORDER BY InvoiceID DESC", typeof(tblCustInkAktInvoice));
                if (inv != null)
                {
                    inv.InvoiceTax += diff;
                    RecordSet.Update(inv);
                }
            }
        }
        /*
        private void CreateForderung(tblCustInkAkt akt, object amt)
        {
            double amount = GetAmount(amt);
            if (amount > 0)
            {
                RecordSet set = new RecordSet();
                
                tblCustInkAktFord ford = new tblCustInkAktFord();
                ford.CustInkAktFordBetrag = amount;
                ford.CustInkAktFordAkt = akt.CustInkAktID;
                ford.CustInkAktFordCaption = "Kapital - Forderung";
                ford.CustInkAktFordDueDatum = akt.CustInkAktEnterDate;
                ford.CustInkAktFordReDatum = akt.CustInkAktInvoiceDate;
                ford.CustInkAktFordReNr = akt.CustInkAktKunde;
                ford.CustInkAktFordType = 1;
                ford.CustInkAktFordSaldo = ford.CustInkAktFordBetrag;
                
                tblCustInkAktPos pos = new tblCustInkAktPos();
                pos.CustInkAktPosAkt = akt.CustInkAktID;
                pos.CustInkAktPosDate = akt.CustInkAktEnterDate;
                pos.CustInkAktPosCaption = ford.CustInkAktFordCaption;
                pos.CustInkAktPosValue = ford.CustInkAktFordBetrag;
                pos.CustInkAktPosSaldo = ford.CustInkAktFordSaldo;

                set.InsertRecord(ford);
                set.InsertRecord(pos);
            }
        }
        */
        private void CreatePayment(tblCustInkAkt akt, object amt)
        {
            double crrBalance = GetAmount(amt);
            if (crrBalance > 0)
            {
                AktUtils utils = new AktUtils(akt.CustInkAktID);
                double aktTotal = utils.GetAktBalance();
                double payment = aktTotal - crrBalance;
                if (payment > 0)
                {
                    InvoiceManager invMgr = new InvoiceManager();
                    invMgr.CreateAndSavePayment(akt.CustInkAktID, payment);

                    ArrayList list = invMgr.GetOpenedPayments(akt.CustInkAktID);
                    foreach (tblCustInkAktInvoice inv in list)
                        invMgr.ApplyPayment(inv.InvoiceID);
                }
                Console.WriteLine("Payment: " + (aktTotal - crrBalance));
            }
        }

        private void CreatePayment(tblCustInkAkt akt, DataRow row, object klientAmount, object ecpAmount)
        {
            if (akt.CustInkAktAZ == "16072008001")
            {
                Console.Write("");
            }
            double klientAmt = GetAmount(klientAmount);
            double ecpAmt = GetAmount(ecpAmount);
            double total = klientAmt+ecpAmt;
            if (total > 0)
            {
                InvoiceManager invMgr = new InvoiceManager();
                tblCustInkAktInvoice payment = invMgr.CreatePayment(akt.CustInkAktID, GetPaymentType(NormalizeString(row["uakbuchungsart"])), total);

                payment.InvoiceDate = Convert.ToDateTime(row["uakdatum"]);
                payment.InvoiceDueDate = payment.InvoiceDate;
                payment.InvoicePaymentReceivedDate = Convert.ToDateTime(row["uakdatum"]);
                payment.InvoicePaymentTransferToClientDate = Convert.ToDateTime(row["uakklientecpdatum"]);
                payment.InvoicePaymentTransferToClientAmount = klientAmt;
                payment.InvoiceBillNumber = NormalizeString(row["uakbeleg"]);
                
                int payId = invMgr.SaveInvoice(payment);
                if (payId >= 0)
                {
                    invMgr.ApplyPayment(payId, klientAmt, ecpAmt);
                }
                else
                {
                    Console.WriteLine("ERROR: Could not create payment");
                }
                //Thread.Sleep(500);
            }
        }

        private int GetPaymentType(string type)
        {
            switch (type)
            {
                case "UW":
                    return tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_TRANSFER;
                case "BA":
                    return tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_CASH;
                case "DI":
                    return tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_DIRECT_TO_CLIENT;
                default:
                    throw new Exception("Unknown payment type");
            }
        }

        private double GetAmount(object amt)
        {

            double amount = 0;
            try
            {
                amount = Convert.ToDouble(amt.ToString());
            }
            catch
            {
                amount = 0;
            }
            return amount;
        }
        
        private DateTime GetDate(String d)
        {
            if (d.IndexOf(".") >= 0)
            {
                try
                {
                    return Convert.ToDateTime(d);
                }
                catch
                {
                    return new DateTime(1900, 1, 1);
                }
            }
            else
            {
                try
                {
                    return new DateTime(Convert.ToInt16(d.Substring(0, 4)), Convert.ToInt16(d.Substring(4, 2)), Convert.ToInt16(d.Substring(6)));
                }
                catch
                {
                    return new DateTime(1900, 1, 1);
                }
            }
        }

        private void SaveAktText(tblCustInkAkt akt)
        {
            using (StreamWriter outfile = new StreamWriter(@"c:\temp\imports\Akt.txt"))
            {
                outfile.WriteLine("========================================");
                outfile.WriteLine(akt.ToXmlString());
                outfile.WriteLine("\r\n========================================");
                
            }
        }

        private object[] GetActions(string line)
        {
            ArrayList list = new ArrayList();
            int start = 0;
            for (int i = 0; i < line.Length - 8; i++)
            {
                if (i < line.Length - 10)
                {
                    string s = line.Substring(i, 10);
                    if (IsDate(s))
                    {
                        if (line.Substring(i + s.Length, 1) == " ")
                        {
                            if (i > start)
                            {
                                list.Add(line.Substring(start, i - start));
                            }
                            start = i;
                        }
                        else
                        {
                            line = line.Remove(i + s.Length, 1);
                        }
                    }
                    else
                    {
                        s = line.Substring(i, 8);
                        if (IsDate(s))
                        {
                            if (line.Substring(i + s.Length, 1) == " ")
                            {
                                if (i > start)
                                {
                                    list.Add(line.Substring(start, i - start));
                                }
                                start = i;
                            }
                            else
                            {
                                line = line.Remove(i + s.Length, 1);
                            }
                        }
                    }
                }
            }
            list.Add(line.Substring(start));
            string[] actions = new string[list.Count];
            int count = 0;
            foreach (object o in list)
            {
                string waction = o.ToString();
                if (waction.IndexOf(" ") > 0)
                {
                    string wdte = waction.Substring(0, waction.IndexOf(" "));
                    actions[count] = GetDateFromString(wdte).ToShortDateString() + waction.Substring(waction.IndexOf(" "));
                }
                else
                {
                    actions[count] = waction;
                }
                count++;
            }

            return actions;
        }

        private object[] GetActions2(string line)
        {
            ArrayList list = new ArrayList();
            StringBuilder sb = new StringBuilder();

            string[] words = line.Split();
            foreach (string word in words)
            {
                string str = word;
                if (IsDate(str))
                {
                    if (sb.Length > 0)
                    {
                        list.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                }
                else if (str.Length > 1)       // remove the additional character added to the date to indicate it is NOT a new action
                {
                    if (IsDate(word.Substring(0, str.Length - 1)))
                    {
                        str = word.Substring(0, str.Length - 1);
                    }
                }
                sb.Append(str);
                sb.Append(" ");
            }
            list.Add(sb.ToString());
            return list.ToArray();
        }
        private DateTime GetDateFromString(String dte)
        {
            try
            {
                string d, m, y;
                d = dte.Substring(0, 2);
                m = dte.Substring(3, 2);

                if (dte.Length == 8)
                {
                    y = "20" + dte.Substring(6);
                }
                else
                {
                    y = dte.Substring(6);
                }
                return new DateTime(Convert.ToInt16(y), Convert.ToInt16(m), Convert.ToInt16(d));
            }
            catch
            {
                return new DateTime(1900, 1, 1);
            }
        }
        
        private bool IsDate(string str)
        {
            if (str.Length == 8 || str.Length == 10)
            {
                if ((str.Substring(2, 1) == "." || str.Substring(2, 1) == "-") && (str.Substring(5, 1) == "." || str.Substring(5, 1) == "-"))
                {
                    if (IsNumber(str.Substring(0, 2)) &&
                        IsNumber(str.Substring(3, 2)) &&
                        IsNumber(str.Substring(6)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool IsNumber(string str)
        {
            try
            {
                Convert.ToInt16(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private DataTable GetDatabaseTables_SQL()
        {
            
            OleDbConnection conn = null;
            try
            {
                conn = new OleDbConnection(strAktConn);
                conn.Open();
                DataTable schemaTable = conn.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables,
                    new object[] {null, null, null, "TABLE"});
                return schemaTable;
            }
            catch (OleDbException ex)
            {
                Trace.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        private string NormalizeString(object text)
        {
            if (text == null)
                return null;
            else 
                return text.ToString().Replace("'", "").Trim();
        }
        static void Main(string[] args)
        {
            new InkassoAktImporter();
        }
        
    }
}
