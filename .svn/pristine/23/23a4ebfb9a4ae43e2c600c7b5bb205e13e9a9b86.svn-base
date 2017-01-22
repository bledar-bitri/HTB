using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBUtilities;

namespace HTB.v2.intranetx.bank
{
    public partial class BankTransferOLD : System.Web.UI.Page
    {

        ArrayList _paymentsList = new ArrayList();
        private double _totalAmount;
        private readonly tblControl _control = HTBUtils.GetControlRecord();

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadPaymentList();
            // DO NOT RE-BIND THE GRID ON POSTBACK (otherwise we lose the data entry)
            if (!IsPostBack)
                SetValues();
        }

        private void LoadPaymentList()
        {
            _paymentsList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkUberwiesung", typeof(qryCustInkUberwiesung));
        }
        private void SetValues()
        {
            PopulateAppliedGrid();
        }

        private double GetAppliedToClientAmount(int invId)
        {
            string sql = "SELECT * FROM qryCustInkAktInvoiceApplyFrom WHERE ApplyFromInvoiceId = " + invId;

            ArrayList applyList = HTBUtils.GetSqlRecords(sql, typeof(qryCustInkAktInvoiceApply));
            return (from qryCustInkAktInvoiceApply applyRec in applyList where applyRec.IsClientInvoice() select applyRec.ApplyAmount).Sum();
        }

        private double GetAppliedToClientInterest(int invId)
        {
            string sql = "SELECT * FROM qryCustInkAktInvoiceApplyFrom WHERE ApplyFromInvoiceId = " + invId;

            ArrayList applyList = HTBUtils.GetSqlRecords(sql, typeof(qryCustInkAktInvoiceApply));
            return (from qryCustInkAktInvoiceApply applyRec in applyList where applyRec.IsClientInterest() select applyRec.ApplyAmount).Sum();
        }
        
        private double GetCollectionAmount(int invId)
        {
            string sql = "SELECT * FROM qryCustInkAktInvoiceApplyFrom WHERE ApplyFromInvoiceId = " + invId;

            ArrayList applyList = HTBUtils.GetSqlRecords(sql, typeof(qryCustInkAktInvoiceApply));
            return (from qryCustInkAktInvoiceApply applyRec in applyList where !applyRec.IsClientInvoice() select applyRec.ApplyAmount).Sum();
        }

        private void PopulateAppliedGrid()
        {
            DataTable dt = GetGridDataTableStructure();
            _totalAmount = 0;
            foreach (qryCustInkUberwiesung rec in _paymentsList)
            {
                double appliedToClientAmount = GetAppliedToClientAmount(rec.InvoiceID);
                double collectionAmount = GetCollectionAmount(rec.InvoiceID);
                double toTransferAmount = appliedToClientAmount;
                if(!rec.KlientReceivesInterest)
                {
                    toTransferAmount -= GetAppliedToClientInterest(rec.InvoiceID);

                }
                if (toTransferAmount > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["InvoiceID"] = rec.InvoiceID;
                    dr["AktNumber"] = rec.CustInkAktAZ.Trim() == string.Empty ?  rec.InvoiceCustInkAktId.ToString() : rec.CustInkAktAZ.Trim();
                    dr["Note"] = string.IsNullOrEmpty(rec.CustInkAktGothiaNr) ? rec.CustInkAktKunde : rec.CustInkAktKunde +"<br/>Kundennummer: " + rec.CustInkAktGothiaNr;
                    dr["PaymentReceivedDate"] = rec.InvoicePaymentReceivedDate.ToShortDateString();
                    dr["PaymentAmount"] = HTBUtils.FormatCurrency(rec.InvoiceAmount);
                    dr["CollectionAmount"] = HTBUtils.FormatCurrency(collectionAmount);
                    dr["NameSchuldner"] = rec.GegnerName1 + "  " + rec.GegnerName2;
                    dr["ClientName"] = rec.KlientName1 + "  " + rec.KlientName2;

                    dr["BankAccountNumber"] = rec.KlientBankCaption1.Trim()+"<br/>"+rec.KlientBLZ1+" - "+ rec.KlientKtoNr1;
                    dr["TransferDate"] = DateTime.Now.ToShortDateString();
                    dr["TransferAmount"] = HTBUtils.FormatCurrencyNumber(toTransferAmount);
                    _totalAmount += toTransferAmount;
                    dt.Rows.Add(dr);
                }
            }
            gvTransfers.DataSource = dt;
            gvTransfers.DataBind();
        }

        private DataTable GetGridDataTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("InvoiceID", typeof(int)));
            dt.Columns.Add(new DataColumn("AktNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("Note", typeof(string)));
            dt.Columns.Add(new DataColumn("PaymentReceivedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("PaymentAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("CollectionAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("NameSchuldner", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("BankAccountNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferAmount", typeof(string)));
            
            return dt;
        }

        private void SetTransferred(int invId, DateTime transferredDate, double transferredAmount)
        {
            var set = new RecordSet();
            var inv = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + invId, typeof(tblCustInkAktInvoice));
            inv.InvoicePaymentTransferToClientDate = transferredDate;
            inv.InvoicePaymentTransferToClientAmount = transferredAmount;

            set.UpdateRecord(inv);

            if (HTBUtils.IsZero(new AktUtils(inv.InvoiceCustInkAktId).GetAktBalance()))
            {
                var akt = HTBUtils.GetInkassoAkt(inv.InvoiceCustInkAktId);
                akt.CustInkAktStatus = _control.InkassoAktFertigStatus;
                akt.CustInkAktCurStatus = 25; // Erledigt: Fal abgeschlossen
                set.UpdateRecord(akt);
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            lblMessage.Text = "";
            for (int i = 0; i < gvTransfers.Rows.Count; i++)
            {
                GridViewRow row = gvTransfers.Rows[i];
                var lblInvoiceId = (Label)row.Cells[0].FindControl("lblInvoiceId");
                var txtTransDate = (TextBox)row.Cells[10].FindControl("txtTransferDate");
                var txtTransAmount = (TextBox)row.Cells[10].FindControl("txtTransferAmmount");
                var chkTransferred = (CheckBox)row.Cells[11].FindControl("chkTransferred");
                int invId = Convert.ToInt32(lblInvoiceId.Text);
                double amount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtTransAmount.Text);
                sb.Append(invId);
                sb.Append(":  [");
                sb.Append(amount);
                sb.Append(chkTransferred.Checked ? " Y " : " N ");
                sb.Append("]    ");
                if (chkTransferred.Checked)
                {
                    DateTime transferDate = DateTime.Now;
                    double transferAmount = 0;
                    try
                    {
                        transferDate = Convert.ToDateTime(txtTransDate.Text);
                        transferAmount = Convert.ToDouble(txtTransAmount.Text);
                    }
                    catch
                    {
                        sb.Append(" ERROR: Invalid Entry");
                    }
                    try
                    {
                        SetTransferred(invId, transferDate, transferAmount);
                    }
                    catch
                    {
                        sb.Append(" ERROR WHILE SAVING TO DB");
                    }
                }
            }
            LoadPaymentList();
            PopulateAppliedGrid(); // reload grid
            lblMessage.Text = sb.ToString();
        }

        protected string GetTotalAmountString()
        {
            return HTBUtils.FormatCurrency(_totalAmount, true);
        }
    }
}