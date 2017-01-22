using System;
using System.Data;
using System.Collections;
using HTB.Database.HTB.StoredProcs;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint
{
    public partial class WeeklyValidation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetWeeklyValidationResults", null, typeof (spGetWeeklyValidationResults));
            DataTable dt = GetDataTableStructure();
            foreach (spGetWeeklyValidationResults result in list)
            {
                DataRow dr = dt.NewRow();
                dr["Akt"] = result.Akt;
                dr["ErrorDescription"] = result.ErrorDescription;
                dt.Rows.Add(dr);
            }
            gvErrors.DataSource = dt;
            gvErrors.DataBind();
        }


        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("Akt", typeof(string)));
            dt.Columns.Add(new DataColumn("ErrorDescription", typeof(string)));
            return dt;
        }
    }
}