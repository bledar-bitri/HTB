using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.permissions;
using HTB.v2.intranetx.progress;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.routeplanter.bingmaps
{
    public partial class BingRoutePlanerAutomatic : Page, ICallbackEventHandler
    {
        private static string _startAddress = "";
        private string _taskID;
        private readonly PermissionsRoutePlanner _permissions = new PermissionsRoutePlanner();

        protected void Page_Load(object sender, EventArgs e)
        {
//            Session["MM_UserID"] = "99";
            Server.ScriptTimeout = 6200; // two hours
            ctlMessage.Clear();
            _permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            if (_permissions.GrantRoutePlannerForAll || _permissions.GrantRoutePlannerForSelfOnly)
            {
                _taskID = Context.User.Identity.Name + ":BingRoutePlannerAutomatic";

                if (!IsPostBack)
                {
                    SetUserInfo();
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
                }
                if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.BingRouteRecalculate]) == GlobalHtmlParams.YES)
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
                trButtons.Visible = false;
                ctlMessage.ShowError("Sie dürfen nicht den Routenplaner benützen!");
            }
        }

        #region Event Handlers

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                int foundCount = HTBUtils.GetSqlRecords(GetSqlQueryBasedOnInput(true), typeof (qryAktenInt)).Count;
                if (foundCount == 0)
                {
                    ctlMessage.ShowError("Keine Akte gefunden!");
                }
                else
                {
                    _startAddress = txtStartAddress.Text;

                    var status = new TaskStatus();
                    Context.Cache[_taskID] = status;

                    string script1 = ScriptHelpers.GetStarterScript(this, "StartTask", "btnSubmit");
                    ClientScript.RegisterClientScriptBlock(GetType(), "StartTask", script1, true);
                    bdy.Attributes["onload"] = "javascript:StartTask()";
                }
            }
        }

        protected void btnSubmitAnt_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                int foundCount = HTBUtils.GetSqlRecords(GetSqlQueryBasedOnInput(true), typeof (qryAktenInt)).Count;
                if (foundCount == 0)
                {
                    ctlMessage.ShowError("Keine Akte gefunden!");
                }
                else
                {
                    RoutePlanerManagerAutomatic rpManager = CalculateRouteAnt();
                    Session[GlobalHtmlParams.SessionCurrentPosition] = 0;
                    Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;
                    Response.Redirect("BingRouteDisplayTablet.aspx");
                }
            }
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

        private int GetRouteUserId()
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
            return userId;
        }

        private void SetUserInfo()
        {

            int userId = GetRouteUserId();
            
            if (userId > 0)
            {
                var user = GetUser(userId);
                if (user != null)
                {
                    txtStartAddress.Text = user.UserStrasse + ", " + user.UserZIP + ", " + user.UserOrt + ", "+ HTBUtils.GetCountryName(user.UserZIPPrefix);
                    lblAD.Text = user.UserVorname + " " + user.UserNachname;
                }
            }
        }

        private tblUser GetUser(int userId)
        {
            return (tblUser) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + userId, typeof (tblUser));
        }

        #region "ICallbackEventHandler"

        public void RaiseCallbackEvent(string argument)
        {
        }

        public string GetCallbackResult()
        {
            RoutePlanerManagerAutomatic rpManager;

            if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.BingRouteRecalculate]) == GlobalHtmlParams.YES)
                rpManager = ReCalculateRoute();
            else
                rpManager = CalculateRoute();

//            string routeFilePath = RoutePlannerManager.GetRouteFilePath(rpManager.RouteUser, rpManager.RouteName);

//            FileSerializer<RoutePlannerManager>.Serialize(routeFilePath, rpManager);

            Session[GlobalHtmlParams.SessionCurrentPosition] = 0;
            Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;
            return "";
        }

        private RoutePlanerManagerAutomatic CalculateRoute()
        {
            var rpManager = new RoutePlanerManagerAutomatic(GlobalUtilArea.GetUserId(Session), (TaskStatus)Context.Cache[_taskID])
                                {
                                    Source = "BingRoutePlanerAutomatic.aspx",
                                    RouteName = txtRouteName.Text,
                                    RouteUser = _permissions.GrantRoutePlannerForAll ? Convert.ToInt32(ddlAD.SelectedValue) : GlobalUtilArea.GetUserId(Session)
                                };

            DateTime firstAppt = GlobalUtilArea.GetTodayAtTime(txtFirstAppointment.Text);
            DateTime backTime = GlobalUtilArea.GetTodayAtTime(txtBackHome.Text);
            long tripDuration = GetTripDuration(firstAppt, backTime);

            ArrayList aktenList = HTBUtils.GetSqlRecords(GetSqlQueryBasedOnInput(), typeof (qryAktenInt));
            LoadAddresses(rpManager, aktenList);
            rpManager.RunAutomatic(GetRouteUserId(), firstAppt, tripDuration);
            return rpManager;
        }

        private RoutePlanerManagerAutomatic ReCalculateRoute()
        {
            var rpManager = (RoutePlanerManagerAutomatic)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            rpManager.Source = "BingRoutePlanerAutomatic.aspx";
            rpManager.TskStatus = (TaskStatus) Context.Cache[_taskID];
            rpManager.RouteName = txtRouteName.Text;
            rpManager.RouteUser = _permissions.GrantRoutePlannerForAll ? Convert.ToInt32(ddlAD.SelectedValue) : GlobalUtilArea.GetUserId(Session);
            ReplaceAddresses(rpManager);
            rpManager.Run();
            return rpManager;
        }

        private void LoadAddresses(RoutePlanerManagerAutomatic rpManager, ArrayList akten)
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

        private void ReplaceAddresses(RoutePlanerManagerAutomatic rpManager)
        {
            var replacementList = (List<AddressWithID>) Session[GlobalHtmlParams.SessionReplacementAddresses];
            foreach (var addr in replacementList)
            {
                rpManager.ReplaceAddress(addr);
            }
        }

        private String GetSqlQueryBasedOnInput(bool addTop5Clause = false)
        {
            var sb = new StringBuilder("SELECT " + (addTop5Clause ? "TOP 5" : "") + " * FROM qryAktenInt WHERE AktIntStatus = 1 AND AktIntSB = ");
            sb.Append(GetRouteUserId());
            return sb.ToString();
        }

        private long GetTripDuration(DateTime startTime, DateTime endTime)
        {
            TimeSpan ts = endTime.Subtract(startTime);
            return (long)ts.TotalSeconds;
        }

        private RoutePlanerManagerAutomatic CalculateRouteAnt()
        {
            var rpManager = new RoutePlanerManagerAutomatic(GlobalUtilArea.GetUserId(Session), (TaskStatus)Context.Cache[_taskID])
            {
                Source = "BingRoutePlanerAutomatic.aspx",
                RouteName = txtRouteName.Text,
                RouteUser = _permissions.GrantRoutePlannerForAll ? Convert.ToInt32(ddlAD.SelectedValue) : GlobalUtilArea.GetUserId(Session)
            };

            DateTime firstAppt = GlobalUtilArea.GetTodayAtTime(txtFirstAppointment.Text);
            DateTime backTime = GlobalUtilArea.GetTodayAtTime(txtBackHome.Text);
            long tripDuration = GetTripDuration(firstAppt, backTime);

            ArrayList aktenList = HTBUtils.GetSqlRecords(GetSqlQueryBasedOnInput(), typeof(qryAktenInt));
            LoadAddresses(rpManager, aktenList);
            rpManager.Run(GetRouteUserId());
            return rpManager;
        }

        #endregion
    }
}