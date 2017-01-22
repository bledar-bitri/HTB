using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Collections;
//using HTBUtilities;

namespace MapPoint
{
    public class MapPointManager
    {
        public static ArrayList GetCalculatedRoute(string startAddress, string endAddress, ArrayList addressList)
        {
            ArrayList list = new ArrayList();
            //set up application
            MapPoint.Application objApp = new MapPoint.Application();
            MapPoint.Map objMap = null;
            objApp.Visible = true;
            objApp.UserControl = true;
            
            //objApp.Visible = false;
            //objApp.UserControl = true;
            
            objMap = objApp.ActiveMap;

            objMap.Parent.PaneState = MapPoint.GeoPaneState.geoPaneRoutePlanner;

            //Zoom in on the area
            Object obj = 1;
            Object obj3 = 3;
            Console.WriteLine("==================== ADDRESSES =========================");
            ArrayList addedAddressesList = new ArrayList();
            Console.WriteLine(" --> Start: "+startAddress);
            Console.WriteLine(" --> End: " + endAddress);
            Console.WriteLine("====================");
            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults(startAddress).get_Item(ref obj), "");
            foreach (string addr in addressList)
            {
                Console.Write(addr);
                if (addr.Trim() != string.Empty)
                {
                    bool added = false;
                    foreach (string addedAddr in addedAddressesList)
                    {
                        //if (HTBUtils.RemoveAllSpecialChars(addr).Replace(" ", "").ToLower() == HTBUtils.RemoveAllSpecialChars(addedAddr).Replace(" ", "").ToLower())
                        if (MapPointUtils.RemoveAllSpecialChars(addr).Replace(" ", "").ToLower() == MapPointUtils.RemoveAllSpecialChars(addedAddr).Replace(" ", "").ToLower())
                        {
                            added = true;
                            break;
                        }
                    }
                    if (!added)
                    {
                        try
                        {
                            Console.WriteLine(" ADDED");
                            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults(addr).get_Item(ref obj), "");
                        }
                        catch { }
                        addedAddressesList.Add(addr);
                    }
                    else
                    {
                        Console.WriteLine(" SKIPPED");
                    }
                }
            }
            objMap.ActiveRoute.Waypoints.Add(objMap.FindResults(endAddress).get_Item(ref obj), "");
            Console.WriteLine("==================== ADDRESSES END =========================");
            Console.WriteLine("  --> Optimising");
            objMap.ActiveRoute.Waypoints.Optimize();
            Console.WriteLine("  --> Calculating");
            objMap.ActiveRoute.Calculate();
            Console.WriteLine("  --> Generating Return List");
            

            //Create an object to hold index value
            object index = null;
            //Start with index 1 instead of 0
            for (int i = 1; i <= objMap.ActiveRoute.Waypoints.Count; i++)
            {
                //Box the integer value as an object
                index = i;
                //Pass the index as a reference to the get_Item accessor method
                MapPoint.Waypoint loc = objMap.ActiveRoute.Waypoints.get_Item(ref index) as MapPoint.Waypoint;
                if (loc != null)
                    list.Add(loc.Name);
            }
            return list;
        }

        public static ArrayList FindLocations(ArrayList locations)
        {
            ArrayList list = new ArrayList();
            MapPoint.Application app = new MapPoint.Application();
            MapPoint.Map map = app.ActiveMap;

            foreach (string adr in locations)
            {
                MapPoint.FindResults findResults = map.FindPlaceResults(adr);
                
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
                    {
                        list.Add(loc.Name);
                        break;
                    }
                }
            }
            return list;
        }

    }
}