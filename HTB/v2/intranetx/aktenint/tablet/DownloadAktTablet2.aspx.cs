using System;
using System.Collections;
using System.Data;
using System.Reflection;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBExtras.XML;
using HTBUtilities;
using System.Security;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadAktTablet2 : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INTERVENTION_AKT]);
            
            if (aktId <= 0)
            {
                Response.Write("Error: Kein Akttyp ID!");
                return;
            }
            qryAktenInt qryAkt = HTBUtils.GetInterventionAktQry(aktId);
            if (qryAkt == null)
            {
                Response.Write("Error: Akt [" + aktId + "] Nicht Gefunden");
                return;
            }
            try
            {
                var rec = new XmlInterventionAkt2();
                rec.Assign(qryAkt);
                rec.aktIntAmounts = AktInterventionUtils.GetAktAmounts(qryAkt, false);
                rec.aktIntAmounts.GetTotal();

                var sv = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT COUNT(*) IntValue FROM tblAktenIntAction WHERE AktIntActionAkt = " + aktId, typeof(SingleValue));
                try
                {
                    rec.AktHasActions = sv.IntValue > 0;
                }
                catch
                {
                }

                AddAktDocuments(rec);
                AddAktAddressesAndPhones(rec);
                AddAktJournal(rec);

                var protocol = (tblProtokol)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblProtokol WHERE AktIntID = " + aktId + " ORDER BY ProtokolID DESC", typeof(tblProtokol));
                if (protocol != null)
                {
                    rec.protocol = protocol;
                }

                var protocolUbername = (tblProtokolUbername)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblProtokolUbername WHERE UbernameAktIntID = " + aktId, typeof(tblProtokolUbername));
                if (protocolUbername != null)
                {
                    rec.protocolUbername = protocolUbername;
                }

                Response.Write(rec.ToXmlString().Replace("XmlInterventionAkt2", "XmlInterventionAkt"));
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
                Log.Error("AktId: " + aktId + " "+ex.Message);
                Log.Error(ex);
            }
        }

        private void AddAktJournal(XmlInterventionAkt2 akt)
        {
            ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetAktJournal",
                                                   new ArrayList
                                                       {
                                                           new StoredProcedureParameter("intAktId", SqlDbType.Int, akt.AktIntID)
                                                       },
                                                   typeof(XmlJournalRecord)
                    );
            foreach (XmlJournalRecord rec in list)
            {
                rec.ActionMemo = SecurityElement.Escape(rec.ActionMemo);
                akt.AddJournal(rec);
            }
        }
        private void AddAktDocuments(XmlInterventionAkt2 akt)
        {
            ArrayList docsList = HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + akt.AktIntID, typeof(qryDoksIntAkten));
            if (akt.IsInkasso())
                HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksInkAkten WHERE CustInkAktID = " + akt.AktIntCustInkAktID, typeof(qryDoksInkAkten)), docsList);
            
            foreach (Record doc in docsList)
            {
                akt.AddDocument(doc, Request.Url.Scheme, Request.Url.Host);
            }
        }

        private void AddAktAddressesAndPhones(XmlInterventionAkt2 akt)
        {
            foreach (Record rec in HTBUtils.GetSqlRecords("SELECT * FROM tblGegnerAdressen WHERE GAGegner = " + akt.GegnerID, typeof(XmlGegnerAddress)))
            {
                akt.addAddress(rec);
            }
            foreach (Record rec in HTBUtils.GetSqlRecords("SELECT * FROM tblGegnerAdressen WHERE GAGegner = " + akt.Gegner2ID, typeof(XmlGegner2Address)))
            {
                akt.addAddress2(rec);
            }

            foreach (Record rec in HTBUtils.GetSqlRecords("SELECT * FROM qryGegnerPhone WHERE GPhoneGegnerID = " + akt.GegnerID, typeof(XmlGegnerPhone)))
            {
                akt.addPhone(rec);
            }
            foreach (Record rec in HTBUtils.GetSqlRecords("SELECT * FROM qryGegnerPhone WHERE GPhoneGegnerID = " + akt.Gegner2ID, typeof(XmlGegner2Phone)))
            {
                akt.addPhone2(rec);
            }
        }
    }
}