using System;

namespace HTB.Database.HTB.StoredProcs
{
    public class spGetPromoterProvision : Record
    {
        public int ID { get; set; }
		public int AktId { get; set; }
		public string AktAZ { get; set; }
        public string AktStatus { get; set; }
        public DateTime AktDate { get; set; }
        public double KlientAmount { get; set; }
		public double ECPAmount { get; set; }
		public double KlientAmountReceived { get; set; }
		public double ECPAmountReceived { get; set; }
		public double ProjectedProvision { get; set; }
		public double Provision { get; set; }
		public double ProvisionTransferred { get; set; }
		public string GegnerName { get; set; }
        public string KlientName { get; set; }
    }
}
