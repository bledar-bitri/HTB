using System;
using System.Collections;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBExtras;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadActionsTablet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INTERVENTION_AKT]);
            if (aktId <= 0)
            {
                Response.Write("Kein Aktenzahl!");
                return;
            }
            var akt = HTBUtils.GetInterventionAkt(aktId);
            if (akt == null)
            {
                Response.Write("Akt [" + aktId + "] Nicht Gefunden");
                return;
            }
            try
            {
                ArrayList actions = GlobalUtilArea.GetInterventionActions(akt.AktIntAuftraggeber, akt.AktIntAktType, Session);

//                HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT AktIntActionTypeID AS ActionID, AktIntActionTypeCaption AS ActionCaption, AktIntActionMemo AS ActionMemo, AktIntActionTime AS ActionDate, 11 AS RecordStatus FROM tblAktenIntAction A INNER JOIN tblAktenIntActionType T on A.AktIntActionType = T.AktIntActionTypeID WHERE AktIntActionAkt = " + aktId, typeof(ActionRecord)), actions);
                
                foreach (ActionRecord action in actions)
                {
                    if (action.RecordStatus == 0) 
                        action.RecordStatus = 10;
                    Response.Write(action.ToXmlString());
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