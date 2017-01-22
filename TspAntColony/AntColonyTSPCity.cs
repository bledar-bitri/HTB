using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace TspAntColony
{


    [DebuggerDisplay("{Name}")]
    public class AntColonyTSPCity
    {

        private readonly string m_Name;

        private readonly Dictionary<City, Road> m_Roads;

        public string Name
        {
            get { return m_Name; }
        }

        public IEnumerable<City> NeighbourCities
        {
            get { return m_Roads.Keys; }
        }

        public Road Roads
        {
            get
            {
                Road ret = null;
                m_Roads.TryGetValue(to, ret);
                return ret;
            }
        }

        public AntColonyTSPCity(string name)
        {
            m_Name = name;
            m_Roads = new Dictionary<City, Road>();
        }

        internal void AddRoad(Road road, City other_city)
        {
            m_Roads.Add(other_city, road);
        }
    }
}


