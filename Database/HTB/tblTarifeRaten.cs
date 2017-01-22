/*
 * Author:			Generated Code
 * Date Created:	07.03.2011
 * Description:		Represents a row in the tblTarifeRaten table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblTarifeRaten : Record
	{
		#region Property Declaration
		private int _tarifeRatenID;
		private int _tarifeRatenKlient;
		private double _tarifeRatenSWvon;
		private double _tarifeRatenSWbis;
		private double _tarifeRatenBetrag;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int TarifeRatenID
		{
			get { return _tarifeRatenID; }
			set { _tarifeRatenID = value; }
		}
		public int TarifeRatenKlient
		{
			get { return _tarifeRatenKlient; }
			set { _tarifeRatenKlient = value; }
		}
		public double TarifeRatenSWvon
		{
			get { return _tarifeRatenSWvon; }
			set { _tarifeRatenSWvon = value; }
		}
		public double TarifeRatenSWbis
		{
			get { return _tarifeRatenSWbis; }
			set { _tarifeRatenSWbis = value; }
		}
		public double TarifeRatenBetrag
		{
			get { return _tarifeRatenBetrag; }
			set { _tarifeRatenBetrag = value; }
		}
		#endregion
	}
}
