using System;
using System.Collections.Generic;

namespace TspAntColony
{
    
    public class World
    {

        public const double α = -1.5;
        public const double β = 1.5;
        public const int NumberOfAnts = 100;
        public const double InitialPheromoneValue = 1;
        public const double PheromoneDecayFactor = 0.1;

        public const int MaxIterations = 20;
        public event UpdateEventHandler Update;
        public delegate void UpdateEventHandler(World sender, UpdateEventArgs e);

        private static readonly Random m_Random = new Random();
        private readonly List<City> m_Cities;
        private readonly List<Road> m_Roads;
        private readonly double m_WorstTourValue;

        private readonly double m_WeightingFactor;
        public IList<City> Cities
        {
            get { return m_Cities; }
        }

        public IEnumerable<Road> Roads
        {
            get { return m_Roads; }
        }

        public World(WorldBuilder prototype)
            : this(prototype.Cities, prototype.Roads)
        {
        }

        public World(IEnumerable<City> cities, IEnumerable<Road> roads)
        {
            m_Cities = new List<City>(cities);
            m_Roads = new List<Road>(roads);
            m_WorstTourValue = roadsSum(road.Distance);
            m_WeightingFactor = cities.Count / roads.Count;
        }

        public IEnumerable<City> FindTour()
        {
            IList<City> best_tour = null;
            dynamic best_tour_value = m_WorstTourValue;
            dynamic iterations = 0;
            dynamic iterations_without_change = 0;
            dynamic number_of_failures = 0;
            double last_value = 0;
            int num_success = 0;

            foreach (object road_loopVariable in Roads)
            {
                road = road_loopVariable;
                road.PheromoneLevel = InitialPheromoneValue;
            }

            dynamic ant_pheromone_capacity = 0.2;
            dynamic overall_decay_value = PheromoneDecayFactor * InitialPheromoneValue * m_Roads.Count;

            while (iterations_without_change < MaxIterations)
            {
                iterations += 1;
                List<Ant> ants = new List<Ant>(NumberOfAnts);
                Dictionary<Ant, bool> ant_success = new Dictionary<Ant, bool>(NumberOfAnts);

                for (i = 1; i <= NumberOfAnts; i++)
                {
                    // We have to pick a random starting city in order to distribute
                    // the pheromones for the last route of a tour randomly!
                    dynamic rnd_index = m_Random.Next(Cities.Count);
                    ants.Add(new Ant(Cities[rnd_index], ant_pheromone_capacity));
                }

                last_value = 0;
                num_success = 0;

                foreach (object ant_loopVariable in ants)
                {
                    ant = ant_loopVariable;
                    dynamic success = ant.SearchTour() && ant.VisitedCities.Count == m_Cities.Count;
                    ant_success[ant] = success;

                    if (success)
                    {
                        // We use a delta to compensate mathematical instabilities in
                        // floating point operations.
                        dynamic delta = ant.TourValue - best_tour_value;

                        // We're talking about pixels here! 0.01 is small enough.
                        if (Math.Abs(delta) <= 0.01)
                        {
                            iterations_without_change += 1;
                        }
                        else if (delta < 0)
                        {
                            best_tour_value = ant.TourValue;
                            best_tour = ant.VisitedCities;
                            iterations_without_change = 0;
                        }
                        else if (delta <= best_tour_value * 0.01)
                        {
                            // The difference is too small to yield correct phermone
                            // values (that is: the tour is nearly as good as the
                            // current best one): We give up.
                            iterations_without_change += 1;
                        }
                        else
                        {
                            iterations_without_change = 0;
                        }

                        last_value += ant.TourValue;
                        num_success += 1;
                    }
                    else
                    {
                        iterations_without_change = 0;
                        number_of_failures += 1;
                    }
                }

                last_value /= num_success;

                foreach (object road_loopVariable in Roads)
                {
                    road = road_loopVariable;
                    // No decay for the currently best tour!
                    // This is an extension to the algorithm to ensure its termination.

                    dynamic road_is_in_best_tour = false;

                    if (best_tour != null && best_tour.Count > 0)
                    {
                        dynamic first = best_tour[0];

                        for (i = 1; i <= best_tour.Count - 1; i++)
                        {
                            if (object.ReferenceEquals(first.Roads(best_tour[i]), road))
                            {
                                road_is_in_best_tour = true;
                                break; // TODO: might not be correct. Was : Exit For
                            }

                            first = best_tour[i];
                        }

                        if (object.ReferenceEquals(best_tour[best_tour.Count - 1].Roads(best_tour[0]), road))
                        {
                            road_is_in_best_tour = true;
                        }
                    }

                    if (!road_is_in_best_tour)
                    {
                        UpdatePheromoneLevel(road);
                    }
                }

                dynamic individual_pheromone_level = overall_decay_value;

                foreach (object annotated_ant_loopVariable in ant_success)
                {
                    annotated_ant = annotated_ant_loopVariable;
                    if (annotated_ant.Value)
                    {
                        annotated_ant.Key.Pheromones = individual_pheromone_level;
                        dynamic cities = annotated_ant.Key.VisitedCities;
                        dynamic tour_bonus = TourPheromoneBonus(annotated_ant.Key);
                        for (i = 1; i <= cities.Count - 1; i++)
                        {
                            dynamic road = cities(i - 1).Roads(cities(i));
                            road.PheromoneLevel += tour_bonus;
                        }

                        dynamic last_road = cities(cities.Count - 1).Roads(cities(0));
                        last_road.PheromoneLevel += tour_bonus;
                    }
                }

                RaiseUpdate(new UpdateEventArgs(iterations, iterations_without_change, number_of_failures, best_tour_value, last_value, best_tour));
            }

            return best_tour;
        }

        private void RaiseUpdate(UpdateEventArgs args)
        {
            if (Update != null)
            {
                Update(this, args);
            }
        }

        private void UpdatePheromoneLevel(Road road)
        {
            const double RemainingPheromoneFactor = 1.0 - PheromoneDecayFactor;
            road.PheromoneLevel = road.PheromoneLevel * RemainingPheromoneFactor;
        }

        private double TourPheromoneBonus(Ant ant)
        {
            // We penalize long tours and try to get the worst possible tour
            // to yield 0 pheromone.
            // The square tries to "stretch" the range of possible bonuses.
            return Math.Pow((ant.Pheromones * (m_WorstTourValue / ant.TourValue - 1)), 2);
        }

    }

}
