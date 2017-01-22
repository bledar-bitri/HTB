using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using HTB.Database;
using HTB.Database.HTB.StoredProcs;
using HTB.v2.intranetx.util;
using HTBUtilities;
using System.Web.UI.WebControls;

namespace HTB.v2.intranetx.routeplanter.bingmaps
{
    public partial class BingRouteActionsDisplay : Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected string JsPushpins;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadUserDropdownList(ddlUser, GlobalUtilArea.GetUsers(Session));
                /*
                var parameters = new ArrayList
                                     {
                                         new StoredProcedureParameter("userID", SqlDbType.Int, 99), 
                                         new StoredProcedureParameter("startActionDate", SqlDbType.DateTime, "01.05.2000"), 
                                         new StoredProcedureParameter("endActionDate", SqlDbType.DateTime, "01.11.2013")
                                     };
                ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetActionCoordinates", parameters, typeof(spGetActionCoordinates));
                var sb = new StringBuilder();

                for (int i = 0; i < list.Count; i++)
                {
                    var coordinate = (spGetActionCoordinates)list[i];
                    sb.Append(GetPushpin(i + 1, coordinate.AktIntActionLatitude, coordinate.AktIntActionLongitude));
                    sb.Append(Environment.NewLine);
                }
                JsPushpins = sb.ToString();
                PopulateCoordinatesGrid(list);
//                 */
            }
        }

        #region Event Handler
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DateTime dateFrom = GlobalUtilArea.GetNowIfConvertToDateError(txtDateFrom);
            DateTime dateTo = GlobalUtilArea.GetNowIfConvertToDateError(txtDateTo);

            var parameters = new ArrayList
                                     {
                                         new StoredProcedureParameter("userID", SqlDbType.Int, ddlUser.SelectedValue), 
                                         new StoredProcedureParameter("startActionDate", SqlDbType.DateTime, dateFrom.ToShortDateString()), 
                                         new StoredProcedureParameter("endActionDate", SqlDbType.DateTime, dateTo.ToShortDateString())
                                     };
            ArrayList list = HTBUtils.GetStoredProcedureRecords("spGetActionCoordinates", parameters, typeof(spGetActionCoordinates));
            var sb = new StringBuilder();
            
            for (int i = 0; i < list.Count; i++) 
            {
                var coordinate = (spGetActionCoordinates)list[i];
                sb.Append(GetPushpin(i + 1, coordinate.AktIntActionLatitude, coordinate.AktIntActionLongitude));
                sb.Append(Environment.NewLine);
            }
            JsPushpins = sb.ToString();
            PopulateCoordinatesGrid(list);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private string GetPushpin (int index, double lat, double lon)
        {
            var sb = new StringBuilder("var pushpinOptions");
            sb.Append(index);
            sb.Append(" = { icon:'/v2/intranet/images/poi_custom.png', text: '");
            sb.Append(index);
            sb.Append("', visible: true, textOffset: offset };");
            sb.Append(Environment.NewLine);

            sb.Append("var pushpin");
            sb.Append(index);
            sb.Append(" = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(");
            sb.Append(lat.ToString().Replace(",", "."));
            sb.Append(", ");
            sb.Append(lon.ToString().Replace(",", "."));
            sb.Append("), pushpinOptions");
            sb.Append(index);
            sb.Append(");");
            sb.Append(Environment.NewLine);
            sb.Append("map.entities.push(pushpin");
            sb.Append(index);
            sb.Append(");");
            sb.Append(Environment.NewLine);

            sb.Append("pushpin");
            sb.Append(index);
            sb.Append(".Name = '");
            sb.Append(index);
            sb.Append("';");
            sb.Append(Environment.NewLine);

            sb.Append("Microsoft.Maps.Events.addHandler(pushpin");
            sb.Append(index);
            sb.Append(", 'mouseover', pushPinMouseOver);");
            sb.Append(Environment.NewLine);

            sb.Append("Microsoft.Maps.Events.addHandler(pushpin");
            sb.Append(index);
            sb.Append(", 'mouseout', pushPinMouseOut);");
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
        
        #region Grid
        private void PopulateCoordinatesGrid(ArrayList list)
        {
            DataTable dt = GetCoordinatesDataTableStructure();
            int idx = 1;
            foreach (spGetActionCoordinates rec in list)
            {

                DataRow dr = dt.NewRow();

                var actionDate = new StringBuilder("<nobr>");
                actionDate.Append(GetDateAndTime(rec.AktIntActionDate));
                actionDate.Append("</nobr>");

                var actionAdr = new StringBuilder("<nobr>");
                actionAdr.Append(rec.AktIntActionAddress);
                actionAdr.Append("</nobr>");
                if (rec.AktIntActionLatitude > 0)
                {
                    actionAdr.Append("<BR/>");
                    actionAdr.Append(rec.AktIntActionLatitude.ToString().Replace(",", "."));
                    actionAdr.Append(" ");
                    actionAdr.Append(rec.AktIntActionLongitude.ToString().Replace(",", "."));
                }


                var actionCaption = new StringBuilder("<nobr>");
                actionCaption.Append(rec.AktIntActionTypeCaption);
                actionCaption.Append("</nobr>");
                if(rec.AktIntActionBetrag > 0)
                {
                    actionCaption.Append("<BR/><font class=\"headerText\">KASSIERT:&nbsp;&nbsp;&nbsp;");
                    actionCaption.Append(HTBUtils.FormatCurrency(rec.AktIntActionBetrag));
                    actionCaption.Append("<BR/></font>");
                }

                dr["ActionIndex"] = idx.ToString();
                dr["ActionDate"] = actionDate.ToString();
                dr["ActionAddress"] = actionAdr.ToString();
                dr["ActionCaption"] = actionCaption.ToString();
                dr["GegnerName"] = "<nobr>" + rec.GegnerLastName1 + " " + rec.GegnerLastName2 + "</nobr>";
                dr["GegnerAddress"] = "<nobr>" + rec.GegnerLastStrasse + "</nobr><BR/><nobr/>" + 
                    rec.GegnerLastZip + " " +
                    rec.GegnerLastOrt + "&nbsp;&nbsp;&nbsp;[" + 
                    rec.GegnerLatitude.ToString().Replace(",", ".") + " " +
                    rec.GegnerLongitude.ToString().Replace(",", ".") + "]</nobr>";

                idx++;
                dt.Rows.Add(dr);
            }
            gvCoordinates.DataSource = dt;
            gvCoordinates.DataBind();
            PopulateSummaryGrid(list);
        }
        private DataTable GetCoordinatesDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("ActionIndex", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionDate", typeof(string)));

            dt.Columns.Add(new DataColumn("ActionAddress", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("GegnerName", typeof(string)));
            dt.Columns.Add(new DataColumn("GegnerAddress", typeof(string)));
            return dt;
        }

        private void PopulateSummaryGrid(ArrayList list)
        {
            DataTable dt = GetSummaryTableStructure();
            int positivActions = GetPositiveActions(list);
            AddDataRowForTextAndValue(dt, "Totale Aktionen", list.Count.ToString(), true);
            AddDataRowForTextAndValue(dt, "Positive", positivActions.ToString(), true);
            AddDataRowForTextAndValue(dt, "Negative", (list.Count - positivActions).ToString(), true);
            if(positivActions > 0 && list.Count > 0)
                AddDataRowForTextAndValue(dt, "Positivprocent", Math.Round((decimal)((double)positivActions / (double)list.Count * 100), 1).ToString()+"%", true);
            AddDataRowForTextAndValue(dt, "Gelt Kassiert", HTBUtils.FormatCurrency(GetCollectedAmount(list)), true);

            if (list.Count > 0)
            {
//                DateTime firstActionTime = ((spGetActionCoordinates) list[0]).AktIntActionDate;
//                DateTime lastActionTime = ((spGetActionCoordinates)list[list.Count-1]).AktIntActionDate;

                DateTime firstActionTime = GetFirstOrLastActionTime(list, true);
                DateTime lastActionTime = GetFirstOrLastActionTime(list, false);

                TimeSpan tripDuration = lastActionTime.Subtract(firstActionTime);

                var duration = new StringBuilder();
                if(tripDuration.Days > 0)
                {
                    duration.Append(tripDuration.Days);
                    duration.Append("T ");
                }
                if (tripDuration.Hours > 0)
                {
                    duration.Append(tripDuration.Hours);
                    duration.Append("H ");
                }
                if (tripDuration.Minutes > 0)
                {
                    duration.Append(tripDuration.Minutes);
                    duration.Append("M ");
                }

                AddDataRowForTextAndValue(dt, "Erste Aktion", GetDateAndTime(firstActionTime));
                AddDataRowForTextAndValue(dt, "Letzte Aktion", GetDateAndTime(lastActionTime));
                AddDataRowForTextAndValue(dt, "Dauer", duration.ToString());
                AddDataRowForTextAndValue(dt, "Distanz", "&nbsp;");
            }
            AddDataRowForTextAndValue(dt, "Tank_beleg", "&nbsp;");
            AddDataRowForTextAndValue(dt, "Tank_liter", "&nbsp;");
            AddDataRowForTextAndValue(dt, "Tank_betrag", "&nbsp;");
            AddDataRowForTextAndValue(dt, "N&auml;chtigung", "&nbsp;");
            AddDataRowForTextAndValue(dt, "&nbsp;", "&nbsp;");
            PopulateDailyInfo(dt, list);

            gvSummary.DataSource = dt;
            gvSummary.DataBind();
        }

        private void PopulateDailyInfo(DataTable dt, ArrayList list)
        {
            // populate dates to lookup
            var dates = new List<DateTime>();
            foreach (spGetActionCoordinates coor in list)
            {
                bool found = false;
                foreach (var dte in dates)
                {
                    if (coor.AktIntActionDate.ToShortDateString() == dte.ToShortDateString())
                        found = true;
                }
                if(!found)
                {
                    dates.Add(coor.AktIntActionDate);
                }
            }

            foreach (var dte in dates)
            {
                List<spGetActionCoordinates> dailyActions = GetListForDay(list, dte);
                PopulateDailyInfo(dt, dte, dailyActions);
            }
        }

        private void PopulateDailyInfo(DataTable dt, DateTime dte, List<spGetActionCoordinates> list)
        {
            int positivActions = GetPositiveActions(list);

            AddDataRowForTextAndValue(dt, " ", " ");
            AddDataRowForTextAndValue(dt, GlobalUtilArea.GetDayOfWeekName(dte), dte.ToShortDateString(), true);
            AddDataRowForTextAndValue(dt, "Totale Aktionen", list.Count.ToString(), false, true);
            AddDataRowForTextAndValue(dt, "Positive", positivActions.ToString(), false, true);
            AddDataRowForTextAndValue(dt, "Negative", (list.Count - positivActions).ToString(), false, true);
            AddDataRowForTextAndValue(dt, "Gelt Kassiert", HTBUtils.FormatCurrency(GetCollectedAmount(list)), false, true);

            if (list.Count > 0)
            {
//                DateTime firstActionTime = list[0].AktIntActionDate;
//                DateTime lastActionTime = list[list.Count - 1].AktIntActionDate;
                
                DateTime firstActionTime = GetFirstOrLastActionTime(list, true);
                DateTime lastActionTime = GetFirstOrLastActionTime(list, false);

                TimeSpan tripDuration = lastActionTime.Subtract(firstActionTime);

                var duration = new StringBuilder();
                if (tripDuration.Hours > 0)
                {
                    duration.Append(tripDuration.Hours);
                    duration.Append("H ");
                }
                if (tripDuration.Minutes > 0)
                {
                    duration.Append(tripDuration.Minutes);
                    duration.Append("M ");
                }

                AddDataRowForTextAndValue(dt, "Erste Aktion", firstActionTime.ToShortTimeString(), false, true);
                AddDataRowForTextAndValue(dt, "Letzte Aktion", lastActionTime.ToShortTimeString(), false, true);
                AddDataRowForTextAndValue(dt, "Dauer", duration.ToString(), false, true);
            }

            gvSummary.DataSource = dt;
            gvSummary.DataBind();
        }

        private int GetPositiveActions(ArrayList list)
        {
            return list.Cast<spGetActionCoordinates>().Count(rec => rec.AktIntActionIsPositive);
        }

        private int GetPositiveActions(IEnumerable<spGetActionCoordinates> list)
        {
            return list.Count(rec => rec.AktIntActionIsPositive);
        }

        private double GetCollectedAmount(ArrayList list)
        {
            return list.Cast<spGetActionCoordinates>().Sum(rec => rec.AktIntActionBetrag);
        }
        private double GetCollectedAmount(IEnumerable<spGetActionCoordinates> list)
        {
            return list.Sum(rec => rec.AktIntActionBetrag);
        }

        private List<spGetActionCoordinates> GetListForDay(ArrayList list, DateTime dte)
        {
            return list.Cast<spGetActionCoordinates>().Where(coor => coor.AktIntActionDate.ToShortDateString() == dte.ToShortDateString()).ToList();
        }

        private string GetDateAndTime(DateTime dte)
        {
            var dteString = new StringBuilder(dte.ToShortDateString());

            if (dte.ToShortTimeString() != "00:00")
            {
                dteString.Append(" ");
                dteString.Append(dte.ToShortTimeString());
            }
            return dteString.ToString();
        }

        private DateTime GetFirstOrLastActionTime(ArrayList list, bool isFirst)
        {
            return GetFirstOrLastActionTime(list.Cast<spGetActionCoordinates>().ToList(), isFirst);
        }
        private DateTime GetFirstOrLastActionTime(List<spGetActionCoordinates> list, bool isFirst)
        {

            if (list.Count > 0)
            {
                int currentIdx = 0;
                int endIdx = list.Count - 1;
                int increment = 1;
                if (!isFirst)
                {
                    currentIdx = list.Count - 1;
                    endIdx = 0;
                    increment = -1;
                }
                DateTime actionTime = list[currentIdx].AktIntActionDate;
                if (actionTime.ToShortTimeString() == "00:00")
                    for (int i = currentIdx; (isFirst && i < endIdx) || (!isFirst && i >= endIdx); i += increment)
                    {
                        actionTime = list[i].AktIntActionDate;
                        if (actionTime.ToShortTimeString() != "00:00")
                        {
                            return actionTime;
                        }
                    }
                return list[currentIdx].AktIntActionDate;
            }
            return HTBUtils.DefaultDate;
        }

        private void AddDataRowForTextAndValue(DataTable dt, string text, string value, bool isBold = false, bool isDetail = false)
        {
            DataRow dr = dt.NewRow();
            var sbText = new StringBuilder();
            var sbValue = new StringBuilder();

            if (isDetail)
                sbText.Append("&nbsp;&nbsp;&nbsp;&nbsp;");


            if(isBold)
            {
                sbText.Append("<strong>");
                sbValue.Append("<strong>");
            }
            sbText.Append(text);
            sbValue.Append(value);
            if (isBold)
            {
                sbText.Append("</strong>");
                sbValue.Append("</strong>");
            }

            dr["IsDetail"] = isDetail.ToString();
            dr["Text"] = sbText.ToString();
            dr["Value"] = sbValue.ToString();
            dt.Rows.Add(dr);
            
        }
        private DataTable GetSummaryTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("IsDetail", typeof(string)));
            dt.Columns.Add(new DataColumn("Text", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));
            return dt;
        }

        protected void gvCoordinates_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("ID", "CoordinateRow_"+(e.Row.DataItemIndex+1));
            }
        }
        /*
        protected void gvSummary_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lblIsDetail = (Label)e.Row.FindControl("lblIsDetail");
                if(lblIsDetail != null && HTBUtils.GetBoolValue(lblIsDetail.Text))
                {
                    e.Row.Attributes.Add("style", "name=\"detailStyle\"");
                }
            }
        }*/
        protected void gvSummary_RowCreated(object sender, EventArgs e)
        {
            int count = 0;
            for (int i = 0; i < gvSummary.Rows.Count; i++)
            {
                GridViewRow row = gvSummary.Rows[i];
                var lblIsDetail = (Label)row.FindControl("lblIsDetail");
                if(lblIsDetail != null && HTBUtils.GetBoolValue(lblIsDetail.Text))
                {
                    row.Attributes.Add("class", count++ % 2 == 0 ? "detailStyle" : "detailStyle1");
                }
                else
                {
                    count = 0;
                }
            }
        }
        #endregion
    }
}