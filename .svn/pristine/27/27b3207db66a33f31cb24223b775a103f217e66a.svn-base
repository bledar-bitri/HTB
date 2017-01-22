using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using HTBExcel;
using HTBExtras;
using HTB.Database;
using System.Collections;
using System.Data;
using HTBUtilities;
using HTB.v2.intranetx.util;
using System.Reflection;

namespace HTB.v2.intranetx.customer
{
    public partial class TransferToCustomer : System.Web.UI.Page
    {
        private const int IdxDetail = 0;
        private const int IdxSummary = 1;

        private DateTime _startDate;
        private DateTime _endDate;
        private ArrayList[] _invoicesList;
        private static int _klientId = -1;
        private tblKlient _klient;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
//            Session["MM_Klient"] = "10455";
            GlobalUtilArea.GetUserId(Session); // force session validation
            _klientId = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_Klient"]);
            if (_klientId > 0)
            {
                _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientID = " + _klientId, typeof(tblKlient));
                if (_klient != null)
                {
                    lblKlientInfo.Text = "<em>&nbsp;von&nbsp;ECP&nbsp;an&nbsp;" + _klient.KlientName1 + "&nbsp;" + _klient.KlientName2 + "</em>";
                }
            }
            if (!IsPostBack)
            {
                txtDateStart.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                txtDateEnd.Text = DateTime.Now.ToShortDateString();
                btnSubmit_Click(null, null);
            }
        }

        public void LoadInvoices()
        {
            _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart);
            _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd);

            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("startDate", SqlDbType.DateTime, _startDate),
                                     new StoredProcedureParameter("endDate", SqlDbType.DateTime, _endDate),
                                     new StoredProcedureParameter("klientId", SqlDbType.Int, _klientId)
                                 };

            _invoicesList = HTBUtils.GetMultipleListsFromStoredProcedure("spGetTransferredToKlient", parameters, new[] { typeof(KlientTransferRecord), typeof(KlientTransferRecord) });
        }

        #region Grids

        private void PopulateDetailGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Asc)
        {

            DataTable dt = GetDataTableStructure();
            ArrayList list = _invoicesList[IdxDetail];
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                list.Sort(new KlientTransferRecordComparer(sortField, sortDirection));

            foreach (KlientTransferRecord rec in list)
                AddDataRow(dt, rec);


            gvDetail.DataSource = dt;
            gvDetail.DataBind();
        }

        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("AktId", typeof(int)));
            dt.Columns.Add(new DataColumn("AktAZ", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("KlientAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("KlientBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("Comment", typeof(string)));
            dt.Columns.Add(new DataColumn("GegnerName", typeof(string)));

            return dt;
        }

        private void AddDataRow(DataTable dt, KlientTransferRecord rec)
        {
            DataRow dr = dt.NewRow();
            dr["AktId"] = rec.AktId;
            dr["AktAZ"] = rec.AktAZ;
            dr["TransferDate"] = rec.TransferDate.ToShortDateString();
            dr["TransferAmount"] = HTBUtils.FormatCurrency(rec.TransferAmount);
            dr["KlientAmount"] = HTBUtils.FormatCurrency(rec.KlientAmount);
            dr["KlientBalance"] = HTBUtils.FormatCurrency(rec.KlientBalance);
            dr["GegnerName"] = rec.GegnerName;
            dr["Comment"] = rec.Comment;
            dt.Rows.Add(dr);
        }
        #endregion

        #region Access
        public string GetTotalAmount()
        {
            return HTBUtils.FormatCurrency(GetTotalAmount(_invoicesList[IdxSummary]));
        }

        public double GetTotalAmount(ArrayList list)
        {
            return list.Cast<KlientTransferRecord>().Sum(rec => rec.TransferAmount);
        }

        #endregion

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                LoadInvoices();
                PopulateDetailGrid();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        protected void btnXL_Click(object sender, EventArgs e)
        {
            LoadInvoices();
//            PopulateDetailGrid();
            EnableViewState = false;

            var ms = new MemoryStream();
            var exporter = new ExcelExporter();

            exporter.WriteExcelFile(ms, _invoicesList[IdxDetail]);
            
            ms.Seek(0, SeekOrigin.Begin);

            var xlData = new StreamReader(ms).ReadToEnd();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Erhebungen.xls");
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(xlData);
            HttpContext.Current.Response.End();
        }

        protected void btnXL2_Click(object sender, EventArgs e)
        {
            LoadInvoices();
            PopulateDetailGrid();
            //GridViewExporter.Export("Erhebungsx.xls", gvDetail);
            GridViewExporter.ExportToExcel(gvDetail, "Erhebung.xls");
        }

        #endregion

        #region Sorting
        protected void gvSummary_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd = GetGridViewSortDirection((GridView)sender, e);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);
        }
        protected void gvDetail_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd = GetGridViewSortDirection((GridView)sender, e);
            PopulateDetailGrid(e.SortExpression, sd);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);
        }

        private void ReloadData()
        {
            LoadInvoices();
        }

        private HTBUtilities.SortDirection GetSortDirection(System.Web.UI.WebControls.SortDirection sortDireciton)
        {
            string mSortDirection = String.Empty;
            switch (sortDireciton)
            {
                case System.Web.UI.WebControls.SortDirection.Descending:
                    return HTBUtilities.SortDirection.Desc;

                default:
                    return HTBUtilities.SortDirection.Asc;
            }
        }

        private HTBUtilities.SortDirection GetGridViewSortDirection(GridView g, GridViewSortEventArgs e)
        {
            string f = e.SortExpression;
            System.Web.UI.WebControls.SortDirection d = e.SortDirection;

            //Check if GridView control has required Attributes
            if (g.Attributes["CurrentSortField"] != null && g.Attributes["CurrentSortDir"] != null)
            {
                if (f == g.Attributes["CurrentSortField"])
                {
                    d = System.Web.UI.WebControls.SortDirection.Descending;
                    if (g.Attributes["CurrentSortDir"] == "ASC")
                    {
                        d = System.Web.UI.WebControls.SortDirection.Ascending;
                    }
                }
            }
            g.Attributes["CurrentSortField"] = f;
            g.Attributes["CurrentSortDir"] = (d == System.Web.UI.WebControls.SortDirection.Ascending ? "DESC" : "ASC");
            switch (d)
            {
                case System.Web.UI.WebControls.SortDirection.Descending:
                    return HTBUtilities.SortDirection.Desc;

                default:
                    return HTBUtilities.SortDirection.Asc;
            }
        }
        #endregion
    }
}