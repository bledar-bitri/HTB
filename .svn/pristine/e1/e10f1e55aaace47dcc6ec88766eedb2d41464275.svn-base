using System;
using System.Web.UI;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.gegner
{
    public partial class GegnerBrowser : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlBrowser.SetReturnToURL(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]));
            ctlBrowser.SetReturnGegner2(HTBUtils.GetBoolValue(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_GEGNER2])));
            ctlBrowser.SetReturnExtraParams(GlobalUtilArea.DecodeUrl(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.EXTRA_PARAMS])));
        }
    }
}