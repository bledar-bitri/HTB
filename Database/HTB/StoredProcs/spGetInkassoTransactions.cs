
using System;

namespace HTB.Database.HTB.StoredProcs 
{
    public class spGetInkassoTransactions : Record
    {
        public int InvoiceID { get; set; }
        public DateTime TransactionDate { get; set; }
        public double AppliedAmount { get; set; }
        public int InvoiceCustInkAktId { get; set; }
        public string GegnerName { get; set; }
        public string KlientName { get; set; }
        public string Description { get; set; }
        public bool IsECP { get; set; }
        public bool IsProvision { get; set; }
    }
}
