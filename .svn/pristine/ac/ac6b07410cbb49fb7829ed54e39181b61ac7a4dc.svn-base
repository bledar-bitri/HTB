using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HTB.Database;
using HTB.v2.intranetx.routeplanter;
using HTB.v2.intranetx.routeplanter.bingmaps;
using HTB.v2.intranetx.util;
using HTBExtras.XML;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class LoginTablet : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string userName = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.USER_NAME]).Replace("'", "''");
            string password = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.PASSWORD]).Replace("'", "''");
            try
            {
                var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserUsername = '" + userName + "' AND UserPasswort = '" + password + "'", typeof(tblUser));

                if(user == null)
                {
                    Response.Write("ERROR: Benutzername oder Passwort falsch");
                }
                else if (user.UserStatus != 1 )
                {
                    Response.Write("ERROR: Benutzer nicht erlaubt");
                }
                else
                {
                    var rec = new XmlLoginRecord(user);
                    foreach (RouteFileRecord route in GetSavedRouteRecords(user.UserID))
                    {
                        rec.Roads.Add(new XmlRoadRecord {Date = route.RouteDate, Name = route.RouteName, RoadUserID = route.RouteUser.ToString()});
                    }

                    Response.Write(rec.ToXmlString());
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }

        private IEnumerable<RouteFileRecord> GetSavedRouteRecords(int userId)
        {
            Log.Info("RouteFolder: "+RoutePlanerManager.RouteFolder);

            string[] fileEntries = Directory.GetFiles(RoutePlanerManager.RouteFolder);
            Log.Info("FileEntries: " + fileEntries.Length);
            return (from fileName in fileEntries
                    where fileName.EndsWith(RoutePlanerManager.RouteExtension)
                    select GetRouteFileRecord(fileName, Directory.GetCreationTime(fileName), userId) into rec
                    where rec != null
                    select rec).ToList();
        }
        private RouteFileRecord GetRouteFileRecord(string fileName, DateTime fileDate, int userId)
        {
            string[] tokens = fileName.Split('`');
            if (tokens.Length == 3)
            {
                var rec = new RouteFileRecord
                {
                    RouteUser = Convert.ToInt32(tokens[1]),
                    RouteName = tokens[2].Replace("." + RoutePlanerManager.RouteExtension, ""),
                    RouteDate = fileDate.ToShortDateString() + " " + fileDate.ToShortTimeString()
                };
                if (rec.RouteUser == userId)
                {
                    Log.Info("checkign file: " + fileName + " " + fileDate + " " + userId);
                    try
                    {
                        /* read the road to make sure the file is not corrupted */
                        FileSerializer<RoutePlanerManager>.DeSerialize(RoutePlanerManager.GetRouteFilePath(rec.RouteUser, rec.RouteName));
                        Log.Info("File OK");
                        return rec;
                    }
                    catch
                    {
                        Log.Info("Corrupted file");
                    }
                }
            }
            return null;
        }
    }
}