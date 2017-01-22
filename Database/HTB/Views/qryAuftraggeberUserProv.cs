using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryAuftraggeberUserProv : tblAuftraggeberUserProv
    {
        #region Property Declaration tblAktenIntActionType
        private string _aktIntActionTypeCaption;
        public string AktIntActionTypeCaption
        {
            get { return _aktIntActionTypeCaption; }
            set { _aktIntActionTypeCaption = value; }
        }
        #endregion
    }
}
