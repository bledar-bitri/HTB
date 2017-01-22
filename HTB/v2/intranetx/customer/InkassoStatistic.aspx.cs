using System;
using System.Linq;
using System.Web.UI.WebControls;
using HTBExtras;
using HTB.Database;
using System.Collections;
using System.Data;
using HTBUtilities;
using HTB.v2.intranetx.util;
using System.Web.UI.DataVisualization.Charting;
using System.Reflection;

namespace HTB.v2.intranetx.customer
{
    public partial class InkassoStatistic : System.Web.UI.Page
    {
        private const int IdxAllCasses = 0;
        private const int IdxCassesInProgress = 1;
        private const int IdxCassesFinished = 2;
        private const int IdxCassesFinishedWithSuccess = 3;
        private const int IdxCassesFinishedWithoutSuccess = 4;

        private DateTime _startDate;
        private DateTime _endDate;
        private ArrayList[] _aktsList;
        private int _totalAkts;
        private double _totalAmount;
        private static int _klientId = -1;
        private tblKlient _klient;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["MM_Klient"] = "13439";
            GlobalUtilArea.GetUserId(Session); // force session validation
            _klientId = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_Klient"]);
            if (_klientId > 0)
            {
                _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientID = " + _klientId, typeof(tblKlient));
                if (_klient != null)
                {
                    lblKlientInfo.Text = "<em> - Klient:&nbsp;" + _klient.KlientName1 + "&nbsp;" + _klient.KlientName2 + "</em>";
                }
            }
            if (!IsPostBack)
            {
                txtDateStart.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                txtDateEnd.Text = DateTime.Now.ToShortDateString();
                btnSubmit_Click(null, null);
            }
        }

        public void LoadAkts()
        {
            _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart);
            _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd);
            
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("startDate", SqlDbType.DateTime, _startDate),
                                     new StoredProcedureParameter("endDate", SqlDbType.DateTime, _endDate),
                                     new StoredProcedureParameter("klientId", SqlDbType.Int, _klientId)
                                 };

            _aktsList = HTBUtils.GetMultipleListsFromStoredProcedure("spGetInkassoKlientStatisticAkten", parameters, new[] { typeof(StatisticRecord), typeof(StatisticRecord), typeof(StatisticRecord), typeof(StatisticRecord), typeof(StatisticRecord) });
            
            ArrayList list = _aktsList[IdxAllCasses];
            
            _totalAkts = 0;
            _totalAmount = 0;
            
            foreach (StatisticRecord rec in list)
            {
                _totalAkts += rec.Akts;
                _totalAmount += rec.Amount;
            }
        }

        #region Grids

        private void PopulateAllCasesGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Asc)
        {

            DataTable dt = GetDataTableStructure();
            ArrayList list = _aktsList[IdxAllCasses];
            CalcPercentages(list);
            
            if (sortField != null)
                list.Sort(new StatisticRecordComparer(sortField, sortDirection));

            foreach (StatisticRecord rec in list)
                AddDataRow(dt, rec);
            
            
            
            gvAllCases.DataSource = dt;
            gvAllCases.DataBind();
        }

        private void PopulateInProgressCasesGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Asc)
        {

            DataTable dt = GetDataTableStructure();
            ArrayList list = _aktsList[IdxCassesInProgress];
            CalcPercentages(list);
            
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                list.Sort(new StatisticRecordComparer(sortField, sortDirection));
            
            foreach (StatisticRecord rec in list)
                AddDataRow(dt, rec);
            

            gvInProgress.DataSource = dt;
            gvInProgress.DataBind();
        }

        private void PopulateFinishedCasesGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Asc)
        {
            DataTable dt = GetDataTableStructure();
            ArrayList list = _aktsList[IdxCassesFinished];
            CalcPercentages(list);
            
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                list.Sort(new StatisticRecordComparer(sortField, sortDirection));
            
            foreach (StatisticRecord rec in list)
                AddDataRow(dt, rec);
            

            gvFinished.DataSource = dt;
            gvFinished.DataBind();
        }

        private void PopulateSuccessfullActions(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Asc)
        {
            DataTable dt = GetDataTableStructure();
            ArrayList list = _aktsList[IdxCassesFinishedWithSuccess];
            CalcPercentages(list);
            
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                list.Sort(new StatisticRecordComparer(sortField, sortDirection));

            foreach (StatisticRecord rec in list)
                AddDataRow(dt, rec);
            

            gvSuccessActions.DataSource = dt;
            gvSuccessActions.DataBind();
        }

        private void PopulateUnSuccessfullActions(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Asc)
        {
            DataTable dt = GetDataTableStructure();
            ArrayList list = _aktsList[IdxCassesFinishedWithoutSuccess];
            CalcPercentages(list);

            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                list.Sort(new StatisticRecordComparer(sortField, sortDirection));

            foreach (StatisticRecord rec in list)
                AddDataRow(dt, rec);
            
            gvUnSuccessActions.DataSource = dt;
            gvUnSuccessActions.DataBind();
        }

        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Akts", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("PercentAkt", typeof(string)));
            dt.Columns.Add(new DataColumn("PercentAmount", typeof(string)));
            return dt;
        }

        private void AddDataRow(DataTable dt, StatisticRecord rec)
        {
            DataRow dr = dt.NewRow();
            dr["Description"] = rec.Description;
            dr["Akts"] = rec.Akts;
            dr["Amount"] = HTBUtils.FormatCurrency(rec.Amount);
            dr["PercentAkt"] = HTBUtils.FormatPercent(rec.PercentAkt);
            dr["PercentAmount"] = HTBUtils.FormatPercent(rec.PercentAmount);
            
            dt.Rows.Add(dr);
        }

        private void CalcPercentages(ArrayList list)
        {
            foreach (StatisticRecord rec in list)
                rec.CalcPercent(_totalAkts, _totalAmount);
        }
        #endregion

        #region Access
        public string GetTotalAkts()
        {
            return _totalAkts.ToString();
        }
        public string GetTotalAmount()
        {
            return HTBUtils.FormatCurrency(_totalAmount);
        }
        
        #region In Progress
        public string GetTotalInProgress()
        {
            return GetTotalAkts(_aktsList[IdxCassesInProgress]).ToString();
        }
        public string GetTotalInProgressPct()
        {
            return GetTotalAktPct(_aktsList[IdxCassesInProgress]);
        }
        public string GetTotalInProgressAmount()
        {
            return HTBUtils.FormatCurrency(GetTotalAmount(_aktsList[IdxCassesInProgress]));
        }
        public string GetTotalInProgressAmountPct()
        {
            return GetTotalAmountPct(_aktsList[IdxCassesInProgress]);
        }
        #endregion
        
        #region In Finished
        public string GetTotalFinished()
        {
            return GetTotalAkts(_aktsList[IdxCassesFinished]).ToString();
        }
        public string GetTotalFinishedPct()
        {
            return GetTotalAktPct(_aktsList[IdxCassesFinished]);
        }
        public string GetTotalFinishedAmount()
        {
            return HTBUtils.FormatCurrency(GetTotalAmount(_aktsList[IdxCassesFinished]));
        }
        public string GetTotalFinishedAmountPct()
        {
            return GetTotalAmountPct(_aktsList[IdxCassesFinished]);
        }
        #endregion

        #region UnSuccessfull
        public string GetTotalUnSuccessfull()
        {
            return GetTotalAkts(_aktsList[IdxCassesFinishedWithoutSuccess]).ToString();
        }
        public string GetTotalUnSuccessfullPct()
        {
            return GetTotalAktPct(_aktsList[IdxCassesFinishedWithoutSuccess]);
        }
        public string GetTotalUnSuccessfullAmount()
        {
            return HTBUtils.FormatCurrency(GetTotalAmount(_aktsList[IdxCassesFinishedWithoutSuccess]));
        }
        public string GetTotalUnSuccessfullAmountPct()
        {
            return GetTotalAmountPct(_aktsList[IdxCassesFinishedWithoutSuccess]);
        }
        #endregion

        #region Successfull
        public string GetTotalSuccessfull()
        {
            return GetTotalAkts(_aktsList[IdxCassesFinishedWithSuccess]).ToString();
        }
        public string GetTotalSuccessfullPct()
        {
            return GetTotalAktPct(_aktsList[IdxCassesFinishedWithSuccess]);
        }
        public string GetTotalSuccessfullAmount()
        {
            return HTBUtils.FormatCurrency(GetTotalAmount(_aktsList[IdxCassesFinishedWithSuccess]));
        }
        public string GetTotalSuccessfullAmountPct()
        {
            return GetTotalAmountPct(_aktsList[IdxCassesFinishedWithSuccess]);
        }
        #endregion

        public string GetTotalAktPct(ArrayList list)
        {
            int akts = GetTotalAkts(list);
            if (akts > 0 && _totalAkts > 0)
                return HTBUtils.FormatPercent(akts / (double)_totalAkts);
            return HTBUtils.FormatPercent(0);
        }
        public string GetTotalAmountPct(ArrayList list)
        {
            double amount = GetTotalAmount(list);
            if (amount > 0 && _totalAmount > 0)
                return HTBUtils.FormatPercent(amount / _totalAmount);
            return HTBUtils.FormatPercent(0);
        }
        public int GetTotalAkts(ArrayList list)
        {
            return list.Cast<StatisticRecord>().Sum(rec => rec.Akts);
        }

        public double GetTotalAmount(ArrayList list)
        {
            return list.Cast<StatisticRecord>().Sum(rec => rec.Amount);
        }

        #endregion

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                LoadAkts();
                PopulateAllCasesGrid();
                PopulateInProgressCasesGrid();
                PopulateFinishedCasesGrid();
                PopulateSuccessfullActions();
                PopulateUnSuccessfullActions();
                LoadCharts();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        #endregion

        #region Sorting
        protected void gvAllCases_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd =  GetGridViewSortDirection((GridView)sender, e);
            PopulateAllCasesGrid(e.SortExpression, sd);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);
        }
        protected void gvInProgress_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd = GetGridViewSortDirection((GridView)sender, e);
            PopulateInProgressCasesGrid(e.SortExpression, sd);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);
        }
        protected void gvFinished_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd = GetGridViewSortDirection((GridView)sender, e);
            PopulateFinishedCasesGrid(e.SortExpression, sd);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);
        }
        protected void gvSuccessActions_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd = GetGridViewSortDirection((GridView)sender, e);
            PopulateSuccessfullActions(e.SortExpression, sd);
            BindChartData(chrtSuccess, _aktsList[IdxCassesFinishedWithSuccess]);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);

        }
        protected void gvUnSuccessActions_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd = GetGridViewSortDirection((GridView)sender, e);
            PopulateUnSuccessfullActions(e.SortExpression, sd);
            BindChartData(chrtUnSuccess, _aktsList[IdxCassesFinishedWithoutSuccess]);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);
        }

        private void ReloadData()
        {
            LoadAkts();            
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

        #region Charts
        private void LoadCharts()
        {
            BindChartData(chrtSuccess, _aktsList[IdxCassesFinishedWithSuccess]);
            BindChartData(chrtUnSuccess, _aktsList[IdxCassesFinishedWithoutSuccess]);
        }

        private void BindChartData(Chart chrt, ArrayList list)
        {
            chrt.Visible = true;

            if (list == null || list.Count == 0)
                chrt.Visible = false;

            if (list != null)
            {
                var xval1 = new string[list.Count];
                var yval1 = new int[list.Count];
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var rec = (StatisticRecord)list[i];
                    xval1[list.Count - i - 1] = rec.Description.Length > 25 ? rec.Description.Substring(0,25) : rec.Description;
                    yval1[list.Count - i - 1] = rec.Akts;
                }
                chrt.Series["Series1"].Points.DataBindXY(xval1, yval1);
                chrt.Series["Series1"].ChartType = SeriesChartType.Bar;
            }
        }
        #endregion
    }
}