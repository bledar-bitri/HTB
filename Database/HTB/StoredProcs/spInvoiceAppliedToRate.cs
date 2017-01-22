using System;

namespace HTB.Database.StoredProcs
{
    public class spInvoiceAppliedToRate : Record
    {
        #region Property Declaration tblCustInkAktInvoice
        private int _invoiceID;

        public int InvoiceID
        {
            get { return _invoiceID; }
            set { _invoiceID = value; }
        }
        private DateTime _invoiceDate;

        public DateTime InvoiceDate
        {
            get { return _invoiceDate; }
            set { _invoiceDate = value; }
        }
        private string _invoiceDescription;

        public string InvoiceDescription
        {
            get { return _invoiceDescription; }
            set { _invoiceDescription = value; }
        }
        private double _invoiceAmountNetto;

        public double InvoiceAmountNetto
        {
            get { return _invoiceAmountNetto; }
            set { _invoiceAmountNetto = value; }
        }
        private double _invoiceAmount;

        public double InvoiceAmount
        {
            get { return _invoiceAmount; }
            set { _invoiceAmount = value; }
        }
        private double _invoiceTax;

        public double InvoiceTax
        {
            get { return _invoiceTax; }
            set { _invoiceTax = value; }
        }
        private int _invoiceType;

        public int InvoiceType
        {
            get { return _invoiceType; }
            set { _invoiceType = value; }
        }
        private DateTime _invoiceDueDate;

        public DateTime InvoiceDueDate
        {
            get { return _invoiceDueDate; }
            set { _invoiceDueDate = value; }
        }
        private int _invoiceCustInkAktId;

        public int InvoiceCustInkAktId
        {
            get { return _invoiceCustInkAktId; }
            set { _invoiceCustInkAktId = value; }
        }
        private double _invoiceBalance;

        public double InvoiceBalance
        {
            get { return _invoiceBalance; }
            set { _invoiceBalance = value; }
        }
        private DateTime _invoicePaymentReceivedDate;

        public DateTime InvoicePaymentReceivedDate
        {
            get { return _invoicePaymentReceivedDate; }
            set { _invoicePaymentReceivedDate = value; }
        }
        private string _invoiceComment;

        public string InvoiceComment
        {
            get { return _invoiceComment; }
            set { _invoiceComment = value; }
        }
        #endregion

        #region Property Declaration tblCustInkAktRate

        public int CustInkAktRateID { get; set; }

        public double CustInkAktRateAmount { get; set; }

        public DateTime CustInkAktRateDate { get; set; }

        public DateTime CustInkAktRateDueDate { get; set; }

        public string CustInkAktRateComment { get; set; }

        public double CustInkAktRateReceivedAmount { get; set; }

        public double CustInkAktRateBalance { get; set; }
        public DateTime CustInkAktRatePostponeTillDate { get; set; }
        public bool CustInkAktRatePostponeWithNoOverdue { get; set; }
        public string CustInkAktRatePostponeReason { get; set; }
        public int CustInkAktRatePostponedUser { get; set; }


        #endregion

        #region Property Declaration : tblCustInkAktRateApply
        private int _rateApplyID;

        public int RateApplyID
        {
            get { return _rateApplyID; }
            set { _rateApplyID = value; }
        }
        private DateTime _rateApplyDate;

        public DateTime RateApplyDate
        {
            get { return _rateApplyDate; }
            set { _rateApplyDate = value; }
        }
        private double _rateApplyAmount;

        public double RateApplyAmount
        {
            get { return _rateApplyAmount; }
            set { _rateApplyAmount = value; }
        }
        #endregion

        #region Property Declaration tblUser
        public string UserVorname { get; set; }
        public string UserNachname { get; set; }
        #endregion
    }
}
