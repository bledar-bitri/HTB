using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;
using System.Windows.Forms;
using HTBAntColonyTSP;

namespace HTBWorldView
{
    public partial class Map
    {

        private PictureBox m_Display;
        private Bitmap m_Bitmap;
        private Image m_BackgroundPicture;
        private readonly List<City> m_Cities = new List<City>();
        private readonly List<Pair<City, City>> m_Roads = new List<Pair<City, City>>();
        private Dictionary<HTBAntColonyTSP.TspCity, City> m_CityMap = new Dictionary<HTBAntColonyTSP.TspCity, City>();
        private Dictionary<Road, Pair<City, City>> m_RoadMap;
        private bool m_ShowLabels;
        private bool m_ReturnToStart;
        public Map(PictureBox display)
        {
            m_Display = display;
            m_Display.Resize += new System.EventHandler(Display_Resize);
            m_Display.Paint += new System.Windows.Forms.PaintEventHandler(Display_Paint);
            m_Bitmap = new Bitmap(display.Width, display.Height);
            m_ReturnToStart = true;
        }

        public bool ShowLabels
        {
            get
            {
                return m_ShowLabels;
            }
            set
            {
                m_ShowLabels = value;
                Redraw();
            }
        }

        public Image BackgroundPicture
        {
            get
            {
                return m_BackgroundPicture;
            }
            set
            {
                m_BackgroundPicture = value;
                Redraw();
            }
        }

        public int CityCount
        {
            get
            {
                return m_Cities.Count;
            }
        }

        public bool ReturnToStart
        {
            get { return m_ReturnToStart; }
            set { m_ReturnToStart = value; }
        }

        public void AddCity(Point location)
        {
            City city = new City(location, NameFromLocation(location), null);

            foreach (var c in m_Cities)
            {
                m_Roads.Add(new Pair<City, City>(city, c));
            }

            m_Cities.Add(city);
            Redraw();
        }

        public City FindCity(Point location)
        {
            return m_Cities.Find(c => City.Distance(location, c.Location) <= City.Radius * 2);
        }

        public void RemoveCity(Point location)
        {
            var city = FindCity(location);
            m_Cities.Remove(city);

            m_Roads.RemoveAll(road => road.First == city || road.Second == city);
            Redraw();
        }

        public void Clear()
        {
            m_Cities.Clear();
            m_Roads.Clear();
            Redraw();
        }

        public World ConstructTsp()
        {
            var wb = new WorldBuilder();

            m_CityMap.Clear();
            m_CityMap = new Dictionary<HTBAntColonyTSP.TspCity, City>(m_Cities.Count);

            foreach (var c in m_Cities)
            {
                c.TspTspCity = wb.AddCity((string)c.Name);
                m_CityMap.Add(c.TspTspCity, c);
            }

            m_RoadMap = new Dictionary<Road, Pair<City, City>>(System.Convert.ToInt32(Math.Pow(m_Cities.Count, 2)));

            foreach (var road in m_Roads)
            {
                m_RoadMap.Add(wb.AddRoad(City.Distance(road.First.Location, road.Second.Location), 0, road.First.TspTspCity, road.Second.TspTspCity), road);
            }

            return new World(wb);
        }

        private static string NameFromLocation(Point location)
        {
            return location.ToString();
        }

        public void DrawBestTour(IEnumerable<TspCity> tour)
        {
            if (tour == null)
            {
                return;
            }

            using (var g = Graphics.FromImage(m_Bitmap))
            {
                DrawTour(tour, g, Color.Red);
            }

            m_Display.Invalidate();
        }

        public void Redraw(World world = null, UpdateEventArgs e = null)
        {
            using (var g = Graphics.FromImage(m_Bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                //
                // Draw the background picture.
                //

                if (m_BackgroundPicture == null)
                {
                    g.Clear(Color.White);
                }
                else
                {
                    g.DrawImage(m_BackgroundPicture, 0, 0, m_Bitmap.Width, m_Bitmap.Height);
                }

                //
                // Draw the roads.
                //

                if (world == null)
                {
                    // We're not currently running the heuristic: Draw standard roads.
                    using (Pen p = new Pen(Color.FromArgb(26, Color.Blue), 2))
                    {
                        foreach (var road in m_Roads)
                        {
                            g.DrawLine(p, road.First.Location, road.Second.Location);
                        }
                    }

                }
                else
                {
                    // While running the heuristic, we don't draw the usual roads.
                    // Instead, we draw the roads according to their pheromone level.

                    var sum_of_pheromones = (from road in world.Roads select road).Sum(road => road.PheromoneLevel);
                    var factor = 255 * world.Roads.Count();

                    foreach (var road in world.Roads)
                    {
                        var line = m_RoadMap[road];
                        var alpha = Math.Min(Math.Max(System.Convert.ToInt32(road.PheromoneLevel / sum_of_pheromones * factor), 0), 255);
                        using (Pen p = new Pen(Color.FromArgb(alpha, Color.Blue), 2))
                        {
                            g.DrawLine(p, line.First.Location, line.Second.Location);
                        }

                    }
                }

                //
                // Draw the cities.
                //

                using (Pen p = new Pen(Color.Black, City.Radius / 3))
                {
                    foreach (var c in m_Cities)
                    {
                        g.DrawEllipse(p, System.Convert.ToSingle(c.Location.X - City.Radius), System.Convert.ToSingle(c.Location.Y - City.Radius), System.Convert.ToSingle(City.Radius * 2), System.Convert.ToSingle(City.Radius * 2));
                    }
                }


                //
                // Draw the labels.
                //

                if (ShowLabels)
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(128, Color.Blue)))
                    {
                        foreach (var road in m_Roads)
                        {
                            PointF text_pos = new PointF((road.First.Location.X + road.Second.Location.X) / 2.0F, (road.First.Location.Y + road.Second.Location.Y) / 2.0F);
                            g.DrawString((string)(City.Distance(road.First.Location, road.Second.Location).ToString("0")), m_Display.Font, b, text_pos);
                        }
                    }


                    using (SolidBrush b = new SolidBrush(Color.Black))
                    {
                        foreach (var c in m_Cities)
                        {
                            var text_pos = c.Location;
                            text_pos.Offset(City.Radius + 5, 0);
                            g.DrawString((string)c.Name, m_Display.Font, b, text_pos);
                        }
                    }

                }

                //
                // If we're running the heuristic, draw the currently best tour.
                //

                if (e != null)
                {
                    if (e.BestTour != null)
                    {
                        DrawTour(e.BestTour, g, Color.FromArgb(192, Color.DarkGreen));
                    }
                }
            }


            m_Display.Invalidate();
        }

        private void DrawTour(IEnumerable<TspCity> tour, Graphics g, Color color)
        {
            using (Pen p = new Pen(color, 2))
            {
                var i = tour.GetEnumerator();
                i.MoveNext();
                var first = m_CityMap[i.Current];
                var c1 = first;

                while (i.MoveNext())
                {
                    var c2 = m_CityMap[i.Current];
                    g.DrawLine(p, c1.Location, c2.Location);
                    c1 = c2;
                }
                if(m_ReturnToStart)
                    g.DrawLine(p, c1.Location, first.Location);
            }

        }

        private void Display_Resize(object sender, EventArgs e)
        {
            m_Bitmap = new Bitmap(m_Display.Width, m_Display.Height);
            Redraw();
        }

        private void Display_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(m_Bitmap, 0, 0);
        }

    }

}
