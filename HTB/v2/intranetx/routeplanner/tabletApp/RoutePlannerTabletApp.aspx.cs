using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.permissions;
using HTB.v2.intranetx.progress;
using HTB.v2.intranetx.routeplanner.dto;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.routeplanner.tabletApp
{
    public partial class RoutePlannerTabletApp : Page, ICallbackEventHandler
    {
        private const string TxtPlzFromPrefix = "txtPlzFrom_";
        private const string TxtPlzToPrefix = "txtPlzTo_";
        private static string _startAddress = "";
        private string _taskId;
        private readonly PermissionsRoutePlanner _permissions = new PermissionsRoutePlanner();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["MM_UserID"] = "99";
            Server.ScriptTimeout = 3600 * 3;
            ctlMessage.Clear();
            _permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            DrawPlzTable();

            if (_permissions.GrantRoutePlannerForAll || _permissions.GrantRoutePlannerForSelfOnly)
            {
                _taskId = Context.User.Identity.Name + ":BingRoutePlanner";

                if (!IsPostBack)
                {
                    SetUserInfo();
                    GlobalUtilArea.LoadDropdownList(ddlAktType,
                                                    "SELECT * FROM tblAktTypeINT ORDER BY AktTypeINTCaption ASC",
                                                    typeof (tblAktTypeInt),
                                                    "AktTypeINTID",
                                                    "AktTypeINTCaption",
                                                    true
                        );
                    if (_permissions.GrantRoutePlannerForAll)
                    {
                        GlobalUtilArea.LoadUserDropdownList(ddlAD, GlobalUtilArea.GetUsers(Session));
                        lblAD.Visible = false;
                        try
                        {
                            ddlAD.SelectedValue = GlobalUtilArea.GetUserId(Session).ToString();
                        }
                        catch
                        {
                        }
                    }
                    else if (_permissions.GrantRoutePlannerForSelfOnly)
                    {
                        ddlAD.Visible = false;
                    }
                    PopulateSavedRoutes();
                }
                if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RecalculateRoute]) == GlobalHtmlParams.YES)
                {
                    var rpManager = (RoutePlanerManager) Session[GlobalHtmlParams.SessionRoutePlannerManager];
                    if (rpManager != null)
                        txtRouteName.Text = rpManager.RouteName;
                }
            }
            else
            {
                trAD.Visible = false;
                trAddress.Visible = false;
                trPLZ.Visible = false;
                trAktType.Visible = false;
                trButtons.Visible = false;
                ctlMessage.ShowError("Sie dürfen den Routenplaner nicht benützen!");
            }
        }

        #region Event Handlers

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                int foundCount = HTBUtils.GetSqlRecords(GetSqlQueryBasedOnInput(), typeof (qryAktenInt)).Count;
                if (foundCount == 0)
                {
                    ctlMessage.ShowError("Keine Akte gefunden!");
                }
                else
                {
                    _startAddress = txtStartAddress.Text;

                    var status = new TaskStatus();
                    Context.Cache[_taskId] = status;

                    string script1 = ScriptHelpers.GetStarterScript(this, "StartTask", "btnSubmit");
                    ClientScript.RegisterClientScriptBlock(GetType(), "StartTask", script1, true);
                    bdy.Attributes["onload"] = "javascript:StartTask()";
                }
            }
        }

        protected void lnkAddPlz_Clicked(object sender, EventArgs e)
        {
            int existingRows = GetNumberOfPlzRanges();
            AddRowToPlzTable(existingRows);
            SetNumberOfPlzRanges(existingRows + 1);
        }

        protected void ddlAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetUserInfo();
        }
        #endregion

        private bool ValidateEntry()
        {
            bool ok = true;
            var sb = new StringBuilder();
            for (int i = 0; i < GetNumberOfPlzRanges(); i++)
            {
                var txtStartPLZ = (TextBox) tblPlz.FindControl(TxtPlzFromPrefix + i);
                var txtEndPLZ = (TextBox) tblPlz.FindControl(TxtPlzToPrefix + i);
                if ((string.IsNullOrEmpty(txtStartPLZ.Text) && !string.IsNullOrEmpty(txtEndPLZ.Text)) ||
                    (!string.IsNullOrEmpty(txtStartPLZ.Text) && string.IsNullOrEmpty(txtEndPLZ.Text)))
                {
                    sb.Append("Sie m&uuml;ssen [PLZ von] UND [PLZ bis] eingeben!<BR/>");
                }
                else {
                    if (!string.IsNullOrEmpty(txtStartPLZ.Text) && GlobalUtilArea.GetZeroIfConvertToIntError(txtStartPLZ.Text) == 0)
                    {
                        ok = false;
                        sb.Append("PLZ von ist falsch!<BR/>");
                    }
                    if (!string.IsNullOrEmpty(txtEndPLZ.Text) && GlobalUtilArea.GetZeroIfConvertToIntError(txtEndPLZ.Text) == 0)
                    {
                        ok = false;
                        sb.Append("PLZ bis ist falsch!<BR/>");
                    }
                }
            }
            if (string.IsNullOrEmpty(txtRouteName.Text.Trim()))
            {
                ok = false;
                sb.Append("Sie müssen ein Routennamen eingeben!<BR/>");
            }
            if (!ok)
            {
                ctlMessage.ShowError(sb.ToString());
            }
            return ok;
        }

        private void SetUserInfo()
        {

            int userId = GlobalUtilArea.GetUserId(Session);
            if (_permissions.GrantRoutePlannerForAll)
            {
                try
                {
                    userId = Convert.ToInt32(ddlAD.SelectedValue);
                }
                catch
                {
                    userId = GlobalUtilArea.GetUserId(Session);
                }
            }
            if (userId > 0)
            {
                var user = GetUser(userId);
                if (user != null)
                {
                    txtStartAddress.Text = user.UserStrasse + ", " + user.UserZIP + ", " + user.UserOrt + ", " + HTBUtils.GetCountryName(user.UserZIPPrefix);
                    lblAD.Text = user.UserVorname + " " + user.UserNachname;
                }
            }
        }

        private tblUser GetUser(int userId)
        {
            return (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + userId, typeof(tblUser));
        }
        #region "ICallbackEventHandler"

        public void RaiseCallbackEvent(string argument)
        {
            //            _taskID = argument;
        }

        public string GetCallbackResult()
        {
            RoutePlanerManager rpManager;

            if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.RecalculateRoute]) == GlobalHtmlParams.YES)
                rpManager = ReCalculateRoute();
            else
                rpManager = CalculateRoute();
            
            string routeFilePath = RoutePlanerManager.GetRouteFilePath(rpManager.RouteUser, rpManager.RouteName);
            
            FileSerializer<RoutePlanerManager>.Serialize(routeFilePath, rpManager);

            Session[GlobalHtmlParams.SessionCurrentPosition] = 0;
            Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;
            return "";
        }

        private RoutePlanerManager CalculateRoute()
        {
            var rpManager = new RoutePlanerManager(GlobalUtilArea.GetUserId(Session), (TaskStatus) Context.Cache[_taskId])
                                {
                                    Source = "RoutePlannerTabletApp.aspx",
                                    RouteName = txtRouteName.Text,
                                    RouteUser = _permissions.GrantRoutePlannerForAll ? Convert.ToInt32(ddlAD.SelectedValue) : GlobalUtilArea.GetUserId(Session)
                                };


            ArrayList aktenList = HTBUtils.GetSqlRecords(GetSqlQueryBasedOnInput(), typeof (qryAktenInt));
            LoadAddresses(rpManager, aktenList);
            rpManager.Run();
            return rpManager;
        }

        private RoutePlanerManager ReCalculateRoute()
        {
            var rpManager = (RoutePlanerManager) Session[GlobalHtmlParams.SessionRoutePlannerManager];
            rpManager.Source = "BingRoutePlaner.aspx";
            rpManager.TaskStatus = (TaskStatus) Context.Cache[_taskId];
            rpManager.RouteName = txtRouteName.Text;
            rpManager.RouteUser = _permissions.GrantRoutePlannerForAll ? Convert.ToInt32(ddlAD.SelectedValue) : GlobalUtilArea.GetUserId(Session);
            ReplaceAddresses(rpManager);
            rpManager.Run();
            return rpManager;
        }

        private void LoadAddresses(RoutePlanerManager rpManager, ArrayList akten)
        {
            if (!string.IsNullOrEmpty(_startAddress))
                rpManager.AddAddress(new AddressWithID(-1, _startAddress), false);

            foreach (qryAktenInt akt in akten)
            {
                var address = new StringBuilder(HTBUtils.ReplaceStringAfter(HTBUtils.ReplaceStringAfter(akt.GegnerLastStrasse, " top ", ""), "/", ""));
                address.Append(", ");
                address.Append(akt.GegnerLastZip);
                address.Append(" ");
                address.Append(akt.GegnerLastOrt);
                address.Append(", ");
                address.Append(HTBUtils.GetCountryName(akt.GegnerLastZipPrefix));
                rpManager.AddAddress(new AddressWithID(akt.AktIntID, address.ToString()));
            }
        }

        private void ReplaceAddresses(RoutePlanerManager rpManager)
        {
            var replacementList =  (List<AddressWithID>) Session[GlobalHtmlParams.SessionReplacementAddresses];
            foreach (var addr in replacementList)
            {
                rpManager.ReplaceAddress(addr);
            }

        }

        private String GetSqlQueryBasedOnInput()
        {
            string aktType = GlobalUtilArea.GetEmptyIfNull(ddlAktType.SelectedValue);
            var sb = new StringBuilder("SELECT * FROM qryAktenInt WHERE AktIntStatus = 1 AND (");

            if(GetNumberOfPlzRanges() > 0)
            {
                for (int i = 0; i < GetNumberOfPlzRanges(); ++i)
                {
                    var txtBoxFrom = (TextBox)tblPlz.FindControl(TxtPlzFromPrefix + i);
                    var txtBoxTo = (TextBox)tblPlz.FindControl(TxtPlzToPrefix + i);
                    if (i > 0)
                        sb.Append(" OR  ");
                    sb.Append("(GegnerLastZip between '");
                    sb.Append(GlobalUtilArea.GetEmptyIfNull(txtBoxFrom.Text));
                    sb.Append("' AND '");
                    sb.Append(GlobalUtilArea.GetEmptyIfNull(txtBoxTo.Text));
                    sb.Append("')");
                }
            }
            sb.Append(")");
            if (aktType != GlobalUtilArea.DefaultDropdownValue)
            {
                sb.Append(" AND AktIntAktType = ");
                sb.Append(aktType);
            }
            sb.Append(" AND AktIntSB = ");
            if (_permissions.GrantRoutePlannerForAll)
            {
                sb.Append(ddlAD.SelectedValue);
            }
            else
            {
                sb.Append(GlobalUtilArea.GetUserId(Session));
            }
            return sb.ToString();
        }

        #endregion

        #region Saved Routes GRID
        private IEnumerable<RouteFileRecord> GetSavedRouteRecords()
        {
            string[] fileEntries = Directory.GetFiles(RoutePlanerManager.RouteFolder);
            return (from fileName in fileEntries 
                    where fileName.EndsWith(RoutePlanerManager.RouteExtension) 
                    select GetRouteFileRecord(fileName, Directory.GetCreationTime(fileName)) into rec where rec != null select rec).ToList();
        }
        private RouteFileRecord GetRouteFileRecord(string fileName, DateTime fileDate)
        {
            string[] tokens = fileName.Split('`');
            if(tokens.Length == 3)
            {
                var rec = new RouteFileRecord
                              {
                                  RouteUser = Convert.ToInt32(tokens[1]),
                                  RouteName = tokens[2].Replace("."+RoutePlanerManager.RouteExtension, ""),
                                  RouteDate = fileDate.ToShortDateString()+" "+fileDate.ToShortTimeString()
                              };
                if (_permissions.GrantRoutePlannerForAll || (_permissions.GrantRoutePlannerForSelfOnly && rec.RouteUser == GlobalUtilArea.GetUserId(Session)))
                    return rec;
                
            }
            return null;
        }
        private void PopulateSavedRoutes()
        {
            DataTable dt = GetDataTableStructure();
            foreach (RouteFileRecord rec in GetSavedRouteRecords())
            {
                DataRow dr = dt.NewRow();
                var user = GetUser(rec.RouteUser);
                if (user != null)
                {
                    var sb = new StringBuilder("<a href=\"/v2/intranetx/routeplanter/bingmaps/BingRouteLoader.aspx?");
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.USER, rec.RouteUser.ToString());
                    GlobalUtilArea.AppendHtmlParamIfNotEmpty(sb, GlobalHtmlParams.NAME, rec.RouteName);
                    sb.Append("\">");
                    sb.Append(rec.RouteName);
                    sb.Append("</a>");
                    dr["RouteUser"] = user.UserVorname + " " + user.UserNachname;
                    dr["RouteNameLink"] = sb.ToString();
                    dr["RouteDate"] = rec.RouteDate;
                    dt.Rows.Add(dr);
                }
            }
            gvRouten.DataSource = dt;
            gvRouten.DataBind();
        }

        private DataTable GetDataTableStructure()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("RouteNameLink", typeof (string)));
            dt.Columns.Add(new DataColumn("RouteUser", typeof(string)));
            dt.Columns.Add(new DataColumn("RouteDate", typeof(string)));
            return dt;
        }

        #endregion

        private void DrawPlzTable()
        {
            
            int existingRows = GetNumberOfPlzRanges();
            
            if (existingRows == 0)
                existingRows = 1;
            
            for (int i = 0; i < existingRows; ++i)
            {
                AddRowToPlzTable(i);
            }

            SetNumberOfPlzRanges(existingRows);
        }

        private void AddRowToPlzTable(int index)
        {
            string from;
            string to;

            var row = new HtmlTableRow();
            var cell1 = new HtmlTableCell();
            var cell2 = new HtmlTableCell();
            var cell3 = new HtmlTableCell();
            var cell4 = new HtmlTableCell();

            var txtPlzFrom = new TextBox();
            var txtPlzTo = new TextBox();

            from = GlobalUtilArea.GetEmptyIfNull(Request.Form[TxtPlzFromPrefix + index]);
            to = GlobalUtilArea.GetEmptyIfNull(Request.Form[TxtPlzToPrefix + index]);

            cell1.InnerHtml = index == 0 ? "vom:" : "" + "&nbsp;";
            cell2.Controls.Add(txtPlzFrom);
            cell3.InnerHtml = index == 0 ? "bis:" : "" + "&nbsp;";
            cell4.Controls.Add(txtPlzTo);

            txtPlzFrom.ID = TxtPlzFromPrefix + index;
            txtPlzFrom.CssClass = "docText";
            txtPlzFrom.Attributes.Add("onfocus", "this.style.backgroundColor='#DFF4FF';");
            txtPlzFrom.Attributes.Add("onblur", "this.style.backgroundColor=''");
//            txtPlzFrom.Text = from;

            txtPlzTo.ID = TxtPlzToPrefix + index;
            txtPlzTo.CssClass = "docText";
            txtPlzTo.Attributes.Add("onfocus", "this.style.backgroundColor='#DFF4FF';");
            txtPlzTo.Attributes.Add("onblur", "this.style.backgroundColor=''");
//            txtPlzTo.Text = to;

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);
            row.Cells.Add(cell3);
            row.Cells.Add(cell4);
            if (index == 0)
            {
                var cell5 = new HtmlTableCell();
                var lnk = new LinkButton
                {
                    ID = "lnkAddPlz" + index,
                    Text = "<strong>+</strong>"
                };
                lnk.Click += lnkAddPlz_Clicked;
                cell5.Controls.Add(lnk);
                row.Cells.Add(cell5);
            }

            tblPlz.Rows.Add(row);
        }

        #region setter / getter
        private void SetNumberOfPlzRanges(int val)
        {
            hdnNumberOfPlzRanges.Value = val.ToString();
        }
        private int GetNumberOfPlzRanges()
        {
            try
            {
                return Convert.ToInt32(hdnNumberOfPlzRanges.Value);
            }
            catch
            {
                return 0;
            }
        }
        #endregion
    }
}