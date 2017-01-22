using System;
using System.Collections;
using System.Data;
using System.Text;
using HTB.Database;
using HTB.Database.HTB.StoredProcs;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.bank
{
    public partial class BankInkassoProvisionToTransfer : System.Web.UI.Page
    {
        private DateTime _startDate;
        private DateTime _endDate;

        private double _totalProvision;

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            if(!IsPostBack)
            {
                _startDate = GlobalUtilArea.GetDateAtTime(HTBUtils.GetFirstDayOfMonth(), "00:00");
                _endDate = GlobalUtilArea.GetDateAtTime(HTBUtils.GetLastDayOfMonth(), "23:59");
                txtDateFrom.Text = _startDate.ToShortDateString();
                txtDateTo.Text = _endDate.ToShortDateString();

                PopulteGrids();
            }
        }
        
        #region Event Handlers
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/v2/intranetx/bank/Bank.aspx");
        }
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                _startDate = GlobalUtilArea.GetDateAtTime(_startDate, "00:00");
                _endDate = GlobalUtilArea.GetDateAtTime(_endDate, "23:59");

                PopulteGrids();
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("/v2/intranetx/bank/BankAccountMonthlyStatus.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void PopulteGrids()
        {
            var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("startDate", SqlDbType.DateTime, _startDate),
                                        new StoredProcedureParameter("endDate", SqlDbType.DateTime, _endDate)
                                    };

            ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spGetInkassoMonthlyBankStatus", parameters, new Type[] { typeof(spGetInkassoTransactions), typeof(spGetInkassoProvision), typeof(spGetInkassoProvision) });
            PopulateProvisionDetailGrid(lists[1]);
            PopulateProvisionSummaryGrid(lists[2]);
        }

        #region Provision Grids
        private void PopulateProvisionSummaryGrid(ArrayList list)
        {
            DataTable dt = GetProvisionDataTableStructure();

            _totalProvision = 0;

            foreach (spGetInkassoProvision rec in list)
            {
                DataRow dr = dt.NewRow();

                dr["UserName"] = rec.UserVorname + " " + rec.UserNachname;
                dr["AktIntActionProvision"] = HTBUtils.FormatCurrency(rec.AktIntActionProvision);
                dt.Rows.Add(dr);

                _totalProvision += rec.AktIntActionProvision;
            }
            gvProvisionSummary.DataSource = dt;
            gvProvisionSummary.DataBind();
        }
        private void PopulateProvisionDetailGrid(ArrayList list)
        {
            DataTable dt = GetProvisionDataTableStructure();

            foreach (spGetInkassoProvision rec in list)
            {
                DataRow dr = dt.NewRow();
                
                dr["UserName"] = rec.UserVorname + " " + rec.UserNachname;
                dr["CustInkAktID"] = rec.CustInkAktID.ToString();
                dr["AktIntID"] = rec.AktIntID.ToString();
                dr["AktIntActionDate"] = rec.AktIntActionDate.ToShortDateString();
                dr["AktIntActionProvision"] = HTBUtils.FormatCurrency(rec.AktIntActionProvision);
                dt.Rows.Add(dr);
            }
            gvProvisionDetail.DataSource = dt;
            gvProvisionDetail.DataBind();
        }
        private DataTable GetProvisionDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("UserName", typeof(string)));
            dt.Columns.Add(new DataColumn("CustInkAktID", typeof(string)));
            dt.Columns.Add(new DataColumn("AktIntID", typeof(string)));
            dt.Columns.Add(new DataColumn("AktIntActionDate", typeof(string)));
            dt.Columns.Add(new DataColumn("AktIntActionProvision", typeof(string)));
            return dt;
        }
        protected string GetTotalProvision()
        {
            return HTBUtils.FormatCurrency(_totalProvision);
        }
        #endregion

        private bool IsFormValid(bool validateEndDate = true)
        {
            _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateFrom);
            if (!HTBUtils.IsDateValid(_startDate))
            {
                ctlMessage.ShowError("Datum [vom] ist ung&uuml;ltig!");
                return false;
            }
            
            if (validateEndDate)
            {
                _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateTo);
                if (!HTBUtils.IsDateValid(_endDate))
                {
                    ctlMessage.ShowError("Datum [bis] ist ung&uuml;ltig!");
                    return false;
                }
            }
            return true;
        }

        private string GetRedIfNegative(double amount)
        {
            return (amount < 0) ? "<font color=\"red\">" + HTBUtils.FormatCurrency(amount) + "</font>" : HTBUtils.FormatCurrency(amount);
        }
    }
}