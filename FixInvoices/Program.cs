using System;
using System.Collections;
using HTB.Database;
using HTB.Database.Views;
using HTB.Globals;
using HTBInvoiceManager;
using HTBUtilities;

namespace FixInvoices
{
    class Program
    {
        private readonly InvoiceManager _invMgr = new InvoiceManager();
       

        static void Main(string[] args)
        {
            new Program();
            Console.ReadLine();
        }
        private Program()
        {
//            string qry = "SELECT * FROM qryCustInkAkt WHERE CustInkAktIsPartial = 0 AND CustInkAktEnterDate > '01.07.2011'";
            string qry = "SELECT * FROM qryCustInkAkt WHERE CustInkAktID in (31750, 31751, 31753, 31749, 31779)";
            ArrayList custInAktProcessList = HTBUtils.GetSqlRecords(qry, typeof(tblCustInkAkt));
            Console.WriteLine(qry);
            foreach (tblCustInkAkt custInkAkt in custInAktProcessList)
                CalculateKosten(custInkAkt.CustInkAktID);
        }

        private void CalculateKosten(int aktId)
        {
            tblCustInkAkt akt = HTBUtils.GetInkassoAkt(aktId);
            
            var inv = _invMgr.GetInvoiceByAktAndType(aktId, tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL);
            decimal kapital = inv.InvoiceAmount > 0 ? Convert.ToDecimal(inv.InvoiceAmount) : 0;
            
            inv = _invMgr.GetInvoiceByAktAndType(aktId, tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST);
            decimal klientKosten = inv != null && inv.InvoiceAmount > 0 ? Convert.ToDecimal(inv.InvoiceAmount) : 0;

            decimal forderung = kapital + klientKosten;
            
            var list = LoadInitialKostenBasedOnForderung(forderung);
            foreach (qryKosten record in list)
            {
                double amount = Convert.ToDouble(HTBUtils.GetCalculatedKost(forderung, record, akt.CustInkAktInvoiceDate)); // calc based on balance
                inv = _invMgr.GetInvoiceByAktAndType(aktId, record.KostenInvoiceType);
                if (inv != null && Math.Abs(inv.InvoiceAmountNetto - amount) > 2)
                {
                    
                    Console.WriteLine(aktId + ";" + akt.CustInkAktStatus + ";" + record.KostenArtText.Trim() + ";" + inv.InvoiceAmountNetto + ";" + amount.ToString());
                    double appliedAmount = inv.InvoiceAmount - inv.InvoiceBalance;

                    inv.InvoiceAmountNetto = amount;

                    inv.InvoiceTax = Math.Round(amount*Globals.TAX_RATE, 2);
                    inv.InvoiceAmount = inv.InvoiceTax + amount;
                    if(HTBUtils.IsZero(appliedAmount))
                    {
                        inv.InvoiceBalance = inv.InvoiceAmount;
                    }
                    else
                    {
                        inv.InvoiceBalance = inv.InvoiceAmount - appliedAmount;
                        if (HTBUtils.IsZero(inv.InvoiceBalance) || inv.InvoiceBalance < 0)
                        {
                            inv.InvoiceBalance = 0;
                        }
                    }
                    RecordSet.Update(inv);
                }
            }
        }

        public ArrayList LoadInitialKostenBasedOnForderung(decimal forderung)
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM qryKosten WHERE ((IsImErstenSchritt = 1 AND IsZinsen = 0) OR (KostenInvoiceType in (201, 202, 203)) )AND Von <= " + GetNormalizedDbAmount(forderung) + " and bis >= " + GetNormalizedDbAmount(forderung), typeof(qryKosten));
        }

        private void CreateInvoiceRecord(int aktID, qryKosten record, decimal amount)
        {
            int invType = tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE; // collection 'inkasso' cost
            if (record.KostenInvoiceType > 0)
            {
                invType = record.KostenInvoiceType;
            }
            //CreateAndSaveInvoice(aktID, invType, Convert.ToDouble(amount), record.KostenArtText, true);
        }

        public tblCustInkAktInvoice CreateInvoice(int aktId, int type, double amount, string description, string comment, bool isTaxable)
        {
            var inv = new tblCustInkAktInvoice
                          {
                              InvoiceAmountNetto = amount
                          };
            if (isTaxable)
            {
                inv.InvoiceTax = Math.Round(amount * Globals.TAX_RATE, 2);
                inv.InvoiceAmount = inv.InvoiceTax + amount;    
            }
            else
            {
                inv.InvoiceTax = 0;
                inv.InvoiceAmount = amount;
            }
            inv.InvoiceBalance = inv.InvoiceAmount;
            inv.InvoiceType = type;
            inv.InvoiceCustInkAktId = aktId;
            inv.InvoiceDate = DateTime.Now;
            inv.InvoiceDueDate = inv.InvoiceDate;
            inv.InvoiceLastInterestDate = inv.InvoiceDate;
            inv.InvoiceDescription = description;
            inv.InvoiceComment = comment;
            inv.InvoicePaymentReceivedDate = new DateTime(1900, 01, 01);
            inv.InvoicePaymentTransferToClientDate = new DateTime(1900, 01, 01);
            inv.InvoiceClientDate = new DateTime(1900, 01, 01);
            return inv;
        }

        
        private ArrayList GetAktAktionKosten(int aktId)
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM qryAktAktionKosten WHERE WFPPosition = 1 AND WFPAkt = " + aktId, typeof(qryAktAktionKosten));
        }

        private string GetNormalizedDbAmount(decimal amt)
        {
            return amt.ToString().Replace(",", ".");
        }
    }
}
