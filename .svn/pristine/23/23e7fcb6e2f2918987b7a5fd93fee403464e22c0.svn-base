using System;
using System.Web.UI;
using HTB.Database;
using HTBUtilities;
using System.Text;
using HTB.v2.intranetx.util;
using HTB.Database.Views;

namespace HTB.v2.intranetx.aktenink
{
    public partial class PostponeRate : Page
    {
        private int _rateId;
        private tblCustInkAktRate _rate;
        private qryCustInkAkt _akt;

        protected void Page_Load(object sender, EventArgs e)
        {
            _rateId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.ID]);
            btnSubmit.Visible = true;
            if (_rateId > 0)
            {
                LoadRecords();
                if (!IsPostBack)
                    SetValues();
            }
            else
            {
                ctlMessage.ShowError("Falsche Ratenid :-(");
                btnSubmit.Visible = false;
            }
        }

        private void LoadRecords()
        {
            try
            {
                _rate = (tblCustInkAktRate)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktRate WHERE CustInkAktRateID = " + _rateId, typeof(tblCustInkAktRate));
                if (_rate != null)
                {
                    _akt = (qryCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + _rate.CustInkAktRateAktID, typeof(qryCustInkAkt));
                }
                else
                {
                    ctlMessage.ShowError("Rate nicht gefunden :-(");
                    btnSubmit.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        private void SetValues()
        {
            lblAktID.Text = _akt.CustInkAktID.ToString() + (!string.IsNullOrEmpty(_akt.CustInkAktOldID) ? "[" + _akt.CustInkAktOldID + "]" : "");

            lblCustInkAktEnterDate.Text = _akt.CustInkAktEnterDate.ToShortDateString();
            lblAuftraggeberName1.Text = _akt.AuftraggeberName1;
            lblAuftraggeberName2.Text = _akt.AuftraggeberName2;
            lblAuftraggeberStrasse.Text = _akt.AuftraggeberStrasse;
            lblAuftraggeberLKZ.Text = _akt.AuftraggeberLKZ;
            lblAuftraggeberPLZ.Text = _akt.AuftraggeberPLZ;
            lblAuftraggeberOrt.Text = _akt.AuftraggeberOrt;
            lblKlientName1.Text = _akt.KlientName1;
            lblKlientName2.Text = _akt.KlientName2;
            lblKlientStrasse.Text = _akt.KlientStrasse;
            lblKlientLKZ.Text = _akt.KlientLKZ;
            lblKlientPLZ.Text = _akt.KlientPLZ;
            lblKlientOrt.Text = _akt.KlientOrt;
            
            lblGegner.Text += _akt.GegnerLastName1 + ", " + _akt.GegnerLastName2 + "<br/>";
            lblGegner.Text += _akt.GegnerLastStrasse + "<br/>";
            lblGegner.Text += _akt.GegnerLastZipPrefix + "&nbsp;" + _akt.GegnerLastZip + "&nbsp;" + _akt.GegnerLastOrt;

            lblCustInkAktGothiaNr.Text = _akt.CustInkAktGothiaNr;
            lblCustInkAktKunde.Text = _akt.CustInkAktKunde;
            lblCustInkAktInvoiceDate.Text = _akt.CustInkAktInvoiceDate.ToShortDateString();
            lblCustInkAktNextWFLStep.Text = _akt.CustInkAktNextWFLStep.ToShortDateString();

            txtCustInkAktRatePostponeReason.Text = _rate.CustInkAktRatePostponeReason;
            txtCustInkAktRatePostponeTillDate.Text = GetPostponedTillDate().ToShortDateString();
            chkCustInkAktRatePostponeWithNoOverdue.Checked = _rate.CustInkAktRatePostponeWithNoOverdue;
        }

        private DateTime GetPostponedTillDate()
        {
            var lastDueDate = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT MAX (CustInkAktRateDueDate) AS [DateValue] FROM tblCustInkAktRate WHERE CustInkAktRateAktID = " + _rate.CustInkAktRateAktID, typeof(SingleValue));
            var lastPostponedDate = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT MAX (CustInkAktRatePostponeTillDate) AS [DateValue] FROM tblCustInkAktRate WHERE CustInkAktRateAktID = " + _rate.CustInkAktRateAktID, typeof(SingleValue));
            
            if (lastPostponedDate.DateValue > lastDueDate.DateValue)
                return lastPostponedDate.DateValue.AddMonths(1);
            
            return lastDueDate.DateValue.AddMonths(1);
        }
        #region Even Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if(ValidateEntry())
            {
                _rate.CustInkAktRatePostponeTillDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtCustInkAktRatePostponeTillDate);
                _rate.CustInkAktRatePostponeReason = GlobalUtilArea.GetEmptyIfNull(txtCustInkAktRatePostponeReason.Text);
                _rate.CustInkAktRatePostponeWithNoOverdue = chkCustInkAktRatePostponeWithNoOverdue.Checked;
                _rate.CustInkAktRatePostponedUser = GlobalUtilArea.GetUserId(Session);
                try
                {
                    RecordSet.Update(_rate);
                    CloseWindowAndRefresh();
                }
                catch(Exception ex)
                {
                    ctlMessage.ShowException(ex);
                }

            }
        }

        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWindow();
        }
        #endregion

        private bool ValidateEntry()
        {
            var sb = new StringBuilder();
            bool ok = true;
            DateTime postponedDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtCustInkAktRatePostponeTillDate);
            if (postponedDate == HTBUtils.DefaultDate)
            {
                ok = false;
                sb.Append("<strong>Verschiben bis</strong> falsch!<br/>");
            }
            if(postponedDate < DateTime.Now)
            {
                ok = false;
                sb.Append("<strong>Verschiben bis</strong> muss gr&ouml;&szlig;er alst Heute sein!<br/>");
            }
            if (txtCustInkAktRatePostponeReason.Text.Trim().Length < 5)
            {
                ok = false;
                sb.Append("Sie m&uuml;ssen eine <strong>Verschiebungsgrund</strong> eingeben!<br/>");
            }
            if(!ok)
            {
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }
        

        private void CloseWindowAndRefresh()
        {
            ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "closeScript", "MM_refreshParentAndClose();", true);
        }

        private void CloseWindow()
        {
            ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "closeScript", "window.close();", true);
        }
    }
}