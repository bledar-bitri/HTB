using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MapPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            //Program p = new Program();
            //p.TestFindLocation();
            //p.TestAddPPWaypoints();
            //p.CalculateRoute();
            //new MapPointServer();
        }

        public void TestFindLocation()
        {
            MapPoint.Application app = new MapPoint.Application();
            MapPoint.Map map = app.ActiveMap;
            MapPoint.FindResults findResults = map.FindPlaceResults("Salzburg");

            //Create an object to hold index value
            object index = null;
            //Start with index 1 instead of 0
            for (int i = 1; i <= findResults.Count; i++)
            {
                //Box the integer value as an object
                index = i;
                //Pass the index as a reference to the get_Item accessor method
                MapPoint.Location loc = findResults.get_Item(ref index) as MapPoint.Location;
                if (loc != null)
                    Console.WriteLine(loc.Name);
            }

            Console.Read();
        }

        void TestAddPPWaypoints()
        {
            //set up application
            MapPoint.Application objApp = new MapPoint.Application();
            MapPoint.Route objRoute;
            MapPoint.Map objMap;
            MapPoint.Location objLoc;

            objApp.Visible = true;
            objApp.UserControl = true;
            objMap = objApp.ActiveMap;
            objRoute = objMap.ActiveRoute;

            // Ensure we're working in miles
            objApp.Units = MapPoint.GeoUnits.geoMiles;

            string strDB = objApp.Path +
               @"\Samples\Clients.MDB!AddressQuery";

            // Add all local addresses as waypoints
            MapPoint.DataSet dsDataSet;
            object missing = System.Type.Missing;
            dsDataSet = objMap.DataSets.ImportData(
               strDB, missing, MapPoint.GeoCountry.geoCountryAustria,
               MapPoint.GeoDelimiter.geoDelimiterDefault,
               0);

            Object obj = 1;
            objLoc = (MapPoint.Location)objMap.FindResults("Salzburg").get_Item(ref obj);

            // Find all clients within 300 miles of Creve Coeur
            MapPoint.Recordset rsDataSet;
            rsDataSet = dsDataSet.QueryCircle(objLoc, 300);

            // Add those addresses as Waypoints
            MapPoint.Waypoints objWayPoints;
            objWayPoints = objMap.ActiveRoute.Waypoints;
            rsDataSet.MoveFirst();
            while (!rsDataSet.EOF)
            {
                objWayPoints.Add(rsDataSet.Pushpin, "");
                rsDataSet.MoveNext();
            }

            //calulate route
            objMap.ActiveRoute.Calculate();

            //how many did we get?
            Console.WriteLine(objRoute.Waypoints.Count.ToString(), "How Many?");
        }

        void CalculateRoute()
        {
            //set up application

            MapPoint.Application objApp = new MapPoint.Application();
            MapPoint.Map objMap = null;
            //objApp.Visible = true;
            objApp.Visible = false;
            //objApp.UserControl = true;
            objApp.UserControl = false;
            objMap = objApp.ActiveMap;

            objMap.Parent.PaneState = MapPoint.GeoPaneState.geoPaneRoutePlanner;

            //Zoom in on the area
            Object obj = 1;
            Object obj3 = 3;
            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults("Neudastrasse 5, 3381, Austria").get_Item(ref obj), "");
            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults("Feltmuellerstrasse 5, 3370, Austria").get_Item(ref obj), "");
            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults("Bachgasse 1, 3386, Austria").get_Item(ref obj), "");
            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults("Jubilaeumsstrasse 1, 3386, Austria").get_Item(ref obj), "");
            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults("Loosdorferstrasse 51, 3382, Austria").get_Item(ref obj), "");
            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults("Alterradhausplatz 18, 3382, Austria").get_Item(ref obj), "");
            objMap.ActiveRoute.Waypoints.Optimize();
            objMap.ActiveRoute.Calculate();
            Thread.Sleep(1000);

            //Create an object to hold index value
            object index = null;
            //Start with index 1 instead of 0
            Console.WriteLine("1");
            for (int i = 1; i <= objMap.ActiveRoute.Waypoints.Count; i++)
            {
                //Box the integer value as an object
                index = i;
                //Pass the index as a reference to the get_Item accessor method
                MapPoint.Waypoint loc = objMap.ActiveRoute.Waypoints.get_Item(ref index) as MapPoint.Waypoint;
                if (loc != null)
                    Console.WriteLine(loc.Name);
            }
            Console.Read();
        }
    }
}
