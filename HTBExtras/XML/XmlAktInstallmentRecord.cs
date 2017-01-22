using HTB.Database;

namespace HTBExtras.XML
{
    public class XmlAktInstallmentRecord : Record
    {
        public double Balance { get; set; }
        public double OriginalInvoiceAmount { get; set; }
    }
}
