using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using HTBExtras;
using HTBUtilities;
using System.Collections;
using HTBAktLayer;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTB.v2.intranetx.routeplanner;
using System.Text;

namespace HTB.v2.intranetx.aktenint
{
    public partial class PrintAuftrag : Page
    {
        public ArrayList aktenList = new ArrayList();
        public ArrayList aktePos = new ArrayList();
        public bool printRV = false;

        tblControl control = HTBUtils.GetControlRecord();

        string aktIdList;
        string qryAktCommand;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string source = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.SOURCE]);
            if(source == GlobalHtmlParams.RoutePlanner)
                LoadFromRoutePlannerManager();
            else 
                LoadFromRequestParams();
            printRV = !string.IsNullOrEmpty(Request["RV"]);
        }

        private void LoadFromRequestParams()
        {
            if (!string.IsNullOrEmpty(Request[GlobalHtmlParams.ID]))
            {
                aktIdList = Request[GlobalHtmlParams.ID];

                qryAktCommand = "SELECT * FROM dbo.qryAktenInt WHERE AktIntID in (" + aktIdList + ")";
                if (Request["sortfield"] != null && Request["sortfield"] != "")
                {
                    qryAktCommand += "ORDER BY " + Request["sortfield"];
                }
                aktenList = HTBUtils.GetSqlRecords(qryAktCommand, typeof(qryAktenInt));
            }
        }

        private void LoadFromRoutePlannerManager()
        {
            var rpManager = (RoutePlanerManager) Session[GlobalHtmlParams.SessionRoutePlannerManager];

            //Get AktIds from the Route Planner
            List<int> ids = rpManager.GetAllIDs();
            var sb = new StringBuilder();

            for (int i = 0; i < ids.Count; i++)
            {
                sb.Append(ids[i].ToString());
                if(i < ids.Count - 1)
                    sb.Append(", ");
            }

            qryAktCommand = "SELECT * FROM dbo.qryAktenInt WHERE AktIntID in (" + sb + ")";
            List<qryAktenInt> recordsList = HTBUtils.GetSqlRecords(qryAktCommand, typeof (qryAktenInt)).Cast<qryAktenInt>().ToList();

            var sortedList = from i in ids
                             join records in recordsList
                                 on i equals records.AktIntID
                             select records;

            var enumList = sortedList.GetEnumerator();

            while (enumList.MoveNext())
                aktenList.Add(enumList.Current);
        }


        public void SetStatusPrinted(int aktId)
        {
            new RecordSet().ExecuteNonQuery("Update tblAktenInt Set AktIntDruckkennz = 1 WHERE AKTIntId = " + aktId);
        }

        public ArrayList GetPosList(int aktId)
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + aktId, typeof(tblAktenIntPos));
        }
    }

    
    
}