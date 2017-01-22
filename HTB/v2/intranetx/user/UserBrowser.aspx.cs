using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.util;

namespace HTB.v2.intranetx.klienten
{
    public partial class UserBrowser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlBrowser.SetReturnToURL(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RETURN_TO_URL_ON_SUBMIT]));
            ctlBrowser.SetReturnExtraParams(GlobalUtilArea.DecodeUrl(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.EXTRA_PARAMS])));
        }
    }
}