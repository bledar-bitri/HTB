using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Serialization;


namespace HTBAntColonyTSP
{
	[DebuggerDisplay("{Name}")]
    [XmlRoot(ElementName = "TspCity", IsNullable = false)]
    [Serializable]
    public class TspCity : ISerializable
	{
        
		private string m_Name;
	    private Dictionary<TspCity, Road> m_Roads { get; set; }
		
        public TspCity()
        {
        }

	    public string Name
		{
            get { return m_Name; }
            set { m_Name = value; }
		}
		
		public IEnumerable<TspCity> NeighbourCities
		{
            get { return m_Roads.Keys; }
		}
		
		public Road Roads(TspCity to)
		{
			Road ret;
			m_Roads.TryGetValue(to, out ret);
			return ret;
		}
		
		public TspCity(string name)
		{
			m_Name = name;
			m_Roads = new Dictionary<TspCity, Road>();
		}
		
		internal void AddRoad(Road road, TspCity otherTspCity)
		{
			m_Roads.Add(otherTspCity, road);
		}

        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private TspCity(SerializationInfo info, StreamingContext ctxt)
        {
            m_Name = info.GetString("Name");
            m_Roads = (Dictionary<TspCity, Road>)info.GetValue("Roads", typeof(Dictionary<TspCity, Road>));
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", m_Name);
            info.AddValue("Roads", m_Roads, typeof(Dictionary<TspCity, Road>));
        }
        #endregion
	}
}
