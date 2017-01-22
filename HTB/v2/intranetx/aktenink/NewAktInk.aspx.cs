using System;
using System.Collections;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database.LookupRecords;
using HTB.v2.intranetx.global_files;
using HTB.v2.intranetx.util;
using HTB.Database;
using HTBUtilities;
using System.Text;
using HTB.Database.Views;
using HTBInvoiceManager;
using System.Web;

namespace HTB.v2.intranetx.aktenink
{
    public partial class NewAktInk : Page, IWorkflow
    {
        private tblKlient _klient;
        private tblGegner _gegner;
        private qryUsers _user;
        private tblCustInkAkt _akt;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Registering the buttons control with RegisterPostBackControl early in the page lifecycle 
            // so that the FileUploadControl will upload files inside the UpdatePanel control
            ScriptManager1.RegisterPostBackControl(btnSubmit);
            ScriptManager1.RegisterPostBackControl(btnSubmit2);
            ScriptManager1.RegisterPostBackControl(cmdClientSBLoad);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (GlobalUtilArea.StringToBool(Request[GlobalHtmlParams.REFRESH_INKASSO_WORK_AKT]))
                Session[GlobalHtmlParams.INKASSO_AKT] = null;

            var componentPrefix = TabContainer1.ID + "_" + TabPanel1.ID + "_";
            ctlLookupKlient.SetParent(this);
            ctlLookupKlient.SetComponentName(componentPrefix + ctlLookupKlient.ID);
            ctlLookupKlient.SetNextFocusableComponentId(componentPrefix + ctlLookupGegner.ID + "_" + ctlLookupGegner.GetTextBox().ID);
            ctlLookupGegner.SetComponentName(componentPrefix + ctlLookupGegner.ID);
            ctlLookupGegner.SetNextFocusableComponentId(componentPrefix + txtClientInvoiceNumber.ID);
            ctlLookupUser.SetComponentName(componentPrefix + ctlLookupUser.ID);
            ctlLookupUser.SetNextFocusableComponentId(componentPrefix + txtMemo.ID);
            
            if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.CLIENT_ID]).Equals(""))
            {
                _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientID = " + Request[GlobalHtmlParams.CLIENT_ID], typeof(tblKlient));
                ctlLookupKlient.SetKlient(_klient);
                ctlLookupGegner.GetTextBox().Focus();
                
            }
            if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.GEGNER_ID]).Equals(""))
            {
                _gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + Request[GlobalHtmlParams.GEGNER_ID], typeof(tblGegner));
                ctlLookupGegner.SetGegner(_gegner);
                txtClientInvoiceNumber.Focus();
            }
            if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.USER_ID]).Equals(""))
            {
                _user = (qryUsers)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryUsers WHERE UserID = " + Request[GlobalHtmlParams.USER_ID], typeof(qryUsers));
                ctlLookupUser.SetUser(_user);
                txtMemo.Focus();
            }
            else
            {
                _user = (qryUsers)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryUsers WHERE UserID = " + GlobalUtilArea.GetUserId(Session), typeof(qryUsers));
                if (_user != null)
                {
                    ctlLookupUser.SetUser(_user);
                }
            }
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlAuftraggeber,
                    "SELECT * FROM tblAuftraggeber ORDER BY AuftraggeberName1 ASC",
                    typeof(tblAuftraggeber),
                    "AuftraggeberID",
                    "AuftraggeberName1", false);

                if (Session[GlobalHtmlParams.INKASSO_AKT] != null)
                {
                    _akt = (tblCustInkAkt)Session[GlobalHtmlParams.INKASSO_AKT];
                }
                if (_akt != null)
                {
                    LoadScreen();
                }
                else
                {
                    _akt = new tblCustInkAkt();
                }
                SetValuesFromParams();

            }
            else
            {
                if (_akt == null)
                    _akt = new tblCustInkAkt();
                LoadAkt();
                LoadLookupControlsFromAkt();
            }
            ctlWorkflow.SetWftInterface(this);
            ctlWorkflow.SetLastActionVisble(false);
            ctlWorkflow.SetNextActionVisible(false);
            
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ctlWorkflow.ClearMessage();
            ctlMessage.Clear();
            try
            {
                if (ValidateEntry())
                {
                    tblCustInkAkt akt = GetEnteredAkt(true);
                    CreateInvoices(akt);
                    if (ctlWorkflow.IsWorkflowEntered())
                    {
                        ctlWorkflow.SaveWorkFlow(akt.CustInkAktID, false);
                    }
                    // lastly update akt so that it can be picked up by the system
                    akt.CustInkAktIsPartial = false;
                    RecordSet.Update(akt);
                    var msg = "Akt Gespeichert! [" + akt.CustInkAktID + "]";
                    if(!UploadFiles(akt))
                    {
                        msg += "<br/> <strong>***Aber nicht alle Dokumente wurden upgeloaded***</strong>";
                    }

                    ClearScreen();
                    ctlMessage.ShowSuccess(msg);
                    ctlWorkflow.ShowSuccess(msg);
                    Session[GlobalHtmlParams.INKASSO_AKT] = null; // clear session
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
                ctlWorkflow.ShowException(ex);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (GlobalUtilArea.IsPopUp(Request))
                ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "closeScript", "window.close();", true);
            else if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]) != string.Empty)
                Response.Redirect(GetCancelUrl(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL])));
            else
                ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "backScript", "javascript:history.go(-1)", true);
        }
        protected void cmdClientSearch_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            Response.Redirect(GetSearchClientUrl());
        }
        protected void cmdClientNew_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            Response.Redirect(GetNewClientUrl());
        }
        protected void cmdGegnerSearch_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            Response.Redirect(GetSearchGegnerUrl());
        }
        protected void cmdGegnerNew_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            Response.Redirect(GetNewGegnerUrl());
        }
        protected void cmdUserSearch_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            Response.Redirect(GetSearchUserUrl());
        }
        protected void cmdClientSBLoad_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            LoadScreen();
        }
        #endregion

        #region Create URLs
        private string GetSearchClientUrl()
        {
            var sb = new StringBuilder();
            sb.Append("../klienten/KlientBrowser.aspx?" + GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT + "=" + GlobalHtmlParams.URL_NEW_AKT);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.SELECTED_VALUE, "15");
            AppendExtraParams(sb, false, true, true);
            return sb.ToString();
        }
        private string GetNewClientUrl()
        {
            var sb = new StringBuilder();
            sb.Append("../klienten/NewKlient.aspx?" + GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT + "=" + GlobalHtmlParams.URL_NEW_AKT);
            AppendExtraParams(sb, false, true, true);
            return sb.ToString();
        }

        private string GetSearchGegnerUrl()
        {
            var sb = new StringBuilder();
            sb.Append("../gegner/GegnerBrowser.aspx?" + GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT + "=" + GlobalHtmlParams.URL_NEW_AKT);
            AppendExtraParams(sb, true, false, true);
            return sb.ToString();
        }
        private string GetNewGegnerUrl()
        {
            var sb = new StringBuilder();
            sb.Append("../gegner/NewGegner.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.URL_NEW_AKT);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT);
            AppendExtraParams(sb, true, false, true);
            return sb.ToString();
        }

        private string GetSearchUserUrl()
        {
            var sb = new StringBuilder();
            sb.Append("../user/UserBrowser.aspx?" + GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT + "=" + GlobalHtmlParams.URL_NEW_AKT);
            AppendExtraParams(sb, true, true, false);
            return sb.ToString();
        }

        private string GetCancelUrl(string urlCode)
        {
            switch (urlCode)
            {
                case GlobalHtmlParams.INKASSO_MAIN_SCREEN:
                    return "../../intranet/aktenink/AktenStaff.asp?" + Session["var"];
                default:
                    return "";
            }
        }

        private void AppendExtraParams(StringBuilder sb, bool getClient, bool getGegner, bool getUser)
        {
            string extraParams = GetEntriesAsParams(getClient, getGegner, getUser);
            if (extraParams.Length > 1)
            {
                sb.Append("&");
                sb.Append(GlobalHtmlParams.EXTRA_PARAMS);
                sb.Append("=");
                sb.Append(GlobalUtilArea.EncodeURL(extraParams.Substring(1)));
            }
        }
        #endregion

        private tblCustInkAkt GetEnteredAkt(bool save)
        {
            var akt = new tblCustInkAkt
                          {
                              CustInkAktAuftraggeber =
                                  GlobalUtilArea.GetZeroIfConvertToIntError(ddlAuftraggeber.SelectedValue),
                              CustInkAktKlient =
                                  GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupKlient.GetKlientID()),
                              CustInkAktGegner =
                                  GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupGegner.GetGegnerID()),
                              CustInkAktKunde = txtClientInvoiceNumber.Text,
                              CustInkAktEnterDate = DateTime.Now,
                              CustInkAktLastChange = DateTime.Now,
                              CustInkAktEnterUser = GlobalUtilArea.GetUserId(Session),
                              CustInkAktGothiaNr = txtClientReferenceNumber.Text,
                              CustInkAktInvoiceDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtClientInvoiceDate),
                              CustInkAktForderung = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtForderung),
                              CustInkAktSB = GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupUser.GetUserID()),
                              CustInkAkKlientSB = GlobalUtilArea.GetZeroIfConvertToIntError(ddlClientSB.SelectedValue),
                              CustInkAktNextWFLStep = ctlWorkflow.GetNextWFLStepDate(),
                              CustInkAktStatus = ctlWorkflow.GetMainStatus(),
                              CustInkAktCurStatus = ctlWorkflow.GetSecondaryStatus(),
                              CustInkAktIsWflStopped = ctlWorkflow.IsWflStopped(),
                              CustInkAktIsPartial = true,
                              CustInkAktSendBericht = true,
                              CustInkAktMemo = txtMemo.Text
                          };
            akt.CustInkAktLastChangeUser = akt.CustInkAktEnterUser;

            _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientId = " + akt.CustInkAktKlient, typeof(tblKlient));
            
            /* assign lawyer info from klient */

            akt.CustInkAktLawyerId = _klient.KlientLawyerId;
            
            if (akt.CustInkAktSB == 0)
                akt.CustInkAktSB = 99; // Nadine Auer
            if (save)
            {
                RecordSet.Insert(akt);
                akt = (tblCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblCustInkAkt ORDER BY CustInkAktID DESC", typeof(tblCustInkAkt));
            }
            return akt;
        }

        private void LoadAkt(bool saveInSession = false)
        {
            _akt.CustInkAktAuftraggeber = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAuftraggeber.SelectedValue);
            _akt.CustInkAktKlient = GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupKlient.GetKlientID());
            _akt.CustInkAktGegner = GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupGegner.GetGegnerID());
            _akt.CustInkAktKunde = txtClientInvoiceNumber.Text;
            _akt.CustInkAktEnterDate = DateTime.Now;
            _akt.CustInkAktLastChange = DateTime.Now;
            _akt.CustInkAktEnterUser = GlobalUtilArea.GetUserId(Session);
            _akt.CustInkAktGothiaNr = txtClientReferenceNumber.Text;
            _akt.CustInkAktInvoiceDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtClientInvoiceDate);
            _akt.CustInkAktForderung = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtForderung);
            _akt.CustInkAktSB = GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupUser.GetUserID());
            _akt.CustInkAkKlientSB = GlobalUtilArea.GetZeroIfConvertToIntError(ddlClientSB.SelectedValue);
            _akt.CustInkAktNextWFLStep = ctlWorkflow.GetNextWFLStepDate();
            _akt.CustInkAktStatus = ctlWorkflow.GetMainStatus();
            _akt.CustInkAktCurStatus = ctlWorkflow.GetSecondaryStatus();
            _akt.CustInkAktIsWflStopped = ctlWorkflow.IsWflStopped();
            _akt.CustInkAktIsPartial = true;
            _akt.CustInkAktSendBericht = true;
            _akt.CustInkAktMemo = txtMemo.Text;
            
            if (saveInSession)
                Session[GlobalHtmlParams.INKASSO_AKT] = _akt;
        }

        private void LoadScreen()
        {
            if (_akt == null) 
                return;

            LoadKlientSbDdl(_akt.CustInkAktKlient);
            
            txtClientInvoiceNumber.Text = _akt.CustInkAktKunde;
            txtClientReferenceNumber.Text =_akt.CustInkAktGothiaNr;
            txtClientInvoiceDate.Text = HTBUtils.IsDateValid(_akt.CustInkAktInvoiceDate) ? _akt.CustInkAktInvoiceDate.ToShortDateString() : "";
            txtForderung.Text = HTBUtils.FormatCurrencyNumber(_akt.CustInkAktForderung);

            GlobalUtilArea.SetSelectedValue(ddlAuftraggeber, _akt.CustInkAktAuftraggeber.ToString());
            GlobalUtilArea.SetSelectedValue(ddlClientSB, _akt.CustInkAkKlientSB.ToString());
            
            LoadLookupControlsFromAkt();
        }

        private void LoadLookupControlsFromAkt()
        {
            if (_akt.CustInkAktGegner > 0)
            {
                _gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + _akt.CustInkAktGegner, typeof(tblGegner));
                if (_gegner != null)
                    ctlLookupGegner.SetGegner(_gegner);
            }
            if (_akt.CustInkAktKlient > 0)
            {
                _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientID = " + _akt.CustInkAktKlient, typeof(tblKlient));
                if (_klient != null)
                    ctlLookupKlient.SetKlient(_klient);
            }
            if (_akt.CustInkAktSB > 0)
            {
                _user = (qryUsers)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryUsers WHERE UserID = " + _akt.CustInkAktSB, typeof(qryUsers));
                if (_user != null)
                    ctlLookupUser.SetUser(_user);
            }
        }
        private void LoadKlientSbDdl(int klientId)
        {
            if (klientId > 0)
            {
                ArrayList klientSbList = HTBUtils.GetSqlRecords("SELECT UserId, UserVorname + ' ' + UserNachname [UserName] FROM tblUser WHERE UserKlient = " + klientId + " ORDER BY UserVorname", typeof(UserLookup));
                GlobalUtilArea.LoadDropdownList(ddlClientSB, klientSbList, "UserId", "UserName", false);
            }
        }
        
        private void CreateInvoices(tblCustInkAkt akt)
        {
            var invMgr = new InvoiceManager();
            invMgr.CreateAndSaveInvoice(akt.CustInkAktID, tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL, akt.CustInkAktForderung, "Kapital - Forderung", false);
            double clientCosts = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtKosten);
            if (clientCosts > 0)
            {
                invMgr.CreateAndSaveInvoice(akt.CustInkAktID, tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST, clientCosts, "Klient - Kosten", false);
            }
        }

        private bool ValidateEntry()
        {
            var sb = new StringBuilder();
            bool ok = true;
            if (GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtClientInvoiceDate).ToShortDateString() == HTBUtils.DefaultShortDate)
            {
                sb.Append("<strong><i>Rechnungsdatum</i></strong> falsch!<br/>");
                ok = false;
            }
            if (GlobalUtilArea.IsZero(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtForderung)))
            {
                sb.Append("<strong><i>Forderung</i></strong> falsch!<br/>");
                ok = false;
            }
            if (GlobalUtilArea.IsZero(GlobalUtilArea.GetZeroIfConvertToDoubleError(ddlAuftraggeber.SelectedValue)))
            {
                sb.Append("<strong><i>Auftraggeber</i></strong> falsch!<br/>");
                ok = false;
            }
            if (GlobalUtilArea.IsZero(GlobalUtilArea.GetZeroIfConvertToDoubleError(ctlLookupKlient.GetKlientID())))
            {
                sb.Append("<strong><i>Klient</i></strong> falsch!<br/>");
                ok = false;
            }
            if (GlobalUtilArea.IsZero(GlobalUtilArea.GetZeroIfConvertToDoubleError(ctlLookupGegner.GetGegnerID())))
            {
                sb.Append("<strong><i>Gegner</i></strong> falsch!<br/>");
                ok = false;
            }
            var gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + ctlLookupGegner.GetGegnerID(), typeof(tblGegner));

            bool isGegnerOutsideAustria = gegner.GegnerLastZipPrefix.ToUpper() != "A";
            if(isGegnerOutsideAustria)
            {
                // if the zip coe is 83000 - 83999 treat the akt as if it were within the country
                int gegnerZipCode = GlobalUtilArea.GetZeroIfConvertToIntError(gegner.GegnerLastZip);
                isGegnerOutsideAustria = !(gegner.GegnerLastZipPrefix.ToUpper() == "D" && gegnerZipCode >= 83000 && gegnerZipCode <= 83999);
            }
            if(isGegnerOutsideAustria)
            {
                if (!ctlWorkflow.IsWorkflowEntered())
                {
                    ok = false;
                    sb.Append("<strong><i>Gegner im Ausland:</i></strong> Die workflow muss eingegeben werden!<br/>");
                }
                else
                {
                    ok &= ctlWorkflow.ValidateWorkflow(false);
                }
            }
            else if (ctlWorkflow.IsWorkflowEntered()) {
                    ok &= ctlWorkflow.ValidateWorkflow();
            }
            
            if (!ok)
            {
                ctlWorkflow.AppendError(sb.ToString());
                ctlMessage.AppendError(sb.ToString());
            }
            return ok;
        }

        private bool UploadFiles(tblCustInkAkt akt)
        {
            try
            {
                string folderPath = HTBUtils.GetConfigValue("DocumentsFolder");
                // Get the HttpFileCollection
                HttpFileCollection hfc = Request.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFile hpf = hfc[i];
                    if (hpf.ContentLength > 0)
                    {
                        string fileName = akt.CustInkAktID + "_" + Path.GetFileName(hpf.FileName);

                        hpf.SaveAs(folderPath + fileName);
                        RecordSet.Insert(new tblDokument
                        {
                            // CollectionInvoice
                            DokDokType = 25,
                            DokCaption = HTBUtils.GetJustFileName(hpf.FileName),
                            DokInkAkt = akt.CustInkAktID,
                            DokCreator = akt.CustInkAktSB,
                            DokAttachment = fileName,
                            DokCreationTimeStamp = DateTime.Now,
                            DokChangeDate = DateTime.Now
                        });
                        var doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof(tblDokument));
                        if (doc != null)
                        {
                            RecordSet.Insert(new tblAktenDokumente { ADAkt = akt.CustInkAktID, ADDok = doc.DokID, ADAkttyp = 1 });
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetEntriesAsParams(bool getClient, bool getGegner, bool getUser)
        {
            var sb = new StringBuilder();
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.CLIENT_INVOICE_NUMBER, txtClientInvoiceNumber.Text);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.CLIENT_REFERENCE_NUMBER, txtClientReferenceNumber.Text);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.MEMO, txtMemo.Text);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.AUFTRAGGEBER_ID, ddlAuftraggeber.SelectedValue);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.CLIENT_INVOICE_DATE, txtClientInvoiceDate.Text);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.CLIENT_INVOICE_AMOUNT, txtForderung.Text);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.CLIENT_EXTRA_CHARGES, txtKosten.Text);

            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.INKASSO_AKT_MAIN_STATUS, ctlWorkflow.GetMainStatusText());
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.INKASSO_AKT_CURRENT_STATUS, ctlWorkflow.GetSecondaryStatusText());
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.INKASSO_AKT_NEXT_ACTION_DATE, ctlWorkflow.GetNextActionExecDateText());
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.INKASSO_AKT_STOP_WORKFLOW, GlobalUtilArea.BoolToString(ctlWorkflow.IsWflStopped()));
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]));
            

            if (getClient)
            {
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.CLIENT_ID, ctlLookupKlient.GetKlientID());
            }
            if (getGegner)
            {
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.GEGNER_ID, ctlLookupGegner.GetGegnerID());
            }
            if (getUser)
            {
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.USER_ID, ctlLookupUser.GetUserID());
            }
            return sb.ToString();
        }

        private void SetValuesFromParams()
        {
            txtClientInvoiceNumber.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.CLIENT_INVOICE_NUMBER]);
            txtClientReferenceNumber.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.CLIENT_REFERENCE_NUMBER]);
            txtMemo.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.MEMO]);
            txtClientInvoiceDate.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.CLIENT_INVOICE_DATE]);
            txtForderung.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.CLIENT_INVOICE_AMOUNT]);
            txtKosten.Text = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.CLIENT_EXTRA_CHARGES]);

            SetDropdownSelectedValue(ddlAuftraggeber, GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.AUFTRAGGEBER_ID]), "41");
            SetDropdownSelectedValue(ctlWorkflow.GetDdlMainStatus(), GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.INKASSO_AKT_MAIN_STATUS]), "0");
            SetDropdownSelectedValue(ctlWorkflow.GetDdlSecondaryStatus(), GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.INKASSO_AKT_CURRENT_STATUS]), "1");

            ctlWorkflow.SetNextActionExecDateText(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.INKASSO_AKT_NEXT_ACTION_DATE]));
            ctlWorkflow.SetWflStopped(GlobalUtilArea.StringToBool(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.INKASSO_AKT_STOP_WORKFLOW])));
        }

        private void SetDropdownSelectedValue(DropDownList ddl, string val1, string val2)
        {
            if (!string.IsNullOrEmpty(val1))
            {
                try
                {
                    ddl.SelectedValue = val1;
                }
                catch { }
            }
            else if (!string.IsNullOrEmpty(val2))
            {
                try
                {
                    ddl.SelectedValue = val2; 
                }
                catch (Exception){
                    ddl.SelectedIndex = 1;
                }
            }
        }

        private void ClearScreen()
        {
            ctlLookupKlient.Clear(); 
            ctlLookupGegner.Clear();
            ctlLookupUser.Clear();
            txtClientInvoiceDate.Text = "";
            txtClientInvoiceNumber.Text = "";
            txtClientReferenceNumber.Text = "";
            txtForderung.Text = "";
            txtKosten.Text = "";
            txtMemo.Text = "";
            
            ddlAuftraggeber.SelectedValue = "41";
            
            ctlWorkflow.ClearEntireScreen();
            Session[GlobalHtmlParams.INKASSO_AKT] = null;
        }

        public string GetKlientID()
        {
            return ctlLookupKlient.GetKlientID();
        }

        public string GetSelectedMainStatus()
        {
            return "3";
        }

        public string GetSelectedSecondaryStatus()
        {
            return "1";
        }
    }
}