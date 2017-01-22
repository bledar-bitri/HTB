using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryUsers : tblUser
    {
        #region Property Declaration tblAbteilung
        private string _abteilungCaption;
        public string AbteilungCaption
        {
            get { return _abteilungCaption; }
            set { _abteilungCaption = value; }
        }
        #endregion

        #region Property Declaration tblState
        private string _tblStateCaption;
        public string tblStateCaption
        {
            get { return _tblStateCaption; }
            set { _tblStateCaption = value; }
        }
        #endregion

        #region Property Declaration tblUserLevel
        private int _levelID;
        private int _levelLevel;
        private string _levelCaption;
        public int LevelID
        {
            get { return _levelID; }
            set { _levelID = value; }
        }
        public int LevelLevel
        {
            get { return _levelLevel; }
            set { _levelLevel = value; }
        }
        public string LevelCaption
        {
            get { return _levelCaption; }
            set { _levelCaption = value; }
        }
        #endregion

        #region Property Declaration
        private string _mandantCaption;
        public string MandantCaption
        {
            get { return _mandantCaption; }
            set { _mandantCaption = value; }
        }
        #endregion
    }
}
