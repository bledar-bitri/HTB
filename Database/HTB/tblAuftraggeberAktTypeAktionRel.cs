/*
 * Author:			Generated Code
 * Date Created:	28.07.2011
 * Description:		Represents a row in the tblAuftraggeberAktTypeAktionRel table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAuftraggeberAktTypeAktionRel : Record
	{
		#region Property Declaration
		private int _aGAktionTypeAktIntActionTypeID;
		private int _aGAktionTypeAuftraggeberID;
		private int _aGAktionTypeAktTypeIntID;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
		public int AGAktionTypeAktIntActionTypeID
		{
			get { return _aGAktionTypeAktIntActionTypeID; }
			set { _aGAktionTypeAktIntActionTypeID = value; }
		}
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
		public int AGAktionTypeAuftraggeberID
		{
			get { return _aGAktionTypeAuftraggeberID; }
			set { _aGAktionTypeAuftraggeberID = value; }
		}
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
		public int AGAktionTypeAktTypeIntID
		{
			get { return _aGAktionTypeAktTypeIntID; }
			set { _aGAktionTypeAktTypeIntID = value; }
		}
		#endregion
	}
}
