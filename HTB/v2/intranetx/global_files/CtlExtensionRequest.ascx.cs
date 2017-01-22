using System;
using System.Web.UI;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlExtensionRequest : UserControl
    {
        public void PopulateFields(int aktId)
        {
            tblAktenIntExtension ext = GetExtensionForAkt(aktId);
            if (ext == null)
            {
                SetAllVisible(false);
                ddlExtensionDays.Visible = true;
                txtExtensionRequestReason.Visible = true;
            }
            else
            {
                
                int extWeeks = ext.AktIntExtRequestDays / 7;

                lblExtensionDays.Text = extWeeks + " Woche" + (extWeeks == 1 ? "" : "n");
                lblExtensionRequestReason.Text = ext.AktIntExtRequestReason;
                lblExtensionRequestDate.Text = ext.AktIntExtRequestDate.ToShortDateString();
                if (HTBUtils.IsDateValid(ext.AktIntExtApprovedDate))
                    lblExtensionRequestStatus.Text = "Genehmigt am "+ext.AktIntExtApprovedDate.ToShortDateString()+"<BR/>"+ext.AktIntExtMemo;
                else if (HTBUtils.IsDateValid(ext.AktIntExtDeniedDate))
                    lblExtensionRequestStatus.Text = "Verweigert am " + ext.AktIntExtDeniedDate.ToShortDateString() + "<BR/>" + ext.AktIntExtMemo;
                else
                    lblExtensionRequestStatus.Text = "Anh&auml;ngig";

                SetAllVisible(true);
                ddlExtensionDays.Visible = false;
                txtExtensionRequestReason.Visible = false;
            }
        }

        public int SaveExtensionRequest(tblAktenIntAction action)
        {
            tblAktenIntExtension ext = GetExtensionForAkt(action.AktIntActionAkt);
            if (ext != null)
            {
                if (!IsActionTakenByAG(ext))
                {
                    LoadExtensionRecordFromScreen(ext);

                }
                return action.AktIntActionAktIntExtID;
            }
            ext = new tblAktenIntExtension();
                
            LoadExtensionRecordFromScreen(ext);
                
            ext.AktIntExtAkt = action.AktIntActionAkt;
                
            RecordSet.Insert(ext);
            SingleValue sv = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT MAX(AktIntExtID) AS [IntValue] FROM tblAktenIntExtension ", typeof(SingleValue));
            return sv.IntValue;
        }

        private void SetAllVisible(bool visible)
        {
            ddlExtensionDays.Visible = visible;
            txtExtensionRequestReason.Visible = visible;
            lblExtensionDays.Visible = visible;
            lblExtensionRequestReason.Visible = visible;
            trExtensionRequestDate.Visible = visible;
            trStatus.Visible = visible;
        }

        private void LoadExtensionRecordFromScreen(tblAktenIntExtension ext)
        {
            ext.AktIntExtRequestDays = Convert.ToInt16(ddlExtensionDays.SelectedValue) * 7;
            ext.AktIntExtRequestReason = txtExtensionRequestReason.Text;
            ext.AktIntExtRequestDate = DateTime.Now;
            ext.AktIntExtRequestUser = GlobalUtilArea.GetUserId(Session);
            ext.AktIntExtApprovedDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError("");
            ext.AktIntExtDeniedDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError("");
        }

        private tblAktenIntExtension GetExtensionForAkt(int aktId)
        {
            return (tblAktenIntExtension)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntExtension WHERE AktIntExtAkt = " + aktId, typeof(tblAktenIntExtension));
        }

        private bool IsActionTakenByAG(tblAktenIntExtension ext)
        {
            if (HTBUtils.IsDateValid(ext.AktIntExtApprovedDate))
                return true;
            else if (HTBUtils.IsDateValid(ext.AktIntExtDeniedDate))
                return true;
            else if (ext.AktIntExtMemo.Trim() != string.Empty)
                return true;

            return false;  
        }
    }
}