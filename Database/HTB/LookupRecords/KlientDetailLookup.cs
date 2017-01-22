
using System;

namespace HTB.Database.LookupRecords
{
    public class KlientDetailLookup
    {
        #region Property Declaration
        public int KlientID { get; set; }
        public string KlientOldID { get; set; }
        public string KlientType { get; set; }
        public string KlientName { get; set; }
        public string KlientLKZ { get; set; }
        public string KlientOrt { get; set; }
        public string KlientStrasse { get; set; }
        public string KlientPhoneCountry { get; set; }
        public string KlientPhoneCity { get; set; }
        public string KlientPhone { get; set; }
        
        public int InterventionAkte { get; set; }
        public int InkassoAkte { get; set; }
        public double InkassoBalance { get; set; }

        #endregion
    }
}
