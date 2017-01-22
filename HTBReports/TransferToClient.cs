using System;
using System.Collections;
using System.Text;
using HTB.Database;
using HTBPdf;
using HTBUtilities;
using iTextSharp.text;
using System.IO;
using HTB.Database.Views;
using HTBExtras;

namespace HTBReports
{
    public class TransferToClient : IReport
    {
        private int _col;
        private int _col1;
        private int _col2;
        private int _col3;
        private int _col4;
        private int _col5;
        private int _col6;
        private int _col7;
        private int _col8;
        private int _col9;
        private const int MaxLines = 2550;
        private int _startLine;
        private int _lin;
        private int _gap;
        private int _normalGap;

        private readonly string _logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");
        private ECPPdfWriter _writer;
        private tblKlient _klient;
        private int _pageNumber = 1;
        private DateTime _startDate, _endDate;

        public void GenerateTransferList(Stream os, tblKlient klient, ArrayList transfers, DateTime startDate, DateTime endDate)
        {
            _klient = klient;
            _startDate = startDate;
            _endDate = endDate;

            Init();
            Open(os);

            PrintPageHeader();
            _lin = PrintTransfers(transfers);
            _lin += _gap * 2;
            _lin = NewPageOnOverflow();
            PrintFooter();
            Close();
            
        }

        private void Init()
        {
            _startLine = 400;
            _lin = _startLine;
            _normalGap = 37;
            _gap = _normalGap;
            _col = 30;
            _col1 = _col;  // this is the actual start columng (col 1)

            _col2 = _col1 + 220;  // Aktenzeichen     
            _col3 = _col2 + 370;  // Name Schuldner
            _col4 = _col3 + 250;  // Rg. Nummer
            _col5 = _col4 + 250;  // Kundennummer
            _col6 = _col5 + 200;  // Datum
            _col7 = _col6 + 250;  // Betrag
            _col8 = _col7 + 250;  // Buchung Kapital
            _col9 = _col8 + 250;  // Buchung Zinsen
            
        }

        private void Open(Stream os)
        {
            _writer = new ECPPdfWriter();
            _writer.setFormName("A4");
            _writer.open(os);
        }

        private void Close()
        {
            _writer.Close();
        }
        public int PrintPageHeader()
        {
            _lin = _startLine;
            _writer.drawBitmap(350, _col, Image.GetInstance(_logoPath), 40);
            for (int i = 0; i < 3; i++)
            {
                _writer.setFont("Arial", 22, true, false, false);
                _writer.print(50, _col + 1600 + i, "EUROPEAN CAR PROTECT", 'R', BaseColor.BLUE); // give it a bolder look
                _writer.setFont("Arial", 16, true, false, false);
                _writer.print(120, _col + 1600 + i, "INKASSO-SERVICE", 'R', BaseColor.BLUE); // give it a bolder look
            }
            if (_pageNumber == 1)
            {
                _lin += _gap + 20;
                var contact = (tblAnsprechpartner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAnsprechpartner WHERE AnsprechKlient = " + _klient.KlientID, typeof(tblAnsprechpartner));
                _writer.setFont("Calibri", 8);
                _writer.print((_lin += _gap), _col1, "E.C.P. European Car Protect KG");
                _writer.print((_lin += _gap), _col1, "Schwarzparkstrasse 15   A 5020 Salzburg");
                _writer.drawLine(_lin + 30, _col1, _lin + 30, _col1 + 465);
                _lin += _gap;
                SetLineFont();
                _writer.print((_lin += _gap), _col1, _klient.KlientName1);
                _writer.print((_lin += _gap), _col1, _klient.KlientName2);
                if (contact != null)
                {
                    _writer.print((_lin += _gap), _col1, "z.H.  " + contact.AnsprechTitel + " " + contact.AnsprechVorname + " " + contact.AnsprechNachname);
                }

                _writer.print((_lin += _gap), _col1, _klient.KlientStrasse);
                _writer.print((_lin += _gap), _col1, _klient.KlientLKZ + " - " + _klient.KlientPLZ + " " + _klient.KlientOrt);
            }
            SetLineFont();
            _writer.print((_lin += _gap), _col9, "Salzburg, am " + DateTime.Now.ToShortDateString(), 'R');
            _lin += _gap * 5;
            SetLineFont();
            _writer.print(2800, 1900, "Seite " + _pageNumber);
            _pageNumber++;
            
            SetHeadingFont();
            _writer.print(_lin, _col, "Überweisungen: ");
            SetLineFont();
            _writer.print(_lin+8, _col+320,  _startDate.ToShortDateString() + " - " + _endDate.ToShortDateString());
            _lin += _gap * 2;
            SetLineFont();
            PrintHeaderLine();
            return _lin;
        }

        private void PrintBericht(string info)
        {
            SetLineFont();
            _writer.print(_lin, _col3, "Sehr geehrter Damen und Herren,");
            _lin += _gap * 2;
            _lin = ReportUtils.PrintTextInMultipleLines(this, _lin, _col3, _gap, info, 100);
            _lin += _gap;
        }

        private int PrintTransfers(ArrayList list)
        {
            double totalAmount = 0;
            
            #region Print Transfers To Client
            SetLineFont();
            foreach (KlientTransferRecord inv in list)
            {
                PrintInvoiceLine(inv);
                totalAmount += inv.TransferAmount;
                NewPageOnOverflow();
            }
            #endregion
             
            _lin += _gap;
            SetHeadingFont();
            _writer.print(_lin, _col7 - 10, "Total überwiesen:                             " + HTBUtils.FormatCurrency(totalAmount), 'R');
            SetLineFont();
            return _lin;
        }

        private void PrintInvoiceLine(KlientTransferRecord rec)
        {
            const int margin = 10;
            const int maxGegnerNameChars = 20;
            int gap = _gap + margin;

            _writer.drawRectangle(_lin, _col1, _lin +gap, _col9, BaseColor.WHITE);

            _writer.print(_lin + 5, _col1 + margin, string.IsNullOrEmpty(rec.AktAZ) ? rec.AktId.ToString() : rec.AktAZ);
            _writer.print(_lin + 5, _col2 + margin, rec.GegnerName.Length < maxGegnerNameChars ? rec.GegnerName : rec.GegnerName.Substring(0, maxGegnerNameChars) + "...");
            _writer.print(_lin + 5, _col3 + margin, rec.KlientInvoiceNumber);
            _writer.print(_lin + 5, _col4 + margin, rec.KlientCustomerNumber);
            _writer.print(_lin + 5, _col5 + ((_col6 - _col5)/2), rec.TransferDate.ToShortDateString(), 'C');
            _writer.print(_lin + 5, _col7 - margin, HTBUtils.FormatCurrency(rec.TransferAmount), 'R');
            _writer.print(_lin + 5, _col8 - margin, HTBUtils.FormatCurrency(rec.AppliedToInvoice), 'R');
            _writer.print(_lin + 5, _col9 - margin, HTBUtils.FormatCurrency(rec.AppliedToInterest), 'R');


            _writer.drawLine(_lin, _col2, _lin +gap, _col2);
            _writer.drawLine(_lin, _col3, _lin +gap, _col3);
            _writer.drawLine(_lin, _col4, _lin +gap, _col4);
            _writer.drawLine(_lin, _col5, _lin +gap, _col5);
            _writer.drawLine(_lin, _col6, _lin +gap, _col6);
            _writer.drawLine(_lin, _col7, _lin +gap, _col7);
            _writer.drawLine(_lin, _col8, _lin +gap, _col8);
            _writer.drawLine(_lin, _col9, _lin +gap, _col9);
            _lin +=gap;
        }

        private void PrintHeaderLine()
        {
            const int margin = 10;
            int gap = _gap + margin;

            _writer.drawRectangle(_lin, _col1, _lin + gap, _col9, BaseColor.WHITE);

            _writer.print(_lin + 5, _col1 + ((_col2 - _col1) / 2), "Aktenzeichen", 'C');
            _writer.print(_lin + 5, _col2 + ((_col3 - _col2) / 2), "Name Schuldner", 'C');
            _writer.print(_lin + 5, _col3 + ((_col4 - _col3) / 2), "Rg. Nummer", 'C');
            _writer.print(_lin + 5, _col4 + ((_col5 - _col4) / 2), "Kundennummer", 'C');
            _writer.print(_lin + 5, _col5 + ((_col6 - _col5) / 2), "Datum", 'C');
            _writer.print(_lin + 5, _col7 - ((_col7 - _col6) / 2), "Betrag (Ausgang)", 'C');
            _writer.print(_lin + 5, _col8 - ((_col8 - _col7) / 2), "Buchung Kapital", 'C');
            _writer.print(_lin + 5, _col9 - ((_col9 - _col8) / 2), "Buchung Zinsen", 'C');


            _writer.drawLine(_lin, _col2, _lin + gap, _col2);
            _writer.drawLine(_lin, _col3, _lin + gap, _col3);
            _writer.drawLine(_lin, _col4, _lin + gap, _col4);
            _writer.drawLine(_lin, _col5, _lin + gap, _col5);
            _writer.drawLine(_lin, _col6, _lin + gap, _col6);
            _writer.drawLine(_lin, _col7, _lin + gap, _col7);
            _writer.drawLine(_lin, _col8, _lin + gap, _col8);
            _writer.drawLine(_lin, _col9, _lin + gap, _col9);
            _lin += gap;
        }

        private void SetHeadingFont()
        {
            _writer.setFont("Calibri", 12, true, false, true);
        }
        public void SetLineFont()
        {
            _writer.setFont("Calibri", 10);
        }

        public void PrintFooter()
        {
            SetLineFont();
            _writer.print((_lin += _gap), _col1, "Mit freundlichem Gruß,");
            _lin += _gap;
            _writer.print((_lin += _gap), _col1, "E.C.P. European Car Protect KG");
        }

        public void NewPage()
        {
            _writer.newPage();
        }
        
        private string GetAppliedToInClause(ArrayList list)
        {
            var sb = new StringBuilder("(  ");
            if(list.Count > 0)
            {
                foreach (tblCustInkAktInvoice inv in list)
                {
                    if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL || inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST)
                    {
                        sb.Append(inv.InvoiceID);
                        sb.Append(", ");
                    }
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append(")");
            }
            return sb.ToString();
        }

        #region Report Interface
        private int NewPageOnOverflow()
        {
            if (CheckOverflow(_lin))
            {
                NewPage();
                return PrintPageHeader();
            }
            return _lin;
        }
        public bool CheckOverflow(int plin)
        {
            return plin >= MaxLines;
        }
        public ECPPdfWriter GetWriter()
        {
            return _writer;
        }
        #endregion

    }
}
