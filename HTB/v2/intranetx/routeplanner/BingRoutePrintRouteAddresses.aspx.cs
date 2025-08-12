using System;
using System.Data;
using System.Text;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBUtilities;
using Road = HTB.v2.intranetx.routeplanner.dto.Road;

namespace HTB.v2.intranetx.routeplanner
{
    using Road = dto.Road;

    public partial class BingRoutePrintRouteAddresses : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 3600;
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            DataTable dt = GetInvoicesDataTableStructure();
            string addr1 = string.Empty;
            double totalDistance = 0;
            long totalTimeInSeconds = 0;
            DateTime firstAppointmentTime = rpManager.FirstAppointmentTime;
            if (firstAppointmentTime == DateTime.MinValue)
            {
                var ts = new TimeSpan(8, 0, 0);
                firstAppointmentTime = firstAppointmentTime.Date + ts;
            }
            else
            {
                firstAppointmentTime = firstAppointmentTime.Subtract(new TimeSpan(0,0,0, GetInitialTravelTime(rpManager)));
            }
            int idx = 1;
            foreach (var address in rpManager.GetTourAddresses())
            {
               
                string addr2 = address.Address;
                DataRow dr = dt.NewRow();
                dr["Index"] = address.ID == -1 ? "" : idx.ToString();
                dr["Address"] = address.Address;
                dr["Akt"] = address.ID == -1 ? "" : GetAktActionLink(address.ID);
                dr["Gegner"] = GetGegnerInfo(address.ID);
                if (addr1 != string.Empty && addr2 != string.Empty)
                {
                    Road r = rpManager.GetRoadBetweenAddresses(addr1, addr2);
                    totalDistance += r.Distance;
                    totalTimeInSeconds += r.TravelTimeInSeconds;
                    if(r != null)
                    {
                        firstAppointmentTime = firstAppointmentTime.AddSeconds(r.TravelTimeInSeconds);
                        dr["Distance"] = "&nbsp;" + r.Distance.ToString("N2") + "&nbsp;km";
                        dr["Time"] = GetTimeString (r.TravelTimeInSeconds);
                        dr["TotalDistance"] = "&nbsp;" + totalDistance.ToString("N2") + "&nbsp;km";
                        dr["TotalTime"] = GetTimeString(totalTimeInSeconds);
                        dr["ExampleTime"] = firstAppointmentTime.ToShortTimeString()+ "&nbsp;";
                        firstAppointmentTime = firstAppointmentTime.AddSeconds(RoutePlanerManager.ADGegnerStopMinutes * 60);
                        totalTimeInSeconds += RoutePlanerManager.ADGegnerStopMinutes * 60;
                    }
                    dt.Rows.Add(dr);
                    if( address.ID != -1)idx++;
                }
                else
                {
                    dr["ExampleTime"] = firstAppointmentTime.ToShortTimeString();
                    dt.Rows.Add(dr);
                    if (address.ID != -1) idx++;
                }
                foreach (var aktId in address.OtherIds)
                {
                    dr = dt.NewRow();
                    SetGegnerInfo(idx, aktId, dr);
                    dt.Rows.Add(dr);
                    if (address.ID != -1) idx++;
                }
                addr1 = address.Address;
            }
            gvAddresses.DataSource = dt;
            gvAddresses.DataBind();
        }

        private DataTable GetInvoicesDataTableStructure()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("Index", typeof(string)));
            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("Akt", typeof(string)));
            dt.Columns.Add(new DataColumn("Gegner", typeof(string)));
            dt.Columns.Add(new DataColumn("Distance", typeof(string)));
            dt.Columns.Add(new DataColumn("Time", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalDistance", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalTime", typeof(string)));
            dt.Columns.Add(new DataColumn("ExampleTime", typeof(string)));
            return dt;
        }

        private string GetAktActionLink(int aktId)
        {
            if (aktId > 0)
            {
                tblAktenInt akt = HTBUtils.GetInterventionAkt(aktId);
                if (akt != null && akt.AktIntStatus == 1)
                {
                    var sb = new StringBuilder();
                    sb.Append("<a href=\"javascript:MM_openBrWindow('/v2/intranetx/aktenint/workaktint.aspx?");
                    sb.Append(GlobalHtmlParams.ID);
                    sb.Append("=");
                    sb.Append(aktId);
                    sb.Append("', 'ActionPopWindow','menubar=yes,scrollbars=yes,resizable=yes,width=900,height=800')\">");
                    sb.Append(aktId);
                    sb.Append("</a>");
                    return sb.ToString();
                }
            }
            return aktId.ToString();
        }
        private void SetGegnerInfo(int idx, int aktId, DataRow dr)
        {
            qryAktenInt akt = HTBUtils.GetInterventionAktQry(aktId);
            if(akt != null)
            {
                dr["Index"] = idx.ToString();
                dr["Address"] = akt.GegnerLastStrasse + ", " + akt.GegnerLastZip + " " + akt.GegnerLastOrt;
                dr["Akt"] = GetAktActionLink(aktId);
                dr["Gegner"] = GetGegnerInfo(aktId);
            }
        }

        private string GetGegnerInfo(int aktId)
        {

            return GetGegnerInfo(HTBUtils.GetInterventionAktQry(aktId));
        }

        private string GetGegnerInfo(qryAktenInt akt)
        {
            if (akt != null)
            {
                return akt.GegnerLastName1 + " " + akt.GegnerLastName2 + "<br>" + akt.GegnerPhoneCity + " " + akt.GegnerPhone;
            }
            return "";
        }

        private int GetHours(long sec)
        {
            return (int) sec/3600;
        }

        private int GetMins(long sec)
        {
            return (int) sec/60;
        }

        private string GetTimeString(long sec)
        {
            var sb = new StringBuilder("&nbsp;");
            int h = GetHours(sec);
            int m = GetMins(sec - (h*3600));
            if (h > 0)
                sb.Append(h + "&nbsp;Std.&nbsp;");
            if (m > 0)
                sb.Append(m + "&nbsp;Min.");
            if (h == 0 && m == 0)
                sb.Append(sec + "&nbsp;Sec.");
            return sb.ToString();
        }

        private int GetInitialTravelTime(RoutePlanerManager rpManager)
        {
            string addr1 = string.Empty;
            foreach (var address in rpManager.GetTourAddresses())
            {
                string addr2 = address.Address;
                if (addr1 != string.Empty && addr2 != string.Empty)
                {
                    Road r = rpManager.GetRoadBetweenAddresses(addr1, addr2);
                    if (r != null)
                    {
                        return (int)r.TravelTimeInSeconds;
                    }
                }
                addr1 = address.Address;
            }
            return 0;
        }
    }
}