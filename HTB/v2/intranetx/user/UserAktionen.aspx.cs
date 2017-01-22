using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.util;
using System.Collections;
using HTBUtilities;
using System.Data;
using HTB.Database;
using HTB.Database.Views;

namespace HTB.v2.intranetx.user
{
    public partial class UserAktionen : System.Web.UI.Page
    {
        tblUser user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReloadUsers();
                PopulateUserActionsGrid();
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
            if (selValue != "")
            {
                ddlUser.SelectedValue = selValue;
            }
            if (ddlUser.SelectedValue != GlobalUtilArea.DefaultDropdownValue)
            {
                user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + ddlUser.SelectedValue, typeof(tblUser));
            }
            else
            {
                user = null;
            }
        }

        #region Event Handlers
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadUsers();
            PopulateUserActionsGrid();
        }
        protected void btnAddActions_Click(object sender, EventArgs e)
        {
            ReloadUsers();

            if (user != null)
            {
                RecordSet set = new RecordSet();
                for (int i = 0; i < gvActions.Rows.Count; i++)
                {
                    GridViewRow row = gvActions.Rows[i];
                    CheckBox chkSelected = (CheckBox)row.Cells[1].FindControl("chkSelected");

                    if (chkSelected.Checked)
                    {
                        Label lblActionId = (Label)row.Cells[0].FindControl("lblActionID");
                        set.ExecuteNonQuery("INSERT INTO tblUserAktionRel (UserAktionUserID, UserAktionAktIntActionTypeID) VALUES (" + user.UserID+ ", " + lblActionId.Text + ")");
                    }
                }
                PopulateUserActionsGrid();
            }
        }
        protected void btnRemoveActions_Click(object sender, EventArgs e)
        {
            ReloadUsers();
            RecordSet set = new RecordSet();
            for (int i = 0; i < gvUserActions.Rows.Count; i++)
            {
                GridViewRow row = gvUserActions.Rows[i];
                CheckBox chkSelected = (CheckBox)row.Cells[1].FindControl("chkSelected");

                if (chkSelected.Checked)
                {
                    Label lblActionId = (Label)row.Cells[0].FindControl("lblActionID");
                    set.ExecuteNonQuery("DELETE FROM tblUserAktionRel WHERE UserAktionUserID = " + user.UserID + " AND UserAktionAktIntActionTypeID = " + lblActionId.Text);
                }
            }
            PopulateUserActionsGrid();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../../intranet/intranet/intranet.asp");
        }
        protected void gvUserActions_DataBound(object sender, EventArgs e)
        {
            //Each time the data is bound to the grid we need to build up the CheckBoxIDs array 
            //Get the header CheckBox
            if (gvUserActions.HeaderRow != null)
            {
                CheckBox cbHeader = (CheckBox)gvUserActions.HeaderRow.FindControl("chkUserActionHeader");

                //Run the ChangeCheckBoxState client-side function whenever the header checkbox is checked/unchecked
                cbHeader.Attributes.Add("onclick", "ChangeAllUserActionCheckBoxStates(this.checked);");

                foreach (GridViewRow gvr in gvUserActions.Rows)
                {
                    //Get a programmatic reference to the CheckBox control
                    CheckBox cb = (CheckBox)gvr.FindControl("chkSelected");

                    //Add the CheckBox's ID to the client-side CheckBoxIDs array
                    ClientScript.RegisterArrayDeclaration("UserActionCheckBoxIDs", String.Concat("'", cb.ClientID, "'"));
                }
            }
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
        #endregion

        #region AGActions
        private void PopulateUserActionsGrid()
        {
            if (user != null)
            {
                ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM qryUserAktionen WHERE UserAktionUserID = " + user.UserID + " ORDER BY AktIntActionTypeCaption", typeof(qryUserAktionen));
                DataTable dt = GetActionsDataTableStructure();
                foreach (qryUserAktionen agAction in list)
                {
                    DataRow dr = dt.NewRow();
                    dr["ActionID"] = agAction.UserAktionAktIntActionTypeID;
                    dr["ActionCaption"] = agAction.AktIntActionTypeCaption;

                    dt.Rows.Add(dr);
                }
                gvUserActions.DataSource = dt;
                gvUserActions.DataBind();
                PopulateActionsGrid(list);
            }
            else
            {
                PopulateActionsGrid(new ArrayList());
            }
        }
        #endregion

        #region Non Default Actions
        private void PopulateActionsGrid(ArrayList userActionsList)
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntActionType ORDER BY AktIntActionTypeCaption", typeof(tblAktenIntActionType));
            DataTable dt = GetActionsDataTableStructure();
            foreach (tblAktenIntActionType action in list)
            {
                bool found = false;
                foreach (qryUserAktionen agAction in userActionsList)
                {
                    if (action.AktIntActionTypeID == agAction.UserAktionAktIntActionTypeID)
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


        private DataTable GetActionsDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ActionID", typeof(int)));
            dt.Columns.Add(new DataColumn("ActionCaption", typeof(string)));
            return dt;
        }
    }
}