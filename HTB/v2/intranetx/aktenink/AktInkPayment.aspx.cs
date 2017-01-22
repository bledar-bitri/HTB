using System;
using System.Linq;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.util;
using System.Collections;
using System.Text;
using System.Data;
using HTBUtilities;
using HTBAktLayer;
using HTBInvoiceManager;
using HTB.Database.Views;

namespace HTB.v2.intranetx.aktenink
{
    public partial class AktInkPayment : System.Web.UI.Page
    {
        public qryCustAktEdit QryInkassoakt = new qryCustAktEdit();
        private ArrayList _installmentsList = new ArrayList();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.QueryString[GlobalHtmlParams.ID]);
            if (aktId > 0)
            {
                QryInkassoakt = (qryCustAktEdit)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustAktEdit WHERE CustInkAktID = " + aktId, typeof(qryCustAktEdit));
                if (!IsPostBack)
                {
                    lblCustInkAktKunde.Text = QryInkassoakt.CustInkAktKunde;
                    if (lblCustInkAktKunde.Text.Trim() == "")
                    {
                        lblCustInkAktKunde.Text = "&nbsp;";
                    }
                    ctlMessage.Clear();
                    var aktUtils = new AktUtils(QryInkassoakt.CustInkAktID);
                    lblBalance.Text = HTBUtils.FormatCurrency(aktUtils.GetAktBalance());

                    LoadInstallmentsList();
                    PopulateInstallmentsGrid();
                    if (_installmentsList.Count == 0)
                    {
                        trInstallmentsGrid.Visible = false;
                    }
                }
            }
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

        protected void Submit_Click(object sender, EventArgs e)
        {
            btnSubmit.Enabled = false;
            var sb = new StringBuilder();
            double totPay = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtTotalPayment.Text);
            double clientPay = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtClientAmount.Text);
            double collectionPay = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtCollectionAmount.Text);
            int invoiceType = GetInvoiceType();
            double clientPortion = 0;
            double collectionPortion = 0;
            try
            {
                LoadInstallmentsList(); // make sure this takes place coz the list is used in ValidateEntry() and UpdateInstallments(int)
                if (ValidateEntry())
                {
                    if (invoiceType == tblCustInkAktInvoice.INVOICE_TYPE_DEBIT)
                    {
                        var invMgr = new InvoiceManager();
                        invMgr.CreateAndSaveInvoice(QryInkassoakt.CustInkAktID, invoiceType, totPay, "Belastung", txtComment.Text.Trim(), false);
                        ctlMessage.ShowSuccess("Belastung: " + totPay + " OK");
                    }
                    else if (invoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST)
                    {
                        var invMgr = new InvoiceManager();
                        invMgr.CreateAndSaveInvoice(QryInkassoakt.CustInkAktID, invoiceType, totPay, "Klient - Kosten", txtComment.Text.Trim(), false);
                        ctlMessage.ShowSuccess("Klient - Kosten: " + totPay + " OK");
                    }
                    else if (invoiceType == tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_CLIENT)
                    {
                        var invMgr = new InvoiceManager();
                        invMgr.CreateAndSaveInvoice(QryInkassoakt.CustInkAktID, invoiceType, totPay, "Zinsen - Klient", txtComment.Text.Trim(), false);
                        ctlMessage.ShowSuccess("Zinsen - Klient: " + totPay + " OK");
                    }
                    else
                    {
                        #region payment
                        int payId = CreateAndApplyPayment(totPay, clientPay, collectionPay, chkManual.Checked, sb);
                        if (payId >= 0)
                        {
                            ArrayList applyList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktInvoiceApplyFrom WHERE ApplyFromInvoiceId = " + payId, typeof(qryCustInkAktInvoiceApply));
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
                            UpdateInstallments(payId);
                            sb.Append("Klient: [");
                            sb.Append(HTBUtils.FormatCurrency(clientPortion));
                            sb.Append("]&nbsp;&nbsp;&nbsp; ECP: [");
                            sb.Append(HTBUtils.FormatCurrency(collectionPortion));
                            sb.Append("]&nbsp;&nbsp;&nbsp; OPENED: [");
                            sb.Append(HTBUtils.FormatCurrency((totPay - clientPortion - collectionPortion)));
                            sb.Append("]");
                            ResetPage();
                            ctlMessage.ShowSuccess(sb.ToString());
                        }
                        else
                        {
                            ctlMessage.ShowError("Could not apply payment.");
                        }
                        #endregion
                    }
                }
            }
            catch(Exception ex) {
                ctlMessage.ShowException(ex);
            }
            btnSubmit.Enabled = true;
        }
        
        private int CreateAndApplyPayment(double tot, double client, double collection, bool isManual, StringBuilder sb)
        {
            var invMgr = new InvoiceManager();
            var isAutoSplitPayment = true;
            var invoiceType = GetInvoiceType();
            if (invoiceType == tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_CASH ||
                invoiceType == tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_TRANSFER ||
                invoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CREDIT)
            {
                if (invoiceType == tblCustInkAktInvoice.INVOICE_TYPE_CREDIT && !isManual)
                {
                    /* By default: credit ECP Invoices (unless the user applies it manually [see: if (isManual) ..])*/
                    client = 0;
                    collection = tot;
                    isAutoSplitPayment = false;
                }
                if (isManual)
                {
                    if (HTBUtils.IsZero(client)  && HTBUtils.IsZero(collection))
                    {
                        sb.Append("You must enter either Klient or ECP or both when selecting manual application");
                        return -1;
                    }
                    if (!HTBUtils.IsZero(tot - (client + collection))) // if (tot == client + collection)
                    {
                        sb.Append("Klient + ECP must equal Betrag");
                        return -1;
                    }
                    if (HTBUtils.IsZero(client))
                        client = tot - collection;
                    else if (HTBUtils.IsZero(collection))
                        collection = tot - client;
                    isAutoSplitPayment = false;
                }
            }
            else
            {
                isAutoSplitPayment = false;
                client = tot;
                collection = 0;
            }
            tblCustInkAktInvoice payment = invMgr.CreatePayment(QryInkassoakt.CustInkAktID, invoiceType, tot);
            payment.InvoicePaymentReceivedDate = GetPaymentDate();
            payment.InvoiceComment = txtComment.Text;
            payment.InvoiceBillNumber = txtInvoiceBillNumber.Text;
            int payId = invMgr.SaveInvoice(payment);
            if (payId >= 0)
            {
                if (isAutoSplitPayment)
                    invMgr.ApplyPayment(payId);
                else
                    invMgr.ApplyPayment(payId, client, collection);
            }
            else
            {
                sb.Append("ERROR: Could not create payment");
            }
               
            return payId;
        }

        private int GetInvoiceType()
        {
            switch (ddlPaymentType.SelectedValue.Trim().ToLower())
            {
                case "transfer":
                    return tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_TRANSFER;
                case "payment_to_collector":
                    return tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_CASH;
                case "direct_payment":
                    return tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_DIRECT_TO_CLIENT;
                case "return":
                    return tblCustInkAktInvoice.INVOICE_TYPE_RETURNED;
                case "credit":
                    return tblCustInkAktInvoice.INVOICE_TYPE_CREDIT;
                case "debit":
                    return tblCustInkAktInvoice.INVOICE_TYPE_DEBIT;
                case "expense_client":
                    return tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST;
                case "interest_client":
                    return tblCustInkAktInvoice.INVOICE_TYPE_INTEREST_CLIENT;
            }
            return tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_TRANSFER;
        }

        protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int invoiceType = GetInvoiceType();
            ResetPage(false);
            switch (GetInvoiceType())
            {
                case tblCustInkAktInvoice.INVOICE_TYPE_CREDIT:
                case tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_TRANSFER:
                    // keep default settings
                    break;
                case tblCustInkAktInvoice.INVOICE_TYPE_PAYMENT_CASH:
                    trInvoiceBillNumber.Visible = true;
                    break;
                case tblCustInkAktInvoice.INVOICE_TYPE_DEBIT:
                    trDate.Visible = false;
                    trInvoiceBillNumber.Visible = false;
                    trManual.Visible = false;
                    break;
                default:
                    chkManual.Checked = false;
                    trInvoiceBillNumber.Visible = false;
                    trManual.Visible = false;
                    trClientAmount.Visible = false;
                    trCollectionAmount.Visible = false;
                    txtInvoiceBillNumber.Text = "";
                    txtClientAmount.Text = "";
                    txtCollectionAmount.Text = "";
                    break;
            }
        }

        protected void txtTotalPayment_TextChanged(object sender, EventArgs e)
        {
            double amountToSplit = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtTotalPayment.Text);
            if (amountToSplit > 0)
            {
                LoadInstallmentsList();
                for (int i = 0; i < _installmentsList.Count && amountToSplit > 0; i++)
                {
                    var installment = (qryCustInkRate)_installmentsList[i];
                    installment.TempReceivedAmount = amountToSplit >= installment.CustInkAktRateBalance ? installment.CustInkAktRateBalance : amountToSplit;
                    amountToSplit -= installment.TempReceivedAmount;
                }
                PopulateInstallmentsGrid();
            }
        }

        private DateTime GetPaymentDate()
        {
            DateTime dte = DateTime.Now;
            try
            {
                dte = Convert.ToDateTime(txtDate.Text);
            }
            catch{}

            return dte;
        }

        private void ResetPage(bool resetPaymentType = true)
        {
            trDate.Visible = true;
            trManual.Visible = true;
            trClientAmount.Visible = false;
            trCollectionAmount.Visible = false;
            trInvoiceBillNumber.Visible = false;
            if(resetPaymentType)
                ddlPaymentType.SelectedIndex = 0;
            
            txtTotalPayment.Text = ""; 
            txtClientAmount.Text = "";
            txtCollectionAmount.Text = "";
            txtInvoiceBillNumber.Text = "";
            txtComment.Text = "";
            txtDate.Text = "";
            LoadInstallmentsList();
            PopulateInstallmentsGrid();
        }

        private bool ValidateEntry()
        {
            bool ok = true;
            var sb = new StringBuilder();
            double totPayAppliedToInstallments = 0;
            double totPay = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtTotalPayment.Text);
            if (totPay <= 0)
            {
                sb.Append("You must enter a Betrag Amount.<br/>");
                ok = false;
            }
            if (gvInstallments.Rows.Count > 0)
            {
                for (int i = 0; i < gvInstallments.Rows.Count; i++)
                {
                    GridViewRow row = gvInstallments.Rows[i];
                    var txtReceivedAmount = (TextBox)row.FindControl("txtReceivedAmount");
                    double receivedAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtReceivedAmount.Text);

                    totPayAppliedToInstallments += receivedAmount;
                    if (receivedAmount > 0)
                    {
                        var lblInstallmentID = (Label)row.FindControl("lblInstallmentId");
                        tblCustInkAktRate installment = GetInstallmentByID(lblInstallmentID.Text);

                        if (installment != null && installment.CustInkAktRateBalance < receivedAmount)   
                        {
                            sb.Append("Rate [" + installment.CustInkAktRateDueDate.ToShortDateString() + "]: Betrag (Eingang) ist größer als Betrag.<br/>");
                            ok = false;
                        }
                    }
                }
                /* Skip this validation (causing headaches)
                if (!HTBUtils.IsZero(totPayAppliedToInstallments - totPay)) // totPayAppliedToInstallments != totPay
                {
                    sb.Append("Total rate distributed must be the same amout as the payment.<br/>");
                    ok = false;
                }
                */
            }
            if (!ok)
                ctlMessage.ShowError(sb.ToString());
            return ok;
        }
        
        #region Installments
        private void LoadInstallmentsList()
        {
            string where = "CustInkAktRateAktId = " + QryInkassoakt.CustInkAktID + " AND (CustInkAktRateBalance > 0)";
            const string orderBy = "CustInkAktRatePostponeTillDate, CustInkAktRateDueDate";
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
                dr["RateAmount"] = HTBUtils.FormatCurrency(installment.CustInkAktRateBalance);
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
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("InstallmentID", typeof(int)));
            dt.Columns.Add(new DataColumn("AktNumber", typeof(int)));
            dt.Columns.Add(new DataColumn("RateDueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("RateAmount", typeof(string)));
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

        private void UpdateInstallments(int payId)
        {

            var invMgr = new InvoiceManager();
            var payment = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + payId, typeof(tblCustInkAktInvoice));
            if (payment != null && payment.IsPayment())
            {
                var set = new RecordSet();
                for (int i = 0; i < gvInstallments.Rows.Count; i++)
                {
                    GridViewRow row = gvInstallments.Rows[i];
                    var txtReceivedAmount = (TextBox)row.FindControl("txtReceivedAmount");
                    double receivedAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtReceivedAmount.Text);
                    if (receivedAmount > 0)
                    {
                        var lblInstallmentID = (Label)row.Cells[0].FindControl("lblInstallmentId");
                        tblCustInkAktRate installment = GetInstallmentByID(lblInstallmentID.Text);

                        if (installment != null)
                        {
                            var rec = new tblCustInkAktRate();
                            rec.Assign(installment);
                            rec.CustInkAktRateInvoiceID = payId;
                            rec.CustInkAktRateLastChanged = DateTime.Now;
                            set.UpdateRecord(rec);

                            invMgr.ApplyInstallmentAmount(payment, rec, receivedAmount);
                        }
                    }
                }
            }
        }
        private tblCustInkAktRate GetInstallmentByID(string installmentId)
        {
            return GetInstallmentByID(GlobalUtilArea.GetZeroIfConvertToIntError(installmentId));
        }
        private tblCustInkAktRate GetInstallmentByID(int installmentId)
        {
            return _installmentsList.Cast<tblCustInkAktRate>().FirstOrDefault(installment => installment.CustInkAktRateID == installmentId);
        }

        #endregion

    }
}