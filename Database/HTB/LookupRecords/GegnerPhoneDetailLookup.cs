
using System;

namespace HTB.Database.LookupRecords
{
    public class GegnerPhoneDetailLookup : Record
    {
        #region Property Declaration
        public int GegnerID { get; set; }
        public string GegnerPhoneCountry { get; set; }
        public string GegnerPhoneCity { get; set; }
        public string GegnerPhone { get; set; }
        
        #endregion
    }
}
