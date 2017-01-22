using System.Diagnostics;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;


namespace HTBAntColonyTSP
{
	[DebuggerDisplay("Distance: {Distance}, Pheromone level: {PheromoneLevel}")]
    [Serializable]
    public class Road : ISerializable
	{
		private const double Alpha = -1.5;
		private const double Beta = 1.5;
//        private const double Alpha = -0.2;
//        private const double Beta = 9.6;
		private readonly double _distance;
		private double _pheromoneLevel;
        private readonly long _travelTime;

		public double Distance
		{
			get
			{
				return _distance;
			}
		}
        public long TravelTime
        {
            get
            {
                return _travelTime;
            }
        }
		
		public double PheromoneLevel
		{
			get
			{
				return _pheromoneLevel;
			}
			set
			{
				_pheromoneLevel = value;
			}
		}
		
		public double WeighedValue
		{
			get
			{
				return Math.Pow(Distance, Alpha) * Math.Pow(PheromoneLevel, Beta);
			}
		}
		
        public Road(double distance, long travelTime)
        {
            _distance = distance;
            _travelTime = travelTime;
        }
        #region ISerializable
        //note: this is private to control access;
        //the serializer can still access this constructor
        private Road(SerializationInfo info, StreamingContext ctxt)
        {
            _distance = info.GetDouble("Distance");
            _pheromoneLevel = info.GetDouble("PheromoneLevel");
            try
            {
                _travelTime = info.GetInt64("TravelTime"); // new field (backward compatibility)
            }
            catch
            {
            }
            
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Distance", _distance);
            info.AddValue("PheromoneLevel", _pheromoneLevel);
            info.AddValue("TravelTime", _travelTime);
        }
        #endregion

        public String GetDumpData()
        {
            var sb = new StringBuilder();
            sb.Append("[Distance: ");
            sb.Append(Distance);
            sb.Append("] [Pheromne: ");
            sb.Append(PheromoneLevel);
            sb.Append("]");
            return sb.ToString();
        }
	}
	
}
