using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Services;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBExtras.XML;
using HTBUtilities;

namespace HTB.v2.intranetx.WS
{
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WsTabletSync : WebService
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [WebMethod()]
        public bool IsAktChanged(int aktId, double balanceAmount, string timeStamp, string phoneTimeStamps, string addressTimeStamps, string[] docTimeStamps)
        {
            var sp = new Stopwatch();
            sp.Start();

            //            Log.Info("IsAktChanged START: [aktId: " + aktId + "]  [balanceAmount: " + balanceAmount + "]  [timeStamp:" + timeStamp + "]  [phoneTimeStamps: " + phoneTimeStamps + "] [addressTimeStamps: " + addressTimeStamps + "]  [docTimeStamps: " + docTimeStamps + "]");
            var sv = (SingleValue) HTBUtils.GetSqlSingleRecord("SELECT AktIntTimestamp [LongValue] FROM tblAktenInt WHERE AktIntID = " + aktId, typeof (SingleValue));

//            return "[ "+sv.LongValue.ToString()+" ]  " + (sv.LongValue.ToString() == timeStamp ? "N" : "Y");
            if (sv.LongValue.ToString() != timeStamp)
            {
                //                Log.Info("IsAktChanged DONE_1  [aktId: " + aktId + "] [ElapsedMilliseconds: +" + sp.ElapsedMilliseconds + "] [balanceAmount: " + balanceAmount + "]  [timeStamp:" + timeStamp + "]  [phoneTimeStamps: " + phoneTimeStamps + "] [addressTimeStamps: " + addressTimeStamps + "]  [docTimeStamps: " + docTimeStamps + "]");
                return true;
            }

            qryAktenInt qryAkt = HTBUtils.GetInterventionAktQry(aktId);
            if (qryAkt != null)
            {
                if (!HTBUtils.AlmostEqual(AktInterventionUtils.GetAktAmounts(qryAkt).GetTotal(), balanceAmount))
                {
                    //                    Log.Info("IsAktChanged DONE_2  [aktId: " + aktId + "] [ElapsedMilliseconds: +" + sp.ElapsedMilliseconds + "] [balanceAmount: " + balanceAmount + "]  [timeStamp:" + timeStamp + "]  [phoneTimeStamps: " + phoneTimeStamps + "] [addressTimeStamps: " + addressTimeStamps + "]  [docTimeStamps: " + docTimeStamps + "]");
                    return true;
                }

                if (AreDocTimeStampsOK(qryAkt, docTimeStamps))
                {
                    //                    Log.Info("IsAktChanged DONE_3  [aktId: " + aktId + "] [aktId: " + aktId + "]  [balanceAmount: " + balanceAmount + "]  [timeStamp:" + timeStamp + "]  [phoneTimeStamps: " + phoneTimeStamps + "] [addressTimeStamps: " + addressTimeStamps + "]  [docTimeStamps: " + docTimeStamps + "]");
                    return true;
                }
            }
            //            Log.Info("IsAktChanged DONE_4  [aktId: " + aktId + "] [ElapsedMilliseconds: +" + sp.ElapsedMilliseconds + "] [balanceAmount: " + balanceAmount + "]  [timeStamp:" + timeStamp + "]  [phoneTimeStamps: " + phoneTimeStamps + "] [addressTimeStamps: " + addressTimeStamps + "]  [docTimeStamps: " + docTimeStamps + "]");
            return false;
        }

        [WebMethod()]
        public string GetAktTimestamp(int aktId)
        {
            var sv = (SingleValue)HTBUtils.GetSqlSingleRecord("SELECT AktIntTimestamp [LongValue] FROM tblAktenInt WHERE AktIntID = " + aktId, typeof(SingleValue));

            return sv.LongValue.ToString();
        }

        [WebMethod()]
        public string SaveInstallmentInfo(int aktId, DateTime startDate, double amount, int months, int day, int inkassoType, string info)
        {
            var sb = new StringBuilder("UPDATE tblAktenInt SET AKTIntRVStartDate = '");
            sb.Append(startDate.ToShortDateString());
            sb.Append("', AKTIntRVAmmount = ");
            sb.Append(amount.ToString());
            sb.Append(", AKTIntRVNoMonth = ");
            sb.Append(months.ToString());
            sb.Append(", AKTIntRVIntervallDay = ");
            sb.Append(day.ToString());
            sb.Append(", AKTIntRVInkassoType = ");
            sb.Append(inkassoType.ToString());
            sb.Append(", AKTIntRVInfo = '");
            sb.Append(info);
            
            sb.Append("' WHERE AktIntID = ");
            sb.Append(aktId);

            return new RecordSet().ExecuteNonQuery(sb.ToString()) > 0 ? "Y" : "N";
        }

        [WebMethod()]
        public string SaveInstallmentPlan(int aktId, XmlAktInstallmentCalcRecord rec)
        {
            return "Got it!";
        }

        private bool AreDocTimeStampsOK(qryAktenInt qryAkt, string[] docTimeStamps)
        {
            if (docTimeStamps == null)
                return true;
            ArrayList docTimestampsList = HTBUtils.GetSqlRecords("SELECT DokTimestamp [LongValue] FROM qryDoksIntAkten WHERE AktIntID = " + qryAkt.AktIntID, typeof(SingleValue));
            if (qryAkt.IsInkasso())
            {
                HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT DokTimestamp [LongValue] FROM qryDoksInkAkten WHERE CustInkAktID = " + qryAkt.AktIntCustInkAktID, typeof(SingleValue)), docTimestampsList);
            }

            return !AreTimestampsEqual(docTimestampsList, docTimeStamps);
        }

        private bool AreTimestampsEqual(ArrayList list, string[] timestamps)
        {
            if (list.Count != timestamps.Length)
                return true;
            if ((from SingleValue sv in list select timestamps.Any(timestamp => sv.LongValue.ToString() == timestamp)).Any(found => !found))
            {
                return false;
            }
            return true;
        }
    }
}
