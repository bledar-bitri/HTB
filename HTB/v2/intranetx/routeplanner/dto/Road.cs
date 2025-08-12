using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Xml.Serialization;

namespace HTB.v2.intranetx.routeplanner.dto
{
    [DebuggerDisplay("Road {DebuggerDisplay}")]
    [XmlRoot(ElementName = "Road", IsNullable = false)]
    [Serializable]
    public class Road : ISerializable
    {
        public City From { get; set; }
        public City To { get; set; }
        public double Distance { get; set; }
        public long TravelTimeInSeconds { get; set; }

        public Road()
        {
        }

        public Road(City fromAddress, City toAddress)
        {
            From = fromAddress;
            To = toAddress;
        }

        public bool Equals(Road r)
        {
            return (From.Equals(r.From) && To.Equals(r.To)) || (From.Equals(r.To) && To.Equals(r.From));
        }

        public string DebuggerDisplay
        {
            get { return string.Format("[FROM: {0}] [TO: {1}] [Distance: {2}] [TravelTime: {3}]", From.DebuggerDisplay, To.DebuggerDisplay, Distance, TravelTimeInSeconds); }
        }

        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private Road(SerializationInfo info, StreamingContext ctxt)
        {
            From = (City)info.GetValue("From", typeof(City));
            To = (City)info.GetValue("To", typeof(City));
            Distance = info.GetDouble("Distance");
            TravelTimeInSeconds = info.GetInt64("TimeInSeconds");
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("From", From, typeof(City));
            info.AddValue("To", To, typeof(City));
            info.AddValue("Distance", Distance);
            info.AddValue("TimeInSeconds", TravelTimeInSeconds);
        }
        #endregion
    }
    
}