using System.Collections;
using HTB.Database;

namespace HTBExtras.KingBill
{
    public class InkassoAktStatusResponse : Record
    {
        public int EcpAktNummer { get; set; }
        public string Inkassomemo { get; set; }
        public int AktHauptStatusCode { get; set; }
        public string AktHauptStatusBeschreibung { get; set; }
        public int AktSecundaerenStatusCode { get; set; }
        public string AktSecundaerenStatusBeschreibung { get; set; }
        public string Interventionsmemo { get; set; }
        public string Meldememo { get; set; }
        public int ResponeCode { get; set; }
        public string ErrorMessage { get; set; }

        public ArrayList Aktionen { get; set; }
        public ArrayList Dokumente { get; set; }

        public InkassoAktStatusResponse()
        {
            EcpAktNummer = 0;
            Inkassomemo = string.Empty;
            Interventionsmemo = string.Empty;
            ResponeCode = 0;
            ErrorMessage = string.Empty;
            Meldememo = string.Empty;
            
            Aktionen = new ArrayList();
            Dokumente = new ArrayList();
            
        }
    }
}
