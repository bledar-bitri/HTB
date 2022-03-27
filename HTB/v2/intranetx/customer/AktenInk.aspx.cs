using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using HTB.Database.LookupRecords;
using System.Collections;
using HTB.Database;
using System.Data;
using HTBInvoiceManager;
using HTBUtilities;
using System.Text;
using HTB.v2.intranetx.util;
using System.IO;
using HTBServices;
using HTBServices.Mail;

namespace HTB.v2.intranetx.customer
{
    public partial class AktenInk : Page
    {
        private static int _aktStatus = -1;
        private static int _aktCount;
        private static int _klientId = -1;
        private static int _userId = -1;
        private static string _az = string.Empty;
        private static string _invoiceNumber = string.Empty;
        private static string _gegnerName = string.Empty;
        private static int _startIndex;
        private static int _endIndex;
        private tblKlient _klient;
        private static tblUser _user;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
//            Session["MM_UserID"] = "545";
//            Session["MM_Klient"] = "10676";
            _userId = GlobalUtilArea.GetUserId(Session); // force session validation
            _klientId = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_Klient"]);
            if (_klientId > 0 && _userId > 0)
            {
                _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientID = " + _klientId, typeof(tblKlient));
                _user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + _userId, typeof(tblUser));

                if (_klient != null && _user != null)
                {
                    lblKlientInfo.Text = "<em>- Klient:&nbsp;" + _klient.KlientName1 + "&nbsp;" + _klient.KlientName2 + "</em>";
                    if (!_user.UserIsSbAdmin)
                        lblKlientInfo.Text += " [" + _user.UserVorname + " " + _user.UserNachname + "]";
                }
            }
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlAktStatus,
                        "SELECT * FROM tblCustInkAktStatus ORDER BY CustInkAktStatusCode",
                        typeof(tblCustInkAktStatus),
                        "CustInkAktStatusCode",
                        "CustInkAktStatusCaption",
                        true);
                SetAktsPageSize();
//                if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR]).Equals(string.Empty))
//                {
//                    txtGegnerName.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SEARCH_FOR]);
                    btnSubmit_Click(null, null);
//                }
//                else
//                {
                    //LoadSearchCriteriaFromSession();
                    //gvAkts.DataBind();
//                }
            }
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            LoadSearchCriteriaFromScreen();
            SaveSearchCriteriaInSession();
            gvAkts.DataBind();
            txtGegnerName.Focus();
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

        protected void btnSendDirectPay_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                int selectedRowIndex = GetSelectedRowIndex(((Button)sender).CommandArgument);
                if (selectedRowIndex >= 0)
                {
                    DateTime paymentDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(GetEnteredValue(selectedRowIndex, "txtPaymentDate"));
                    //string paymentText = GetEnteredValue(selectedRowIndex, "txtPaymentText");
                    string paymentText = null;
                    double paymentAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(GetEnteredValue(selectedRowIndex, "txtAmount"));
                    string paymentMemo = GetEnteredValue(selectedRowIndex, "txtMemo");
                    var ctlMsg = (global_files.CtlMessage)GetUserControl(selectedRowIndex, "cltPaymentMessage"); 
                    if(ctlMsg != null) {
                        ctlMsg.Visible = false;
                        if (ValidatePaymentEntries(paymentDate, paymentAmount, ctlMsg))
                        {
                            int aktID = GetSelectedAktNumber(selectedRowIndex);

                            if (aktID > 0)
                            {
                                var paymentErrMsg = new StringBuilder();
                                CrateDirectPayment(aktID, paymentAmount, paymentDate, paymentText, paymentMemo.Replace(Environment.NewLine, "<br/>"), paymentErrMsg);

                                string body = GetKlientDirectPaymentText().Replace("[name]", "Damen und Herren").Replace("[aktID]", aktID.ToString()).Replace("[AktType]", "Inkassoakt");

                                var sb = new StringBuilder();

                                sb.Append("<tr><td valign=\"top\" class=\"EditCaption\">Auftrag: </td><td class=\"EditData\">");
                                sb.Append(aktID);
                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
                                sb.Append("Datum Zahlungseingang: </td><td valign=\"top\" class=\"EditData\">");
                                sb.Append(paymentDate.ToShortDateString());
                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
//                                sb.Append("Buchungstext: </td><td valign=\"top\" class=\"EditData\">");
//                                sb.Append(paymentText);
//                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
                                sb.Append("Bemerkungen des Kunden: </td><td valign=\"top\" class=\"EditData\">");
                                sb.Append(paymentMemo.Replace(Environment.NewLine, "<br/>"));
                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
                                sb.Append("Betrag: </td><td valign=\"top\" class=\"EditData\">");
                                sb.Append(HTBUtils.FormatCurrency(paymentAmount));
                                sb.Append("</td></tr><tr><td valign=\"top\" class=\"EditCaption\">");
                                sb.Append("Zahlung wurde Gebucht: </td><td valign=\"top\" class=\"EditData\">");
                                    
                                if (paymentErrMsg.Length > 0)
                                {
                                    sb.Append("NEIN: ");
                                    sb.Append(HTBUtils.FormatCurrency(paymentAmount));
                                }
                                else
                                    sb.Append("JA: ");
                                sb.Append("</td></tr>");

                                body = body.Replace("[Payment_Info]", sb.ToString());
                                
                                var email = ServiceFactory.Instance.GetService<IHTBEmail>();
                                email.SendGenericEmail(HTBUtils.GetConfigValue("From_Customer_Email"), "Direktzahlung: Interventionsakt " + aktID, body, true, aktID);
                                
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

        private bool CrateDirectPayment(int aktId, double amount, DateTime payDate, string text, string memo, StringBuilder errMsg)
        {
            try
            {
                var comment = new StringBuilder();
                if(!string.IsNullOrEmpty(text))
                {
                    comment.Append("<br/><strong>Text:</strong> ");
                    comment.Append(text);
                }
                if (!string.IsNullOrEmpty(memo))
                {
                    comment.Append("<br/><strong>Bemerkungen des Kunden:</strong> ");
                    comment.Append(memo);
                }
                tblUser user = HTBUtils.GetUser(GlobalUtilArea.GetUserId(Session));
                var invMgr = new InvoiceManager();
                tblCustInkAktInvoice payment = invMgr.CreatePayment(aktId, tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_DIRECT_TO_CLIENT, amount);
                payment.InvoicePaymentReceivedDate = payDate;
                payment.InvoiceComment = "Am "+DateTime.Now.ToShortDateString()+" laut: " + user.UserVorname + " " + user.UserNachname +comment.ToString();
                int payId = invMgr.SaveInvoice(payment);
                if (payId >= 0)
                {
                    invMgr.ApplyPayment(payId, amount, 0);
                    return true;
                }
                errMsg.Append("Fehler bei Zahlungsbuchung: Could not create payment record.");
                return false;
                
            }
            catch(Exception e)
            {
                errMsg.Append("Fehler bei Zahlungsbuchung: "+e.Message);
            }
            return false;
        }

        protected void btnShowEMail_Click(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            HideAllPanels();
            int selectedRowIndex = GetSelectedRowIndex(((ImageButton)sender).CommandArgument);
            if (selectedRowIndex >= 0)
                ShowPanel(selectedRowIndex, "pnlEMail", "txtFromName");
        }

        protected void btnSendEMail_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                int selectedRowIndex = GetSelectedRowIndex(((Button)sender).CommandArgument);
                if (selectedRowIndex >= 0)
                {
//                    string name = GetEnteredValue(selectedRowIndex, "txtFromName");
//                    string from = GetEnteredValue(selectedRowIndex, "txtEMailFrom");
//                    string subject = GetEnteredValue(selectedRowIndex, "txtSubject");
                    string name = _user.UserVorname + " " + _user.UserNachname;
                    string from = _user.UserEMailOffice;
                    string subject = "Betreffend: "+GetSelectedAktNumber(selectedRowIndex);
                    string emailBody = GetEnteredValue(selectedRowIndex, "txtEmailBody").Replace(Environment.NewLine, "<br/>");
                    var ctlMsg = (global_files.CtlMessage)GetUserControl(selectedRowIndex, "cltEMailMessage");
                    if (ctlMsg != null)
                    {
                        if (ValidateEMailEntries(name, from, emailBody, ctlMsg))
                        {
                            int aktID = GetSelectedAktNumber(selectedRowIndex);
                            if (aktID > 0)
                            {
                                string body = GetKlientEMailWrapperText().Replace("[name]", "Damen und Herren").Replace("[aktID]", aktID.ToString()).Replace("[AktType]", "Inkassoakt");

                                var sb = new StringBuilder();
                                sb.Append("<tr><td valign=\"top\" class=\"EditCaption\">Vom: </td><td class=\"EditData\">");
                                sb.Append(name);
                                sb.Append("<tr><td valign=\"top\" class=\"tblDataAll\" colspan=\"2\">&nbsp;</td>");
                                sb.Append("<tr><td valign=\"top\" class=\"EditCaption\">Email: </td><td class=\"EditData\">");
                                sb.Append(emailBody);
                                sb.Append("</td></tr>");
                                body = body.Replace("[EMail_Info]", sb.ToString());

                                var email = ServiceFactory.Instance.GetService<IHTBEmail>();
                                email.SendGenericEmail(from, new[] { HTBUtils.GetConfigValue("From_Customer_Email") }, subject, body, true, aktID, 0);
                                
                                CreateAction(aktID, "<strong>Von Kudne:</strong> " + name + "<br/><strong>Betref:</strong> "+ subject, emailBody);

                                ShowEmailInfoMsg(selectedRowIndex, "Info gesendet!");
                                
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
        public void CreateAction(int aktId, string caption, string memo, int actionType = 72) // default email
        {
            var akt = HTBUtils.GetInkassoAkt(aktId);
            if (akt != null)
            {
                // Create action
                var action = new tblCustInkAktAktion
                                 {
                                     CustInkAktAktionAktID = akt.CustInkAktID,
                                     CustInkAktAktionDate = DateTime.Now,
                                     CustInkAktAktionMemo = memo,
                                     CustInkAktAktionCaption = caption,
                                     CustInkAktAktionUserId = GlobalUtilArea.GetUserId(Session),
                                     CustInkAktAktionTyp = actionType
                                 };
                action.CustInkAktAktionEditDate = action.CustInkAktAktionDate;
                RecordSet.Insert(action);
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
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("clientID", SqlDbType.Int, _klientId),
                                     new StoredProcedureParameter("startIndex", SqlDbType.Int, startIndex),
                                     new StoredProcedureParameter("pageSize", SqlDbType.Int, pageSize),
                                     new StoredProcedureParameter("sortBy", SqlDbType.Int, sortBy),
                                     new StoredProcedureParameter("totalAkten", SqlDbType.Int, totalAkts, ParameterDirection.Output)
                                 };

            if (_gegnerName != null && _gegnerName.Trim() != string.Empty)
                parameters.Add(new StoredProcedureParameter("gegnerName", SqlDbType.NVarChar, _gegnerName));
            if (_aktStatus >= 0)
                parameters.Add(new StoredProcedureParameter("aktStatus", SqlDbType.Int, _aktStatus));
            if(_az != null && _az.Trim() != string.Empty)
                parameters.Add(new StoredProcedureParameter("aktAZ", SqlDbType.VarChar, _az));
            if (_invoiceNumber != null && _invoiceNumber.Trim() != string.Empty)
                parameters.Add(new StoredProcedureParameter("aktKunde", SqlDbType.VarChar, _invoiceNumber));
            if (_user != null)
            {
                if (!_user.UserIsSbAdmin)
                    parameters.Add(new StoredProcedureParameter("clientSbId", SqlDbType.Int, _user.UserID));
            }
            else
                parameters.Add(new StoredProcedureParameter("clientSbId", SqlDbType.Int, -999)); // the database should not return anything
                
            ArrayList list = HTBUtils.GetStoredProcedureRecords("spClientAkten", parameters, typeof(CustomerAktLookup));

            int rowID = 0;
            foreach (CustomerAktLookup akt in list)
            {
                akt.RowId = rowID++;
                akt.AktIdLink = "<a href=\"ShowAktInk.aspx?" + GlobalHtmlParams.ID + "=" + akt.AktId + "\">" + akt.AktId + "</a>";
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
        public void SetAktsPageSize()
        {
            int ret = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_DataRows"]);
            if (ret > 0 && ret < 50)
            {
                gvAkts.PageSize = ret;
            }
            else
            {
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
            return lblAktId != null ? GlobalUtilArea.GetZeroIfConvertToIntError(lblAktId.Text) : 0;
        }
        private string GetEnteredValue(int rowIndex, string lblFieldName)
        {
            GridViewRow row = gvAkts.Rows[rowIndex];
            var txtbox = (TextBox)row.FindControl(lblFieldName);
            return txtbox != null ? txtbox.Text : null;
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
            if (pan == null) return;
            pan.Visible = true;
            var focusTb = (TextBox)row.FindControl(focusTextBox);
            if (focusTb != null)
            {
                focusTb.Focus();
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
            var re = File.OpenText(HTBUtils.GetConfigValue("Klient_DirectPayment_Notification_Text"));
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

        private bool ValidateEMailEntries(string name, string from, string body, global_files.CtlMessage ctlMsg)
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
        private bool ValidatePaymentEntries(DateTime date, double payAmount, global_files.CtlMessage ctlMsg)
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
        
        private void LoadSearchCriteriaFromScreen()
        {
            _gegnerName = txtGegnerName.Text;
            _aktStatus = Convert.ToInt32(ddlAktStatus.SelectedValue);
            _az = txtAZ.Text;
            _invoiceNumber = txtInvoiceNumber.Text;
        }
        private void SaveSearchCriteriaInSession()
        {
            Session["_gegnerName"] = _gegnerName;
            Session["_aktStatus"] = _aktStatus;
            Session["_az"] = _az;
            Session["_invoiceNumber"] = _invoiceNumber;
        }

        private void LoadSearchCriteriaFromSession()
        {
            _gegnerName = GlobalUtilArea.GetEmptyIfNull(Session["_gegnerName"]);
            _aktStatus = GlobalUtilArea.GetZeroIfConvertToIntError(Session["_aktStatus"]);
            _az = GlobalUtilArea.GetEmptyIfNull(Session["_az"]);
            _invoiceNumber = GlobalUtilArea.GetEmptyIfNull(Session["_invoiceNumber"]);

            txtGegnerName.Text = _gegnerName;
            txtAZ.Text = _az;
            txtInvoiceNumber.Text = _invoiceNumber;
            GlobalUtilArea.SetSelectedValue(ddlAktStatus, _aktStatus.ToString());
        }
        #endregion
    }
}