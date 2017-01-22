/*
 * Author:			Generated Code
 * Date Created:	22.03.2013
 * Description:		Represents a row in the tblProtokolPaymentType table
*/

namespace HTB.Database
{
	public class tblProtokolPaymentType : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int PptID{get; set; }
		public int PptCode{get; set; }
		public string PptCaption{get; set; }
		#endregion
	}
}
