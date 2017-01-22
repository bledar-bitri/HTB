using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.aktenink
{
    public partial class EditInkAction : System.Web.UI.Page
    {
        tblCustInkAktAktion action;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].ToString().Trim().Equals(string.Empty))
            {
                action = (tblCustInkAktAktion)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblCustInkAktAktion WHERE CustInkAktAktionID = " + Request.QueryString["ID"], typeof(tblCustInkAktAktion));
                if (!IsPostBack && action != null)
                {
                    SetValues();
                }
            }
        }

        private void SetValues()
        {
            ctlMessage.Clear();
            if (action != null)
            {
                lblActionId.Text = action.CustInkAktAktionID.ToString();
                lblAktId.Text = action.CustInkAktAktionAktID.ToString();
                txtCaption.Text = action.CustInkAktAktionCaption;
                txtMemo.Text = action.CustInkAktAktionMemo;
                txtActionDate.Text = action.CustInkAktAktionDate.ToShortDateString();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action != null)
            {
                action.CustInkAktAktionDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtActionDate.Text);
                if (action.CustInkAktAktionDate.ToShortDateString() == HTBUtils.DefaultShortDate)
                    action.CustInkAktAktionDate = DateTime.Now;
                action.CustInkAktAktionCaption = txtCaption.Text;
                action.CustInkAktAktionMemo = txtMemo.Text;
                action.CustInkAktAktionUserId = GlobalUtilArea.GetUserId(Session);
                try
                {
                    if (RecordSet.Update(action))
                    {
                        CloseWindowAndRefresh();
                    }
                    else
                    {
                        ctlMessage.ShowError("Could Not Update Action");
                    }
                }
                catch (Exception ex)
                {
                    ctlMessage.ShowError(ex.Message);
                    ctlMessage.AppendError("<BR/>"+ex.StackTrace);
                }
            }
            else
            {
                ctlMessage.ShowError("Could Not Find Action");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWindow();
        }

        private void CloseWindowAndRefresh()
        {
            ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "MM_refreshParentAndClose();", true);
        }

        private void CloseWindow()
        {
            ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "window.close();", true);
        }
    }
}