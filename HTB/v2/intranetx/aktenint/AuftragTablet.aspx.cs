using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.aktenintprovfix;
using HTB.v2.intranetx.permissions;
using HTBAktLayer;
using HTBExtras;
using HTBUtilities;
using System.Collections;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTB.v2.intranetx.routeplanner;
using System.Text;
using ActionRecord = HTBExtras.ActionRecord;
using HTBServices;
using HTBServices.Mail;

namespace HTB.v2.intranetx.aktenint
{
    public partial class AuftragTablet : Page
    {
        private static string _appName = HTBUtils.GetConfigValue("iPad_AppName");
        #region New Action Variables
        private int _aktId;
        tblAktenInt _akt;
        tblUser _user;
        tblAuftraggeber _ag;
        tblAktenIntAction _aktAction;
        bool _isNewAction = true;
        #endregion

        private readonly PermissionsNewAction _permissions = new PermissionsNewAction();

        private ArrayList _aktenList = new ArrayList();
        protected AktIntAmounts Pa;
        
        readonly tblControl _control = HTBUtils.GetControlRecord();

        string _aktIdList;
        string _qryAktCommand;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ctlInstallment.Parent = this;
                ctlInstallmentOld.Parent = this;

                LoadAkt();
                LoadActions();
                if (!IsPostBack)
                {
                    PopulateDocumentsGrid();
                    ctlInstallment.SetAktIntId(_aktId);
                    ctlInstallment.LoadAll();
                    ctlInstallmentOld.SetAktIntId(_aktId);
                    ctlInstallmentOld.LoadAll();
                }
            }
            catch
            {
                lblGegnerInfo.Text = "NO DATA :-(";
                lnkPrevious.Visible = false;
                lnkNext.Visible = false;
                btnSaveAction.Visible = false;
            }
            lnkPrintReceipt.Visible = false;
        }

        private void LoadAkt()
        {
            string source = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SOURCE]);
            if (source == GlobalHtmlParams.RoutePlanner)
                LoadFromRoutePlannerManager();
            else
                LoadFromRequestParams();

            if (!IsPostBack)
            {
                LoadPageData(GetPageIndex());
            }
            SetNavigationLinksVisible();
        }
        private void LoadActions(int actionID = -1, bool forceReload = false)
        {
            ClearErrors();
            txtProvision.Visible = false;
            lblProvision.Visible = true;
            trSaveWithMissingBeleg.Visible = false;
            if (actionID > 0)
            {
                _aktAction = (tblAktenIntAction)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntAction WHERE AktIntActionID = " + actionID, typeof(tblAktenIntAction));
                if (_aktAction != null)
                {
                    _isNewAction = false;
                    if (!_permissions.GrantChangeBeleg)
                        txtBeleg.ReadOnly = true;
                }
            }
            _aktId = ((qryAktenInt)_aktenList[GetPageIndex()]).AktIntID;
            _akt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntID = " + _aktId, typeof(tblAktenInt));
            _user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT UserID, UserRoleFB, UserSex, UserVorname, UserNachname, UserHasIPad FROM tblUser WHERE UserID = " + _akt.AktIntSB, typeof(tblUser));
            _ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + _akt.AktIntAuftraggeber, typeof(tblAuftraggeber));
            PopulateExistingActionsGrid();
            if (!IsPostBack || forceReload)
            {
                SetValues();
            }
            lblPrice.Text = "&nbsp;";
            txtBeleg.Visible = !_user.UserHasIPad;
            lblBeleg.Visible = !_user.UserHasIPad;
            if (Session["MM_UserLevelLevel"] != null && Session["MM_UserLevelLevel"].ToString() == "255")
            {
                txtProvision.Visible = true;
                lblProvision.Visible = false;
            }
            if (lblProvision.Text.Trim() == string.Empty)
                lblProvision.Text = "&nbsp;";
            if (ddlAktion.SelectedValue == GlobalUtilArea.DefaultDropdownValue)
                trBetrag.Visible = false;
        }
        private void LoadFromRequestParams()
        {
            if (!string.IsNullOrEmpty(Request[GlobalHtmlParams.ID]))
            {
                _aktIdList = Request[GlobalHtmlParams.ID];

                _qryAktCommand = "SELECT * FROM qryAktenInt WHERE AktIntID in (" + _aktIdList + ")";
                if (Request["sortfield"] != null && Request["sortfield"] != "")
                {
                    _qryAktCommand += "ORDER BY " + Request["sortfield"];
                }
                _aktenList = HTBUtils.GetSqlRecords(_qryAktCommand, typeof(qryAktenInt));
            }
            else if (!string.IsNullOrEmpty(Request[GlobalHtmlParams.GEGNER_ID]))
            {
                _qryAktCommand = "SELECT * FROM qryAktenInt WHERE AktIntStatus in (1, 2) AND GegnerID = " + Request[GlobalHtmlParams.GEGNER_ID];
                if (Request["sortfield"] != null && Request["sortfield"] != "")
                {
                    _qryAktCommand += "ORDER BY " + Request["sortfield"];
                }
                _aktenList = HTBUtils.GetSqlRecords(_qryAktCommand, typeof(qryAktenInt));
            }
        }

        private void LoadFromRoutePlannerManager()
        {
            var rpManager = (RoutePlanerManager) Session[GlobalHtmlParams.SessionRoutePlannerManager];

            //Get AktIds from the Route Planner
            List<int> ids = rpManager.GetAllIDs();
            var sb = new StringBuilder();

            for (int i = 0; i < ids.Count; i++)
            {
                sb.Append(ids[i].ToString());
                if(i < ids.Count - 1)
                    sb.Append(", ");
            }

            _qryAktCommand = "SELECT * FROM dbo.qryAktenInt WHERE AktIntID in (" + sb + ")";
            List<qryAktenInt> recordsList = HTBUtils.GetSqlRecords(_qryAktCommand, typeof (qryAktenInt)).Cast<qryAktenInt>().ToList();

            var sortedList = from i in ids
                             join records in recordsList
                                 on i equals records.AktIntID
                             select records;

            var enumList = sortedList.GetEnumerator();

            while (enumList.MoveNext())
                _aktenList.Add(enumList.Current);
        }

        private void LoadPageData(int index)
        {

            var akt = (qryAktenInt)_aktenList[index];
            Pa = AktInterventionUtils.GetAktAmounts(akt);
            if(akt != null)
            {
                _aktId = akt.AktIntID;

                LoadActions();
                PopulateDocumentsGrid();
                
                lblAuftraggeberName1.Text = akt.AuftraggeberName1;
                lblAuftraggeberName1_2.Text = akt.AuftraggeberName1;
                lblAuftraggeberName1_4.Text = akt.AuftraggeberName1;

                lblAuftraggeberName2.Text = akt.AuftraggeberName2;
                lblAuftraggeberName2_2.Text = akt.AuftraggeberName2;
                lblAuftraggeberName2_4.Text = akt.AuftraggeberName2;
                lblAuftraggeberStrasse.Text = akt.AuftraggeberStrasse;
                lblAuftraggeberLKZ.Text = akt.AuftraggeberLKZ;
                lblAuftraggeberPLZ.Text = akt.AuftraggeberPLZ;
                lblAuftraggeberOrt.Text = akt.AuftraggeberOrt;

                lblAuftraggeberStrasse_4.Text = akt.AuftraggeberStrasse;
                lblAuftraggeberLKZ_4.Text = akt.AuftraggeberLKZ;
                lblAuftraggeberPLZ_4.Text = akt.AuftraggeberPLZ;
                lblAuftraggeberOrt_4.Text = akt.AuftraggeberOrt;


                lblAktIntAZ.Text = akt.AktIntAZ + (akt.AKTIntDub == 1 ? "DUB!" : "");
                lblAktIntID.Text = akt.AktIntID + (akt.AKTIntDub == 1 ? "DUB!" : "");
                
                lblAktIntTerminAD.Text = akt.AktIntTerminAD.ToShortDateString();
                lblAktTypeINTCaption.Text = akt.AktTypeINTCaption;
                lblDate.Text = DateTime.Now.ToShortDateString();

                lblAKTIntAGSB.Text = akt.AKTIntAGSB;

                var sb = new StringBuilder();
                sb.Append(akt.KlientName1);
                sb.Append("<br/>");
                sb.Append(string.IsNullOrEmpty(akt.KlientName2) ? "": akt.KlientName2 + "<br/>");
                sb.Append(string.IsNullOrEmpty(akt.KlientName3) ? "" : akt.KlientName3 + "<br/>");
                sb.Append(string.IsNullOrEmpty(akt.KlientStrasse) ? "" : akt.KlientStrasse + "<br/>");
                sb.Append(akt.KlientLKZ);
                sb.Append("-");
                sb.Append(akt.KlientPLZ);
                sb.Append("&nbsp;");
                sb.Append(akt.KlientOrt);
                lblKlientInfo.Text = sb.ToString();

                sb.Clear();

                sb.Append("<a href=\"#\" onclick=\"MM_openBrWindow('/v2/intranetx/gegner/tablet/EditGegnerTablet.aspx?");
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.ID, akt.GegnerID.ToString(), false);
                sb.Append("','popWindow','')\">");
                sb.Append(akt.GegnerLastName1);
                sb.Append("</a>");
                
                sb.Append("<br/>");
                sb.Append(string.IsNullOrEmpty(akt.GegnerLastName2) ? "" : akt.GegnerLastName2 + "<br/>");
                sb.Append(string.IsNullOrEmpty(akt.GegnerLastName3) ? "" : akt.GegnerLastName3 + "<br/>");
                sb.Append(string.IsNullOrEmpty(akt.GegnerLastStrasse) ? "" : akt.GegnerLastStrasse + "<br/>");
                sb.Append(akt.GegnerLastZipPrefix);
                sb.Append("-");
                sb.Append(akt.GegnerLastZip);
                sb.Append("&nbsp;");
                sb.Append(akt.GegnerLastOrt);
                lblGegnerInfo.Text = sb.ToString();
                lblGegnerInfo_2.Text = lblGegnerInfo.Text;

                lblGegnerPhone.Text = !string.IsNullOrEmpty(akt.GegnerPhone) ? akt.GegnerPhoneCountry + " " + akt.GegnerPhoneCity + " " + akt.GegnerPhone : "unbekannt!";
                string memo = akt.AktIntOriginalMemo.Trim() + "<br/>" + akt.AKTIntMemo;
                lblAKTIntMemo.Text = memo.Length > 150 ? memo.Substring(0, 150) : memo;
                lblGegnerGebDat.Text = (akt.GegnerGebDat.ToShortDateString() != "" && akt.GegnerGebDat.ToShortDateString() != "01.01.1900") ? akt.GegnerGebDat.ToShortDateString() : "unbekannt!";
                txtMemo.Text = akt.AKTIntMemo;
                PopulateForderungGrid();


                ctlInstallment.SetAktIntId(_aktId);
                ctlInstallment.LoadAll();
                ctlInstallmentOld.SetAktIntId(_aktId);
                ctlInstallmentOld.LoadAll();

                if (_user.UserHasIPad)
                    sb.Append(lnkUploadPics.NavigateUrl = GetIPadUploadURL());
                else
                    lnkUploadPics.Visible = false;
            }
        }

        public void SetStatusPrinted(int aktId)
        {
            new RecordSet().ExecuteNonQuery("Update tblAktenInt Set AktIntDruckkennz = 1 WHERE AKTIntId = " + aktId);
        }

        private int GetPageIndex()
        {
            int pageIndex;
            try
            {
                pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            }
            catch
            {
                pageIndex = 0;
            }
            return pageIndex;
        }

        private void SetNavigationLinksVisible()
        {
            lnkPrevious.Visible = true;
            lnkNext.Visible = true;
            int pageIndex = GetPageIndex();
            if (pageIndex == 0)
                lnkPrevious.Visible = false;
            if (pageIndex + 1 >= _aktenList.Count)
                lnkNext.Visible = false;

        }

        public double GetCollectedAmount()
        {
            return GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text);
        }

        #region Forderungsaufstellung Grid
        private void PopulateForderungGrid()
        {
            DataTable dt = GetInvoicesDataTableStructure();
            DataRow dr;
            foreach (AktIntForderungPrintLine pf in Pa.ForderungList)
            {
                dr = dt.NewRow();
                dr["Description"] = pf.Text;
                dr["Amount"] = HTBUtils.FormatCurrency(pf.Amount, true);
                dt.Rows.Add(dr);
            }
            dr = dt.NewRow();
            dr["Description"] = "inkl. Bearbeitungsgebühren";
            dr["Amount"] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Description"] = "Zinsen";
            dr["Amount"] = HTBUtils.FormatCurrency(Pa.Zinsen, true);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Description"] = "Inkassokosten";
            dr["Amount"] = HTBUtils.FormatCurrency(Pa.InkassoKosten, true);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Description"] = "20% MWSt aus Kosten";
            dr["Amount"] = HTBUtils.FormatCurrency(Pa.MWS, true);
            dt.Rows.Add(dr);
            
            if (Pa.Zahlungen < 0)
            {
                dr = dt.NewRow();
                dr["Description"] = "Zahlungen";
                dr["Amount"] = HTBUtils.FormatCurrency(Pa.Zahlungen, true);
                dt.Rows.Add(dr);
            }

            dr = dt.NewRow();
            dr["Description"] = "<h2/>";
            dr["Amount"] = "<h2/>";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Description"] = "Forderung:";
            dr["Amount"] = HTBUtils.FormatCurrency(Pa.GetTotalLessWeggebuhr(), true);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Description"] = "+ Weggeb&uuml;hr:";
            dr["Amount"] = HTBUtils.FormatCurrency(Pa.Weggebuhr, true);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Description"] = "<h2/>";
            dr["Amount"] = "<h2/>";
            dt.Rows.Add(dr);
            
            gvFA.DataSource = dt;
            gvFA.DataBind();
        }

        private DataTable GetInvoicesDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            return dt;
        }
        #endregion

        #region Event Handlers
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            int pageIndex = GetPageIndex();
            pageIndex++;
            LoadPageData(pageIndex);
            hdnPageIndex.Value = pageIndex.ToString();
            SetNavigationLinksVisible();
        }

        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            int pageIndex = GetPageIndex();
            pageIndex--;
            LoadPageData(pageIndex);
            hdnPageIndex.Value = pageIndex.ToString();
            SetNavigationLinksVisible();
        }
        protected void tabContainerMain_ActiveTabChanged(object sender, EventArgs e)
        {
            _aktId = ((qryAktenInt)_aktenList[GetPageIndex()]).AktIntID;
            switch (tabContainerMain.ActiveTabIndex)
            {
                case 0:
                    break;
                case 1:
                    _akt = HTBUtils.GetInterventionAkt(_aktId);
                    lblAktIntID_Doc.Text = _akt.AktIntID.ToString();
                    lblAktIntAZ_Doc.Text = _akt.AktIntAZ;
                    PopulateDocumentsGrid();
                    break;
                case 2:
                    _akt = HTBUtils.GetInterventionAkt(_aktId);
                    lblAktIntID_Action.Text = _akt.AktIntID.ToString();
                    lblAktIntAZ_Action.Text = _akt.AktIntAZ;
                    txtMemo.Text = _akt.AKTIntMemo;
                    ctlInstallment.SetAktIntId(_aktId);
                    ctlInstallment.LoadAll();
                    ctlInstallmentOld.SetAktIntId(_aktId);
                    ctlInstallmentOld.LoadAll();
                    LoadActions();
                    break;
            }
        }

        protected void gvExistingActions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowAction")
            {
                try
                {
                    var lblActionID = (Label)gvExistingActions.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblActionID");
                    LoadActions(Convert.ToInt32(lblActionID.Text), true);

                }
                catch(Exception ex)
                {
                    ctlMessage.ShowException(ex);
                }
            }
        }
        #endregion

        #region New Action Methods
        private void SetValues()
        {
            // check to see if there are actions attached to this auftraggeber
            //double balance = GetBalance();
            GlobalUtilArea.LoadDropdownList(ddlAktion, GetActions(),
                    "ActionID",
                    "ActionCaption", true);


            if (!_isNewAction)
            {
                ddlAktion.SelectedValue = _aktAction.AktIntActionType.ToString();
                PopulateActionFields();
                txtBetrag.Text = HTBUtils.FormatCurrencyNumber(_aktAction.AktIntActionBetrag);
                txtBeleg.Text = _aktAction.AktIntActionBeleg;
                if (_aktAction.AktIntActionProvision > 0)
                    txtProvision.Text = HTBUtils.FormatCurrencyNumber(_aktAction.AktIntActionProvision);
                else if (_aktAction.AktIntActionHonorar > 0)
                    txtProvision.Text = HTBUtils.FormatCurrencyNumber(_aktAction.AktIntActionHonorar);

                lblProvision.Text = txtProvision.Text;
            }
        }

        private void PopulateActionFields()
        {
            try
            {
                tblAktenIntActionType actionType = GetSelectedActionType();
                if (actionType != null)
                {
                    if (actionType.AktIntActionTypeIsInstallment || actionType.AktIntActionIsExtensionRequest || actionType.AktIntActionIsTelAndEmailCollection)
                    {
                        trExtra.Visible = true;
                        trExtraBlank.Visible = true;
                        if (actionType.AktIntActionIsExtensionRequest)
                        {
                            ctlExtension.PopulateFields(_akt.AktIntID);
                            ctlExtension.Visible = true;
                        }
                        else if (actionType.AktIntActionIsTelAndEmailCollection)
                        {
                            ctlTelAndEmail.PopulateFields(_akt.AktIntGegner);
                            ctlTelAndEmail.Visible = true;
                        }
                        else // Installment
                        {
                            if (_akt.IsInkasso())
                            {
                                if (actionType.AktIntActionIsPersonalCollection)
                                    ctlInstallment.ShowPersonalCollection();
                                else
                                    ctlInstallment.ShowErlagschein();

                                ctlInstallment.Visible = true;
                            }
                            else
                            {
                                ctlInstallmentOld.Visible = true;
                            }
                        }
                    }
                    if (!actionType.AktIntActionIsWithCollection)
                    {
                        txtBetrag.Text = "";
                        txtBeleg.Text = "";
                        trBetrag.Visible = false;
                    }
                    if (actionType.AktIntActionIsTotalCollection)
                    {
                        txtBetrag.Text = HTBUtils.FormatCurrencyNumber(GetBalance());
                    }
                    double balance = GetBalance();
                    var provisionCalc = new ProvisionCalc();
//                    ProvisionCalc.GetPrice(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue));
                    txtProvision.Text = HTBUtils.FormatCurrencyNumber(provisionCalc.GetProvision(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue)));
                    lblProvision.Text = txtProvision.Text;
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowError(ex.Message + "<BR/>" + ex.StackTrace.Replace(" ", "<BR/>"));
            }
        }

        private ArrayList GetActions()
        {
            /* Try AG + Type First */
            string sqlQuery = "SELECT * FROM qryAuftraggeberAktTypeAction " +
                                " WHERE AGAktionTypeAuftraggeberID = " + _akt.AktIntAuftraggeber +
                                " AND AGAktionTypeAktTypeIntID = " + _akt.AktIntAktType +
                                " ORDER BY AktIntActionTypeCaption";
            ArrayList list = GetActions(sqlQuery, typeof(qryAuftraggeberAktTypeAction));
            if (list.Count > 0)
            {
                return list;
            }

            /* Than Try Just Type */
            sqlQuery = "SELECT * FROM qryAktTypeAction " +
                        " WHERE AktTypeActionAktTypeIntID = " + _akt.AktIntAktType +
                        " ORDER BY AktIntActionTypeCaption";
            list = GetActions(sqlQuery, typeof(qryAktTypeAction));
            if (list.Count > 0)
            {
                return list;
            }

            /* Than Try Just AG */
            sqlQuery = "SELECT * FROM qryAuftraggeberAction " +
                        " WHERE AGAktionAuftraggeberID = " + _akt.AktIntAuftraggeber +
                        " ORDER BY AktIntActionTypeCaption";
            list = GetActions(sqlQuery, typeof(qryAuftraggeberAction));
            if (list.Count > 0)
            {
                return list;
            }

            /*If All fails show default actions */
            sqlQuery = "SELECT * FROM tblAktenIntActionType WHERE AktIntActionIsDefault = 1 ORDER BY AktIntActionTypeCaption";
            return GetActions(sqlQuery, typeof(tblAktenIntActionType));
        }
        private ArrayList GetActions(string slqQuery, Type classType)
        {
            var resultsList = HTBUtils.GetSqlRecords(slqQuery, classType);
            var list = new ArrayList();

            if (resultsList.Count > 0)
            {
                foreach (Record action in resultsList)
                {
                    list.Add(new ActionRecord(action));
                }
                return GetUserActions(list);
            }
            return list;
        }

        private ArrayList GetUserActions(ArrayList actionsList)
        {
            ArrayList userActionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryUserAktionen WHERE UserAktionUserID = " + GlobalUtilArea.GetUserId(Session), typeof(qryUserAktionen));
            if (userActionsList.Count > 0)
            {
                var resultsList = new ArrayList();
                foreach (ActionRecord agAction in actionsList)
                {
                    foreach (qryUserAktionen userAction in userActionsList)
                    {
                        if (agAction.ActionID == userAction.UserAktionAktIntActionTypeID)
                        {
                            resultsList.Add(agAction);
                        }
                    }
                }
                return resultsList;
            }
            return actionsList;
        }
        
        #region Event Handlers
        protected void ddlAktion_SelectedIndexChanged(object sender, EventArgs e)
        {

            trExtra.Visible = false;
            ctlInstallment.Visible = false;
            ctlInstallmentOld.Visible = false;
            ctlExtension.Visible = false;
            ctlTelAndEmail.Visible = false;
            trExtraBlank.Visible = false;
            trBetrag.Visible = true;
            PopulateActionFields();
        }
        protected void txtBetrag_TextChanged(object sender, EventArgs e)
        {
            var provisionCalc = new ProvisionCalc();
            double balance = GetBalance();
//            txtProvision.Text = HTBUtils.FormatCurrencyNumber(GetProvision(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue)));
            txtProvision.Text = HTBUtils.FormatCurrencyNumber(provisionCalc.GetProvision(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue)));
            lblProvision.Text = txtProvision.Text;
            lblPrice.Text = HTBUtils.FormatCurrencyNumber(provisionCalc.GetPrice(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), balance, _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue)));
            if (ctlInstallment.Visible)
            {
                ctlInstallment.LoadAll();
                ctlInstallment.SetCollectedAmount(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text));
            }
            txtBeleg.Focus();
        }
        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            if (SaveAction())
            {
                if(!_user.UserHasIPad)
                    SetAktStatusAndCloseWindow();
                else
                    btnSaveAction.Visible = false;
            }
        }

        protected void btnSaveWithMissingBeleg_Click(object sender, EventArgs e)
        {
            if(SaveAction(true))
            {
                if (!_user.UserHasIPad)
                    SetAktStatusAndCloseWindow();
                else
                    btnSaveAction.Visible = false;
            }
        }
        #endregion

        private bool SaveAction(bool saveBelegError = false)
        {
            try
            {
                tblAktenIntActionType actionType = GetSelectedActionType();
                LoadActionFromScreen(actionType);
                bool ok = IsActionValid(_aktAction, actionType, saveBelegError);
                if (ok)
                {
                    if (ctlInstallment.Visible)
                    {
                        ok &= ctlInstallment.SaveInstallment();
                    }
                    else if (ctlInstallmentOld.Visible)
                    {
                        ok &= ctlInstallmentOld.SaveInstallment();
                    }
                    if (ctlTelAndEmail.Visible)
                    {
                        ok &= ctlTelAndEmail.Save(_akt.AktIntGegner);
                    }
                    if (ok)
                    {
                        if(!_user.UserHasIPad)
                            SetLastBelegUsed(_aktAction);
                        else if(actionType.AktIntActionIsWithCollection)
                        {
                            tblAktenIntReceipt receipt = CreateReceipt(actionType);
                            _aktAction.AktIntActionReceiptID = receipt.AktIntReceiptID;
                            lnkPrintReceipt.NavigateUrl = GetReceiptURL(receipt);
                            lnkPrintReceipt.Visible = true;
                        }
                        ok = _isNewAction ? RecordSet.Insert(_aktAction) : RecordSet.Update(_aktAction);
                    }
                    if (!ok)
                    {
                        ctlMessage.ShowError("ERROR: Bitte ECP Anrufen  :-(");
                    }
                }
                return ok;

            }
            catch (Exception e)
            {
                ctlMessage.ShowError(e.Message);
            }
            return false;
        }

        private void SetAktStatusAndCloseWindow(int status = 2, bool closeWindow = true)
        {
            try
            {
                // Set akt satus to 'Ubgegeben'

                new RecordSet().ExecuteNonQuery("UPDATE tblAktenInt SET AktIntStatus = " + status + ", AKTIntMemo = '" + txtMemo.Text.Replace("'", "''") + "' WHERE AktIntID = " + _aktId);
                btnSaveAction.Visible = false;
                ctlMessage.ShowSuccess("Akt ist abgeben!");
                if(closeWindow)
                    ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "closeScript", "MM_refreshParentAndClose();", true);
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        private void LoadActionFromScreen(tblAktenIntActionType actionType)
        {
            var provisionCalc = new ProvisionCalc();
            if (_isNewAction)
            {
                _aktAction = new tblAktenIntAction();
            }
            _aktAction.AktIntActionAkt = _akt.AktIntID;
            _aktAction.AktIntActionSB = _akt.AktIntSB;

            _aktAction.AktIntActionType = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue);
            _aktAction.AktIntActionProvision = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtProvision.Text);
            _aktAction.AktIntActionHonorar = 0; // no more honorar (everything gets calculated into provision)
            _aktAction.AktIntActionPrice = provisionCalc.GetPrice(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), GetBalance(), _akt.AktIntAuftraggeber, _akt.AktIntAktType, _akt.AktIntSB, GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktion.SelectedValue));
            _aktAction.AktIntActionProvAbzug = _ag.AuftraggeberIntAktPovAbzug;
            _aktAction.AktIntActionDate = DateTime.Now;
            _aktAction.AktIntActionTime = DateTime.Now;
            if (trBetrag.Visible)
            {
                _aktAction.AktIntActionBeleg = HTBUtils.RemoveAllSpecialChars(txtBeleg.Text, true);
                _aktAction.AktIntActionBetrag = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text);
            }
            _aktAction.AktIntActionMemo = ""; //TODO: insert memo field

            if (actionType != null && actionType.AktIntActionIsExtensionRequest)
            {
                _aktAction.AktIntActionAktIntExtID = ctlExtension.SaveExtensionRequest(_aktAction);
            }
        }

        private tblAktenIntActionType GetSelectedActionType()
        {
            return (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + ddlAktion.SelectedValue, typeof(tblAktenIntActionType));
        }
        private bool IsActionValid(tblAktenIntAction action, tblAktenIntActionType actionType, bool saveBelegError = false)
        {
            var sb = new StringBuilder();
            bool ok = true;
            if (action.AktIntActionType <= 0)
            {
                sb.Append("<i><strong>Aktion</strong></i> ausw&auml;hlen<br>");
                ddlAktion.BackColor = Color.Beige;
                ok = false;
            }
            if (actionType.AktIntActionIsWithCollection)
            {
                if (HTBUtils.IsZero(action.AktIntActionBetrag))
                {
                    sb.Append("<i><strong>Betrag</strong></i> eingeben<br>");
                    txtBetrag.BackColor = Color.Beige;
                    ok = false;
                }
                if (!_user.UserHasIPad)
                {
                    if (action.AktIntActionBeleg.Trim() == string.Empty)
                    {
                        sb.Append("<i><strong>Beleg</strong></i> eingeben<br>");
                        txtBeleg.BackColor = Color.Beige;
                        ok = false;
                    }
                    else // make sure Beleg is a number
                    {
                        if (_isNewAction)
                        {
                            double num;
                            bool isNum = Double.TryParse(action.AktIntActionBeleg, out num);
                            if (!isNum)
                            {
                                /*
                                string beleg = HTBUtils.RemoveAllSpecialChars(action.AktIntActionBeleg).Replace(" ", "").Trim().ToLower();
                                if (beleg != "zhlganikb" && beleg != "zahlunganikb")
                                {
                                    sb.Append("<i><strong>Beleg</strong></i> ist falsch [Beleg darf entwieder ein Beleg Nummer oder 'Zahlung an IKB' sein]<br>");
                                    txtBeleg.BackColor = Color.Beige;
                                    ok = false;
                                }
                                 */
                                sb.Append("<i><strong>Beleg</strong></i> ist falsch [Beleg darf nur ein Zahl sein]<br>");
                                ok = false;
                            }
                            else
                            {

                                if (saveBelegError)
                                    SaveBelegError((long) num, _akt.AktIntAuftraggeber, _akt.AktIntSB, sb);
                                else
                                    ok = ValidateBelegNumber((long) num, _akt.AktIntAuftraggeber, _akt.AktIntSB, sb);
                            }
                        }
                    }
                }
            }
            if (!ok)
                ctlMessage.ShowError(sb.ToString());
            return ok;
        }

        private bool ValidateBelegNumber(long belegNumber, int agId, int userId, StringBuilder sb)
        {
            bool ok = true;

            var block = GetKassaBlock(belegNumber, agId, userId);
            if (block == null)
            {
                ok = false;
                sb.Append("<i><strong>Belegsnummer</strong></i> ist nicht in die jetztige Blöcke gefunden<br>");
            }
            else
            {
                if (HTBUtils.IsZero(block.KassaBlockLastUsedNr))
                    block.KassaBlockLastUsedNr = block.KassaBlockNrVon - 1;

                if (!HTBUtils.IsZero(belegNumber - 1 - block.KassaBlockLastUsedNr)) // blok.KassaBlockLastUsedNr != belegNumber - 1
                {
                    ok = false;
                    if (block.KassaBlockLastUsedNr > (belegNumber - 1))
                    {
                        sb.Append("<i><strong>Belegsnummer</strong></i> wurde berreits verwendet<br>");
                    }
                    else
                    {
                        sb.Append("<i><strong>Folgende Belegsnummer(n) fehlen</strong></i>:<br>");
                        int count = 0;
                        for (var i = block.KassaBlockLastUsedNr + 1; i < belegNumber; i++)
                        {
                            if (count++ < 10)
                            {
                                sb.Append(i.ToString());
                                sb.Append("<br/>");
                            }
                        }
                        sb.Append("<br/>* Wenn Sie sicher sind, dass die Belegsnummer richtig ist, klicken sie den 'Trotzdem Speichern' Button.<br/>&nbsp;&nbsp;&nbsp;&nbsp;Die fehlenden Belege senden Sie bitte per Email an unser Office.<br/><br/>");
                        sb.Append("<b>* Wenn die fehlenden Belege innerhalb 7 Tagen nicht vorliegen, wird Ihr Account gesperrt!</b><br/>&nbsp;");
                    }
                }
                if (ok)
                {
                    block.KassaBlockLastUsedNr = belegNumber;
                    RecordSet.Update(block);
                }
            }
            if (!ok)
            {
                trSaveWithMissingBeleg.Visible = true;
            }
            return ok;
        }

        private void SaveBelegError(long belegNumber, int agId, int userId, StringBuilder sb)
        {
            var err = new tblKassaBlockError();
            var block = GetKassaBlock(belegNumber, agId, userId);
            var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + userId, typeof(tblUser));

            err.KassaBlockErrDate = DateTime.Now;
            err.KassaBlockErrUser = userId;
            err.KassaBlockErrBeleg = (double)belegNumber;
            err.KassaBlockErrAG = agId;

            bool ok = true;
            if (user != null)
            {
                sb.Append(user.UserVorname + " " + user.UserNachname + ": ");
            }
            if (block == null)
            {
                sb.Append("Belegsnummer [" + belegNumber + "] ist nicht in die jetztige Blöcke gefunden");
                ok = false;
            }
            else
            {
                if (HTBUtils.IsZero(block.KassaBlockLastUsedNr))
                    block.KassaBlockLastUsedNr = block.KassaBlockNrVon - 1;

                if (!HTBUtils.IsZero(belegNumber - 1 - block.KassaBlockLastUsedNr)) // blok.KassaBlockLastUsedNr != belegNumber - 1
                {
                    if (block.KassaBlockLastUsedNr > belegNumber)
                    {
                        sb.Append("Belegsnummer wurde berreits verwendet. Letzte belegnumber: " + block.KassaBlockLastUsedNr);
                        ok = false;
                    }
                    else
                    {
                        //Template missing Beleg record
                        var missTemplate = new tblKassaBlockMissingNr
                        {
                            KbMissBlockID = block.KassaBlockID,
                            KbMissUser = userId,
                            KbMissDate = DateTime.Now,
                            KbMissReceivedDate = HTBUtils.DefaultDate
                        };

                        sb.Append("Folgende Belegsnummer(n) fehlen: [");
                        int count = 0;
                        for (var i = block.KassaBlockLastUsedNr + 1; i < belegNumber; i++)
                        {
                            if (count++ < 10)
                            {
                                sb.Append(i.ToString());
                                sb.Append(" ");
                                ok = false;
                            }
                            //Create missing Beleg record
                            try
                            {
                                RecordSet.Insert(new tblKassaBlockMissingNr(missTemplate) { KbMissNr = (int)i });
                            }
                            catch
                            {
                            }
                        }
                        sb.Append("]");
                    }
                }
            }
            sb.Append(" ");
            sb.Append(_user.UserVorname);
            sb.Append(" ");
            sb.Append(_user.UserNachname);
            err.KassaBlockErrMessage = sb.ToString();
            RecordSet.Insert(err);
            if (!ok)
                ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(new string[] { HTBUtils.GetConfigValue("Default_EMail_Addr"), HTBUtils.GetConfigValue("Office_Email"), "b.bitri@ecp.or.at" },
                                            _user.UserVorname + " " + _user.UserNachname + ":  Belegsnummer Fehler ",
                                            sb.ToString());

        }

        private void SetLastBelegUsed(tblAktenIntAction action)
        {
            if (txtBetrag.Visible && action.AktIntActionBeleg != null)
            {
                double num;
                bool isNum = Double.TryParse(HTBUtils.RemoveAllSpecialChars(action.AktIntActionBeleg, true), out num);
                if (isNum)
                {
                    var belegNumber = (long)num;
                    var block = GetKassaBlock(belegNumber, _akt.AktIntAuftraggeber, _akt.AktIntSB);
                    if (block != null)
                    {
                        block.KassaBlockLastUsedNr = belegNumber;
                        RecordSet.Update(block);
                    }
                }
            }
        }

        private tblKassablock GetKassaBlock(long belegNumber, int agId, int userId)
        {
            var sql = new StringBuilder("SELECT * FROM tblKassablock WHERE (KassaBlockDatumErhalten IS NULL OR KassaBlockDatumErhalten = '01.01.1900') AND KassaBlockNrVon <= ");
            sql.Append(belegNumber);
            sql.Append(" AND KassaBlockNrBis >= ");
            sql.Append(belegNumber);
            sql.Append(" AND KassaBlockUser = ");
            sql.Append(userId);
            sql.Append(" AND KassaBlockAuftraggeber = ");
            sql.Append(agId);
            return (tblKassablock)HTBUtils.GetSqlSingleRecord(sql.ToString(), typeof(tblKassablock));
        }

        private bool IsActionVoid()
        {
            return GetSelectedActionType().AktIntActionIsVoid;
        }

        #region Messages
        private void ClearErrors()
        {
            ctlMessage.Clear();
            ddlAktion.BackColor = Color.White;
            txtBetrag.BackColor = Color.White;
        }
        #endregion

        #region Calculations
        private double GetBalance()
        {
            double balance = 0;
            if (_akt != null)
            {
                if (_akt.IsInkasso())
                {
                    var aktUtils = new AktUtils(_akt.AktIntCustInkAktID);
                    balance = aktUtils.GetAktBalance();
                }
                else
                {
                    ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + _akt.AktIntID, typeof(tblAktenIntPos));
                    foreach (tblAktenIntPos AktIntPos in posList)
                    {
                        balance += AktIntPos.AktIntPosBetrag;
                    }
                    balance += _akt.AKTIntZinsenBetrag;
                    balance += _akt.AKTIntKosten;
                    balance += _akt.AktIntWeggebuehr;
                }
            }
            return balance;
        }
        #endregion

        #endregion

        #region Existing Actions Grid

        private void PopulateExistingActionsGrid()
        {
            ArrayList actionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + _aktId + " ORDER BY AktIntActionDate DESC", typeof(qryInktAktAction));
            DataTable dt = GetExistingActionsDataTableStructure();
            trActionsGrid.Visible = actionsList.Count > 0;

            foreach (qryInktAktAction rec in actionsList)
            {
                DataRow dr = dt.NewRow();
                var sb =
                    new StringBuilder(
                        "<a href=\"#\"><img src=\"/v2/intranet/images/delete.gif\" alt=\"Löscht diese Aktion\" width=\"40\" height=\"40\" border=\"0\" onclick=\"MM_openBrWindow('/v2/intranet/global_forms/globaldelete.asp?strTable=tblAktenIntAction&frage=Sind%20Sie%20sicher,%20dass%20sie%20diese%20T&#228;tigkeit%20l&#246;schen%20wollen?&strTextField=AktIntActionID&strColumn=AktIntActionID&ID=");
                sb.Append(rec.AktIntActionID);
                sb.Append("','popWindow','menubar=yes,scrollbars=yes,resizable=yes,width=300,height=300')\"></a>");
                

                dr["DeleteUrl"] = sb.ToString();

                dr["ActionID"] = rec.AktIntActionID.ToString();
                dr["ActionDate"] = rec.AktIntActionDate.ToShortDateString() + " " + rec.AktIntActionTime.ToShortTimeString();
                dr["ActionCaption"] = rec.AktIntActionTypeCaption;
                if(rec.AktIntActionReceiptID > 0)
                {
                    var receipt = (tblAktenIntReceipt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntReceipt WHERE AktIntReceiptID = " + rec.AktIntActionReceiptID, typeof(tblAktenIntReceipt));
                    if(receipt != null)
                    {
                        sb.Clear();
                        sb.Append("<a href=\"");
                        sb.Append(GetReceiptURL(receipt));
                        sb.Append("\">Beleg Drucken</a>");
                        dr["PrintReceiptUrl"] = sb.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            gvExistingActions.DataSource = dt;
            gvExistingActions.DataBind();
        }
        private DataTable GetExistingActionsDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("ActionID", typeof(string)));
            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("PrintReceiptUrl", typeof(string)));
            return dt;
        }
        #endregion

        #region Documents
        private void PopulateDocumentsGrid()
        {
            ArrayList docsList = HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + _aktId, typeof(qryDoksIntAkten));
            if(_akt.IsInkasso())
                HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksInkAkten WHERE CustInkAktID = " + _akt.AktIntCustInkAktID, typeof(qryDoksInkAkten)), docsList);
            
            DataTable dt = GetDocumentsDataTableStructure();
            foreach (Record rec in docsList)
            {
                if (rec is qryDoksIntAkten)
                {
                    DataRow dr = dt.NewRow();
                    var doc = (qryDoksIntAkten) rec;

                    dr["DokCreationTimeStamp"] = doc.DokCreationTimeStamp + " von " + doc.UserVorname + " " + doc.UserNachname;
                    dr["DokTypeCaption"] = doc.DokTypeCaption;
                    dr["DokCaption"] = doc.DokCaption;
                    dr["DokAttachment"] = doc.DokAttachment;
                    dt.Rows.Add(dr);
                }
                else if (rec is qryDoksInkAkten)
                {
                    DataRow dr = dt.NewRow();
                    var doc = (qryDoksInkAkten) rec;

                    dr["DokCreationTimeStamp"] = doc.DokCreationTimeStamp + " von " + doc.UserVorname + " " + doc.UserNachname;
                    dr["DokTypeCaption"] = doc.DokTypeCaption;
                    dr["DokCaption"] = doc.DokCaption;
                    dr["DokAttachment"] = doc.DokAttachment;
                    dt.Rows.Add(dr);
                }
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

        #region IPad Upload Link
        private String GetIPadUploadURL()
        {
            var sb = new StringBuilder(_appName);
            sb.Append("://?UploadPics=yes");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AktID", _akt.AktIntID.ToString());
            return sb.ToString();
        }
        #endregion

        #region Receipt
        public String GetReceiptURL(tblAktenIntReceipt receipt)
        {   
            var ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + receipt.AktIntReceiptAuftraggeber, typeof(tblAuftraggeber));
            var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT UserSex, UserVorname, UserNachname FROM tblUser WHERE UserID = " + receipt.AktIntReceiptUser, typeof(tblUser));
            var sb = new StringBuilder(_appName);
            sb.Append("://?");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "ID", receipt.AktIntReceiptID.ToString());
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "Date", receipt.AktIntReceiptDate.ToShortDateString()+" "+receipt.AktIntReceiptDate.ToShortTimeString());
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "City", receipt.AktIntReceiptCity);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AktID", receipt.AktIntReceiptAkt.ToString());
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AktAZ", _akt.AktIntAZ);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AgName", ag.AuftraggeberName1 + " "+ag.AuftraggeberName2);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AgZipCity", ag.AuftraggeberPLZ +" "+ag.AuftraggeberOrt);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AgStreet", ag.AuftraggeberStrasse);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AgTel", (ag.AuftraggeberPhoneCity.StartsWith("0") ? ag.AuftraggeberPhoneCity : "0" + ag.AuftraggeberPhoneCity) +" "+ag.AuftraggeberPhone);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AgEmail", ag.AuftraggeberEMail);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AgWeb", ag.AuftraggeberWeb);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AgSb", _akt.AKTIntAGSB);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AgSbEmail", _akt.AKTIntKSVEMail);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "Collector", (user.UserSex == 1 ? "Herr " : "Frau ") + user.UserVorname + " " + user.UserNachname);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "PaymentType", receipt.AktIntReceiptType == 1 ? "Gesamtinkasso" : "Teilzahlung");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "PaymentAmount", HTBUtils.FormatCurrencyNumber(receipt.AktIntReceiptAmount));
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "PaymentTax", "0,00");
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "PaymentTotal", HTBUtils.FormatCurrencyNumber(receipt.AktIntReceiptAmount));

            return sb.ToString();
        }
        public tblAktenIntReceipt CreateReceipt(tblAktenIntActionType actionType)
        {
            var gegner = (tblGegner) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerOldID = " + _akt.AktIntGegner, typeof(tblGegner));
            var receipt = new tblAktenIntReceipt
                              {
                                  AktIntReceiptDate = DateTime.Now, 
                                  AktIntReceiptUser = _user.UserID, 
                                  AktIntReceiptAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtBetrag.Text), 
                                  AktIntReceiptAkt = _akt.AktIntID,
                                  AktIntReceiptCity = gegner.GegnerLastOrt,
                                  AktIntReceiptType = actionType.AktIntActionIsTotalCollection ? 1 : 2,
                                  AktIntReceiptAuftraggeber = _akt.AktIntAuftraggeber
                              };
            if(RecordSet.Insert(receipt))
            {
                return (tblAktenIntReceipt)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblAktenIntReceipt ORDER BY AktIntReceiptID DESC", typeof (tblAktenIntReceipt));
            }
            return null;
        }
        #endregion
    }
}