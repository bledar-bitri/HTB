using System;

namespace HTB.Database.HTB.StoredProcs
{
    public class spAGReceipt : Record
    {
        #region Property Declaration tblCustInkAkt

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int CustInkAktID { get; set; }

        public DateTime CustInkAktEnterDate { get; set; }

        public int CustInkAktStatus { get; set; }

        public string CustInkAktKunde { get; set; }
        public string CustInkAktGothiaNr { get; set; }
        #endregion

        #region Property Declaration tblAktenInt

        public int AktIntID { get; set; }
        public string AktIntAZ { get; set; }
        public int AktIntAuftraggeber { get; set; }
        public DateTime AktIntDatum { get; set; }
        public string AktIntIZ { get; set; }
        public string AktIntAutoIdNr { get; set; }          // VIN (vehicle identification number) [Fahrgestell Nummer]
        public string AktIntAutoKZ { get; set; }            // license plate
        public string AktIntAutoName { get; set; }
        #endregion

        #region Property Declaration tblKlient

        public string KlientAnrede { get;set; }
        public string KlientTitel { get; set; }
        public string KlientName1 { get; set; }
        public string KlientName2 { get; set; }
        
        #endregion

        #region Property Declaration tblAuftraggeber

        public string AuftraggeberAnrede { get; set; }
        public string AuftraggeberTitel { get; set; }
        public string AuftraggeberName1 { get; set; }
        public string AuftraggeberName2 { get; set; }
        public string AuftraggeberName3 { get; set; }
        
        #endregion

        #region Property Declaration tblGegner

        public string GegnerAnrede { get; set; }

        public string GegnerName1 { get; set; }
        public string GegnerName2 { get; set; }
        public string GegnerName3 { get; set; }
        
        public string GegnerLastName1 { get; set; }
        public string GegnerLastName2 { get; set; }
        public string GegnerLastName3 { get; set; }

        #endregion

        #region Property Declaration tblCustInkAktInvoice
        private DateTime _invoiceDate;
        private double _invoiceAmount;
        private double _invoiceBalance;
        
        public DateTime InvoiceDate
        {
            get { return _invoiceDate; }
            set { _invoiceDate = value; }
        }
        
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double InvoiceAmount
        {
            get { return _invoiceAmount; }
            set { _invoiceAmount = value; }
        }
        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double InvoiceBalance
        {
            get { return _invoiceBalance; }
            set { _invoiceBalance = value; }
        }
        #endregion
    }
}
