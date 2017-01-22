/*
 * Author:			Generated Code
 * Date Created:	23.09.2011
 * Description:		Represents a row in the tblKassablockMissingNr table
*/

using System;
namespace HTB.Database
{
	public class tblKassaBlockMissingNr : Record
	{
		#region Property Declaration
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
		public int KbMissUser{get; set; }
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
		public int KbMissNr{get; set; }
		public DateTime KbMissDate{get; set; }
		public DateTime KbMissReceivedDate{get; set; }
        public int KbMissBlockID { get; set; }
		#endregion

        public tblKassaBlockMissingNr() {}
        public tblKassaBlockMissingNr(tblKassaBlockMissingNr rec)
        {
            KbMissUser = rec.KbMissUser;
            KbMissNr = rec.KbMissNr;
            KbMissDate = rec.KbMissDate;
            KbMissReceivedDate = rec.KbMissReceivedDate;
            KbMissBlockID = rec.KbMissBlockID;
        }
	}
}
