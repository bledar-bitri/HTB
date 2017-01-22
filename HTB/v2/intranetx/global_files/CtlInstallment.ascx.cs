using System;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;
using System.Collections;
using System.Data;
using System.Drawing;
using Microsoft.VisualBasic;
using HTB.v2.intranetx.util;
using HTBAktLayer;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlInstallment : System.Web.UI.UserControl
    {
        public string KZID = "";
        public int aktId;
        public int intAktId;
        public tblCustInkAkt custInkAkt;
        public tblKZ kz;
        public double originalAmount;
        public double totalAmount;
        public double totPaid;
        public double balance;
        public string btnCancelText = "Abbrechen";

        private static readonly tblControl Control = HTBUtils.GetControlRecord();
        
        private ArrayList installmentList = new ArrayList();
        private AktUtils aktUtils;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "&nbsp;";
            intAktId = 0;
            aktId = 0;
            if ((Request["ID"] != null && !Request["ID"].Equals("")) || (Request["INTID"] != null && !Request["INTID"].Equals("")))
            {
                try
                {
                    aktId = Convert.ToInt32(Request["ID"]);
                }
                catch { }
                KZID = "10";
                if (Request["KZID"] != null)
                    KZID = Request["KZID"];
                try
                {
                    if (Request["INTID"] != null)
                        intAktId = Convert.ToInt32(Request["INTID"]);
                }
                catch { }
                LoadAll();
            }
        }

        public void LoadAll()
        {
            LoadRecords();
            if (custInkAkt != null)
            {
                aktUtils = new AktUtils(custInkAkt.CustInkAktID);
                originalAmount = aktUtils.GetAktOriginalInvoiceAmount();
                totalAmount = aktUtils.GetAktTotalInvoiceAmount();
                totPaid = aktUtils.GetAktTotalPayments() + GetCurrentPayment();
                balance = aktUtils.GetAktBalance() - GetCurrentPayment();
                if (!IsPostBack)
                {
                    SetValues();
                    PopulateSavedInstallmentsGrid();
                }
            }
        }
        private void SetValues()
        {
            lblHeader.Text = "Ratenvereinbarung";
            txtInterestPct.Text = Control.AnnualInterestRate.ToString();
            lblInterestPct.Text = Control.AnnualInterestRate.ToString();
                
            txtStartDate.Text = DateTime.Now.ToShortDateString();
            lblTotalPaid.Text = HTBUtils.FormatCurrency(totPaid);
            lblOriginalAmount.Text = HTBUtils.FormatCurrency(originalAmount);
            lblBalance.Text = HTBUtils.FormatCurrency(balance);
        }
        public void RefreshScreen()
        {
            SetValues();
        }
        private void LoadRecords()
        {
            kz = (tblKZ)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKZ WHERE KZID = " + KZID.Replace("'", "''"), typeof(tblKZ));
            if (intAktId > 0)
            {
                tblAktenInt intAkt = HTBUtils.GetInterventionAkt(intAktId);
                if (intAkt != null && intAkt.IsInkasso())
                {
                    custInkAkt = HTBUtils.GetInkassoAkt(intAkt.AktIntCustInkAktID);
                }
            }
            else
            {
                custInkAkt = HTBUtils.GetInkassoAkt(aktId);
            }
        }
        protected void btnChangeInterestPct_Clicked(object sender, EventArgs e)
        {
            txtInterestPct.Visible = true;
            lblInterestPct.Visible = false;
            btnChangeInterestPct.Visible = false;
        }
        protected void calcRVNoRates_Click(object sender, EventArgs e)
        {
            int interestPeriod = GetInterestPeriod();
            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            int numberOfInstallments = GlobalUtilArea.GetZeroIfConvertToIntError(txtNumberOfInstallments.Text);
            if (numberOfInstallments > 0)
            {
                try
                {
                    double annualInterestRate = (txtInterestPct.Visible ? GlobalUtilArea.GetZeroIfConvertToDoubleError(txtInterestPct.Text) : Control.AnnualInterestRate);
                    annualInterestRate /= 100;
                    double installmentAmount = Microsoft.VisualBasic.Financial.Pmt(annualInterestRate / interestPeriod, numberOfInstallments, -balance);
                    txtInstallmentAmount.Text = installmentAmount.ToString("N2");
                    double lastInstallment = Microsoft.VisualBasic.Financial.PPmt(annualInterestRate / interestPeriod, numberOfInstallments, numberOfInstallments, -balance);
                    lblLastInstallment.Text = HTBUtils.FormatCurrency(lastInstallment);

                    DateTime endDate = startDate.AddMonths(numberOfInstallments);
                    lblEndDatum.Text = endDate.ToShortDateString();
                    lblTotalInterest.Text = HTBUtils.FormatCurrency(((installmentAmount * (numberOfInstallments - 1)) + lastInstallment - balance));
                    LoadInstallmentsList(numberOfInstallments, installmentAmount, lastInstallment);
                    PopulateInstallmentsGrid();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message + "<br/>" + ex.StackTrace);
                }
            }
            else
            {
                ShowError("<i><strong>Anzahl der Raten</strong></i> muss grosser als 0 (null) sein");
            }
        }

        protected void btnCalcRVRate_Click(object sender, EventArgs e)
        {
            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            int interestPeriod = GetInterestPeriod();
            double installmentAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtInstallmentAmount.Text);
            try
            {
                var annualInterestRate = (txtInterestPct.Visible
                    ? GlobalUtilArea.GetZeroIfConvertToDoubleError(txtInterestPct.Text)
                    : Control.AnnualInterestRate);
                annualInterestRate /= 100;
                int numberOfInstallments;
                double latestInstallment;
                double totalInterest;

                HTBUtils.CalculateInstallmentPlanBasedOnPaymentAmount(balance, installmentAmount, annualInterestRate, interestPeriod, out numberOfInstallments, out latestInstallment, out totalInterest);
                txtNumberOfInstallments.Text = numberOfInstallments.ToString();
                lblLastInstallment.Text = HTBUtils.FormatCurrency(latestInstallment);
                lblTotalInterest.Text = HTBUtils.FormatCurrency(totalInterest);
                LoadInstallmentsList(numberOfInstallments, installmentAmount, latestInstallment);
                PopulateInstallmentsGrid();

                DateTime endDate = startDate.AddMonths(numberOfInstallments);
                lblEndDatum.Text = endDate.ToShortDateString();
//                calcRVNoRates_Click(sender, e);
            }
            catch
            {
                ShowError(
                    "<i><strong>Anzahl der Raten</strong></i> kann nich berechnet werden da die Zinsen h&ouml;er sind als die Ratenh&ouml;he.");
            }
        }

        private int GetInterestPeriod()
        {
            if (ddlInstallmentPeriod.SelectedValue.ToLower().Equals("weekly"))
            {
                return 52;
            }
            else
            {
                return 12;
            }
        }

        private void ShowError(String message)
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = message;
        }

        private void ShowSuccess(String message)
        {
            lblMessage.ForeColor = Color.Green;
            lblMessage.Text = message;
        }

        private void LoadInstallmentsList(int numberOfInstallments, double normalAmount, double lastAmount)
        {
            installmentList.Clear();
            DateTime date = Convert.ToDateTime(txtStartDate.Text);
            for (int i = 0; i < numberOfInstallments; i++)
            {
                var installment = new tblCustInkAktRate
                                      {
                                          CustInkAktRateAmount = i < numberOfInstallments - 1 ? normalAmount : lastAmount, 
                                          CustInkAktRateAktID = custInkAkt.CustInkAktID, 
                                          CustInkAktRateDate = DateTime.Now, 
                                          CustInkAktRateDueDate = date
                                      };

                installmentList.Add(installment);

                date = ddlInstallmentPeriod.SelectedValue.ToLower().Equals("weekly") ? date.AddDays(7) : date.AddMonths(1);
            }
            Session["InstallmentList"] = installmentList;
        }

        private void PopulateInstallmentsGrid()
        {
            DataTable dt = GetGridDataTableStructure();
            foreach (tblCustInkAktRate installment in installmentList)
            {
                DataRow dr = dt.NewRow();
                dr["InstallmentDate"] = installment.CustInkAktRateDueDate.ToShortDateString();
                dr["InstallmentAmount"] = HTBUtils.FormatCurrency(installment.CustInkAktRateAmount);
                dt.Rows.Add(dr);
            }
            gvInstallments.DataSource = dt;
            gvInstallments.DataBind();
        }

        private DataTable GetGridDataTableStructure()
        {
            DataTable dt = new DataTable();
            DataColumn dc;

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "InstallmentDate";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "InstallmentAmount";
            dt.Columns.Add(dc);

            return dt;
        }

        public void ShowPersonalCollection()
        {
            ddlPaymentType.Items.Clear();
            ddlPaymentType.Items.Add(new ListItem("Persönliches CollectionInvoice", "1"));
        }

        public void ShowErlagschein()
        {
            ddlPaymentType.Items.Clear();
            ddlPaymentType.Items.Add(new ListItem("Erlagschein", "0"));
        }

        public bool SaveInstallment()
        {
            var list = (ArrayList)Session["InstallmentList"];
            if (list != null && list.Count > 0)
            {
                HTBUtils.DeleteInstallmentPlan(custInkAkt.CustInkAktID);
                var set = new RecordSet();
                foreach (tblCustInkAktRate installment in list)
                {
                    installment.CustInkAktRateAktID = custInkAkt.CustInkAktID;
                    installment.CustInkAktRateLastChanged = DateTime.Now;
                    installment.CustInkAktRateReceivedAmount = 0;
                    installment.CustInkAktRateBalance = installment.CustInkAktRateAmount;
                    installment.CustInkAktRatePaymentType = Convert.ToInt16(ddlPaymentType.SelectedValue);
                    installment.CustInkAktRatePostponeTillDate = HTBUtils.DefaultDate;
                    installment.CustInkAktRateNotifiedAD = HTBUtils.DefaultDate;
                    set.InsertRecord(installment);
                }
                if (intAktId > 0)
                {
                    UpdateInterventionAkt();
                }
                ShowSuccess("Raten gespechert!");
            }
            else
            {
                ShowError("<i><strong>Bitte Raten eingeben</strong></i>");
                return false;
            }
            return true;
        }

        #region Saved Installments
        private ArrayList GetSavedInstallmentsList()
        {
            string where = "CustInkAktRateAktId = " + custInkAkt.CustInkAktID;
            string orderBy = "CustInkAktRateDueDate";
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktRate WHERE " + where + " ORDER BY " + orderBy, typeof(tblCustInkAktRate));
            bool intallmentAmountSet = false;
            foreach (tblCustInkAktRate installment in list)
            {
                installment.CustInkAktRateBalance = installment.CustInkAktRateAmount - installment.CustInkAktRateReceivedAmount;
                if (!intallmentAmountSet)
                {
                    txtInstallmentAmount.Text = HTBUtils.FormatCurrencyNumber(installment.CustInkAktRateAmount);
                    intallmentAmountSet = true;
                }
            }
            txtNumberOfInstallments.Text = list.Count.ToString();
            Session["InstallmentList"] = list;
            return list;
        }
        private void PopulateSavedInstallmentsGrid()
        {
            DataTable dt = GetGridDataTableStructure();
            ArrayList installmentsList = GetSavedInstallmentsList();
            foreach (tblCustInkAktRate installment in installmentsList)
            {
                DataRow dr = dt.NewRow();
                dr["InstallmentDate"] = installment.CustInkAktRateDueDate.ToShortDateString();
                dr["InstallmentAmount"] = HTBUtils.FormatCurrency(installment.CustInkAktRateAmount);
                dt.Rows.Add(dr);
            }
            gvInstallments.DataSource = dt;
            gvInstallments.DataBind();
        }
        #endregion

        private void UpdateInterventionAkt()
        {
            tblControl control = HTBUtils.GetControlRecord();
            tblAktenInt aktInt = HTBUtils.GetInterventionAkt(intAktId);
            if (aktInt != null)
            {
                aktInt.AktIntProcessCode = control.ProcessCodeInstallment;
                try
                {
                    aktInt.AKTIntRVInkassoType = Convert.ToInt32(ddlPaymentType.SelectedValue);
                }
                catch
                {
                }
                RecordSet.Update(aktInt);
            }
        }

        #region Getter / Setter
        public void SetAktId(int id)
        {
            aktId = id;
        }
        public int GetAktId()
        {
            return aktId;
        }

        public void SetAktIntId(int id)
        {
            intAktId = id;
        }
        public int GetAktIntId()
        {
            return intAktId;
        }
        public double GetCurrentPayment()
        {
            return GlobalUtilArea.GetZeroIfConvertToDoubleError(hdnCurrentPayment.Value);
        }
        public void SetCurrentPayment(double payment)
        {
            hdnCurrentPayment.Value = payment.ToString();
        }
        #endregion
    }
}