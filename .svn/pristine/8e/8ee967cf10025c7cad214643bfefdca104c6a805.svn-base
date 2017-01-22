using System.Collections;
using System.Linq;
using HTB.Database;

namespace HTBExtras
{
    public class AktIntAmounts : Record
    {
        public double Zinsen { get; set; }
        public double InkassoKosten { get; set; }
        public double MWS { get; set; }
        public double Weggebuhr { get; set; }
        public double Zahlungen { get; set; }
        public ArrayList ForderungList { get; set; }
        
        public AktIntAmounts()
        {
            ForderungList = new ArrayList();
        }
        public double GetTotal()
        {
            return GetTotalLessWeggebuhr() + Weggebuhr;
        }

        public double GetTotalLessWeggebuhr()
        {
            return GetTotalForderung() +
                Zinsen +
                InkassoKosten +
                MWS +
                Zahlungen; // this is a negative amount
        }

        public double GetTotalForderung()
        {
            return ForderungList.Cast<AktIntForderungPrintLine>().Sum(pf => pf.Amount);
        }
    }

}
