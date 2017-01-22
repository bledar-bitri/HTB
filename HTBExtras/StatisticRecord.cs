namespace HTBExtras
{
    public class StatisticRecord
    {
        public int Id { get; set; }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public int Akts { get; set; }
        public double Amount { get; set; }
        public double PercentAkt { get; set; }
        public double PercentAmount { get; set; }
        public bool IsPositive { get; set; }

        public StatisticRecord() : this ("")
        { 
        }
        public StatisticRecord(string description)
        {
            Description = description;
        }

        
        public void CalcPercent(int totalAkts, double totalAmount)
        {
            CalcPercentAkt(totalAkts);
            CalcPercentAmount(totalAmount);
        }

        public void CalcPercentAkt(int totalAkts)
        {
            if (Akts > 0 && totalAkts > 0)
                PercentAkt = (double)Akts / (double)totalAkts;
            else
                PercentAkt = 0;
        }

        public void CalcPercentAmount(double totalAmount)
        {
            if (Amount > 0 && totalAmount > 0)
                PercentAmount = Amount / totalAmount;
            else
                PercentAmount = 0;
        }
    }
}
