using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryLookupUser : Record
    {
        #region Property Declaration
        private int _userID;
        private string _userVorname;
        private string _userNachname;
        private string _userUsername;
        private int _userStatus;
        private string _abteilungCaption;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
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
        public string UserUsername
        {
            get { return _userUsername; }
            set { _userUsername = value; }
        }
        public int UserStatus
        {
            get { return _userStatus; }
            set { _userStatus = value; }
        }
        public string AbteilungCaption
        {
            get { return _abteilungCaption; }
            set { _abteilungCaption = value; }
        }
        
        #endregion
    }
}
