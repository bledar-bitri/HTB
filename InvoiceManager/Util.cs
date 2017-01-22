using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using HTB.Database;
using HTB.Globals;
using HTBUtilities;

namespace HTBInvoiceManager
{
    public static class Util
    {

        #region Get Invoices

        public static ArrayList GetOpenedPayments(int aktId)
        {
            // InvoiceStatus = -1 (VOID)
            return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId + " AND InvoiceBalance > 0 AND InvoiceStatus >= 0 AND InvoiceType IN ("+tblCustInkAktInvoice.GetPaymentTypesString()+")", typeof(tblCustInkAktInvoice));
        }

        public static ArrayList GetOpenedInvoices(int aktId)
        {
            // InvoiceStatus = -1 (VOID)
            return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId + " AND InvoiceBalance > 0 AND InvoiceStatus >= 0 AND InvoiceType NOT IN ("+tblCustInkAktInvoice.GetPaymentTypesString()+") ORDER BY InvoiceType", typeof(tblCustInkAktInvoice));
        }

        public static ArrayList GetInvoiceListByAktAndFordID(int aktId, int fordId)
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId + " AND InvoiceCustInkAktFordID = " + fordId, typeof(tblCustInkAktInvoice));
        }

        public static tblCustInkAktInvoice GetInvoiceByAktAndFordID(int aktId, int fordId)
        {
            return (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId + " AND InvoiceCustInkAktFordID = " + fordId, typeof(tblCustInkAktInvoice));
        }

        public static tblCustInkAktInvoice GetInvoiceByAktAndKostenKlientID(int aktId, int kkId)
        {
            return (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId + " AND InvoiceCustInkAktKostenKlientID = " + kkId, typeof(tblCustInkAktInvoice));
        }

        public static tblCustInkAktInvoice GetInvoiceByAktAndType(int aktId, int type)
        {
            return (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId + " AND InvoiceType = " + type, typeof(tblCustInkAktInvoice));
        }

        public static tblCustInkAktInvoice GetInvoiceByID(int invId)
        {
            return (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + invId, typeof(tblCustInkAktInvoice));
        }
        
        public static ArrayList GetKLientInvoices(int aktId)
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId + " AND InvoiceType in (" + tblCustInkAktInvoice.GetKlientInvoiceTypesString() + ")", typeof(tblCustInkAktInvoice));
        }
        public static ArrayList GetOpenedKlientInvoices()
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktOpenedInvoice WHERE InvoiceType in (" + tblCustInkAktInvoice.GetKlientInvoiceTypesString() + ")", typeof(tblCustInkAktInvoice));
        }

        public static ArrayList GetCollectionInvoices(int aktId)
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + aktId + " AND InvoiceType in (" + tblCustInkAktInvoice.GetCollectionInvoiceTypesString() + ")", typeof(tblCustInkAktInvoice));
        }
        public static ArrayList GetOpenedCollectionInvoices()
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktOpenedInvoice WHERE InvoiceType in (" + tblCustInkAktInvoice.GetCollectionInvoiceTypesString() + ")", typeof(tblCustInkAktInvoice));
        }
        #endregion
    }
}
