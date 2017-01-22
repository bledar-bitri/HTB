/*
 * Author:			Generated Code
 * Date Created:	12.11.2012
 * Description:		Represents a row in the tblCustInkAktInvoiceTransfer table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblCustInkAktInvoiceTransfer : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int InvoiceTransferID{get; set; }
		public int InvoiceTransferInvoiceID{get; set; }
		public DateTime InvoiceTransferDate{get; set; }
		public double InvoiceTransferAmount{get; set; }
		public int InvoiceTransferTypeCode{get; set; }
		public int InvoiceTransferKlient{get; set; }
		public int InvoiceTransferUser{get; set; }
		#endregion
	}
}
