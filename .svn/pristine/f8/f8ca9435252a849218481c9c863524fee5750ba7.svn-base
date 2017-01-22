using System;
using System.Collections;
using System.Data;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBExtras;
using HTBExtras.XML;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadJournalTablet : TabletPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INTERVENTION_AKT]);
            if (aktId <= 0)
            {
                WriteError("Kein Aktenzahl!");
                return;
            }
            var akt = HTBUtils.GetInterventionAkt(aktId);
            if (akt == null)
            {
                WriteError("Akt [" + aktId + "] Nicht Gefunden");
                return;
            }
            try
            {
                ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetAktJournal",
                                                   new ArrayList
                                                       {
                                                           new StoredProcedureParameter("intAktId", SqlDbType.Int, aktId)
                                                       },
                                                   typeof (XmlJournalRecord)
                    );
                foreach (XmlJournalRecord rec in list)
                {
                    Response.Write(rec.ToXmlString());
                }
                //Response.Write("OKK");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }

    }
}