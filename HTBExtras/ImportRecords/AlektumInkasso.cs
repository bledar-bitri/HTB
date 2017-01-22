using FileHelpers;

namespace HTBExtras.ImportRecords
{
    [DelimitedRecord(";")]
    [IgnoreFirst]
    public class AlektumInkasso
    {
        public string KundennummerVonSDBeimAG { get; set; }
        public string Auftragsnummer { get; set; }
        public string SchuldnerFirmennummer { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Alias { get; set; }
        public string BranchenCode { get; set; }
        public string SDPLZ { get; set; }
        public string SDOrt { get; set; }
        public string SDStrasse { get; set; }
        public string SDLKZ { get; set; }
        public string Telefonnummer { get; set; }
        public string Glaeubiger { get; set; }
        public string GLPLZ { get; set; }
        public string GLOrt { get; set; }
        public string GLStrasse { get; set; }
        public string UebergebenesKapital { get; set; }
        public string BezKapital { get; set; }
        public string offenesKapital { get; set; }
        public string AGFirmennummer { get; set; }
        public string Auftraggebername { get; set; }
        public string AGPLZ { get; set; }
        public string AGOrt { get; set; }
        public string AGStraße { get; set; }
        public string AGLKZ { get; set; }
        public string SchuldnerGebDat { get; set; }
        public string Sachbearbeiter { get; set; }
        public string SachbearbeiterEmail { get; set; }
        public string InternCode { get; set; }
        public string StatusDa { get; set; }
        public string UebergebenePLZ { get; set; }
        public string UebergebenenOrt { get; set; }
        public string UebergebeneStrasse { get; set; }
        public string OffeneZinsen { get; set; }
        public string OffeneAGSpesen { get; set; }
        public string OffeneKSVKosten { get; set; }
        public string Gesamtsaldo { get; set; }
        public string AktType { get; set; }
        public string Forderungstext { get; set; }
        public string Empty { get; set; }
    }
}
