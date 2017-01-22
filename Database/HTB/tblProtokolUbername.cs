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

        public bool MasterKey { get; set; }
        public bool KzImKfz { get; set; }
        public bool Typenschein { get; set; }
        public bool KzDurchBehoerdeEingezogen{ get; set; }
        public bool KfzAbgemeldet { get; set; }
        public bool InnenraumVerschmutzt{ get; set; }
        public bool SichtbareSchaden{ get; set; }
        public int AnzahlDerBilder { get; set; }
        public bool MoechteAusloesen{ get; set; }
        public DateTime AusloesenDatum{ get; set; }


        #endregion
    }
}
