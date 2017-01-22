using System;

namespace HTBExtras
{
    public class KlientTransferRecord
    {
        public int AktId { get; set; }

        public string AktAZ { get; set; }

        public double KlientAmount { get; set; }

        public double KlientBalance { get; set; }
        public double AppliedToInvoice { get; set; }
        public double AppliedToInterest { get; set; }
        public DateTime TransferDate { get; set; }

        public double TransferAmount { get; set; }

        public string GegnerName { get; set; }
        public string KlientInvoiceNumber { get; set; }
        public string KlientCustomerNumber { get; set; }
        public string Comment { get; set; }
    }
}
