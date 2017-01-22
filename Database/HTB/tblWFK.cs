/*
 * Author:			Generated Code
 * Date Created:	24.02.2011
 * Description:		Represents a row in the tblWFK table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblWFK : Record
	{
		#region Property Declaration
		private int _wFPID;
		private int _wFPPosition;
		private int _wFPAktion;
		private int _wFPPreTime;
		private int _wFPKlient;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int WFPID
		{
			get { return _wFPID; }
			set { _wFPID = value; }
		}
		public int WFPPosition
		{
			get { return _wFPPosition; }
			set { _wFPPosition = value; }
		}
		public int WFPAktion
		{
			get { return _wFPAktion; }
			set { _wFPAktion = value; }
		}
		public int WFPPreTime
		{
			get { return _wFPPreTime; }
			set { _wFPPreTime = value; }
		}
		public int WFPKlient
		{
			get { return _wFPKlient; }
			set { _wFPKlient = value; }
		}
		#endregion
	}
}
