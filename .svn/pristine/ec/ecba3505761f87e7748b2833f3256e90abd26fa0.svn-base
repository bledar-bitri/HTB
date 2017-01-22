using System;

namespace HTB.Database.HTB.StoredProcs
{
    public class spGetBankTransferList : Record
    {
        public int ID { get; set; }
		public int InvoiceId { get; set; }
		public int AktId { get; set; }
		public string AktAZ { get; set; }
		public string CustInkAktGothiaNr { get; set; }
		public string CustInkAktKunde { get; set; }
		public DateTime InvoicePaymentReceivedDate { get; set; }
		public double InvoiceAmount { get; set; }
		public double ToTransferAmount { get; set; }
		public string GegnerName { get; set; }
        public string TransferToName { get; set; }
        public string TransferToBankCaption { get; set; }
        public string TransferToBLZ { get; set; }
        public string TransferToKtoNr { get; set; }
        public bool KlientReceivesInterest { get; set; }
        public int InvoiceTransferID { get; set; }
    }
}
