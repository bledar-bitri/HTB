using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryAktAktionKosten
    {
        #region Property Declaration KostenArt
        private int _kostenArtID;
        private string _kostenArtText;
        private bool _isTaxable;
        private int _seqNummer;
        private bool _isImErstenMahnung;
        private bool _isImFolgendMahnung;
        private bool _isZinsen;
        public int KostenArtID
        {
            get { return _kostenArtID; }
            set { _kostenArtID = value; }
        }
        public string KostenArtText
        {
            get { return _kostenArtText; }
            set { _kostenArtText = value; }
        }
        public bool IsTaxable
        {
            get { return _isTaxable; }
            set { _isTaxable = value; }
        }
        public int SeqNummer
        {
            get { return _seqNummer; }
            set { _seqNummer = value; }
        }
        public bool IsImErstenMahnung
        {
            get { return _isImErstenMahnung; }
            set { _isImErstenMahnung = value; }
        }
        public bool IsImFolgendMahnung
        {
            get { return _isImFolgendMahnung; }
            set { _isImFolgendMahnung = value; }
        }
        public bool IsZinsen
        {
            get { return _isZinsen; }
            set { _isZinsen = value; }
        }
        #endregion

        #region Property Declaration AktAktionen
        private int _aktAktionID;
        private string _aktAktionCode;
        private string _aktAktionCaption;
        private string _aktAktionText;
        private int _aktAktionType;
        public int AktAktionID
        {
            get { return _aktAktionID; }
            set { _aktAktionID = value; }
        }
        public string AktAktionCode
        {
            get { return _aktAktionCode; }
            set { _aktAktionCode = value; }
        }
        public string AktAktionCaption
        {
            get { return _aktAktionCaption; }
            set { _aktAktionCaption = value; }
        }
        public string AktAktionText
        {
            get { return _aktAktionText; }
            set { _aktAktionText = value; }
        }
        public int AktAktionType
        {
            get { return _aktAktionType; }
            set { _aktAktionType = value; }
        }
        #endregion

        #region Property Declaration WFK
        private int _wFPID;
        private int _wFPPosition;
        private int _wFPAktion;
        private int _wFPPreTime;
        private int _wFPKlient;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID)]
        public int WFPID
        {
            get { return _wFPID; }
            set { _wFPID = value; }
        }
        public int WFPPosition
        {
            get { return _wFPPosition; }
            set { _wFPPosition = value; }
        }
        public int WFPAktion
        {
            get { return _wFPAktion; }
            set { _wFPAktion = value; }
        }
        public int WFPPreTime
        {
            get { return _wFPPreTime; }
            set { _wFPPreTime = value; }
        }
        public int WFPKlient
        {
            get { return _wFPKlient; }
            set { _wFPKlient = value; }
        }
        #endregion

        #region Property Declaration WFA
        private int _wFPAkt;
        public int WFPAkt
        {
            get { return _wFPAkt; }
            set { _wFPAkt = value; }
        }
        #endregion
    }
}
