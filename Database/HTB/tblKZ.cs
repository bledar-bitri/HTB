/*
 * Author:			Generated Code
 * Date Created:	07.03.2011
 * Description:		Represents a row in the tblKZ table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblKZ : Record
	{
		#region Property Declaration
		private int _kZID;
		private string _kZKZ;
		private string _kZCaption;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int KZID
		{
			get { return _kZID; }
			set { _kZID = value; }
		}
		public string KZKZ
		{
			get { return _kZKZ; }
			set { _kZKZ = value; }
		}
		public string KZCaption
		{
			get { return _kZCaption; }
			set { _kZCaption = value; }
		}
		#endregion
	}
}
