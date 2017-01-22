using System;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class SaveAktTablet : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INTERVENTION_AKT]);
            var akt = HTBUtils.GetInterventionAkt(aktId);
            if(akt == null)
            {
                Response.Write("Error: Akt ["+aktId+"] nicht gefunden!");
                return;
            }
            try {
                akt.AKTIntMemo = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.MEMO]);
                if(RecordSet.Update(akt))
                    Response.Write("OK: Akt gespeichert!");
            }
            catch (Exception ex)
            {
                Response.Write("Error: ");
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }
    }
}
