using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Xml.Serialization;
using HTBAntColonyTSP;

namespace HTB.v2.intranetx.routeplanner.dto
{
    [XmlRoot(ElementName = "RoutePlannerManager", IsNullable = false)]
    [Serializable]
    public class Map : ISerializable
    {
        private static readonly string[] PushPinColors = new string[]
        {
            "red",
            "green",
            "blue",
        };

        private readonly Random _random = new Random();

        public const string BingJsStart = "directionsManager.addWaypoint(new Microsoft.Maps.Directions.Waypoint({ location: new Microsoft.Maps.Location(";
        public const string BingJsEnd = "), exactLocation: true })); ";

        private List<City> _cities = new List<City>();
        private List<Road> _roads = new List<Road>();
        private Dictionary<TspCity, City> _cityMap = new Dictionary<TspCity, City>();
        private Dictionary<HTBAntColonyTSP.Road, Road> _roadMap;


        public Map()
        {
        }

        public void AddCity(City city)
        {
            _cities.Add(city);
        }

        public int CityCount
        {
            get
            {
                return _cities.Count;
            }
        }

        public void AddRoad(Road road)
        {
            _roads.Add(road);
        }

        public City FindCity(AddressLocation location)
        {
            return _cities.Find(c => c.Location.Address == location.Address);
        }

        public void Clear()
        {
            _cities.Clear();
            _roads.Clear();
        }

        public World ConstructTsp()
        {
            var wb = new WorldBuilder();

            _cityMap.Clear();
            _cityMap = new Dictionary<TspCity, City>(_cities.Count);

            foreach (var c in _cities)
            {
                c.TspTspCity = wb.AddCity(c.Address.Address);
                _cityMap.Add(c.TspTspCity, c);
            }

            _roadMap = new Dictionary<HTBAntColonyTSP.Road, Road>(Convert.ToInt32(Math.Pow(_cities.Count, 2)));

            foreach (var road in _roads)
            {
                _roadMap.Add(wb.AddRoad(road.Distance, road.TravelTimeInSeconds, road.From.TspTspCity, road.To.TspTspCity), road);
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

        public string GetProgressTour(IEnumerable<TspCity> tour)
        {
            var sb = new StringBuilder();
            using (var i = tour.GetEnumerator())
            {
                while (i.MoveNext())
                {
                    if (i.Current != null) sb.Append(_cityMap[i.Current].Location.Address.ID);
                    sb.Append(" > ");
//                if (counter++ % 10 == 0)
//                    sb.Append(" <br/> ");                
                }

                return sb.ToString();
            }
        }

        private string PrintTour(IEnumerable<TspCity> tour)
        {
            var sb = new StringBuilder();
            using (var i = tour.GetEnumerator())
            {
                i.MoveNext();
                if (i.Current != null)
                {
                    var first = _cityMap[i.Current];
                    var c1 = first;

                    while (i.MoveNext())
                    {
                        var c2 = _cityMap[i.Current];
                        sb.Append(c1.Location.Address.ID);
                        sb.Append(" : ");
                        sb.Append(c1.Location.Address.Address);
                        sb.Append(" =====> ");
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
                }

                sb.Append(" <BR> ");
                return sb.ToString();
            }
        }

        public string GetJsWaypoints(IEnumerable<TspCity> tour, int startPoint, int totalPoints)
        {
            var sb = new StringBuilder();
            using (var i = tour.GetEnumerator())
            {
                int idx = 1;
                i.MoveNext();

//            DebugTourAndCity(tour, "GetJsWaypoints");

                if (i.Current == null) return sb.ToString();
                var first = _cityMap[i.Current];
                var city = first;

                if (idx >= startPoint && idx <= startPoint + totalPoints)
                {
                    sb.Append(GetAddressLocationOnTheMap(city, idx, "green"));
                }

                while (i.MoveNext())
                {
                    idx++;
                    if (idx < startPoint || idx > startPoint + totalPoints) continue;
                    city = _cityMap[i.Current];
                    sb.Append(idx == startPoint + totalPoints
                        ? GetAddressLocationOnTheMap(city, idx, "red")
                        : GetAddressLocationOnTheMap(city, idx));
                }

                return sb.ToString();
            }
        }

        public string GetJsOverviewWaypoints(IEnumerable<TspCity> tour, int totalPoints)
        {
            var sb = new StringBuilder();
            var totalCount = tour.Count();

            using (var i = tour.GetEnumerator())
            {
                var idx = 1;
                i.MoveNext();

                if (i.Current == null) return string.Empty;
                var city = _cityMap[i.Current];

                sb.Append(GetAddressLocationOnTheMap(city, idx, "green"));

                var count = 1;

                while (i.MoveNext())
                {
                    idx++;
                    if (idx % (totalCount / totalPoints) != 0 && idx != totalCount) continue;
                    if (count == totalCount - 1 && idx < totalCount) continue;
                    

                    count++;
                    sb.Append(idx == totalCount
                        ? GetAddressLocationOnTheMap(city, idx, "red")
                        : GetAddressLocationOnTheMap(city, idx));
                }

                return sb.ToString();
            }
        }
        
        public string GetJsReversedWaypoints(IEnumerable<TspCity> tour, int startPoint, int totalPoints)
        {
            return GetJsWaypoints(GetReversedTour(tour), startPoint, totalPoints);
        }

        public string GetJsReversedOverviewWaypoints(IEnumerable<TspCity> tour, int totalPoints)
        {
            return GetJsOverviewWaypoints(GetReversedTour(tour), totalPoints);
        }

        #region JS Coordinates Leaflet

        public string GetJsCoordinatesForFirstLocation(IEnumerable<TspCity> tour, bool reverseTour)
        {
            if (reverseTour) tour = GetReversedTour(tour);

            var sb = new StringBuilder();
            using (var tourEnumerator = tour.GetEnumerator())
            {
                if (!tourEnumerator.MoveNext() || tourEnumerator.Current == null) return string.Empty;

                var city = _cityMap[tourEnumerator.Current];
                sb.Append(city.Location.Locations?[0].Latitude.ToString(CultureInfo.InvariantCulture));
                sb.Append(", ");
                sb.Append(city.Location.Locations?[0].Longitude.ToString(CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }
        public string GetJsCoordinates(IEnumerable<TspCity> tour, int startPoint, int totalPoints)
        {
            var sb = new StringBuilder();
            var totalCount = tour.Count();
            var count = 0;
            var tourEnumerator = tour.GetEnumerator();

            var idx = 1;
            tourEnumerator.MoveNext();

            var city = _cityMap[tourEnumerator.Current];
            sb.Append(GetGeoCoordinatesOnTheMap(city, idx));
            if (idx != totalCount)
                sb.Append(",");

            while (tourEnumerator.MoveNext())
            {
                city = _cityMap[tourEnumerator.Current];
                idx++;
                sb.Append(GetGeoCoordinatesOnTheMap(city, idx));

                if (idx != totalCount)
                    sb.Append(",");
            }
            return sb.ToString();
        }

        public string GetJsOverviewCoordinates(IEnumerable<TspCity> tour, int totalPoints)
        {
            return GetJsCoordinates(tour, 0, totalPoints);
        }

        public string GetJsReversedCoordinates(IEnumerable<TspCity> tour, int startPoint, int totalPoints)
        {
            return GetJsCoordinates(GetReversedTour(tour), startPoint, totalPoints);
        }

        public string GetJsReversedOverviewCoordinates(IEnumerable<TspCity> tour, int totalPoints)
        {
            return GetJsOverviewCoordinates(GetReversedTour(tour), totalPoints);
        }


        #endregion


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
                list.Add(_cityMap[i.Current]);

            return list;
        }

        public List<int> GetAllWaypointIDs(IEnumerable<TspCity> tour, bool isReverse)
        {
            var list = new List<int>();
            var i = isReverse ? GetReversedTour(tour).GetEnumerator() : tour.GetEnumerator();
            
            while (i.MoveNext())
                list.Add(_cityMap[i.Current].Address.ID);
                
            return list;
        }

        public List<int> GetAllIDs(IEnumerable<TspCity> tour, bool isReverse)
        {
            var list = new List<int>();
            var i = isReverse ? GetReversedTour(tour).GetEnumerator() : tour.GetEnumerator();

            while (i.MoveNext())
            {
                list.Add(_cityMap[i.Current].Address.ID);
                list.AddRange(_cityMap[i.Current].Address.OtherIds);
            }


            return list;
        }

        public List<AddressWithID> GetTourAddresses(IEnumerable<TspCity> tour, bool isReverse)
        {
            var list = new List<AddressWithID>();
            var i = isReverse ? GetReversedTour(tour).GetEnumerator() : tour.GetEnumerator();

            while (i.MoveNext())
                list.Add(_cityMap[i.Current].Address);

            return list;
        }
        
        private string GetAddressLocationOnTheMap(City city, int idx, string pinColor = "blue")
        {
            var sb = new StringBuilder();

            //lock (lockObj)
            {
                
                sb.Append($"var location{idx} = new Microsoft.Maps.Location({city.Location.Locations[0].Latitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")}, {city.Location.Locations[0].Longitude.ToString(CultureInfo.InvariantCulture).Replace(",", ".")});");
                sb.Append(Environment.NewLine);
                //sb.Append("var pin" + idx + " = new Microsoft.Maps.Pushpin(location" + idx + ", { text: '" + idx + "', color: '" + $"#{random.Next(0x1000000):X6}" + "'});");
                sb.Append("var pin" + idx + " = new Microsoft.Maps.Pushpin(location" + idx + ", { text: '" + idx + "', color: '" + pinColor + "'});");
                sb.Append(Environment.NewLine);
                //sb.Append("var wayPoint" + idx + " = new Microsoft.Maps.Directions.Waypoint({ address: '" + city.Address.Address + "' });");
                sb.Append("var wayPoint" + idx + " = new Microsoft.Maps.Directions.Waypoint({ location: location" + idx + ",  address: '" + city.Address.Address + "' });");
                sb.Append(Environment.NewLine);
                sb.Append($"map.entities.push(pin{idx});");
                sb.Append($"directionsManager.addWaypoint (wayPoint{idx});");
            }
            return sb.ToString();
        }

        private string GetGeoCoordinatesOnTheMap(City city, int idx)
        {
            if (!city.Location.Locations.Any()) return string.Empty;
            var sb = new StringBuilder();

            sb.Append("{coords: [");
            sb.Append(city.Location.Locations[0].Longitude.ToString(CultureInfo.InvariantCulture));
            sb.Append(", ");
            sb.Append(city.Location.Locations[0].Latitude.ToString(CultureInfo.InvariantCulture));
            sb.Append("], name: \"");
            sb.Append(city.Address.ID);
            sb.Append("\"}");

            return sb.ToString();
        }

        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private Map(SerializationInfo info, StreamingContext ctxt)
        {
            _cities = (List<City>)info.GetValue("Cities", typeof(List<City>));
            _roads = (List<Road>)info.GetValue("Roads", typeof(List<Road>));
            _cityMap = (Dictionary<TspCity, City>)info.GetValue("CityMap", typeof(Dictionary<TspCity, City>));
            _roadMap = (Dictionary<HTBAntColonyTSP.Road, Road>)info.GetValue("RoadMap", typeof(Dictionary<HTBAntColonyTSP.Road, Road>));

        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Cities", _cities, typeof(List<City>));
            info.AddValue("Roads", _roads, typeof(List<Road>));
            info.AddValue("CityMap", _cityMap, typeof(Dictionary<TspCity, City>));
            info.AddValue("RoadMap", _roadMap, typeof(Dictionary<HTBAntColonyTSP.Road, Road>));
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
            File.AppendAllText(fname, _cityMap == null ? "_cityMap is null" : "_cityMap is NOT null COUNT " + _cityMap.Count + Environment.NewLine + Environment.NewLine);

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