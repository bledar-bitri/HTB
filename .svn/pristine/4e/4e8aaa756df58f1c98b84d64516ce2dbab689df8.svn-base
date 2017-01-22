using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryDoksInkAkten : tblDokument
    {
        #region Property Declaration tblUser
        private string _userVorname;
        private string _userNachname;
        
        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
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

        #region Property Declaration tblDokumentType
        private string _dokTypeCaption;
        public string DokTypeCaption
        {
            get { return _dokTypeCaption; }
            set { _dokTypeCaption = value; }
        }
        #endregion

        #region Property Declaration tblAktenDokumente
        private int _aDID;
        private int _aDAkttyp;
        private int _aDAkt;
        private int _aDDok;
        public int ADID
        {
            get { return _aDID; }
            set { _aDID = value; }
        }
        public int ADAkttyp
        {
            get { return _aDAkttyp; }
            set { _aDAkttyp = value; }
        }
        public int ADAkt
        {
            get { return _aDAkt; }
            set { _aDAkt = value; }
        }
        public int ADDok
        {
            get { return _aDDok; }
            set { _aDDok = value; }
        }
        #endregion

        #region Property Declaration tblCustInkAkt

        private string _custInkAktGEOrt;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int CustInkAktID { get; set; }

        public string CustInkAktAZ { get; set; }

        public int CustInkAktKlient { get; set; }

        public int CustInkAktGegner { get; set; }

        public DateTime CustInkAktEnterDate { get; set; }

        public int CustInkAktStatus { get; set; }

        public string CustInkAktKunde { get; set; }

        public string CustInkAktGEName { get; set; }

        public string CustInkAktGEStrasse { get; set; }

        public string CustInkAktGEPrefix { get; set; }

        public string CustInkAktGEZIP { get; set; }

        public string CustInkAktGEOrt
        {
            get { return _custInkAktGEOrt; }
            set { _custInkAktGEOrt = value; }
        }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double CustInkAktBetragOffen { get; set; }

        public string CustInkAktStatus2 { get; set; }

        public int CustInkAktCurStatus { get; set; }

        public string CustInkAktCurStatusText { get; set; }

        public DateTime CustInkAktLastChange { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double CustInkAktForderung { get; set; }

        public string CustInkAktOldID { get; set; }

        public int CustInkAktEnterUser { get; set; }

        public int CustInkAktLastChangeUser { get; set; }

        public int CustInkAktSB { get; set; }

        public string CustInkAktMemo { get; set; }

        public int CustInkAktAuftraggeber { get; set; }

        public string CustInkAktGothiaNr { get; set; }

        public DateTime CustInkAktInvoiceDate { get; set; }

        public int CustInkAktBankeinzugFlag { get; set; }

        public int CustInkAktRatenerinnerungFlag { get; set; }

        public int CustInkAktDelFlag { get; set; }

        public DateTime CustInkAktNextWFLStep { get; set; }

        public int CustInkAktRV { get; set; }

        public int CustInkAktDZ { get; set; }

        public int CustInkAktCurStatusHold { get; set; }

        public int CustInkAktMeldeRetour { get; set; }

        public int CustInkAktRT { get; set; }

        public decimal CustInkAktID2 { get; set; }

        public int CustInkAktCurrentStep { get; set; }

        public bool CustInkAktIsPartial { get; set; }

        #endregion
    }
}
