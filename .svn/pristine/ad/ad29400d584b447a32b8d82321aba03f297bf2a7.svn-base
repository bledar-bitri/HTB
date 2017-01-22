using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace TspAntColony
{
    
    public partial class Map
    {

        private PictureBox withEventsField_m_Display;
        private PictureBox m_Display
        {
            get { return withEventsField_m_Display; }
            set
            {
                if (withEventsField_m_Display != null)
                {
                    withEventsField_m_Display.Resize -= Display_Resize;
                    withEventsField_m_Display.Paint -= Display_Paint;
                }
                withEventsField_m_Display = value;
                if (withEventsField_m_Display != null)
                {
                    withEventsField_m_Display.Resize += Display_Resize;
                    withEventsField_m_Display.Paint += Display_Paint;
                }
            }
        }
        private Bitmap m_Bitmap;
        private Image m_BackgroundPicture;
        private readonly List<City> m_Cities = new List<City>();
        private readonly List<Pair<City, City>> m_Roads = new List<Pair<City, City>>();
        private Dictionary<AntColonyTSPCity, City> m_CityMap = new Dictionary<AntColonyTSPCity, City>();
        private Dictionary<Road, Pair<City, City>> m_RoadMap;

        private bool m_ShowLabels;
        public Map(PictureBox display)
        {
            m_Display = display;
            m_Bitmap = new Bitmap(display.Width, display.Height);
        }

        public bool ShowLabels
        {
            get { return m_ShowLabels; }
            set
            {
                m_ShowLabels = value;
                Redraw();
            }
        }

        public Image BackgroundPicture
        {
            get { return m_BackgroundPicture; }
            set
            {
                m_BackgroundPicture = value;
                Redraw();
            }
        }

        public int CityCount
        {
            get { return m_Cities.Count; }
        }

        public void AddCity(Point location)
        {
            var city = new City(location, NameFromLocation(location), null);

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
            dynamic city = FindCity(location);
            m_Cities.Remove(city);

            m_Roads.RemoveAll(road => object.ReferenceEquals(road.First, city) || object.ReferenceEquals(road.Second, city));
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
                c.TspTspCity = wb.AddCity(c.Address.Address);
                m_CityMap.Add(c.TspTspCity, c);
            }

            m_RoadMap = new Dictionary<HTBAntColonyTSP.Road, Road>(Convert.ToInt32(Math.Pow(m_Cities.Count, 2)));

            foreach (var road in m_Roads)
            {
                m_RoadMap.Add(wb.AddRoad(road.Distance, road.TravelTimeInSeconds, road.From.TspTspCity, road.To.TspTspCity), road);
            }

            return new World(wb);
        }

        private static string NameFromLocation(Point location)
        {
            return location.ToString();
        }

        public void DrawBestTour(IEnumerable<AntColonyTSPCity> tour)
        {
            if (tour == null)
            {
                return;
            }

            using (g == Graphics.FromImage(m_Bitmap))
            {
                DrawTour(tour, g, Color.Red);
            }
            m_Display.Invalidate();
        }

        public void Redraw(World world = null, UpdateEventArgs e = null)
        {
            using (g == Graphics.FromImage(m_Bitmap))
            {
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality;
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
                        foreach (object road_loopVariable in m_Roads)
                        {
                            road = road_loopVariable;
                            g.DrawLine(p, road.First.Location, road.Second.Location);
                        }
                    }
                }
                else
                {
                    // While running the heuristic, we don't draw the usual roads.
                    // Instead, we draw the roads according to their pheromone level.

                    dynamic sum_of_pheromones = world.RoadsSum(road.PheromoneLevel);
                    dynamic factor = 255 * world.Roads.Count;

                    foreach (object road_loopVariable in world.Roads)
                    {
                        road = road_loopVariable;
                        dynamic line = m_RoadMap[road];
                        dynamic alpha = Math.Min(Math.Max(Convert.ToInt32(road.PheromoneLevel / sum_of_pheromones * factor), 0), 255);
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
                    foreach (object c_loopVariable in m_Cities)
                    {
                        c = c_loopVariable;
                        g.DrawEllipse(p, c.Location.X - City.Radius, c.Location.Y - City.Radius, City.Radius * 2, City.Radius * 2);
                    }
                }

                //
                // Draw the labels.
                //

                if (ShowLabels)
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(128, Color.Blue)))
                    {
                        foreach (object road_loopVariable in m_Roads)
                        {
                            road = road_loopVariable;
                            PointF text_pos = new PointF((road.First.Location.X + road.Second.Location.X) / 2f, (road.First.Location.Y + road.Second.Location.Y) / 2f);
                            g.DrawString(City.Distance(road.First.Location, road.Second.Location).ToString("0"), m_Display.Font, b, text_pos);
                        }
                    }

                    using (SolidBrush b = new SolidBrush(Color.Black))
                    {
                        foreach (object c_loopVariable in m_Cities)
                        {
                            c = c_loopVariable;
                            dynamic text_pos = c.Location;
                            text_pos.Offset(City.Radius + 5, 0);
                            g.DrawString(c.Name, m_Display.Font, b, text_pos);
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

        private void DrawTour(IEnumerable<HTBAntColonyTSP.TspCity> tour, Graphics g, Color color)
        {
            using (Pen p = new Pen(color, 2))
            {
                dynamic i = tour.GetEnumerator();
                i.MoveNext();
                dynamic first = m_CityMap[i.Current];
                dynamic c1 = first;

                while (i.MoveNext())
                {
                    dynamic c2 = m_CityMap[i.Current];
                    g.DrawLine(p, c1.Location, c2.Location);
                    c1 = c2;
                }

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
