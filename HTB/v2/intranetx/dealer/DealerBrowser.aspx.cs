using System;
using System.Web.UI;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.dealer
{
    public partial class DealerBrowser : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlBrowser.SetReturnToURL(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]));
            ctlBrowser.SetReturnExtraParams(GlobalUtilArea.DecodeUrl(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.EXTRA_PARAMS])));
        }
    }
}