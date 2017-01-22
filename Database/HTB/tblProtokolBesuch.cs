/*
 * Author:			Generated Code
 * Date Created:	16.05.2012
 * Description:		Represents a row in the tblProtokolBesuch table
*/

using System;
namespace HTB.Database
{
	public class tblProtokolBesuch : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int BesuchID{get; set; }
		public int ProtokolID{get; set; }
		public DateTime BesuchAm{get; set; }
		#endregion
	}
}
