using System.Collections.Generic;

namespace HTBAntColonyTSP
{
	public class WorldBuilder
	{
		
		
		private readonly Dictionary<string, TspCity> _cities = new Dictionary<string, TspCity>();
		private readonly List<Road> _roads = new List<Road>();
		
		internal IEnumerable<TspCity> Cities
		{
			get
			{
				return _cities.Values;
			}
		}
		
		internal IEnumerable<Road> Roads
		{
			get
			{
				return _roads;
			}
		}
		
		public TspCity AddCity(string name)
		{
			TspCity tspCity = new TspCity(name);
			_cities.Add(name, tspCity);
			return tspCity;
		}

		public void AddCity(TspCity tspCity)
		{
			_cities.Add(tspCity.Name, tspCity);
		}
		
		public void AddCities(IEnumerable<string> names)
		{
			foreach (var name in names)
			{
				AddCity(name);
			}
		}
		
		public void AddCities(params string[] names)
		{
			foreach (var name in names)
			{
				AddCity(name);
			}
		}
		
		public void AddCities(IEnumerable<TspCity> cities)
		{
			foreach (var city in cities)
			{
				AddCity(city);
			}
		}
		
		public void AddCities(params TspCity[] tspCities)
		{
			foreach (var city in tspCities)
			{
				AddCity(city);
			}
		}

        public Road AddRoad(double distance, long travelTime, TspCity from, TspCity to)
        {
            Road road = new Road(distance, travelTime);
            from.AddRoad(road, to);
            to.AddRoad(road, from);
            _roads.Add(road);
            return road;
        }

        public Road AddRoad(double distance, long travelTime, string from, string to)
		{
			TspCity fromTspCity;
			
			if (! _cities.TryGetValue(from, out fromTspCity))
			{
				fromTspCity = AddCity(from);
			}
			
			TspCity toTspCity;
			if (! _cities.TryGetValue(to, out toTspCity))
			{
				toTspCity = AddCity(to);
			}
            return AddRoad(distance, travelTime, fromTspCity, toTspCity);
		}
		
	}
	
}
