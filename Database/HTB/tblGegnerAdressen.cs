/*
 * Author:			Generated Code
 * Date Created:	10.05.2012
 * Description:		Represents a row in the tblGegnerAdressen table
*/

using System;
namespace HTB.Database
{
	public class tblGegnerAdressen : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int GAID{get; set; }
		public int GAGegner{get; set; }
		public int GAType{get; set; }
		public string GAStrasse{get; set; }
		public string GAZipPrefix{get; set; }
		public string GAZIP{get; set; }
		public string GAOrt{get; set; }
		public DateTime GATimeStamp{get; set; }
		public int GAUserID{get; set; }
		public string GAName1{get; set; }
		public string GAName2{get; set; }
		public string GAName3{get; set; }
		public decimal GALatitude{get; set; }
		public decimal GALongitude{get; set; }
        public string GADescription { get; set; }
		#endregion
	}
}
