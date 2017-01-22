using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryCustAktEdit : Record
    {
        #region Property Declaration tblKlient

        public int KlientID { get; set; }

        public string KlientAnrede { get; set; }

        public string KlientName1 { get; set; }

        public string KlientName2 { get; set; }

        public string KlientName3 { get; set; }

        public string KlientStrasse { get; set; }

        public string KlientLKZ { get; set; }

        public string KlientPLZ { get; set; }

        public string KlientOrt { get; set; }

        #endregion

        #region Property Declaration tblGegner
        
        private string _gegnerPeliID;
        private string _gegnerLastZipPrefix;
        private string _gegnerLastZip;
        private string _gegnerLastOrt;
        private string _gegnerLastStrasse;
        private string _gegnerLastName1;
        private string _gegnerLastName2;
        private string _gegnerLastName3;
        
        public string GegnerPeliID
        {
            get { return _gegnerPeliID; }
            set { _gegnerPeliID = value; }
        }
        public string GegnerLastZipPrefix
        {
            get { return _gegnerLastZipPrefix; }
            set { _gegnerLastZipPrefix = value; }
        }
        public string GegnerLastZip
        {
            get { return _gegnerLastZip; }
            set { _gegnerLastZip = value; }
        }
        public string GegnerLastOrt
        {
            get { return _gegnerLastOrt; }
            set { _gegnerLastOrt = value; }
        }
        public string GegnerLastStrasse
        {
            get { return _gegnerLastStrasse; }
            set { _gegnerLastStrasse = value; }
        }
        public string GegnerLastName1
        {
            get { return _gegnerLastName1; }
            set { _gegnerLastName1 = value; }
        }
        public string GegnerLastName2
        {
            get { return _gegnerLastName2; }
            set { _gegnerLastName2 = value; }
        }
        public string GegnerLastName3
        {
            get { return _gegnerLastName3; }
            set { _gegnerLastName3 = value; }
        }
        
        #endregion

        #region Property Declaration tblCustInkAkt

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int CustInkAktID { get; set; }

        public string CustInkAktAZ { get; set; }

        public int CustInkAktKlient { get; set; }

        public int CustInkAktGegner { get; set; }

        public DateTime CustInkAktEnterDate { get; set; }

        public int CustInkAktStatus { get; set; }

        public string CustInkAktKunde { get; set; }

        public int CustInkAktCurStatus { get; set; }

        public string CustInkAktOldID { get; set; }

        public int CustInkAktSB { get; set; }

        public string CustInkAktMemo { get; set; }

        public int CustInkAktAuftraggeber { get; set; }

        public string CustInkAktGothiaNr { get; set; }

        public DateTime CustInkAktInvoiceDate { get; set; }

        public DateTime CustInkAktNextWFLStep { get; set; }

        public int CustInkAktRV { get; set; }

        public int CustInkAktRT { get; set; }

        public decimal CustInkAktID2 { get; set; }

        public int CustInkAktCurrentStep { get; set; }

        public bool CustInkAktIsPartial { get; set; }
        
        public bool CustInkAktSendBericht { get; set; }

        public int CustInkAktLawyerId { get; set; }
        public int CustInkAkKlientSB { get; set; }
        #endregion

        #region Property Declaration tblUser
        private string _userVorname;
        private string _userNachname;
        
        public string UserVorname
        {
            get { return _userVorname; }
            set { _userVorname = value; }
        }
        public string UserNachname
        {
            get { return _userNachname; }
            set { _userNachname = value; }
        }
        #endregion

        #region Property Declaration tblAuftraggeber
        private string _auftraggeberAnrede;
        private string _auftraggeberName1;
        private string _auftraggeberName2;
        private string _auftraggeberName3;
        private string _auftraggeberStrasse;
        private string _auftraggeberLKZ;
        private string _auftraggeberPLZ;
        private string _auftraggeberOrt;
        public string AuftraggeberAnrede
        {
            get { return _auftraggeberAnrede; }
            set { _auftraggeberAnrede = value; }
        }
        public string AuftraggeberName1
        {
            get { return _auftraggeberName1; }
            set { _auftraggeberName1 = value; }
        }
        public string AuftraggeberName2
        {
            get { return _auftraggeberName2; }
            set { _auftraggeberName2 = value; }
        }
        public string AuftraggeberName3
        {
            get { return _auftraggeberName3; }
            set { _auftraggeberName3 = value; }
        }
        public string AuftraggeberStrasse
        {
            get { return _auftraggeberStrasse; }
            set { _auftraggeberStrasse = value; }
        }
        public string AuftraggeberLKZ
        {
            get { return _auftraggeberLKZ; }
            set { _auftraggeberLKZ = value; }
        }
        public string AuftraggeberPLZ
        {
            get { return _auftraggeberPLZ; }
            set { _auftraggeberPLZ = value; }
        }
        public string AuftraggeberOrt
        {
            get { return _auftraggeberOrt; }
            set { _auftraggeberOrt = value; }
        }
        #endregion

        #region Property Declaration tblLawyer
        public string LawyerAnrede { get; set; }
        public string LawyerName1 { get; set; }
        public string LawyerName2 { get; set; }
        public string LawyerEmail { get; set; }
        #endregion
    }
}
