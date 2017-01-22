/*
 * Author:			Generated Code
 * Date Created:	20.06.2011
 * Description:		Represents a row in the tblAktenIntHonorarGroup table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktenIntHonorarGroup : Record
	{
		#region Property Declaration
		private int _aktIntHonGrpID;
		private string _aktIntHonGrpCaption;
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int AktIntHonGrpID
		{
			get { return _aktIntHonGrpID; }
			set { _aktIntHonGrpID = value; }
		}
		public string AktIntHonGrpCaption
		{
			get { return _aktIntHonGrpCaption; }
			set { _aktIntHonGrpCaption = value; }
		}
		#endregion
	}
}
