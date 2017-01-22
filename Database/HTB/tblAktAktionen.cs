/*
 * Author:			Generated Code
 * Date Created:	25.02.2011
 * Description:		Represents a row in the tblAktAktionen table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktAktionen : Record
	{
		#region Property Declaration
		private int _aktAktionID;
		private string _aktAktionCode;
		private string _aktAktionCaption;
		private string _aktAktionText;
		private int _aktAktionType;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int AktAktionID
		{
			get { return _aktAktionID; }
			set { _aktAktionID = value; }
		}
		public string AktAktionCode
		{
			get { return _aktAktionCode; }
			set { _aktAktionCode = value; }
		}
		public string AktAktionCaption
		{
			get { return _aktAktionCaption; }
			set { _aktAktionCaption = value; }
		}
		public string AktAktionText
		{
			get { return _aktAktionText; }
			set { _aktAktionText = value; }
		}
		public int AktAktionType
		{
			get { return _aktAktionType; }
			set { _aktAktionType = value; }
		}
		#endregion
	}
}
