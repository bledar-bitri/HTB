using System;
using System.Collections;
using HTB.Database;
using HTB.Database.Views;

namespace HTBReports
{
    public class Mahnung : Record
    {

        #region Property Declaration

        public int ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int SchuldnerTyp { get; set; }
        public string SchuldnerAnrede { get; set; }
        public string NameFirma { get; set; }
        public string AnsprechpartnerAnrede { get; set; }
        public string AnsprechpartnerVorename { get; set; }
        public string AnsprechpartnerNachname { get; set; }
        public string SchuldnerVorname { get; set; }
        public string SchuldnerNachname { get; set; }
        public string SchuldnerPostInfo { get; set; }
        public string Strasse { get; set; }
        public string LKZ { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }
        public string RechnungsNummer { get; set; }
        public DateTime RechnungsDatum { get; set; }
        public int Aktenzahl { get; set; }
        public string CustomerAktenzahl { get; set; }
        public string KlientName { get; set; }
        public double Forderung { get; set; }
        public DateTime Faelligkeitsdatum { get; set; }
        public double MahnspesenZinsen { get; set; }
        public double Bearbeitung { get; set; }
        public double Mahnung1 { get; set; }
        public double Mahnung2 { get; set; }
        public double Mahnung3 { get; set; }
        public double Mahnung4 { get; set; }
        public double Mahnung5 { get; set; }
        public double Telefonincasso { get; set; }
        public double Meldeergebung { get; set; }
        public double Evidenzhaltung { get; set; }
        public double InterventionWeg { get; set; }
        public double Ratenangebot { get; set; }
        public double Terminverlust { get; set; }
        public double Porto { get; set; }
        public double Bezahlt { get; set; }
        public double Steuer { get; set; }
        public double Gesamtforderung { get; set; }
        public string Waehrung { get; set; }
        public string UeberwisungEmpfaenger { get; set; }
        public string UeberwisungIBAN { get; set; }
        public string UeberwisungBIC { get; set; }
        public string UeberwisungVerwendungszweck1 { get; set; }
        public string UeberwisungVerwendungszweck2 { get; set; }
        
        
        #endregion

        public Mahnung()
        {
            Date = DateTime.Now;
            UeberwisungEmpfaenger = "E.C.P. EUROPEAN CAR PROTECT KG, 5020 SALZBURG";
            UeberwisungIBAN = "AT94 3500 0000 0214 9110";
            UeberwisungBIC = "RVSAAT2S";
            UeberwisungVerwendungszweck2 = "LW";
        }

        public Mahnung(Mahnung mah)
        {
            Assign(mah);
        }
        public Mahnung(EcpMahnung mah)
        {
            Assign(mah);
            Bezahlt += mah.KostenReduktion;
        }
        private ArrayList _invList = new ArrayList();
        public ArrayList InvList
        {
            get { return _invList; }
            set { _invList = value; }
        }

        public void Assign(tblMahnung rec)
        {
            ID = rec.MahnungID;
            Number = rec.MahnungNr;
        }

        public void Assign(qryCustInkAkt akt)
        {
            KlientName = akt.KlientName1 + akt.KlientName2;
            
            SchuldnerTyp = akt.GegnerType;

            switch (SchuldnerTyp)
            {
                case 0:
                    SchuldnerAnrede = "Firma";
                    break;
                case 1:
                    SchuldnerAnrede = "Herr " + akt.GegnerAnrede.Trim();
                    break;
                case 2:
                    SchuldnerAnrede = "Frau " + akt.GegnerAnrede.Trim();
                    break;
            }
            SchuldnerNachname = akt.GegnerLastName1;
            SchuldnerVorname = akt.GegnerLastName2;
            SchuldnerPostInfo = akt.GegnerLastName3;
            NameFirma = akt.GegnerLastName1;
            AnsprechpartnerAnrede = akt.GegnerAnsprechAnrede;
            AnsprechpartnerVorename = akt.GegnerAnsprechVorname;
            AnsprechpartnerNachname = akt.GegnerAnsprech;

            Strasse = akt.GegnerLastStrasse == string.Empty ? akt.GegnerStrasse : akt.GegnerLastStrasse;
            LKZ = akt.GegnerLastZipPrefix == string.Empty ? akt.GegnerZipPrefix : akt.GegnerLastZipPrefix;
            PLZ = akt.GegnerLastZip == string.Empty ? akt.GegnerZip : akt.GegnerLastZip;
            Ort = akt.GegnerLastOrt == string.Empty ? akt.GegnerOrt : akt.GegnerLastOrt;

            RechnungsNummer = akt.CustInkAktKunde;
            RechnungsDatum = akt.CustInkAktInvoiceDate;

            Aktenzahl = akt.CustInkAktID;
            CustomerAktenzahl = akt.CustInkAktAZ;
            UeberwisungVerwendungszweck1 = "AKTENZAHL " + akt.CustInkAktID;
            Waehrung = akt.KlientLKZ.Equals("CH", StringComparison.CurrentCultureIgnoreCase) ? "CHF" : "EUR";
        }

        public void Assign(Mahnung mah)
        {
            ID = mah.ID;
            Number = mah.Number;
            Date = mah.Date;
            SchuldnerTyp = mah.SchuldnerTyp;
            SchuldnerAnrede = mah.SchuldnerAnrede;
            NameFirma = mah.NameFirma;
            AnsprechpartnerAnrede = mah.AnsprechpartnerAnrede;
            AnsprechpartnerVorename = mah.AnsprechpartnerVorename;
            AnsprechpartnerNachname = mah.AnsprechpartnerNachname;
            SchuldnerVorname = mah.SchuldnerVorname;
            SchuldnerNachname = mah.SchuldnerNachname;
            SchuldnerPostInfo = mah.SchuldnerPostInfo;
            Strasse = mah.Strasse;
            LKZ = mah.LKZ;
            PLZ = mah.PLZ;
            Ort = mah.Ort;
            RechnungsNummer = mah.RechnungsNummer;
            RechnungsDatum = mah.RechnungsDatum;
            Aktenzahl = mah.Aktenzahl;
            CustomerAktenzahl = mah.CustomerAktenzahl;
            KlientName = mah.KlientName;
            Forderung = mah.Forderung;
            Faelligkeitsdatum = mah.Faelligkeitsdatum;
            MahnspesenZinsen = mah.MahnspesenZinsen;
            Bearbeitung = mah.Bearbeitung;
            Mahnung1 = mah.Mahnung1;
            Mahnung2 = mah.Mahnung2;
            Mahnung3 = mah.Mahnung3;
            Mahnung4 = mah.Mahnung4;
            Mahnung5 = mah.Mahnung5;
            Telefonincasso = mah.Telefonincasso;
            Meldeergebung = mah.Meldeergebung;
            Evidenzhaltung = mah.Evidenzhaltung;
            InterventionWeg = mah.InterventionWeg;
            Ratenangebot = mah.Ratenangebot;
            Terminverlust = mah.Terminverlust;
            Porto = mah.Porto;
            Bezahlt = mah.Bezahlt;
            Steuer = mah.Steuer;
            Gesamtforderung = mah.Gesamtforderung;
            Waehrung = mah.Waehrung;
            UeberwisungEmpfaenger = mah.UeberwisungEmpfaenger;
            UeberwisungIBAN = mah.UeberwisungIBAN;
            UeberwisungBIC = mah.UeberwisungBIC;
            UeberwisungVerwendungszweck1 = mah.UeberwisungVerwendungszweck1;
            UeberwisungVerwendungszweck2 = mah.UeberwisungVerwendungszweck2;

        }
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
                Bezahlt;
        }

        public void ClearCosts()
        {
            Forderung = 0;
            MahnspesenZinsen = 0;
            Bearbeitung = 0;
            Mahnung1 = 0;
            Mahnung2 = 0;
            Mahnung3 = 0;
            Mahnung4 = 0;
            Mahnung5 = 0;
            Telefonincasso = 0;
            Meldeergebung = 0;
            Evidenzhaltung = 0;
            InterventionWeg = 0;
            Ratenangebot = 0;
            Terminverlust = 0;
            Porto = 0;
            Bezahlt = 0;
            Steuer = 0;
            Gesamtforderung = 0;
        }
    }
}
