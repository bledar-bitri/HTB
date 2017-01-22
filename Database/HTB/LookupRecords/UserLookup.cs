using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.LookupRecords
{
    public class UserLookup
    {
        #region Property Declaration
        private string _userID;
        private string _userDepartment;
        private string _userName;
        private string _userNameLink;
        private int _userStatus;

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public string UserDepartment
        {
            get { return _userDepartment; }
            set { _userDepartment = value; }
        }
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string UserNameLink
        {
            get { return _userNameLink; }
            set { _userNameLink = value; }
        }
        public int UserStatus
        {
            get { return _userStatus; }
            set { _userStatus = value; }
        }
        #endregion
    }
}
