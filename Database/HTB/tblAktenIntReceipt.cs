/*
 * Author:			Generated Code
 * Date Created:	16.12.2011
 * Description:		Represents a row in the tblAktenIntReceipt table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblAktenIntReceipt : Record
	{
		#region Property Declaration

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int AktIntReceiptID{get; set; }
	    public int AktIntReceiptAkt { get; set; }
	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double AktIntReceiptAmount{get; set; }
		public DateTime AktIntReceiptDate{get; set; }
		public int AktIntReceiptUser{get; set; }
        public string AktIntReceiptCity { get; set; }
		public int AktIntReceiptType{get; set; }
        public int AktIntReceiptAuftraggeber { get; set; }
        public string AktIntReceiptNumber { get; set; }
		#endregion
	}
}
