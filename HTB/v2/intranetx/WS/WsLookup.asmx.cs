using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections;
using System.Web.Script.Serialization;
using HTB.Database;
using AjaxControlToolkit;
using HTB.Database.HTB.Views;
using HTBUtilities;
using HTB.Database.Views;
using System.Data;
using HTB.Database.LookupRecords;

namespace HTB.v2.intranetx.WS
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class WsLookup : WebService
    {

        [WebMethod]
        [ScriptMethod]
        public string[] GetGegnerInfo(string prefixText, int count, string contextKey)
        {
            DateTime now = DateTime.Now;
            var items = new List<string>();
            var serializer = new JavaScriptSerializer();
            
            ArrayList gegners = HTBUtils.GetSqlRecords("SELECT TOP 20 * FROM qryLookupGegner WHERE  " + GetGegnerWhereString(prefixText), typeof(qryLookupGegner));
            //items.Add(AutoCompleteExtender.CreateAutoCompleteItem(
            //        "QRY EXCEC MILI: " + DateTime.Now.Subtract(now).TotalMilliseconds,
            //        serializer.Serialize(new GegnerLookup())));
            foreach (qryLookupGegner gegner in gegners)
            {
                items.Add(AutoCompleteExtender.CreateAutoCompleteItem(
                    gegner.GegnerLastName1 + " " + gegner.GegnerLastName2 + " | " + gegner.GegnerLastStrasse + " " + gegner.GegnerLastZip + " " + gegner.GegnerLastOrt,
                    serializer.Serialize(gegner)));
            }
            //items.Add(AutoCompleteExtender.CreateAutoCompleteItem(
            //        "TOTAL MILI: " + DateTime.Now.Subtract(now).TotalMilliseconds,
            //        serializer.Serialize(new GegnerLookup())));
            return items.ToArray();
        }

        [WebMethod]
        [ScriptMethod]
        public string[] GetGegnerInfoNew(string prefixText, int count, string contextKey)
        {
            var now = DateTime.Now;
            var items = new List<string>();
            var serializer = new JavaScriptSerializer();
            var parameters = new ArrayList();
            int totalGegner = 0;
            
            parameters.Add(new StoredProcedureParameter("@startIndex", SqlDbType.Int, 0));
            parameters.Add(new StoredProcedureParameter("@pageSize", SqlDbType.Int, 20));
            parameters.Add(new StoredProcedureParameter("@sortBy", SqlDbType.Int, "GegnerName1"));
            parameters.Add(new StoredProcedureParameter("@totalGegner", SqlDbType.Int, totalGegner, ParameterDirection.Output));
            parameters.Add(new StoredProcedureParameter("@countGegner", SqlDbType.Bit, false));
            parameters.Add(new StoredProcedureParameter("@name", SqlDbType.NVarChar, prefixText));
            
            ArrayList gegners = HTBUtils.GetStoredProcedureRecords("spSearchGegner", parameters, typeof(GegnerLookup));
            
            //items.Add(AutoCompleteExtender.CreateAutoCompleteItem(
            //        "SP EXCEC MILI: " + DateTime.Now.Subtract(now).TotalMilliseconds,
            //        serializer.Serialize(new GegnerLookup())));

            foreach (GegnerLookup gegner in gegners)
            {
                items.Add(AutoCompleteExtender.CreateAutoCompleteItem(
                    gegner.GegnerName + " | " + gegner.GegnerLastStrasse + " " + gegner.GegnerLastOrt,
                    serializer.Serialize(gegner)));
            }
            //items.Add(AutoCompleteExtender.CreateAutoCompleteItem(
            //        "TOTAL MILI: "+DateTime.Now.Subtract(now).TotalMilliseconds,
            //        serializer.Serialize(new GegnerLookup())));
            return items.ToArray();
        }
        
        private string GetGegnerWhereString(string prefix)
        {

            return " (CASE WHEN GegnerName1 IS NULL  THEN '' ELSE  GegnerName1 END +' ' + CASE WHEN GegnerName2 IS NULL  THEN '' ELSE  GegnerName2 END+' '+CASE WHEN GegnerName3 IS NULL  THEN '' ELSE  GegnerName3 END) like '%" + prefix + "%'";
        }
        
        [WebMethod]
        [ScriptMethod]
        public string[] GetKlientInfo(string prefixText, int count, string contextKey)
        {
            ArrayList clients = HTBUtils.GetSqlRecords("SELECT top 20 * FROM qryLookupKlient WHERE  " + GetKlientWhereString(prefixText), typeof(qryLookupKlient));
            var serializer = new JavaScriptSerializer();
            return (from qryLookupKlient klient in clients select AutoCompleteExtender.CreateAutoCompleteItem(klient.KlientName1 + " " + klient.KlientName2 + " | " + klient.KlientStrasse + " " + klient.KlientPLZ + " " + klient.KlientOrt, serializer.Serialize(klient))).ToArray();
        }

        private string GetKlientWhereString(string prefix)
        {
            return "KlientType = 15 AND (CASE WHEN KlientName1 IS NULL  THEN '' ELSE  KlientName1 END +' ' + CASE WHEN KlientName2 IS NULL  THEN '' ELSE  KlientName2 END+' '+CASE WHEN KlientName3 IS NULL  THEN '' ELSE  KlientName3 END) like '%" + prefix + "%'";
        }

        [WebMethod]
        [ScriptMethod]
        public string[] GetActiveUsersInfo(string prefixText, int count, string contextKey)
        {
            ArrayList users = HTBUtils.GetSqlRecords("SELECT top 20 * FROM qryLookupUser WHERE  " + GetUserWhereString(prefixText), typeof(qryLookupUser));
            var serializer = new JavaScriptSerializer();
            return (from qryLookupUser user in users select AutoCompleteExtender.CreateAutoCompleteItem(user.UserID + " | " + user.UserVorname + " " + user.UserNachname + " | " + user.AbteilungCaption, serializer.Serialize(user))).ToArray();
        }

        private string GetUserWhereString(string prefix)
        {
            return "(CASE WHEN UserVorname IS NULL  THEN '' ELSE  UserVorname END + ' ' + CASE WHEN UserNachname IS NULL  THEN '' ELSE  UserNachname END)  like '%" + prefix + "%'";
        }

        [WebMethod]
        [ScriptMethod]
        public string[] GetAuftraggeberInfo(string prefixText, int count, string contextKey)
        {
            ArrayList auftraggeberList = HTBUtils.GetSqlRecords("SELECT top 20 * FROM qryLookupAuftraggeber WHERE  " + GetAuftraggeberWhereString(prefixText), typeof(qryLookupAuftraggeber));
            var serializer = new JavaScriptSerializer();
            return (from qryLookupAuftraggeber ag in auftraggeberList select AutoCompleteExtender.CreateAutoCompleteItem(ag.AuftraggeberName1 + " " + ag.AuftraggeberName2 + " | " + ag.AuftraggeberStrasse + " " + ag.AuftraggeberPLZ + " " + ag.AuftraggeberOrt, serializer.Serialize(ag))).ToArray();
        }

        private string GetAuftraggeberWhereString(string prefix)
        {
            return "(CASE WHEN AuftraggeberName1 IS NULL  THEN '' ELSE  AuftraggeberName1 END +' ' + CASE WHEN AuftraggeberName2 IS NULL  THEN '' ELSE  AuftraggeberName2 END+' '+CASE WHEN AuftraggeberName3 IS NULL  THEN '' ELSE  AuftraggeberName3 END) like '%" + prefix + "%'";
        }

        [WebMethod]
        [ScriptMethod]
        public string[] GetDealerInfo(string prefixText, int count, string contextKey)
        {
            ArrayList dealers = HTBUtils.GetSqlRecords("SELECT top 20 * FROM tblAutoDealer WHERE  " + GetDealerWhereString(prefixText), typeof(tblAutoDealer));
            var serializer = new JavaScriptSerializer();
            return (from tblAutoDealer dealer in dealers select AutoCompleteExtender.CreateAutoCompleteItem(dealer.AutoDealerName + " | " + dealer.AutoDealerStrasse + " " + dealer.AutoDealerPLZ + " " + dealer.AutoDealerOrt, serializer.Serialize(dealer))).ToArray();
        }

        private string GetDealerWhereString(string prefix)
        {
            return "(CASE WHEN AutoDealerName IS NULL THEN '' ELSE AutoDealerName END) like '%" + prefix + "%'";
        }
    }
}
