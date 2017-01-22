using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using HTB.Database;
using System.Collections;
using HTBUtilities;
using System.Reflection;

namespace HTBInvoiceManager
{
    public class InvoiceAppliedManager
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const double ClientPercent = .7; // 70% of payment goes to the client
//        private long _appliedCount;
        private readonly List<tblCustInkAktInvoice> _appliedInvoicesList = new List<tblCustInkAktInvoice>();
        private readonly List<tblCustInkAktInvoiceApply> _appliedList = new List<tblCustInkAktInvoiceApply>(); 
        #region Apply Invoices
        public double GetAppliedAmount(tblCustInkAktInvoice inv)
        {
            double rett = 0;
            if (inv != null)
            {
                ArrayList recList;
                if (inv.IsPayment())
                    recList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoiceApply WHERE ApplyFromInvoiceId = " + inv.InvoiceID, typeof(tblCustInkAktInvoiceApply));
                else
                    recList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoiceApply WHERE ApplyToInvoiceId = " + inv.InvoiceID, typeof(tblCustInkAktInvoiceApply));

                rett += recList.Cast<tblCustInkAktInvoiceApply>().Sum(record => record.ApplyAmount);
            }
            return rett;
        }

        public double GetAppliedAmount(int invId)
        {
            return GetAppliedAmount(Util.GetInvoiceByID(invId));
        }

        public void ApplyPayment(int invId)
        {
            try
            {
                tblCustInkAktInvoice inv = Util.GetInvoiceByID(invId);

                double clientAmount = Math.Round(inv.InvoiceBalance * ClientPercent, 2);
                double collectionAmount = inv.InvoiceBalance - clientAmount;

                ArrayList openedInvoicesList = Util.GetOpenedInvoices(inv.InvoiceCustInkAktId);
                foreach (tblCustInkAktInvoice toApplyRec in openedInvoicesList)
                {
                    if (!toApplyRec.IsInterest())
                    {
                        if (!toApplyRec.IsClientInvoice())
                        {
                            if (collectionAmount > 0)
                                collectionAmount = ApplyAmount(inv, toApplyRec, collectionAmount);
                        }
                        else
                        {
                            if (clientAmount > 0)
                                clientAmount = ApplyAmount(inv, toApplyRec, clientAmount);
                        }
                    }
                }
                CommitAppliedLists();
                if (clientAmount > 0 || collectionAmount > 0)
                {
                    ApplyOpenedPayment(invId);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while Applying Payment By InvoiceID ", ex);
            }
        }

        public void ApplyPayment(int invId, double clientAmount, double collectionAmount)
        {
            ApplyPayment(invId, ref clientAmount, ref collectionAmount, false);
        }

        private void ApplyPayment(int invId, ref double clientAmount, ref double collectionAmount, bool applyToInterest, bool isRecursiveCall = false)
        {
            try
            {
                Log.Info("Applying Payment");

                tblCustInkAktInvoice inv = Util.GetInvoiceByID(invId);
                ArrayList openedInvoicesList = Util.GetOpenedInvoices(inv.InvoiceCustInkAktId);
                foreach (tblCustInkAktInvoice toApplyRec in openedInvoicesList)
                {
                    if ((!applyToInterest && !toApplyRec.IsInterest()) || (applyToInterest && toApplyRec.IsInterest()))
                    {
                        if (!toApplyRec.IsClientInvoice())
                        {
                            if (collectionAmount > 0)
                                collectionAmount = ApplyAmount(inv, toApplyRec, collectionAmount);
                        }
                        else
                        {
                            if (clientAmount > 0)
                                clientAmount = ApplyAmount(inv, toApplyRec, clientAmount);
                        }
                    }
                }
                CommitAppliedLists();
                if (!isRecursiveCall && (clientAmount > 0 || collectionAmount > 0)) // use isRecursiveCall flag to break infinite loop 'recursive calling'
                {
                    Log.Info("Applying Payment... Calling Recursive [isRecursiveCall: false] [clientAmount: " + clientAmount + "] [collectionAmount: " + collectionAmount + "]");
                    ApplyPayment(invId, ref clientAmount, ref collectionAmount, true, true);
                }
                if (clientAmount > 0 || collectionAmount > 0)
                {
                    Log.Info("Applying Payment... Calling ApplyOpenedPayment [isRecursiveCall: " + isRecursiveCall + "] [clientAmount: " + clientAmount + "] [collectionAmount: " + collectionAmount + "]");
                    ApplyOpenedPayment(invId);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while Applying Payment ", ex);
            }
        }
        /*
        private void ApplyPayment(int invId, ref double clientAmount, ref double collectionAmount, bool applyToInterest, bool isRecursiveCall = false)
        {
            try
            {
                Log.Info("Applying Payment");
                tblCustInkAktInvoice inv = Util.GetInvoiceByID(invId);
                ArrayList openedInvoicesList = Util.GetOpenedInvoices(inv.InvoiceCustInkAktId);
                foreach (tblCustInkAktInvoice toApplyRec in openedInvoicesList)
                {
                    if ((!applyToInterest && !toApplyRec.IsInterest()) || (applyToInterest && toApplyRec.IsInterest()))
                    {
                        if (!toApplyRec.IsClientInvoice())
                        {
                            if (collectionAmount > 0)
                                collectionAmount = ApplyAmount(inv, toApplyRec, collectionAmount);
                        }
                        else
                        {
                            if (clientAmount > 0)
                                clientAmount = ApplyAmount(inv, toApplyRec, clientAmount);
                        }
                    }
                }
                if (!isRecursiveCall && (clientAmount > 0 || collectionAmount > 0)) // use isRecursiveCall flag to break infinite loop 'recursive calling'
                {
                    Log.Info("Applying Payment... Calling Recursive [isRecursiveCall: false] [clientAmount: " + clientAmount + "] [collectionAmount: " + collectionAmount + "]");
                    ApplyPayment(invId, ref clientAmount, ref collectionAmount, true, true);
                }
                if (clientAmount > 0 || collectionAmount > 0)
                {
                    Log.Info("Applying Payment... Calling ApplyOpenedPayment [isRecursiveCall: " + isRecursiveCall + "] [clientAmount: " + clientAmount + "] [collectionAmount: " + collectionAmount + "]");
                    ApplyOpenedPayment(invId);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while Applying Payment ", ex);
            }
        }
         */
        private void ApplyOpenedPayment(int invId)
        {
            try
            {
                Log.Info("Paying opened invoices " + invId);
                tblCustInkAktInvoice inv = Util.GetInvoiceByID(invId);

                double amount = Math.Round(inv.InvoiceBalance, 2);
                ArrayList openedInvoicesList = Util.GetOpenedInvoices(inv.InvoiceCustInkAktId);
                foreach (tblCustInkAktInvoice toApplyRec in openedInvoicesList)
                {
                    amount = ApplyAmount(inv, toApplyRec, amount);
                }
                CommitAppliedLists();
            }
            catch (Exception ex)
            {
                Log.Error("Error while Applying Payment ", ex);
            }
        }
        
        private double ApplyAmount(tblCustInkAktInvoice from, tblCustInkAktInvoice to, double amount)
        {
            if (!HTBUtils.IsZero(amount))
            {
                Log.Info("Applying Amount [from: " + from.InvoiceID + "] [to: " + to.InvoiceID + "] [amount: " + amount + "]");
                double tmpBalance = Math.Round(to.InvoiceBalance, 2);
                to.InvoiceBalance = Math.Round(to.InvoiceBalance, 2);
                to.InvoiceBalance -= amount;
                amount = 0;
                if (to.InvoiceBalance < .005)
                {
                    amount = to.InvoiceBalance*-1;
                    if (amount < .005)
                    {
                        amount = 0;
                    }
                    to.InvoiceBalance = 0;
                }
                to.InvoiceStatus = HTBUtils.IsZero(to.InvoiceBalance) ? 9 : 1;
                from.InvoiceBalance -= Math.Round(tmpBalance - to.InvoiceBalance, 2);
                if (from.InvoiceBalance < .005)
                    from.InvoiceBalance = 0;
                if (from.InvoiceBalance < 0) // this better never happen
                    throw new Exception("Payment Balance less than Zero [INV: " + from.InvoiceID + "] [From Amount: " + from.InvoiceBalance + "]");
                
                _appliedList.Add(new tblCustInkAktInvoiceApply
                                                      {
                                                          ApplyFromInvoiceId = from.InvoiceID,
                                                          ApplyToInvoiceId = to.InvoiceID,
                                                          ApplyAmount = Math.Round(tmpBalance - to.InvoiceBalance, 2),
                                                          ApplyDate = DateTime.Now
                                                      });
                 _appliedInvoicesList.Add(from);
                 _appliedInvoicesList.Add(to);
              
                return amount;
            }
            return 0;
        }
        
        private bool CommitAppliedLists()
        {
            var set = new RecordSet();
            set.StartTransaction();
            try
            {
                foreach (var applied in _appliedList)
                {
                    set.InsertRecordInTransaction(applied);
                }
                foreach (var inv in _appliedInvoicesList)
                {
                    set.UpdateRecordInTransaction(inv);
                }
                set.CommitTransaction();
                _appliedInvoicesList.Clear();
                _appliedList.Clear();
                Log.Info("Applied Commited Successfully");
                return true;
            }
            catch (Exception ex)
            {
                set.RollbackTransaction();
                _appliedInvoicesList.Clear();
                _appliedList.Clear();
                Log.Error("Amount was not applied [COMMITED] ", ex);
                throw;
            }
        }
        /*
        private double ApplyAmount(tblCustInkAktInvoice from, tblCustInkAktInvoice to, double amount)
        {
            if (!HTBUtils.IsZero(amount))
            {
                Log.Info("Applying Amount [from: " + from.InvoiceID + "] [to: " + to.InvoiceID + "] [amount: " + amount + "]");
                double tmpBalance = Math.Round(to.InvoiceBalance, 2);
                to.InvoiceBalance = Math.Round(to.InvoiceBalance, 2);
                to.InvoiceBalance -= amount;
                amount = 0;
                if (to.InvoiceBalance < .005)
                {
                    amount = to.InvoiceBalance*-1;
                    if (amount < .005)
                    {
                        amount = 0;
                    }
                    to.InvoiceBalance = 0;
                }
                to.InvoiceStatus = HTBUtils.IsZero(to.InvoiceBalance) ? 9 : 1;
                from.InvoiceBalance -= Math.Round(tmpBalance - to.InvoiceBalance, 2);
                if (from.InvoiceBalance < .005)
                    from.InvoiceBalance = 0;
                if (from.InvoiceBalance < 0) // this better never happen
                    throw new Exception("Payment Balance less than Zero [INV: " + from.InvoiceID + "] [From Amount: " + from.InvoiceBalance + "]");
                var set = new RecordSet();
                set.StartTransaction();
                try
                {
//                    CreateAndSaveApplyRecord(from.InvoiceID, to.InvoiceID, Math.Round(tmpBalance - to.InvoiceBalance, 2), set, true);
                    set.InsertRecordInTransaction(new tblCustInkAktInvoiceApply
                                                      {
                                                          ApplyFromInvoiceId = from.InvoiceID,
                                                          ApplyToInvoiceId = to.InvoiceID,
                                                          ApplyAmount = Math.Round(tmpBalance - to.InvoiceBalance, 2),
                                                          ApplyDate = DateTime.Now
                                                      });
                    set.UpdateRecordInTransaction(from);
                    set.UpdateRecordInTransaction(to);
                    set.CommitTransaction();
                    Log.Info("Amount Applied Successfully");
                }
                catch (Exception ex)
                {
                    set.RollbackTransaction();
                    Log.Error("Amount was not applied ", ex);
                    throw ex;
                }
                if (_appliedCount++ % 20 == 0)
                {
                    try
                    {
                        Thread.Sleep(1000); // allow time for db to recover
                    }
                    catch
                    {
                    }
                }
                if (_appliedCount == 100000)
                    _appliedCount = 0;// reset counter so that it does not grow too much
                    
                return amount;
            }
            return 0;
        }
         */
        private static void CreateAndSaveApplyRecord(int fromId, int toId, double amount, RecordSet set = null, bool useTransaction = false)
        {
            try
            {
                Log.Info("CreateAndSaveApplyRecord ");
                if (amount > 0)
                {

                    var rec = new tblCustInkAktInvoiceApply
                                  {
                                      ApplyFromInvoiceId = fromId,
                                      ApplyToInvoiceId = toId,
                                      ApplyAmount = amount,
                                      ApplyDate = DateTime.Now
                                  };
                    if (set == null)
                    {
                        set = new RecordSet();
                        useTransaction = false;
                    }
                    if (useTransaction)
                        set.InsertRecordInTransaction(rec);
                    else
                        set.InsertRecord(rec);
                }
                Log.Info("CreateAndSaveApplyRecord: SUCCESS");
            }
            catch (Exception ex)
            {
                Log.Error("Error CreateAndSaveApplyRecord ", ex);
            }
        }
        #endregion

        #region Unapply Invoices
        /*
        public bool UnapplyInvoiceAll(tblCustInkAktInvoice inv)
        {
            bool ok = true;
            if (inv != null)
            {
//                Log.Info("UnapplyingAll: " + inv.InvoiceID);
                Thread.Sleep(100);
                ArrayList recList = null;
                if (inv.IsPayment())
                    recList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoiceApply WHERE ApplyFromInvoiceId = " + inv.InvoiceID, typeof(tblCustInkAktInvoiceApply));
                else
                    recList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoiceApply WHERE ApplyToInvoiceId = " + inv.InvoiceID, typeof(tblCustInkAktInvoiceApply));
                Log.Info("[recList count: " + recList.Count+"] [inv.IsPayment: "+inv.IsPayment()+"]");
//                Thread.Sleep(100);
//                int counter = 0;
                foreach (tblCustInkAktInvoiceApply applied in recList)
                {
//                    Log.Info("in loop ");
                    Thread.Sleep(100);
                    tblCustInkAktInvoice invoice;
                    tblCustInkAktInvoice cash;
                    if (inv.IsPayment())
                    {
//                        Log.Info("Getting Invoice Record [ID: "+applied.ApplyToInvoiceId+"]");
                        Thread.Sleep(100);
                        invoice = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceId = " + applied.ApplyToInvoiceId, typeof(tblCustInkAktInvoice));
//                        invoice = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceId = " + applied.ApplyToInvoiceId, typeof(tblCustInkAktInvoice), DbConnection.ConnectionType_SqlServer, true);
//                        Log.Info("GOT Invoice [ID: " + applied.ApplyToInvoiceId + "]");
//                        Thread.Sleep(100);
                        cash = inv;
                    }
                    else
                    {
//                        Log.Info("Getting Cash Record [ID: " + applied.ApplyFromInvoiceId + "]");
//                        Thread.Sleep(100);
                        cash = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceId = " + applied.ApplyFromInvoiceId, typeof(tblCustInkAktInvoice));
                        invoice = inv;
                    }
//                    Log.Info("Ready to call UnapplyInvoice");
                    ok &= UnapplyInvoice(invoice, cash, applied);
//                    if(ok)
//                        Log.Info("SUCCESS!");
//                    if(counter++ % 1000 == 0)
//                    {
//                        try
//                        {
//                            const int sleepTime = 3000;
//                            Log.Info("Sleeping: "+sleepTime);
//                            Thread.Sleep(sleepTime); // give db time to recover
//                        }
//                        catch
//                        {
//                        }
//                    }
//                    Log.Info("Processed " + counter+" Of " + recList.Count);
                }
            }
            return ok;
        }

        
        private bool UnapplyInvoice(tblCustInkAktInvoice inv, tblCustInkAktInvoice cash, tblCustInkAktInvoiceApply applied)
        {
            Log.Info("Unapplying [inv: " + inv.InvoiceID + "] [cash: " + cash.InvoiceID + "] [amount: " + applied.ApplyAmount + "]");
            Thread.Sleep(100);
            inv.InvoiceBalance += applied.ApplyAmount;
            cash.InvoiceBalance += applied.ApplyAmount;
            var set = new RecordSet();
            set.StartTransaction();
            try
            {
                set.UpdateRecordInTransaction(inv);
                set.UpdateRecordInTransaction(cash);
                set.DeleteRecordInTransaction(applied);
                set.CommitTransaction();
                Log.Info("SUCCESS!");
                return true;
            }
            catch (Exception ex)
            {
                set.RollbackTransaction();
                Log.Info("Error while unaplying: "+ex);
                return false;
            }
        }
         */
        public bool UnapplyInvoiceAll(tblCustInkAktInvoice inv)
        {
            bool ok = true;
            if (inv != null)
            {

                int error = 0;
                var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("invoiceId", SqlDbType.Int, inv.InvoiceID),
                                     new StoredProcedureParameter("error", SqlDbType.Int, error, ParameterDirection.Output)
                                 };
                HTBUtils.GetStoredProcedureRecords("spUnapplyInvoiceAll", parameters, typeof(SingleValue));
                foreach (object o in parameters)
                {
                    if (o is ArrayList)
                    {
                        var outputList = (ArrayList)o;
                        foreach (StoredProcedureParameter p in outputList)
                        {
                            if (p.Name.IndexOf("error") >= 0)
                            {
                                try
                                {
                                    error = Convert.ToInt32(p.Value);
                                }
                                catch (Exception ex)
                                {
                                    Log.Info("ERROR: ", ex);
                                }
                            }
                        }
                    }
                }
                ok = error == 0;
            }
            return ok;
        }

        private bool UnapplyInvoice(tblCustInkAktInvoice inv, tblCustInkAktInvoice cash, tblCustInkAktInvoiceApply applied)
        {
            Log.Info("Unapplying [inv: " + inv.InvoiceID + "] [cash: " + cash.InvoiceID + "] [amount: " + applied.ApplyAmount + "]");
            int error = 0;
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("invoiceId", SqlDbType.Int, inv.InvoiceID),
                                     new StoredProcedureParameter("paymentId", SqlDbType.Int, cash.InvoiceID),
                                     new StoredProcedureParameter("applyId", SqlDbType.Int, applied.ApplyId),
                                     new StoredProcedureParameter("error", SqlDbType.Int, error, ParameterDirection.Output)
                                 };
            HTBUtils.GetStoredProcedureRecords("spUnapplyInvoice", parameters, typeof(SingleValue));
            foreach (object o in parameters)
            {
                if (o is ArrayList)
                {
                    var outputList = (ArrayList)o;
                    foreach (StoredProcedureParameter p in outputList)
                    {
                        if (p.Name.IndexOf("error") >= 0)
                        {
                            try
                            {
                                error = Convert.ToInt32(p.Value);
                            }
                            catch(Exception ex)
                            {
                                Log.Info("ERROR: ", ex);
                            }
                        }
                    }
                }
            }
            return error == 0;
        }
        #endregion

        #region Apply Installments
        public double ApplyInstallmentAmount(tblCustInkAktInvoice from, tblCustInkAktRate to, double amount)
        {
            double tmpBalance = Math.Round(to.CustInkAktRateBalance, 2);
            to.CustInkAktRateBalance = Math.Round(to.CustInkAktRateBalance, 2);
            to.CustInkAktRateBalance -= amount;
            to.CustInkAktRateReceivedAmount += amount;

            amount = 0;
            if (to.CustInkAktRateBalance < .005)
            {
                amount = to.CustInkAktRateBalance * -1;
                if (amount < .005)
                {
                    amount = 0;
                }
                to.CustInkAktRateBalance = 0;
            }
            from.InvoiceBalance -= Math.Round(tmpBalance - to.CustInkAktRateBalance, 2);
            if (from.InvoiceBalance < .005)
                from.InvoiceBalance = 0;
            if (from.InvoiceBalance < 0) // this better never happen
                throw new Exception("Payment Balance less than Zero [INV: " + from.InvoiceID + "]");
            var set = new RecordSet();
            set.StartTransaction();
            try
            {
                CreateAndSaveInstallmentApplyRecord(from.InvoiceID, to.CustInkAktRateID, Math.Round(tmpBalance - to.CustInkAktRateBalance, 2), set, true);
                set.UpdateRecordInTransaction(to);
                set.CommitTransaction();
            }
            catch (Exception)
            {
                set.RollbackTransaction();
                throw;
            }
            return amount;
        }
        public void CreateAndSaveInstallmentApplyRecord(int fromId, int toId, double amount, RecordSet set = null, bool useTransaction = false)
        {
            if (amount > 0)
            {

                var rec = new tblCustInkAktRateApply
                              {
                                  RateApplyInvoiceID = fromId,
                                  RateApplyRateID = toId,
                                  RateApplyAmount = amount,
                                  RateApplyDate = DateTime.Now
                              };
                if (set == null)
                {
                    set = new RecordSet();
                    useTransaction = false;
                }
                if (useTransaction)
                    set.InsertRecordInTransaction(rec);
                else
                    set.InsertRecord(rec);
            }
        }
        #endregion

        #region Unapply Installments
        public bool UnapplyAllInstallments(tblCustInkAktInvoice payment)
        {
            bool ok = true;
            if (payment != null)
            {
                if (payment.IsPayment())
                {
                    ArrayList recList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktRateApply WHERE RateApplyInvoiceID = " + payment.InvoiceID, typeof(tblCustInkAktRateApply));

                    foreach (tblCustInkAktRateApply applied in recList)
                    {
                        var rate = (tblCustInkAktRate)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktRate WHERE CustInkAktRateID = " + applied.RateApplyRateID, typeof(tblCustInkAktRate));
                        ok &= UnapplyInstallments(rate, applied);
                    }
                }
            }
            return ok;
        }

        private bool UnapplyInstallments(tblCustInkAktRate installment, tblCustInkAktRateApply applied)
        {
            int error = 0;
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("installmentId", SqlDbType.Int, installment.CustInkAktRateID),
                                     new StoredProcedureParameter("applyId", SqlDbType.Int, applied.RateApplyID),
                                     new StoredProcedureParameter("error", SqlDbType.Int, error, ParameterDirection.Output)
                                 };
            HTBUtils.GetStoredProcedureRecords("spUnapplyInstallment", parameters, typeof(SingleValue));
            foreach (object o in parameters)
            {
                if (o is ArrayList)
                {
                    var outputList = (ArrayList)o;
                    foreach (StoredProcedureParameter p in outputList)
                    {
                        if (p.Name.IndexOf("error") >= 0)
                        {
                            try
                            {
                                error = Convert.ToInt32(p.Value);
                            }
                            catch (Exception ex)
                            {
                                Log.Info("ERROR: ", ex);
                            }
                        }
                    }
                }
            }
            return error == 0;
        }

        /*
        private bool UnapplyInstallments(tblCustInkAktRate installment, tblCustInkAktRateApply applied)
        {
            installment.CustInkAktRateBalance += applied.RateApplyAmount;
            installment.CustInkAktRateReceivedAmount -= applied.RateApplyAmount;


            var set = new RecordSet();
            set.StartTransaction();
            try
            {
                set.UpdateRecordInTransaction(installment);
                set.DeleteRecordInTransaction(applied);
                set.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                set.RollbackTransaction();
                return false;
            }
        }
         */
        #endregion

    }
}
