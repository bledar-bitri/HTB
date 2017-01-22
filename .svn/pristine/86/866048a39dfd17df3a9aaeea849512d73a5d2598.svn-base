using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using HTB.Database.LookupRecords;
using System.Collections;
using HTB.Database;
using System.Data;
using HTBUtilities;
using System.Text;
using HTB.v2.intranetx.util;
using System.IO;
using System.Reflection;

namespace HTB.v2.intranetx.customer
{
    public partial class AktenInt : Page
    {
        private static int _aktStatus = -1;
        private static int _aktCount;
        private static int _agId = -1;
        private static int _userId = -1;
        
        private static string _gegnerName = string.Empty;
        private static string _aktAZ = string.Empty;
        private static int _startIndex;
        private static int _endIndex;
        private static DateTime _startDate = HTBUtils.DefaultDate;
        private static DateTime _endDate = HTBUtils.DefaultDate;
        private tblAuftraggeber _ag;
        private static tblUser _user;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
//            Session["MM_AG"] = "6";
//            Session["MM_UserID"] = "447";
            ctlMessage.Clear();
            _userId = GlobalUtilArea.GetUserId(Session); // force session validation
            _agId = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_AG"]);
            if (_agId > 0 && _userId > 0)
            {
                _ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + _agId, typeof(tblAuftraggeber));
                _user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + _userId, typeof(tblUser));
                if (_ag != null && _user != null)
                {
                    lblKlientInfo.Text = _ag.AuftraggeberName1 + "&nbsp;" + _ag.AuftraggeberName2;
                    if(!_user.UserIsSbAdmin)
                        lblKlientInfo.Text += " ["+_user.UserVorname+" "+_user.UserNachname+"]";
                }
            }
            if (!IsPostBack)
            {
                txtDateStart.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                txtDateEnd.Text = DateTime.Now.ToShortDateString();
                SetAktsPageSize();
                btnSubmit_Click(null, null);
            }
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //ctlMessage.Clear();
            _gegnerName = txtGegnerName.Text;
            _aktAZ = txtAZ.Text;
            _aktStatus = Convert.ToInt32(ddlAktStatus.SelectedValue);
            _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart);
            _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd);
            _agId = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_AG"]);
            gvAkts.DataBind();
            txtGegnerName.Focus();
            //ctlMessage.AppendInfoLine(msg);
        }
        public void btnShowDirectPay_Click(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            HideAllPanels();
            int selectedRowIndex = GetSelectedRowIndex(((ImageButton)sender).CommandArgument);
            if (selectedRowIndex >= 0)
                ShowPanel(selectedRowIndex, "pnlDirectPay", "txtBuchungsText");
        }
        public void btnHideDirectPay_Click(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            HideAllPanels();
        }
        public void btnSendDirectPay_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                int selectedRowIndex = GetSelectedRowIndex(((Button)sender).CommandArgument);
                if (selectedRowIndex >= 0)
                {
                    var paymentDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(GetEnteredValue(selectedRowIndex, "txtPaymentDate"));
                    var paymentText = GetEnteredValue(selectedRowIndex, "txtPaymentText");
                    var paymentAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(GetEnteredValue(selectedRowIndex, "txtAmount"));
                    var paymentMemo = GetEnteredValue(selectedRowIndex, "txtMemo");
                    var ctlMsg = (global_files.CtlMessage)GetUserControl(selectedRowIndex, "cltPaymentMessage"); 
                    if(ctlMsg != null) {
                        ctlMsg.Visible = false;
                        if (ValidatePaymentEntries(paymentDate, paymentAmount, ctlMsg))
                        {
                            int aktID = GetSelectedAktNumber(selectedRowIndex);

                            if (aktID > 0)
                            {
                                string body = GetKlientDirectPaymentText().Replace("[name]", "Frau Auer").Replace("[aktID]", aktID.ToString()).Replace("[AktType]", "Interventionsakt");

                                var sb = new StringBuilder();

                                sb.Append("<tr><td valign=\"top\" class=\"EditCaption\">Auftrag: </td><td class=\"EditData\">");
                                sb.Append(aktID);
                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
                                sb.Append("Datum Zahlungseingang: </td><td valign=\"top\" class=\"EditData\">");
                                sb.Append(paymentDate.ToShortDateString());
                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
                                sb.Append("Buchungstext: </td><td valign=\"top\" class=\"EditData\">");
                                sb.Append(paymentText);
                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
                                sb.Append("Bemerkungen des Kunden: </td><td valign=\"top\" class=\"EditData\">");
                                sb.Append(paymentMemo.Replace(Environment.NewLine, "<br/>"));
                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
                                sb.Append("Betrag: </td><td valign=\"top\" class=\"EditData\">");
                                sb.Append(HTBUtils.FormatCurrency(paymentAmount));
                                sb.Append("</td></tr>");
                                body = body.Replace("[Payment_Info]", sb.ToString());
                                var email = new HTBEmail {ReplyTo = "<kundenportal@ecp.or.at>"};
                                email.SendGenericEmail(HTBUtils.GetConfigValue("From_Customer_Email"), "Direktzahlung: Interventionsakt " + aktID, body);

                                ShowDirectPaymentInfoMsg(selectedRowIndex, "Direktzahlung gesendet!");
                                
                                HideAllPanels();
                            }
                            else
                            {
                                ctlMsg.ShowError("Fehler [AktId]: Bitte versuchen Sie noch einmal");
                                ctlMsg.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        ctlMessage.ShowError("Fehler [MsgCtl]: Bitte versuchen Sie noch einmal");
                    }
                }
                else
                {
                    ctlMessage.ShowError("Fehler [RowIndex]: Bitte versuchen Sie noch einmal");
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        public void btnShowEMail_Click(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            HideAllPanels();
            int selectedRowIndex = GetSelectedRowIndex(((ImageButton)sender).CommandArgument);
            if (selectedRowIndex >= 0)
                ShowPanel(selectedRowIndex, "pnlEMail", "txtFromName");
        }
        public void btnSendEMail_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                int selectedRowIndex = GetSelectedRowIndex(((Button)sender).CommandArgument);
                if (selectedRowIndex >= 0)
                {
                    string name = GetEnteredValue(selectedRowIndex, "txtFromName");
                    string from = GetEnteredValue(selectedRowIndex, "txtEMailFrom");
                    string subject = GetEnteredValue(selectedRowIndex, "txtSubject");
                    string emailBody = GetEnteredValue(selectedRowIndex, "txtEmailBody").Replace(Environment.NewLine, "<br/>");
                    var ctlMsg = (global_files.CtlMessage)GetUserControl(selectedRowIndex, "cltEMailMessage");
                    if (ctlMsg != null)
                    {
                        if (ValidateEMailEntries(name, from, emailBody, ctlMsg))
                        {
                            int aktID = GetSelectedAktNumber(selectedRowIndex);
                            if (aktID > 0)
                            {
                                string body = GetKlientEMailWrapperText().Replace("[name]", "Frau Auer").Replace("[aktID]", aktID.ToString()).Replace("[AktType]", "Interventionsakt");

                                var sb = new StringBuilder();
                                sb.Append("<tr><td valign=\"top\" class=\"EditCaption\">Vom: </td><td class=\"EditData\">");
                                sb.Append(name);
                                sb.Append("<tr><td valign=\"top\" class=\"tblDataAll\" colspan=\"2\">&nbsp;</td>");
                                sb.Append("<tr><td valign=\"top\" class=\"EditCaption\">Email: </td><td class=\"EditData\">");
                                sb.Append(emailBody);
                                sb.Append("</td></tr>");
                                body = body.Replace("[EMail_Info]", sb.ToString());

                                var email = new HTBEmail();
                                email.SendGenericEmail(from, new[] { HTBUtils.GetConfigValue("From_Customer_Email") }, subject, body, true, 0, aktID);

                                ShowEmailInfoMsg(selectedRowIndex, "Email gesendet!");

                                HideAllPanels();
                            }
                            else
                            {
                                ctlMsg.ShowError("Fehler [AktId]: Bitte versuchen Sie noch einmal");
                                ctlMsg.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        ctlMessage.ShowError("Fehler [MsgCtl]: Bitte versuchen Sie noch einmal");
                    }
                }
                else
                {
                    ctlMessage.ShowError("Fehler [RowIndex]: Bitte versuchen Sie noch einmal");
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        public void btnHideEMail_Click(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            HideAllPanels();
        }

        public void gvAkts_DataBound(object sender, EventArgs e)
        {
            lblTotalAktsMsg.Text = "Akt " + _startIndex + " bis " + _endIndex + " von " + _aktCount;
        }
        #endregion

        #region Akt lookup
        public List<CustomerAktLookup> GetAkts(int startIndex, int pageSize, string sortBy, ref int totalAkts)
        {
            try
            {
                if (_startDate.ToShortDateString() == HTBUtils.DefaultShortDate || _endDate.ToShortDateString() == HTBUtils.DefaultShortDate)
                {
                    Log.Info("[MSG: StartDate = " + _startDate.ToShortDateString() + " EndDate = " + _endDate.ToShortDateString() + "]");
                    return new ArrayList().Cast<CustomerAktLookup>().ToList();
                }
                Log.Info("[AG " + _agId + "] [StartIndex " + startIndex + "] [PageSize " + pageSize + "] [SortBy " + sortBy + "] [TotalAkten " + totalAkts + "]<br/>");
                var parameters = new ArrayList
                                     {
                                         new StoredProcedureParameter("@agID", SqlDbType.Int, _agId),
                                         new StoredProcedureParameter("@startIndex", SqlDbType.Int, startIndex),
                                         new StoredProcedureParameter("@pageSize", SqlDbType.Int, pageSize),
                                         new StoredProcedureParameter("@sortBy", SqlDbType.Int, sortBy),
                                         new StoredProcedureParameter("@totalAkten", SqlDbType.Int, totalAkts, ParameterDirection.Output)
                                     };

                if (!string.IsNullOrEmpty(_gegnerName))
                {
                    parameters.Add(new StoredProcedureParameter("@gegnerName", SqlDbType.NVarChar, _gegnerName));
                    Log.Info("[GegnerName = " + _gegnerName + "]");
                }
                if (!string.IsNullOrEmpty(_aktAZ))
                {
                    parameters.Add(new StoredProcedureParameter("@aktAZ", SqlDbType.NVarChar, _aktAZ.Trim()));
                    Log.Info("[AktAZ = " + _aktAZ.Trim() + "]");
                }
                if (_aktStatus >= 0)
                {
                    parameters.Add(new StoredProcedureParameter("@aktStatus", SqlDbType.Int, _aktStatus));
                    Log.Info("[AktStatus = " + _aktStatus + "]");
                }
                if (_startDate != HTBUtils.DefaultDate)
                {
                    parameters.Add(new StoredProcedureParameter("@startDate", SqlDbType.DateTime, _startDate));
                    Log.Info("[StartDate = " + _startDate.ToShortDateString() + "]");
                }
                if (_endDate != HTBUtils.DefaultDate)
                {
                    parameters.Add(new StoredProcedureParameter("@endDate", SqlDbType.DateTime, _endDate));
                    Log.Info("[EndDate = " + _endDate.ToShortDateString() + "]");
                }
                if (_user != null)
                {
                    if (!_user.UserIsSbAdmin)
                        parameters.Add(new StoredProcedureParameter("agSbEmail", SqlDbType.VarChar, _user.UserEMailOffice));
                }
                else
                {
                    parameters.Add(new StoredProcedureParameter("agSbEmail", SqlDbType.VarChar, "NOT_VALID_USER")); // the database should not return anything
                }

                Log.Info("[Calling spAGAkten]<br/>");
                ArrayList list = HTBUtils.GetStoredProcedureRecords("spAGAkten", parameters, typeof(CustomerAktLookup));
                Log.Info("[ListSize " + list.Count + "]<br/>");
                int rowId = 0;
                foreach (CustomerAktLookup akt in list)
                {
                    akt.RowId = rowId++;
                    akt.AktAge = DateTime.Now.Subtract(akt.AktEnteredDate).Days;
                    akt.AktStatusCaption = GetAktStatus(akt.AktStausID);

                    akt.AktIdLink = "<a href=\"javascript:MM_openBrWindow('../../intranet/custint/popzahlung.asp?" + GlobalHtmlParams.ID + "=" + akt.AktId + "','popZahlung','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=600')\">" + akt.AktAZ + "</a>";
                    akt.GegnerInfo = akt.GegnerName + "<br/> <br/>" + akt.GegnerAddress + "<br/>" + akt.GegnerCountry + " - " + akt.GegnerZip + " " + akt.GegnerCity;
                }

                foreach (object o in parameters)
                {
                    if (o is ArrayList)
                    {
                        var outputList = (ArrayList)o;
                        foreach (StoredProcedureParameter p in outputList)
                        {
                            if (p.Name.IndexOf("totalAkten") >= 0)
                            {
                                totalAkts = Convert.ToInt32(p.Value);
                                break;
                            }
                        }
                    }
                }
                _startIndex = startIndex + 1;
                _endIndex = startIndex + list.Count;
                return list.Cast<CustomerAktLookup>().ToList();
            }
            catch (Exception ex) { 
                Log.Error(ex.ToString());
                return new ArrayList().Cast<CustomerAktLookup>().ToList(); 
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CustomerAktLookup> GetAllAkts(int startIndex, int pageSize, string sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
                sortBy = "GegnerName";
            List<CustomerAktLookup> result = GetAkts(startIndex, pageSize, sortBy, ref _aktCount);
            return result;
        }
        public int GetTotalAktsCount()
        {
            return _aktCount;
        }
        private string GetAktStatus(int status)
        {
            switch (status)
            {
                case 0:
                    return "0 - Neu Erfasst";
                case 1:
                    return "1 - In Bearbeitung";
                case 2:
                    return "2 - Abgegeben";
                case 3:
                    return "3 - Fertig";
                case 4:
                    return "4 - Abgeschlossen";
            }
            return "";
        }

        public void SetAktsPageSize()
        {
            int ret = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_DataRows"]);
            if (ret > 0 && ret < 50 ) {
                gvAkts.PageSize = ret;
            }
            else {
                gvAkts.PageSize = 50;
            }
        }
        #endregion

        #region Grid
        private int GetSelectedRowIndex(string commandArg)
        {
            for (int i = 0; i < gvAkts.Rows.Count; i++)
            {
                GridViewRow row = gvAkts.Rows[i];
                var btnShowDirectPay = (ImageButton)row.FindControl("btnShowDirectPay");
                if (btnShowDirectPay != null && btnShowDirectPay.CommandArgument == commandArg)
                    return i;
            }
            return -1;
        }
        private int GetSelectedAktNumber(int rowIndex)
        {
            GridViewRow row = gvAkts.Rows[rowIndex];
            var lblAktId = (Label)row.FindControl("lblAktId");
            if (lblAktId != null)
            {
                return GlobalUtilArea.GetZeroIfConvertToIntError(lblAktId.Text);
            }
            return 0;
        }
        private string GetEnteredValue(int rowIndex, string lblFieldName)
        {
            GridViewRow row = gvAkts.Rows[rowIndex];
            var txtbox = (TextBox)row.FindControl(lblFieldName);
            if (txtbox != null)
            {
                return txtbox.Text;
            }
            return null;
        }
        private void HideAllPanels()
        {
            HideAllPanels("pnlDirectPay");
            HideAllPanels("pnlEMail");
        }
        private void HideAllPanels(string panelName)
        {
            for (int i = 0; i < gvAkts.Rows.Count; i++)
            {
                GridViewRow row = gvAkts.Rows[i];
                var pnlDirectPay = (UpdatePanel)row.FindControl(panelName);
                if (pnlDirectPay != null)
                {
                    pnlDirectPay.Visible = false;
                }
            }
        }
        private void ShowPanel(int rowIndex, string panelName, string focusTextBox)
        {
            GridViewRow row = gvAkts.Rows[rowIndex];
            var pan = (UpdatePanel)row.FindControl(panelName);
            if (pan != null)
            {
                pan.Visible = true;
                var focusTb = (TextBox)row.FindControl(focusTextBox);
                if (focusTb != null)
                {
                    focusTb.Focus();
                }
            }
        }
        private UserControl GetUserControl(int rowIndex, string controlName)
        {
            return (UserControl)(gvAkts.Rows[rowIndex]).FindControl(controlName);
        }
        private void ShowDirectPaymentInfoMsg(int rowIndex, string msg)
        {
            ShowInfoMsg(rowIndex, msg, "lblDirectPaymentInfo");
        }
        private void ShowEmailInfoMsg(int rowIndex, string msg)
        {
            ShowInfoMsg(rowIndex, msg, "lblEmailInfo");
        }
        private void ShowInfoMsg(int rowIndex, string msg, string labelName)
        {
            GridViewRow row = gvAkts.Rows[rowIndex];
            var lbl = (Label)row.FindControl(labelName);
            if (lbl != null)
            {
                lbl.Text = msg;
            }
        }
        #endregion

        #region utility methods

        private string GetKlientDirectPaymentText()
        {
            var sb = new StringBuilder();
            StreamReader re = File.OpenText(HTBUtils.GetConfigValue("Klient_DirectPayment_Notification_Text"));
            string input;
            while ((input = re.ReadLine()) != null)
            {
                sb.Append(input);
            }
            re.Close();
            re.Dispose();
            return sb.ToString();
        }
        private string GetKlientEMailWrapperText()
        {
            var sb = new StringBuilder();
            StreamReader re = File.OpenText(HTBUtils.GetConfigValue("Klient_EMail_Wrapper_Text"));
            string input;
            while ((input = re.ReadLine()) != null)
            {
                sb.Append(input);
            }
            re.Close();
            re.Dispose();
            return sb.ToString();
        }

        private static bool ValidateEMailEntries(string name, string from, string body, global_files.CtlMessage ctlMsg)
        {
            var sb = new StringBuilder();
            bool ok = true;
            if (name.Trim() == string.Empty)
            {
                sb.Append("Bitte Ihre Namen eingeben!<br/>");
                ok = false;
            }
            if (from.Trim() == string.Empty)
            {
                sb.Append("Bitte Ihre Emailadresse eingeben!<br/>");
                ok = false;
            }
            if (body.Trim() == string.Empty)
            {
                sb.Append("Bitte Text eingeben!<br/>");
                ok = false;
            }
            if (!HTBUtils.IsValidEmail(from))
            {
                sb.Append("Ihre Emailadresse ist falsch!<br/>");
                ok = false;
            }
            if (!ok)
            {
                ctlMsg.ShowError(sb.ToString());
                ctlMsg.Visible = true;
            }

            return ok;
        }
        private static bool ValidatePaymentEntries(DateTime date, double payAmount, global_files.CtlMessage ctlMsg)
        {
            var sb = new StringBuilder();
            bool ok = true;
            if (date.ToShortDateString() == HTBUtils.DefaultShortDate)
            {
                sb.Append("Datum ist ungültig!<br/>");
                ok = false;
            }
            if (payAmount <= 0)
            {
                sb.Append("Summe ist ungültig!<br/>");
                ok = false;
            }
            if (!ok)
            {
                ctlMsg.ShowError(sb.ToString());
                ctlMsg.Visible = true;
            }

            return ok;
        }
        
        #endregion
    }
}