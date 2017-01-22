using System;
using System.Diagnostics;

namespace TspAntColony
{
   

    [DebuggerDisplay("Distance: {Distance}, Pheromone level: {PheromoneLevel}")]
    public class Road
    {

        private const double α = World.α;
        private const double β = World.β;
        private double m_Distance;

        private double m_PheromoneLevel;
        public double Distance
        {
            get { return m_Distance; }
        }

        public double PheromoneLevel
        {
            get { return m_PheromoneLevel; }
            set { m_PheromoneLevel = value; }
        }

        public double WeighedValue
        {
            get { return Math.Pow(Distance, α) * Math.Pow(PheromoneLevel, β); }
        }

        public Road(double distance)
        {
            m_Distance = distance;
        }
    }

}
