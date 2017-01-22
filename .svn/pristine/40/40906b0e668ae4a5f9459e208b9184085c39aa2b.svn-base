using System;
using System.Collections;
using System.Data;
using System.Text;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.bank
{
    public partial class BankTransactions : System.Web.UI.Page
    {
        private int _id;
        private bool _isNew;
        private tblBankAccountTransaction _record;
        protected void Page_Load(object sender, EventArgs e)
        {
            _id = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.ID]); 
            _isNew = _id <= 0;
            if(!IsPostBack)
            {
                if(_id > 0)
                {
                    _record = (tblBankAccountTransaction) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblBankAccountTransaction WHERE BankKtoTransactionID = " + _id, typeof (tblBankAccountTransaction)) ?? new tblBankAccountTransaction();
                    SetFormValues();
                }
                else
                {
                    txtDate.Text = DateTime.Now.ToShortDateString();
                }
            }
            PopulateTransactionsGrid();
        }
        #region Event Handlers
        protected void chkIsTransactionIncoming_CheckedChanged(object sender, EventArgs e)
        {
            ShowIncommingFields();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsFormValid())
                {
                    string from = "ECP";
                    string to = txtTo.Text;
                    if(chkIsTransactionIncoming.Checked)
                    {
                        from = txtFrom.Text;
                        to = "ECP";
                    }
                    var rec = new tblBankAccountTransaction
                                  {
                                      BankKtoTransactionDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDate),
                                      BankKtoTransactionFrom = from,
                                      BankKtoTransactionTo = to,
                                      BankKtoTransactionDescription = txtDescription.Text,
                                      BankKtoTransactionAmount =  GlobalUtilArea.GetZeroIfConvertToDoubleError(txtAmount),
                                      BankKtoIsTransactionIncoming = chkIsTransactionIncoming.Checked
                                  };
                    if (!_isNew)
                    {
                        rec.BankKtoTransactionID = _id;
                        RecordSet.Update(rec);
                    }
                    else
                    {
                        RecordSet.Insert(rec); 
                    }
                    Response.Redirect("BankTransactions.aspx");
                }
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("BankTransactions.aspx");
        }

        #endregion
        
        private void SetFormValues()
        {
            if(!_isNew && _record != null)
            {
                lblID.Text = _record.BankKtoTransactionID.ToString(); 
                txtDate.Text = HTBUtils.IsDateValid(_record.BankKtoTransactionDate) ? _record.BankKtoTransactionDate.ToShortDateString() : "";
                txtTo.Text = _record.BankKtoTransactionTo;
                txtDescription.Text = _record.BankKtoTransactionDescription;
                txtAmount.Text = HTBUtils.FormatCurrencyNumber(_record.BankKtoTransactionAmount);
                chkIsTransactionIncoming.Checked = _record.BankKtoIsTransactionIncoming;
                txtFrom.Text = _record.BankKtoTransactionFrom;
                lblFrom.Text = _record.BankKtoTransactionFrom;

                txtTo.Text = _record.BankKtoTransactionTo;
                lblTo.Text = _record.BankKtoTransactionTo;

                trID.Visible = true;
            }
            else
            {
                lblID.Text = "";
                txtDate.Text = "";
                txtFrom.Text = "";
                chkIsTransactionIncoming.Checked = false;
                txtTo.Text = "";
                txtDescription.Text = "";
                txtAmount.Text = "";
            }
            ShowIncommingFields();
        }
        private bool IsFormValid()
        {
            DateTime dte = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDate);
            if(!HTBUtils.IsDateValid(dte))
            {
                ctlMessage.ShowError("Datum ist Ung&uuml;ltig!");
                return false;
            }

            double amount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtAmount);
            if(HTBUtils.IsZero(amount))
            {
                ctlMessage.ShowError("Betrag ist Ung&uuml;ltig!");
                return false;
            }
            return true;

        }
        
        private void ShowIncommingFields()
        {
            // just to be on the safe side of things
            lblFrom.Text = "ECP";
            lblTo.Text = "ECP";

            // move on with the program
            txtFrom.Visible = chkIsTransactionIncoming.Checked;
            lblFrom.Visible = !chkIsTransactionIncoming.Checked;

            txtTo.Visible = !chkIsTransactionIncoming.Checked;
            lblTo.Visible = chkIsTransactionIncoming.Checked;
        }
        #region Transaction Grid
        private void PopulateTransactionsGrid()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblBankAccountTransaction order by BankKtoTransactionDate desc", typeof(tblBankAccountTransaction));
            DataTable dt = GetTransactionsDataTableStructure();
            foreach (tblBankAccountTransaction rec in list)
            {
                DataRow dr = dt.NewRow();

                var sbDel = new StringBuilder("<a href=\"javascript:void(window.open('");
                // the actual link
                sbDel.Append("/v2/intranetx/global_forms/GlobalDelete.aspx?strTable=tblBankAccountTransaction&frage=Sind%20Sie%20sicher,%20dass%20sie%20diese%20Buchung%20l&#246;schen%20wollen?&strTextField=BankKtoTransactionDescription&strColumn=BankKtoTransactionID&ID=");
                sbDel.Append(rec.BankKtoTransactionID);
                // continue with the popup params
                sbDel.Append("','_blank','toolbar=no,menubar=no'))\">");
                sbDel.Append("<img src=\"../../intranet/images/delete2hover.gif\" width=\"16\" height=\"16\" alt=\"L&ouml;scht diesen Datensatz.\" style=\"border-color:White;border-width:0px;\"/>");
                sbDel.Append("</a>");


                var sbEdit = new StringBuilder("<a href=\"BankTransactions.aspx?");
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sbEdit, GlobalHtmlParams.ID, rec.BankKtoTransactionID.ToString(), false);
                sbEdit.Append("\">");
                sbEdit.Append("<img src=\"../../intranet/images/edit.gif\" width=\"16\" height=\"16\" alt=\"&Auml;ndern diese Buchung.\" style=\"border-color:White;border-width:0px;\"/>");
                sbEdit.Append("</a>");

                dr["DeleteUrl"] = sbDel.ToString();
                dr["EditUrl"] = sbEdit.ToString();
                dr["Date"] = rec.BankKtoTransactionDate.ToShortDateString();
                dr["From"] = rec.BankKtoTransactionFrom;
                dr["To"] = rec.BankKtoTransactionTo;
                dr["Description"] = rec.BankKtoTransactionDescription;
                dr["Amount"] = HTBUtils.FormatCurrency(rec.BankKtoTransactionAmount);                
                dt.Rows.Add(dr);
            }
            gvTransactions.DataSource = dt;
            gvTransactions.DataBind();
        }

        private DataTable GetTransactionsDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(string)));
            dt.Columns.Add(new DataColumn("From", typeof(string)));
            dt.Columns.Add(new DataColumn("To", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            return dt;
        }
        #endregion
    }
}