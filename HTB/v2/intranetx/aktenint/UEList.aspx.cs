using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.HTB.StoredProcs;
using HTB.v2.intranetx.permissions;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint
{
    public partial class UEList : System.Web.UI.Page
    {
        private double _totalToTransferAmount = 0;
        private double _totalTransferredAmount = 0;
        private double _totalCollectionAmount = 0;
        private ArrayList _toTransferList;
        private ArrayList _transferredList;
        private ArrayList _collectionFeesList;
        private PermissionsUEList permissions = new PermissionsUEList();

        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.USER_ID]);

            string userName = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.USER_NAME]).Replace("'", "''");
            string password = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.PASSWORD]).Replace("'", "''");

            bool ok = true;
            if(userId == 0 || (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)))
            {
                ok = GlobalUtilArea.Login(userName, password, Session, Response);
            }
            if (ok)
            {
                permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));

                if(!permissions.GrantUEListManager)
                {
                    ddlUsers.Visible = false;
                    chkShowCollectionFees.Visible = false;
                    trCollection.Visible = false;
                }
                // DO NOT RE-BIND THE GRID ON POSTBACK (otherwise we lose the data entry)
                if (!IsPostBack)
                {
                    LoadPaymentList(GlobalUtilArea.GetUserId(Session));
                
                    ReloadUsersDropDownList(GlobalUtilArea.GetUserId(Session));
                    SetValues();
                }
            }
        }

        private void LoadPaymentList(int userId)
        {
            var parameters = new ArrayList();

            if (userId > 0)
            {
                parameters.Add(new StoredProcedureParameter("userId", SqlDbType.Int, userId));
            }

            if (chkShowPassedTransfers.Checked)
            {
                parameters.Add(new StoredProcedureParameter("transferredDateFrom", SqlDbType.DateTime, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateFrom)));
                parameters.Add(new StoredProcedureParameter("transferredDateTo", SqlDbType.DateTime, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateTo)));
            }

            parameters.Add(new StoredProcedureParameter("loadCollection", SqlDbType.Bit, chkShowCollectionFees.Checked));

            ArrayList[] results = HTBUtils.GetMultipleListsFromStoredProcedure("spGetAussendienstTransferList", parameters, new[] { typeof(spGetAussendienstTransferList), typeof(spGetAussendienstTransferList), typeof(spGetAussendienstTransferList) });
            _toTransferList = results[0];
            _transferredList = results[1];
            _collectionFeesList = results[2];
        }

        private void SetValues()
        {
            DateTime now = DateTime.Now;
            txtDateFrom.Text = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            txtDateTo.Text = now.ToShortDateString();

            PopulateGrids();
        }

        #region Event Handlers
        protected void btnGo_Click(object sender, EventArgs e)
        {
            int selectedUser = GlobalUtilArea.GetZeroIfConvertToIntError(ddlUsers.SelectedValue);
            LoadPaymentList(selectedUser);
            PopulateGrids();
            ReloadUsersDropDownList(selectedUser);

            trTransferred.Visible = chkShowPassedTransfers.Checked;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            var posIds = new List<string>();
            
            for (int i = 0; i < gvTransfers.Rows.Count; i++)
            {
                GridViewRow row = gvTransfers.Rows[i];
                var lblPosId = (Label) row.Cells[0].FindControl("lblPosId");
                int posId = GlobalUtilArea.GetZeroIfConvertToIntError(lblPosId.Text);
                posIds.Add(posId.ToString());
            }
            string sql = "UPDATE tblAktenIntPos SET AktIntPosIsTransferred = 1, AktIntPosTransferredDate = '" + DateTime.Now + "' WHERE AktIntPosID IN ";
            sql += GlobalUtilArea.GetSqlInClause(posIds);
            new RecordSet().ExecuteNonQuery(sql);
            try
            {
                Thread.Sleep(200); 
            }
            catch
            {
            }
            LoadPaymentList(GlobalUtilArea.GetZeroIfConvertToIntError(ddlUsers.SelectedValue));
            PopulateGrids(); // reload grid
//            ctlMessage.ShowInfo(sql);
            ctlMessage.ShowSuccess("Die &Uuml;berweisunge sind durchgef&uuml;hrt!");

            trTransferred.Visible = chkShowPassedTransfers.Checked;
        }

        protected void btnCollectionReceived_Click(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            var posIds = new List<string>();

            for (int i = 0; i < gvCollection.Rows.Count; i++)
            {
                GridViewRow row = gvCollection.Rows[i];
                var lblPosId = (Label)row.FindControl("lblPosId");
                var chkIsReceived = (CheckBox)row.FindControl("chkIsReceived");
                if (chkIsReceived != null && chkIsReceived.Checked)
                {
                    int posId = GlobalUtilArea.GetZeroIfConvertToIntError(lblPosId.Text);
                    posIds.Add(posId.ToString());
                }
            }
            if (posIds.Count > 0)
            {
                string sql = "UPDATE tblAktenIntPos SET AktIntPosIsTransferred = 1, AktIntPosTransferredDate = '" + DateTime.Now + "' WHERE AktIntPosID IN ";
                sql += GlobalUtilArea.GetSqlInClause(posIds);
                new RecordSet().ExecuteNonQuery(sql);
                try
                {
                    Thread.Sleep(200);
                }
                catch
                {
                }
                LoadPaymentList(GlobalUtilArea.GetZeroIfConvertToIntError(ddlUsers.SelectedValue));
            }
            PopulateGrids(); // reload grid
            ctlMessage.ShowSuccess("Kostenemfangun durchgef&uuml;hrt!");

            trTransferred.Visible = chkShowPassedTransfers.Checked;
        }
        #endregion

        private void ReloadUsersDropDownList(int selectedUserId)
        {
            GlobalUtilArea.LoadUserDropdownList(ddlUsers, GlobalUtilArea.GetUsers(Session), true);
            try
            {
                ddlUsers.SelectedValue = selectedUserId.ToString();
            }
            catch
            {
            }
        }

        private void PopulateGrids()
        {
            PopulateGrid(_toTransferList, gvTransfers, ref _totalToTransferAmount);
            PopulateGrid(_transferredList, gvTransferred, ref _totalTransferredAmount);
            PopulateGrid(_collectionFeesList, gvCollection, ref _totalCollectionAmount);
        }

        private void PopulateGrid(ArrayList list, GridView grid, ref double totalAmount)
        {
            DataTable dt = GetGridDataTableStructure();
            totalAmount = 0;
            foreach (spGetAussendienstTransferList rec in list)
            {
                DataRow dr = dt.NewRow();
                dr["PosId"] = rec.PosId;
                dr["AktId"] = rec.AktId;
                dr["AktAZ"] = rec.AktAZ.Trim();
                dr["PosAmount"] = HTBUtils.FormatCurrency(rec.PosAmount);
                dr["PosDate"] = rec.PosDate.ToShortDateString();
                dr["PosCaption"] = rec.PosCaption;
                dr["AuftraggeberName"] = rec.AuftraggeberName;
                dr["GegnerName"] = rec.GegnerName;
                dr["UserName"] = rec.UserName;

                dr["TransferToBankAccount"] = rec.TransferToBankAccount;
                dr["Memo"] = rec.Memo;
                dr["TransferredDate"] = rec.TransferredDate.ToShortDateString();
                totalAmount += rec.PosAmount;
                dt.Rows.Add(dr);
            }
            grid.DataSource = dt;
            grid.DataBind();
        }

        private DataTable GetGridDataTableStructure()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("PosId", typeof(int)));
            dt.Columns.Add(new DataColumn("AktId", typeof(int)));
            dt.Columns.Add(new DataColumn("AktAZ", typeof(string)));
            dt.Columns.Add(new DataColumn("PosAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PosDate", typeof(string)));
            dt.Columns.Add(new DataColumn("PosCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("AuftraggeberName", typeof(string)));
            dt.Columns.Add(new DataColumn("GegnerName", typeof(string)));
            dt.Columns.Add(new DataColumn("UserName", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferToBankAccount", typeof(string)));
            dt.Columns.Add(new DataColumn("Memo", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferredDate", typeof(string)));

            return dt;
        }

        protected string GetTotalToTransferAmountString()
        {
            return HTBUtils.FormatCurrency(_totalToTransferAmount, true);
        }
        protected string GetTotalTransferredAmountString()
        {
            return HTBUtils.FormatCurrency(_totalTransferredAmount, true);
        }
        protected string GetTotalCollectionAmountString()
        {
            return HTBUtils.FormatCurrency(_totalCollectionAmount, true);
        }
    }
}