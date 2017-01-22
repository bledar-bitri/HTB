/*
 * Author:			Generated Code
 * Date Created:	09.05.2011
 * Description:		Represents a row in the tblState table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblState : Record
	{
		#region Property Declaration
		private int _tblStateID;
		private string _tblStateCaption;
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int tblStateID
		{
			get { return _tblStateID; }
			set { _tblStateID = value; }
		}
		public string tblStateCaption
		{
			get { return _tblStateCaption; }
			set { _tblStateCaption = value; }
		}
		#endregion
	}
}
