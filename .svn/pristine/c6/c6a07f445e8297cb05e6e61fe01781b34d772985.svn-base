using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.Views;
using HTBUtilities;

namespace HTB.v2.intranetx.kassablock
{
    public partial class MissingBeleg : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateMissingGrid();
            }
        }

        private void PopulateMissingGrid()
        {
            var missingBelegsList = HTBUtils.GetSqlRecords("SELECT * FROM qryKassaBlockMissingNr", typeof(qryKassaBlockMissingNr));
            var dt = GetGridDataTableStructure();
            foreach (qryKassaBlockMissingNr rec in missingBelegsList)
            {
                var dr = dt.NewRow();
                dr["User"] = rec.UserVorname + " " + rec.UserNachname;
                dr["KbMissUser"] = rec.KbMissUser;
                dr["KbMissNr"] = rec.KbMissNr;
                dr["KbMissDate"] = rec.KbMissDate.ToShortDateString();
                dt.Rows.Add(dr);
            }
            gvBelege.DataSource = dt;
            gvBelege.DataBind();
            btnSubmit.Visible = missingBelegsList.Count != 0;
        }

        private static DataTable GetGridDataTableStructure()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("KbMissUser", typeof(string)));
            dt.Columns.Add(new DataColumn("KbMissNr", typeof(string)));
            dt.Columns.Add(new DataColumn("KbMissDate", typeof(string)));
            dt.Columns.Add(new DataColumn("KbReceived", typeof(bool)));
            dt.Columns.Add(new DataColumn("User", typeof(string)));
            
            return dt;
        }

        #region Event Handlers
        protected void Submit_Click(object sender, EventArgs e)
        {
            var set = new RecordSet();
            for (int i = 0; i < gvBelege.Rows.Count; i++)
            {
                GridViewRow row = gvBelege.Rows[i];
                var lblMissingUserID = (Label)row.FindControl("lblMissingUserID");
                var lblMissingBelegNr = (Label)row.FindControl("lblMissingBelegNr");
                var chkReceived = (CheckBox)row.FindControl("chkReceived");

                if (chkReceived.Checked)
                {
                    var sb = new StringBuilder("UPDATE tblKassaBlockMissingNr SET KbMissReceivedDate = '");
                    sb.Append(DateTime.Now);
                    sb.Append("' WHERE KbMissUser = ");
                    sb.Append(lblMissingUserID.Text);
                    sb.Append(" AND KbMissNr = ");
                    sb.Append(lblMissingBelegNr.Text);
                    set.ExecuteNonQuery(sb.ToString());
                }
            }
            PopulateMissingGrid();
        }
        #endregion

    }
}