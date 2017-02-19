/*
 * Author:			Generated Code
 * Date Created:	16.05.2012
 * Description:		Represents a row in the tblProtokol table
*/

using System;
using System.Collections;
using System.Globalization;

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

        public tblProtokol()
        {
        }
        public tblProtokol(tblProtokolNoDb rec)
	    {
	        Assign(rec);
	    }

	    public void Assign(tblProtokolNoDb rec)
	    {
	        ProtokolID = rec.ProtokolID;
	        ProtokolAkt = rec.ProtokolAkt;
	        ProtokolActionTypeID = rec.ProtokolActionTypeID;
	        SicherstellungDatum = rec.SicherstellungDatum;
	        UbernahmeOrt = rec.UbernahmeOrt;
	        UbernommentMitZulassung = rec.UbernommentMitZulassung;
	        KZ = rec.KZ;
	        KzVonEcpAnAg = rec.KzVonEcpAnAg;
	        ProtokolServiceheftID = rec.ProtokolServiceheftID;
	        AnzahlSchlussel = rec.AnzahlSchlussel;
	        Tachometer = rec.Tachometer;
	        VersicherungBarKassiert = fixIpadNumber(rec.VersicherungBarKassiert);
	        VersicherungUberwiesen = rec.VersicherungUberwiesen;
	        ForderungBarKassiert = fixIpadNumber(rec.ForderungBarKassiert);
	        ForderungUberwiesen = rec.ForderungUberwiesen;
	        KostenBarKassiert = fixIpadNumber(rec.KostenBarKassiert);
	        KostenUberwiesen = rec.KostenUberwiesen;
	        Direktzahlung = fixIpadNumber(rec.Direktzahlung);
	        DirektzahlungAm = rec.DirektzahlungAm;
	        DirektzahlungVersicherung = fixIpadNumber(rec.DirektzahlungVersicherung);
	        DirektzahlungVersicherungAm = rec.DirektzahlungVersicherungAm;
	        Abschleppdienst = rec.Abschleppdienst;
	        AbschleppdienstName = rec.AbschleppdienstName;
	        AbschleppdienstGrund = rec.AbschleppdienstGrund;
	        ZusatzkostenAbschleppdienst = fixIpadNumber(rec.ZusatzkostenAbschleppdienst);
	        ZusatzkostenPannendienst = fixIpadNumber(rec.ZusatzkostenPannendienst);
	        ZusatzkostenStandgebuhren = fixIpadNumber(rec.ZusatzkostenStandgebuhren);
	        ZusatzkostenReparaturen = fixIpadNumber(rec.ZusatzkostenReparaturen);
	        ZusatzkostenMaut = fixIpadNumber(rec.ZusatzkostenMaut);
	        ZusatzkostenTreibstoff = fixIpadNumber(rec.ZusatzkostenTreibstoff);
	        ZusatzkostenVignette = fixIpadNumber(rec.ZusatzkostenVignette);
	        ZusatzkostenSostige = fixIpadNumber(rec.ZusatzkostenSostige);
	        ZusatzkostenEcp = fixIpadNumber(rec.ZusatzkostenEcp);
	        Uberstellungsdistanz = fixIpadNumber(rec.Uberstellungsdistanz);
	        HandlerName = rec.HandlerName;
	        HandlerStrasse = rec.HandlerStrasse;
	        HandlerLKZ = rec.HandlerLKZ;
	        HandlerPLZ = rec.HandlerPLZ;
	        HandlerOrt = rec.HandlerOrt;
	        UpdateDate = rec.UpdateDate;
	        Beifahrer = rec.Beifahrer;
	        UbernommenVon = rec.UbernommenVon;
	        SchadenComment = rec.SchadenComment;
	        Memo = rec.Memo;
	        SignaturePath = rec.SignaturePath;
	        Serviceheft = rec.Serviceheft;
	        PolizieInformiert = rec.PolizieInformiert;
	        PolizeiDienststelle = rec.PolizeiDienststelle;
	        RechnungNr = rec.RechnungNr;
	        HandlerEMail = rec.HandlerEMail;
	        MasterKey = rec.MasterKey;
	        AnzahlKZ = rec.AnzahlKZ;
	        VisitsList = rec.VisitsList;
	    }

	    private double fixIpadNumber(string number)
	    {
	        if (number == null) return 0;
	        if (number.Trim() == "") return 0;
	        number = ReplaceLastOccurrence(number, ",", ".").Replace(",", "");
	        try
	        {
	            return double.Parse(number, CultureInfo.InvariantCulture.NumberFormat);
	        }
	        catch
	        {
	            return 0;
	        }
	    }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
    }
}
