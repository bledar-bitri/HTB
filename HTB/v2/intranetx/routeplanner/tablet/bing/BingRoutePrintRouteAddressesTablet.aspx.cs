using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls;
using dto_Road = HTB.v2.intranetx.routeplanner.dto.Road;
using Road = HTB.v2.intranetx.routeplanner.dto.Road;
using routeplanner_dto_Road = HTB.v2.intranetx.routeplanner.dto.Road;

namespace HTB.v2.intranetx.routeplanner.tablet.bing
{
    public partial class BingRoutePrintRouteAddressesTablet : System.Web.UI.Page
    {
        private readonly string _tabletProcessedAktRowBackColor = HTBUtils.GetConfigValue("TabletProcessedAktRowBackColor");
        private static readonly string AppName = HTBUtils.GetConfigValue("iPad_AppName");

        protected void Page_Load(object sender, EventArgs e)
        {
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            var dt = GetInvoicesDataTableStructure();
            var addr1 = string.Empty;
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
                var akt = HTBUtils.GetInterventionAktQry(address.ID);
                dr["Index"] = address.ID == -1 ? "" : idx.ToString();
                dr["Address"] = address.Address;
                dr["Akt"] = address.ID == -1 ? "" : address.ID.ToString();
                dr["Gegner"] = GetGegnerInfo(akt);
                SetRowBackColor(akt, dr);
                if (addr1 != string.Empty && addr2 != string.Empty)
                {
                    routeplanner_dto_Road r = rpManager.GetRoadBetweenAddresses(addr1, addr2);
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
                    akt = HTBUtils.GetInterventionAktQry(aktId);
                    SetGegnerInfo(idx, akt, dr);
                    SetRowBackColor(akt, dr);
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
            dt.Columns.Add(new DataColumn("RowBackColor", typeof(string)));
            return dt;
        }

        private void SetGegnerInfo(int idx, qryAktenInt akt, DataRow dr)
        {
            if(akt != null)
            {
                dr["Index"] = idx.ToString();
                dr["Address"] = akt.GegnerLastStrasse + ", " + akt.GegnerLastZip + " " + akt.GegnerLastOrt;
                dr["Akt"] = akt.AktIntID.ToString();
                dr["Gegner"] = GetGegnerInfo(akt);   
            }
        }

        private void SetRowBackColor(qryAktenInt akt, DataRow dr)
        {
            if (akt != null && akt.AktIntStatus != 1)
                dr["RowBackColor"] = _tabletProcessedAktRowBackColor;
            else
                dr["RowBackColor"] = "WHITE";
        }

        private string GetGegnerInfo(qryAktenInt akt)
        {
            if (akt != null)
            {
                var sb = new StringBuilder();
                sb.Append("<a href=\"javascript:MM_openBrWindow('/v2/intranetx/aktenint/AuftragTablet.aspx?");
                sb.Append(GlobalHtmlParams.GEGNER_ID);
                sb.Append("=");
                sb.Append(akt.GegnerID);
                sb.Append("', 'AuftragPopWindow','menubar=yes,scrollbars=yes,resizable=yes,width=900,height=800')\">");
                sb.Append(akt.GegnerLastName1);
                sb.Append(" ");
                sb.Append(akt.GegnerLastName2);
                sb.Append("<br>");
                sb.Append(akt.GegnerPhoneCity);
                sb.Append(" ");
                sb.Append(akt.GegnerPhone);

                sb.Append("</a><br/><br/><a href=\"");
                sb.Append(AppName);
                sb.Append("://?");
                GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, "AktID", akt.AktIntID.ToString());
                sb.Append("\">iPad App</a><br/>&nbsp;<br/>&nbsp;");
                return sb.ToString();
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
                    routeplanner_dto_Road r = rpManager.GetRoadBetweenAddresses(addr1, addr2);
                    if (r != null)
                    {
                        return (int)r.TravelTimeInSeconds;
                    }
                }
                addr1 = address.Address;
            }
            return 0;
        }

        protected void gvAddresses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lblBgColor = (Label)e.Row.FindControl("lblRowBackColor");
                if(lblBgColor != null)
                    e.Row.BackColor = Color.FromName(lblBgColor.Text);
            }
        }

    }
}