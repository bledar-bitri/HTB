using System;

namespace HTBReports
{
    public class ReportParameters
    {
        DateTime _startDate = new DateTime(1900, 1, 1);
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        DateTime _endDate = new DateTime(1900, 1, 1);
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public int StartKlient { get; set; }
        public int EndKlient { get; set; }

        public int StartAuftraggeber { get; set; }
        public int EndAuftraggeber { get; set; }

        public int KlientSB { get; set; }

        public ReportParameters()
        {
            StartKlient = 0;
        }

        
    }
}
