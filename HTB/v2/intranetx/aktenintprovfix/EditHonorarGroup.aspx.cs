using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBUtilities;
using System.Text;
using System.Drawing;

namespace HTB.v2.intranetx.aktenintprovfix
{
    public partial class EditHonorarGroup : System.Web.UI.Page
    {
        string id;
        tblAktenIntHonorarGroup group;
        bool isNew = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].ToString().Trim().Equals(""))
            {
                id = Request.QueryString["ID"];
                group = (tblAktenIntHonorarGroup)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntHonorarGroup WHERE AktIntHonGrpId = " + id, typeof(tblAktenIntHonorarGroup));
                if (group == null)
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
            txtCaption.Text = group.AktIntHonGrpCaption;
        }

        private void LoadValues()
        {
            if (isNew)
                group = new tblAktenIntHonorarGroup();

            group.AktIntHonGrpCaption = txtCaption.Text;
        }

        private bool IsGroupValid()
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();
            if (group.AktIntHonGrpCaption.Trim() == string.Empty)
            {
                sb.Append("Beschreibung (falsch)<BR>");
                txtCaption.BackColor = Color.Beige;
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
            if (IsGroupValid())
            {
                if (isNew)
                    RecordSet.Insert(group);
                else
                    RecordSet.Update(group);

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