using System;
using System.Collections;
using System.IO;
using HTB.Database.Views;
using iTextSharp.text;

namespace HTBReports
{
    public class UebergabenAkten : BasicReport
    {
        private qryAkten _akt;
        private int _rightLine;

        public void GenerateUebergabenAktenPDF(ArrayList aktList, Stream os)
        {
            Init(os);
            _rightLine = col + 1600;
            foreach (qryAkten akt in aktList)
                Generate(akt);

            writer.Close();
        }
        private void Generate(qryAkten akt)
        {
            this._akt = akt;
            lin = PrintPageHeader();
            PrintBericht();
        }

        public override int PrintPageHeader()
        {
            if (pageNumber > 1)
                writer.newPage();
            lin = startLine;
            writer.PrintLeftEcpInfo();
            writer.drawBitmap(350, col, Image.GetInstance(logoPath), 66);
            for (int i = 0; i < 3; i++)
            {
                writer.setFont("Arial", 22, true, false, false);
                writer.print(50, col + 1600 + i, "EUROPEAN CAR PROTECT", 'R', BaseColor.BLUE); // give it a bolder look
                writer.setFont("Arial", 16, true, false, false);
                writer.print(120, col + 1600 + i, "INKASSO-SERVICE", 'R', BaseColor.BLUE); // give it a bolder look
            }
            
            lin += gap + 20;
            writer.setFont("Calibri", 8);
            writer.print((lin += gap), col1, "E.C.P. European Car Protect KG");
            writer.print((lin += gap), col1, "Schwarzparkstrasse 15   A 5020 Salzburg");
            writer.drawLine(lin + 30, col1, lin + 30, col1 + 465);
            lin += gap;
            SetLineFont();
            writer.print((lin += gap), col1, _akt.KlientName1);
            writer.print((lin += gap), col1, _akt.KlientName2);
            writer.print((lin += gap), col1, _akt.KlientName3);
                
            writer.print((lin += gap), col1, _akt.KlientStrasse);
            writer.print((lin += gap), col1, _akt.KlientLKZ + " - " + _akt.KlientPLZ + " " + _akt.KlientOrt);
            
            SetLineFont();
            writer.print((lin += gap), _rightLine, "Salzburg, am   " + DateTime.Now.ToShortDateString(), 'R');
            writer.print((lin += gap), _rightLine, "Unser AZ:   " + _akt.AktCaption, 'R');
            writer.print((lin += gap), _rightLine, "Ihr AZ:   " + _akt.AktIZ, 'R');
            string gegnerName = _akt.GegnerName1 + " " + _akt.GegnerName2;
            if (_akt.GegnerGebDat.ToShortDateString() != "01.01.1900")
                gegnerName += " " + _akt.GegnerGebDat.ToShortDateString();
            lin += gap * 5;
            SetHeadingFont();
            writer.print((lin += gap), col3, "Anschrifterhebung: " + gegnerName);
            lin += gap;
            writer.print((lin += gap), col3+200, _akt.GegnerStrasse+", "+_akt.GegnerZip+" "+_akt.GegnerOrt);
            lin += gap * 3;
            pageNumber++;
            
            return lin;
        }

        private void PrintBericht()
        {

            SetLineFont();
            lin = HTBReports.ReportUtils.PrintTextInMultipleLines(this, lin, col3, gap, _akt.AKTBericht, 90);

            lin = 2350;
            writer.print((lin+=gap*2), col3, "Mit freundlichem Gruß  [with kind regards]");
            writer.print((lin += gap), col3, "ECP - European Car Protect");
            writer.print((lin += gap*2), col3, "Inh. Thomas Jaky, Helmut Ammer");
            writer.print((lin += gap * 2), col3, "+43 (0) 662 20 34 10 - 0");
            writer.print((lin += gap), col3, "office@ecp.or.at");
            writer.print((lin += gap), col3, "www.ecp.or.at");
            int tmpLine = lin + gap;
            
            lin = 2400;
            writer.print((lin += gap), _rightLine, "Kosten der Erhebung   "+HTBUtilities.HTBUtils.FormatCurrency(_akt.AKTPreis), 'R');
            writer.print((lin += gap), _rightLine, "zzgl. 20% MWSt.   " + HTBUtilities.HTBUtils.FormatCurrency(_akt.AKTPreis * .2), 'R');
            writer.print((lin += gap * 2), _rightLine, "GESAMTBETRAG   " + HTBUtilities.HTBUtils.FormatCurrency(_akt.AKTPreis * 1.2), 'R');

            lin = tmpLine;
            writer.setFont("Calibri", 8);
            writer.print((lin += gap), col3, "Unsere Auskünfte werden entsprechend unseren Geschäftsbedinungen und unter Ausschluss jeder Haftung erteilt");
            writer.print((lin += gap), col3, "und sind streng vertraulich. Für jegliche Indiskretion haftet der Auftraggeber, bzw. der Empfänger");
            
        }
    }
}
