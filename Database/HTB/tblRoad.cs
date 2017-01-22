/*
 * Author:			Generated Code
 * Date Created:	02.11.2011
 * Description:		Represents a row in the tblRoadInfo table
*/
using System;
namespace HTB.Database
{
	public class tblRoad : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int FromLatitude { get; set; }
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int FromLongitude { get; set; }
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int ToLatitude { get; set; }
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int ToLongitude { get; set; }

        public long TimeInSeconds { get; set; }

		public double Distance{get; set; }
		public DateTime LookupDate{get; set; }
		#endregion
	}
}
