using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.LookupRecords
{
    public class KlientLookup
    {
        #region Property Declaration
        private int _klientID;
        private string _klientType;
        private string _klientName;
        private string _klientOrt;
        private string _klientOldID;
        private string _klientNameLink;

        public int KlientID
        {
            get { return _klientID; }
            set { _klientID = value; }
        }
        public string KlientType
        {
            get { return _klientType; }
            set { _klientType = value; }
        }
        public string KlientName
        {
            get { return _klientName; }
            set { _klientName = value; }
        }
        public string KlientOrt
        {
            get { return _klientOrt; }
            set { _klientOrt = value; }
        }
        public string KlientOldID
        {
            get { return _klientOldID; }
            set { _klientOldID = value; }
        }
        public string KlientNameLink
        {
            get { return _klientNameLink; }
            set { _klientNameLink = value; }
        }
        #endregion
    }
}
