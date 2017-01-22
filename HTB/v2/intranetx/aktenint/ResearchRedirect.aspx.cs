using System;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint
{
    public partial class ResearchRedirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktid = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.ID]);
            if (aktid > 0)
            {
                tblAktenInt akt = HTBUtils.GetInterventionAkt(aktid);
                if (akt != null && akt.AktIntResearchedSource.ToUpper().IndexOf(Request[GlobalHtmlParams.SOURCE].ToUpper()) < 0)
                {

                    var sql = "UPDATE tblAktenInt SET AktIntResearchedSource = AktIntResearchedSource + ' ' + '" +
                              Request[GlobalHtmlParams.SOURCE] + "' WHERE AktIntID = " + Request[GlobalHtmlParams.ID];

                    var set = new RecordSet();
                    set.ExecuteNonQuery(sql);
                }
            }
            if (Request[GlobalHtmlParams.SOURCE].ToUpper() == "DELTAVISTA")
            {
                string userId = Request[GlobalHtmlParams.USER_ID];
                if (string.IsNullOrEmpty(userId))
                    Response.Redirect("/v2/intranetx/dv/DVRedirect.aspx?URL=" + Request[GlobalHtmlParams.URL]);
                else
                    Response.Redirect("/v2/intranetx/dv/DVRedirect.aspx?" + GlobalHtmlParams.USER_ID + "=" + userId + "&URL=" + Request[GlobalHtmlParams.URL]);
            }
            else if (Request[GlobalHtmlParams.SOURCE].ToUpper() == "HEROLD")
                Response.Redirect(GlobalUtilArea.DecodeUrl(Request[GlobalHtmlParams.URL]));
        }
    }
}