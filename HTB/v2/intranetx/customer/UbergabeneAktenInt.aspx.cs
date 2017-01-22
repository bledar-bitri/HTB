using System;
using System.Linq;
using System.Web.UI.WebControls;
using HTBExtras;
using HTB.Database;
using System.Collections;
using System.Data;
using HTBUtilities;
using HTB.v2.intranetx.util;
using System.Reflection;

namespace HTB.v2.intranetx.customer
{
    public partial class UbergabeneAktenInt : System.Web.UI.Page
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private readonly UbergebeneAktenRecordList _list = new UbergebeneAktenRecordList();
        private int _agId = -1;
        private tblAuftraggeber _ag;
        private int _userId;
        private tblUser _user;
        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
//            Session["MM_AG"] = "6";
//            Session["MM_UserID"] = "447";
            _userId = GlobalUtilArea.GetUserId(Session); 
            _agId = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_AG"]);
            if (_agId > 0 && _userId > 0)
            {
                _ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + _agId, typeof(tblAuftraggeber));
                _user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + _userId, typeof(tblUser));
                if (_ag != null && _user != null)
                {
                    lblAGInfo.Text = "von " + _ag.AuftraggeberName1 + "&nbsp;" + _ag.AuftraggeberName2;
                    if (!_user.UserIsSbAdmin)
                        lblAGInfo.Text += " [" + _user.UserVorname + " " + _user.UserNachname + "]";
                }
            }
            if (!IsPostBack)
            {
                txtDateStart.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                txtDateEnd.Text = DateTime.Now.ToShortDateString();
                btnSubmit_Click(null, null);
            }
            chrChart.AgId = _agId;
            chrChart.User = _user;
            chrChart.Generate();
        }

        private void LoadUbergebeneAkten()
        {
            _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateStart);
            _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateEnd);
            
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("startDate", SqlDbType.DateTime, _startDate),
                                     new StoredProcedureParameter("endDate", SqlDbType.DateTime, _endDate),
                                     new StoredProcedureParameter("agId", SqlDbType.Int, _agId)
                                 };
            if (_user != null)
            {
                if (!_user.UserIsSbAdmin)
                    parameters.Add(new StoredProcedureParameter("agSbEmail", SqlDbType.VarChar, _user.UserEMailOffice));
            }
            else
            {
                parameters.Add(new StoredProcedureParameter("agSbEmail", SqlDbType.VarChar, "NOT_VALID_USER")); // the database should not return anything
            }
            _list.Clear();
            var aktenList = HTBUtils.GetStoredProcedureRecords("spAGUbergebeneAkten", parameters, typeof(AktIntShortRecord));
            foreach (AktIntShortRecord akt in aktenList)
                _list.Incr(akt);
            
        }

        #region Grids

        private void PopulateDetailGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Asc)
        {

            DataTable dt = GetDataTableStructure();
            ArrayList list = _list;
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                list.Sort(new UbergebeneAktenRecordComparer(sortField, sortDirection));

            foreach (UbergebeneAktenRecord rec in list)
                AddDataRow(dt, rec);
            

            gvDetail.DataSource = dt;
            gvDetail.DataBind();
        }

        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("AgSbName", typeof(string)));
            dt.Columns.Add(new DataColumn("InterventionAkten", typeof(string)));
            dt.Columns.Add(new DataColumn("DubAkten", typeof(string)));
            dt.Columns.Add(new DataColumn("TestAkten", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalAkten", typeof(string)));
            
            return dt;
        }

        private void AddDataRow(DataTable dt, UbergebeneAktenRecord rec)
        {
            DataRow dr = dt.NewRow();
            dr["AgSbName"] = rec.AgSbName;
            dr["InterventionAkten"] = rec.InterventionAkten;
            dr["DubAkten"] = rec.DubAkten;
            dr["TestAkten"] = rec.TestAkten;
            dr["TotalAkten"] = rec.TotalAkten;
            dt.Rows.Add(dr);
        }
        #endregion

        #region Access

        protected string GetTotalAkten()
        {
            return _list.Cast<UbergebeneAktenRecord>().Sum(rec => rec.TotalAkten).ToString();
        }

        protected string GetTotalIntAkten()
        {
            return _list.Cast<UbergebeneAktenRecord>().Sum(rec => rec.InterventionAkten).ToString();
        }

        protected string GetTotalDupAkten()
        {
            return _list.Cast<UbergebeneAktenRecord>().Sum(rec => rec.DubAkten).ToString();
        }
        protected string GetTotalTestAkten()
        {
            return _list.Cast<UbergebeneAktenRecord>().Sum(rec => rec.TestAkten).ToString();
        }
        
        #endregion

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                LoadUbergebeneAkten();
                PopulateDetailGrid();
                chrChart.Generate();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
        #endregion

        #region Sorting
        protected void gvSummary_Sorting(object sender, GridViewSortEventArgs e)
        {
            ReloadData();
            HTBUtilities.SortDirection sd =  GetGridViewSortDirection((GridView)sender, e);
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
            LoadUbergebeneAkten();            
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