using HTB.Database;

namespace HTBExtras.XML
{
    public class XmlPrintableReceiptRecord : Record
    {
        public string ReceiptId { get; set; }
        public int ActionId { get; set; }
        public string ReceiptDate { get; set; }
        public string ActionNumber { get; set; }
        public string AktId { get; set; }
        public string AktAz { get; set; }
        public string City { get; set; }
        public string AgName { get; set; }
        public string AgZipCity { get; set; }
        public string AgStreet { get; set; }
        public string AgTel { get; set; }
        public string AgEmail { get; set; }
        public string AgWeb { get; set; }
        public string AgSb { get; set; }
        public string AgSbEmail { get; set; }
        public string Collector { get; set; }
        public string PaymentType { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentTax { get; set; }
        public string PaymentTotal { get; set; }
        public string eceiptNumber { get; set; }
    }
}
