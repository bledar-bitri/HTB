using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.gegner.tablet
{
    public partial class EditGegnerTablet : System.Web.UI.Page
    {
        private string _gegnerId = "";
        private tblGegner _gegner;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!GlobalUtilArea.GetEmptyIfNull(Request.QueryString[GlobalHtmlParams.ID]).ToString().Trim().Equals(string.Empty))
            {
                _gegnerId = Request.QueryString[GlobalHtmlParams.ID];
                LoadGegner();
                if (_gegner != null)
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
            _gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerId = " + _gegnerId, typeof(tblGegner));
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
            ddlGegnerType.SelectedValue = _gegner.GegnerType.ToString();
            txtAnrede.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerAnrede);
            txtLastName1.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerLastName1);
            txtLastName2.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerLastName2);
            txtLastName3.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerLastName3);
            txtLastStrasse.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerLastStrasse);
            txtLastZIPPrefix.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerLastZipPrefix);
            txtLastZIP.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerLastZip);
            txtLastOrt.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerLastOrt);

            txtPhoneCountry.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerPhoneCountry);
            txtPhoneCity.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerPhoneCity);
            txtPhone.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerPhone);

            txtFax.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerFax);
            txtFaxCity.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerFaxCity);
            txtFaxCountry.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerFaxCountry);

            txtEmail.Text = GlobalUtilArea.GetEmptyIfNull(_gegner.GegnerEmail);
        }

        private bool SaveGegner()
        {
            _gegner.GegnerType = Convert.ToInt16(ddlGegnerType.SelectedValue);
            _gegner.GegnerAnrede = txtAnrede.Text;
            _gegner.GegnerName1 = txtLastName1.Text;
            _gegner.GegnerName2 = txtLastName2.Text;
            _gegner.GegnerName3 = txtLastName3.Text;

            _gegner.GegnerLastName1 = txtLastName1.Text;
            _gegner.GegnerLastName2 = txtLastName2.Text;
            _gegner.GegnerLastName3 = txtLastName3.Text;

            _gegner.GegnerStrasse = txtLastStrasse.Text;
            _gegner.GegnerZipPrefix = txtLastZIPPrefix.Text;
            _gegner.GegnerZip = txtLastZIP.Text;
            _gegner.GegnerOrt = txtLastOrt.Text;

            _gegner.GegnerPhone = txtPhone.Text;
            _gegner.GegnerPhoneCity = txtPhoneCity.Text;
            _gegner.GegnerPhoneCountry = txtPhoneCountry.Text;

            _gegner.GegnerFax = txtFax.Text;
            _gegner.GegnerFaxCity = txtFaxCity.Text;
            _gegner.GegnerFaxCountry = txtFaxCountry.Text;

            _gegner.GegnerEmail = txtEmail.Text;
            try
            {
                return RecordSet.Update(_gegner);
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
            var sb = new StringBuilder();
            int gegnerType = -1;
            try
            {
                gegnerType = Convert.ToInt16(ddlGegnerType.SelectedValue);
                if (gegnerType < 0)
                {
                    sb.Append("Gegner Typ ist ungültig.<br>");
                    ok = false;
                }
            }
            catch
            {
                ok = false;
            }
            if (txtLastName1.Text.Trim() == "")
            {
                sb.Append("Nachname / Firma ist ungültig.<br>");
                ok = false;
            }
            if (txtLastName2.Text.Trim() == "" && gegnerType > 0)
            {
                sb.Append("Vorname ist ungültig.<br>");
                ok = false;
            }
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
                ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "MM_refreshParentAndClose();", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updPanel1, typeof(string), "closeScript", "window.close();", true);
        }
        #endregion
    }
}