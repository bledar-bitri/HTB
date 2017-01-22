using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.StoredProcs
{
    public class spProvisionDetailAG : Record
    {
        #region Property Declaration
        private int _auftraggeberID;
        public int AuftraggeberID
        {
            get { return _auftraggeberID; }
            set { _auftraggeberID = value; }
        }

        private string _auftraggeberName1;
        public string AuftraggeberserNachname
        {
            get { return _auftraggeberName1; }
            set { _auftraggeberName1 = value; }
        }

        private string _aktIntActionTypeCaption;
        public string AktIntActionTypeCaption
        {
            get { return _aktIntActionTypeCaption; }
            set { _aktIntActionTypeCaption = value; }
        }

        private double _aktIntActionProvision;
        public double AktIntActionProvision
        {
            get { return _aktIntActionProvision; }
            set { _aktIntActionProvision = value; }
        }

        private double _aktIntActionPrice;
        public double AktIntActionPrice
        {
            get { return _aktIntActionPrice; }
            set { _aktIntActionPrice = value; }
        }

        private int _aktIntID;
        public int AktIntID
        {
            get { return _aktIntID; }
            set { _aktIntID = value; }
        }

        private DateTime _aktIntActionDate;
        public DateTime AktIntActionDate
        {
            get { return _aktIntActionDate; }
            set { _aktIntActionDate = value; }
        }

        private string _aktTypeINTCaption;
        public string AktTypeINTCaption
        {
            get { return _aktTypeINTCaption; }
            set { _aktTypeINTCaption = value; }
        }
        #endregion
    }
}
