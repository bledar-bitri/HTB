using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System.Drawing;
using System.Text;

namespace HTB.v2.intranetx.gegner
{
    public partial class EditGegner : System.Web.UI.Page
    {
        string gegnerId = "";
        tblGegner gegner;
        bool submitFormOnClose = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.ID]).ToString().Trim().Equals(string.Empty))
            {
                gegnerId = Request.QueryString[GlobalHtmlParams.ID];
                submitFormOnClose = GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.SUBMIT_ON_CLOSE]).ToString().ToUpper().Trim() == GlobalHtmlParams.YES;
                LoadGegner();
                if (gegner != null)
                {
                    if (!IsPostBack)
                    {
                        LoadGegnerType();
                        SetValues();
                    }
                }
            }
        }

        private void LoadGegner()
        {
            gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerId = " + gegnerId, typeof(tblGegner));
        }
        private void LoadGegnerType()
        {
            ddlGegnerType.Items.Clear();
            ddlGegnerType.Items.Add(new ListItem("*** bitte auswählen ***", "-1"));
            ddlGegnerType.Items.Add(new ListItem("Firma", "0"));
            ddlGegnerType.Items.Add(new ListItem("Herr", "1"));
            ddlGegnerType.Items.Add(new ListItem("Frau", "2"));
        }

        private void SetValues()
        {
            ddlGegnerType.SelectedValue = gegner.GegnerType.ToString();
            lblAnrede.Text = GlobalUtilArea.GetHtmlSpaceIfEmptyOrNull(gegner.GegnerAnrede);
            lblName1.Text = GlobalUtilArea.GetHtmlSpaceIfEmptyOrNull(gegner.GegnerName1);
            lblName2.Text =GlobalUtilArea.GetHtmlSpaceIfEmptyOrNull( gegner.GegnerName2);
            lblName3.Text = GlobalUtilArea.GetHtmlSpaceIfEmptyOrNull(gegner.GegnerName3);
            lblStrasse.Text = GlobalUtilArea.GetHtmlSpaceIfEmptyOrNull(gegner.GegnerStrasse);
            lblZIPPrefix.Text = GlobalUtilArea.GetHtmlSpaceIfEmptyOrNull(gegner.GegnerZipPrefix);
            lblZIP.Text = GlobalUtilArea.GetHtmlSpaceIfEmptyOrNull(gegner.GegnerZip);
            lblOrt.Text = GlobalUtilArea.GetHtmlSpaceIfEmptyOrNull(gegner.GegnerOrt);
        }

        private bool SaveGegner()
        {
            gegner.GegnerType = Convert.ToInt16(ddlGegnerType.SelectedValue);
            /*
            gegner.GegnerAnrede = txtAnrede.Text;
            gegner.GegnerName1 = txtName1.Text;
            gegner.GegnerName2 = txtName2.Text;
            gegner.GegnerName3 = txtName3.Text;
            
            gegner.GegnerLastName1 = txtName1.Text;
            gegner.GegnerLastName2 = txtName2.Text;
            gegner.GegnerLastName3 = txtName3.Text;

            gegner.GegnerStrasse = txtStrasse.Text;
            gegner.GegnerZipPrefix = txtZIPPrefix.Text;
            gegner.GegnerZip = txtZIP.Text;
            gegner.GegnerOrt = txtOrt.Text;
             */ 
            try
            {
                return RecordSet.Update(gegner);
            }
            catch (Exception e)
            {
                ctlMessage.ShowException(e);
                return false;
            }
        }


        private bool IsEntryValid()
        {
            bool ok = true;
            StringBuilder sb = new StringBuilder();
            int gegnerType = -1;
            try {
                gegnerType = Convert.ToInt16(ddlGegnerType.SelectedValue);
                if (gegnerType < 0)
                {
                    sb.Append("Gegner Typ ist ungültig.<br>");
                    ok = false;
                }
            }
            catch {
                ok = false;
            }
            /*
            if (txtName1.Text.Trim() == "")
            {
                sb.Append("Nachname / Firma ist ungültig.<br>");
                ok = false;
            }
            if (txtName2.Text.Trim() == "" && gegnerType > 0)
            {
                sb.Append("Vorname ist ungültig.<br>");
                ok = false;
            }*/
            if (!ok)
            {
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsEntryValid() && SaveGegner())
            {
                if (submitFormOnClose)
                {
                    ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "window.opener.document.forms[0].submit();window.close();", true);
                }
                else if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]).Equals(string.Empty))
                {
                    Response.Redirect(
                        GetReturnToUrl(
                            GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]),
                            GlobalUtilArea.DecodeUrl(GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS]))
                        )
                    );
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "MM_refreshParentAndClose();", true);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]).Equals(string.Empty))
            {
                Response.Redirect(
                    GetReturnToUrl(
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_CANCEL]),
                        GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.EXTRA_PARAMS])
                    )
                );
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "window.close();", true);
            }
        }

        protected void txtZip_TextChanged(object sender, EventArgs e)
        {
            /*
            int zip = 9999999;
            try
            {
                zip = Convert.ToInt32(txtZIP.Text);
            }
            catch { zip = 9999999; }
            tblOrte ort = (tblOrte)HTBUtils.GetSqlSingleRecord("SELECT * FROM dbo.tblOrte WHERE " + HTBUtils.GetZipWhere(zip), typeof(tblOrte));
            txtOrt.Text = ort != null ? ort.Ort : "unbekannt";
            txtOrt.Focus();
             */
        }
        #endregion

        #region URL Creation
        private string GetReturnToUrl(string returnToUrlCode, string returnExtraParams)
        {
            StringBuilder sb = new StringBuilder();
            switch (returnToUrlCode)
            {
                case GlobalHtmlParams.URL_NEW_AKT:
                    sb.Append("../aktenink/NewAktInk.aspx?");
                    AppendURLParams(sb, returnExtraParams);
                    return sb.ToString();

                case GlobalHtmlParams.URL_BROWSER_GEGNER:
                    sb.Append("GegnerBrowser.aspx?");
                    AppendURLParams(sb, returnExtraParams, false);
                    return sb.ToString();
            }
            return "";
        }

        private void AppendURLParams(StringBuilder sb, string returnExtraParams)
        {
            AppendURLParams(sb, returnExtraParams, true);
        }
        private void AppendURLParams(StringBuilder sb, string returnExtraParams, bool decodeExtraParams)
        {
            if (gegner != null)
            {
                sb.Append(GlobalHtmlParams.GEGNER_ID);
                sb.Append("=");
                sb.Append(gegner.GegnerID.ToString());

            }
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT, Request.QueryString[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]);
            GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.SEARCH_FOR, Request.QueryString[GlobalHtmlParams.SEARCH_FOR]);
            if (returnExtraParams != "")
            {
                sb.Append("&");
                if (decodeExtraParams)
                    sb.Append(HttpUtility.UrlPathEncode(GlobalUtilArea.DecodeUrl(returnExtraParams)));
                else
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.EXTRA_PARAMS, returnExtraParams);
                
            }
        }
        #endregion

    }
}