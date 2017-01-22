using HTB.Database;

namespace HTBExtras.XML
{
    public class XmlTelAndEmailRecord : Record
    {
        public string PhoneCity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public bool NoPhone { get; set; }
        public bool NoEmail { get; set; }

    }
}
