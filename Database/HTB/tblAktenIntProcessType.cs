/*
 * Author:			Generated Code
 * Date Created:	19.04.2011
 * Description:		Represents a row in the tblAktenIntProcessType table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktenIntProcessType : Record
	{
		#region Property Declaration
		private int _aktIntProcessID;
		private int _aktIntProcessCode;
		private string _aktIntProcessDescription;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int AktIntProcessID
		{
			get { return _aktIntProcessID; }
			set { _aktIntProcessID = value; }
		}
		public int AktIntProcessCode
		{
			get { return _aktIntProcessCode; }
			set { _aktIntProcessCode = value; }
		}
		public string AktIntProcessDescription
		{
			get { return _aktIntProcessDescription; }
			set { _aktIntProcessDescription = value; }
		}
		#endregion
	}
}
