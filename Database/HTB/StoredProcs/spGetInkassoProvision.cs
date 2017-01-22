
using System;

namespace HTB.Database.HTB.StoredProcs 
{
    public class spGetInkassoProvision : Record
    {
        public string UserVorname { get; set; }
        public string UserNachname { get; set; }
        public int CustInkAktID { get; set; }
        public int AktIntID { get; set; }
        public DateTime AktIntActionDate { get; set; }
        public double AktIntActionProvision { get; set; }
    }
}
