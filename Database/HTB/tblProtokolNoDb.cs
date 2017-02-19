using System;
using System.Collections;

namespace HTB.Database
{
	public class tblProtokolNoDb : Record
	{
		#region Property Declaration

	    public int ProtokolID { get; set; }
        public int ProtokolAkt { get; set; }
        public int ProtokolActionTypeID { get; set; }

        public DateTime SicherstellungDatum{get; set; }
		public string UbernahmeOrt{get; set; }
		public bool UbernommentMitZulassung{get; set; }
		public string KZ{get; set; }
		public bool KzVonEcpAnAg  { get; set; }
        public int ProtokolServiceheftID{get; set; }
		public int AnzahlSchlussel{get; set; }
	    public int Tachometer { get; set; }
	    public string VersicherungBarKassiert {get; set; }
		public DateTime VersicherungUberwiesen{get; set; }
        public string ForderungBarKassiert { get; set; }
		public DateTime ForderungUberwiesen{get; set; }
        public string KostenBarKassiert { get; set; }
		public DateTime KostenUberwiesen{get; set; }
        public string Direktzahlung { get; set; }
		public DateTime DirektzahlungAm{get; set; }
        public string DirektzahlungVersicherung { get; set; }
        public DateTime DirektzahlungVersicherungAm { get; set; }
        
		public bool Abschleppdienst{get; set; }
		public string AbschleppdienstName{get; set; }
		public string AbschleppdienstGrund{get; set; }
        
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenAbschleppdienst { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenPannendienst { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenStandgebuhren { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenReparaturen { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenMaut { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenTreibstoff { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenVignette { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenSostige { get; set; }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public string ZusatzkostenEcp { get; set; }

        public string Uberstellungsdistanz { get; set; }
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

        public String VisitsList { get; set; }
        
		#endregion
	}
}
