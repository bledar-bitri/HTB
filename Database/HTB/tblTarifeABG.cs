/*
 * Author:			Generated Code
 * Date Created:	24.02.2011
 * Description:		Represents a row in the tblTarifeABG table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblTarifeABG : Record
	{
		#region Property Declaration
		private int _tarifeABGID;
		private int _tarifeABGKlient;
		private double _tarifeABGSWvon;
		private double _tarifeABGSWbis;
		private double _tarifeABGPS;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int TarifeABGID
		{
			get { return _tarifeABGID; }
			set { _tarifeABGID = value; }
		}
		public int TarifeABGKlient
		{
			get { return _tarifeABGKlient; }
			set { _tarifeABGKlient = value; }
		}
		public double TarifeABGSWvon
		{
			get { return _tarifeABGSWvon; }
			set { _tarifeABGSWvon = value; }
		}
		public double TarifeABGSWbis
		{
			get { return _tarifeABGSWbis; }
			set { _tarifeABGSWbis = value; }
		}
		public double TarifeABGPS
		{
			get { return _tarifeABGPS; }
			set { _tarifeABGPS = value; }
		}
		#endregion
	}
}
