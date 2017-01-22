using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using HTBUtilities;
using HTB.Database;
using HTB.v2.intranetx.util;
using System.Drawing;
using HTB.Database.StoredProcs;
using System.Text;

namespace HTB.v2.intranetx.aktenintprovfix
{
    public partial class ProvBalance : System.Web.UI.Page
    {
        private double totalPrice;
        private double totalProvision;
        private double totalCount;
        private double totalAGPrice;
        private double totalAGProvision;
        private double totalAGCount;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalUtilArea.GetUserId(Session);
        }

        protected void gvHeaderGrid_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < gvHeader.Rows.Count; i++)
            {
                GridViewRow row = gvHeader.Rows[i];
                GridView gv = (GridView)row.FindControl("gvDetail");
                Label lblUserId = (Label)row.FindControl("lblUserID");

                ArrayList list = GetDetailRecords(Convert.ToInt32(lblUserId.Text));
                if (list != null && list.Count > 0)
                {
                    DataTable dt = GetDetailDataTableStructure();
                    foreach (spProvisionDetail rec in list)
                    {
                        DataRow dr = dt.NewRow();
                        dr["AktIdLink"] = GetAktLink(rec.AktIntID);
                        dr["AktId"] = rec.AktIntID.ToString();
                        dr["Aktion"] = rec.AktIntActionTypeCaption;
                        dr["ActionDate"] = rec.AktIntActionDate.ToShortDateString();
                        dr["ActionPrice"] = HTBUtils.FormatCurrency(rec.AktIntActionPrice);
                        dr["ActionProv"] = HTBUtils.FormatCurrency(rec.AktIntActionProvision);
                        dr["ActionDifference"] = HTBUtils.FormatCurrency(rec.AktIntActionPrice - rec.AktIntActionProvision);
                        dr["AktType"] = rec.AktTypeINTCaption;

                        SetForeColor(dr, rec.AktIntActionPrice, rec.AktIntActionProvision);
                        dt.Rows.Add(dr);
                    }
                    gv.DataSource = dt;
                    gv.DataBind();
                }
            }
        }

        protected void gvAGHeaderGrid_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < gvAGHeader.Rows.Count; i++)
            {
                GridViewRow row = gvAGHeader.Rows[i];
                GridView gv = (GridView)row.FindControl("gvAGDetail");
                Label lblAgId = (Label)row.FindControl("lblAGID");

                ArrayList list = GetAGDetailRecords(Convert.ToInt32(lblAgId.Text));
                if (list != null && list.Count > 0)
                {
                    DataTable dt = GetDetailDataTableStructure();
                    foreach (spProvisionDetailAG rec in list)
                    {
                        DataRow dr = dt.NewRow();

                        dr["AktIdLink"] = GetAktLink(rec.AktIntID);
                        dr["AktId"] = rec.AktIntID.ToString();
                        dr["Aktion"] = rec.AktIntActionTypeCaption;
                        dr["ActionDate"] = rec.AktIntActionDate.ToShortDateString();
                        dr["ActionPrice"] = HTBUtils.FormatCurrency(rec.AktIntActionPrice);
                        dr["ActionProv"] = HTBUtils.FormatCurrency(rec.AktIntActionProvision);
                        dr["ActionDifference"] = HTBUtils.FormatCurrency(rec.AktIntActionPrice - rec.AktIntActionProvision);
                        dr["AktType"] = rec.AktTypeINTCaption;
                        SetForeColor(dr, rec.AktIntActionPrice, rec.AktIntActionProvision);
                        dt.Rows.Add(dr);
                    }
                    gv.DataSource = dt;
                    gv.DataBind();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateHeaderGrid();
                PopulateAGHeaderGrid();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        private string GetAktLink(int aktId)
        {
            StringBuilder sb = new StringBuilder("<a href=\"javascript:MM_openBrWindow('/v2/intranetx/aktenint/WorkAktInt.aspx?ID=");
            sb.Append(aktId);
            sb.Append("&");
            sb.Append(GlobalHtmlParams.IS_POPUP);
            sb.Append("=");
            sb.Append(GlobalHtmlParams.YES);
            sb.Append("','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10')\">");
            sb.Append(aktId);
            sb.Append("</a>");
            return sb.ToString();
        }

        #region Header Grid
        private ArrayList GetHeaderRecords()
        {
            string spName = "spProvisionHeader";
            ArrayList list = new ArrayList();
            list.Add(new StoredProcedureParameter("strDate", SqlDbType.Date, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart.Text)));
            list.Add(new StoredProcedureParameter("endDate", SqlDbType.Date, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd.Text)));
            
            return HTBUtils.GetStoredProcedureRecords(spName, list, typeof(spProvisionHeader));
        }
        
        private void PopulateHeaderGrid()
        {
            totalPrice = 0;
            totalProvision = 0;
            totalCount = 0;
            ArrayList list = GetHeaderRecords();
            if (list != null)
            {
                DataTable dt = GetHeaderDataTableStructure();
                foreach (spProvisionHeader rec in list)
                {
                    totalPrice += rec.TotalPrice;
                    totalProvision += rec.TotalProvision;
                    totalCount += rec.TotalActions;
                    DataRow dr = dt.NewRow();
                    dr["UserId"] = rec.UserID;
                    dr["User"] = rec.UserVorname + " " + rec.UserNachname;
                    dr["TotalPrice"] = HTBUtils.FormatCurrency(rec.TotalPrice);
                    dr["TotalProv"] = HTBUtils.FormatCurrency(rec.TotalProvision);
                    dr["Difference"] = HTBUtils.FormatCurrency(rec.TotalPrice - rec.TotalProvision);
                    dr["Count"] = rec.TotalActions;
                    SetForeColor(dr, rec.TotalPrice, rec.TotalProvision);
                    dt.Rows.Add(dr);
                }
                gvHeader.DataSource = dt;
                gvHeader.DataBind();
            }
        }
        
        private DataTable GetHeaderDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("UserID", typeof(int)));
            dt.Columns.Add(new DataColumn("User", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalProv", typeof(string)));
            dt.Columns.Add(new DataColumn("Difference", typeof(string)));
            dt.Columns.Add(new DataColumn("Count", typeof(string)));
            dt.Columns.Add(new DataColumn("ForeColor", typeof(System.Drawing.Color)));
            return dt;
        }
        #endregion

        #region Detail Grid
        private ArrayList GetDetailRecords(int userId)
        {
            string spName = "spProvisionDetail";
            ArrayList list = new ArrayList();
            list.Add(new StoredProcedureParameter("strDate", SqlDbType.Date, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart.Text)));
            list.Add(new StoredProcedureParameter("endDate", SqlDbType.Date, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd.Text)));
            list.Add(new StoredProcedureParameter("userId", SqlDbType.Int, userId));

            return HTBUtils.GetStoredProcedureRecords(spName, list, typeof(spProvisionDetail));
        }
        
        private DataTable GetDetailDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("AktId", typeof(int)));
            dt.Columns.Add(new DataColumn("Aktion", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionProv", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionDifference", typeof(string)));
            dt.Columns.Add(new DataColumn("Count", typeof(string)));
            dt.Columns.Add(new DataColumn("AktType", typeof(string)));
            dt.Columns.Add(new DataColumn("ForeColor", typeof(System.Drawing.Color)));
            dt.Columns.Add(new DataColumn("AktIdLink", typeof(string)));
            return dt;
        }
        #endregion

        #region AG Header Grid
        private ArrayList GetAGHeaderRecords()
        {
            string spName = "spProvisionHeaderAG";
            ArrayList list = new ArrayList();
            list.Add(new StoredProcedureParameter("strDate", SqlDbType.Date, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart.Text)));
            list.Add(new StoredProcedureParameter("endDate", SqlDbType.Date, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd.Text)));

            return HTBUtils.GetStoredProcedureRecords(spName, list, typeof(spProvisionHeaderAG));
        }

        private void PopulateAGHeaderGrid()
        {
            totalAGPrice = 0;
            totalAGProvision = 0;
            totalAGCount = 0;
            ArrayList list = GetAGHeaderRecords();
            if (list != null)
            {
                DataTable dt = GetAGHeaderDataTableStructure();
                foreach (spProvisionHeaderAG rec in list)
                {
                    totalAGPrice += rec.TotalPrice;
                    totalAGProvision += rec.TotalProvision;
                    totalAGCount += rec.TotalActions;
                    DataRow dr = dt.NewRow();
                    dr["AuftraggeberID"] = rec.AuftraggeberID;
                    dr["Auftraggeber"] = rec.AuftraggeberName1 + " " + rec.AuftraggeberName2;
                    dr["TotalPrice"] = HTBUtils.FormatCurrency(rec.TotalPrice);
                    dr["TotalProv"] = HTBUtils.FormatCurrency(rec.TotalProvision);
                    dr["Difference"] = HTBUtils.FormatCurrency(rec.TotalPrice - rec.TotalProvision);
                    dr["Count"] = rec.TotalActions;
                    SetForeColor(dr, rec.TotalPrice, rec.TotalProvision);
                    dt.Rows.Add(dr);
                }
                gvAGHeader.DataSource = dt;
                gvAGHeader.DataBind();
            }
        }

        private DataTable GetAGHeaderDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("AuftraggeberID", typeof(int)));
            dt.Columns.Add(new DataColumn("Auftraggeber", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalProv", typeof(string)));
            dt.Columns.Add(new DataColumn("Difference", typeof(string)));
            dt.Columns.Add(new DataColumn("Count", typeof(string)));
            dt.Columns.Add(new DataColumn("ForeColor", typeof(System.Drawing.Color)));
            return dt;
        }
        #endregion

        #region AG Detail Grid
        private ArrayList GetAGDetailRecords(int auftraggeberId)
        {
            string spName = "spProvisionDetailAG";
            ArrayList list = new ArrayList();
            list.Add(new StoredProcedureParameter("strDate", SqlDbType.Date, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart.Text)));
            list.Add(new StoredProcedureParameter("endDate", SqlDbType.Date, GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd.Text)));
            list.Add(new StoredProcedureParameter("agId", SqlDbType.Int, auftraggeberId));

            return HTBUtils.GetStoredProcedureRecords(spName, list, typeof(spProvisionDetailAG));
        }
        #endregion

        #region Get Methods
        public double GetTotalPrice()
        {
            return totalPrice;
        }

        public double GetTotalProvision()
        {
            return totalProvision;
        }

        public double GetTotalCount()
        {
            return totalCount;
        }

        public double GetDifference()
        {
            return totalPrice - totalProvision;
        }

        public double GetAGTotalPrice()
        {
            return totalAGPrice;
        }

        public double GetAGTotalProvision()
        {
            return totalAGProvision;
        }
        public double GetAGTotalCount()
        {
            return totalAGCount;
        }
        public double GetAGDifference()
        {
            return totalAGPrice - totalAGProvision;
        }
        #endregion

        private void SetForeColor(DataRow dr, double price, double prov)
        {

            if (price > prov)
                dr["ForeColor"] = Color.Green;
            else if (price < prov)
                dr["ForeColor"] = Color.Red;
            else
                dr["ForeColor"] = Color.White; // invisible
        }
    }
}