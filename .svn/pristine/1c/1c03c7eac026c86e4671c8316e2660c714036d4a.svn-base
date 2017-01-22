using System;
using System.Reflection;
using System.Text;
using HTB.Database;
using HTB.Globals;
using System.Collections;
using HTBUtilities;
using HTB.Database.Views;

namespace HTBInvoiceManager
{
    public class InvoiceManager
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly InvoiceAppliedManager _appliedMgr = new InvoiceAppliedManager();
        private static DateTime _lastTimeInterestRan = new DateTime(1900, 1, 1);

        // Generate Interest (zinsen)
        public void GenerateInterest()
        {
            if (DateTime.Now.Subtract(_lastTimeInterestRan).TotalHours >= 10) // run this thing every 10 hours
            {
                Log.Info("Generating Interest");
                _lastTimeInterestRan = DateTime.Now;
                var set = new RecordSet();
                var qryKostenSet = new qryKostenSet();
                qryKostenSet.LoadZinsen();
                foreach (qryKosten record in qryKostenSet.qryKostenList)
                {
                    decimal amount;
                    if (!record.IsInkassoZinsen)
                    {
                        ArrayList aeInvoiceList = Util.GetOpenedKlientInvoices();
                        foreach (tblCustInkAktInvoice inv in aeInvoiceList)
                        {
                            amount = HTBUtils.GetCalculatedInterest(Convert.ToDecimal(inv.InvoiceBalance), record.KostenPct, inv.InvoiceLastInterestDate);
                            if (amount > 0)
                            {
                                tblCustInkAktInvoice zinsInv = CreateInvoice(inv.InvoiceCustInkAktId, tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_CLIENT, Convert.ToDouble(amount), "Zinsen: " + inv.InvoiceDescription.Replace("Zinsen: Zinsen: ", "Zinsen: "), false); // non taxable
                                if (zinsInv.InvoiceAmount > .05)
                                {
                                    zinsInv.InvoiceInterestID = inv.InvoiceID;
                                    SaveInvoice(zinsInv);

                                    inv.InvoiceLastInterestDate = DateTime.Now;
                                    set.UpdateRecord(inv);
                                }
                            }
                        }
                    }
                    else
                    {
                        ArrayList aeInvoiceList = Util.GetOpenedCollectionInvoices();
                        foreach (tblCustInkAktInvoice inv in aeInvoiceList)
                        {
                            amount = HTBUtils.GetCalculatedInterest(Convert.ToDecimal(inv.InvoiceBalance), record.KostenPct, inv.InvoiceLastInterestDate);
                            if (amount > 0)
                            {
                                tblCustInkAktInvoice zinsInv = CreateInvoice(inv.InvoiceCustInkAktId, tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_COLECTION, Convert.ToDouble(amount), "Zinsen: " + inv.InvoiceDescription.Replace("Zinsen: Zinsen: ", "Zinsen: "), false); // non taxable
                                if (zinsInv.InvoiceAmount > 0.5)
                                {
                                    zinsInv.InvoiceInterestID = inv.InvoiceID;
                                    SaveInvoice(zinsInv);

                                    inv.InvoiceLastInterestDate = DateTime.Now;
                                    set.UpdateRecord(inv);
                                }
                            }
                        }
                    }
                    //Console.WriteLine(record.KostenArtText.Trim() + "\t\t" + amount);
                }
            }
            else
            {
                Log.Info("Skipping interest generation");
            }
        }

        public tblCustInkAktInvoice CreateInvoice(int aktId, int type, double amount, string description, bool isTaxable)
        {
            return CreateInvoice(aktId, type, amount, description, "", isTaxable);
        }
        public tblCustInkAktInvoice CreateInvoice(int aktId, int type, double amount, string description, string comment,bool isTaxable)
        {
            return CreateInvoice(aktId, type, amount, description, comment, isTaxable, new DateTime(1900, 01, 01));
        }
        public tblCustInkAktInvoice CreateInvoice(int aktId, int type, double amount, string description, string comment, bool isTaxable, DateTime paymentReceivedDate)
        {
            var now = DateTime.Now;
            var inv = new tblCustInkAktInvoice
                {
                    InvoiceAmountNetto = amount,
                    InvoiceType = type,
                    InvoiceCustInkAktId = aktId,
                    InvoiceDate = now,
                    InvoiceDueDate = now,
                    InvoiceLastInterestDate = now,
                    InvoiceDescription = description,
                    InvoiceComment = comment,
                    InvoicePaymentReceivedDate = paymentReceivedDate,
                    InvoicePaymentTransferToClientDate = new DateTime(1900, 01, 01),
                    InvoiceClientDate = new DateTime(1900, 01, 01),
                    InvoiceTax = 0,
                    InvoiceAmount = amount
                };
            if (isTaxable)
            {
                inv.InvoiceTax = Math.Round(amount * Globals.TAX_RATE, 2);
                inv.InvoiceAmount = inv.InvoiceTax + amount;
            }
            inv.InvoiceBalance = inv.InvoiceAmount;
            return inv;
        }

        #region Payments
        public int CreateAndSavePayment(int aktId, double amount)
        {
            return CreateAndSavePayment(aktId, tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_CASH, amount);
        }
        public int CreateAndSavePayment(int aktId, int invoiceType, double amount)
        {
            return CreateAndSavePayment(aktId, invoiceType, amount, new DateTime(1900, 1, 1));
        }
        public int CreateAndSavePayment(int aktId, int invoiceType, double amount, DateTime paymentReceivedDate)
        {
            return CreateAndSaveInvoice(aktId, invoiceType, amount, "Zahlung", false, paymentReceivedDate);
        }
        public tblCustInkAktInvoice CreatePayment(int aktId, int invoiceType, double amount)
        {
            return CreateInvoice(aktId, invoiceType, amount, "Zahlung", false);
        }
        #endregion

        public int CreateAndSaveInvoice(int aktId, int type, double amount, string description, bool isTaxable)
        {
            return CreateAndSaveInvoice(aktId, type, amount, description, isTaxable, new DateTime(1900, 1, 1));
        }
        public int CreateAndSaveInvoice(int aktId, int type, double amount, string description, bool isTaxable, DateTime paymentReceivedDate)
        {
            return CreateAndSaveInvoice(aktId, type, amount, description, "", isTaxable, paymentReceivedDate);
        }
        public int CreateAndSaveInvoice(int aktId, int type, double amount, string description, string comment, bool isTaxable)
        {
            return CreateAndSaveInvoice(aktId, type, amount, description, comment, isTaxable, new DateTime(1900, 1, 1));
        }
        public int CreateAndSaveInvoice(int aktId, int type, double amount, string description, string comment, bool isTaxable, DateTime paymentReceivedDate)
        {
            return SaveInvoice(CreateInvoice(aktId, type, amount, description, comment, isTaxable, paymentReceivedDate));
        }

        
        #region Save
        public int SaveInvoice(tblCustInkAktInvoice inv)
        {
            var set = new RecordSet();
            inv.InvoiceLastUpdated = DateTime.Now;
            set.InsertRecord(inv);
            set.LoadRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + inv.InvoiceCustInkAktId + 
                " AND InvoiceAmount = " + inv.InvoiceAmount.ToString().Replace(",", ".") + 
                " AND InvoiceLastUpdated = '" + inv.InvoiceLastUpdated +"'"+
                " AND InvoiceType = " + inv.InvoiceType, 
                typeof(tblCustInkAktInvoice));

            if (set.RecordsList.Count > 0)
            {
                return ((tblCustInkAktInvoice)set.RecordsList[0]).InvoiceID;
            }
            else
            {
                return -1;
            }
        }

        public void SaveForderungInvoice(tblCustInkAktInvoice inv)
        {
            var set = new RecordSet();
            tblCustInkAktInvoice tmp = GetInvoiceByAktAndFordID(inv.InvoiceCustInkAktId, inv.InvoiceCustInkAktFordID);
            if (tmp == null)
            {
                inv.InvoiceLastUpdated = DateTime.Now;
                set.InsertRecord(inv);
            }
            else
            {
                inv.InvoiceID = tmp.InvoiceID;
                inv.InvoiceLastUpdated = DateTime.Now;
                set.UpdateRecord(inv);
            }
        }

        public void SaveKlientKostenInvoice(tblCustInkAktInvoice inv)
        {
            RecordSet set = new RecordSet();
            tblCustInkAktInvoice tmp = GetInvoiceByAktAndKostenKlientID(inv.InvoiceCustInkAktId, inv.InvoiceCustInkAktKostenKlientID);
            if (tmp == null)
            {
                inv.InvoiceLastUpdated = DateTime.Now;
                set.InsertRecord(inv);
            }
            else
            {
                inv.InvoiceID = tmp.InvoiceID;
                inv.InvoiceLastUpdated = DateTime.Now;
                set.UpdateRecord(inv);
            }
        }
        #endregion

        public bool ExistInvoiceForForderung(int aktId, int fordId, decimal amount)
        {
            return ExistInvoiceForForderung(aktId, fordId, Convert.ToDouble(amount));
        }

        public bool ExistInvoiceForForderung(int aktId, int fordId, double amount)
        {
            ArrayList list = Util.GetInvoiceListByAktAndFordID(aktId, fordId);
            foreach (tblCustInkAktInvoice inv in list)
                if (inv.InvoiceAmount == amount)
                    return true;

            return false;
        }
        
        #region Get Invoice
        public tblCustInkAktInvoice GetInvoiceByAktAndKostenKlientID(int aktId, int kkId)
        {
            return Util.GetInvoiceByAktAndKostenKlientID(aktId, kkId);
        }
        public tblCustInkAktInvoice GetInvoiceByAktAndFordID(int aktId, int fordId)
        {
            return Util.GetInvoiceByAktAndFordID(aktId, fordId);
        }
        public tblCustInkAktInvoice GetInvoiceByAktAndType(int aktId, int type)
        {
            return Util.GetInvoiceByAktAndType(aktId, type);
        }
        #endregion

        #region Apply Invoices
        public double GetAppliedAmount(int invId)
        {
            return _appliedMgr.GetAppliedAmount(invId);
        }
        public double GetAppliedAmount(tblCustInkAktInvoice inv)
        {
            return _appliedMgr.GetAppliedAmount(inv);
        }
        public void ApplyPayment(int invId)
        {
            _appliedMgr.ApplyPayment(invId);
        }
        public void ApplyPayment(int invId, double clientAmount, double collectionAmount)
        {
            _appliedMgr.ApplyPayment(invId, clientAmount, collectionAmount);
        }
        public ArrayList GetOpenedPayments(int aktId)
        {
            return Util.GetOpenedPayments(aktId);
        }
        public bool UnapplyInvoiceAll(tblCustInkAktInvoice inv)
        {
            return _appliedMgr.UnapplyInvoiceAll(inv);
        }
        #endregion

        #region Apply Installments
        public double ApplyInstallmentAmount(tblCustInkAktInvoice from, tblCustInkAktRate to, double amount)
        {
            return _appliedMgr.ApplyInstallmentAmount(from, to, amount);
        }
        public bool UnapplyAllInstallments(tblCustInkAktInvoice payment)
        {
            return _appliedMgr.UnapplyAllInstallments(payment);
        }
        #endregion

        #region Delete
        public int DeleteCollectionInvoicesForAct(int aktId, bool includeMelde)
        {
            StringBuilder sb = new StringBuilder("DELETE FROM tblCustInkAktInvoice WHERE  InvoiceCustInkAktId = ");
            sb.Append(aktId);
            sb.Append(" AND InvoiceType IN (");
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE);
            sb.Append(", ");
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_COLECTION);
            sb.Append(", ");
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_BEARBEIT);
            sb.Append(", ");
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_1);
            sb.Append(", ");
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_2);
            sb.Append(", "); 
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_3);
            sb.Append(", "); 
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_4);
            sb.Append(", "); 
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_5);
            sb.Append(", "); 
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_TELEFON);
            sb.Append(", "); 
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_EVIDENZ);
            sb.Append(", "); 
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_PERSONAL_INTERVENTION);
            sb.Append(", "); 
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_RATE_ANGEBOT);
            sb.Append(", ");
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MAIL_FEE);
            sb.Append(", ");
            sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_OVERDUE_CHARGE);           
            if (includeMelde)
            {
                sb.Append(", ");
                sb.Append(tblCustInkAktInvoice.INVOICE_TYPE_COLLECTION_INVOICE_MELDE);
            }
            sb.Append(")");

            RecordSet set = new RecordSet();
            return set.ExecuteNonQuery(sb.ToString());
        }
        #endregion

    }
}
