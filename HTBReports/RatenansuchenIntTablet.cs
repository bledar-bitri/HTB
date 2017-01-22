using System;
using System.IO;
using HTB.Database;
using HTB.Database.Views;
using HTBAktLayer;
using HTBExtras;
using HTBPdf;
using HTBUtilities;
using System.Drawing;

namespace HTBReports
{
    public class RatenansuchenIntTablet : IReport
    {
        private const int MiddleCol = 1050;
        private const int RightCol = 2000;
        private const int StartLine = 30;
        private const int NormalGap = 39;

        private int _col;
        private int _lin;
        private int _gap;

        public ECPPdfWriter Writer { get; set; }
        private qryAktenInt _akt { get; set; }
        private tblAktenIntRatenansuchen _rate { get; set; }
        private AktIntAmounts _aktAmounts;

        private readonly string _logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");
        private int _seblstBoxFirstLine;
        private byte[] _signature;
        private byte[] _partnerSignature;

        public void Init()
        {
            _lin = StartLine;
            _gap = NormalGap;
            _col = 100;
        }
        public void GenerateRatenansuchen(qryAktenInt akt, AktIntAmounts aktAmounts, tblAktenIntRatenansuchen rate, Stream os, bool isOnlyOneAkt = true)
        {
            GenerateRatenansuchen(akt, aktAmounts, rate, os, null, null, isOnlyOneAkt);
        }
        public void GenerateRatenansuchen(qryAktenInt akt, AktIntAmounts aktAmounts, tblAktenIntRatenansuchen rate, Stream os, byte[] signature = null, byte[] partnerSignature = null, bool isOnlyOneAkt = true)
        {
            _akt = akt;
            _rate = rate;
            _aktAmounts = aktAmounts;
            _signature = signature;
            _partnerSignature = partnerSignature;

            Init();

            if (isOnlyOneAkt)
                Open(os);
            PrintPageHeader();
            PrintRatenansuchen();
            Close();
        }

        public void PrintRatenansuchen()
        {
            var aktUtils = new AktUtils(_akt.AktIntID);
            
            SetLineFont();
//            /*
            Writer.print(_lin += _gap, _col, _akt.GegnerName1);
            Writer.print(_lin += _gap, _col, _akt.GegnerName2);
            Writer.print(_lin += _gap, _col, _akt.GegnerLastStrasse);
            Writer.print(_lin += _gap, _col, _akt.GegnerLastZipPrefix + "-" + _akt.GegnerLastZip+" "+_akt.GegnerLastOrt);

            Writer.print(_lin += _gap * 2, RightCol, _akt.GegnerLastOrt + ", am "+DateTime.Now.ToShortDateString(), 'R');
            
            Writer.print(_lin += _gap * 2, _col, _akt.AuftraggeberName1);
            Writer.print(_lin += _gap, _col, _akt.AuftraggeberName2);
            Writer.print(_lin += _gap, _col, _akt.AuftraggeberStrasse);
            Writer.print(_lin += _gap, _col, _akt.AuftraggeberLKZ + "-" + _akt.AuftraggeberPLZ + " " + _akt.AuftraggeberOrt);
//            */
            SetHeadingFont();
            Writer.print(_lin += _gap * 2, MiddleCol, "ANERKENNTNIS - RATENANSUCHEN - STUNDUNG", 'C');
            SetLineFont();
            Writer.print(_lin += _gap * 2, MiddleCol, "Aktenzeichen - " + _akt.AktIntID, 'C');
            Writer.print(_lin += _gap, MiddleCol, "Forderung von - " + _akt.KlientName1, 'C');

            Writer.print(_lin += _gap * 2, _col, "Sehr geehrte Damen und Herren,");
            Writer.print(_lin += _gap, _col, "da ich die Gesamtforderung aus dem Schreiben vom " + DateTime.Now.ToShortDateString() + " in der Höhe von " + HTBUtils.FormatCurrencyNumber(_aktAmounts.GetTotal()) + " EUR");
            Writer.print(_lin += _gap, _col, "innerhalb der gesetzten Frist nicht bezahlen kann, unterbreite ich folgenden Vorschlag.");
            Writer.print(_lin += _gap, _col, "Ich anerkenne hiermit diese Forderung inkl. der aufgeschlüsselten Kosten lt. Beilage, sowie der");
            Writer.print(_lin += _gap, _col, "aufgelaufenen Zinsen und ersuche um Zustimmung zur nachstehenden Ratenzahlung bzw. Stundung:");
            SetLineFont(true, false, false);
            Writer.print(_lin += _gap * 2, _col, "A");
            SetLineFont();
            Writer.print(_lin, _col + 27, " - monatliche und pünktliche Ratenzahlung in der Höhe von " + HTBUtils.FormatCurrencyNumber(_rate.AktIntRateRequestRateAmount)+" EUR");
            Writer.print(_lin += _gap, _col, "mit dem Zahlungsbeginn der 1.Rate am " + _rate.AktIntRateRequestStartDate.ToShortDateString() + " (Datum).");
            if (_rate.AktIntRateRequestPayment > 0)
            {
                Writer.print(_lin += _gap, _col, "Eine Anzahlung über " + HTBUtils.FormatCurrencyNumber(_rate.AktIntRateRequestPayment) + " EUR wurden per heutigem Datum an");
                Writer.print(_lin += _gap, _col, HTBUtils.GetADSalutationAndName(_akt.AktIntID, false, true) + " (Partner/in der Firma E.C.P.) gegen Aushändigung einer Quittung bezahlt.");
            }

            SetLineFont(true, false, false);
            Writer.print(_lin += _gap * 2, _col, "B");
            SetLineFont();
            Writer.print(_lin, _col + 27, " - Stundung der gesamten Forderung bis zum "+_rate.AktIntRateRequestEndDate.ToShortDateString()+" (Datum).");
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
            Writer.print(_lin += _gap, _col, _akt.GegnerName1 + " " + _akt.GegnerName2);
            Writer.print(_lin += _gap * 2, RightCol, "Unterschrift: Ehepartner/Lebensgefährte/Bürge", 'R');
            if(_signature != null)
            {
                Writer.drawBitmap(2800, _col, _signature, 20);
            }
            if (_partnerSignature != null)
            {
                Writer.drawBitmap(2850, 1300, _partnerSignature, 20);
            }
            Writer.print(2900, 1800, DateTime.Now.ToShortDateString(), 'R');
        }


        private void DrawSelbstauskunftBox()
        {
            DrawSelbstauskunftLine("Zahlungspflichtiger", "Ehepartner/Lebensgefährte/Bürge", true);
            DrawSelbstauskunftLine(_rate.AktIntRateRequestName, _rate.AktIntRateRequestNamePartner,false, false, "Vor- und Zuname:");
            DrawSelbstauskunftLine(_rate.AktIntRateRequestAddress, _rate.AktIntRateRequestAddressPartner, false, false, "Anschrift:");
            DrawSelbstauskunftLine(_rate.AktIntRateRequestPhone, _rate.AktIntRateRequestPhonePartner, false, false, "Telefon:");
            DrawSelbstauskunftLine(HTBUtils.IsDateValid(_rate.AktIntRateRequestDOB) ? _rate.AktIntRateRequestDOB.ToShortDateString() : "", HTBUtils.IsDateValid(_rate.AktIntRateRequestDOBPartner) ? _rate.AktIntRateRequestDOBPartner.ToShortDateString() : "", false, false, "Geburtsdatum:");
            DrawSelbstauskunftLine(_rate.AktIntRateRequestSVA, _rate.AktIntRateRequestSVAPartner, false, false, "SVA Nummer:");
            DrawSelbstauskunftLine(_rate.AktIntRateRequestJob, _rate.AktIntRateRequestJobPartner, false, false, "Beruf:");
            DrawSelbstauskunftLine(_rate.AktIntRateRequestEmployer, _rate.AktIntRateRequestEmployerPartner, false, true, "Arbeitgeber:");
        }

        private void DrawSelbstauskunftLine(String left)
        {
            DrawSelbstauskunftLine(left, left);
        }

        private void DrawSelbstauskunftLine(String left, String right, bool isFirst = false, bool isLast = false, string header = null)
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
                if(!string.IsNullOrEmpty(header))
                {
                    SetSmallHeaderFont();
                    Writer.print(_lin, _col + 20, header);
                    Writer.print(_lin, MiddleCol + 20, header);
                    _lin += (int) (_gap*1.3);
                }
                SetLineFont();
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
        public void SetSmallHeaderFont()
        {
            Writer.setFont("Calibri", 9, false, true, false);
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
