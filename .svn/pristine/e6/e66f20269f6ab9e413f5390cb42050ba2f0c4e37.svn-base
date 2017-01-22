using System;
using System.IO;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBReports;
using HTBUtilities;
using HTB.Database.Views;

namespace HTB.v2.intranetx.global_forms
{
    public partial class Forderungsaufstellung : System.Web.UI.Page
    {
        public string aktId;
        qryCustInkAkt akt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null && !Request.QueryString["ID"].ToString().Trim().Equals(""))
            {
                aktId = Request.QueryString["ID"];
                akt = (qryCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + aktId, typeof(qryCustInkAkt));
                if (akt != null)
                {
                    if (!IsPostBack)
                    {
                        SetValues();
                    }
                    else
                    {
                        btnSubmit.Visible = false;
                    }
                }
            }
        }

        private void SetValues()
        {
            lblAktNr.Text = akt.CustInkAktID.ToString();
            lblAZ.Text = akt.CustInkAktAZ.Trim() == "" ? "&nbsp;" : akt.CustInkAktAZ;
            lblAuftraggeber.Text = akt.CustInkAktAuftraggeber.ToString();
            lblAuftraggeberName.Text = akt.AuftraggeberName1 + akt.AuftraggeberName2;
            lblKlient.Text = akt.CustInkAktKlient.ToString();
            lblKlientName.Text = akt.KlientName1 + akt.KlientName2;
            lblGegner.Text = akt.CustInkAktGegner.ToString();
            lblGegnerName.Text = akt.GegnerName1 + akt.GegnerName2;
        }

        private void PrintForderungsaufstellung()
        {
            var mahMgr = new MahnungManager();
            EcpMahnung mah = mahMgr.GetMahnungForAkt(akt);
            var ms = new MemoryStream();
            var gen = new MahnungPdfReportGenerator();
            
            gen.GenerateForderungsaufstellung(mah, ms, txtMemo.Text, chkRatenansuchen.Checked, chkRatenplan.Checked, chkForderungsaufstellung.Checked);

            Response.Clear();
            Response.ContentType = "Application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.End();
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Create action
            var action = new tblCustInkAktAktion
                             {
                                 CustInkAktAktionAktID = akt.CustInkAktID, 
                                 CustInkAktAktionDate = DateTime.Now,
                                 CustInkAktAktionMemo = txtMemo.Text.Replace("'", ""),
                                 CustInkAktAktionCaption = "Forderungsschreiben für Schuldner",
                                 CustInkAktAktionUserId = GlobalUtilArea.GetUserId(Session)
                             };
            action.CustInkAktAktionEditDate = action.CustInkAktAktionDate;
            RecordSet.Insert(action);
            PrintForderungsaufstellung();
        }
    }
}