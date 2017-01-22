/*
 * Author:			Generated Code
 * Date Created:	11.03.2011
 * Description:		Represents a row in the tblCustInkAktInvoice table
*/

using System;
using System.Text;
namespace HTB.Database
{
	public class tblCustInkAktInvoice : Record
    {
        #region Const Declaration
        public const int INVOICE_TYPE_ORIGINAL = 1;
        public const int INVOICE_TYPE_CLIENT_COST = 11;
        public const int INVOICE_TYPE_INTEREST_CLIENT_ORIGINAL = 111;

        public const int INVOICE_TYPE_COLLECTION_INVOICE = 2;                   // Generic Holder of collection invoice
        public const int INVOICE_TYPE_COLLECTION_INVOICE_BEARBEIT = 200; 
        public const int INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_1 = 201;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_2 = 202;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_3 = 203;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_4 = 204;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_5 = 205;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_TELEFON = 206;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_MELDE = 207;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_EVIDENZ = 208;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_PERSONAL_INTERVENTION = 209;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_RATE_ANGEBOT = 210;
        public const int INVOICE_TYPE_COLLECTION_INVOICE_MAIL_FEE = 211;        // Porto
        public const int INVOICE_TYPE_COLLECTION_INVOICE_OVERDUE_CHARGE = 220;  // Terminverlust
        

        public const int INVOICE_TYPE_INTEREST_COLECTION = 3;
        public const int INVOICE_TYPE_INTEREST_CLIENT = 33;
        
        public const int INVOICE_TYPE_TAX = 4;
        
        public const int INVOICE_TYPE_DEBIT = 5;
        
        public const int INVOICE_TYPE_RETURNED = 6;

        public const int INVOICE_TYPE_CREDIT = 7;
        
        public const int INVOICE_TYPE_PAYMENT_CASH = 9;
        public const int INVOICE_TYPE_PAYMENT_TRANSFER = 91;
        public const int INVOICE_TYPE_CLIENT_INITIAL_PAYMENT = 99;
        public const int INVOICE_TYPE_PAYMENT_DIRECT_TO_CLIENT = 999;
        
        public const int INVOICE_STATUS_NO_PAYMENT_APPLIED = 0;
        public const int INVOICE_STATUS_PARTIAL = 1;
        public const int INVOICE_STATUS_PAID = 9;
        public const int INVOICE_STATUS_VOID = -1;
        #endregion

        #region Property Declaration
        private int _invoiceID;
		private DateTime _invoiceDate;
        private DateTime _invoiceLastInterestDate;
	    private double _invoiceAmount;
	    private int _invoiceType;
		private DateTime _invoiceDueDate;
		private int _invoiceCustInkAktId;
		private double _invoiceBalance;
		private int _invoiceStatus;
	    private int _invoiceCustInkAktKostenKlientID;

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int InvoiceID
        {
            get { return _invoiceID; }
            set { _invoiceID = value; }
        }
        public DateTime InvoiceDate
        {
            get { return _invoiceDate; }
            set { _invoiceDate = value; }
        }
        public DateTime InvoiceLastInterestDate
        {
            get { return _invoiceLastInterestDate; }
            set { _invoiceLastInterestDate = value; }
        }
		[MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double InvoiceAmount
		{
			get { return _invoiceAmount; }
			set { _invoiceAmount = value; }
		}
		public int InvoiceType
		{
			get { return _invoiceType; }
			set { _invoiceType = value; }
		}
		public DateTime InvoiceDueDate
		{
			get { return _invoiceDueDate; }
			set { _invoiceDueDate = value; }
		}
		public int InvoiceCustInkAktId
		{
			get { return _invoiceCustInkAktId; }
			set { _invoiceCustInkAktId = value; }
		}
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double InvoiceBalance
		{
			get { return _invoiceBalance; }
			set { _invoiceBalance = value; }
		}
		public int InvoiceStatus
		{
			get { return _invoiceStatus; }
			set { _invoiceStatus = value; }
		}

	    public int InvoiceCustInkAktFordID { get; set; }

	    public int InvoiceCustInkAktKostenKlientID
		{
			get { return _invoiceCustInkAktKostenKlientID; }
			set { _invoiceCustInkAktKostenKlientID = value; }
		}

	    public string InvoiceDescription { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double InvoiceTax { get; set; }

	    public DateTime InvoiceLastUpdated { get; set; }

	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
	    public double InvoiceAmountNetto { get; set; }

	    public int InvoiceInterestID { get; set; }

	    public string InvoiceReference { get; set; }

	    public string InvoiceClientReference { get; set; }

	    public DateTime InvoiceClientDate { get; set; }

	    public DateTime InvoicePaymentReceivedDate { get; set; }

	    public DateTime InvoicePaymentTransferToClientDate { get; set; }

	    public double InvoicePaymentTransferToClientAmount { get; set; }

	    public string InvoiceComment { get; set; }

	    public int InvoiceCustInkAktRateID { get; set; }

	    public string InvoiceBillNumber { get; set; }

        #endregion

        public string GetShortComment()
        {
            if (InvoiceComment.Length > 20)
                return InvoiceComment.Substring(0, 20);
            else
                return InvoiceComment;
        }
        public bool IsPayment()
        {
            return InvoiceType == INVOICE_TYPE_CREDIT || 
                InvoiceType == INVOICE_TYPE_PAYMENT_CASH || 
                InvoiceType == INVOICE_TYPE_CLIENT_INITIAL_PAYMENT || 
                InvoiceType == INVOICE_TYPE_PAYMENT_DIRECT_TO_CLIENT || 
                InvoiceType == INVOICE_TYPE_PAYMENT_TRANSFER ||
                InvoiceType == INVOICE_TYPE_RETURNED;
        }
        
        public bool IsBankTransferrable()
        {
            return InvoiceType == INVOICE_TYPE_PAYMENT_CASH || InvoiceType == INVOICE_TYPE_PAYMENT_TRANSFER;
        }
        public bool IsDebit()
        {
            return InvoiceType == INVOICE_TYPE_DEBIT;
        }
        public bool IsInterest()
        {
            return InvoiceType == INVOICE_TYPE_INTEREST_CLIENT || InvoiceType == INVOICE_TYPE_INTEREST_CLIENT_ORIGINAL || InvoiceType == INVOICE_TYPE_INTEREST_COLECTION;
        }

        public bool IsVoid()
        {
            return InvoiceStatus == INVOICE_STATUS_VOID;
        }

        public bool IsClientInvoice()
        {
            return InvoiceType == INVOICE_TYPE_ORIGINAL || InvoiceType == INVOICE_TYPE_CLIENT_COST || InvoiceType == INVOICE_TYPE_INTEREST_CLIENT_ORIGINAL || InvoiceType == INVOICE_TYPE_INTEREST_CLIENT;
        }
        public bool IsClientInterest()
        {
            return InvoiceType == INVOICE_TYPE_INTEREST_CLIENT;
        }
        
        #region Invoice Strings (used in IN clauses)
        public static string GetKlientInvoiceTypesString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(INVOICE_TYPE_ORIGINAL.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_CLIENT_COST.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_INTEREST_CLIENT_ORIGINAL.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_INTEREST_CLIENT.ToString());
            return sb.ToString();
        }
        public static string GetCollectionInvoiceTypesString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_BEARBEIT.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_1.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_2.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_3.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_4.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_MAHNUNG_5.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_TELEFON.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_MELDE.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_EVIDENZ.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_PERSONAL_INTERVENTION.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_RATE_ANGEBOT.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_MAIL_FEE.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_COLLECTION_INVOICE_OVERDUE_CHARGE.ToString());
            sb.Append(",");
            sb.Append(INVOICE_TYPE_TAX.ToString());
            return sb.ToString();
        }
        public static string GetPaymentTypesString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(INVOICE_TYPE_CREDIT);
            sb.Append(",");
            sb.Append(INVOICE_TYPE_PAYMENT_CASH);
            sb.Append(",");
            sb.Append(INVOICE_TYPE_CLIENT_INITIAL_PAYMENT);
            sb.Append(",");
            sb.Append(INVOICE_TYPE_PAYMENT_DIRECT_TO_CLIENT);
            sb.Append(",");
            sb.Append(INVOICE_TYPE_PAYMENT_TRANSFER);
            sb.Append(",");
            sb.Append(INVOICE_TYPE_RETURNED);
            return sb.ToString();
        }
        #endregion

    }
}
