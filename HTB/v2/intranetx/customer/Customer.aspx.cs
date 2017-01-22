using System;
using HTB.Database;
using HTBUtilities;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.customer
{
    public partial class Customer : System.Web.UI.Page
    {
        tblKlient _klient;
        tblAuftraggeber _ag;
        private bool _isKlient;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["MM_AG"] = "6";
            GlobalUtilArea.GetUserId(Session); // force session validation
            if(Session["MM_AG"] == null && Session["MM_Klient"] == null)
                Response.Redirect(GlobalUtilArea.LoginExpiredLink);
                
            if (!IsPostBack)
            {
                if(Session["MM_AG"] != null) 
                    _ag = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = "+(string)Session["MM_AG"], typeof(tblAuftraggeber));
                if (Session["MM_Klient"] != null)
                {
                    _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientID = " + (string)Session["MM_Klient"], typeof(tblKlient));
                    if (_klient != null)
                    {
                        _isKlient = true;
                    }
                }
                SetValues();
            }
            pnlInkasso.Visible = false;
            pnlIntervention.Visible = false;
            if (_isKlient)
                pnlInkasso.Visible = true;
            else
                pnlIntervention.Visible = true;
        }

        private void SetValues()
        {
            if (_ag != null)
                lblHeader.Text = "Kundenportal " + _ag.AuftraggeberName1 + "&nbsp;" + _ag.AuftraggeberName2;
            if (_klient != null)
                lblHeader.Text += "<em>- Klient:&nbsp;" + _klient.KlientName1 + "&nbsp;" + _klient.KlientName2 + "</em>";
            
            lblHeaderIntervention.Text = lblHeader.Text;
        }
    }
}