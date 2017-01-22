using System;
using System.Collections;
using HTB.Database;

namespace HTBExtras.KingBill
{
    public class InkassoAkt : Record
    {
        /********** Klient **********/

        public string KlientAnrede { get; set; }
        public string KlientTitel { get; set; }
        public string KlientVorname { get; set; }
        public string KlientNachname { get; set; }
        public int KlientGeschlecht { get; set; }       // [0 = Firma] [1 = Herr] [2 = Frau]
        public string KlientStrasse { get; set; }
        public string KlientLKZ { get; set; }
        public string KlientPLZ { get; set; }
        public string KlientOrt { get; set; }
//        public string KlientTelefonVorwahlLand { get; set; }
//        public string KlientTelefonVorwahl { get; set; }
        public string KlientTelefonNummer { get; set; }
        public string KlientEMail { get; set; }
        public string KlientBLZ { get; set; }
        public string KlientKontonummer { get; set; }
        public string KlientBank { get; set; }
        public string KlientBIC { get; set; }
        public string KlientIBAN { get; set; }
        
        public string KlientFirmenbuchnummer { get; set; }
//        public string KlientVersicherungnummer { get; set; }
//        public string KlientPolizzennummer { get; set; }
        public string KlientAnschprechPartnerAnrede { get; set; }
        public string KlientAnschprechPartnerNachname { get; set; }
        public string KlientAnschprechPartnerVorname { get; set; }
        public DateTime KlientAnschprechPartnerGeburtsdatum { get; set; }
//        public string KlientAnschprechPartnerTelefonVorwahlLand { get; set; }
//        public string KlientAnschprechPartnerTelefonVorwahl { get; set; }
        public string KlientAnschprechPartnerTelefonNummer { get; set; }
        public string KlientAnschprechPartnerEMail { get; set; }
//        public string KlientMemo { get; set; }


        /********** Schuldner **********/
        public string SchuldnerAnrede { get; set; } 
        public string SchuldnerVorname { get; set; }
        public string SchuldnerNachname { get; set; }
        public int SchuldnerGeschlecht { get; set; }    // [0 = Firma] [1 = Herr] [2 = Frau]
        public string SchuldnerStrasse { get; set; }
        public string SchuldnerLKZ { get; set; }
        public string SchuldnerPLZ { get; set; }
        public string SchuldnerOrt { get; set; }
//        public string SchuldnerTelefonVorwahlLand { get; set; }
//        public string SchuldnerTelefonVorwahl { get; set; }
        public string SchuldnerTelefonNummer { get; set; }
        public string SchuldnerEMail { get; set; }
        public DateTime SchuldnerGeburtsdatum { get; set; }
        public string SchuldnerMemo { get; set; }
        
        /********** Workflow **********/
        public bool WorkflowErsteMahnung { get; set; }
        public int WorkflowErsteMahnungsfrist { get; set; }
        public bool WorkflowIntervention { get; set; }
        public int WorkflowInterventionsfrist { get; set; }
        public bool WorkflowZweiteMahnung { get; set; }
        public int WorkflowZweiteMahnungsfrist { get; set; }
        public bool WorkflowDritteMahnung { get; set; }
        public int WorkflowDritteMahnungsfrist { get; set; }
        public bool WorkflowRechtsanwaltMahnung { get; set; }


        /********** Akt **********/
        public string RechnungNummer { get; set; }
        public string RechnungReferencenummer { get; set; }
        public DateTime RechnungDatum { get; set; }
        public double RechnungForderungOffen { get; set; }
        public double RechnungMahnKosten { get; set; }
        public string RechnungMemo { get; set; }

        public string AktSource { get; set; }
        
        public ArrayList Dokumente { get; set; }

        public InkassoAkt()
        {
            Dokumente = new ArrayList();
        }
    }
}
