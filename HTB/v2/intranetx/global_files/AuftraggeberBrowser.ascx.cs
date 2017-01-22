using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using HTB.Database;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.global_files
{
    public partial class AuftraggeberBrowser : System.Web.UI.UserControl
    {
        public ArrayList agList = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            //agList = HTBUtilities.HTBUtils.GetSqlRecords("SELECT *  FROM tblAuftraggeber  WHERE AuftraggeberName1 LIKE '%" + Request.QueryString["name"] + "%' ORDER BY AuftraggeberName1", HTB.Globals.Globals.DB_PACKAGE + ".tblAuftraggeber");
            agList = HTBUtilities.HTBUtils.GetSqlRecords("SELECT *  FROM tblAuftraggeber ORDER BY AuftraggeberName1", typeof(tblAuftraggeber));
            PopulateAGGrid();
        }

        private void PopulateAGGrid()
        {
            DataTable dt = GetGridDataTableStructure();
            foreach (tblAuftraggeber ag in agList)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = ag.AuftraggeberID;
                dr["Name"] = ag.AuftraggeberName1+", "+ag.AuftraggeberName2;
                dr["Address"] = ag.AuftraggeberPLZ + " " + ag.AuftraggeberOrt;
                dt.Rows.Add(dr);
            }
            gvAG.DataSource = dt;
            gvAG.DataBind();
        }
        private DataTable GetGridDataTableStructure()
        {
            DataTable dt = new DataTable();
            DataColumn dc;

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.Int32");
            dc.ColumnName = "ID";

            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Name";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "Address";
            dt.Columns.Add(dc);

            return dt;
        }
    }
}