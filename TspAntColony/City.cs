using System.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace TspAntColony
{
    
    [DebuggerDisplay("City {Name}")]
    public class City
    {


        public const int Radius = 6;
        private readonly Point m_Location;
        private readonly string m_Name;

        public Point Location
        {
            get { return m_Location; }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public AntColonyTSPCity TspCity { get; set; }

        public City(Point location, string name, AntColonyTSPCity tsp_city)
        {
            m_Location = location;
            m_Name = name;
            TspCity = tsp_city;
        }

        public static double Distance(Point pt1, Point pt2)
        {
            return Math.Sqrt(Math.Pow((pt1.X - pt2.X), 2) + Math.Pow((pt1.Y - pt2.Y), 2));
        }

        public static double Distance(KeyValuePair<Point, Point> line)
        {
            return Distance(line.Key, line.Value);
        }

    }

}
