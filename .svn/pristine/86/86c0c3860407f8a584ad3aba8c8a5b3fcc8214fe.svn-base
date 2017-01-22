using System;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.permissions;
using HTB.v2.intranetx.util;
using HTB.Database;
using HTBUtilities;
using System.Collections;
using System.Data;
using HTB.Database.Views;
using HTBExtras;

namespace HTB.v2.intranetx.aktenintprovfix
{
    public partial class ActionProv : System.Web.UI.Page
    {
        const string FILTER_AG_TYPE_USER = "FILTER_AG_TYPE_USER";
        const string FILTER_USER_TYPE = "FILTER_USER_TYPE"; 
        const string FILTER_AG_USER = "FILTER_AG_USER";
        const string FILTER_AG_TYPE = "FILTER_AG_TYPE";
        const string FILTER_USER = "FILTER_USER";
        const string FILTER_AG = "FILTER_AG";
        const string FILTER_NONE = "FILTER_NONE";

        PermissionsProvision permission = new PermissionsProvision();
        string filter;

        protected void Page_Load(object sender, EventArgs e)
        {
            permission.LoadPermissions(GlobalUtilArea.GetUserId(Session));

            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlAuftraggeber,
                    "SELECT * FROM tblAuftraggeber ORDER BY AuftraggeberName1 ASC",
                    typeof(tblAuftraggeber),
                    "AuftraggeberID",
                    "AuftraggeberName1", false);
                GlobalUtilArea.LoadDropdownList(ddlAktType,
                        "SELECT * FROM tblAktTypeINT ORDER BY AktTypeINTCaption ASC",
                        typeof(tblAktTypeInt),
                        "AktTypeINTID",
                        "AktTypeINTCaption", false);

                ddlAuftraggeber.Items.Insert(0, new ListItem("Alle Auftraggeber", GlobalUtilArea.DefaultDropdownValue));
                ddlAktType.Items.Insert(0, new ListItem("Alle Akttypen", GlobalUtilArea.DefaultDropdownValue));

                ReloadUsers();
                ShowActions();
            }
            SetDefaultValues();
            SetDropdownVisible();
        }

        #region Event Handlers
        protected void ddlAuftraggeber_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadUsers();
            ShowActions();
        }

        protected void ddlAktType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadUsers();
            ShowActions();
        }

        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadUsers();
            ShowActions();
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            RecordSet set = new RecordSet();
            for (int i = 0; i < gvActionProv.Rows.Count; i++)
            {
                GridViewRow row = gvActionProv.Rows[i];
                Label lblActionId = (Label)row.FindControl("lblActionID");
                TextBox txtPrice = (TextBox)row.FindControl("txtPrice");
                TextBox txtProvAmount = (TextBox)row.FindControl("txtProvAmount");
                TextBox txtProvAmountForZero = (TextBox)row.FindControl("txtProvAmountForZero");
                TextBox txtProvPct = (TextBox)row.FindControl("txtProvPct");
                DropDownList ddlHonorarGroup = (DropDownList)row.FindControl("ddlHonorarGroup");
                SaveProvision(
                    GlobalUtilArea.GetZeroIfConvertToIntError(lblActionId.Text),
                    GlobalUtilArea.GetNegOneIfConvertToDoubleError(txtPrice.Text),
                    GlobalUtilArea.GetNegOneIfConvertToDoubleError(txtProvAmount.Text),
                    GlobalUtilArea.GetNegOneIfConvertToDoubleError(txtProvAmountForZero.Text),
                    GlobalUtilArea.GetNegOneIfConvertToDoubleError(txtProvPct.Text),
                    GlobalUtilArea.GetZeroIfConvertToIntError(ddlHonorarGroup.SelectedValue),
                    set);
            }
            ShowActions();
            ReloadUsers();
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        
        protected void btnShowActions_Click(object sender, EventArgs e)
        {
            panActionProv.Visible = false;
            panActions.Visible = true;
            btnSubmit.Visible = false;
            btnCancel.Visible = false;
            btnAddSelectedProv.Visible = true;
            btnCancelSelectedProv.Visible = true;
            PopulateActionsGrid();
            ReloadUsers();
        }
        
        protected void btnAddSelectedProv_Click(object sender, EventArgs e)
        {
            
            ShowActions();

            var list = new ArrayList();

            for (int i = 0; i < gvActions.Rows.Count; i++)
            {
                GridViewRow row = gvActions.Rows[i];
                Label lblActionId = (Label)row.Cells[0].FindControl("lblActionID");
                CheckBox chkSelected = (CheckBox)row.Cells[1].FindControl("chkSelected");
                Label lblActionCaption = (Label)row.Cells[2].FindControl("lblActionCaption");
                if (chkSelected.Checked)
                {
                    bool found = false;
                    for (int j = 0; j < gvActionProv.Rows.Count; j++)
                    {
                        GridViewRow provRow = gvActionProv.Rows[j];
                        Label lblProvActionId = (Label)provRow.Cells[0].FindControl("lblActionID");
                        if (lblProvActionId.Text == lblActionId.Text)
                        {
                            found = true;
                            j = gvActionProv.Rows.Count;
                        }
                    }
                    if (!found)
                    {
                        ProvisionRecord rec = new ProvisionRecord();
                        rec.ActionID = GlobalUtilArea.GetZeroIfConvertToIntError(lblActionId.Text);
                        rec.ActionCaption = lblActionCaption.Text;
                        list.Add(rec);
                    }
                }
            }
            PopulateProvActionsGrid(list);
            panActionProv.Visible = true;
            panActions.Visible = false;
            btnSubmit.Visible = true;
            btnCancel.Visible = true;
            btnAddSelectedProv.Visible = false;
            btnCancelSelectedProv.Visible = false;
            ReloadUsers();
        }
        
        protected void btnCancelSelectedProv_Click(object sender, EventArgs e)
        {
            ShowActions();
            panActionProv.Visible = true;
            panActions.Visible = false;
            btnSubmit.Visible = true;
            btnCancel.Visible = true;
            btnAddSelectedProv.Visible = false;
            btnCancelSelectedProv.Visible = false;
            ReloadUsers();
        }
        protected void gvActions_DataBound(object sender, EventArgs e)
        {
            //Each time the data is bound to the grid we need to build up the CheckBoxIDs array 
            //Get the header CheckBox
            if (gvActions.HeaderRow != null)
            {
                CheckBox cbHeader = (CheckBox)gvActions.HeaderRow.FindControl("chkActionHeader");

                //Run the ChangeCheckBoxState client-side function whenever the header checkbox is checked/unchecked
                cbHeader.Attributes.Add("onclick", "ChangeAllActionCheckBoxStates(this.checked);");

                foreach (GridViewRow gvr in gvActions.Rows)
                {
                    //Get a programmatic reference to the CheckBox control
                    CheckBox cb = (CheckBox)gvr.FindControl("chkSelected");

                    //Add the CheckBox's ID to the client-side ActionCheckBoxIDs array
                    ClientScript.RegisterArrayDeclaration("ActionCheckBoxIDs", String.Concat("'", cb.ClientID, "'"));
                }
            }
        }
        protected void gvActionProv_DataBound(object sender, EventArgs e)
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntHonorarGroup ORDER BY AktIntHonGrpCaption", typeof(tblAktenIntHonorarGroup));
            foreach (GridViewRow gvr in gvActionProv.Rows)
            {
                Label lblHonorarGrpId = (Label)gvr.FindControl("lblHonorarGrpId");
                DropDownList ddlHonorarGroup = (DropDownList)gvr.FindControl("ddlHonorarGroup");
                GlobalUtilArea.LoadDropdownList(
                    ddlHonorarGroup,
                    list,
                    "AktIntHonGrpID",
                    "AktIntHonGrpCaption",
                    false
                );
                ddlHonorarGroup.Items.Insert(0, new ListItem("", "0"));
                ddlHonorarGroup.SelectedValue = lblHonorarGrpId.Text;
            }
        }
        #endregion

        private void ShowActions()
        {
            string sqlQuery;
            SetFilter();
            ArrayList list;
            ArrayList resultsList;
            switch (filter)
            {
                #region AG + TYPE + USER
                case FILTER_AG_TYPE_USER:

                    sqlQuery = "SELECT * FROM qryAuftraggeberAktTypeActionUserProv " +
                                " WHERE AGAktTypeAktionUserProvAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " AND AGAktTypeActionUserProvAktTypeIntID = " + ddlAktType.SelectedValue +
                                " AND AGAktTypeActionUserProvUserID = " + ddlUser.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryAuftraggeberAktTypeActionUserProv));
                    list = new ArrayList();
                    foreach (qryAuftraggeberAktTypeActionUserProv prov in resultsList)
                    {
                        ProvisionRecord rec = new ProvisionRecord();
                        rec.ActionID = prov.AGAktTypeActionUserProvAktAktionTypeID;
                        rec.ActionCaption = prov.AktIntActionTypeCaption;
                        rec.Price = prov.AGAktTypeActionUserProvPrice;
                        rec.ProvAmount = prov.AGAktTypeActionUserProvAmount;
                        rec.ProvAmountForZero = prov.AGAktTypeActionUserProvAmountForZeroCollection;
                        rec.ProvPct = prov.AGAktTypeActionUserProvPct;
                        rec.HonorarGrpId = prov.AGAktTypeActionUserProvHonGrpID;
                        
                        list.Add(rec);
                    }
                    PopulateProvActionsGrid(list);
                    break;
                #endregion
                #region AG + TYPE
                case FILTER_AG_TYPE:

                    sqlQuery = "SELECT * FROM qryAuftraggeberAktTypeActionProv " +
                                " WHERE AGAktTypeAktionProvAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " AND AGAktTypeActionProvAktTypeIntID = " + ddlAktType.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryAuftraggeberAktTypeActionProv));
                    list = new ArrayList();
                    foreach (qryAuftraggeberAktTypeActionProv prov in resultsList)
                    {
                        ProvisionRecord rec = new ProvisionRecord();
                        rec.ActionID = prov.AGAktTypeActionProvAktAktionTypeID;
                        rec.ActionCaption = prov.AktIntActionTypeCaption;
                        rec.Price = prov.AGAktTypeActionProvPrice;
                        rec.ProvAmount = prov.AGAktTypeActionProvAmount;
                        rec.ProvAmountForZero = prov.AGAktTypeActionProvAmountForZeroCollection;
                        rec.ProvPct = prov.AGAktTypeActionProvPct;
                        rec.HonorarGrpId = prov.AGAktTypeActionProvHonGrpID;
                        list.Add(rec);
                    }
                    PopulateProvActionsGrid(list);
                    break;
                #endregion
                #region AG + USER
                case FILTER_AG_USER:

                    sqlQuery = "SELECT * FROM qryAuftraggeberUserProv " +
                                " WHERE AGUserProvAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " AND AGUserProvUserID = " + ddlUser.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryAuftraggeberUserProv));
                    list = new ArrayList();
                    foreach (qryAuftraggeberUserProv prov in resultsList)
                    {
                        ProvisionRecord rec = new ProvisionRecord();
                        rec.ActionID = prov.AGUserProvAktAktionTypeID;
                        rec.ActionCaption = prov.AktIntActionTypeCaption;
                        rec.Price = prov.AGUserProvPrice;
                        rec.ProvAmount = prov.AGUserProvAmount;
                        rec.ProvAmountForZero = prov.AGUserProvAmountForZeroCollection;
                        rec.ProvPct = prov.AGUserProvPct;
                        rec.HonorarGrpId = prov.AGUserProvHonGrpID;
                        list.Add(rec);
                    }
                    PopulateProvActionsGrid(list);
                    break;
                #endregion
                #region AG
                case FILTER_AG:

                    sqlQuery = "SELECT * FROM qryAuftraggeberActionProv " +
                                " WHERE AGAktionProvAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryAuftraggeberActionProv));
                    list = new ArrayList();
                    foreach (qryAuftraggeberActionProv prov in resultsList)
                    {
                        ProvisionRecord rec = new ProvisionRecord();
                        rec.ActionID = prov.AGActionProvAktAktionTypeID;
                        rec.ActionCaption = prov.AktIntActionTypeCaption;
                        rec.Price = prov.AGActionProvPrice;
                        rec.ProvAmount = prov.AGActionProvAmount;
                        rec.ProvAmountForZero = prov.AGActionProvAmountForZeroCollection;
                        rec.ProvPct = prov.AGActionProvPct;
                        rec.HonorarGrpId = prov.AGActionProvHonGrpID;
                        list.Add(rec);
                    }
                    PopulateProvActionsGrid(list);
                    break;
                #endregion
                #region USER + TYPE
                case FILTER_USER_TYPE:

                    sqlQuery = "SELECT * FROM qryUserAktTypeActionProv " +
                                " WHERE UserAktTypeAktionProvUserID = " + ddlUser.SelectedValue +
                                " AND UserAktTypeActionProvAktTypeIntID = " + ddlAktType.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryUserAktTypeActionProv));
                    list = new ArrayList();
                    foreach (qryUserAktTypeActionProv prov in resultsList)
                    {
                        ProvisionRecord rec = new ProvisionRecord();
                        rec.ActionID = prov.UserAktTypeActionProvAktAktionTypeID;
                        rec.ActionCaption = prov.AktIntActionTypeCaption;
                        rec.Price = prov.UserAktTypeActionProvPrice;
                        rec.ProvAmount = prov.UserAktTypeActionProvAmount;
                        rec.ProvAmountForZero = prov.UserAktTypeActionProvAmountForZeroCollection;
                        rec.ProvPct = prov.UserAktTypeActionProvPct;
                        rec.HonorarGrpId = prov.UserAktTypeActionProvHonGrpID;
                        list.Add(rec);
                    }
                    PopulateProvActionsGrid(list);
                    break;
                #endregion
                #region USER
                case FILTER_USER:

                    sqlQuery = "SELECT * FROM qryUserActionProv " +
                                " WHERE UserAktionProvUserID = " + ddlUser.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryUserActionProv));
                    list = new ArrayList();
                    foreach (qryUserActionProv prov in resultsList)
                    {
                        ProvisionRecord rec = new ProvisionRecord();
                        rec.ActionID = prov.UserActionProvAktAktionTypeID;
                        rec.ActionCaption = prov.AktIntActionTypeCaption;
                        rec.Price = prov.UserActionProvPrice;
                        rec.ProvAmount = prov.UserActionProvAmount;
                        rec.ProvAmountForZero = prov.UserActionProvAmountForZeroCollection;
                        rec.ProvPct = prov.UserActionProvPct;
                        rec.HonorarGrpId = prov.UserActionProvHonGrpID;
                        list.Add(rec);
                    }
                    PopulateProvActionsGrid(list);
                    break;
                #endregion
                #region NO FILTER
                case FILTER_NONE:

                    sqlQuery = "SELECT * FROM tblAktenIntActionType ORDER BY AktIntActionTypeCaption";

                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(tblAktenIntActionType));
                    list = new ArrayList();
                    foreach (tblAktenIntActionType prov in resultsList)
                    {
                        ProvisionRecord rec = new ProvisionRecord();
                        rec.ActionID = prov.AktIntActionTypeID;
                        rec.ActionCaption = prov.AktIntActionTypeCaption;
                        rec.Price = prov.AktIntActionProvPrice;
                        rec.ProvAmount = prov.AktIntActionProvAmount;
                        rec.ProvAmountForZero = prov.AktIntActionProvAmountForZeroCollection;
                        rec.ProvPct = prov.AktIntActionProvPct;
                        rec.HonorarGrpId = prov.AktIntActionProvHonGrpID;
                        list.Add(rec);
                    }
                    PopulateProvActionsGrid(list);
                    break;
                default:
                    PopulateProvActionsGrid(new ArrayList());
                    break;
                #endregion
            }
        }

        private void ReloadUsers()
        {
            string selValue = "";
            if (ddlUser.Items.Count > 0)
            {
                selValue = ddlUser.SelectedValue;
            }
            GlobalUtilArea.LoadUserDropdownList(ddlUser, GlobalUtilArea.GetUsers(Session));
            ddlUser.Items.Insert(0, new ListItem("Alle Benutzer", GlobalUtilArea.DefaultDropdownValue));
            if (selValue != "")
            {
                ddlUser.SelectedValue = selValue;
            }
        }

        private void SetFilter()
        {
            if (ddlAuftraggeber.SelectedValue != GlobalUtilArea.DefaultDropdownValue && ddlAktType.SelectedValue != GlobalUtilArea.DefaultDropdownValue && ddlUser.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                filter = FILTER_AG_TYPE_USER;
            }
            else if (ddlAuftraggeber.SelectedValue != GlobalUtilArea.DefaultDropdownValue && ddlAktType.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                filter = FILTER_AG_TYPE;
            }
            else if (ddlAuftraggeber.SelectedValue != GlobalUtilArea.DefaultDropdownValue && ddlUser.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                filter = FILTER_AG_USER;
            }
            else if (ddlAuftraggeber.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                filter = FILTER_AG;
            }
            else if (ddlAuftraggeber.SelectedValue == GlobalUtilArea.DefaultDropdownValue && ddlAktType.SelectedValue != GlobalUtilArea.DefaultDropdownValue && ddlUser.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                filter = FILTER_USER_TYPE;
            }
            else if (ddlAuftraggeber.SelectedValue == GlobalUtilArea.DefaultDropdownValue && ddlAktType.SelectedValue == GlobalUtilArea.DefaultDropdownValue && ddlUser.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                filter = FILTER_USER;
            }
            else
            {
                filter = FILTER_NONE;
            }
            lblMessage.Text = filter;
        }

        private void SetDefaultValues()
        {
            panActionProv.Visible = true;
            panActions.Visible = false;
            btnSubmit.Visible = true;
            btnCancel.Visible = true;
            btnAddSelectedProv.Visible = false;
            btnCancelSelectedProv.Visible = false;
            lblRowCount.Text = "";
        }
        
        private void SetDropdownVisible()
        {
            ddlAktType.Visible = true;
            ddlUser.Visible = true;
            if (ddlAuftraggeber.SelectedValue == GlobalUtilArea.DefaultDropdownValue)
            {
                //ddlAktType.Visible = false;
                //ddlUser.Visible = false;
                //lblAktType.Visible = false;
                //lblUser.Visible = false;
            }
            else if (ddlAktType.SelectedValue == GlobalUtilArea.DefaultDropdownValue)
            {
                //ddlUser.Visible = false;
                //lblUser.Visible = false;
            }
            
        }

        private void SaveProvision(int actionId, double price, double provAmt, double provAmtForZero, double provPct, int honorarGrpId, RecordSet set)
        {
            SetFilter();
            string sqlWhere;
            switch (filter)
            {
                #region AG + TYPE + USER
                case FILTER_AG_TYPE_USER:

                    sqlWhere = " WHERE AGAktTypeAktionUserProvAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " AND AGAktTypeActionUserProvAktTypeIntID = " + ddlAktType.SelectedValue +
                                " AND AGAktTypeActionUserProvAktAktionTypeID = " + actionId +
                                " AND AGAktTypeActionUserProvUserID = " + ddlUser.SelectedValue;

                    set.ExecuteNonQuery("DELETE FROM tblAuftraggeberAktTypeActionUserProv " + sqlWhere);

                    if (provAmt != -1 || provAmtForZero != -1 || provPct != -1)
                    {
                        var rec = new tblAuftraggeberAktTypeActionUserProv
                                      {
                                          AGAktTypeAktionUserProvAuftraggeberID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAuftraggeber.SelectedValue),
                                          AGAktTypeActionUserProvAktTypeIntID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktType.SelectedValue),
                                          AGAktTypeActionUserProvUserID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlUser.SelectedValue),
                                          AGAktTypeActionUserProvAktAktionTypeID = actionId,
                                          AGAktTypeActionUserProvPrice = price,
                                          AGAktTypeActionUserProvAmount = provAmt == -1 ? 0 : provAmt,
                                          AGAktTypeActionUserProvAmountForZeroCollection = provAmtForZero == -1 ? 0 : provAmtForZero,
                                          AGAktTypeActionUserProvPct = provPct == -1 ? 0 : provPct,
                                          AGAktTypeActionUserProvHonGrpID = honorarGrpId
                                      };



                        set.InsertRecord(rec);
                    }
                    break;
                #endregion
                #region AG + TYPE
                case FILTER_AG_TYPE:

                    sqlWhere = " WHERE AGAktTypeAktionProvAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " AND AGAktTypeActionProvAktTypeIntID = " + ddlAktType.SelectedValue +
                                " AND AGAktTypeActionProvAktAktionTypeID = " + actionId;

                    set.ExecuteNonQuery("DELETE FROM tblAuftraggeberAktTypeActionProv " + sqlWhere);

                    if (provAmt != -1 || provAmtForZero != -1 || provPct != -1)
                    {
                        tblAuftraggeberAktTypeActionProv rec = new tblAuftraggeberAktTypeActionProv();

                        rec.AGAktTypeAktionProvAuftraggeberID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAuftraggeber.SelectedValue);
                        rec.AGAktTypeActionProvAktTypeIntID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktType.SelectedValue);
                        rec.AGAktTypeActionProvAktAktionTypeID = actionId;

                        rec.AGAktTypeActionProvPrice = price;
                        rec.AGAktTypeActionProvAmount = provAmt == -1 ? 0 : provAmt;
                        rec.AGAktTypeActionProvAmountForZeroCollection = provAmtForZero == -1 ? 0 : provAmtForZero;
                        rec.AGAktTypeActionProvPct = provPct == -1 ? 0 : provPct;
                        rec.AGAktTypeActionProvHonGrpID = honorarGrpId;
                        set.InsertRecord(rec);
                    }
                    break;
                #endregion
                #region AG + USER
                case FILTER_AG_USER:
                    sqlWhere = " WHERE AGUserProvAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " AND AGUserProvUserID = " + ddlUser.SelectedValue +
                                " AND AGUserProvAktAktionTypeID = " + actionId;

                    set.ExecuteNonQuery("DELETE FROM tblAuftraggeberUserProv " + sqlWhere);

                    if (provAmt != -1 || provAmtForZero != -1 || provPct != -1)
                    {
                        tblAuftraggeberUserProv rec = new tblAuftraggeberUserProv();

                        rec.AGUserProvAuftraggeberID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAuftraggeber.SelectedValue);
                        rec.AGUserProvUserID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlUser.SelectedValue);
                        rec.AGUserProvAktAktionTypeID = actionId;

                        rec.AGUserProvPrice = price;
                        rec.AGUserProvAmount = provAmt == -1 ? 0 : provAmt;
                        rec.AGUserProvAmountForZeroCollection = provAmtForZero == -1 ? 0 : provAmtForZero;
                        rec.AGUserProvPct = provPct == -1 ? 0 : provPct;
                        rec.AGUserProvHonGrpID = honorarGrpId;
                        set.InsertRecord(rec);
                    }
                    break;
                #endregion
                #region AG
                case FILTER_AG:

                    sqlWhere = " WHERE AGAktionProvAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " AND AGActionProvAktAktionTypeID = " + actionId;
                    set.ExecuteNonQuery("DELETE FROM tblAuftraggeberActionProv " + sqlWhere);
                    if (provAmt != -1 || provAmtForZero != -1 || provPct != -1)
                    {
                        tblAuftraggeberActionProv rec = new tblAuftraggeberActionProv();

                        rec.AGAktionProvAuftraggeberID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAuftraggeber.SelectedValue);
                        rec.AGActionProvAktAktionTypeID = actionId;

                        rec.AGActionProvPrice = price;
                        rec.AGActionProvAmount = provAmt == -1 ? 0 : provAmt;
                        rec.AGActionProvAmountForZeroCollection = provAmtForZero == -1 ? 0 : provAmtForZero;
                        rec.AGActionProvPct = provPct == -1 ? 0 : provPct;
                        rec.AGActionProvHonGrpID = honorarGrpId;
                        set.InsertRecord(rec);
                    }
                    break;
                #endregion
                #region USER + TYPE
                case FILTER_USER_TYPE:

                    sqlWhere = " WHERE UserAktTypeAktionProvUserID = " + ddlUser.SelectedValue +
                                " AND UserAktTypeActionProvAktTypeIntID = " + ddlAktType.SelectedValue +
                                " AND UserAktTypeActionProvAktAktionTypeID = " + actionId;

                    set.ExecuteNonQuery("DELETE FROM tblUserAktTypeActionProv " + sqlWhere);

                    if (provAmt != -1 || provAmtForZero != -1 || provPct != -1)
                    {
                        tblUserAktTypeActionProv rec = new tblUserAktTypeActionProv();

                        rec.UserAktTypeAktionProvUserID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlUser.SelectedValue);
                        rec.UserAktTypeActionProvAktTypeIntID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlAktType.SelectedValue);
                        rec.UserAktTypeActionProvAktAktionTypeID = actionId;

                        rec.UserAktTypeActionProvPrice = price;
                        rec.UserAktTypeActionProvAmount = provAmt == -1 ? 0 : provAmt;
                        rec.UserAktTypeActionProvAmountForZeroCollection = provAmtForZero == -1 ? 0 : provAmtForZero;
                        rec.UserAktTypeActionProvPct = provPct == -1 ? 0 : provPct;
                        rec.UserAktTypeActionProvHonGrpID = honorarGrpId;

                        set.InsertRecord(rec);
                    }
                    break;
                #endregion
                #region USER
                case FILTER_USER:

                    sqlWhere = " WHERE UserAktionProvUserID = " + ddlUser.SelectedValue +
                               " AND UserActionProvAktAktionTypeID = " + actionId;
                    
                    set.ExecuteNonQuery("DELETE FROM tblUserActionProv " + sqlWhere);
                    if (provAmt != -1 || provAmtForZero != -1 || provPct != -1)
                    {
                        tblUserActionProv rec = new tblUserActionProv();

                        rec.UserAktionProvUserID = GlobalUtilArea.GetZeroIfConvertToIntError(ddlUser.SelectedValue);
                        rec.UserActionProvAktAktionTypeID = actionId;

                        rec.UserActionProvPrice = price;
                        rec.UserActionProvAmount = provAmt == -1 ? 0 : provAmt;
                        rec.UserActionProvAmountForZeroCollection = provAmtForZero == -1 ? 0 : provAmtForZero;
                        rec.UserActionProvPct = provPct == -1 ? 0 : provPct;
                        rec.UserActionProvHonGrpID = honorarGrpId;
                        set.InsertRecord(rec);
                    }
                    break;
                #endregion
                #region NO FILTER
                case FILTER_NONE:

                    tblAktenIntActionType type = (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + actionId, typeof(tblAktenIntActionType));
                    if (type != null)
                    {
                        type.AktIntActionProvPrice = price;
                        type.AktIntActionProvAmount = provAmt == -1 ? 0 : provAmt;
                        type.AktIntActionProvAmountForZeroCollection = provAmtForZero == -1 ? 0 : provAmtForZero;
                        type.AktIntActionProvPct = provPct == -1 ? 0 : provPct;
                        type.AktIntActionProvHonGrpID = honorarGrpId;
                        set.UpdateRecord(type);
                    }
                    break;
                #endregion
            }
        }
        
        #region Prov Actions
        private void PopulateProvActionsGrid(ArrayList list)
        {
            DataTable dt = GetProvActionsDataTableStructure();
            foreach (ProvisionRecord prov in list)
            {
                DataRow dr = dt.NewRow();
                dr["ActionID"] = prov.ActionID;
                dr["ActionCaption"] = prov.ActionCaption;
                //dr["ProvAmount"] = prov.ProvAmount == 0 ? "" : prov.ProvAmount.ToString();
                //dr["ProvAmountForZero"] = prov.ProvAmountForZero == 0 ? "" : prov.ProvAmountForZero.ToString();
                //dr["ProvPct"] = prov.ProvPct == 0 ? "" : prov.ProvPct.ToString();
                dr["Price"] = prov.Price;
                dr["ProvAmount"] = prov.ProvAmount;
                dr["ProvAmountForZero"] = prov.ProvAmountForZero;
                dr["ProvPct"] = prov.ProvPct;
                dr["HonorarGrpId"] = prov.HonorarGrpId;
                dt.Rows.Add(dr);
            }
            gvActionProv.DataSource = dt;
            gvActionProv.DataBind();
            if (list.Count > 0)
            {
                lblRowCount.Text = "Datensätze 1 bis " + list.Count + " von " + list.Count;
            }
            else
            {
                lblRowCount.Text = "";
            }
        }
        private DataTable GetProvActionsDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ActionID", typeof(int)));
            dt.Columns.Add(new DataColumn("ActionCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("Price", typeof(string)));
            dt.Columns.Add(new DataColumn("ProvAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ProvAmountForZero", typeof(string)));
            dt.Columns.Add(new DataColumn("ProvPct", typeof(string)));
            dt.Columns.Add(new DataColumn("HonorarGrpId", typeof(string)));
            return dt;
        }
        #endregion

        #region Actions
        private void PopulateActionsGrid()
        {
            ShowActions();
            DataTable dt = GetActionsDataTableStructure();
            int showedCount = 0;
            ArrayList list;
            if (ddlAuftraggeber.SelectedValue == GlobalUtilArea.DefaultDropdownValue)
            {
                list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntActionType ORDER BY AktIntActionTypeCaption", typeof(tblAktenIntActionType));
            }
            else
            {
                list = HTBUtils.GetSqlRecords("SELECT * FROM qryAuftraggeberAction WHERE AGAktionAuftraggeberID = " + ddlAuftraggeber.SelectedValue + " ORDER BY AktIntActionTypeCaption", typeof(qryAuftraggeberAction));
            }
            if (list.Count == 0)
            {
                list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntActionType WHERE AktIntActionIsDefault = 1 ORDER BY AktIntActionTypeCaption", typeof(tblAktenIntActionType));
            }
            
            for (int i = 0; i <list.Count; i++) 
            {
                int actionId = 0;
                string actionCaption = "";
                if (list[i] is qryAuftraggeberAction)
                {
                    actionId = ((qryAuftraggeberAction)list[i]).AGAktionAktIntActionTypeID;
                    actionCaption = ((qryAuftraggeberAction)list[i]).AktIntActionTypeCaption;
                }
                else
                {
                    actionId = ((tblAktenIntActionType)list[i]).AktIntActionTypeID;
                    actionCaption = ((tblAktenIntActionType)list[i]).AktIntActionTypeCaption;
                }
                bool found = false;
                for (int j = 0; j < gvActionProv.Rows.Count; j++)
                {
                    GridViewRow provRow = gvActionProv.Rows[j];
                    Label lblProvActionId = (Label)provRow.Cells[0].FindControl("lblActionID");
                    if (lblProvActionId.Text == actionId.ToString())
                    {
                        found = true;
                        j = gvActionProv.Rows.Count;
                    }
                }
                if (!found)
                {
                    DataRow dr = dt.NewRow();
                    dr["ActionID"] = actionId;
                    dr["ActionCaption"] = actionCaption;
                    dt.Rows.Add(dr);
                    showedCount++;
                }
            }
            
            gvActions.DataSource = dt;
            gvActions.DataBind();
            if (showedCount > 0)
            {
                lblRowCount.Text = "Datensätze 1 bis " + showedCount + " von " + showedCount;
            }
            else
            {
                lblRowCount.Text = "";
            }
        }
        private DataTable GetActionsDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ActionID", typeof(int)));
            dt.Columns.Add(new DataColumn("ActionCaption", typeof(string)));
            return dt;
        }
        #endregion
    }
}