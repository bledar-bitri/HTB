using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBReports;
using HTBUtilities;

namespace HTB.v2.intranetx.test
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (HTBUtils.IsTestEnvironment) return;

            ctlMessage.ShowError("This environment is not being tested currently");
            btnTestLaywerPackage.Enabled = false;
             */
        }

        protected void btnTestLaywerPackage_Clicked(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(txtAktID.Text);
            if (aktId > 0)
            {
                try
                {
                    var attachment = new HTBEmailAttachment(ReportFactory.GetZwischenbericht(aktId), "Zwischenbericht.pdf", "Application/pdf");
                    new AktUtils(aktId).SendInkassoPackageToLawyer(attachment, true, "c:/temp/lawyerTest/");
                }
                catch (Exception ex)
                {
                    ctlMessage.ShowException(ex);
                }
            }
            else
            {
                ctlMessage.ShowError("Invalid Akt ID");
            }
        }
    }
}