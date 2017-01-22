using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database.HTB.Views;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class ProvisionAbrechnungTablet : System.Web.UI.Page
    {
        private double _totalProvision;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                DateTime today = DateTime.Now;
                DayOfWeek day = today.DayOfWeek;
                int days = day - DayOfWeek.Monday;
                txtDateStart.Text = today.AddDays(-days).ToShortDateString();
                txtDateEnd.Text = today.ToShortDateString();
                PopulateProvisionGrid();
            }
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateProvisionGrid();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        #endregion

        #region Grid
        private void PopulateProvisionGrid()
        {
            ArrayList qryProvList = HTBUtils.GetSqlRecords("SELECT * FROM dbo.qryAktIntProvabrechnung WHERE AktIntSB = "+GlobalUtilArea.GetUserId(Session)+" AND AktIntActionDate >= '" + GlobalUtilArea.GetNowIfConvertToDateError(txtDateStart.Text).ToShortDateString() + "' AND AktIntActionDate <= '" + GlobalUtilArea.GetNowIfConvertToDateError(txtDateEnd.Text).ToShortDateString() + " 23:59:59'", typeof(qryAktIntProvabrechnung));
            _totalProvision = 0;
            DataTable dt = GetAGHeaderDataTableStructure();
            foreach (qryAktIntProvabrechnung rec in qryProvList)
            {
                _totalProvision += rec.AktIntActionProvision;
                DataRow dr = dt.NewRow();
                dr["Akt"] = rec.AktIntAZ;
                dr["ActionDate"] = rec.AktIntActionDate.ToShortDateString();
                dr["ActionSB"] = rec.UserNachname+" "+rec.UserVorname;
                dr["Auftraggeber"] = rec.AuftraggeberName1;
                dr["Gegner"] = rec.GegnerLastName1 + " " + rec.GegnerLastName2;
                dr["Action"] = rec.AktIntActionTypeCaption + "<br/>" + HTBUtils.FormatCurrency(rec.AktIntActionBetrag);
                dr["Beleg"] = rec.AktIntActionBeleg;
                dr["Provision"] = HTBUtils.FormatCurrency(rec.AktIntActionProvision);
                dt.Rows.Add(dr);
            }
            gvProvision.DataSource = dt;
            gvProvision.DataBind();
            
        }
        private DataTable GetAGHeaderDataTableStructure()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Akt", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionSB", typeof(string)));
            dt.Columns.Add(new DataColumn("Auftraggeber", typeof(string)));
            dt.Columns.Add(new DataColumn("Gegner", typeof(string)));
            dt.Columns.Add(new DataColumn("Action", typeof(string)));
            dt.Columns.Add(new DataColumn("Beleg", typeof(string)));
            dt.Columns.Add(new DataColumn("Provision", typeof(string)));
            dt.Columns.Add(new DataColumn("Collected", typeof(string)));
            dt.Columns.Add(new DataColumn("OpenedAll", typeof(string)));
            return dt;
        }
        public string GetTotalProvision()
        {
            return HTBUtils.FormatCurrency(_totalProvision);
        }
        #endregion
    }
}