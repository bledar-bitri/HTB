/*
 * Author:			Generated Code
 * Date Created:	21.09.2011
 * Description:		Represents a row in the tblAktMeldeInfoAuswahl table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktMeldeInfoAuswahl : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int MeldeInfoAuswahlID{get; set; }
		public int MeldeInfoAuswahl{get; set; }
		public string MeldeInfoAuswahlCaption{get; set; }
		#endregion
	}
}
