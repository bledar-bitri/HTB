using System.Collections;
using HTB.Database;

namespace HTBExtras.XML
{
    public class XmlChangedAktsRequestRecord : Record
    {
        public int AktIntId { get; set; }
        public string AktIntTimestamp { get; set; }
        public double AktIntBalance { get; set; }
        public ArrayList PhoneTimeStampsList { get; set; }
        public ArrayList AddressTimeStampsList { get; set; }
        public ArrayList DocTimeStampsList { get; set; }
        public ArrayList ActionsTimeStampsList { get; set; }

        public XmlChangedAktsRequestRecord ()
        {
            PhoneTimeStampsList = new ArrayList();
            AddressTimeStampsList = new ArrayList();
            DocTimeStampsList = new ArrayList();
            ActionsTimeStampsList = new ArrayList();
        }
    }
}
