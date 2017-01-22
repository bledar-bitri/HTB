using System.Collections.Generic;
using System;
using System.Linq;

namespace HTBAntColonyTSP
{
	public class Ant
	{
		private static readonly Random _random = new Random();
	    private readonly List<TspCity> _visitedCities;

	    private static Random Random
		{
			get
			{
				return _random;
			}
		}
		
		public List<TspCity> VisitedCities
		{
			get
			{
				return _visitedCities;
			}
		}

	    private TspCity CurrentPosition { get; set; }

	    public double Pheromones { get; set; }

	    public double TourValue { get; private set; }
        public long TimeTraveled { get; private set; }

	    public Ant(TspCity startPosition, double pheromones)
		{
			CurrentPosition = startPosition;
			Pheromones = pheromones;
			_visitedCities = new List<TspCity>();
			VisitedCities.Add(startPosition);
		}
		
		public bool SearchTour(long maxTravelTime, bool returnToStart = true)
		{
			TourValue = 0.0;
		    TimeTraveled = 0;

			while (TravelOn(maxTravelTime))
			{
			}

		    if (!returnToStart)return true;

			var closingRoad = CurrentPosition.Roads(VisitedCities.First());
		    if (closingRoad == null) return false;

		    TourValue += closingRoad.Distance;
		    TimeTraveled += closingRoad.TravelTime;
		    return true;
		}

        private bool TravelOn(long maxTravelTime)
		{
			var nextCity = GetNextCity();
			if (nextCity == null)
			{
				return false;
			}
			
			CurrentPosition = nextCity;
			TourValue += VisitedCities.Last().Roads(CurrentPosition).Distance;
            TimeTraveled += VisitedCities.Last().Roads(CurrentPosition).TravelTime;
//            if(TimeTraveled > maxTravelTime)
//                return false;
			VisitedCities.Add(CurrentPosition);
			return true;
		}
		
		private TspCity GetNextCity()
		{
			var cityWeights = new Dictionary<TspCity, double>();
			var sumOfWeights = 0.0;
			
			foreach (var city in CurrentPosition.NeighbourCities)
			{
				if (! VisitedCities.Contains(city))
				{
					var weight = CurrentPosition.Roads(city).WeighedValue;
					cityWeights.Add(city, weight);
					sumOfWeights += Convert.ToDouble(weight);
				}
			}
			
			var rnd = Random.NextDouble();
			var sum = 0.0;
			
			foreach (var pair in cityWeights)
			{
				sum += Convert.ToDouble(pair.Value / sumOfWeights);
				if (sum >= rnd)
				{
					return pair.Key;
				}
			}
			return null;
		}
	}
	
}
