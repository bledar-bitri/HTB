using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;
using HTB.v2.intranetx.util;
using System.Drawing;
using System.Text;

namespace HTB.v2.intranetx.aktenintprovfix
{
    public partial class EditHonorar : System.Web.UI.Page
    {
        string id;
        tblAktenIntHonorar honorar;
        bool isNew = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].ToString().Trim().Equals(""))
            {
                id = Request.QueryString["ID"];
                honorar = (tblAktenIntHonorar)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntHonorar WHERE AktIntHonId = " + id, typeof(tblAktenIntHonorar));
                if (honorar == null)
                    isNew = true;
                else if (!IsPostBack)
                {
                    SetValues();
                }
            }
            else
            {
                isNew = true;
            }
        }

        private void SetValues()
        {
            txtFrom.Text = HTBUtils.FormatCurrencyNumber(honorar.AktIntHonFrom);
            txtTo.Text = HTBUtils.FormatCurrencyNumber(honorar.AktIntHonTo);
            txtPrice.Text = HTBUtils.FormatCurrencyNumber(honorar.AktIntHonPrice);
            txtMaxPrice.Text = HTBUtils.FormatCurrencyNumber(honorar.AktIntHonMaxPrice);
            txtPct.Text = honorar.AktIntHonPct.ToString();
            txtProvAmount.Text = HTBUtils.FormatCurrencyNumber(honorar.AktIntHonProvAmount);
            txtMaxProvAmount.Text = HTBUtils.FormatCurrencyNumber(honorar.AktIntHonMaxProvAmount);
            txtProvPct.Text = honorar.AktIntHonProvPct.ToString();

            ddlPercentOf.SelectedValue = honorar.AktIntHonPctOf.ToString();
            ddlProvPercentOf.SelectedValue = honorar.AktIntHonProvPctOf.ToString();
        }

        private void LoadValues()
        {
            if (isNew)
                honorar = new tblAktenIntHonorar();

            honorar.AktIntHonFrom = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtFrom.Text);
            honorar.AktIntHonTo = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtTo.Text);
            honorar.AktIntHonPrice = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtPrice.Text);
            honorar.AktIntHonMaxPrice = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtMaxPrice.Text);
            honorar.AktIntHonPct = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtPct.Text);
            honorar.AktIntHonProvAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtProvAmount.Text);
            honorar.AktIntHonProvPct = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtProvPct.Text);
            honorar.AktIntHonMaxProvAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtMaxProvAmount.Text);

            honorar.AktIntHonPctOf = GlobalUtilArea.GetZeroIfConvertToIntError(ddlPercentOf.SelectedValue);
            honorar.AktIntHonProvPctOf = GlobalUtilArea.GetZeroIfConvertToIntError(ddlProvPercentOf.SelectedValue);
        }

        private bool IsHonorarValid()
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();
            if (honorar.AktIntHonFrom < 0)
            {
                sb.Append("Betrag von (falsch)<BR>");
                txtFrom.BackColor = Color.Beige;
                ok = false;
            }
            if (honorar.AktIntHonTo <= 0)
            {
                sb.Append("Betrag bis (falsch)<BR>");
                txtTo.BackColor = Color.Beige;
                ok = false;
            }
            if (!ok)
                ShowError(sb.ToString());
            return ok;
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LoadValues();
            if (IsHonorarValid())
            {
                if (isNew)
                    RecordSet.Insert(honorar);
                else
                    RecordSet.Update(honorar);

                if (Request.QueryString["IsPopUp"] != null && Request.QueryString["IsPopUp"].ToString().Trim().ToLower().Equals("y"))
                {
                    ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "MM_refreshParentAndClose();", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        #endregion

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
    }
}