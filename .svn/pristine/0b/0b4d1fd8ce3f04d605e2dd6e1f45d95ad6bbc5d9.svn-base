using System;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadActionTypeTablet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktionTypeId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.ACTION_TYPE_ID]);
            if (aktionTypeId <= 0)
            {
                Response.Write("Error: Kein Akttyp ID!");
                return;
            }
            var aktionType = (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + aktionTypeId, typeof(tblAktenIntActionType));
            if (aktionType == null)
            {
                Response.Write("Error: Akttyp [" + aktionTypeId + "] Nicht Gefunden");
                return;
            }
            try
            {
                Response.Write(aktionType.ToXmlString());
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }
    }
}