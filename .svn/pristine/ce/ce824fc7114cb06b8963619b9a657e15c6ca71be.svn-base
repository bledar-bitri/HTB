using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.aktenint;
using HTBUtilities;
using System.Collections;
using System.Data;
using System.Drawing;
using Microsoft.VisualBasic;
using HTB.v2.intranetx.util;
using HTBAktLayer;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlInstallmentTablet : System.Web.UI.UserControl, IInstallment
    {
        public tblCustInkAkt custInkAkt;
        public tblGegner gegner;
        public double originalAmount;
        public double totalAmount;
        public double totPaid;
        public double balance;

        private static readonly tblControl Control = HTBUtils.GetControlRecord();
        private static readonly double AnnualInterestRate = Control.AnnualInterestRate / 100;
        
        private ArrayList installmentList = new ArrayList();
        private AktUtils aktUtils;
        private double installmentAmount;
        public AuftragTablet Parent { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "&nbsp;";
            ctlInstallmentInfo.parent = this;
        }

        public void LoadAll()
        {
            LoadRecords();
            if (custInkAkt != null)
            {
                aktUtils = new AktUtils(custInkAkt.CustInkAktID);
                originalAmount = aktUtils.GetAktOriginalInvoiceAmount();
                totalAmount = aktUtils.GetAktTotalInvoiceAmount();
                totPaid = aktUtils.GetAktTotalPayments() + Parent.GetCollectedAmount();
                balance = aktUtils.GetAktBalance() - Parent.GetCollectedAmount();

                SetValues();
                PopulateSavedInstallmentsGrid();
            }
        }
        private void SetValues()
        {
            lblHeader.Text = "Ratenvereinbarung";

            txtStartDate.Text = DateTime.Now.ToShortDateString();
            lblTotalPaid.Text = HTBUtils.FormatCurrency(totPaid);
            lblOriginalAmount.Text = HTBUtils.FormatCurrency(originalAmount);
            lblBalance.Text = HTBUtils.FormatCurrency(balance);
        }

        private void LoadRecords()
        {
            if (GetAktIntId() > 0)
            {
                tblAktenInt intAkt = HTBUtils.GetInterventionAkt(GetAktIntId());
                if (intAkt != null && intAkt.IsInkasso())
                {
                    custInkAkt = HTBUtils.GetInkassoAkt(intAkt.AktIntCustInkAktID);
                }
            }
            else
            {
                custInkAkt = HTBUtils.GetInkassoAkt(GetAktId());
            }
            if(custInkAkt != null)
            {
                gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerID = " + custInkAkt.CustInkAktGegner, typeof (tblGegner));
                ctlInstallmentInfo.SetGegnerInfo(gegner);
            }
        }

        protected void calcRVNoRates_Click(object sender, EventArgs e)
        {
            int noi = GlobalUtilArea.GetZeroIfConvertToIntError(txtNumberOfInstallments.Text);
            LoadAll();
            int interestPeriod = GetInterestPeriod();
            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            txtNumberOfInstallments.Text = noi.ToString();
            if (noi > 0)
            {
                try
                {
                    double lastInstallment = 0;
                    DateTime endDate = startDate.AddMonths(noi - 1);
                    if (noi == 1)
                    {
                        installmentAmount = balance;
                        lastInstallment = balance;
                    }
                    else
                    {
                        installmentAmount = Microsoft.VisualBasic.Financial.Pmt(AnnualInterestRate / interestPeriod, noi, -balance);
                        lastInstallment = Microsoft.VisualBasic.Financial.PPmt(AnnualInterestRate / interestPeriod, noi, noi, -balance);
                    }
                    txtInstallmentAmount.Text = installmentAmount.ToString("N2");
                    lblLastInstallment.Text = HTBUtils.FormatCurrency(lastInstallment);

                    lblEndDatum.Text = endDate.ToShortDateString();
                    lblTotalInterest.Text = HTBUtils.FormatCurrency(((installmentAmount * (noi - 1)) + lastInstallment - balance));
                    LoadInstallmentsList(noi, installmentAmount, lastInstallment);
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
            installmentAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtInstallmentAmount.Text);
            LoadAll();
            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            int interestPeriod = GetInterestPeriod();
            try
            {
                
                    int numberOfInstallments;
                    double latestInstallment;
                    double totalInterest;
                    HTBUtils.CalculateInstallmentPlanBasedOnPaymentAmount(balance, installmentAmount, AnnualInterestRate, interestPeriod, out numberOfInstallments, out latestInstallment, out totalInterest);
                   
                    txtNumberOfInstallments.Text = numberOfInstallments.ToString();
                    lblLastInstallment.Text = HTBUtils.FormatCurrency(latestInstallment);
                    lblTotalInterest.Text = HTBUtils.FormatCurrency(totalInterest);

                    LoadInstallmentsList(numberOfInstallments, installmentAmount, latestInstallment);
                    PopulateInstallmentsGrid();
                
                DateTime endDate = startDate.AddMonths(numberOfInstallments-1);
                lblEndDatum.Text = endDate.ToShortDateString();
                txtInstallmentAmount.Text = HTBUtils.FormatCurrencyNumber(installmentAmount);
            }
            catch
            {
                ShowError("<i><strong>Anzahl der Raten</strong></i> kann nich berechnet werden da die Zinsen h&ouml;er sind als die Ratenh&ouml;he.");
            }
        }

        private int GetInterestPeriod()
        {
            if (ddlInstallmentPeriod.SelectedValue.ToLower().Equals("weekly"))
            {
                return 52;
            }
            return 12;
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
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("InstallmentDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InstallmentAmount", typeof(string)));
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
            LoadRecords();
            var list = GetIntallmentList();
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
                if (GetAktIntId() > 0)
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

        private List<tblCustInkAktRate> GetIntallmentList()
        {
            var list = new List<tblCustInkAktRate>();
            for (int i = 0; i < gvInstallments.Rows.Count; i++)
            {
                GridViewRow row = gvInstallments.Rows[i];
                var lblInstallmentDate = (Label)row.FindControl("lblInstallmentDate");
                var lblInstallmentAmount = (Label)row.FindControl("lblInstallmentAmount");
                double amount = GlobalUtilArea.GetZeroIfConvertToDoubleError(lblInstallmentAmount.Text.Replace("€ ", "").Replace("€", ""));

                list.Add(new tblCustInkAktRate
                             {
                                 CustInkAktRateAmount = amount,
                                 CustInkAktRateAktID = custInkAkt.CustInkAktID,
                                 CustInkAktRateDate = DateTime.Now,
                                 CustInkAktRateDueDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(lblInstallmentDate.Text)
                             });

            }
            return list;
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
            tblAktenInt aktInt = HTBUtils.GetInterventionAkt(GetAktIntId());
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
            hdnAktId.Text = id.ToString();
        }
        public int GetAktId()
        {
            return GlobalUtilArea.GetZeroIfConvertToIntError(hdnAktId.Text);
        }

        public void SetAktIntId(int id)
        {
            hdnAktIntId.Text = id.ToString();
            ctlInstallmentInfo.SetAktId(id);
        }
        public int GetAktIntId()
        {
            return GlobalUtilArea.GetZeroIfConvertToIntError(hdnAktIntId.Text);
        }

        public void SetCollectedAmount(double paid)
        {
            hdnCollectedAmount.Text = paid.ToString();
            totPaid = aktUtils.GetAktTotalPayments() + paid;
            balance = aktUtils.GetAktBalance() - paid;
            SetValues();
        }

        public void SetInstallmentInfo()
        {

            ctlInstallmentInfo.SetInstallmentAmount(GlobalUtilArea.GetZeroIfConvertToDoubleError(txtInstallmentAmount.Text));
            ctlInstallmentInfo.SetFirstInstallmentDate(GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtStartDate.Text));
            ctlInstallmentInfo.SetPayment(Parent.GetCollectedAmount());
            ctlInstallmentInfo.SetLastInstallmentDate(GlobalUtilArea.GetDefaultDateIfConvertToDateError(lblEndDatum.Text));
        }
        #endregion
    }
}