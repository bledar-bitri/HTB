using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryCustInkAkt : tblCustInkAkt
    {
        #region Property Declaration tblKlient
        private int _klientID;
        private string _klientName1;
        private string _klientName2;
        private string _klientName3;
        private string _klientStrasse;
        private string _klientLKZ;
        private string _klientPLZ;
        private string _klientOrt;
        private string _klientEMail;
        private string _klientOldID;
        
        public int KlientID
        {
            get { return _klientID; }
            set { _klientID = value; }
        }
        public string KlientName1
        {
            get { return _klientName1; }
            set { _klientName1 = value; }
        }
        public string KlientName2
        {
            get { return _klientName2; }
            set { _klientName2 = value; }
        }
        public string KlientName3
        {
            get { return _klientName3; }
            set { _klientName3 = value; }
        }
        public string KlientStrasse
        {
            get { return _klientStrasse; }
            set { _klientStrasse = value; }
        }
        public string KlientLKZ
        {
            get { return _klientLKZ; }
            set { _klientLKZ = value; }
        }
        public string KlientPLZ
        {
            get { return _klientPLZ; }
            set { _klientPLZ = value; }
        }
        public string KlientOrt
        {
            get { return _klientOrt; }
            set { _klientOrt = value; }
        }
        public string KlientEMail
        {
            get { return _klientEMail; }
            set { _klientEMail = value; }
        }
        public string KlientOldID
        {
            get { return _klientOldID; }
            set { _klientOldID = value; }
        }
        #endregion

        #region Property Declaration tblUser
        private int _userID;
        private string _userVorname;
        private string _userNachname;
        private string _userEMailOffice;
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
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
        public string UserEMailOffice
        {
            get { return _userEMailOffice; }
            set { _userEMailOffice = value; }
        }
        #endregion

        #region Property Declaration tblAuftraggeber
        private string _auftraggeberName1;
        private string _auftraggeberName2;
        private string _auftraggeberStrasse;
        private string _auftraggeberLKZ;
        private string _auftraggeberPLZ;
        private string _auftraggeberOrt;
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

        #region Property Declaration tblGegner
        private int _gegnerID;
        private string _gegnerAnrede;
        private string _gegnerName1;
        private string _gegnerName2;
        private string _gegnerStrasse;
        private string _gegnerZipPrefix;
        private string _gegnerZip;
        private string _gegnerOrt;
        private DateTime _gegnerGebDat;
        private string _gegnerOldID;
        private int _gegnerVVZ;
        private DateTime _gegnerVVZDate;
        private string _gegnerLastZipPrefix;
        private string _gegnerLastZip;
        private string _gegnerLastOrt;
        private string _gegnerLastStrasse;
        private string _gegnerLastName1;
        private int _gegnerType;
        private string _gegnerAnsprechAnrede;
        private string _gegnerAnsprechVorname;
        private string _gegnerAnsprech;

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
        public DateTime GegnerGebDat
        {
            get { return _gegnerGebDat; }
            set { _gegnerGebDat = value; }
        }
        public string GegnerOldID
        {
            get { return _gegnerOldID; }
            set { _gegnerOldID = value; }
        }
        public int GegnerVVZ
        {
            get { return _gegnerVVZ; }
            set { _gegnerVVZ = value; }
        }
        public DateTime GegnerVVZDate
        {
            get { return _gegnerVVZDate; }
            set { _gegnerVVZDate = value; }
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

        public string GegnerLastName2 { get; set; }
        public string GegnerLastName3 { get; set; }

        public int GegnerType
        {
            get { return _gegnerType; }
            set { _gegnerType = value; }
        }
        public string GegnerAnsprechAnrede
        {
            get { return _gegnerAnsprechAnrede; }
            set { _gegnerAnsprechAnrede = value; }
        }
        public string GegnerAnsprechVorname
        {
            get { return _gegnerAnsprechVorname; }
            set { _gegnerAnsprechVorname = value; }
        }
        public string GegnerAnsprech
        {
            get { return _gegnerAnsprech; }
            set { _gegnerAnsprech = value; }
        }
        #endregion

        #region Property Declaration tblKZ
        private string _kZKZ;
        private string _kZCaption;

        public string KZKZ
        {
            get { return _kZKZ; }
            set { _kZKZ = value; }
        }
        public string KZCaption
        {
            get { return _kZCaption; }
            set { _kZCaption = value; }
        }
        #endregion
    }
}
