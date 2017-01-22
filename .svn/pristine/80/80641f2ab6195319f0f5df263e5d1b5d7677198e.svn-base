using System;
using System.Collections.Generic;

namespace TspAntColony
{
    
    public class Ant
    {

        private static readonly Random m_Random = new Random();
        private double m_Pheromones;
        private readonly List<City> m_VisitedCities;
        private City m_CurrentPosition;

        private double m_TourValue;
        private static Random Random
        {
            get { return m_Random; }
        }

        public IList<City> VisitedCities
        {
            get { return m_VisitedCities; }
        }

        public City CurrentPosition
        {
            get { return m_CurrentPosition; }
            private set { m_CurrentPosition = value; }
        }

        public double Pheromones
        {
            get { return m_Pheromones; }
            set { m_Pheromones = value; }
        }

        public double TourValue
        {
            get { return m_TourValue; }
            private set { m_TourValue = value; }
        }

        public Ant(City start_position, double pheromones)
        {
            m_CurrentPosition = start_position;
            m_Pheromones = pheromones;
            m_VisitedCities = new List<City>();
            VisitedCities.Add(start_position);
        }

        public bool SearchTour()
        {
            TourValue = 0.0;

            while (TravelOn())
            {
            }

            dynamic closing_road = CurrentPosition.Roads(VisitedCities.First());
            if (closing_road != null)
            {
                TourValue += closing_road.Distance;
                return true;
            }

            return false;
        }

        private bool TravelOn()
        {
            dynamic next_city = GetNextCity();
            if (next_city == null)
            {
                return false;
            }

            CurrentPosition = next_city;
            TourValue += VisitedCities.Last().Roads(CurrentPosition).Distance;
            VisitedCities.Add(CurrentPosition);
            return true;
        }

        private City GetNextCity()
        {
            Dictionary<City, double> city_weights = new Dictionary<City, double>();
            dynamic sum_of_weights = 0.0;

            foreach (object city_loopVariable in CurrentPosition.NeighbourCities)
            {
                city = city_loopVariable;
                if (!VisitedCities.Contains(city))
                {
                    dynamic weight = CurrentPosition.Roads(city).WeighedValue;
                    city_weights.Add(city, weight);
                    sum_of_weights += weight;
                }
            }

            dynamic rnd = Random.NextDouble();
            dynamic sum = 0.0;

            foreach (object pair_loopVariable in city_weights)
            {
                pair = pair_loopVariable;
                sum += pair.Value / sum_of_weights;
                if (sum >= rnd)
                {
                    return pair.Key;
                }
            }

            return null;
        }
    }

}
