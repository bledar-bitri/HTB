using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Xml.Serialization;
using HTBAntColonyTSP;

namespace HTB.v2.intranetx.routeplanter
{
    [XmlRoot(ElementName = "RoutePlannerManager", IsNullable = false)]
    [Serializable]
    public class Map : ISerializable
    {
        public const string BingJsStart = "directionsManager.addWaypoint(new Microsoft.Maps.Directions.Waypoint({ location: new Microsoft.Maps.Location(";
        public const string BingJsEnd = "), exactLocation: true })); ";

        private List<City> m_Cities = new List<City>();
        private List<Road> m_Roads = new List<Road>();
        private Dictionary<TspCity, City> m_CityMap = new Dictionary<TspCity, City>();
        private Dictionary<HTBAntColonyTSP.Road, Road> m_RoadMap;
        
        public Map()
        {
        }

        public void AddCity(City city)
        {
            m_Cities.Add(city);
        }

        public int CityCount
        {
            get
            {
                return m_Cities.Count;
            }
        }

        public void AddRoad(Road road)
        {
            m_Roads.Add(road);
        }

        public City FindCity(AddressLocation location)
        {
            return m_Cities.Find(c => c.Location.Address == location.Address);
        }

        public void Clear()
        {
            m_Cities.Clear();
            m_Roads.Clear();
        }

        public World ConstructTsp()
        {
            var wb = new WorldBuilder();

            m_CityMap.Clear();
            m_CityMap = new Dictionary<HTBAntColonyTSP.TspCity, City>(m_Cities.Count);

            foreach (var c in m_Cities)
            {
                c.TspTspCity = wb.AddCity(c.Address.Address);
                m_CityMap.Add(c.TspTspCity, c);
            }

            m_RoadMap = new Dictionary<HTBAntColonyTSP.Road, Road>(Convert.ToInt32(Math.Pow(m_Cities.Count, 2)));

            foreach (var road in m_Roads)
            {
                m_RoadMap.Add(wb.AddRoad(road.Distance, road.TravelTimeInSeconds, road.From.TspTspCity, road.To.TspTspCity), road);
            }

            return new World(wb);
        }

        public string GetTourString(IEnumerable<TspCity> tour)
        {
            if (tour == null)
            {
                return null;
            }

            return PrintTour(tour);
        }

        public String GetProgressTour(IEnumerable<TspCity> tour)
        {
            var sb = new StringBuilder();
            var i = tour.GetEnumerator();
            int counter = 1;
            while (i.MoveNext())
            {
                sb.Append(m_CityMap[i.Current].Location.Address.ID);
                sb.Append(" > ");
//                if (counter++ % 10 == 0)
//                    sb.Append(" <br/> ");                
            }
            return sb.ToString();
        }

        private String PrintTour(IEnumerable<TspCity> tour)
        {
            var sb = new StringBuilder();
            var i = tour.GetEnumerator();
            i.MoveNext();
            var first = m_CityMap[i.Current];
            var c1 = first;

            while (i.MoveNext())
            {
                var c2 = m_CityMap[i.Current];
                sb.Append(c1.Location.Address.ID);
                sb.Append(" : ");
                sb.Append(c1.Location.Address.Address);
                sb.Append( " =====> ");
                sb.Append(c2.Location.Address.ID);
                sb.Append(" : ");
                sb.Append(c2.Location.Address.Address);
                sb.Append(" <BR> ");
                c1 = c2;
            }
            sb.Append(" LASTLY: ");
            sb.Append(c1.Location.Address.ID);
            sb.Append(" : ");
            sb.Append(c1.Location.Address.Address);
            sb.Append(" =====> ");
            sb.Append(first.Location.Address.ID);
            sb.Append(" : ");
            sb.Append(first.Location.Address.ID);
            sb.Append(" <BR> ");
            return sb.ToString();
        }

        public String GetJsWaypoints(IEnumerable<TspCity> tour, int startPoint, int totalPoints)
        {
            var sb = new StringBuilder();
            var i = tour.GetEnumerator();
            int idx = 1;
            i.MoveNext();

//            DebugTourAndCity(tour, "GetJsWaypoints");

            var first = m_CityMap[i.Current];
            var c1 = first;
            
            if (idx >= startPoint && idx <= startPoint + totalPoints)
            {
                sb.Append(BingJsStart);
                sb.Append(c1.Location.Locations[0].Latitude.ToString().Replace(",", "."));
                sb.Append(", ");
                sb.Append(c1.Location.Locations[0].Longitude.ToString().Replace(",", "."));
                sb.Append(BingJsEnd);
                sb.Append("  //");
                sb.Append(c1.Address.Address);
                sb.Append(Environment.NewLine);
            }
            
            while (i.MoveNext())
            {
                idx++;
                if (idx >= startPoint && idx <= startPoint + totalPoints)
                {
                    c1 = m_CityMap[i.Current];
                    sb.Append(BingJsStart);
                    sb.Append(c1.Location.Locations[0].Latitude.ToString().Replace(",", "."));
                    sb.Append(", ");
                    sb.Append(c1.Location.Locations[0].Longitude.ToString().Replace(",", "."));
                    sb.Append(BingJsEnd);
                    sb.Append("  //");
                    sb.Append(c1.Address.Address);
                    sb.Append(Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        public String GetJsOverwiewWaypoints(IEnumerable<TspCity> tour, int totalPoints)
        {
            var sb = new StringBuilder();
            var totalCount = tour.Count();
            var count = 0;
            var i = tour.GetEnumerator();
            
            int idx = 1;
            i.MoveNext();
            
            var first = m_CityMap[i.Current];
            var c1 = first;

            sb.Append(BingJsStart);
            sb.Append(c1.Location.Locations[0].Latitude.ToString().Replace(",", "."));
            sb.Append(", ");
            sb.Append(c1.Location.Locations[0].Longitude.ToString().Replace(",", "."));
            sb.Append(BingJsEnd);
            sb.Append("  //");
            sb.Append(c1.Address.Address);
            sb.Append(Environment.NewLine);
            count = 1;
            
            while (i.MoveNext())
            {
                idx++;
                if (idx % (totalCount / totalPoints) == 0 || idx == totalCount)
                {
                    if (count == totalCount - 1 && idx < totalCount)
                    {
                        continue;
                    }
                    count++;
                    c1 = m_CityMap[i.Current];
                    sb.Append(BingJsStart);
                    sb.Append(c1.Location.Locations[0].Latitude.ToString().Replace(",", "."));
                    sb.Append(", ");
                    sb.Append(c1.Location.Locations[0].Longitude.ToString().Replace(",", "."));
                    sb.Append(BingJsEnd);
                    sb.Append("  //");
                    sb.Append(c1.Address.Address);
                    sb.Append(Environment.NewLine);
                    
                }
            }
            return sb.ToString();
        }

        public String GetJsReversedWaypoints(IEnumerable<TspCity> tour, int startPoint, int totalPoints)
        {
            return GetJsWaypoints(GetReversedTour(tour), startPoint, totalPoints);
        }

        public String GetJsReversedOverwiewWaypoints(IEnumerable<TspCity> tour, int totalPoints)
        {
            return GetJsOverwiewWaypoints(GetReversedTour(tour), totalPoints);
        }

        private IEnumerable<TspCity> GetReversedTour(IEnumerable<TspCity> tour)
        {
            List<City> original = GetAllWaypoints(tour);
            var reversed = new List<HTBAntColonyTSP.TspCity>
                               {
                                   original[0].TspTspCity
                               };
            for (int i = original.Count - 1; i > 0; i--)
            {
                reversed.Add(original[i].TspTspCity);
            }
            return reversed;
        }

        public List<City> GetAllWaypoints(IEnumerable<TspCity> tour)
        {
            var list = new List<City>();
            var i = tour.GetEnumerator();

            while (i.MoveNext())
                list.Add(m_CityMap[i.Current]);

            return list;
        }

        public List<int> GetAllWaypointIDs(IEnumerable<TspCity> tour, bool isReverse)
        {
            var list = new List<int>();
            var i = isReverse ? GetReversedTour(tour).GetEnumerator() : tour.GetEnumerator();
            
            while (i.MoveNext())
                list.Add(m_CityMap[i.Current].Address.ID);
                
            return list;
        }

        public List<int> GetAllIDs(IEnumerable<TspCity> tour, bool isReverse)
        {
            var list = new List<int>();
            var i = isReverse ? GetReversedTour(tour).GetEnumerator() : tour.GetEnumerator();

            while (i.MoveNext())
            {
                list.Add(m_CityMap[i.Current].Address.ID);
                list.AddRange(m_CityMap[i.Current].Address.OtherIds);
            }


            return list;
        }

        public List<AddressWithID> GetTourAddresses(IEnumerable<TspCity> tour, bool isReverse)
        {
            var list = new List<AddressWithID>();
            var i = isReverse ? GetReversedTour(tour).GetEnumerator() : tour.GetEnumerator();

            while (i.MoveNext())
                list.Add(m_CityMap[i.Current].Address);

            return list;
        }

        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private Map(SerializationInfo info, StreamingContext ctxt)
        {
            m_Cities = (List<City>)info.GetValue("Cities", typeof(List<City>));
            m_Roads = (List<Road>)info.GetValue("Roads", typeof(List<Road>));
            m_CityMap = (Dictionary<TspCity, City>)info.GetValue("CityMap", typeof(Dictionary<TspCity, City>));
            m_RoadMap = (Dictionary<HTBAntColonyTSP.Road, Road>)info.GetValue("RoadMap", typeof(Dictionary<HTBAntColonyTSP.Road, Road>));

        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Cities", m_Cities, typeof(List<City>));
            info.AddValue("Roads", m_Roads, typeof(List<Road>));
            info.AddValue("CityMap", m_CityMap, typeof(Dictionary<TspCity, City>));
            info.AddValue("RoadMap", m_RoadMap, typeof(Dictionary<HTBAntColonyTSP.Road, Road>));
        }
        #endregion

        public void DebugTourAndCityX(IEnumerable<TspCity> tour, string msg)
        {
            string fname = "C:/temp/MapTourAndCityLog.txt";
            var i = tour.GetEnumerator();
            i.MoveNext();

            File.AppendAllText(fname, msg + " start"+Environment.NewLine+Environment.NewLine);

            File.AppendAllText(fname, i == null ? "i is null" : "i is NOT null" + Environment.NewLine + Environment.NewLine);
            File.AppendAllText(fname, i.Current == null ? "i.Current is null" : "i.Current is NOT null" + Environment.NewLine + Environment.NewLine);
            File.AppendAllText(fname, m_CityMap == null ? "m_CityMap is null" : "m_CityMap is NOT null COUNT " + m_CityMap.Count + Environment.NewLine + Environment.NewLine);

            File.AppendAllText(fname, msg + " END" + Environment.NewLine + Environment.NewLine);
        }

        public static void DebugInfoX(string msg)
        {
            string fname = "C:/temp/MapLog.txt";
            File.AppendAllText(fname, msg + Environment.NewLine);
        }

        public static void DebugTourX(IEnumerable<TspCity> tour, string msg)
        {
            string fname = "C:/temp/MapTourLog.txt";
            var i = tour.GetEnumerator();
            i.MoveNext();

            File.AppendAllText(fname, msg + " start" + Environment.NewLine + Environment.NewLine);

            File.AppendAllText(fname, i == null ? "i is null" : "i is NOT null" + Environment.NewLine + Environment.NewLine);
            File.AppendAllText(fname, i.Current == null ? "i.Current is null" : "i.Current is NOT null" + Environment.NewLine + Environment.NewLine);

            File.AppendAllText(fname, msg + " END" + Environment.NewLine + Environment.NewLine);
        }
    }
}