using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.util;
using HTB.Database;
using System.Data;
using System.Collections;
using HTBUtilities;
using HTB.Database.Views;

namespace HTB.v2.intranetx.aktenintprovfix
{
    public partial class HonorarSchema : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(
                    ddlHonorarGroup,
                    "SELECT * FROM tblAktenIntHonorarGroup ORDER BY AktIntHonGrpCaption",
                    typeof(tblAktenIntHonorarGroup),
                    "AktIntHonGrpID",
                    "AktIntHonGrpCaption",
                    false
                );
                PopulateGroupHonorarsGrid();
            }
        }

        #region Event Handlers
        protected void ddlHonorarGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateGroupHonorarsGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            tblAktenIntHonorarGroup group = (tblAktenIntHonorarGroup)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntHonorarGroup WHERE AktIntHonGrpID = " + ddlHonorarGroup.SelectedValue, typeof(tblAktenIntHonorarGroup));
            if (group != null)
            {
                SingleValue seqNbr = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT MAX(AktIntHonGrpRelSeqNbr) AS [IntValue] FROM tblAktenIntHonorarGroupRel WHERE AktIntHonGrpID = " + group.AktIntHonGrpID, typeof(SingleValue));
                RecordSet set = new RecordSet();
                for (int i = 0; i < gvHonorarItems.Rows.Count; i++)
                {
                    GridViewRow row = gvHonorarItems.Rows[i];
                    CheckBox chkSelected = (CheckBox)row.Cells[1].FindControl("chkSelected");
                    if (chkSelected.Checked)
                    {
                        Label lblHonorarId = (Label)row.Cells[0].FindControl("lblHonorarId");
                        seqNbr.IntValue++;
                        set.ExecuteNonQuery("INSERT INTO tblAktenIntHonorarGroupRel (AktIntHonGrpID, AktIntHonID, AktIntHonGrpRelSeqNbr) VALUES (" + group.AktIntHonGrpID + ", " + lblHonorarId.Text + ", " + seqNbr.IntValue + ")");
                    }
                }
                PopulateGroupHonorarsGrid();
            }
            else
            {
                // TODO: Show Error Message
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            tblAktenIntHonorarGroup group = (tblAktenIntHonorarGroup)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntHonorarGroup WHERE AktIntHonGrpID = " + ddlHonorarGroup.SelectedValue, typeof(tblAktenIntHonorarGroup));
            RecordSet set = new RecordSet();
            for (int i = 0; i < gvHonorarGroupItems.Rows.Count; i++)
            {
                GridViewRow row = gvHonorarGroupItems.Rows[i];
                CheckBox chkSelected = (CheckBox)row.Cells[1].FindControl("chkSelected");
                if (chkSelected.Checked)
                {
                    Label lblHonorarId = (Label)row.Cells[0].FindControl("lblHonorarId");
                    set.ExecuteNonQuery("DELETE FROM tblAktenIntHonorarGroupRel WHERE AktIntHonGrpID = " + group.AktIntHonGrpID + " AND AktIntHonID = " + lblHonorarId.Text);
                }
            }
            PopulateGroupHonorarsGrid();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Group Honorar Grid
        private void PopulateGroupHonorarsGrid()
        {
            if (ddlHonorarGroup.Items.Count > 0 &&  ddlHonorarGroup.SelectedValue != null && ddlHonorarGroup.SelectedValue != string.Empty)
            {
                ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM qryAktenIntGroupHonorar WHERE AktIntHonGrpID = " + ddlHonorarGroup.SelectedValue + " ORDER BY AktIntHonGrpRelSeqNbr", typeof(qryAktenIntGroupHonorar));
                DataTable dt = GetHonorarDataTableStructure();
                foreach (tblAktenIntHonorar honorar in list)
                {
                    DataRow dr = dt.NewRow();
                    dr["HonorarID"] = honorar.AktIntHonID;
                    dr["From"] = HTBUtils.FormatCurrency(honorar.AktIntHonFrom);
                    dr["To"] = HTBUtils.FormatCurrency(honorar.AktIntHonTo);
                    dr["Price"] = HTBUtils.FormatCurrency(honorar.AktIntHonPrice);
                    dr["Pct"] = honorar.AktIntHonPct;
                    dr["PctOf"] = honorar.AktIntHonPctOf == 0 ? "Kassierten Betrag" : "Ofenen Forderung";
                    dr["MaxPrice"] = HTBUtils.FormatCurrency(honorar.AktIntHonMaxPrice);
                    dr["ProvAmount"] = HTBUtils.FormatCurrency(honorar.AktIntHonProvAmount);
                    dr["ProvPct"] = honorar.AktIntHonProvPct;
                    dr["MaxProvAmount"] = HTBUtils.FormatCurrency(honorar.AktIntHonMaxProvAmount);
                    dr["ProvPctOf"] = honorar.AktIntHonProvPctOf == 0 ? "Kassierten Betrag" : "Ofenen Forderung";

                    dt.Rows.Add(dr);
                }
                gvHonorarGroupItems.DataSource = dt;
                gvHonorarGroupItems.DataBind();
                PopulateHonorarsGrid(list);
            }
        }
        #endregion
        
        #region Honorar Grid
        private void PopulateHonorarsGrid(ArrayList groupList)
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntHonorar", typeof(tblAktenIntHonorar));
            DataTable dt = GetHonorarDataTableStructure();
            foreach (tblAktenIntHonorar honorar in list)
            {
                bool found = false;
                foreach (qryAktenIntGroupHonorar groupHonorar in groupList)
                {
                    if (groupHonorar.AktIntHonID == honorar.AktIntHonID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    DataRow dr = dt.NewRow();
                    dr["HonorarID"] = honorar.AktIntHonID;
                    dr["From"] = HTBUtils.FormatCurrency(honorar.AktIntHonFrom);
                    dr["To"] = HTBUtils.FormatCurrency(honorar.AktIntHonTo);
                    dr["Price"] = HTBUtils.FormatCurrency(honorar.AktIntHonPrice);
                    dr["Pct"] = honorar.AktIntHonPct;
                    dr["PctOf"] = honorar.AktIntHonPctOf == 0 ? "Kassierten Betrag" : "Ofenen Forderung";
                    dr["MaxPrice"] = HTBUtils.FormatCurrency(honorar.AktIntHonMaxPrice);
                    dr["ProvAmount"] = HTBUtils.FormatCurrency(honorar.AktIntHonProvAmount);
                    dr["ProvPct"] = honorar.AktIntHonProvPct;
                    dr["MaxProvAmount"] = HTBUtils.FormatCurrency(honorar.AktIntHonMaxProvAmount);
                    dr["ProvPctOf"] = honorar.AktIntHonProvPctOf == 0 ? "Kassierten Betrag" : "Ofenen Forderung";
                    dr["EditUrl"] = "../../intranet/images/edit.gif";
                    dr["EditPopupUrl"] = "EditHonorar.aspx?IsPopUp=Y&ID=" + honorar.AktIntHonID;
                    dt.Rows.Add(dr);
                }
            }
            gvHonorarItems.DataSource = dt;
            gvHonorarItems.DataBind();
        }
        private DataTable GetHonorarDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("HonorarID", typeof(int)));
            dt.Columns.Add(new DataColumn("From", typeof(string)));
            dt.Columns.Add(new DataColumn("To", typeof(string)));
            dt.Columns.Add(new DataColumn("Price", typeof(string)));
            dt.Columns.Add(new DataColumn("Pct", typeof(string)));
            dt.Columns.Add(new DataColumn("PctOf", typeof(string)));
            dt.Columns.Add(new DataColumn("MaxPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("ProvAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ProvPct", typeof(string)));
            dt.Columns.Add(new DataColumn("MaxProvAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ProvPctOf", typeof(string)));
            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditPopupUrl", typeof(string)));
            return dt;
        }
        #endregion
    }
}