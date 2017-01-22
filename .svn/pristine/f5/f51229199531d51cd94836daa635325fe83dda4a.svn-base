using System;
using System.Collections;
using System.IO;
using HTB.Database;
using HTB.Database.Views;
using HTBPdf;
using HTBUtilities;

namespace HTBReports
{
    public class RatenPlan : IReport
    {
        private const int MaxLines = 2550;
        private const int MiddleCol = 1050;
        private const int RightCol = 2000;
        private const int StartLine = 300;
        private const int NormalGap = 39;

        private int _col;
        private int _lin;
        private int _gap;

        public ECPPdfWriter Writer { get; set; }
        public qryCustInkAkt Akt { get; set; }
        
        private readonly string _logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");
        //private int _seblstBoxFirstLine;

        public RatenPlan()
        {

        }

        public void Init()
        {
            _lin = StartLine;
            _gap = NormalGap;
            _col = 100;
        }

        public void GenerateRatenPlan(qryCustInkAkt inkAkt, Stream os, bool isOnlyOneAkt = true)
        {
            Akt = inkAkt;
            Init();

            if (isOnlyOneAkt)
                Open(os);
            PrintPageHeader();
            PrintRatenplan();
            Close();
        }

        public void PrintRatenplan()
        {
            
            
            ArrayList ratesList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktRate WHERE CustInkAktRateAktID = " + Akt.CustInkAktID + " AND CustInkAktRateBalance > 0", typeof(tblCustInkAktRate));
            int middle = _col + (RightCol - _col)/2;
            int left = middle - 300;
            int right = middle + 300;
            _gap += 20;
            SetHeadingFont();
            PrintRatenHeading(left, middle, right);
            SetLineFont();

            foreach (tblCustInkAktRate rate in ratesList)
            {
                Writer.drawLine(_lin, left, _lin, right);
                Writer.drawLine(_lin + _gap, left, _lin, left);
                Writer.drawLine(_lin + _gap, middle, _lin, middle);
                Writer.drawLine(_lin + _gap, right, _lin, right);
                Writer.print(_lin + 5, left+10, rate.CustInkAktRateDueDate.ToShortDateString());
                Writer.print(_lin + 5, right - 10, HTBUtils.FormatCurrency(rate.CustInkAktRateBalance), 'R');
                _lin += _gap;
                Writer.drawLine(_lin, left, _lin, right);
                if(_lin >= MaxLines)
                {
                    Writer.print(2900, 1800, DateTime.Now.ToShortDateString(), 'R');
                    Writer.newPage();
                    PrintPageHeader();
                    SetHeadingFont();
                    PrintRatenHeading(left, middle, right);
                    SetLineFont();
                }
            }
            Writer.print(2900, 1800, DateTime.Now.ToShortDateString(), 'R');
        }

        private void PrintRatenHeading(int left, int middle, int right)
        {
            Writer.drawLine(_lin, left, _lin, right);
            Writer.drawLine(_lin + _gap, left, _lin, left);
            Writer.drawLine(_lin + _gap, middle, _lin, middle);
            Writer.drawLine(_lin + _gap, right, _lin, right);
            Writer.print(_lin, middle - (middle - left) / 2, "Ratendatum", 'C');
            Writer.print(_lin, middle + (right - middle) / 2, "Ratenbetrag", 'C');
            _lin += _gap;
            Writer.drawLine(_lin, left, _lin, right);
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
//            /*
            _lin = StartLine;
            Writer.print(_lin += _gap, _col, Akt.GegnerLastName1);
            Writer.print(_lin += _gap, _col, Akt.GegnerLastName2);
            Writer.print(_lin += _gap, _col, Akt.GegnerLastStrasse);
            Writer.print(_lin += _gap, _col, Akt.GegnerLastZipPrefix + "-" + Akt.GegnerLastZip+" "+Akt.GegnerLastOrt);

//            Writer.print(_lin += _gap * 2, RightCol, Akt.GegnerLastOrt + ", am _______________________", 'R');
            
            Writer.print(_lin += _gap * 2, _col, Akt.AuftraggeberName1);
            Writer.print(_lin += _gap, _col, Akt.AuftraggeberName2);
            Writer.print(_lin += _gap, _col, Akt.AuftraggeberStrasse);
            Writer.print(_lin += _gap, _col, Akt.AuftraggeberLKZ + "-" + Akt.AuftraggeberPLZ + " " + Akt.AuftraggeberOrt);

            SetHeadingFont();
            Writer.print(_lin += _gap * 2, MiddleCol, "RATENPLAN", 'C');
//            SetLineFont();
            Writer.print(_lin += _gap * 2, MiddleCol, "Aktenzeichen - " + Akt.CustInkAktID, 'C');
            Writer.print(_lin += _gap, MiddleCol, "Forderung von - " + Akt.KlientName1, 'C');
            _lin += _gap*2;
            return _lin;
//             */
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
