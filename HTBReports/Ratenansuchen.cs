using System;
using System.IO;
using HTB.Database.Views;
using HTBAktLayer;
using HTBPdf;
using HTBUtilities;
using iTextSharp.text;

namespace HTBReports
{
    public class Ratenansuchen : IReport
    {
        private const int MaxLines = 2550;
        private const int MiddleCol = 1050;
        private const int RightCol = 2000;
//        private const int StartLine = 360;
        private const int StartLine = 300;
        private const int NormalGap = 39;

        private int _col;
        private int _lin;
        private int _gap;

        public ECPPdfWriter Writer { get; set; }
        public qryCustInkAkt Akt { get; set; }
        
        private readonly string _logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");
        private int _seblstBoxFirstLine;

        public Ratenansuchen ()
        {

        }

        public void Init()
        {
            _lin = StartLine;
            _gap = NormalGap;
            _col = 100;
        }

        public void GenerateRatenansuchen(qryCustInkAkt inkAkt, Stream os, bool isOnlyOneAkt = true)
        {
            Akt = inkAkt;
            Init();

            if (isOnlyOneAkt)
                Open(os);
            PrintPageHeader();
            PrintRatenansuchen();
            Close();
        }

        public void PrintRatenansuchen()
        {
            var aktUtils = new AktUtils(Akt.CustInkAktID);
            
            SetLineFont();
//            /*
            Writer.print(_lin += _gap, _col, Akt.GegnerName1);
            Writer.print(_lin += _gap, _col, Akt.GegnerName2);
            Writer.print(_lin += _gap, _col, Akt.GegnerLastStrasse);
            Writer.print(_lin += _gap, _col, Akt.GegnerLastZipPrefix + "-" + Akt.GegnerLastZip+" "+Akt.GegnerLastOrt);

            Writer.print(_lin += _gap * 2, RightCol, Akt.GegnerLastOrt + ", am _______________________", 'R');
            
            Writer.print(_lin += _gap * 2, _col, Akt.AuftraggeberName1);
            Writer.print(_lin += _gap, _col, Akt.AuftraggeberName2);
            Writer.print(_lin += _gap, _col, Akt.AuftraggeberStrasse);
            Writer.print(_lin += _gap, _col, Akt.AuftraggeberLKZ + "-" + Akt.AuftraggeberPLZ + " " + Akt.AuftraggeberOrt);
//            */
            SetHeadingFont();
            Writer.print(_lin += _gap * 2, MiddleCol, "ANERKENNTNIS - RATENANSUCHEN - STUNDUNG", 'C');
            SetLineFont();
            Writer.print(_lin += _gap * 2, MiddleCol, "Aktenzeichen - " + Akt.CustInkAktID, 'C');
            Writer.print(_lin += _gap, MiddleCol, "Forderung von - " + Akt.KlientName1, 'C');

            Writer.print(_lin += _gap * 2, _col, "Sehr geehrte Damen und Herren,");
            Writer.print(_lin += _gap, _col, "da ich die Gesamtforderung aus dem Schreiben vom " + DateTime.Now.ToShortDateString() + " in der Höhe von " + HTBUtils.FormatCurrency(aktUtils.GetAktBalance()) + " EUR");
            Writer.print(_lin += _gap, _col, "innerhalb der gesetzten Frist nicht bezahlen kann, unterbreite ich folgenden Vorschlag.");
            Writer.print(_lin += _gap, _col, "Ich anerkenne hiermit diese Forderung inkl. der aufgeschlüsselten Kosten lt. Beilage, sowie der");
            Writer.print(_lin += _gap, _col, "aufgelaufenen Zinsen und ersuche um Zustimmung zur nachstehenden Ratenzahlung bzw. Stundung:");
            SetLineFont(true, false, false);
            Writer.print(_lin += _gap * 2, _col, "A");
            SetLineFont();
            Writer.print(_lin, _col + 27, " - monatliche und pünktliche Ratenzahlung in der Höhe von _______________ EUR");
            Writer.print(_lin += _gap, _col, "mit dem Zahlungsbeginn der 1.Rate am _______________ (Datum).");
            SetLineFont(true, false, false);
            Writer.print(_lin += _gap * 2, _col, "B");
            SetLineFont();
            Writer.print(_lin, _col + 27, " - Stundung der gesamten Forderung bis zum _______________ (Datum).");
            Writer.print(_lin += _gap * 2, MiddleCol, "Diese Vereinbarung tritt vorbehaltlich der Zustimmung des Klienten in Kraft. Zahlungen werden vorerst", 'C');
            Writer.print(_lin += _gap, MiddleCol, "auf Kosten und Zinsen verrechnet. Im Falle des Verzuges mit auch nur einer Rate von mindestens 6", 'C');
            Writer.print(_lin += _gap, MiddleCol, "Wochen und Nachfristsetzung von 2 Wochen, ist die gesamte Forderung fällig (Terminverlust).", 'C');

            SetHeadingFont();
            Writer.print(_lin += _gap * 2, _col, "SELBSTAUSKUNFT");
            SetHeadingFont(true, false);
            Writer.print(_lin, _col + 375, "(persönliche Daten)");
            _lin += (int) (_gap*1.5);
            DrawSelbstauskunftBox();
            Writer.print(_lin += _gap * 2, _col, "Ich erkläre hiermit die derzeitige Forderung (zzgl. weitere Zinsen und Inkassokosten) ausdrücklich und");
            Writer.print(_lin += _gap, _col, "unwiderruflich anzuerkennen und verpflichte mich die Gesamtforderung zu bezahlen. Ich habe die");
            Writer.print(_lin += _gap, _col, "aufgedruckten Bedingungen zur Gänze gelesen und anerkenne diese vollinhaltlich. Damit mein Ansuchen");
            Writer.print(_lin += _gap, _col, "bearbeitet werden kann, bestätige ich mit meiner Unterschrift, dass meine Angaben wahrheitsgemäß und");
            Writer.print(_lin += _gap, _col, "vollständig sind.");

            Writer.print(_lin += _gap * 2, _col, "Mit freundlichen Grüßen,");
            Writer.print(_lin += _gap, _col, Akt.GegnerName1 + " " + Akt.GegnerName2);
            Writer.print(_lin += _gap * 2, RightCol, "Unterschrift: Ehepartner/Lebensgefährte/Bürge", 'R');

            Writer.print(2900, 1800, DateTime.Now.ToShortDateString(), 'R');
        }

        private void DrawSelbstauskunftBox()
        {
            DrawSelbstauskunftLine("Zahlungspflichtiger", "Ehepartner/Lebensgefährte/Bürge", true);
            DrawSelbstauskunftLine("Vor- und Zuname:");
            DrawSelbstauskunftLine("Anschrift:");
            DrawSelbstauskunftLine("Telefon:");
            DrawSelbstauskunftLine("Geburtsdatum:");
            DrawSelbstauskunftLine("SVA Nummer:");
            DrawSelbstauskunftLine("Beruf:");
            DrawSelbstauskunftLine("Arbeitgeber:", "Arbeitgeber:", false, true);
        }

        private void DrawSelbstauskunftLine(String left)
        {
            DrawSelbstauskunftLine(left, left);
        }

        private void DrawSelbstauskunftLine(String left, String right, bool isFirst = false, bool isLast = false)
        {
            int topLineWidth = isFirst ? 4 : 2;
            int bottomLineWidth = isLast ? 4 : 2;
            
            if (isFirst)
            {
                _seblstBoxFirstLine = _lin;
                for (int i = 0; i < topLineWidth; i++)
                {
                    Writer.drawLine(_lin + i, _col, _lin + i, RightCol);
                }
            }
            
            if(isFirst)
                SetHeadingFont(true, false);
            else 
                SetLineFont();

            _lin += 5;
            if (isFirst)
            {
                Writer.print(_lin, _col + ((MiddleCol - _col)/2), left, 'C');
                Writer.print(_lin, MiddleCol + ((RightCol - MiddleCol)/2), right, 'C');
                _lin += 10;

            }
            else
            {
                Writer.print(_lin, _col + 20, left);
                Writer.print(_lin, MiddleCol + 20, right);
            }

            _lin += _gap + (isLast ? 13 : 10);
            for (int i = 0; i < bottomLineWidth; i++)
            {
                Writer.drawLine(_lin + i, _col, _lin + i, RightCol);
            }

            if (isLast)
            {
                for (int i = 0; i < 4; i++)
                {
                    Writer.drawLine(_seblstBoxFirstLine, _col + i, _lin, _col);
                    Writer.drawLine(_seblstBoxFirstLine, MiddleCol + i, _lin, MiddleCol);
                    Writer.drawLine(_seblstBoxFirstLine, RightCol + i, _lin, RightCol);
                }
            }
        }

        public void Open(Stream os)
        {
            Writer = new ECPPdfWriter();
            Writer.setFormName("A4");
            Writer.open(os);
        }

        public void Close()
        {
            Writer.Close();
        }

        public bool CheckOverflow(int plin)
        {
            throw new System.NotImplementedException();
        }

        public int PrintPageHeader()
        {
            /*
            _lin = StartLine;
            Writer.drawBitmap(350, _col, Image.GetInstance(_logoPath), 40);
            for (int i = 0; i < 3; i++)
            {
                Writer.setFont("Arial", 22, true, false, false);
                Writer.print(50, _col + 1600 + i, "EUROPEAN CAR PROTECT", 'R', BaseColor.BLUE); // give it a bolder look
                Writer.setFont("Arial", 16, true, false, false);
                Writer.print(120, _col + 1600 + i, "INKASSO-SERVICE", 'R', BaseColor.BLUE); // give it a bolder look
            }
            return _lin;
             */
            return StartLine;
        }

        public ECPPdfWriter GetWriter()
        {
            return Writer;
        }

        public void SetLineFont()
        {
            Writer.setFont("Calibri", 11);
        }
        public void SetLineFont(bool bold, bool italics, bool underline)
        {
            Writer.setFont("Calibri", 11, bold, italics, underline);
        }

        private void SetHeadingFont()
        {
           Writer.setFont("Calibri", 13, true, false, true);
        }

        private void SetHeadingFont(bool italics, bool underline)
        {
            Writer.setFont("Calibri", 13, true, italics, underline);
        }
    }
}
