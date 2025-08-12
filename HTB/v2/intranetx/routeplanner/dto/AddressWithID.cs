using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace HTB.v2.intranetx.routeplanner.dto
{
    [DebuggerDisplay("AddressWithID {" + nameof(DebuggerDisplay) + "}")]
    [XmlRoot(ElementName = "AddressWithID", IsNullable = false)]
    [Serializable]
    public class AddressWithID : ISerializable
    {
        public int ID { get; set; }
        public string Address { get; set; }
        public List<int> OtherIds { get; set; }
        public List<AddressLocation> SuggestedAddresses { get; set; }
        
        public AddressWithID()
        {
        }

        public AddressWithID(int id, string address)
        {
            ID = id;
            Address = address;
            OtherIds = new List<int>();
            SuggestedAddresses = new List<AddressLocation>();
        }

        public bool Equals(AddressWithID address)
        {
            //            return ID == address.ID && Address == address.Address;
            return Address == address.Address;
        }

        public string DebuggerDisplay => $"[ADDR: {Address}] [ID: {ID}]";

        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private AddressWithID(SerializationInfo info, StreamingContext ctxt)
        {
            ID = info.GetInt32("ID");
            Address = info.GetString("Address");
            OtherIds = (List<int>)info.GetValue("OtherIds", typeof(List<int>));
            SuggestedAddresses = (List<AddressLocation>)info.GetValue("SuggestedAddresses", typeof(List<AddressLocation>));

        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Address", Address);
            info.AddValue("OtherIds", OtherIds, typeof(List<int>));
            info.AddValue("SuggestedAddresses", SuggestedAddresses, typeof(List<AddressLocation>));
        }
        #endregion
    }
}