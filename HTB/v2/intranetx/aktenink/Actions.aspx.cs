using System;
using System.Web.UI;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;
using HTBAktLayer;
using HTB.Database.Views;
using HTBServices;
using HTBServices.Mail;

namespace HTB.v2.intranetx.aktenink
{
    public partial class Actions : Page
    {
        private string _kzId = "";
        private int _aktId;
        private string _actionName = "";

        private qryCustInkAkt _custInkAkt;
        private tblKZ _kz;
        private AktUtils _aktUtils;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlMessage.Clear();
            if (Request["KZID"] != null && !Request["KZID"].Equals(""))
            {
                _kzId = Request["KZID"];
                _aktId = Convert.ToInt32(Request["ID"]);
                _actionName = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ACTION_NAME]);
                
                LoadRecords();
                SetValues();
            }
        }

        private void SetValues()
        {
            _aktUtils = new AktUtils(_custInkAkt.CustInkAktID);
            lblAktId.Text = _aktId.ToString();
            lblKZCaption.Text = _kz.KZCaption;
            lblDescription.Text = _aktId.ToString() + " " + _kzId + " " + DateTime.Now.ToString();
            double balance = _aktUtils.GetAktBalance();
            if (balance <= 0)
            {
                btnSubmit.Enabled = false;
            }
        }

        private void LoadRecords()
        {
            _custInkAkt = HTBUtils.GetInkassoAktQry(_aktId);
            _kz = (tblKZ)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKZ WHERE KZID = " + _kzId.Replace("'", "''"), typeof(tblKZ));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool ok = true;
                if (_actionName == GlobalHtmlParams.ACTION_INTERVENTION)
                    CreateInterventionAkt();
                else if (_actionName == GlobalHtmlParams.ACTION_MELDE)
                    CreateMeldeAkt();
                else
                {
                    ok = false;
                }
                if (ok)
                    ScriptManager.RegisterStartupScript(updPanel1, typeof (string), "closeScript", "MM_refreshParentAndClose();", true);
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }

        private void CreateInterventionAkt()
        {
            var interventionAkt = HTBUtils.GetInterventionAkt(_custInkAkt.CustInkAktID);
            ctlMessage.ShowInfo(interventionAkt != null ? "Interventionsakt exists" : "Interventionsakt DOES NOT EXIST<br/>");
            _aktUtils.CreateInterventionCosts(_custInkAkt.CustInkAktID, _custInkAkt.CustInkAktInvoiceDate);
            tblAktenInt aktInt =  _aktUtils.CreateNewInterventionAkt(_custInkAkt, txtMemo.Text, 21);
            _aktUtils.SetInkassoStatusBasedOnWflAction(_aktUtils.control.InterventionKostenArtId, GlobalUtilArea.GetUserId(Session), aktInt);
            ctlMessage.AppendInfo("NEU CREATED !!!<br/>");
        }

        private void CreateMeldeAkt()
        {
            int meldeAktId = _aktUtils.CreateMeldeAkt(_custInkAkt);
            _aktUtils.SetInkassoStatusBasedOnWflAction(_aktUtils.control.MeldeKostenArtId, GlobalUtilArea.GetUserId(Session), null, txtMemo.Text);
            ServiceFactory.Instance.GetService<IHTBEmail>().SendGenericEmail(new string[]{HTBUtils.GetConfigValue("Melde_Email"), HTBUtils.GetConfigValue("Default_EMail_Addr")}, "Neu Meldeakt: " + meldeAktId + " Akt: " + _custInkAkt.CustInkAktID, "Melde: " + meldeAktId + " InkassoAkt: " + _custInkAkt.CustInkAktID);
        }
    }
}