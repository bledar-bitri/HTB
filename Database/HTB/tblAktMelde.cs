/*
 * Author:			Generated Code
 * Date Created:	21.09.2011
 * Description:		Represents a row in the tblAktMelde table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktMelde : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int AMID{get; set; }
		public string AMNr{get; set; }
		public string AMSBKlient{get; set; }
		public int AMGegner{get; set; }
		public string AMBemerkungen{get; set; }
		public int AMStatus{get; set; }
		public DateTime AMErfasstDatum{get; set; }
		public DateTime AMErledigtDatum{get; set; }
		public DateTime AMUebergabeDatum{get; set; }
		public string AmInfoText1{get; set; }
		public string AMInfoText2{get; set; }
		public string AMInfoText3{get; set; }
		public int AMInfoAuswahl{get; set; }
		public string AMBKZ{get; set; }
		public int AMSB{get; set; }
		public int AMKlient{get; set; }
		public int AMCreatedUser{get; set; }
		public int AMGegnerAdresse{get; set; }
		public int AMAktFlagInvoice{get; set; }
		public string AMBemerkungenExt{get; set; }
		public string AMBericht{get; set; }
		public int AMAktImport{get; set; }
		public string AMKSVNummer{get; set; }
		public int AMProcessCode{get; set; }
		#endregion
	}
}
