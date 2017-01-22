/*
 * Author:			Generated Code
 * Date Created:	22.03.2013
 * Description:		Represents a row in the tblProtokolPayment table
*/

using System;
namespace HTB.Database
{
	public class tblProtokolPayment : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int PpID{get; set; }
		public int PpProtokolID{get; set; }
		public int PpTypeCode{get; set; }
		public double PpAmount{get; set; }
		public DateTime PpDate{get; set; }
		public string PpMemo{get; set; }
		#endregion
	}
}
