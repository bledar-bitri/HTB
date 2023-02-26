using System;
using System.Collections;
using HTBUtilities;
using HTB.Database.Views;
using HTBReports;
using System.IO;
using HTB.v2.intranetx.util;
using HTB.Database;

namespace HTB.v2.intranetx.akten
{
    public partial class UebergabeAkten : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT * FROM qryAkten WHERE (AKTID = " + Request["SQLSET"] + ") ";
                if (GlobalUtilArea.GetUserId(Session) == 99)
                    sql += "AND AktStatus in (4, 5) AND KlientNachricht = 3";
                else
                    sql += "AND AktStatus = 4 AND KlientNachricht = 3";

                ArrayList aktList = HTBUtils.GetSqlRecords(sql, typeof(qryAkten));
                var ms = new MemoryStream();

                var gen = new UebergabenAkten();
                gen.GenerateUebergabenAktenPDF(aktList, ms);
                
                var set = new RecordSet();
                set.StartTransaction();
                try
                {
                    foreach (qryAkten akt in aktList)
                    {
                        sql = "Update tblAkten Set AktStatus = 5, AKTVersandDatum = '" + DateTime.Now.ToShortDateString() + "' WHERE AKTId = " + akt.AktID;
                        set.ExecuteNonQueryInTransaction(sql);
                    }
                    set.CommitTransaction();
                }
                catch (Exception exx)
                {
                    set.RollbackTransaction();
                    ctlMessage.ShowException(exx);
                }
                Response.Clear();
                Response.ContentType = "Application/pdf";
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
    }
}