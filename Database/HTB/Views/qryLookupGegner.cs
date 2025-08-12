namespace HTB.Database.Views
{
    public class qryLookupGegner : Record
    {
        public string Delimiter = " ";
        #region Property Declaration

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int GegnerID { get; set; }
        public string GegnerAnrede { get; set; }
        public string GegnerName1 { get; set; }
        public string GegnerName2 { get; set; }
        public string GegnerName3 { get; set; }
        public string GegnerAnsprech { get; set; }
        public string GegnerStrasse { get; set; }
        public string GegnerZipPrefix { get; set; }
        public string GegnerZip { get; set; }
        public string GegnerOrt { get; set; }
        public string GegnerOldID { get; set; }
        public string GegnerLastZipPrefix { get; set; }
        public string GegnerLastZip { get; set; }
        public string GegnerLastOrt { get; set; }
        public string GegnerLastStrasse { get; set; }
        public string GegnerLastName1 { get; set; }
        public string GegnerLastName2 { get; set; }
        public string GegnerLastName3 { get; set; }
        #endregion

        public string GetLookupString()
        {
            return GegnerLastName1 + Delimiter +
                GegnerLastName2 + Delimiter +
                GegnerLastName3 + Delimiter +
                GegnerLastStrasse + Delimiter +
                GegnerLastZipPrefix + Delimiter +
                GegnerLastZip + Delimiter +
                GegnerOldID + Delimiter +
                GegnerLastOrt;
        }
    }
}
