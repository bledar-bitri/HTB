using System;
using System.Collections;
using System.Data;
using System.Reflection;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBExtras.XML;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadAktsWithOpenedTransfersTablet : System.Web.UI.Page
    {

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            int routeUser = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INKASSANT_ID]);
            try
            {

                var aktList = HTBUtils.GetStoredProcedureRecords("spGetAussendienstTransferListForTablet",
                    new ArrayList
                    {
                        new StoredProcedureParameter("userId", SqlDbType.Int, routeUser)
                    }, typeof (XmlRoadAktRecord));



                // Send Records
                foreach (XmlRoadAktRecord rec in aktList)
                {
                    Response.Write(rec.ToXmlString());
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
                Log.Error(ex);
            }
        }
    }
}