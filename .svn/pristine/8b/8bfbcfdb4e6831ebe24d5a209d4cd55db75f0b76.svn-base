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
    public partial class BankAccountMonthlyStatus : System.Web.UI.Page
    {
        private DateTime _startDate;
        private DateTime _endDate;

        private double _startingBalance;
        private double _transferredOther;

        private double _receivedECP;
        private double _receivedClient;
        private double _receivedTotal;

        private double _totalTransferred;
        private double _transferredClient;
        private double _totalTransactionsAmount;

        protected void Page_Load(object sender, EventArgs e)
        {
            ctlMessage.Clear();
        }
        
        #region Event Handlers
        
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                _startDate = GlobalUtilArea.GetDateAtTime(_startDate, "00:00");
                _endDate = GlobalUtilArea.GetDateAtTime(_endDate, "23:59");

                PopulteGrids();

                _startingBalance = GetStartingBalance();

//                lblDate.Text = _startDate.ToShortDateString() + " - " + _endDate.ToShortDateString();
                lblStartingBalance.Text = GetRedIfNegative(_startingBalance);
                lblPaymentsECP.Text = GetRedIfNegative(_receivedECP);
                lblPaymentsClient.Text = GetRedIfNegative(_receivedClient);

                lblOtherExpenses.Text = GetRedIfNegative(_transferredOther);
                lblTransfersToClient.Text = GetRedIfNegative(_transferredClient);
                lblTotalPayments.Text = GetRedIfNegative(_receivedTotal);
                lblTotalExpenses.Text = GetRedIfNegative(_totalTransferred);

                lblMonthlyBalance.Text = GetRedIfNegative(_receivedTotal + _totalTransferred);

                lblBalance.Text = GetRedIfNegative(_startingBalance + _receivedTotal + _totalTransferred);
                
//                txtDateFrom.Visible = false;
//                lblFrom.Visible = false;
//                Datum_CalendarButton.Visible = false;
//                txtDateTo.Visible = false;
//                lblTo.Visible = false;
//                Datum_CalendarButton2.Visible = false;
                
//                txtStartingBalance.Visible = false;
                
//                lblDate.Visible = true;
                trStartingBalance.Visible = true;
                trPaymentsECP.Visible = true;
                trPaymentsClient.Visible = true;
                trTransfersToClient.Visible = true;
                trProvision.Visible = true;
                trTotalPayments.Visible = true;
                trTotalExpenses.Visible = true;
                trMonthlyBalance.Visible = true;
                trBalance.Visible = true;
                
//                btnCalculate.Visible = false;
//                btnSave.Visible = HTBUtils.IsLastDayOfMonthForDate(_startDate, _endDate);
//                btnNew.Visible = true;

                trEmpty1.Visible = true;
                trEmpty2.Visible = true;
                trEmpty3.Visible = true;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("/v2/intranetx/bank/BankAccountMonthlyStatus.aspx");
        }
        
        /*
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                try
                {
                    _startDate = GlobalUtilArea.GetDateAtTime(_startDate, "00:00");
                    _endDate = GlobalUtilArea.GetDateAtTime(_endDate, "23:59");

                    PopulteGrids();

                    _startingBalance = GetStartingBalance();
                    
                    if (HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblBankMonthly WHERE BamStartDate = '" + _startDate.ToShortDateString() + "'", typeof(tblBankMonthly)) != null)
                    {
                        ctlMessage.ShowError("The report exists and it cannot be re-written!");
                    }
                    else
                    {
                        if (!RecordSet.Insert(new tblBankMonthly()
                                                  {
                                                      BamStartDate = _startDate,
                                                      BamEndDate = _endDate,
                                                      BamReceived = _receivedTotal,
                                                      BamTransferredToClient = _transferredClient,
                                                      BamStartBalance = _startingBalance,
                                                      BamExpenseOther = _transferredOther,
                                                      BamEndBalance = _receivedTotal + _totalTransferred
                                                  }))
                        {
                            ctlMessage.ShowError("Fehler bei speichern!");
                        }
                        else
                        {
                            ctlMessage.ShowSuccess("Info Gespeichert!");
                        }
                    }
                }
                catch(Exception ex)
                {
                    ctlMessage.ShowException(ex);
                }
            }
        }

        
        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            if(IsFormValid(false))
            {
                var bm = (tblBankMonthly)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblBankMonthly WHERE BamEndDate <'" + _startDate.ToShortDateString() + "' ORDER BY BamEndDate DESC", typeof(tblBankMonthly));
                if (bm != null)
                {
                    txtStartingBalance.Text = HTBUtils.FormatCurrencyNumber(bm.BamEndBalance);
                }
                else
                {
                    ctlMessage.ShowInfo("Anfangssaldo nicht gefunden.... bitte eingeben.");
                }
            }
        }
        */

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/v2/intranetx/bank/BankAccountMonthlyStatus.aspx");
        }
        #endregion

        private void PopulteGrids()
        {
            var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("startDate", SqlDbType.DateTime, _startDate),
                                        new StoredProcedureParameter("endDate", SqlDbType.DateTime, _endDate),
                                        new StoredProcedureParameter("onlyKlient", SqlDbType.Bit, chkOnlyKlient.Checked)
                                    };

            ArrayList[] lists = HTBUtils.GetMultipleListsFromStoredProcedure("spGetInkassoMonthlyBankStatus", parameters, new Type[] { typeof(spGetInkassoTransactions)});
            PopulateTransactionsGrid(lists[0]);
        }

        private double GetStartingBalance()
        {
            double balance = 0;
            var parameters = new ArrayList
                                    {
                                        new StoredProcedureParameter("date", SqlDbType.DateTime, _startDate),
                                        new StoredProcedureParameter("amount", SqlDbType.Float, balance, ParameterDirection.Output)
                                    };

            HTBUtils.GetStoredProcedureSingleRecord("spGetBankAccountStartingBalance", parameters, typeof(Record));
            foreach (object o in parameters)
            {
                if (o is ArrayList)
                {
                    var outputList = (ArrayList)o;
                    foreach (StoredProcedureParameter p in outputList)
                    {
                        if (p.Name.IndexOf("amount") >= 0)
                        {
                            balance = Convert.ToDouble(p.Value);
                            break;
                        }
                    }
                }
            }
            return balance;
        }

        #region Transaction Grid
        
        private void PopulateTransactionsGrid(ArrayList list)
        {
            DataTable dt = GetTransactionsDataTableStructure();

            _receivedECP = 0;
            _receivedClient = 0;
            _transferredClient = 0;
            _receivedTotal = 0;
            _totalTransferred = 0;
            _totalTransactionsAmount = 0;
            _transferredOther = 0;

            foreach (spGetInkassoTransactions rec in list)
            {
                DataRow dr = dt.NewRow();
                var sb = new StringBuilder();
                if (rec.InvoiceID > 0)
                {
                    sb.Append("<a href=\"javascript:MM_openBrWindow('/v2/intranetx/aktenink/ShowInvoice.aspx?");
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "InvId", rec.InvoiceID.ToString(), false);
                    sb.Append("','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10')\">");
                    sb.Append(rec.InvoiceID);
                    sb.Append("</a>");
                    dr["InvoiceID"] = sb.ToString();

                    sb.Clear();
                    if (rec.AppliedAmount > 0)
                    {
                        dr["Sender"] = rec.GegnerName;
                        if (rec.IsECP)
                        {
                            sb.Append("ECP");
                        }
                        else
                        {
                            sb.Append("[Eingang Kunde] ");
                            sb.Append(rec.KlientName);
                            if (!string.IsNullOrEmpty(rec.Description))
                            {
                                sb.Append("<BR/>");
                                sb.Append(rec.Description.Replace("\n", "<BR/>"));
                            }
                        }
                    }
                    else
                    {
                        dr["Sender"] = "ECP";
                        if (rec.IsProvision)
                        {
                            sb.Append("[Ausgang Provision] ");
                            sb.Append(rec.KlientName);
                            if (!string.IsNullOrEmpty(rec.Description))
                            {
                                sb.Append("<BR/>");
                                sb.Append(rec.Description.Replace("\n", "<BR/>"));
                            }
                        }
                        else
                        {
                            sb.Append("[Ausgang Kunde] ");
                            sb.Append(rec.KlientName);
                            if (!string.IsNullOrEmpty(rec.Description))
                            {
                                sb.Append("<BR/>");
                                sb.Append(rec.Description.Replace("\n", "<BR/>"));
                            }
                        }
                    }

                    dr["Receiver"] = sb.ToString();
                }
                else
                {
                    sb.Clear();
                    sb.Append(rec.KlientName);
                    if (!string.IsNullOrEmpty(rec.Description))
                    {
                        sb.Append("<BR/>");
                        sb.Append(rec.Description.Replace("\n", "<BR/>"));
                    }
                    dr["InvoiceID"] = "&nbsp;";
                    dr["Sender"] = rec.GegnerName;
                    dr["Receiver"] = sb.ToString();
                }

                dr["Date"] = rec.TransactionDate.ToShortDateString();
                dr["AppliedAmount"] = rec.AppliedAmount < 0 ? "<font color=\"red\">" + HTBUtils.FormatCurrency(rec.AppliedAmount) + "</font>" : HTBUtils.FormatCurrency(rec.AppliedAmount);
                dr["InvoiceCustInkAktId"] = rec.InvoiceCustInkAktId.ToString();

                
                dt.Rows.Add(dr);

                if (rec.AppliedAmount < 0)
                {
                    _totalTransferred += rec.AppliedAmount;
                    if (rec.InvoiceID > 0 && !rec.IsProvision)
                        _transferredClient += rec.AppliedAmount;
                    else
                        _transferredOther += rec.AppliedAmount;
                }
                else
                {
                    _receivedTotal += rec.AppliedAmount;
                    if (rec.IsECP)
                        _receivedECP += rec.AppliedAmount;
                    else
                        _receivedClient += rec.AppliedAmount;

                }

                _totalTransactionsAmount += rec.AppliedAmount;
            }
            gvTransactions.DataSource = dt;
            gvTransactions.DataBind();
        }

        private DataTable GetTransactionsDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("InvoiceID", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(string)));
            dt.Columns.Add(new DataColumn("AppliedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoicePaymentTransferToClientDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoicePaymentTransferToClientAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("EcpBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceCustInkAktId", typeof(string)));
            dt.Columns.Add(new DataColumn("Sender", typeof(string)));
            dt.Columns.Add(new DataColumn("Receiver", typeof(string)));
            return dt;
        }
        protected string GetTotalReceived()
        {
            return GetRedIfNegative(_receivedECP);
        }
        protected string GetTotalTransferred()
        {
            return GetRedIfNegative(_totalTransferred);
        }
        protected string GetTotalTransactionsAmount()
        {
            return GetRedIfNegative(_totalTransactionsAmount);
        }
        #endregion

        private bool IsFormValid(bool validateEndDate = true)
        {
            _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateFrom);
            if (!HTBUtils.IsDateValid(_startDate))
            {
                _startDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(HTBUtils.GetConfigValue("BankStartingBalanceDate"));
                txtDateFrom.Text = _startDate.AddDays(1).ToShortDateString();
                if (!HTBUtils.IsDateValid(_startDate))
                {
                    ctlMessage.ShowError("Datum [vom] ist ung&uuml;ltig!");
                    return false;
                }
            }

            DateTime minStartDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(HTBUtils.GetConfigValue("BankStartingBalanceDate"));
            if (_startDate.CompareTo(minStartDate) < 0)
            {
                ctlMessage.ShowError("Es gibt kein data vor "+minStartDate.AddDays(1).ToShortDateString());
                return false;
            }
            
            if (validateEndDate)
            {
                _endDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDateTo);
                if (!HTBUtils.IsDateValid(_endDate))
                {
                    _endDate = DateTime.Now;
                }
                txtDateTo.Text = _endDate.ToShortDateString();
            }
            return true;
        }

        private string GetRedIfNegative(double amount)
        {
            return (amount < 0) ? "<font color=\"red\">" + HTBUtils.FormatCurrency(amount) + "</font>" : HTBUtils.FormatCurrency(amount);
        }
    }
}