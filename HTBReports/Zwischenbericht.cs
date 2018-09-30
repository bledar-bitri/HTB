using System;
using System.Collections;
using System.Text;
using HTB.Database;
using HTBExtras.KingBill;
using HTBPdf;
using HTBUtilities;
using iTextSharp.text;
using System.IO;
using HTB.Database.Views;
using HTBExtras;

namespace HTBReports
{
    public class Zwischenbericht : IReport
    {
        private int _col;
        private int _col1;
        private int _col2;
        private int _col3;
        private int _col4;
        private int _col5;
        private int _col6;
        private int _col7;
        private const int MaxLines = 2750;
        private int _startLine;
        private int _lin;
        private int _gap;
        private int _normalGap;

        private readonly string _logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");
        private ECPPdfWriter _writer;
        private tblKlient _klient;
        private qryCustInkAkt _akt;
        private int _pageNumber = 1;
        public void GenerateZwischenbericht(qryCustInkAkt inkAkt, ArrayList actions, string info, Stream os, bool isOnlyOneAkt = true)
        {
            _akt = inkAkt;
            _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientId = " + _akt.KlientID, typeof(tblKlient));
            ArrayList invoices = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + _akt.CustInkAktID, typeof(tblCustInkAktInvoice));
            // let the caller handle exceptions
            //if (klient == null)
            //    return;

            Init(isOnlyOneAkt);
            
            if (isOnlyOneAkt)
                Open(os);

            PrintPageHeader();
            if (info != null && info.Trim() != string.Empty)
            {
                PrintBericht(info);
                _lin += _gap * 2;
                _lin = NewPageOnOverflow(_lin);
            }
            _lin = PrintInvoices(_lin, _col3, 1200, _gap, invoices);
            _lin += _gap * 2;
            _lin = NewPageOnOverflow(_lin);
            PrintActions(actions);
            _lin += _gap * 2;
            _lin = NewPageOnOverflow(_lin);

            if (isOnlyOneAkt)
            {
                PrintFooter();
                CloseReport();
            }
        }

        private void Init(bool isOnlyOne)
        {
            _startLine = 400;
            _lin = _startLine;
            _normalGap = 37;
            _gap = _normalGap;
            _col = 400;
            _col1 = _col + 33;

            _col2 = 150;         // this is the actual start columng (col 1)
            _col3 = _col2 + 320;  // Auftragsnummer
            _col4 = _col3 + 600;  // Gegnername
            _col5 = _col4 + 450;  // Betreff
            _col6 = _col5 + 200;  // Kapital
            _col7 = _col6 + 200;  // Uebergabe
            
        }

        public void Open(Stream os)
        {
            _writer = new ECPPdfWriter();
            _writer.setFormName("A4");
            _writer.open(os);
        }

        public void CloseReport()
        {
            _writer.Close();
            _writer.Document.Dispose();
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
                _writer.PrintLeftEcpInfo();
                    
                _lin += _gap + 20;
                var contact = (tblAnsprechpartner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAnsprechpartner WHERE AnsprechKlient = " + _akt.KlientID, typeof(tblAnsprechpartner));
                _writer.setFont("Calibri", 8);
                _writer.print((_lin += _gap), _col1, "E.C.P. European Car Protect KG");
                _writer.print((_lin += _gap), _col1, "Loigerstr. 89   A 5071 Wals");
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
            _writer.print((_lin += _gap), _col + 1600, "Salzburg, am " + DateTime.Now.ToShortDateString(), 'R');
            _lin += _gap * 5;
            if (_pageNumber == 1)
            {
                SetHeadingFont();
                _writer.print((_lin += _gap), _col1, "ZWISCHENBERICHT / ABSCHLUSSBERICHT");
                _lin += _gap * 2;
            }
            _writer.setFont("Calibri", 11, true, false, false);
            _writer.print((_lin += _gap), _col1, "AKTENZAHL:                           " + _akt.CustInkAktID + (_akt.CustInkAktAZ.Trim() != string.Empty ? " [" + _akt.CustInkAktAZ + "]" : ""));
            _writer.print((_lin += _gap), _col1, "SCHULDNER:                          " + _akt.GegnerLastName1+" "+_akt.GegnerLastName2);
            _writer.print((_lin += _gap), _col1, "RECHNUNGSMUMMER:      " + _akt.CustInkAktKunde);
            if(!string.IsNullOrEmpty(_akt.CustInkAktGothiaNr))
                _writer.print((_lin += _gap), _col1, "KUNDENNUMMER:              " + _akt.CustInkAktGothiaNr);

            if (_akt.CustInkAktStatus == 4 || _akt.CustInkAktStatus == 5)
                _writer.print((_lin += _gap), _col4,  "ABSCHLUSSBERICHT");
            else
                _writer.print((_lin += _gap), _col4, "ZWISCHENBERICHT");

            _lin += _gap * 3;
            SetLineFont();
            _writer.print(2800, 1900, "Seite " + _pageNumber);
            _pageNumber++;
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

        private void PrintActions(ArrayList list)
        {
            const int dateWidth = 180;
            SetHeadingFont();
            _writer.print((_lin += _gap), _col3, "BEARBEITUNGSVERLAUF");
            _lin += _gap*2;
            SetLineFont();
            for (int i = 0; i < list.Count; i++)
            {
                var action = (InkassoActionRecord) list[i];
                if (!action.ActionMemo.ToLower().StartsWith("interninfo:") && !action.ActionMemo.ToLower().StartsWith("status:"))
                {
                    var col = _col3;
                    if (!action.IsOnlyMemo)
                    {
                        _writer.print(_lin, col, action.ActionDate.ToShortDateString());
                        col += dateWidth;
                        if (action.ActionCaption != null && action.ActionCaption.Trim() != string.Empty)
                        {
                            _writer.print(_lin, col, action.ActionCaption);
                            _lin += _gap;
                        }
                        if (action.ActionMemo != null && action.ActionMemo.Trim() != string.Empty)
                        {
                            _lin = ReportUtils.PrintTextInMultipleLines(this, _lin, col, _gap, HTBUtils.ReplaceHtmlBreakWithNewLine(action.ActionMemo), 80);
                        }
                    }
                    else
                    {
                        _writer.setFont("Calibri", 10, true, false, true);
                        _writer.print(_lin, col, action.ActionCaption);
                        SetLineFont();

                        col += 80;
                        _lin += _gap;
                        _lin = ReportUtils.PrintTextInMultipleLines(this, _lin, col, _gap, HTBUtils.ReplaceHtmlBreakWithNewLine(action.ActionMemo), 80);
                    }
                    _lin += _gap;
                    if (i < list.Count - 1 && CheckOverflow(_lin))
                    {
                        NewPage();
                        PrintPageHeader();
                        SetLineFont();
                    }
                }
            }
        }

        private int PrintInvoices(int lin, int colStart, int width, int gap, ArrayList list)
        {
            double openedBalance = 0;
            int gap1 = gap + 20;
            SetHeadingFont();
            _writer.print(lin, colStart, "FORDERUNGSAUFSTELLUNG");
            lin += gap*2;

            #region Print First Invoices
            foreach (tblCustInkAktInvoice inv in list)
            {
                if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL || inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST)
                {
                    SetLineFont();
                    lin = PrintInvoiceLine(_writer, lin, colStart, width, gap1, inv.InvoiceDate, inv.InvoiceDescription, inv.InvoiceAmount);
                    openedBalance += inv.InvoiceBalance;
                    lin = NewPageOnOverflow(lin);
                }
            }
            #endregion

            #region Payments from Debtor
            //ArrayList appliedList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoiceApply WHERE ApplyToInvoiceId IN " + GetAppliedToInClause(list), typeof(tblCustInkAktInvoiceApply));
            ArrayList appliedList = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceCustInkAktId = " + _akt.CustInkAktID + " AND InvoicePaymentTransferToClientAmount > 0", typeof(tblCustInkAktInvoice));
            if (appliedList.Count > 0)
            {
                lin += gap * 3;
                lin = NewPageOnOverflow(lin);
                SetHeadingFont();
                _writer.print(lin, colStart, "BEREITS BEZAHLT");
                lin += gap * 2;
                DateTime dte = HTBUtils.DefaultDate;
                double amount = 0;
                foreach (tblCustInkAktInvoice inv in appliedList)
                {
                    if (dte != HTBUtils.DefaultDate && !dte.ToShortDateString().Equals(inv.InvoicePaymentTransferToClientDate.ToShortDateString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetLineFont();
                        lin = PrintInvoiceLine(_writer, lin, colStart, width, gap1, dte, "Bezahlt an Klient", amount);
                        lin = NewPageOnOverflow(lin);
                        amount = 0;
                    }
                    dte = inv.InvoicePaymentTransferToClientDate;
                    amount += inv.InvoicePaymentTransferToClientAmount;

//                      lin = PrintInvoiceLine(_writer, lin, colStart, width, gap1, inv.ApplyDate, "Bezahlt an Klient", inv.ApplyAmount);
                }
                if(amount > 0)
                {
                    SetLineFont();
                    lin = PrintInvoiceLine(_writer, lin, colStart, width, gap1, dte, "Bezahlt an Klient", amount);
                    lin = NewPageOnOverflow(lin);
                }
            }
            #endregion

            #region Transfers To Client [Commented Out]
            /*
            bool hasTransfers = false;
            #region Check if there are any  Transfers To Client
            foreach (tblCustInkAktInvoice inv in list)
            {
                if (inv.IsPayment() && inv.InvoicePaymentTransferToClientDate.ToShortDateString() != "01.01.1900")
                {
                    hasTransfers = true;
                    break;
                }
            }
            #endregion
            if (hasTransfers)
            {
                lin += gap * 3;
                lin = NewPageOnOverflow(lin);
                SetHeadingFont();
                writer.print(lin, colStart, "BEREITS ÜBERWIESEN");
                lin += gap * 2;

                #region Print Transfers To Client
                foreach (tblCustInkAktInvoice inv in list)
                {
                    if (inv.IsPayment() && inv.InvoicePaymentTransferToClientDate.ToShortDateString() != "01.01.1900")
                    {
                        SetLineFont();
                        lin = PrintInvoiceLine(writer, lin, colStart, width, gap1, inv.InvoicePaymentTransferToClientDate, "Überweisung an Klient", inv.InvoicePaymentTransferToClientAmount);
                        openedBalance -= inv.InvoicePaymentTransferToClientAmount;
                        lin = NewPageOnOverflow(lin);
                    }
                }
                #endregion
            }
             */
            #endregion

            lin += gap;
            //SetHeadingFont();
            //_writer.print(lin, colStart + width - 10, "Saldo:                             " + HTBUtils.FormatCurrency(openedBalance), 'R');
            SetLineFont();
            return lin;
        }

        private int PrintInvoiceLine(ECPPdfWriter writer, int lin, int colStart, int width, int gap, DateTime date, string text, double amount)
        {
            int wcol1 = colStart;
            int wcol2 = wcol1 + 200;
            int wcol3 = wcol1 + width - 300;
            int lineMargin = 10;

            writer.drawRectangle(lin, wcol1, lin + gap, wcol1 + width, BaseColor.WHITE);
            writer.print(lin + 5, wcol1 + lineMargin, date.ToShortDateString());
            writer.print(lin + 5, wcol2 + lineMargin, text);

            if (amount > 0)
                writer.print(lin + 5, wcol1 + width - lineMargin, HTBUtils.FormatCurrency(amount), 'R');

            writer.drawLine(lin, wcol2, lin + gap, wcol2);
            writer.drawLine(lin, wcol3, lin + gap, wcol3);
            lin += gap;
            return lin;
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
                    if (inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL ||
                        inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST ||
                        inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_CLIENT_ORIGINAL ||
                        (_klient.KlientReceivesInterest && inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_CLIENT))
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
        private int NewPageOnOverflow(int lin)
        {
            if (CheckOverflow(lin))
            {
                NewPage();
                return PrintPageHeader();
            }
            return lin;
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
