/*
 * Author:			Generated Code
 * Date Created:	28.07.2011
 * Description:		Represents a row in the tblAktTypeIntActionRel table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktTypeIntActionRel : Record
	{
		#region Property Declaration
		private int _aktTypeActionAktTypeIntID;
        private int _aktTypeActionAktIntActionTypeID;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int AktTypeActionAktTypeIntID
        {
            get { return _aktTypeActionAktTypeIntID; }
            set { _aktTypeActionAktTypeIntID = value; }
        }
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
		public int AktTypeActionAktIntActionTypeID
		{
			get { return _aktTypeActionAktIntActionTypeID; }
			set { _aktTypeActionAktIntActionTypeID = value; }
		}
		#endregion
	}
}
