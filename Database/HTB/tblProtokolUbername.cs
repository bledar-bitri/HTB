/*
 * Author:			Generated Code
 * Date Created:	12.06.2012
 * Description:		Represents a row in the tblProtokolUbername table
*/

using System;
namespace HTB.Database
{
	public class tblProtokolUbername : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int UbernameAktIntID { get; set; }
		
	    public DateTime UbernameDatum{get; set; }
		public string UbernameName{get; set; }
		public string UbernameAuto{get; set; }
		public bool UbernameZulassungsschein{get; set; }
		public string UbernameZulassungsscheinGrund{get; set; }
		public int UbernameSchlussel{get; set; }
		public string UbernameKennzeichen{get; set; }
		public bool UbernameServiceheft{get; set; }
		public string UbernameZip{get; set; }
		public string UbernameOrt{get; set; }
		public int UbernameSB{get; set; }
		public string UbernameSignaturePath{get; set; }
		public double UbernameLatitude{get; set; }
		public double UbernameLongitude{get; set; }
		public string UbernameMemo{get; set; }

        public bool UbernameMasterKey { get; set; }
        public bool UbernameKzImKfz { get; set; }
        public bool UbernameTypenschein { get; set; }
        public bool UbernameKzDurchBehoerdeEingezogen { get; set; }
        public bool UbernameKfzAbgemeldet { get; set; }
        public bool UbernameInnenraumVerschmutzt { get; set; }
        public bool UbernameSichtbareSchaden { get; set; }
        public int UbernameAnzahlDerBilder { get; set; }
        public bool UbernameMoechteAusloesen { get; set; }
        public DateTime UbernameAusloesenDatum { get; set; }
        public int UbernameTachometer { get; set; }


        #endregion
    }
}
