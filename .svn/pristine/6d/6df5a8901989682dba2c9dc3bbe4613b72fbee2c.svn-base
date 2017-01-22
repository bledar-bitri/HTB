using System;
using System.Linq;
using System.Text;
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
    public partial class CollectedByAussendienst : System.Web.UI.Page
    {
        private const int IdxDetail = 0;
        private const int IdxSummary = 1;
        
        private DateTime _startDate;
        private DateTime _endDate;
        private ArrayList _collectedList = new ArrayList();
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
            if (_agId > 0)
            {
                _ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + _agId, typeof(tblAuftraggeber));
                _user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + _userId, typeof(tblUser));
                if (_ag != null && _user != null)
                {
                    lblAGInfo.Text = "f&uuml;r " + _ag.AuftraggeberName1 + "&nbsp;" + _ag.AuftraggeberName2;
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
        }

        public void LoadCollected()
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

            _collectedList = HTBUtils.GetStoredProcedureRecords("spGetCollectedByAussendienst", parameters, typeof(AussendienstCollectedRecord));
            foreach (AussendienstCollectedRecord rec in _collectedList)
            {
                var sb = new StringBuilder();
                if (rec.GegnerName1 != null && rec.GegnerName1.Trim() != string.Empty)
                    sb.Append(rec.GegnerName1 + "<br/>");
                if (rec.GegnerName2 != null && rec.GegnerName2.Trim() != string.Empty)
                    sb.Append(rec.GegnerName2 + "<br/>");
                if (rec.GegnerName3 != null && rec.GegnerName3.Trim() != string.Empty)
                    sb.Append(rec.GegnerName3 + "<br/>");
                rec.GegnerName = sb.ToString();
            }
        }

        #region Grids

        private void PopulateDetailGrid(string sortField = null, HTBUtilities.SortDirection sortDirection = HTBUtilities.SortDirection.Asc)
        {

            DataTable dt = GetDataTableStructure();
            ArrayList list = _collectedList;
            if (GlobalUtilArea.GetEmptyIfNull(sortField) != string.Empty)
                list.Sort(new AussendienstCollectedRecordComparer(sortField, sortDirection));

            foreach (AussendienstCollectedRecord rec in list)
                AddDataRow(dt, rec);
            

            gvDetail.DataSource = dt;
            gvDetail.DataBind();
        }

        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("AktId", typeof(int)));
            dt.Columns.Add(new DataColumn("AktAZ", typeof(string)));
            dt.Columns.Add(new DataColumn("CollectedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("CollectedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("CollectedBillNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("Comment", typeof(string)));
            dt.Columns.Add(new DataColumn("GegnerName", typeof(string)));
            dt.Columns.Add(new DataColumn("CollectorName", typeof(string)));
            
            return dt;
        }

        private void AddDataRow(DataTable dt, AussendienstCollectedRecord rec)
        {
            DataRow dr = dt.NewRow();
            dr["AktId"] = rec.AktId;
            dr["AktAZ"] = rec.AktAZ;
            dr["CollectedDate"] = rec.CollectedDate.ToShortDateString();
            dr["CollectedAmount"] = HTBUtils.FormatCurrency(rec.CollectedAmount);
            dr["CollectedBillNumber"] = rec.CollectedBillNumber;
            dr["GegnerName"] = rec.GegnerName;
            dr["CollectorName"] = rec.CollectorName;
            dr["Comment"] = rec.Comment;
            dt.Rows.Add(dr);
        }
        #endregion

        #region Access
        public string GetTotalAmount()
        {
            return HTBUtils.FormatCurrency(GetTotalAmount(_collectedList));
        }
        
        public double GetTotalAmount(ArrayList list)
        {
            return list.Cast<AussendienstCollectedRecord>().Sum(rec => rec.CollectedAmount);
        }

        #endregion

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ctlMessage.Clear();
                LoadCollected();
                PopulateDetailGrid();
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
            LoadCollected();            
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