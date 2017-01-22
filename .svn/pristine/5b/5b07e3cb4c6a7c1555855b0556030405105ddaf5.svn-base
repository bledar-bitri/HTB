using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.HTB.StoredProcs;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBUtilities;

namespace HTB.v2.intranetx.werber
{
    public partial class Werber : System.Web.UI.Page
    {
        private ArrayList[] results;
        private double _totalKlientAmount;
        private double _totalEcpKosten;
        private double _totalEcpReceived;
        private double _totalProjectedProvision;
        private double _totalProvision;
        private double _totalProvisionTransferred;

        private double _totalStornoKlientAmount;
        private double _totalStornoEcpKosten;
        private double _totalStornoEcpReceived;
        private double _totalStornoProjectedProvision;
        private double _totalStornoProvision;
        private double _totalStornoProvisionTransferred;

        private int _userId;

        private readonly tblControl _control = HTBUtils.GetControlRecord();

        protected void Page_Load(object sender, EventArgs e)
        {
//            Session["MM_UserID"] = "541";
            _userId = GlobalUtilArea.GetUserId(Session);
            if (!IsPostBack)
                PopulateGrids();
        }

        private void PopulateGrids()
        {
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("startDate", SqlDbType.DateTime, "1.11.2012"),
                                     new StoredProcedureParameter("endDate", SqlDbType.DateTime, "20.11.2012"),
                                     new StoredProcedureParameter("userId", SqlDbType.Int, _userId)
                                 };
            results = HTBUtils.GetMultipleListsFromStoredProcedure("spGetPromoterProvision", parameters, new Type[] {typeof (spGetPromoterProvision), typeof (spGetPromoterProvision)});
            PopulateGrid(gvTransfers, results[0]);
            PopulateGrid(gvStorno, results[1], true);
        }

        private void PopulateGrid(GridView grid, ArrayList list, bool isStorno = false)
        {
            DataTable dt = GetGridDataTableStructure();
            if (isStorno)
            {
                _totalStornoKlientAmount = 0;
                _totalStornoEcpKosten = 0;
                _totalStornoEcpReceived = 0;
                _totalStornoProjectedProvision = 0;
                _totalStornoProvision = 0;
                _totalStornoProvisionTransferred = 0;
            }
            else
            {
                _totalKlientAmount = 0;
                _totalEcpKosten = 0;
                _totalEcpReceived = 0;
                _totalProjectedProvision = 0;
                _totalProvision = 0;
                _totalProvisionTransferred = 0;
            }
            foreach (spGetPromoterProvision rec in list)
            {
                DataRow dr = dt.NewRow();
                dr["AktID"] = rec.AktId;
                dr["AktDate"] = rec.AktDate.ToShortDateString();
                dr["AktStatus"] = rec.AktStatus;
                dr["KlientAmount"] = HTBUtils.FormatCurrency(rec.KlientAmount);
                dr["ECPAmount"] = HTBUtils.FormatCurrency(rec.ECPAmount);
                dr["ECPAmountReceived"] = HTBUtils.FormatCurrency(rec.ECPAmountReceived);
                dr["ProjectedProvision"] = HTBUtils.FormatCurrency(rec.ProjectedProvision);
                dr["Provision"] = HTBUtils.FormatCurrency(rec.Provision);
                dr["ProvisionTransferred"] = HTBUtils.FormatCurrency(rec.ProvisionTransferred);
                dr["Client"] = rec.KlientName;
                dr["Gegner"] = rec.GegnerName;
                dt.Rows.Add(dr);
                if (isStorno)
                {
                    _totalStornoKlientAmount += rec.KlientAmount;
                    _totalStornoEcpKosten += rec.ECPAmount;
                    _totalStornoEcpReceived += rec.ECPAmountReceived;
                    _totalStornoProjectedProvision += rec.ProjectedProvision;
                    _totalStornoProvision += rec.Provision;
                    _totalStornoProvisionTransferred += rec.ProvisionTransferred;
                }
                else
                {
                    _totalKlientAmount += rec.KlientAmount;
                    _totalEcpKosten += rec.ECPAmount;
                    _totalEcpReceived += rec.ECPAmountReceived;
                    _totalProjectedProvision += rec.ProjectedProvision;
                    _totalProvision += rec.Provision;
                    _totalProvisionTransferred += rec.ProvisionTransferred;
                }
            }
            grid.DataSource = dt;
            grid.DataBind();
        }

        private DataTable GetGridDataTableStructure()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("AktID", typeof(int)));
            dt.Columns.Add(new DataColumn("AktAZ", typeof(string)));
            dt.Columns.Add(new DataColumn("AktDate", typeof(string)));
            dt.Columns.Add(new DataColumn("AktStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("KlientAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ECPAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("ECPAmountReceived", typeof(string)));
            dt.Columns.Add(new DataColumn("ProjectedProvision", typeof(string)));
            dt.Columns.Add(new DataColumn("Provision", typeof(string)));
            dt.Columns.Add(new DataColumn("ProvisionTransferred", typeof(string)));
            dt.Columns.Add(new DataColumn("Client", typeof(string)));
            dt.Columns.Add(new DataColumn("Gegner", typeof(string)));
            return dt;
        }

        protected string GetTotalCount(int idx)
        {
            return results[idx].Count.ToString();
        }
        protected string GetTotalForderung(bool isStorno = false)
        {
            return isStorno ? HTBUtils.FormatCurrency(_totalStornoKlientAmount, true) : HTBUtils.FormatCurrency(_totalKlientAmount, true);
        }
        protected string GetTotalEcpKostenString(bool isStorno = false)
        {
            return isStorno ? HTBUtils.FormatCurrency(_totalStornoEcpKosten, true) : HTBUtils.FormatCurrency(_totalEcpKosten, true);
        }
        protected string GetTotalEcpReceivedString(bool isStorno = false)
        {
            return isStorno ? HTBUtils.FormatCurrency(_totalStornoEcpReceived, true) : HTBUtils.FormatCurrency(_totalEcpReceived, true);
        }
        protected string GetTotalProjectedProvision(bool isStorno = false)
        {
            return isStorno ? HTBUtils.FormatCurrency(_totalStornoProjectedProvision, true) : HTBUtils.FormatCurrency(_totalProjectedProvision, true);
        }
        protected string GetTotalProvision(bool isStorno = false)
        {
            return isStorno ? HTBUtils.FormatCurrency(_totalStornoProvision, true) : HTBUtils.FormatCurrency(_totalProvision, true);
        }
        protected string GetTotalProvisionTransferred(bool isStorno = false)
        {
            return isStorno ? HTBUtils.FormatCurrency(_totalStornoProvisionTransferred, true) : HTBUtils.FormatCurrency(_totalProvisionTransferred, true);
        }
    }
}