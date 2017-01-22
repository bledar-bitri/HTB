using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using HTB.v2.intranetx.progress;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.routeplanter
{
    [Serializable]
    public class RoutePlanerManagerAutomatic : RoutePlanerManager
    {
        public static readonly int MaxAddressesPerRoute = Convert.ToInt32(HTBUtils.GetConfigValue("MaximumAddressesPerRoute"));
        public static readonly string DefaultRouteName = HTBUtils.GetConfigValue("Default_Route_Name");
        

        #region Constructors
        public RoutePlanerManagerAutomatic(int userId, TaskStatus tskStatus): base(userId, tskStatus)
        {
            
        }
        #endregion

        public void RunAutomatic(int routeUser, DateTime departureTime, long tripDuration)
        {
            RunAutomatic(routeUser, departureTime, tripDuration, DefaultRouteName);
        }
        public void RunAutomatic(int routeUser, DateTime departureTime, long tripDuration, string routeName)
        {
            try
            {
                IsRouteCalculationDone = false;
                FirstAppointmentTime = departureTime;
                MaxTravelTime = tripDuration;
                if (Addresses.Count <= 0)
                    return;
                UpdateProgressStatus(10, "Loading Addresses");
                LoadGeocodeAddresses(true);
//            Map.DebugInfo("Addresses count "+Addresses.Count);
                if (BadAddresses.Count == 0)
                {
                    UpdateProgressStatus(20, "Loading Distances");
                    LoadDistances(null, true);
                    UpdateProgressStatus(40, "Saving Distances");
                    SaveDistances();
                    UpdateProgressStatus(60, "Removing Zero-Length Roads");
                    RemoveZeroLengthRoads();
                    UpdateProgressStatus(70, "Getting addresses closest to you");

                    var tour = new List<Road>();
                    City startCity = GetStartingPoint();
                    AddNearestNeighbourTour(startCity, startCity, new List<City>(), tour, 0);
                    UpdateProgressStatus(80, string.Format("Tour Calculated [Road Count: {0}] ", tour.Count));
                    AddTourCities(tour);
                    if (Cities.Count > 4)
                    {
                        ReCalculateRoute();
                    }
                    if (RouteUser > 0)
                    {
                        string routeFilePath = GetRouteFilePath(RouteUser, routeName);
                        FileSerializer<RoutePlanerManager>.Serialize(routeFilePath, this);
                    }
                }
            }
            catch( Exception e) {
                UpdateProgressStatus(100, e.Message);
                UpdateProgressStatus(100, e.StackTrace);
        }
            UpdateProgressStatus(100, "DONE!");
            IsRouteCalculationDone = true;
        }

        private void ReCalculateRoute()
        {
            UpdateProgressStatus(20, "Loading Distances 2");
            LoadDistances(null, true);
            UpdateProgressStatus(40, "Saving Distances 2");
            SaveDistances();
            UpdateProgressStatus(60, "Removing Zero-Length Roads 2");
            RemoveZeroLengthRoads();
            UpdateProgressStatus(80, "Calculating Tour");
            CalcBestTourUsingThread(true);
            
        }

        public City GetStartingPoint()
        {
            return Cities.Where(t => t.Address.Equals(Addresses[0])).FirstOrDefault();
        }

        #region Nearest Neighbour Tour
        protected void AddNearestNeighbourTour(City startCity, City currentCity, List<City> visitedCities, List<Road> tour, long travelTime)
        {
            UpdateProgressStatus(80, string.Format("Calculating Tour [Traveled: {0} of {1}] ", travelTime, MaxTravelTime));
            var backToStartRoad = Roads.Where(r => (r.From.Equals(startCity) && r.To.Equals(currentCity)) || (r.To.Equals(startCity) && r.From.Equals(currentCity))).FirstOrDefault();
            long backToStartTravelTime = 0;

            if (backToStartRoad != null)
                backToStartTravelTime = backToStartRoad.TravelTimeInSeconds;
            if (travelTime + backToStartTravelTime >= MaxTravelTime)
                return;
            
            if(tour.Count == 1)
                travelTime = 0; // throw away the time it takes to get to the first address
            
            visitedCities.Add(currentCity);
            
            var list = Roads.Where(r => r.From.Address.Equals(currentCity.Address) || r.To.Address.Equals(currentCity.Address)).ToList();
            UpdateStatusAndSleep(string.Format("Total Connections: {0} Of {1} ", list.Count, Roads.Count));
            list.Sort((r1, r2) => r1.Distance.CompareTo(r2.Distance));
            foreach (var road in list)
            {
                if (!tour.Any(road1 => road1.Equals(road)))
                {
                    bool addToList = true;
                    if (!road.From.Equals(currentCity) && !road.To.Equals(currentCity))
                    {
                        addToList = false;
                    }
                    else if (road.From.Equals(currentCity) && visitedCities.Contains(road.To))
                    {
                        addToList = false;
                    }
                    else if (road.To.Equals(currentCity) && visitedCities.Contains(road.From))
                    {
                        addToList = false;
                    }
                    
                    if (addToList)
                    {
                        UpdateStatusAndSleep(string.Format("Adding Road To Tour From: {0} To {1} ", road.From.Address.Address, road.To.Address.Address));
                        travelTime += road.TravelTimeInSeconds;
                        tour.Add(road);
                        City destination = road.From.Equals(currentCity) ? road.To : road.From;
                        backToStartRoad = Roads.Where(r => (r.From.Equals(startCity) && r.To.Equals(destination)) || (r.To.Equals(startCity) && r.From.Equals(destination))).FirstOrDefault();
                        backToStartTravelTime = 0;

                        // check if we can go to the destination and back within the time allocated
                        if (backToStartRoad != null)
                            backToStartTravelTime = backToStartRoad.TravelTimeInSeconds;
                        if(tour.Count > 0 && travelTime + backToStartTravelTime >= MaxTravelTime)
                            return;
                        UpdateStatusAndSleep("Recursive Call");
                        AddNearestNeighbourTour(startCity, destination, visitedCities, tour, travelTime + ADGegnerStopMinutes * 60);
                        UpdateStatusAndSleep("Outside Recursive Call");
                        break;
                    }
                }
            }
        }

        private void UpdateStatusAndSleep(String msg)
        {
            UpdateProgressStatus(80, msg);
//            Thread.Sleep(10000);
        }
        protected void AddTourCities(List<Road> tour)
        {
            Cities.Clear();
            foreach (var road in tour)
            {
                if (!ExistsInCities(road.From))
                    Cities.Add(road.From);
                if (!ExistsInCities(road.To))
                    Cities.Add(road.To);
            }
        }
        #endregion

        #region ISerializable
        protected RoutePlanerManagerAutomatic(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        #endregion

        #region Test
        public void Test()
        {
            IsRouteCalculationDone = false;
            FirstAppointmentTime = DateTime.Now;
            MaxTravelTime = 8 * 60 * 60 * 1000; // * hours

//            UpdateProgressStatus(20, "Loading Distances");
//            LoadDistances(null, true);
//            UpdateProgressStatus(40, "Saving Distances");
//            SaveDistances();
            UpdateProgressStatus(60, "Removing Zero-Length Roads");
            RemoveZeroLengthRoads();
            UpdateProgressStatus(70, "Getting addresses closest to you");

            var tour = new List<Road>();
            City startCity = GetStartingPoint();
            AddNearestNeighbourTour(startCity, startCity, new List<City>(), tour, 0);
            UpdateProgressStatus(80, string.Format("Tour Calculated [Road Count: {0}] ", tour.Count));
            AddTourCities(tour);
            if (Cities.Count > 4)
            {
                ReCalculateRoute();
            }
            
            UpdateProgressStatus(100, "DONE!");
            IsRouteCalculationDone = true;
        }

        #endregion

    }

}