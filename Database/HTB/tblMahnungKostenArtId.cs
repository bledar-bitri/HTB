/*
 * Author:			Generated Code
 * Date Created:	30.05.2011
 * Description:		Represents a row in the tblMahnungKostenArtId table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblMahnungKostenArtId : Record
	{
		#region Property Declaration
		private int _mahKostId;
		private int _mahKostArtId;
		private int _mahKostMahnungNummer;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int MahKostId
		{
			get { return _mahKostId; }
			set { _mahKostId = value; }
		}
		public int MahKostArtId
		{
			get { return _mahKostArtId; }
			set { _mahKostArtId = value; }
		}
		public int MahKostMahnungNummer
		{
			get { return _mahKostMahnungNummer; }
			set { _mahKostMahnungNummer = value; }
		}
		#endregion
	}
}
