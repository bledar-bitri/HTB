/*
 * Author:			Generated Code
 * Date Created:	16.05.2012
 * Description:		Represents a row in the tblProtokol table
*/

using System;
using System.Collections;

namespace HTB.Database
{
	public class tblProtokol : Record
	{
		#region Property Declaration

	    [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
	    public int ProtokolID { get; set; }

	    [MappingAttribute(FieldName = "AktIntID")]
	    public int ProtokolAkt { get; set; }

	    [MappingAttribute(FieldName = "AktIntActionTypeID")]
	    public int ProtokolActionTypeID { get; set; }

	    public DateTime SicherstellungDatum{get; set; }
		public string UbernahmeOrt{get; set; }
		public bool UbernommentMitZulassung{get; set; }
		public string KZ{get; set; }
		public bool KzVonEcpAnAg  { get; set; }
        public int ProtokolServiceheftID{get; set; }
		public int AnzahlSchlussel{get; set; }
	    public int Tachometer { get; set; }
	    [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
		public double VersicherungBarKassiert{get; set; }
		public DateTime VersicherungUberwiesen{get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ForderungBarKassiert { get; set; }
		public DateTime ForderungUberwiesen{get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double KostenBarKassiert { get; set; }
		public DateTime KostenUberwiesen{get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double Direktzahlung { get; set; }
		public DateTime DirektzahlungAm{get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double DirektzahlungVersicherung { get; set; }
        public DateTime DirektzahlungVersicherungAm { get; set; }
        
		public bool Abschleppdienst{get; set; }
		public string AbschleppdienstName{get; set; }
		public string AbschleppdienstGrund{get; set; }
        
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenAbschleppdienst { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenPannendienst { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenStandgebuhren { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenReparaturen { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenMaut { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenTreibstoff { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenVignette { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenSostige { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ZusatzkostenEcp { get; set; }

        public double Uberstellungsdistanz { get; set; }
        public string HandlerName{get; set; }
		public string HandlerStrasse{get; set; }
		public string HandlerLKZ{get; set; }
		public string HandlerPLZ{get; set; }
		public string HandlerOrt{get; set; }
		public DateTime UpdateDate{get; set; }
		public string Beifahrer{get; set; }
		public string UbernommenVon{get; set; }
		public string SchadenComment{get; set; }
        public string Memo { get; set; }
        public string SignaturePath { get; set; }
        public string Serviceheft { get; set; }
        public bool PolizieInformiert { get; set; }
        public string PolizeiDienststelle { get; set; }
        public string RechnungNr { get; set; }
        public string HandlerEMail { get; set; }

        public bool MasterKey { get; set; }

        public int AnzahlKZ { get; set; }

        [MappingAttribute(FieldType = MappingAttribute.NO_DB_SAVE)]
        public String VisitsList { get; set; }
        
		#endregion
	}
}
