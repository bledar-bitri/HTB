/*
 * Author:			Generated Code
 * Date Created:	08.05.2012
 * Description:		Represents a row in the tblPhoneType table
*/

namespace HTB.Database
{
	public class tblPhoneType : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int PhoneTypeID{get; set; }
		public int PhoneTypeCode{get; set; }
		public string PhoneTypeCaption{get; set; }
        public int PhoneTypeSequence { get; set; }
		#endregion
	}
}
