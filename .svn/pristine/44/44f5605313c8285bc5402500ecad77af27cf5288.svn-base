using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTBReports
{
    public class EcpMahnung : Mahnung
    {
        public double KostenReduktion { get; set; }

        public void CalculateTotal()
        {
            Gesamtforderung = Forderung +
                MahnspesenZinsen +
                Bearbeitung +
                Mahnung1 +
                Mahnung2 +
                Mahnung3 +
                Mahnung4 +
                Mahnung5 +
                Telefonincasso +
                Meldeergebung +
                Evidenzhaltung +
                InterventionWeg +
                Ratenangebot +
                Terminverlust +
                Porto +
                Steuer -
                KostenReduktion -
                Bezahlt;
        }

    }
}
