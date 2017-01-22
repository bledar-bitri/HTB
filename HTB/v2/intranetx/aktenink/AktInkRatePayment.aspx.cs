using System;
using System.Web.UI.WebControls;
using System.Data;
using HTB.Database;
using System.Collections;
using HTB.v2.intranetx.util;
using System.Text;
using HTBUtilities;
using HTB.Database.Views;

namespace HTB.v2.intranetx.aktenink
{
    public partial class AktInkRatePayment : System.Web.UI.Page
    {
        internal ArrayList InstallmentsList = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadInstallmentsList();
            // DO NOT REBIND THE GRID ON POSTBACK (otherwise we lose the data entry)
            if (!IsPostBack)
                SetValues();
        }

        private void LoadInstallmentsList()
        {

            DateTime startDate = DateTime.Now.AddDays(-5);
            DateTime endDate = DateTime.Now.AddDays(5);
            String where = "CustInkAktRateDueDate BETWEEN '" + startDate.ToShortDateString() + "' AND '" + endDate.ToShortDateString() + "'";
            //where += " OR (CustInkAktRateDueDate < "+ startDate.ToShortDateString()
     
            InstallmentsList = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkRate WHERE "+where, typeof(qryCustInkRate));
        }
        private void SetValues()
        {
            PopulateAppliedGrid();
        }

        
        private void PopulateAppliedGrid()
        {
            DataTable dt = GetGridDataTableStructure();
            foreach (qryCustInkRate appInv in InstallmentsList)
            {
                DataRow dr = dt.NewRow();
                dr["RateID"] = appInv.CustInkAktRateID;
                dr["AktNumber"] = appInv.CustInkAktRateAktID;
                dr["RateDueDate"] = appInv.CustInkAktRateDueDate.ToShortDateString();
                dr["RateAmount"] = HTBUtils.FormatCurrency(appInv.CustInkAktRateAmount);
                dr["NameSchuldner"] = appInv.GegnerName1 + "  " + appInv.GegnerName2;
                dr["ClientName"] = appInv.KlientName1 + "  " + appInv.KlientName2;
                dr["BankAccountNumber"] = appInv.KlientKtoNr1;
                dr["ReceivedDate"] = DateTime.Now.ToShortDateString();
                dr["ReceivedAmount"] = HTBUtils.FormatCurrency(appInv.CustInkAktRateAmount);
                dt.Rows.Add(dr);
            }
            gvTransfers.DataSource = dt;
            gvTransfers.DataBind();
        }

        private DataTable GetGridDataTableStructure()
        {
            var dt = new DataTable();
            
            dt.Columns.Add(new DataColumn("RateID", typeof(int)));
            dt.Columns.Add(new DataColumn("AktNumber", typeof(int)));
            dt.Columns.Add(new DataColumn("RateDueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("RateAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("NameSchuldner", typeof(string)));
            dt.Columns.Add(new DataColumn("ClientName", typeof(string)));
            dt.Columns.Add(new DataColumn("BankAccountNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceivedAmount", typeof(string)));

            return dt;
        }

        private void SetTransferred(int invId, DateTime transferredDate, double transferredAmount)
        {
            var inv = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + invId, typeof(tblCustInkAktInvoice));
            inv.InvoicePaymentTransferToClientDate = transferredDate;
            inv.InvoicePaymentTransferToClientAmount = transferredAmount;
            var set = new RecordSet();
            set.UpdateRecord(inv);
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
                    LoadInstallmentsList();
                    PopulateAppliedGrid(); // reload grid
                }
                    
            }
            lblMessage.Text = sb.ToString();
        }
    }
}