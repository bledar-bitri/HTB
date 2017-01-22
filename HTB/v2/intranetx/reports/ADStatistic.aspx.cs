using System;
using System.Linq;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.permissions;
using HTBExtras;
using HTB.Database;
using System.Collections;
using System.Data;
using HTBUtilities;
using HTB.v2.intranetx.util;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using System.Reflection;

namespace HTB.v2.intranetx.reports
{
    public partial class ADStatistic : System.Web.UI.Page
    {
        private const string CasesInProgress = "Aufträge in Bearbeitung";
        private const string CasesDone = "Aufträge erledigt";
        private const string CasesNew = "Aufträge neu erfasst";
        private const string CasesReturned = "Aufträge vom Aussendienst abgegeben";
        private const string CasesSuccessfull = "Aufträge erfolgreich erledigt";
        private const string CasesUnSuccessfull = "Aufträge erfolglos erledigt";

        private DateTime _startDate;
        private DateTime _endDate;
        private ArrayList _aktsList = new ArrayList();
        private int _totalAkts;
        private int _totalFinished;
        private int _totalSuccess;
        private int _totalUnSuccess;
        
        private ArrayList _actionDistributionsSuccess = new ArrayList();
        private ArrayList _actionDistributionsUnSuccess = new ArrayList();
        private ArrayList _inProgressList = new ArrayList();
        private ArrayList _finishedList = new ArrayList();
        private readonly PermissionsADStatistic _permissions = new PermissionsADStatistic();

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
//            Session["MM_AG"] = "6";
//            Session["MM_UserID"] = "99";
            _permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            
            if (IsPostBack) return;
            GlobalUtilArea.LoadUserDropdownList(ddlSB, GlobalUtilArea.GetUsers(Session));

            txtDateStart.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            txtDateEnd.Text = DateTime.Now.ToShortDateString();
            btnSubmit_Click(null, null);
        }

        public void LoadAllAkts()
        {
            _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart);
            _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd);
            String sql = "SELECT * FROM tblAktenInt WHERE AktIntSB = " + ddlSB.SelectedValue + " AND AktIntDatum BETWEEN '" + _startDate.ToShortDateString() + "' AND '" + _endDate.ToShortDateString() + "'";
            _aktsList = HTBUtils.GetSqlRecords(sql, typeof(tblAktenInt));
            _totalAkts = _aktsList.Count;
        }

        #region Grids

        private void PopulateAllCasesGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Desc)
        {

            DataTable dt = GetDataTableStructure();
            StatisticRecord inProgress = GetAllCasesInProgress();
            if (sortField != null)
            {
                var list = new ArrayList {inProgress, GetAllCasesFinished(inProgress.Akts)};
                list.Sort(new StatisticRecordComparer(sortField, sortDirection));
                foreach (StatisticRecord rec in list)
                {
                    AddDataRow(dt, rec);
                }
            }
            else
            {
                AddDataRow(dt, inProgress);
                AddDataRow(dt, GetAllCasesFinished(inProgress.Akts));
            }
            gvAllCases.DataSource = dt;
            gvAllCases.DataBind();
        }

        private void PopulateInProgressCasesGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Desc)
        {
            DataTable dt = GetDataTableStructure();
            _inProgressList = GetInProgressCases();
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                _inProgressList.Sort(new StatisticRecordComparer(sortField, sortDirection));
            foreach (StatisticRecord rec in _inProgressList)
                AddDataRow(dt, rec);

            gvInProgress.DataSource = dt;
            gvInProgress.DataBind();
        }

        private void PopulateFinishedCasesGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Desc)
        {
            _finishedList = GetFinishedCases();
            DataTable dt = GetDataTableStructure();
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                _finishedList.Sort(new StatisticRecordComparer(sortField, sortDirection));
            foreach (StatisticRecord rec in _finishedList)
            {
                rec.CalcPercentAkt(_totalAkts);
                AddDataRow(dt, rec);
            }

            gvFinished.DataSource = dt;
            gvFinished.DataBind();
        }

        private void PopulateSuccessfullActions(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Desc)
        {
            LoadActionDistributions(true);
            DataTable dt = GetDataTableStructure();
            
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                _actionDistributionsSuccess.Sort(new StatisticRecordComparer(sortField, sortDirection));

            foreach (StatisticRecord rec in _actionDistributionsSuccess)
            {
                rec.CalcPercentAkt(_totalAkts);
                AddDataRow(dt, rec);
            }

            gvSuccessActions.DataSource = dt;
            gvSuccessActions.DataBind();
        }

        private void PopulateUnSuccessfullActions(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Desc)
        {
            LoadActionDistributions(false);
            DataTable dt = GetDataTableStructure();
            
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                _actionDistributionsUnSuccess.Sort(new StatisticRecordComparer(sortField, sortDirection));

            foreach (StatisticRecord rec in _actionDistributionsUnSuccess)
            {
                rec.CalcPercentAkt(_totalAkts);
                AddDataRow(dt, rec);
            }

            gvUnSuccessActions.DataSource = dt;
            gvUnSuccessActions.DataBind();
        }

        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Akts", typeof(string)));
            dt.Columns.Add(new DataColumn("Percent", typeof(string)));
            return dt;
        }

        private void AddDataRow(DataTable dt, StatisticRecord rec)
        {
            DataRow dr = dt.NewRow();
            dr["Description"] = rec.Description;
            dr["Akts"] = rec.Akts;
            dr["Percent"] = HTBUtils.FormatPercent(rec.PercentAkt);
            dt.Rows.Add(dr);
        }

        private string GetAktIdList(bool onlyFinished)
        {
            var sb = new StringBuilder();
            foreach (tblAktenInt akt in _aktsList)
            {
                if (onlyFinished)
                {
                    if (akt.AktIntStatus >= 3 && akt.AktIntStatus <= 5)
                    {
                        sb.Append(akt.AktIntID);
                        sb.Append(",");
                    }
                }
                else
                {
                    sb.Append(akt.AktIntID);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Access
        private StatisticRecord GetAllCasesInProgress()
        {
            var rec = new StatisticRecord {Description = CasesInProgress};
            foreach (tblAktenInt akt in _aktsList)
            {
                switch ((int)akt.AktIntStatus)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        rec.Akts++;
                        break;

                }
            }
            rec.CalcPercentAkt(_totalAkts);
            return rec;
        }
        private StatisticRecord GetAllCasesFinished(int inProgressAkts)
        {
            var rec = new StatisticRecord {Description = CasesDone, Akts = _totalAkts - inProgressAkts};
            rec.CalcPercentAkt(_totalAkts);
            _totalFinished = rec.Akts;
            return rec;
        }
        private ArrayList GetInProgressCases()
        {
            var list = new ArrayList();
            var recNew = new StatisticRecord(CasesNew);
            var recInProgress = new StatisticRecord(CasesInProgress);
            var recGotBackFromAd = new StatisticRecord(CasesReturned);
            foreach (tblAktenInt akt in _aktsList)
            {
                switch ((int)akt.AktIntStatus)
                {
                    case 0:
                        recNew.Akts++;
                        break;
                    case 2:
                        recGotBackFromAd.Akts++;
                        break;
                    case 1:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        recInProgress.Akts++;
                        break;

                }
            }
            recNew.CalcPercentAkt(_totalAkts);
            recInProgress.CalcPercentAkt(_totalAkts);
            recGotBackFromAd.CalcPercentAkt(_totalAkts);

            list.Add(recNew);
            list.Add(recInProgress);
            list.Add(recGotBackFromAd);
            return list;
        }
        private ArrayList GetFinishedCases()
        {
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("startDate", SqlDbType.Int, _startDate),
                                     new StoredProcedureParameter("endDate", SqlDbType.Int, _endDate),
                                     new StoredProcedureParameter("sbId", SqlDbType.VarChar, ddlSB.SelectedValue)
                                 };

            ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetSbStatisticUnSuccessfullAkten", parameters, typeof(StatisticRecord));
            ((StatisticRecord)list[0]).Description = CasesUnSuccessfull;
            ((StatisticRecord)list[0]).CalcPercentAkt(_totalAkts);

            _totalUnSuccess = ((StatisticRecord)list[0]).Akts;
            _totalSuccess = _totalFinished - _totalUnSuccess;
            if(_totalSuccess < 0)
                _totalSuccess = 0;


            var rec = new StatisticRecord {Description = CasesSuccessfull, Akts = _totalSuccess};
            rec.CalcPercentAkt(_totalAkts);
            list.Add(rec);

            return list;
        }
        private void LoadActionDistributions(bool isPositive)
        {
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("startDate", SqlDbType.Int, _startDate),
                                     new StoredProcedureParameter("endDate", SqlDbType.Int, _endDate),
                                     new StoredProcedureParameter("isPostive", SqlDbType.Bit, isPositive),
                                     new StoredProcedureParameter("sbId", SqlDbType.VarChar, ddlSB.SelectedValue)
                                 };

            ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetSbStatisticDistribution", parameters, typeof(StatisticRecord));
            foreach (StatisticRecord rec in list)
                rec.CalcPercentAkt(_totalAkts);

            if (isPositive)
                _actionDistributionsSuccess = list; // keep track of the lists
            else
                _actionDistributionsUnSuccess = list;
        }

        public string GetTotalAkts()
        {
            return _totalAkts.ToString();
        }
        public string GetTotalInProgress()
        {
            return GetTotalAkts(_inProgressList).ToString();
        }
        public string GetTotalInProgressPct()
        {
            return GetTotalAktPct(_inProgressList);
        }
        public string GetTotalFinished()
        {
            return GetTotalAkts(_finishedList).ToString();
        }
        public string GetTotalFinishedPct()
        {
            return GetTotalAktPct(_finishedList);
        }

        public string GetTotalUnSuccessfull()
        {
            return GetTotalAkts(_actionDistributionsUnSuccess).ToString();
        }
        public string GetTotalUnSuccessfullPct()
        {
            return GetTotalAktPct(_actionDistributionsUnSuccess);
        }

        public string GetTotalSuccessfull()
        {
            return GetTotalAkts(_actionDistributionsSuccess).ToString();
        }
        public string GetTotalSuccessfullPct()
        {
            return GetTotalAktPct(_actionDistributionsSuccess);
        }

        private int GetTotalAkts(ArrayList list)
        {
            return list.Cast<StatisticRecord>().Sum(rec => rec.Akts);
        }

        public string GetTotalAktPct(ArrayList list)
        {
            int akts = GetTotalAkts(list);
            if (akts > 0 && _totalAkts > 0)
                return HTBUtils.FormatPercent(akts / (double)_totalAkts);
            return HTBUtils.FormatPercent(0);
        }
        
        #endregion

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                LoadAllAkts();
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
                try
                {
                    ctlMessage.AppendError("<br/>" + GetAktIdList(true) + "<br/>&nbsp;");
                }
                catch
                { }
            }
            GlobalUtilArea.LoadUserDropdownList(ddlSB, GlobalUtilArea.GetUsers(Session));
        }

        protected void DdlSbSelectedIndexChanged(object sender, EventArgs e)
        {
            btnSubmit_Click(sender, e);
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
            BindChartData(chrtSuccess, _actionDistributionsSuccess);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);

        }
        protected void gvUnSuccessActions_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd = GetGridViewSortDirection((GridView)sender, e);
            PopulateUnSuccessfullActions(e.SortExpression, sd);
            BindChartData(chrtUnSuccess, _actionDistributionsUnSuccess);
            GlobalUtilArea.AddSortArrow((GridView)sender, sd, e.SortExpression);
        }

        private void ReloadData()
        {
            LoadAllAkts();
            StatisticRecord inProgressRec = GetAllCasesInProgress();
            GetAllCasesFinished(inProgressRec.Akts);
            GetFinishedCases();
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
                    return HTBUtilities.SortDirection.Asc;

                default:
                    return HTBUtilities.SortDirection.Desc;
            }
        }
        #endregion

        #region Charts
        private void LoadCharts()
        {
            BindChartData(chrtSuccess, _actionDistributionsSuccess);
            BindChartData(chrtUnSuccess, _actionDistributionsUnSuccess);
        }

        private void BindChartData(Chart chrt, ArrayList list)
        {
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