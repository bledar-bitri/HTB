using System;
using HTB.Database;
using System.Collections;
using System.Data;
using HTB.Database.StoredProcs;
using HTBUtilities;
using HTB.Database.Views;

namespace HTB.v2.intranetx.aktenink
{
    public partial class ShowInvoice : System.Web.UI.Page
    {
        public qryCustAktEdit qryInkassoakt = new qryCustAktEdit();
        public tblCustInkAktInvoice inv = new tblCustInkAktInvoice();
        public double totalApplied = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["InvID"] != null && !Request.QueryString["InvID"].ToString().Trim().Equals(""))
            {
                inv = (tblCustInkAktInvoice)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceID = " + Request.QueryString["InvID"], typeof(tblCustInkAktInvoice));
                qryInkassoakt = (qryCustAktEdit)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustAktEdit WHERE CustInkAktID = " + inv.InvoiceCustInkAktId, typeof(qryCustAktEdit));
            }
            SetValues();
        }

        private void SetValues()
        {
            lblMessage.Text = "&nbsp;";
            lblInvoiceType.Text = HTBUtils.GetHtmlSpaceForEmpty(inv.InvoiceDescription);
            lblInvoiceDate.Text = inv.InvoiceDate.ToShortDateString();
            lblInvoiceAmount.Text = HTBUtils.FormatCurrency(inv.InvoiceAmount);
            lblInvoiceBalance.Text = HTBUtils.FormatCurrency(inv.InvoiceBalance);
            lblCustInkAktKunde.Text = HTBUtils.GetHtmlSpaceForEmpty(inv.InvoiceClientReference);

            if (!string.IsNullOrEmpty(inv.InvoiceComment))
            {
                lblComment.Text = HTBUtils.GetHtmlSpaceForEmpty(inv.InvoiceComment.Replace("\n", "<br>"));
                trComment.Visible = true;
            }
            else
                lblComment.Text = "&nbsp;";

            if (inv.IsPayment())
            {
                lblPaymentReceivedDate.Text = inv.InvoicePaymentReceivedDate.ToShortDateString();
                if(inv.InvoicePaymentTransferToClientAmount > 0)
                {
                    lblInvoicePaymentTransferToClientDate.Text = inv.InvoicePaymentTransferToClientDate.ToShortDateString(); 
                    lblInvoicePaymentTransferToClientAmount.Text = HTBUtils.FormatCurrency(inv.InvoicePaymentTransferToClientAmount);
                    trInvoicePaymentTransferToClientDate.Visible = true;
                    trInvoicePaymentTransferToClientAmount.Visible = true;
                }
                trPaymentReceivedDate.Visible = true;
            }
            else
            {
                lblInvoiceAmountNetto.Text = HTBUtils.FormatCurrency(inv.InvoiceAmountNetto);
                lblTax.Text = HTBUtils.FormatCurrency(inv.InvoiceTax);
                trInvoiceAmountNetto.Visible = true;
                trTax.Visible = true;
            }
            PopulateAppliedGrid(GetApplied());
            PopulateAppliedInstallmentsGrid();
        }

        private ArrayList GetApplied()
        {
            ArrayList applyList;
            string qryName = "qryCustInkAktInvoiceApplyFrom";
            string fieldName = "ApplyFromInvoiceId";

            if (!inv.IsPayment())
            {
                qryName = "qryCustInkAktInvoiceApplyTo";
                fieldName = "ApplyToInvoiceId";
            }
            string sql = "SELECT * FROM "+qryName +" WHERE "+fieldName+" = "+inv.InvoiceID+"ORDER BY ApplyDate";

            applyList = HTBUtils.GetSqlRecords(sql, typeof(qryCustInkAktInvoiceApply));

            return applyList;
        }

        private void PopulateAppliedGrid(ArrayList applyList)
        {
            totalApplied = 0;
            DataTable dt = GetAppliedDataTableStructure();
            foreach (qryCustInkAktInvoiceApply appInv in applyList)
            {
                DataRow dr = dt.NewRow();
                dr["InvoiceID"] = appInv.InvoiceID;
                dr["InvoiceDate"] = appInv.InvoiceDate.ToShortDateString();
                dr["InvoiceDescription"] = appInv.InvoiceDescription;
                dr["AppliedAmount"] = appInv.ApplyAmount;
                dr["InvoiceAmount"] = HTBUtils.FormatCurrency(appInv.InvoiceAmount);
                dr["InvoiceBalance"] = HTBUtils.FormatCurrency(appInv.InvoiceBalance);
                dt.Rows.Add(dr);
                totalApplied += appInv.ApplyAmount;
            }
            gvAppliedTo.DataSource = dt;
            gvAppliedTo.DataBind();
        }

        private DataTable GetAppliedDataTableStructure()
        {
            DataTable dt = new DataTable();
            DataColumn dc;

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.Int32");
            dc.ColumnName = "InvoiceID";
            dt.Columns.Add(dc);
            
            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "InvoiceDate";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "InvoiceDescription";
            dt.Columns.Add(dc);
            
            dc = new DataColumn();
            dc.DataType = Type.GetType("System.Double");
            dc.ColumnName = "AppliedAmount";
            dt.Columns.Add(dc);dc = new DataColumn();

            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "InvoiceBalance";
            dt.Columns.Add(dc);dc = new DataColumn();

            dc.DataType = Type.GetType("System.String");
            dc.ColumnName = "InvoiceAmount";
            dt.Columns.Add(dc);

            return dt;
        }

        public double GetTotalApplied()
        {
            return totalApplied;
        }

        #region Installments Read Only (Applied)
        private ArrayList LoadAppliedInstallmentsList()
        {
            const string spName = "spInvoiceAppliedToRate";
            var list = new ArrayList();
            list.Add(new StoredProcedureParameter("invoiceID", SqlDbType.Int, inv.InvoiceID));

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
            return dt;
        }
        #endregion

    }
}