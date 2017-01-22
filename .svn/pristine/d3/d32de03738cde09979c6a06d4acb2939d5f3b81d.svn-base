using System;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.util;
using System.Collections;
using HTBUtilities;
using System.Data;
using HTB.Database;
using HTB.Database.Views;
using HTBExtras;

namespace HTB.v2.intranetx.auftraggeber
{
    public partial class AuftraggeberAktionen : System.Web.UI.Page
    {
        const string FilterAGType = "FILTER_AG_TYPE";
        const string FilterType = "FILTER_TYPE";
        const string FilterAG = "FILTER_AG";
        const string FilterNone = "FILTER_NONE";
        private string _filter;

        protected void Page_Load(object sender, EventArgs e)
        {
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

                ShowCurrentActions();
                PopulateDefaultActionsGrid();
            }
        }

        #region Event Handlers
        protected void ddlAuftraggeber_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowCurrentActions();
        }
        protected void ddlAktType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowCurrentActions();
        }

        protected void btnAddActions_Click(object sender, EventArgs e)
        {
            SetFilter();
            var set = new RecordSet();
            for (int i = 0; i < gvActions.Rows.Count; i++)
            {
                GridViewRow row = gvActions.Rows[i];
                var chkSelected = (CheckBox)row.Cells[1].FindControl("chkSelected");

                if (chkSelected.Checked)
                {
                    var lblActionId = (Label)row.Cells[0].FindControl("lblActionID");
                    switch (_filter)
                    {
                        case FilterAGType:
                            set.ExecuteNonQuery("INSERT INTO tblAuftraggeberAktTypeAktionRel (AGAktionTypeAuftraggeberID, AGAktionTypeAktTypeIntID, AGAktionTypeAktIntActionTypeID) VALUES (" + ddlAuftraggeber.SelectedValue + ", " + ddlAktType.SelectedValue + ", " + lblActionId.Text + ")");
                            break;
                        case FilterAG:
                            set.ExecuteNonQuery("INSERT INTO tblAuftraggeberAktionRel (AGAktionAuftraggeberID, AGAktionAktIntActionTypeID) VALUES (" + ddlAuftraggeber.SelectedValue + ", " + lblActionId.Text + ")");
                            break;
                        case FilterType:
                            set.ExecuteNonQuery("INSERT INTO tblAktTypeIntActionRel (AktTypeActionAktTypeIntID, AktTypeActionAktIntActionTypeID) VALUES (" + ddlAktType.SelectedValue + ", " + lblActionId.Text + ")");
                            break;
                    }
                }
            }
            ShowCurrentActions();
        }
        protected void btnRemoveActions_Click(object sender, EventArgs e)
        {
            SetFilter();
            RecordSet set = new RecordSet();
            for (int i = 0; i < gvAGActions.Rows.Count; i++)
            {
                GridViewRow row = gvAGActions.Rows[i];
                CheckBox chkSelected = (CheckBox)row.Cells[1].FindControl("chkSelected");

                if (chkSelected.Checked)
                {
                    Label lblActionId = (Label)row.Cells[0].FindControl("lblActionID");
                    switch (_filter)
                    {
                        case FilterAGType:
                            set.ExecuteNonQuery("DELETE FROM tblAuftraggeberAktTypeAktionRel WHERE AGAktionTypeAuftraggeberID = " + ddlAuftraggeber.SelectedValue + " AND AGAktionTypeAktTypeIntID =  " + ddlAktType.SelectedValue + " AND AGAktionTypeAktIntActionTypeID = " + lblActionId.Text);
                            break;
                        case FilterAG:
                            set.ExecuteNonQuery("DELETE FROM tblAuftraggeberAktionRel WHERE AGAktionAuftraggeberID = " + ddlAuftraggeber.SelectedValue + " AND AGAktionAktIntActionTypeID = " + lblActionId.Text);
                            break;
                        case FilterType:
                            set.ExecuteNonQuery("DELETE FROM tblAktTypeIntActionRel WHERE AktTypeActionAktTypeIntID = " + ddlAktType.SelectedValue +" AND AktTypeActionAktIntActionTypeID = " + lblActionId.Text);
                            break;
                    }
                }
            }
            ShowCurrentActions();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../../intranet/intranet/intranet.asp");
        }
        #endregion

        private void SetFilter()
        {
            if (ddlAuftraggeber.SelectedValue != GlobalUtilArea.DefaultDropdownValue && ddlAktType.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                _filter = FilterAGType;
            }
            else if (ddlAuftraggeber.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                _filter = FilterAG;
            }
            else if (ddlAktType.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                _filter = FilterType;
            }
            else
            {
                _filter = FilterNone;
            }
            ctlMessage.ShowInfo(_filter);
        }

        #region Existing Actions
        private void PopulateCurrentActionsGrid(ArrayList list)
        {
            DataTable dt = GetActionsDataTableStructure();
            foreach (ActionRecord agAction in list)
            {
                DataRow dr = dt.NewRow();
                dr["ActionID"] = agAction.ActionID;
                dr["ActionCaption"] = agAction.ActionCaption;

                dt.Rows.Add(dr);
            }
            gvAGActions.DataSource = dt;
            gvAGActions.DataBind();
            PopulateActionsGrid(list);
        }
        
        private void ShowCurrentActions()
        {
            string sqlQuery;
            SetFilter();
            ArrayList list;
            ArrayList resultsList;
            switch (_filter)
            {
                #region AG + TYPE
                case FilterAGType:

                    sqlQuery = "SELECT * FROM qryAuftraggeberAktTypeAction " +
                                " WHERE AGAktionTypeAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " AND AGAktionTypeAktTypeIntID = " + ddlAktType.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryAuftraggeberAktTypeAction));
                    list = new ArrayList();
                    foreach (qryAuftraggeberAktTypeAction action in resultsList)
                    {
                        list.Add(new ActionRecord(action));
                    }
                    PopulateCurrentActionsGrid(list);
                    break;
                #endregion
                #region AG
                case FilterAG:

                    sqlQuery = "SELECT * FROM qryAuftraggeberAction " +
                                " WHERE AGAktionAuftraggeberID = " + ddlAuftraggeber.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryAuftraggeberAction));
                    list = new ArrayList();
                    foreach (qryAuftraggeberAction action in resultsList)
                    {
                        list.Add(new ActionRecord(action));
                    }
                    PopulateCurrentActionsGrid(list);
                    break;
                #endregion
                #region Type
                case FilterType:

                    sqlQuery = "SELECT * FROM qryAktTypeAction " +
                                " WHERE AktTypeActionAktTypeIntID = " + ddlAktType.SelectedValue +
                                " ORDER BY AktIntActionTypeCaption";
                    resultsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(qryAktTypeAction));
                    list = new ArrayList();
                    foreach (qryAktTypeAction action in resultsList)
                    {
                        list.Add(new ActionRecord(action));
                    }
                    PopulateCurrentActionsGrid(list);
                    break;
                #endregion
                #region NO FILTER
                case FilterNone:

                    break;

                    #endregion
            }
        }
        #endregion

        #region Non Default Actions
        private void PopulateActionsGrid(ArrayList agActionsList)
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntActionType  WHERE AktIntActionIsInternal = 0  AND AktIntActionIsDefault = 0 ORDER BY AktIntActionTypeCaption", typeof(tblAktenIntActionType));
            DataTable dt = GetActionsDataTableStructure();
            foreach (tblAktenIntActionType action in list)
            {
                bool found = false;
                foreach (ActionRecord agAction in agActionsList)
                {
                    if (action.AktIntActionTypeID == agAction.ActionID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    DataRow dr = dt.NewRow();
                    dr["ActionID"] = action.AktIntActionTypeID;
                    dr["ActionCaption"] = action.AktIntActionTypeCaption;
                    dt.Rows.Add(dr);
                }
            }
            gvActions.DataSource = dt;
            gvActions.DataBind();
        }
        #endregion

        #region Default Actions
        private void PopulateDefaultActionsGrid()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntActionType WHERE AktIntActionIsInternal = 0  AND AktIntActionIsDefault = 1 ORDER BY AktIntActionTypeCaption", typeof(tblAktenIntActionType));
            DataTable dt = GetActionsDataTableStructure();
            foreach (tblAktenIntActionType action in list)
            {
                DataRow dr = dt.NewRow();
                dr["ActionCaption"] = action.AktIntActionTypeCaption;
                dt.Rows.Add(dr);
            }
            gvDefaultActions.DataSource = dt;
            gvDefaultActions.DataBind();
        }
        #endregion

        private DataTable GetActionsDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ActionID", typeof(int)));
            dt.Columns.Add(new DataColumn("ActionCaption", typeof(string)));
            return dt;
        }
    }
}