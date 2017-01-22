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
using HTB.Database.HTB.StoredProcs;

namespace HTB.v2.intranetx.bank
{
    public partial class BankTransfer : System.Web.UI.Page
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
            _paymentsList = HTBUtils.GetStoredProcedureRecords("spGetBankTransferList", null, typeof(spGetBankTransferList));
        }
        private void SetValues()
        {
            PopulateAppliedGrid();
        }

        
        private void PopulateAppliedGrid()
        {
            DataTable dt = GetGridDataTableStructure();
            _totalAmount = 0;
            foreach (spGetBankTransferList rec in _paymentsList)
            {
                string bankInfo = GlobalUtilArea.GetEmptyIfNull(rec.TransferToBankCaption).Trim() + "<br/>" + GlobalUtilArea.GetEmptyIfNull(rec.TransferToBLZ) + " - " + GlobalUtilArea.GetEmptyIfNull(rec.TransferToKtoNr);
                double ecpAmount = rec.InvoiceAmount - rec.ToTransferAmount;
                var note = string.IsNullOrEmpty(rec.CustInkAktGothiaNr) ? rec.CustInkAktKunde : rec.CustInkAktKunde + "<br/>Kundennummer: " + rec.CustInkAktGothiaNr;
                if (rec.InvoiceTransferID > 0)
                {
                    ecpAmount = 0;
                    note = "Provision";
                }
                DataRow dr = dt.NewRow();
                dr["InvoiceID"] = rec.InvoiceId;
                dr["AktNumber"] = string.IsNullOrEmpty(rec.AktAZ) ? rec.AktId.ToString() : rec.AktAZ.Trim();
                dr["Note"] = note;
                dr["PaymentReceivedDate"] = rec.InvoicePaymentReceivedDate.ToShortDateString();
                dr["PaymentAmount"] = HTBUtils.FormatCurrency(rec.InvoiceAmount);
                dr["CollectionAmount"] = HTBUtils.FormatCurrency(ecpAmount);
                dr["NameSchuldner"] = rec.GegnerName;
                dr["ClientName"] = rec.TransferToName;
            
                dr["BankAccountNumber"] = bankInfo;
                dr["TransferDate"] = DateTime.Now.ToShortDateString();
                dr["TransferAmount"] = HTBUtils.FormatCurrencyNumber(rec.ToTransferAmount);
                dr["InvoiceTransferID"] = rec.InvoiceTransferID.ToString();
                _totalAmount += rec.ToTransferAmount;
                dt.Rows.Add(dr);
            }
            gvTransfers.DataSource = dt;
            gvTransfers.DataBind();
        }

        private DataTable GetGridDataTableStructure()
        {
            var dt = new DataTable();
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
            dt.Columns.Add(new DataColumn("InvoiceTransferID", typeof(string)));
            
            return dt;
        }

        private void SetTransferred(int invId, DateTime transferredDate, double transferredAmount, int invoiceTransferID)
        {
            if (invoiceTransferID > 0)
            {
                var sql = new StringBuilder("UPDATE tblCustInkAktInvoiceTransfer SET InvoiceTransferAmount = ");
                sql.Append(HTBUtils.FormatCurrencyNumber(transferredAmount).Replace(",", "."));
                sql.Append(", InvoiceTransferDate = '");
                sql.Append(transferredDate.ToShortDateString());
                sql.Append("', InvoiceTransferStatus = 1");
                sql.Append(" WHERE InvoiceTransferID = ");
                sql.Append(invoiceTransferID);
                new RecordSet().ExecuteNonQuery(sql.ToString());
            }
            else
            {
                var set = new RecordSet();
                var inv = (tblCustInkAktInvoice) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + invId, typeof (tblCustInkAktInvoice));
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
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            lblMessage.Text = "";
            for (int i = 0; i < gvTransfers.Rows.Count; i++)
            {
                GridViewRow row = gvTransfers.Rows[i];
                var lblInvoiceId = (Label)row.Cells[0].FindControl("lblInvoiceId");
                var lblInvoiceTransferID = (Label)row.Cells[0].FindControl("lblInvoiceTransferID");
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
                    int invTransferId = 0;
                
                    try
                    {
                        transferDate = Convert.ToDateTime(txtTransDate.Text);
                        transferAmount = Convert.ToDouble(txtTransAmount.Text);
                        invTransferId = Convert.ToInt32(lblInvoiceTransferID.Text);
                    }
                    catch
                    {
                        sb.Append(" ERROR: Invalid Entry");
                    }
                    try
                    {
                        SetTransferred(invId, transferDate, transferAmount, invTransferId);
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