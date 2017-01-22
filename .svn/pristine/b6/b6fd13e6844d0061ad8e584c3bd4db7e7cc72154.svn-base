using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HTB.Database;
using HTB.v2.intranetx.progress;
using HTBUtilities;
using System.Text;
using HTB.v2.intranetx.util;
using System.Collections;
using HTB.Database.Views;
using System.Web.UI;

namespace HTB.v2.intranetx.routeplanter.bingmaps
{
    public partial class BingRoutePlanerForAkten : Page, ICallbackEventHandler
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string StartAddress = "";
        private string _taskID;

        protected void Page_Load(object sender, EventArgs e)
        {
            _taskID = Context.User.Identity.Name + ":BingRoutePlanerForAkten";
            Server.ScriptTimeout = 3600;
            if (!IsPostBack)
            {
                SetStartingAddress();
                if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.BingRouteRecalculate]) != GlobalHtmlParams.YES)
                {
                    var aktIdList = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ID]).Split(',');
                    if (aktIdList.Length < 4)
                    {
                        ctlMessage.ShowError("<strong><font size=\"+1\">Sie müssen mindestens 4 Akten wehlen!<br/>&nbsp;<br/>Schlisen Sie diesen Fenster und wehlen Sie genuk Akte.</font></strong>");
                        trHeader.Visible = false;
                        trAddress.Visible = false;
                        btnSubmit.Visible = false;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            StartAddress = txtStartAddress.Text;

            var status = new TaskStatus();
            Context.Cache[_taskID] = status;

            string script1 = ScriptHelpers.GetStarterScript(this, "StartTask", "btnSubmit");
            ClientScript.RegisterClientScriptBlock(GetType(), "StartTask", script1, true);
            bdy.Attributes["onload"] = "javascript:StartTask()";
        }

        private void SetStartingAddress()
        {
            int userId = GlobalUtilArea.GetUserId(Session);
            if (userId > 0)
            {
                var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + userId, typeof(tblUser));
                if (user != null)
                {
                    txtStartAddress.Text = user.UserStrasse + ", " + user.UserZIP + ", " + user.UserOrt + ", "+HTBUtils.GetCountryName(user.UserZIPPrefix);
                }
            }
        }

        private void ReplaceAddresses(RoutePlanerManager rpManager)
        {
            List<AddressWithID> replacementList = null;
            replacementList = (List<AddressWithID>)Session[GlobalHtmlParams.SessionReplacementAddresses];
            foreach (var addr in replacementList)
            {
                rpManager.ReplaceAddress(addr);
            }

        }
        private void LoadAddresses(RoutePlanerManager rpManager, ArrayList akten)
        {
            if (!string.IsNullOrEmpty(StartAddress))
                rpManager.AddAddress(new AddressWithID(-1, StartAddress), false);

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
            //            var endAddr = new AddressWithID(-1, "") { Address = string.IsNullOrEmpty(txtEnd.Text) ? StartAndEndAddress : txtEnd.Text };
            //            rpManager.AddAddress(endAddr, false);
        }

        public AddressWithID GetReplacementAddress(List<AddressWithID> list, int id)
        {
            if (list != null)
                return list.FirstOrDefault(address => address.ID == id);

            return null;
        }

        #region "ICallbackEventHandler"
        public void RaiseCallbackEvent(string argument)
        {
            //            _taskID = argument;
        }

        public string GetCallbackResult()
        {
            RoutePlanerManager rpManager;

            if (GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.BingRouteRecalculate]) == GlobalHtmlParams.YES)
                rpManager = ReCalculateRoute();
            else
                rpManager = CalculateRoute();
//                rpManager = CalculateRouteTest();

            Session[GlobalHtmlParams.SessionCurrentPosition] = 0;
            Session[GlobalHtmlParams.SessionRoutePlannerManager] = rpManager;

            return "";
        }

        #endregion

        #region Calculate Route
        private RoutePlanerManager CalculateRoute()
        {
            var rpManager = new RoutePlanerManager(GlobalUtilArea.GetUserId(Session), (TaskStatus)Context.Cache[_taskID]);
            rpManager.Source = "BingRoutePlanerForAkten.aspx";
            string aktIdList = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ID]);
//            aktIdList = "146241, 146240, 146239, 146237, 146234, 146232, 146226, 146225, 146219, 146218, 146217, 146212, 146203, 146202, 146201, 146200, 146199, 146196, 146192, 146183, 140690, 140689, 140688, 140687, 140684, 140676";
//            aktIdList = "146241, 146240, 146239, 146237, 146234, 146232, 146226, 146225";
            
            if (aktIdList.Trim().Length > 0)
            {
                string qryAktCommand = "SELECT * FROM dbo.qryAktenInt WHERE AktIntID in (" + aktIdList + ")";
                ArrayList aktenList = HTBUtils.GetSqlRecords(qryAktCommand, typeof(qryAktenInt));
                LoadAddresses(rpManager, aktenList);

                rpManager.Run();
            }
            return rpManager;
        }

        private RoutePlanerManager CalculateRouteTest()
        {
            var rpManager = new RoutePlanerManager(GlobalUtilArea.GetUserId(Session), (TaskStatus)Context.Cache[_taskID]);
            rpManager.Source = "BingRoutePlanerForAkten.aspx";
            LoadTestAddresses(rpManager, true);
            rpManager.Run();
            return rpManager;
        }

        private RoutePlanerManager ReCalculateRoute()
        {
            var rpManager = (RoutePlanerManager)Session[GlobalHtmlParams.SessionRoutePlannerManager];
            rpManager.Source = "BingRoutePlanerForAkten.aspx";
            rpManager.TskStatus = (TaskStatus)Context.Cache[_taskID];
            ReplaceAddresses(rpManager);
            rpManager.Run();
            return rpManager;
        }

        private void LoadTestAddresses(RoutePlanerManager rpManager, bool LoadBadAddresses = false)
        {
            if (!string.IsNullOrEmpty(StartAddress))
                rpManager.AddAddress(new AddressWithID(-1, StartAddress), false);

            rpManager.AddAddress(new AddressWithID(154364, "Danreitergasse 4, 5020 Salzburg, Austria"));
            rpManager.AddAddress(new AddressWithID(165613, "Kellerstraße 18, 5082 Grödig, Austria"));
            rpManager.AddAddress(new AddressWithID(166161, "Lofer 237, 5090 Lofer, Austria"));
            rpManager.AddAddress(new AddressWithID(166951, "Salzburgerstraße 285, 5084 Großgmain, Austria"));
            rpManager.AddAddress(new AddressWithID(168614, "Hellbrunnerstraße 17, 5081 Anif, Austria"));
            rpManager.AddAddress(new AddressWithID(170706, "Ahornstraße 11 , 5081 Anif, Austria"));
            rpManager.AddAddress(new AddressWithID(171131, "Geyereckstraße 1, 5082 Gröding, Austria"));
            rpManager.AddAddress(new AddressWithID(171812, "Lofer 25, 5090 Lofer, Austria"));
            rpManager.AddAddress(new AddressWithID(177087, "Kellerstr. 18c, 5082 Grödig, Austria"));
            rpManager.AddAddress(new AddressWithID(179524, "Karl-Adrian-Straße 11, 5020 Salzburg, Austria"));
            rpManager.AddAddress(new AddressWithID(177863, "Kellerstraße 7, 5082 Gröding, Austria"));
            rpManager.AddAddress(new AddressWithID(179530, "Gorianstr. 34, 5020 Salzburg, Austria"));
            rpManager.AddAddress(new AddressWithID(177848, "Vorderfager 20, 5061 Elsbethen, Austria"));
            rpManager.AddAddress(new AddressWithID(179578, "Kellerstraße 18b, 5082 Grödig, Austria"));
            rpManager.AddAddress(new AddressWithID(179842, "Brunnengasse 4, 5020 Salzburg, Austria"));
            rpManager.AddAddress(new AddressWithID(179899, "Griesgasse 29, 5020 Salzburg, Austria"));
            rpManager.AddAddress(new AddressWithID(179621, "Glockmühlstr. 6a, 5020 Salzburg, Austria"));
            rpManager.AddAddress(new AddressWithID(179893, "Residenzplatz 3, 5020 Salzburg, Austria"));
            if (LoadBadAddresses)
            {
                rpManager.AddAddress(new AddressWithID(168423, "Alte Schulgasse 48, 8950 Stainach, Austria"));
                rpManager.AddAddress(new AddressWithID(170529, "Nr.309, 5090 Lofer, Austria"));
                rpManager.AddAddress(new AddressWithID(175434, "Lagerstraße 3, 5071 Wals - Siezenheim, Austria"));
                rpManager.AddAddress(new AddressWithID(179927, "Sanatoriumstr. 287a, 5020 Salzburg, Austria"));
                rpManager.AddAddress(new AddressWithID(179624, "Schmittensteinstraße 26, 5071 Wals-Siezenheim , Austria"));
                rpManager.AddAddress(new AddressWithID(179528, "Oberwinkl 16, 5026 Salzburg-Aigen, Austria"));

            }

        }
        #endregion
    }
}


