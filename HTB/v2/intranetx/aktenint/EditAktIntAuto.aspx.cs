using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint
{
    public partial class EditAktIntAuto : System.Web.UI.Page
    {
        private tblAktenInt _akt;
        private tblGegner _gegner;
        private tblAuftraggeber _ag;
        private tblAutoDealer _dealer;
        private bool _isNew = true;
        private double _totalAmountDue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Registering the buttons control with RegisterPostBackControl early in the page lifecycle 
            // so that the FileUploadControl will upload files inside the UpdatePanel control
            ScriptManager1.RegisterPostBackControl(btnSubmit);
            ScriptManager1.RegisterPostBackControl(btnCancel);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Session["MM_UserID"] = "99";
            const string componentPrefix = "";

            if (GlobalUtilArea.StringToBool(Request[GlobalHtmlParams.REFRESH_INTERVENTION_WORK_AKT]))
                Session[GlobalHtmlParams.INTERVENTION_WORK_AKT] = null;

            ctlLookupAuftraggeber.SetComponentName(componentPrefix + ctlLookupAuftraggeber.ID);
            ctlLookupAuftraggeber.SetNextFocusableComponentId(componentPrefix + ctlLookupGegner.ID + "_" + ctlLookupGegner.GetTextBox().ID);

            ctlLookupGegner.SetComponentName(componentPrefix + ctlLookupGegner.ID);
            ctlLookupGegner.SetNextFocusableComponentId(componentPrefix + ctlLookupGegner2.ID + "_" + ctlLookupGegner2.GetTextBox().ID);

            ctlLookupGegner2.SetComponentName(componentPrefix + ctlLookupGegner2.ID);
            ctlLookupGegner2.SetNextFocusableComponentId(componentPrefix + ctlLookupDealer.ID + "_" + ctlLookupDealer.GetTextBox().ID);

            ctlLookupDealer.SetComponentName(componentPrefix + ctlLookupDealer.ID);
            ctlLookupDealer.SetNextFocusableComponentId(componentPrefix + txtTermin.ID);
            
            _akt = HTBUtils.GetInterventionAkt(GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.ID]));

            _isNew = _akt == null;

            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlAktType,
                                                "SELECT * FROM tblAktTypeInt WHERE AktTypeINTCaption LIKE '%Auto%' ORDER BY AktTypeIntCaption DESC",
                                                typeof (tblAktTypeInt),
                                                "AktTypeINTID",
                                                "AktTypeINTCaption",
                                                false);
                GlobalUtilArea.LoadDropdownList(ddlAutoType,
                                                "SELECT * FROM tblAutoType ORDER BY AutoTypeCaption ASC",
                                                typeof(tblAutoType),
                                                "AutoTypeID",
                                                "AutoTypeCaption",
                                                true);

                GlobalUtilArea.LoadUserDropdownList(ddlSB,GlobalUtilArea.GetUsers(Session));
                if (Session[GlobalHtmlParams.INTERVENTION_WORK_AKT] != null)
                {
                    _akt = (tblAktenInt) Session[GlobalHtmlParams.INTERVENTION_WORK_AKT];
                }
                if(_akt != null)
                {
                    LoadScreen();
                }
                else
                {
                    _akt = new tblAktenInt();
                }

                LoadLookupControlsFromUrlParameters();
            }
            else
            {
                if(_akt == null)
                    _akt = new tblAktenInt();
                LoadAkt();
                LoadLookupControlsFromAkt();
            }

            trFileUpload.Visible = _isNew;
            trEditOnly.Visible = !_isNew;
            trStatus.Visible = !_isNew;
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            try
            {
                if (IsAktValid())
                {
                    if (SaveAkt())
                    {
                        ctlMessage.ShowSuccess("Akt gespeichert [" + _akt.AktIntID + "]");
                        // Clear Screen
                        _akt = new tblAktenInt();
                        LoadScreen();
                        ctlLookupAuftraggeber.Clear();
                        ctlLookupGegner.Clear();
                        ctlLookupGegner2.Clear();
                        ctlLookupDealer.Clear();
                        Session[GlobalHtmlParams.INTERVENTION_WORK_AKT] = null;     // release temporary akt info
                    }
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session[GlobalHtmlParams.INTERVENTION_WORK_AKT] = null;
            try
            {
                if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]) == GlobalHtmlParams.BACK)
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "backScript", "javascript:history.go(-1);", true);
                else
                    ShowParentScreen();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        protected void txtAZ_TextChanged(object sender, EventArgs e)
        {
            var akt = (tblAktenInt) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntAZ = '" + txtAZ.Text + "' ORDER BY AktIntDatum DESC", typeof (tblAktenInt));
            if(akt != null)
            {
                _akt = akt;
                LoadScreen();
                LoadLookupControlsFromUrlParameters();
//                ctlLookupAuftraggeber.GetTextBox().Focus();
            }
            txtAktIntAutoVertragArt.Focus();
        }
        protected void ddlAktType_SelectionChanged(object sender, EventArgs e)
        {
            bool isInkasso = isInkassoAllowed();
            
            trAktIntMissingInstallments.Visible = isInkasso;
            trAgKosten.Visible = isInkasso;
            trInsuranceAmount.Visible = isInkasso;
            trOpenedAmount.Visible = isInkasso;


        }
        protected void cmdGegnerSearch_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("../gegner/GegnerBrowser.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO, false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            
            Response.Redirect(sb.ToString());
        }

        protected void cmdGegnerNew_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("../gegner/NewGegner.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO, false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            Response.Redirect(sb.ToString());
        }

        protected void cmdGegnerPhone_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("/v2/intranetx/gegner/EditGegnerPhone.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.GEGNER_ID, ctlLookupGegner.GetGegnerID(), false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            Response.Redirect(sb.ToString());
        }

        protected void cmdGegnerAddress_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("/v2/intranetx/gegner/EditGegnerAddress.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.GEGNER_ID, ctlLookupGegner.GetGegnerID(), false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            Response.Redirect(sb.ToString());
        }

        protected void cmdGegner2Search_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("../gegner/GegnerBrowser.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO, false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_GEGNER2, GlobalHtmlParams.YES);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            Response.Redirect(sb.ToString());
        }

        protected void cmdGegner2New_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("../gegner/NewGegner.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO, false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_GEGNER2, GlobalHtmlParams.YES);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            Response.Redirect(sb.ToString());
        }

        protected void cmdGegner2Phone_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("/v2/intranetx/gegner/EditGegnerPhone.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.GEGNER_ID, ctlLookupGegner2.GetGegnerID(), false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            Response.Redirect(sb.ToString());
        }

        protected void cmdGegner2Address_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("/v2/intranetx/gegner/EditGegnerAddress.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.GEGNER_ID, ctlLookupGegner2.GetGegnerID(), false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            Response.Redirect(sb.ToString());
        }
        
        protected void cmdDealerSearch_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("../dealer/DealerBrowser.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO, false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());

            Response.Redirect(sb.ToString());
        }

        protected void cmdDealerNew_Click(object sender, EventArgs e)
        {
            LoadAkt(true);
            var sb = new StringBuilder();
            sb.Append("../dealer/EditDealer.aspx?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO, false);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL, GlobalHtmlParams.URL_NEW_AKT_INT_AUTO);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, _akt.AktIntID.ToString());
            Response.Redirect(sb.ToString());
        }

        protected void cmdSearchSB_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(ctlLookupGegner.GetGegnerOldID()))
                ddlSB.SelectedValue = HTBUtils.GetGegnerSB(ctlLookupGegner.GetGegnerOldID()).ToString();
            else
                ctlMessage.ShowError("Bitte Shuldner eingeben!");
        }

        protected void cmdGetCollectionAmount_Click(object sender, EventArgs e)
        {
            var ok = true;
            double amount = 0;
            var aktTypeId = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktType.SelectedValue);
            var agIg = GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupAuftraggeber.GetAuftraggeberID());
            ctlMessage.Clear();

            if (aktTypeId < 1)
            {
                ctlMessage.ShowError("Bitte Akt Typ w&auml;hlen!");
                ok = false;
            }
            if (ok && agIg < 1)
            {
                ctlMessage.ShowError("Bitte Auftraggeber w&auml;hlen!");
                ok = false;
            }
            var aktType = (tblAktTypeInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktTypeInt WHERE AktTypeIntId = " + aktTypeId, typeof(tblAktTypeInt));
            if (ok && aktType == null)
            {
                ctlMessage.ShowError("Akttype nicht gefunden.... sehr Komisch!");
                ok = false;
            }
            if (ok)
            {
                var parameters = new ArrayList
                                     {
                                         new StoredProcedureParameter("agId", SqlDbType.Int, agIg),
                                         new StoredProcedureParameter("aktTypeCode", SqlDbType.Int, aktType.AktTypeINTCode),
                                         new StoredProcedureParameter("amount", SqlDbType.Float, amount, ParameterDirection.Output)
                                     };

                HTBUtils.GetStoredProcedureSingleRecord("spGetAutoAktCollectionPrice", parameters, typeof (Record));
                foreach (object o in parameters)
                {
                    if (o is ArrayList)
                    {
                        var outputList = (ArrayList) o;
                        foreach (StoredProcedureParameter p in outputList)
                        {
                            if (p.Name.IndexOf("amount") >= 0)
                            {
                                amount = Convert.ToDouble(p.Value);
                                break;
                            }
                        }
                    }
                }
            }
            txtAktIntKosten.Text = HTBUtils.FormatCurrencyNumber(amount);
        }

        #endregion

        private void LoadAkt(bool saveInSession = false)
        {
            _akt.AktIntAktType = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktType.SelectedValue);
            _akt.AktIntAZ = txtAZ.Text;
            _akt.AktIntAutoVertragArt = txtAktIntAutoVertragArt.Text;
            _akt.AktIntAuftraggeber = GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupAuftraggeber.GetAuftraggeberID());
            _akt.AktIntGegner = ctlLookupGegner.GetGegnerOldID();
            _akt.AktIntGegner2 = GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupGegner2.GetGegnerID());
            _akt.AktIntAutoDealerId = GlobalUtilArea.GetZeroIfConvertToIntError(ctlLookupDealer.GetDealerID());
            _akt.AktIntSB = GlobalUtilArea.GetZeroIfConvertToIntError(ddlSB.SelectedValue);
            _akt.AktIntTermin = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtTermin.Text);
            _akt.AktIntTerminAD = _akt.AktIntTermin;
            _akt.AktIntAgSbType = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktIntAgSbType.SelectedValue);
            _akt.AKTIntAGSB = txtAktIntAGSB.Text;
            _akt.AKTIntKSVEMail = txtAktIntKSVEMail.Text;
            _akt.AktIntStatus = GlobalUtilArea.GetZeroIfConvertToIntError(ddlStatus.SelectedValue);

            _akt.AktIntAutoTypeId = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAutoType.SelectedValue);
            _akt.AktIntAutoName = txtAktIntAutoName.Text;
            _akt.AktIntAutoIdNr = txtAktIntAutoIdNr.Text;
            _akt.AktIntAutoFirstRegistrationDate =
                GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtAktIntAutoFirstRegistrationDate.Text);
            _akt.AktIntAutoKZ = txtAktIntAutoKZ.Text;
            _akt.AktIntAutoColor = txtAktIntAutoColor.Text;

            _akt.AktIntMissingInstallments = GlobalUtilArea.GetZeroIfConvertToIntError(txtAktIntMissingInstallments);
            _akt.AktIntAmountOpened = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtAmountOpened);
            _akt.AktIntAgKosten = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtAgKosten);
            _akt.AktIntInsuranceAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtInsuranceAmount);
            _akt.AKTIntKosten = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtAktIntKosten);

            _akt.AktIntInsuranceName = txtAktIntInsuranceName.Text;
            _akt.AktIntInsurancePhone = txtAktIntInsurancePhone.Text;
            _akt.AktIntInsuranceAccount = txtAktIntInsuranceAccount.Text;
            _akt.AktIntInsuranceBankName = txtAktIntInsuranceBankName.Text;
            _akt.AktIntInsuranceBLZ = txtAktIntInsuranceBLZ.Text;
            _akt.AktIntInsuranceKtoNr = txtAktIntInsuranceKtoNr.Text;

            _akt.AKTIntMemo = txtMemo.Text;
            if (saveInSession)
                Session[GlobalHtmlParams.INTERVENTION_WORK_AKT] = _akt;
        }

        private void LoadScreen()
        {
            if (_akt == null) return;
            
            lblAktNr.Text = _isNew ? "(wird autom. vergeben)" : _akt.AktIntID.ToString();

            SetSelectedValue(ddlAktType, _akt.AktIntAktType.ToString());
            txtAZ.Text = _akt.AktIntAZ;
            txtAktIntAutoVertragArt.Text = _akt.AktIntAutoVertragArt;
            SetSelectedValue(ddlSB, _akt.AktIntSB.ToString());
            txtTermin.Text = GlobalUtilArea.GetStringValueForNonDefaultDate(_akt.AktIntTermin);
            SetSelectedValue(ddlAktIntAgSbType, _akt.AktIntAgSbType.ToString());
            txtAktIntAGSB.Text = _akt.AKTIntAGSB;
            txtAktIntKSVEMail.Text = _akt.AKTIntKSVEMail;
            
            ddlStatus.SelectedValue = _akt.AktIntStatus.ToString();

            SetSelectedValue(ddlAutoType, _akt.AktIntAutoTypeId.ToString());
            txtAktIntAutoName.Text = _akt.AktIntAutoName;
            txtAktIntAutoIdNr.Text = _akt.AktIntAutoIdNr;
            txtAktIntAutoFirstRegistrationDate.Text = GlobalUtilArea.GetStringValueForNonDefaultDate(_akt.AktIntAutoFirstRegistrationDate);
            txtAktIntAutoKZ.Text = _akt.AktIntAutoKZ;
            txtAktIntAutoColor.Text = _akt.AktIntAutoColor;

            txtAktIntMissingInstallments.Text = GlobalUtilArea.GetStringValueForNonZeroAmount(_akt.AktIntMissingInstallments);
            txtAmountOpened.Text = GlobalUtilArea.GetCurrencyValueForNonZeroAmount(_akt.AktIntAmountOpened);
            txtAgKosten.Text = GlobalUtilArea.GetCurrencyValueForNonZeroAmount(_akt.AktIntAgKosten);
            txtInsuranceAmount.Text = GlobalUtilArea.GetCurrencyValueForNonZeroAmount(_akt.AktIntInsuranceAmount);
            txtAktIntKosten.Text = GlobalUtilArea.GetCurrencyValueForNonZeroAmount(_akt.AKTIntKosten);

            txtAktIntInsuranceName.Text =_akt.AktIntInsuranceName;
            txtAktIntInsurancePhone.Text = _akt.AktIntInsurancePhone;
            txtAktIntInsuranceAccount.Text = _akt.AktIntInsuranceAccount;
            txtAktIntInsuranceBankName.Text = _akt.AktIntInsuranceBankName;
            txtAktIntInsuranceBLZ.Text = _akt.AktIntInsuranceBLZ;
            txtAktIntInsuranceKtoNr.Text = _akt.AktIntInsuranceKtoNr;

            txtMemo.Text = _akt.AKTIntMemo;
                
            LoadLookupControlsFromAkt();
            PopulateInvoicesGrid();
            PopulateDocumentsGrid();

            trOpenedAmount.Visible = _isNew;
            trAgKosten.Visible = _isNew;
            trInsuranceAmount.Visible = _isNew;
            
        }

        private void LoadLookupControlsFromAkt()
        {
            if (_akt.AktIntAuftraggeber > 0)
            {
                _ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + _akt.AktIntAuftraggeber, typeof(tblAuftraggeber));
                if (_ag != null)
                    ctlLookupAuftraggeber.SetAuftraggeber(_ag);
            }
            if (!string.IsNullOrEmpty(_akt.AktIntGegner))
            {
                _gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerOldID = " + _akt.AktIntGegner, typeof(tblGegner));
                if (_gegner != null)
                    ctlLookupGegner.SetGegner(_gegner);
            }
            if (_akt.AktIntGegner2 > 0)
            {
                _gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + _akt.AktIntGegner2, typeof(tblGegner));
                if (_gegner != null)
                    ctlLookupGegner2.SetGegner(_gegner);
            }
            if (_akt.AktIntAutoDealerId > 0)
            {
                _dealer = (tblAutoDealer)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAutoDealer WHERE AutoDealerID = " + _akt.AktIntAutoDealerId, typeof(tblAutoDealer));
                if (_dealer != null)
                    ctlLookupDealer.SetDealer(_dealer);
            }
        }

        private void LoadLookupControlsFromUrlParameters()
        {
            if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.AUFTRAGGEBER_ID]).Equals(""))
            {
                _ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + Request[GlobalHtmlParams.AUFTRAGGEBER_ID], typeof(tblAuftraggeber));
                ctlLookupAuftraggeber.SetAuftraggeber(_ag);
                ctlLookupGegner.GetTextBox().Focus();

            }
            if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.GEGNER_ID]).Equals(""))
            {
                _gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + Request[GlobalHtmlParams.GEGNER_ID], typeof(tblGegner));
                ctlLookupGegner.SetGegner(_gegner);
                ctlLookupGegner2.GetTextBox().Focus();
            }
            if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.GEGNER2_ID]).Equals(""))
            {
                _gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + Request[GlobalHtmlParams.GEGNER2_ID], typeof(tblGegner));
                ctlLookupGegner2.SetGegner(_gegner);
                ctlLookupDealer.GetTextBox().Focus();
            }
            if (!GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.DEALER_ID]).Equals(""))
            {
                _dealer = (tblAutoDealer)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAutoDealer WHERE AutoDealerID = " + Request[GlobalHtmlParams.DEALER_ID], typeof(tblAutoDealer));
                ctlLookupDealer.SetDealer(_dealer);
                txtTermin.Focus();
            }
        }

        private bool IsAktValid()
        {
            var sb = new StringBuilder();
            bool ok = true;
            
            if (string.IsNullOrEmpty(_akt.AktIntAZ))
            {
                sb.Append("Vertragsnummer ung&uuml;tig!<BR/>");
                ok = false;
            }
            if(_akt.AktIntAuftraggeber <= 0)
            {
                sb.Append("Auftraggeber ung&uuml;tig!<BR/>");
                ok = false;
            }
            if(string.IsNullOrEmpty(_akt.AktIntGegner))
            {
                sb.Append("Gegner ung&uuml;tig!<BR/>");
                ok = false;
            }
            if(_isNew && isInkassoAllowed() && GlobalUtilArea.GetZeroIfConvertToDoubleError(txtAmountOpened) <= 0)
            {
                sb.Append("Offene Leasingrate ung&uuml;tig!<BR/>");
                ok = false;
            }
            if(!ok)
            {
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }
        private bool isInkassoAllowed()
        {
            return ddlAktType.SelectedItem.ToString().ToLower().IndexOf("inkasso") >= 0;
        }
        private bool SaveAkt()
        {
            _akt.AktIntKlient = HTBUtils.GetConfigValue("AutoEinzug_Old_Klient");
            _akt.AktIntDatum = DateTime.Now;
            bool ok;
            var set = new RecordSet();
            if (_isNew)
            {
                _akt.AktIntStatus = 1; // in bearbeitung (only for new casses)
                ok = set.InsertRecord(_akt);
                if (ok)
                {
                    _akt = (tblAktenInt) HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblAktenInt Order By AktIntID DESC", typeof (tblAktenInt));
                    set = new RecordSet();
                    try
                    {
                        set.StartTransaction();
                        InsertPosRecord(set, _akt.AktIntID, "Buchung lt. " + _akt.AktIntAZ, "Offene Forderungen", GlobalUtilArea.GetZeroIfConvertToDoubleError(txtAmountOpened), tblAktenIntPosType.INVOICE_TYPE_ORIGINAL);
                        InsertPosRecord(set, _akt.AktIntID, "Buchung lt. " + _akt.AktIntAZ, "Auftraggeber Kosten", GlobalUtilArea.GetZeroIfConvertToDoubleError(txtAgKosten), tblAktenIntPosType.INVOICE_TYPE_CLIENT_COST);
                        InsertPosRecord(set, _akt.AktIntID, "Buchung lt. " + _akt.AktIntAZ, "Versicherung", GlobalUtilArea.GetZeroIfConvertToDoubleError(txtInsuranceAmount), tblAktenIntPosType.INVOICE_TYPE_INSURANCE);
                        set.CommitTransaction();
                        UploadFiles();
                        HTBUtils.NotifySBAboutNewAkt(_akt.AktIntID);
                    }
                    catch (Exception ex)
                    {
                        set.RollbackTransaction();
                        ctlMessage.ShowException(ex);
                        ok = false;
                    }
                }
            }
            else
            {
                ok = set.UpdateRecord(_akt);
                ShowParentScreen();
            }
            if (ok)
            {
                Session[GlobalHtmlParams.INTERVENTION_WORK_AKT] = null;
                return true;
            }
            ctlMessage.ShowError("Akt konte nicht gespeichert werden!");
            return false;
        }

        private void InsertPosRecord(RecordSet set, int aktid, string posNr, string posCaption, double posAmount, int posType)
        {
            if (!HTBUtils.IsZero(posAmount))
            {

                set.InsertRecordInTransaction(new tblAktenIntPos
                           {
                               AktIntPosAkt = aktid,
                               AktIntPosBetrag = posAmount,
                               AktIntPosCaption = posCaption,
                               AktIntPosDatum = DateTime.Now,
                               AktIntPosDueDate = DateTime.Now,
                               AktIntPosNr = posNr,
                               AktIntPosTypeCode = posType
                           });
            }
        }
        
        private void SetSelectedValue(DropDownList ddl, string str)
        {
            if (str.Equals("0"))
                ddl.SelectedIndex = 0;
            else
            {
                try
                {
                    ddl.SelectedValue = str;
                }
                catch
                {
                }
            }
        }

        public tblAktenInt GetAkt()
        {
            return _akt;
        }

        public string GetGegnerID()
        {
            return ctlLookupGegner.GetGegnerID();
        }

        public string GetKlientID()
        {
            return HTBUtils.GetConfigValue("AutoEinzug_Klient");
        }

        public void ShowParentScreen()
        {
            Response.Redirect("../../intranet/aktenint/aktenint.asp?" + Session["var"]);
        }

        #region Invoices
        private ArrayList GetInvoicesList()
        {
            if (_akt.IsInkasso())
            {
                //tdNewBuchung.Visible = false;
                return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceStatus <> -1 AND InvoiceCustInkAktId = " + _akt.AktIntCustInkAktID + " ORDER BY InvoiceDate", typeof(tblCustInkAktInvoice));
            }
            else
            {
                tdNewBuchung.Visible = true;
                ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + _akt.AktIntID, typeof(tblAktenIntPos));
                ArrayList list = new ArrayList();
                foreach (tblAktenIntPos pos in posList)
                {
                    tblCustInkAktInvoice inv = new tblCustInkAktInvoice();
                    inv.InvoiceID = -1;
                    inv.InvoiceCustInkAktId = pos.AktIntPosID;
                    inv.InvoiceDate = pos.AktIntPosDatum;
                    inv.InvoiceDescription = pos.AktIntPosCaption;
                    inv.InvoiceAmount = pos.AktIntPosBetrag;
                    inv.InvoiceComment = pos.AktIntPosNr;
                    inv.InvoiceBalance = pos.AktIntPosBetrag;
                    inv.InvoiceDueDate = pos.AktIntPosDueDate;
                    list.Add(inv);
                }
                return list;
            }
        }
        private void PopulateInvoicesGrid()
        {
            ArrayList invList = GetInvoicesList();
            _totalAmountDue = 0;
            DataTable dt = GetInvoicesDataTableStructure();
            foreach (tblCustInkAktInvoice inv in invList)
            {
                if (inv.IsPayment())
                {
                    inv.InvoiceAmount *= -1;
                    inv.InvoiceBalance *= -1;
                }
                _totalAmountDue += inv.InvoiceBalance;
                DataRow dr = dt.NewRow();

                dr["DeleteUrl"] = "../../intranet/images/delete2hover.gif";
                dr["EditUrl"] = "../../intranet/images/edit.gif";
                dr["InvoiceID"] = inv.InvoiceID;
                dr["InvoiceDate"] = inv.InvoiceDate.ToShortDateString();
                dr["InvoiceDescription"] = inv.InvoiceDescription;
                dr["InvoiceAmount"] = HTBUtils.FormatCurrency(inv.InvoiceAmount);
                dr["DueDate"] = inv.InvoiceDueDate.ToShortDateString();
                if (inv.InvoiceID >= 0)
                {
                    dr["DeletePopupUrl"] = "../global_forms/GlobalDelete.aspx?titel=Position%20löschen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20löschen:&amp;strTable=tblCustInkAktInvoice&amp;strTextField=InvoiceDescription&amp;strColumn=InvoiceID&amp;ID=" + inv.InvoiceID;
                    dr["EditPopupUrl"] = "../aktenink/EditInvoice.aspx?ID=" + inv.InvoiceID;
                }
                else
                {
                    dr["PosInvoiceID"] = inv.InvoiceComment;
                    dr["DeletePopupUrl"] = "../../intranet/global_forms/globaldelete.asp?titel=Position%20entfernen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20entfernen:&amp;strTable=tblAktenIntPos&amp;strTextField=AktIntPosCaption&amp;strColumn=AktIntPosID&amp;ID=" + inv.InvoiceCustInkAktId;
                    dr["EditPopupUrl"] = "/v2/intranetx/aktenint/EditBooking.aspx?" + GlobalHtmlParams.INTERVENTION_AKT + "=" + _akt.AktIntID + "&" + GlobalHtmlParams.ID + "=" + inv.InvoiceCustInkAktId;
                }
                dt.Rows.Add(dr);
            }
            // show / hide the right Invoice Number
            if (_akt.IsInkasso())
            {
                gvInvoices.Columns[3].Visible = false;
                tdNewBuchung.Visible = false;
            }
            else
            {
                tdNewBuchung.Visible = true;
                gvInvoices.Columns[2].Visible = false;
            }
            gvInvoices.DataSource = dt;
            gvInvoices.DataBind();
        }
        private DataTable GetInvoicesDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("DeletePopupUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditPopupUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceID", typeof(int)));
            dt.Columns.Add(new DataColumn("PosInvoiceID", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("AppliedAmount", typeof(double)));
            dt.Columns.Add(new DataColumn("DueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceAmount", typeof(string)));

            return dt;
        }
        public double GetTotalDue()
        {
            return _totalAmountDue;
        }
        #endregion

        #region Documents
        private void PopulateDocumentsGrid()
        {
            DataTable dt = GetDocumentsDataTableStructure();
            foreach (qryDoksIntAkten doc in HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + _akt.AktIntID, typeof(qryDoksIntAkten)))
            {
                DataRow dr = dt.NewRow();

                dr["DeleteUrl"] = "../../intranet/images/delete2hover.gif";
                dr["DeletePopupUrl"] = "../global_forms/GlobalDelete.aspx?titel=Position%20löschen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20löschen:&amp;strTable=tblDokument&amp;strTextField=DokAttachment&amp;strColumn=DokID&amp;ID=" + doc.DokID;

                dr["DokCreationTimeStamp"] = doc.DokCreationTimeStamp + " von " + doc.UserVorname + " " + doc.UserNachname;
                dr["DokTypeCaption"] = doc.DokTypeCaption;
                dr["DokCaption"] = doc.DokCaption;
                dr["DokAttachment"] = doc.DokAttachment;
                dt.Rows.Add(dr);
            }
            gvDocs.DataSource = dt;
            gvDocs.DataBind();
        }
        private DataTable GetDocumentsDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("DeletePopupUrl", typeof(string)));

            dt.Columns.Add(new DataColumn("DokCreationTimeStamp", typeof(string)));
            dt.Columns.Add(new DataColumn("DokTypeCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DokCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DokAttachment", typeof(string)));
            return dt;
        }
        #endregion

        private bool UploadFiles()
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
                        string fileName = _akt.AktIntID + "_" + Path.GetFileName(hpf.FileName.Replace(" ", "_"));

                        hpf.SaveAs(folderPath + fileName);
                        RecordSet.Insert(new tblDokument
                        {
                            // Intervention
                            DokDokType = 3,
                            DokCaption = HTBUtils.GetJustFileName(hpf.FileName),
                            DokIntAkt = _akt.AktIntID,
                            DokCreator = GlobalUtilArea.GetUserId(Session),
                            DokAttachment = fileName,
                            DokCreationTimeStamp = DateTime.Now,
                            DokChangeDate = DateTime.Now
                        });
                        var doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof(tblDokument));
                        if (doc != null)
                        {
                            RecordSet.Insert(new tblAktenDokumente { ADAkt = _akt.AktIntID, ADDok = doc.DokID, ADAkttyp = 3 });
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

    }
}