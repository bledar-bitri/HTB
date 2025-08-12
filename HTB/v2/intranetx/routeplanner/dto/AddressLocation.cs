using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace HTB.v2.intranetx.routeplanner.dto
{
    [DebuggerDisplay("AddressLocation {DebuggerDisplay}")]
    [XmlRoot(ElementName = "AddressLocation", IsNullable = false)]
    [Serializable]
    public class AddressLocation : ISerializable
    {
        public AddressWithID Address { get; set; }
        public GeocodeService.GeocodeLocation[] Locations { get; set; }

        public AddressLocation() {}
        public AddressLocation(AddressWithID address, GeocodeService.GeocodeLocation[] locations)
        {
            Address = address;
            Locations = locations;
        }
        public bool Equals(AddressLocation address)
        {
            return address.Address == Address;
        }
        public string DebuggerDisplay
        {
            get { return string.Format("{0} Locations [{1}]", Address.DebuggerDisplay, String.Join("; ", Locations.Select(l => l.Latitude+", "+l.Longitude))); }
        }
        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private AddressLocation(SerializationInfo info, StreamingContext ctxt)
        {
            Address = (AddressWithID)info.GetValue("Address", typeof(AddressWithID));
            Locations = (GeocodeService.GeocodeLocation[])info.GetValue("Locations", typeof(GeocodeService.GeocodeLocation[]));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Address", Address, typeof(AddressWithID));
            info.AddValue("Locations", Locations, typeof(GeocodeService.GeocodeLocation[]));
        }
        #endregion

        public string GetDumpData()
        {
            return DebuggerDisplay;
        }
    }

}