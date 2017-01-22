using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System;

namespace HTBWorldView
{
    [DebuggerDisplay("City {Name}")]
    public class City
    {


        public const int Radius = 6;

        private readonly Point m_Location;
        private readonly string m_Name;
        private HTBAntColonyTSP.TspCity _mTspTspCity;

        public Point Location
        {
            get
            {
                return m_Location;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public HTBAntColonyTSP.TspCity TspTspCity
        {
            get
            {
                return _mTspTspCity;
            }
            set
            {
                _mTspTspCity = value;
            }
        }

        public City(Point location, string name, HTBAntColonyTSP.TspCity tspTspCity)
        {
            m_Location = location;
            m_Name = name;
            _mTspTspCity = tspTspCity;
        }

        public static double Distance(Point pt1, Point pt2)
        {
            return Math.Sqrt(Convert.ToDouble(Math.Pow((pt1.X - pt2.X), 2) + Math.Pow((pt1.Y - pt2.Y), 2)));
        }

        public static double Distance(KeyValuePair<Point, Point> line)
        {
            return Distance(line.Key, line.Value);
        }

    }

}
