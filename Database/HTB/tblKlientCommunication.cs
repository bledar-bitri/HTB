/*
 * Author:			Generated Code
 * Date Created:	22.10.2012
 * Description:		Represents a row in the tblAuftraggeberCommunication table
*/

using System;
namespace HTB.Database
{
    public class tblKlientCommunication : Record
	{
		#region Property Declaration

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int KlientComID { get; set; }

		public int KlientComKlient{get; set; }
		public DateTime KlientComDate{get; set; }
		public int KlientComUser{get; set; }
		public string KlientComText{get; set; }
		#endregion
	}
}
