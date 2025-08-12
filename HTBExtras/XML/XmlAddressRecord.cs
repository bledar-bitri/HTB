using HTB.Database;
using System;
using System.Xml.Serialization;

namespace HTBExtras.XML
{
    [Serializable]
    public class XmlAddressRecord : Record
    {
        public string AddressId { get; set; }
        [XmlElement("AddressText")]
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
