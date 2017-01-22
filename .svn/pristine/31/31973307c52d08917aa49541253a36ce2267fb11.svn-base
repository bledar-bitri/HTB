using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace HTBAntColonyTSP
{
	public class World
	{
	    private const int NumberOfAnts = 200;
	    private const double InitialPheromoneValue = 1;
	    private const double PheromoneDecayFactor = 0.1;
	    private const int MaxIterationsWithoutChange = 20;
	    public const int MinimumCitiesNeededToCalculateRoute = 4;
	    public static int MaxOptimationTime = 10*60*1000;
		
		public delegate void UpdateEventHandler(World sender, UpdateEventArgs e);
		private UpdateEventHandler _updateEvent;
        public event UpdateEventHandler Update
        {
            add
            {
                _updateEvent = (UpdateEventHandler)Delegate.Combine(_updateEvent, value);
            }
            remove
            {
                _updateEvent = (UpdateEventHandler)Delegate.Remove(_updateEvent, value);
            }
        }
		
		private static readonly Random Rndom = new Random();
		private readonly List<TspCity> _cities;
		private readonly List<Road> _roads;
		private readonly double _worstTourValue;
		private readonly double _weightingFactor;

        private List<ITspProgressListener> _listeners = new List<ITspProgressListener>();


		public IList<TspCity> Cities
		{
			get
			{
				return _cities;
			}
		}
		
		public IEnumerable<Road> Roads
		{
			get
			{
				return _roads;
			}
		}

	    public int UserID = -1;
	    public long MaxTravelTime { get; set; }

		public World(WorldBuilder prototype) : this(prototype.Cities, prototype.Roads)
		{
		}
		
		public World(IEnumerable<TspCity> cities, IEnumerable<Road> roads)
		{
			_cities = new List<TspCity>(cities);
		    _roads = new List<Road>(roads);
		    _worstTourValue = Convert.ToDouble((from road in roads select road).Sum(road => road.Distance));
            if(Cities.Count > 0 && Roads.Count() > 0)
		        _weightingFactor = Cities.Count / Roads.Count();
		}
		
		public List<TspCity> FindTour(int startIndex = -1, bool returnToStart = true)
		{

            if (Cities.Count < MinimumCitiesNeededToCalculateRoute)
            {
                if (startIndex < 0)
                    return Cities.ToList();

                var tour = Cities.Where((t, i) => i == startIndex).ToList();
                tour.AddRange(Cities.Where((t, i) => i != startIndex).ToList());
                return tour;
            }



		    List<TspCity> bestTour = null;
		    var bestTourValue = _worstTourValue;
			var iterations = 0;
			var iterationsWithoutChange = 0;
			var numberOfFailures = 0;
		    double lastBestTourValue = -1;

		    foreach (var road in Roads)
			{
				road.PheromoneLevel = Convert.ToDouble(InitialPheromoneValue);
			}
			
			const double antPheromoneCapacity = 0.2;
			var overallDecayValue = PheromoneDecayFactor * InitialPheromoneValue * _roads.Count;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
			while (iterationsWithoutChange < MaxIterationsWithoutChange)
			{
			    iterations++;
				var ants = new List<Ant>(NumberOfAnts);
				var antSuccess = new Dictionary<Ant, bool>(NumberOfAnts);
				
				for (var i = 1; i <= NumberOfAnts; i++)
				{
					// We have to pick a random starting city (IF NO STARTING CITY IS PASSED) in order to distribute
					// the pheromones for the last route of a tour randomly!
                    if(startIndex == -1)
					    startIndex = Rndom.Next(Cities.Count);
					ants.Add(new Ant(Cities[startIndex], antPheromoneCapacity));
				}
				
				double lastValue = 0;
				int numSuccess = 0;
				
				foreach (var ant in ants)
				{
					var success = ant.SearchTour(MaxTravelTime, returnToStart) &&
                        (ant.VisitedCities.Count == _cities.Count || (!returnToStart && ant.VisitedCities.Count == _cities.Count - 1));
					antSuccess[ant] = success;
					
					if (success)
					{
						// We use a delta to compensate mathematical instabilities in
						// floating point operations.
						var delta = ant.TourValue - bestTourValue;
						
						// We're talking about km here! 0.5 is small enough (pixels would be 0.01).
						if (Math.Abs(delta) <= 0.5)
						{
							iterationsWithoutChange++;
						}
						else if (delta < 0)
						{
							bestTourValue = ant.TourValue;
							bestTour = ant.VisitedCities;
							iterationsWithoutChange = 0;
						}
						else if (delta <= bestTourValue * 0.01)
						{
							// The difference is too small to yield correct phermone
							// values (that is: the tour is nearly as good as the
							// current best one): We give up.
							iterationsWithoutChange++;
						}
						else
						{
							iterationsWithoutChange = 0;
						}
						
						lastValue += ant.TourValue;
						numSuccess++;
					}
					else
					{
						iterationsWithoutChange = 0;
						numberOfFailures++;
					}
				}
				
				lastValue /= numSuccess;
				
				foreach (var road in Roads)
				{
					// No decay for the currently best tour!
					// This is an extension to the algorithm to ensure its termination.
					
					var roadIsInBestTour = false;
					
					if (bestTour != null && bestTour.Count > 0)
					{
						var first = bestTour[0];
						
						for (var i = 1; i <= bestTour.Count - 1; i++)
						{
							if (first.Roads(bestTour[i]) == road)
							{
								roadIsInBestTour = true;
								break;
							}
							
							first = bestTour[i];
						}
						
						if (bestTour[bestTour.Count - 1].Roads(bestTour[0]) == road)
						{
							roadIsInBestTour = true;
						}
					}
					
					if (! roadIsInBestTour)
					{
						UpdatePheromoneLevel(road);
					}
				}
				
				var individualPheromoneLevel = overallDecayValue;
				
				foreach (var annotatedAnt in antSuccess)
				{
					if (annotatedAnt.Value)
					{
						annotatedAnt.Key.Pheromones = individualPheromoneLevel;
						var cities = annotatedAnt.Key.VisitedCities;
						var tourBonus = TourPheromoneBonus(annotatedAnt.Key);
						for (var i = 1; i <= Cities.Count - 1; i++)
						{
							var road = cities[i - 1].Roads(cities[i]);
							road.PheromoneLevel += tourBonus;
						}
						
						var lastRoad = cities[Cities.Count - 1].Roads(cities[0]);
						lastRoad.PheromoneLevel += tourBonus;
					}
				}

                RaiseUpdate(new UpdateEventArgs(iterations, iterationsWithoutChange, numberOfFailures, bestTourValue, lastValue, bestTour, UserID));
			    FileProgressEvent(stopwatch.Elapsed, bestTourValue);

                /* restart stopwatch the first pass */
                if (lastBestTourValue < 0)
                {
                    lastBestTourValue = bestTourValue;
                    stopwatch.Restart();
                }
//                File.AppendAllText("C:/temp/WorldLog.txt", string.Format("Total Run time: {0} of {1}" + Environment.NewLine, 
//                    stopwatch.ElapsedMilliseconds,
//                    MaxOptimationTime));
			    if(lastBestTourValue > bestTourValue) 
                {
                    // found new best tour, restart the stopwatch
//                    File.AppendAllText("C:/temp/WorldLog.txt", 
//                        String.Format("New best tour: from:{0} TO: {1} Restarting Time from: {2}"+Environment.NewLine, 
//                        lastBestTourValue.ToString("N2"), 
//                        bestTourValue.ToString("N2"), 
//                        stopwatch.ElapsedMilliseconds));
                    stopwatch.Restart();
                    lastBestTourValue = bestTourValue;  
                }
                else if (stopwatch.ElapsedMilliseconds >= MaxOptimationTime)
                {
                    // if we search without change for a set amount of time [MaxOptimationTime] break the loop
//                    File.AppendAllText("C:/temp/WorldLog.txt", "STOPING LOOP DONE!");
                    iterationsWithoutChange = MaxIterationsWithoutChange; // break the loop and get out of here
                }   
			}
			return bestTour;
		}
		
		private void RaiseUpdate(UpdateEventArgs args)
		{
			if (_updateEvent != null)
				_updateEvent(this, args);
		}
		
		private void UpdatePheromoneLevel(Road road)
		{
			const double remainingPheromoneFactor = 1.0 - PheromoneDecayFactor;
			road.PheromoneLevel = road.PheromoneLevel * remainingPheromoneFactor;
		}
		
		private double TourPheromoneBonus(Ant ant)
		{
			// We penalize long tours and try to get the worst possible tour
			// to yield 0 pheromone.
			// The square tries to "stretch" the range of possible bonuses.
			return Math.Pow((ant.Pheromones * (_worstTourValue / ant.TourValue - 1)), 2);
		}
		
        public void AddTspProgressListener(ITspProgressListener l)
        {
            _listeners.Add(l);
        }
        public void RemoveTspProgressListener(ITspProgressListener l)
        {
            _listeners.Remove(l);
        }
        private void FileProgressEvent(TimeSpan ts, double bestValue)
        {
            foreach (ITspProgressListener l in _listeners)
            {
                l.SetBestTourValue(bestValue);
                l.SetTotalTime(ts);
            }
        }
	}
	
}
