using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryAktenIntExtension : tblAktenIntExtension
    {
        #region Property Declaration tblAktenInt
        private string _aktIntAZ;
        private DateTime _aktIntDatum;
        private double _aktIntStatus;
        private DateTime _aktIntTermin;
        private DateTime _aktIntTerminAD;
        private int _aktIntSB;
        private int _aktIntCustInkAktID;
        private string _aKTIntAGSB;
        private string _aKTIntKSVEMail;
        private double _aKTIntZinsenBetrag;
        private double _aKTIntKosten;

        public string AktIntAZ
        {
            get { return _aktIntAZ; }
            set { _aktIntAZ = value; }
        }
        public DateTime AktIntDatum
        {
            get { return _aktIntDatum; }
            set { _aktIntDatum = value; }
        }
        public double AktIntStatus
        {
            get { return _aktIntStatus; }
            set { _aktIntStatus = value; }
        }
        public DateTime AktIntTermin
        {
            get { return _aktIntTermin; }
            set { _aktIntTermin = value; }
        }
        public DateTime AktIntTerminAD
        {
            get { return _aktIntTerminAD; }
            set { _aktIntTerminAD = value; }
        }
        public int AktIntSB
        {
            get { return _aktIntSB; }
            set { _aktIntSB = value; }
        }
        public int AktIntCustInkAktID
        {
            get { return _aktIntCustInkAktID; }
            set { _aktIntCustInkAktID = value; }
        }
        public string AKTIntAGSB
        {
            get { return _aKTIntAGSB; }
            set { _aKTIntAGSB = value; }
        }
        public string AKTIntKSVEMail
        {
            get { return _aKTIntKSVEMail; }
            set { _aKTIntKSVEMail = value; }
        }
        public double AKTIntZinsenBetrag
        {
            get { return _aKTIntZinsenBetrag; }
            set { _aKTIntZinsenBetrag = value; }
        }
        public double AKTIntKosten
        {
            get { return _aKTIntKosten; }
            set { _aKTIntKosten = value; }
        }
        #endregion

        #region Property Declaration tblAuftraggeber
        private int _auftraggeberID;
        private string _auftraggeberName1;
        private string _auftraggeberName2;
        private string _auftraggeberEMail;
        
        public int AuftraggeberID
        {
            get { return _auftraggeberID; }
            set { _auftraggeberID = value; }
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
        public string AuftraggeberEMail
        {
            get { return _auftraggeberEMail; }
            set { _auftraggeberEMail = value; }
        }
        #endregion

        #region Property Declaration tblGegner
        private int _gegnerID;
        private string _gegnerAnrede;
        private string _gegnerName1;
        private string _gegnerName2;
        private string _gegnerStrasse;
        private string _gegnerZipPrefix;
        private string _gegnerZip;
        private string _gegnerOrt;
        private string _gegnerOldID;
        private string _gegnerLastZipPrefix;
        private string _gegnerLastZip;
        private string _gegnerLastOrt;
        private string _gegnerLastStrasse;
        private string _gegnerLastName1;
        private string _gegnerLastName2;
        private string _gegnerLastName3;
        
        public int GegnerID
        {
            get { return _gegnerID; }
            set { _gegnerID = value; }
        }
        public string GegnerAnrede
        {
            get { return _gegnerAnrede; }
            set { _gegnerAnrede = value; }
        }
        public string GegnerName1
        {
            get { return _gegnerName1; }
            set { _gegnerName1 = value; }
        }
        public string GegnerName2
        {
            get { return _gegnerName2; }
            set { _gegnerName2 = value; }
        }
        public string GegnerStrasse
        {
            get { return _gegnerStrasse; }
            set { _gegnerStrasse = value; }
        }
        public string GegnerZipPrefix
        {
            get { return _gegnerZipPrefix; }
            set { _gegnerZipPrefix = value; }
        }
        public string GegnerZip
        {
            get { return _gegnerZip; }
            set { _gegnerZip = value; }
        }
        public string GegnerOrt
        {
            get { return _gegnerOrt; }
            set { _gegnerOrt = value; }
        }
        public string GegnerOldID
        {
            get { return _gegnerOldID; }
            set { _gegnerOldID = value; }
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

        public bool IsInkassoAkt()
        {
            return AktIntCustInkAktID > 0;
        }
    }
}
