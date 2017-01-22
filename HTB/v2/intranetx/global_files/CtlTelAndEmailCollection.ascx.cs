using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTBUtilities;
using HTB.Database;
using System.Text;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlTelAndEmailCollection : System.Web.UI.UserControl
    {
        private static string tempPhoneCity = "";
        private static string tempPhone = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        internal bool Save(string gegnerOldID)
        {
            if (IsScreenValid())
            {
                tblGegner gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerOldId = '" + gegnerOldID + "'", typeof(tblGegner));
                gegner.GegnerPhoneCountry = "43";
                gegner.GegnerPhoneCity = txtPhoneCity.Text;
                gegner.GegnerPhone = txtPhone.Text;
                gegner.GegnerEmail = txtEmail.Text;
                if (IsEntryValid(gegner))
                {
                    if (RecordSet.Update(gegner))
                    {
                        ctlMessage.ShowSuccess("Telefon und Email gespeichert!");
                        return true;
                    }
                    else
                    {
                        ctlMessage.ShowError("Error Saving Gegner!");
                    }
                }
            }
            return false;
        }

        private bool IsEntryValid(tblGegner gegner)
        {
            ctlMessage.Clear();
            StringBuilder sb = new StringBuilder();
            bool ok = true;
            
            if (!chkNoPhone.Checked && gegner.GegnerPhoneCity.Trim().Equals(string.Empty))
            {
                sb.Append("<i><strong>Telefonnummer (vorwahl)</strong></i> eingeben!<br>");
                ok = false;
            }
            if (!chkNoPhone.Checked && gegner.GegnerPhone.Trim().Equals(string.Empty))
            {
                sb.Append("<i><strong>Telefonnummer</strong></i> eingeben!<br>");
                ok = false;
            }
            if (!chkNoEmail.Checked && gegner.GegnerEmail.Trim().Equals(string.Empty))
            {
                sb.Append("<i><strong>Emailadresse</strong></i> eingeben!<br>");
                ok = false;
            }
            if (ok)
            {
                if (!gegner.GegnerPhoneCity.Trim().Equals(string.Empty))
                {
                    if (!HTBUtils.IsNumber(gegner.GegnerPhoneCity))
                    {
                        sb.Append("<i><strong>Telefonnummer (vorwahl)</strong></i> muss ein Zahl sein!<br>");
                        ok = false;
                    }
                }
                if (!gegner.GegnerEmail.Trim().Equals(string.Empty))
                {
                    if (!HTBUtils.IsValidEmail(gegner.GegnerEmail))
                    {
                        sb.Append("<i><strong>Emailadresse </strong></i> ist falsch!<br>");
                        ok = false;
                    }
                }
            }
            if (!ok)
            {
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }

        private bool IsScreenValid()
        {
            ctlMessage.Clear();
            StringBuilder sb = new StringBuilder();
            bool ok = true;
            if (chkNoPhone.Checked && chkNoEmail.Checked)
            {
                sb.Append("<i><strong>Die Felder 'keine Telefonnummer' und 'keine Emailadresse' d&uuml;rfen nicht gleichzeitig gechecked werden!</strong></i><br/>");
                ok = false;
            }
            if (chkNoPhone.Checked && (!txtPhoneCity.Text.Trim().Equals(string.Empty) || !txtPhone.Text.Trim().Equals(string.Empty)))
            {
                sb.Append("<i><strong>Wenn das Feld 'keine Telefonnummer' gechecked ist, dann darf keine Telefonnummer eingegeben werden!</strong></i><br>");
                ok = false;
            }
            if (chkNoEmail.Checked && !txtEmail.Text.Trim().Equals(string.Empty))
            {
                sb.Append("<i><strong>Wenn das Feld 'keine Emailadresse' gechecked ist, dann darf keine Emailadresse eingegeben werden!</strong></i><br>");
                ok = false;
            }
            if (!ok)
            {
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }

        internal void PopulateFields(string gegnerOldID)
        {
            tblGegner gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegner WHERE GegnerOldId = '" + gegnerOldID + "'", typeof(tblGegner));
            if (gegner != null)
            {
                txtPhoneCity.Text = gegner.GegnerPhoneCity;
                txtPhone.Text = gegner.GegnerPhone;
                txtEmail.Text = gegner.GegnerEmail;
            }
            else
            {
                ctlMessage.ShowError("Could not find gegner with old ID: " + gegnerOldID);
            }
        }

        private void SetCheckboxesStatesBasedOnDataEntry()
        {
            if (!txtPhoneCity.Text.Trim().Equals(string.Empty) || !txtPhone.Text.Trim().Equals(string.Empty))
                chkNoPhone.Checked = false;
            if(txtEmail.Text.Trim().Equals(string.Empty))
                chkNoEmail.Checked = false;
        }

        protected void chkNoPhone_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoPhone.Checked)
            {
                tempPhoneCity = txtPhoneCity.Text;
                tempPhone = txtPhone.Text;
                txtPhoneCity.Text = "";
                txtPhone.Text = "";
            }
            else
            {
                txtPhoneCity.Text = tempPhoneCity.Trim();
                txtPhone.Text = tempPhone.Trim();
            }
        }
    }
}