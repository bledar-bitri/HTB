using System;

namespace HTBReports
{
    public class TerminverlustRec : Mahnung
    {

        public TerminverlustRec() : base() 
        {
            RateDate = new DateTime(1900, 1,1);
        }
        public TerminverlustRec(EcpTerminverlustRec ecpTvl)
        {
            Assign(ecpTvl);

            RateID = ecpTvl.RateID;
            RateDate = ecpTvl.RateDate;
            Bezahlt += ecpTvl.KostenReduktion;
        }

        public int RateID { get; set; }
        public DateTime RateDate { get; set; }
    }
}
