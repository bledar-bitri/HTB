using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Xml.Serialization;
using HTBAntColonyTSP;

namespace HTB.v2.intranetx.routeplanner.dto
{
    [DebuggerDisplay("City {DebuggerDisplay}")]
    [XmlRoot(ElementName = "City", IsNullable = false)]
    [Serializable]
    public class City : ISerializable
    {
        public const int Radius = 6;
        
        public AddressLocation Location { get; set; }

        public AddressWithID Address => Location.Address;

        public TspCity TspTspCity { get; set; }

        public City()
        {
        }

        public City(AddressLocation location, TspCity tspTspCity)
        {
            Location = location;
            TspTspCity = tspTspCity;
        }

        public bool Equals(City c)
        {
            return Address.Equals(c.Address);
        }

        public string DebuggerDisplay => Location.DebuggerDisplay;

        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private City(SerializationInfo info, StreamingContext ctxt)
        {
            TspTspCity = (TspCity) info.GetValue("TspCity", typeof (TspCity));
            Location = (AddressLocation) info.GetValue("AddressLocation", typeof (AddressLocation));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("TspCity", TspTspCity, typeof(TspCity));
            info.AddValue("AddressLocation", Location, typeof(AddressLocation));
        }
        #endregion

        public string GetDumpData()
        {
            var sb = new StringBuilder();
            sb.Append("City: ");
            sb.Append(Location.DebuggerDisplay);
            return sb.ToString();
        }
    }
}