using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;
using System.Text;
using HTBInvoiceManager;
using HTB.v2.intranetx.util;
using System.Data;
using System.Collections;
using System.Drawing;
using HTB.Database.Views;
using HTB.Database.StoredProcs;

namespace HTB.v2.intranetx.aktenink
{
    public partial class EditInvoice : Page
    {
        int id = -1;
        tblCustInkAktInvoice _inv = new tblCustInkAktInvoice();
        readonly tblControl _control = HTBUtils.GetControlRecord();
        ArrayList _installmentsList = new ArrayList();
        
        public double AppliedAmount;
        public double UnappliedAmount;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                id = Convert.ToInt32(Request.QueryString["ID"]);
            }
            catch
            {
                id = -1;
            }

            if (id >= 0)
            {
                LoadInvoice();
                if (!IsPostBack)
                {
                    SetValues();
                }
            }
        }

        private void LoadInvoice()
        {
            _inv = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + id, typeof(tblCustInkAktInvoice));
            if (_inv != null)
            {
                var invMgr = new InvoiceManager();
                AppliedAmount = invMgr.GetAppliedAmount(_inv);
                UnappliedAmount = _inv.InvoiceAmount - AppliedAmount;
                
                btnApply.Enabled = true;
                btnUnApply.Enabled = true;

                if (UnappliedAmount > 0)
                {
                    lblUnappliedAmount.ForeColor = Color.Green;
                    btnApply.Enabled = true;
                }
                else
                {
                    lblUnappliedAmount.ForeColor = Color.Black;
                    btnApply.Enabled = false;
                }
                if (AppliedAmount == 0)
                {
                    btnUnApply.Enabled = false;
                }
            }
        }

        private void SetValues()
        {
            ctlMessage.Clear();
            lblAktId.Text = _inv.InvoiceCustInkAktId.ToString();
            lblInvoiceId.Text = _inv.InvoiceID.ToString();
            txtInvoiceDate.Text = _inv.InvoiceDate.ToShortDateString();
            txtDescription.Text = _inv.InvoiceDescription.Trim();
            txtInvoiceNetAmount.Text = HTBUtils.FormatCurrencyNumber(_inv.InvoiceAmountNetto);
            txtDueDate.Text = _inv.InvoiceDueDate.ToShortDateString();
            txtMemo.Text = _inv.InvoiceComment;
            lblAppliedAmount.Text = HTBUtils.FormatCurrency(AppliedAmount);
            lblUnappliedAmount.Text = HTBUtils.FormatCurrency(UnappliedAmount);
            lblTaxAmount.Text = HTBUtils.FormatCurrency(_inv.InvoiceTax);
            lblInvoiceAmount.Text = HTBUtils.FormatCurrency(_inv.InvoiceAmount);
            if (_inv.InvoicePaymentTransferToClientDate.ToShortDateString() != HTBUtils.DefaultShortDate)
            {
                chkTransferred.Checked = true;
                txtTransferDate.Text = _inv.InvoicePaymentTransferToClientDate.ToShortDateString();
                txtTransferredAmount.Text = HTBUtils.FormatCurrencyNumber(_inv.InvoicePaymentTransferToClientAmount);
            }
            chkTaxable.Checked = _inv.InvoiceTax > 0;
            PopulateAppliedGrid(GetApplied());
            LoadInstallmentsList();
            PopulateInstallmentsGrid();
            PopulateAppliedInstallmentsGrid();

            if (gvInstallments.Rows.Count == 0)
            {
                trInstallmentsGrid.Visible = false;
            }
            if (_inv.IsPayment())
            {
                trTaxable.Visible = false;
                trDueDate.Visible = false;
                trTaxAmount.Visible = false;
            }
            ShowOrHideTransferFields();
        }

        #region Even Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (SaveEntry(false))
            {
                CloseWindowAndRefresh();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWindowAndRefresh();
        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (_inv.IsPayment() && SaveEntry(true))
            {
                ApplyPayment(_inv.InvoiceAmount,
                    GlobalUtilArea.GetZeroIfConvertToDoubleError(txtClientAmount.Text),
                    GlobalUtilArea.GetZeroIfConvertToDoubleError(txtCollectionAmount.Text),
                    chkManual.Checked);
                double clientPortion = 0;
                double collectionPortion = 0;
                ArrayList applyList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktInvoiceApplyFrom WHERE ApplyFromInvoiceId = " + _inv.InvoiceID, typeof(qryCustInkAktInvoiceApply));
                foreach (qryCustInkAktInvoiceApply applyInv in applyList)
                {
                    if (applyInv.IsClientInvoice())
                    {
                        clientPortion += applyInv.ApplyAmount;
                    }
                    else
                    {
                        collectionPortion += applyInv.ApplyAmount;
                    }
                }
                StringBuilder sb = new StringBuilder();
                LoadInstallmentsList();
                UpdateInstallments();
                sb.Append("Applied SUCCESS:<br/>Klient: [");
                sb.Append(HTBUtils.FormatCurrency(clientPortion));
                sb.Append("]&nbsp;&nbsp;&nbsp; ECP: [");
                sb.Append(HTBUtils.FormatCurrency(collectionPortion));
                sb.Append("]&nbsp;&nbsp;&nbsp; OPENED: [");
                sb.Append(HTBUtils.FormatCurrency((_inv.InvoiceAmount - clientPortion - collectionPortion)));
                sb.Append("]");
                LoadInvoice();
                SetValues();
                ctlMessage.ShowSuccess(sb.ToString());
                
                //ResetPage();

            }
        }
        protected void btnUnApply_Click(object sender, EventArgs e)
        {
            InvoiceManager invMgr = new InvoiceManager();
            invMgr.UnapplyInvoiceAll(_inv);
            invMgr.UnapplyAllInstallments(_inv);
            LoadInvoice();
            _inv.InvoicePaymentTransferToClientAmount = 0;
            _inv.InvoicePaymentTransferToClientDate = HTBUtils.DefaultDate;
            SetValues();
            ctlMessage.ShowSuccess("Unapplied SUCCESS:");
        }

        protected void chkManual_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManual.Checked)
            {
                trClientAmount.Visible = true;
                trCollectionAmount.Visible = true;
            }
            else
            {
                txtClientAmount.Text = "";
                txtCollectionAmount.Text = "";
                trClientAmount.Visible = false;
                trCollectionAmount.Visible = false;
            }
        }
        protected void chkTransferred_CheckedChanged(object sender, EventArgs e)
        {
            ShowOrHideTransferFields();
        }
        protected void txtInvoiceNetAmount_TextChanged(object sender, EventArgs e)
        {
            if (LoadAndValidateEntry())
            {
                if (_inv.IsPayment())
                {
                    double amountToSplit = _inv.InvoiceAmount;
                    if (amountToSplit > 0)
                    {
                        LoadInstallmentsList();
                        for (int i = 0; i < _installmentsList.Count && amountToSplit > 0; i++)
                        {
                            qryCustInkRate installment = (qryCustInkRate)_installmentsList[i];
                            if (amountToSplit >= installment.CustInkAktRateBalance)
                            {
                                installment.TempReceivedAmount = installment.CustInkAktRateBalance;
                            }
                            else
                            {
                                installment.TempReceivedAmount = amountToSplit;
                            }
                            amountToSplit -= installment.TempReceivedAmount;
                        }
                        PopulateInstallmentsGrid();
                    }
                }
            }
        }
        #endregion

        private void CloseWindowAndRefresh()
        {
            ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "closeScript", "MM_refreshParentAndClose();", true);
        }

        private void CloseWindow()
        {
            ScriptManager.RegisterStartupScript(updPanel1, typeof(string), "closeScript", "window.close();", true);
        }

        #region Validation
        private bool LoadAndValidateEntry()
        {
            var sb = new StringBuilder();
            bool okk = true;
            _inv.InvoiceAmountNetto = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtInvoiceNetAmount.Text);
            _inv.InvoiceDescription = txtDescription.Text.Trim();
            if (!_inv.IsPayment())
            {
                _inv.InvoiceTax = chkTaxable.Checked ? Double.Parse(((_control.TaxRate / 100) * _inv.InvoiceAmountNetto).ToString("N2")) : 0;
            }
            else
            {
                _inv.InvoiceTax = 0;
            }
            _inv.InvoiceAmount = _inv.InvoiceAmountNetto + _inv.InvoiceTax;
            _inv.InvoiceBalance = _inv.InvoiceAmount - AppliedAmount;
            if (_inv.InvoiceBalance < -0.005)
            {
                sb.Append("Invalid Invoice Amount: The amount [" + _inv.InvoiceAmount + "] must be greater than the applied amount [" + AppliedAmount + "]");
                okk = false;
            }
            _inv.InvoiceDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtInvoiceDate.Text.ToString());
            _inv.InvoiceDueDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDueDate.Text.ToString());
            _inv.InvoiceComment = txtMemo.Text;

            if (chkManual.Checked)
            {
                if ((GlobalUtilArea.GetZeroIfConvertToDoubleError(txtClientAmount.Text) + GlobalUtilArea.GetZeroIfConvertToDoubleError(txtCollectionAmount.Text)) != _inv.InvoiceAmount)
                {
                    sb.Append("Invalid Amount: The Invoice amount [" + _inv.InvoiceAmount + "] must be equal to the sum of  the applied amount [" + AppliedAmount + "]");
                    okk = false;
                }
            }

            if (chkTransferred.Checked)
            {
                _inv.InvoicePaymentTransferToClientDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtTransferDate);
                _inv.InvoicePaymentTransferToClientAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtTransferredAmount);

                if (_inv.InvoicePaymentTransferToClientDate.ToShortDateString() == HTBUtils.DefaultShortDate)
                {
                    sb.Append("Invalid &Uuml;berwiesen am [Datum]");
                    okk = false;
                }
            }
            else
            {
                _inv.InvoicePaymentTransferToClientDate = HTBUtils.DefaultDate;
                _inv.InvoicePaymentTransferToClientAmount = 0;
            }
            if (_inv.InvoiceDate.ToShortDateString() == HTBUtils.DefaultShortDate)
                _inv.InvoiceDate = DateTime.Now;
            if (_inv.InvoiceDueDate.ToShortDateString() == HTBUtils.DefaultShortDate)
                _inv.InvoiceDueDate = DateTime.Now;

            if (!okk)
                ctlMessage.ShowError(sb.ToString());
            return okk;
        }
        private bool ValidateRateDistribution()
        {
            bool okk = true;
            double totPayAppliedToInstallments = 0;
            var sb = new StringBuilder();
            if (gvInstallments.Rows.Count > 0)
            {
                for (int i = 0; i < gvInstallments.Rows.Count; i++)
                {
                    GridViewRow row = gvInstallments.Rows[i];
                    var txtReceivedAmount = (TextBox)row.Cells[4].FindControl("txtReceivedAmount");
                    double receivedAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtReceivedAmount.Text);

                    totPayAppliedToInstallments += receivedAmount;
                    if (receivedAmount > 0)
                    {
                        var lblInstallmentID = (Label)row.Cells[0].FindControl("lblInstallmentId");
                        tblCustInkAktRate installment = GetInstallmentByID(lblInstallmentID.Text);

                        if (installment != null && installment.CustInkAktRateBalance < receivedAmount)
                        {
                            sb.Append("Rate [" + installment.CustInkAktRateDueDate.ToShortDateString() + "]: Betrag (Eingang) ist größer als Betrag.<br/>");
                            okk = false;
                        }
                    }
                }
                /*
                if (!HTBUtils.IsZero(totPayAppliedToInstallments - _inv.InvoiceAmount))
                {
                    sb.Append("Total rate distributed must be the same amout as the payment.<br/>");
                    okk = false;
                }
                 */
            }
            if (!okk)
                ctlMessage.AppendError(sb.ToString());
            return okk;
        }
        #endregion

        private void ShowOrHideTransferFields()
        {
            if (chkTransferred.Checked)
            {
                trTransferredDate.Visible = true;
                trTransferredAmount.Visible = true;
            }
            else
            {
                trTransferredDate.Visible = false;
                trTransferredAmount.Visible = false;
            }
        }

        #region Applied Grid
        private ArrayList GetApplied()
        {
            ArrayList applyList;
            string qryName = "qryCustInkAktInvoiceApplyFrom";
            string fieldName = "ApplyFromInvoiceId";

            if (!_inv.IsPayment())
            {
                qryName = "qryCustInkAktInvoiceApplyTo";
                fieldName = "ApplyToInvoiceId";
            }
            string sql = "SELECT * FROM " + qryName + " WHERE " + fieldName + " = " + _inv.InvoiceID + "ORDER BY ApplyDate";

            applyList = HTBUtils.GetSqlRecords(sql, typeof(qryCustInkAktInvoiceApply));

            return applyList;
        }

        private void PopulateAppliedGrid(ArrayList applyList)
        {
            DataTable dt = GetAppliedDataTableStructure();
            foreach (qryCustInkAktInvoiceApply appInv in applyList)
            {
                DataRow dr = dt.NewRow();
                dr["InvoiceID"] = appInv.InvoiceID;
                dr["InvoiceDate"] = appInv.InvoiceDate.ToShortDateString();
                dr["InvoiceDescription"] = appInv.InvoiceDescription.Trim();
                dr["AppliedAmount"] = appInv.ApplyAmount;
                dr["InvoiceAmount"] = HTBUtils.FormatCurrency(appInv.InvoiceAmount);
                dr["InvoiceBalance"] = HTBUtils.FormatCurrency(appInv.InvoiceBalance);
                dt.Rows.Add(dr);
            }
            gvAppliedTo.DataSource = dt;
            gvAppliedTo.DataBind();
        }

        private DataTable GetAppliedDataTableStructure()
        {
            DataTable dt = new DataTable();
            DataColumn dc;

            dc = new DataColumn();
            dt.Columns.Add(new DataColumn("InvoiceID", typeof(int)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("AppliedAmount", typeof(double)));
            dt.Columns.Add(new DataColumn("InvoiceBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceAmount", typeof(string)));
            return dt;
        }

        public double GetTotalApplied()
        {
            return AppliedAmount;
        }
        #endregion

        #region Save Entry
        private bool SaveEntry(bool validateRateDistribution)
        {
            ctlMessage.Clear();
            
            bool okk = LoadAndValidateEntry();
            if (validateRateDistribution) 
                okk &= ValidateRateDistribution();

            if (okk)
            {
                return RecordSet.Update(_inv);
            }
            return false;
        }
        #endregion

        #region Apply Payment
        private void ApplyPayment(double tot, double client, double collection, bool isManual)
        {
            if (_inv.IsPayment())
            {
                InvoiceManager invMgr = new InvoiceManager();
                bool isAutoSplitPayment = true;
                if (_inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_CASH ||
                    _inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_TRANSFER ||
                    _inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CREDIT)
                {
                    if (_inv.InvoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CREDIT && !isManual)
                    {
                        /* By default: credit ECP Invoices (unless the user applies it manually [see: if (isManual) ..])*/
                        client = 0;
                        collection = tot;
                        isAutoSplitPayment = false;
                    }
                    if (isManual)
                    {
                        if (client == 0 && collection == 0)
                        {
                            ctlMessage.AppendError("You must enter either Klient or ECP or both when selecting manual application");
                            return;
                        }
                        else if (client >= 0 && collection >= 0 && (tot != client + collection))
                        {
                            ctlMessage.AppendError("Klient + ECP must equal Betrag");
                            return;
                        }
                        else
                        {
                            if (client == 0)
                                client = tot - collection;
                            else if (collection == 0)
                                collection = tot - client;
                        }
                        isAutoSplitPayment = false;
                    }
                }
                else
                {
                    isAutoSplitPayment = false;
                    client = tot;
                    collection = 0;
                }

                if (isAutoSplitPayment)
                    invMgr.ApplyPayment(_inv.InvoiceID);
                else
                    invMgr.ApplyPayment(_inv.InvoiceID, client, collection);

            }
        }
        #endregion
        
        #region Installments
        private void LoadInstallmentsList()
        {
            string where = "CustInkAktRateAktId = " + _inv.InvoiceCustInkAktId + " AND (CustInkAktRateBalance > 0)";
            string orderBy = "CustInkAktRatePostponeTillDate, CustInkAktRateDueDate";
            _installmentsList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkRate WHERE " + where + " ORDER BY " + orderBy, typeof(qryCustInkRate));
        }
        private void PopulateInstallmentsGrid()
        {
            DataTable dt = GetGridDataTableStructure();
            foreach (qryCustInkRate installment in _installmentsList)
            {
                DataRow dr = dt.NewRow();
                dr["InstallmentID"] = installment.CustInkAktRateID;
                dr["AktNumber"] = installment.CustInkAktRateAktID;
                dr["RateDueDate"] = installment.CustInkAktRateDueDate.ToShortDateString();
                dr["RateAmount"] = HTBUtils.FormatCurrency(installment.CustInkAktRateAmount);
                dr["RateBalance"] = HTBUtils.FormatCurrency(installment.CustInkAktRateBalance);
                dr["NameSchuldner"] = installment.GegnerName1 + "  " + installment.GegnerName2;
                dr["ClientName"] = installment.KlientName1 + "  " + installment.KlientName2;
                dr["BankAccountNumber"] = installment.KlientKtoNr1;
                dr["ReceivedDate"] = DateTime.Now.ToShortDateString();
                dr["ReceivedAmount"] = installment.TempReceivedAmount > 0 ? installment.TempReceivedAmount.ToString("N2") : "";
                dr["PostponeTillDate"] = installment.CustInkAktRatePostponeTillDate != HTBUtils.DefaultDate ? installment.CustInkAktRatePostponeTillDate.ToShortDateString() : "";
                dr["PostponeReason"] = installment.CustInkAktRatePostponeReason.Replace(Environment.NewLine, "<br/>");
                dr["PostponeBy"] = installment.UserVorname + "&nbsp;" + installment.UserNachname;
                dt.Rows.Add(dr);
            }
            gvInstallments.DataSource = dt;
            gvInstallments.DataBind();
        }
        private DataTable GetGridDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("InstallmentID", typeof(int)));
            dt.Columns.Add(new DataColumn("AktNumber", typeof(int)));
            dt.Columns.Add(new DataColumn("RateDueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("RateAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("RateBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("NameSchuldner", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("BankAccountNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeTillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeReason", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeBy", typeof(string)));
            return dt;
        }
        private tblCustInkAktRate GetInstallmentByID(string installmentId)
        {
            return GetInstallmentByID(GlobalUtilArea.GetZeroIfConvertToIntError(installmentId));
        }
        private tblCustInkAktRate GetInstallmentByID(int installmentId)
        {
            foreach (tblCustInkAktRate installment in _installmentsList)
                if (installment.CustInkAktRateID == installmentId)
                    return installment;

            return null;
        }
        #endregion

        #region Installments Read Only (Applied)
        private ArrayList LoadAppliedInstallmentsList()
        {
            string spName = "spInvoiceAppliedToRate";
            ArrayList list = new ArrayList();
            list.Add(new StoredProcedureParameter("invoiceID", SqlDbType.Int, _inv.InvoiceID));

            return HTBUtils.GetStoredProcedureRecords(spName, list, typeof(spInvoiceAppliedToRate));
        }

        private void PopulateAppliedInstallmentsGrid()
        {
            ArrayList list = LoadAppliedInstallmentsList();
            DataTable dt = GetAppliedGridDataTableStructure();
            foreach (spInvoiceAppliedToRate applied in list)
            {
                DataRow dr = dt.NewRow();
                dr["InstallmentID"] = applied.CustInkAktRateID;
                dr["RateDueDate"] = applied.CustInkAktRateDueDate.ToShortDateString();
                dr["RateAmount"] = HTBUtils.FormatCurrency(applied.CustInkAktRateAmount);
                dr["RateBalance"] = HTBUtils.FormatCurrency(applied.CustInkAktRateBalance);
                dr["RateAppliedAmount"] = HTBUtils.FormatCurrency(applied.RateApplyAmount);
                dr["RateAppliedDate"] = applied.RateApplyDate.ToShortDateString();
                dr["PostponeTillDate"] = applied.CustInkAktRatePostponeTillDate != HTBUtils.DefaultDate ? applied.CustInkAktRatePostponeTillDate.ToShortDateString() : "";
                dr["PostponeReason"] = applied.CustInkAktRatePostponeReason.Replace(Environment.NewLine, "<br/>");
                dr["PostponeBy"] = applied.UserVorname + "&nbsp;" + applied.UserNachname;

                dt.Rows.Add(dr);
            }
            gvInstallmentsApplied.DataSource = dt;
            gvInstallmentsApplied.DataBind();
        }
        private DataTable GetAppliedGridDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("InstallmentID", typeof(int)));
            dt.Columns.Add(new DataColumn("RateDueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("RateAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("RateBalance", typeof(string)));
            dt.Columns.Add(new DataColumn("RateAppliedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("RateAppliedAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeTillDate", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeReason", typeof(string)));
            dt.Columns.Add(new DataColumn("PostponeBy", typeof(string)));
            return dt;
        }
        private string GetInstallmentReceivedDate(qryCustInkRate installment)
        {
            if (installment != null && installment.CustInkAktRateInvoiceID > 0)
            {
                tblCustInkAktInvoice inv = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + installment.CustInkAktRateInvoiceID, typeof(tblCustInkAktInvoice));
                if (inv != null)
                {
                    installment.TempReceivedAmount = inv.InvoiceAmount;
                    if (inv.IsPayment())
                    {
                        return inv.InvoicePaymentReceivedDate.ToShortDateString();
                    }
                    else
                    {
                        return inv.InvoiceDate.ToShortDateString();
                    }
                }
            }
            return "";
        }
        #endregion

        #region Apply Rate
        private void UpdateInstallments()
        {

            var invMgr = new InvoiceManager();
            if (_inv != null && _inv.IsPayment())
            {
                var set = new RecordSet();
                for (int i = 0; i < gvInstallments.Rows.Count; i++)
                {
                    GridViewRow row = gvInstallments.Rows[i];
                    var txtReceivedAmount = (TextBox)row.Cells[4].FindControl("txtReceivedAmount");
                    double receivedAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtReceivedAmount.Text);
                    if (receivedAmount > 0)
                    {
                        var lblInstallmentID = (Label)row.Cells[0].FindControl("lblInstallmentId");
                        tblCustInkAktRate installment = GetInstallmentByID(lblInstallmentID.Text);

                        if (installment != null)
                        {
                            var rec = new tblCustInkAktRate();
                            rec.Assign(installment);
                            rec.CustInkAktRateInvoiceID = _inv.InvoiceID;
                            rec.CustInkAktRateLastChanged = DateTime.Now;
                            set.UpdateRecord(rec);

                            invMgr.ApplyInstallmentAmount(_inv, rec, receivedAmount);
                        }
                    }
                }
            }
        }
        #endregion
    }
}