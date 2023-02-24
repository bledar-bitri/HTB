using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using HTB.Database;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using HTB.Database.Views;
using HTB.GeocodeService;
using HTB.v2.intranetx.routeplanter;
using HTBAktLayer;
using HTBExtras;
using HTBUtilities;
using Location = HTB.GeocodeService.Location;

namespace HTB.v2.intranetx.util
{
    public static class GlobalUtilArea
    {
        public const string LoginExpiredLink = "~/v2/intranet/login/expired.asp";
        public const string DefaultDropdownValue = "-1";
        private const double Epsilon = 0.001;

        private static readonly bool PerformSessionValidation = StringToBool(HTBUtils.GetConfigValue("PERFORM_SESSION_VALIDATION"));
        private static Dictionary<string, bool> userFunctionPermissionCache = new Dictionary<string, bool>();

        public static bool Rolecheck(int functionID, int userID)
        {
            return Rolecheck(Convert.ToString(functionID), userID);
        }

        private static bool Rolecheck(string functionID, int userID)
        {
            if (userID >= 0)
            {
                if (userFunctionPermissionCache.ContainsKey(userID + functionID))
                {
                    return userFunctionPermissionCache[userID + functionID];
                }
                else
                {
                    string query = "SELECT * FROM qryRoleCheck  WITH (NOLOCK) WHERE UserRoleUser = " + userID + " AND RoleDefinitionRoleFunction = " + functionID.Replace("'", "''");
                    var roleDef = (tblRoleDefinition)HTBUtils.GetSqlSingleRecord(query, typeof(tblRoleDefinition));
                    bool userPermissionGranted = roleDef != null && roleDef.RoleDefinitionGranted == 1;
                    userFunctionPermissionCache[userID + functionID] = userPermissionGranted;
                    return userPermissionGranted;
                }
            }
            return false;
        }

        public static int GetUserId(HttpSessionState session)
        {
            int wuserId;
            try
            {
                if (session["MM_UserID"] == null)
                    wuserId = -1;
                else
                    wuserId = Convert.ToInt32(session["MM_UserID"]);
            }
            catch
            {
                wuserId = -1;
            }
            if (wuserId == -1 && !PerformSessionValidation)
                wuserId = 0;
            return wuserId;
        }

        public static string GetFileName(string name)
        {
            if (name.IndexOf(".") < 0)
            {
                return name;
            }
            return name.Substring(name.LastIndexOf("."));
        }

        public static double GetZeroIfConvertToDoubleError(TextBox txtBox)
        {
            return GetZeroIfConvertToDoubleError(txtBox.Text);
        }

        public static double GetZeroIfConvertToDoubleError(string value)
        {
            double rett;
            try
            {
                rett = Convert.ToDouble(value);
            }
            catch
            {
                rett = 0;
            }
            return rett;
        }

        public static int GetZeroIfConvertToIntError(TextBox txtBox)
        {
            return GetZeroIfConvertToIntError(txtBox.Text);
        }

        public static int GetZeroIfConvertToIntError(string value)
        {
            int rett;
            try
            {
                rett = Convert.ToInt32(value);
            }
            catch
            {
                rett = 0;
            }
            return rett;
        }

        public static int GetZeroIfConvertToIntError(object value)
        {
            int rett;
            try
            {
                rett = Convert.ToInt32(value.ToString());
            }
            catch
            {
                rett = 0;
            }
            return rett;
        }

        public static decimal GetZeroIfConvertToDecimalError(TextBox txtBox)
        {
            return GetZeroIfConvertToDecimalError(txtBox.Text);
        }

        public static decimal GetZeroIfConvertToDecimalError(string value)
        {
            decimal rett;
            try
            {
                rett = Convert.ToDecimal(value);
            }
            catch
            {
                rett = 0;
            }
            return rett;
        }

        public static double GetNegOneIfConvertToDoubleError(TextBox txtBox)
        {
            return GetNegOneIfConvertToDoubleError(txtBox.Text);
        }

        public static double GetNegOneIfConvertToDoubleError(string value)
        {
            double rett;
            try
            {
                rett = Convert.ToDouble(value);
            }
            catch
            {
                rett = -1;
            }
            return rett;
        }

        public static string GetEmptyIfNull(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return value.ToString();
        }

        public static DateTime GetDefaultDateIfConvertToDateError(TextBox txtBox)
        {
            return GetDefaultDateIfConvertToDateError(txtBox.Text);
        }

        public static DateTime GetDefaultDateIfConvertToDateError(string value)
        {
            DateTime rett;
            try
            {
                rett = Convert.ToDateTime(value);
            }
            catch
            {
                rett = new DateTime(1900, 1, 1);
            }
            return rett;
        }

        public static DateTime GetNowIfConvertToDateError(TextBox txtBox)
        {
            return GetNowIfConvertToDateError(txtBox.Text);
        }

        public static DateTime GetNowIfConvertToDateError(string value)
        {
            DateTime rett;
            try
            {
                rett = Convert.ToDateTime(value);
            }
            catch
            {
                rett = DateTime.Now;
            }
            return rett;
        }

        public static DateTime GetTodayAtTime(string time)
        {
            return GetDateAtTime(DateTime.Now, time);
        }

        public static DateTime GetDateAtTime(DateTime dte, string time)
        {
            string[] str = time.Split(':');
            if (str.Length == 2)
            {
                try
                {
                    return new DateTime(dte.Year, dte.Month, dte.Day, Convert.ToInt32(str[0]), Convert.ToInt32(str[1]), 0);
                }
                catch
                {
                }
            }
            return HTBUtils.DefaultDate;
        }

        public static double GetDoubleAmountFromParameter(HttpRequest req, string parameterName)
        {
            try
            {
                string str = req.Params[parameterName];
                if(str.IndexOf(".") > 0 && str.IndexOf(",") > 0)
                {
                    return GetZeroIfConvertToDoubleError(str);
                }
                if(str.IndexOf(".") > 0)
                {
                    str = str.Replace(".", ",");
                }
//                str = str.Replace(".", ";");
//                str = str.Replace(",", ".");
//                str = str.Replace(";", "");
                return GetZeroIfConvertToDoubleError(str);
                //return GetZeroIfConvertToDoubleError(req.Params[parameterName]);
            }
            catch
            {
            }
            return 0;
        }
        public static void SetSelectedValue(DropDownList ddl, string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Equals("0"))
                try
                {
                    ddl.SelectedIndex = 0; // put it in a try in case the dropdown list is empty
                }
                catch
                {
                }
            else
            {
                try
                {
                    ddl.SelectedValue = str;
                }
                catch
                {
                }
            }
        }
        public static string BoolToString(bool bol)
        {
            return bol ? "Y" : "N";
        }

        public static bool StringToBool(string str)
        {
            return str != null && (str.Trim().ToUpper().Equals("Y") || str.Trim().ToUpper().Equals("TRUE"));
        }

        public static string GetHtmlSpaceIfEmptyOrNull(string str)
        {
            if (str == null || str.Trim() == string.Empty)
                return "&nbsp;";
            return str;
        }
        public static string GetCurrencyValueForNonZeroAmount(double amount)
        {
            if(!HTBUtils.IsZero(amount))
            {
                return HTBUtils.FormatCurrencyNumber(amount);
            }
            return "";
        }
        public static string GetStringValueForNonZeroAmount(int amount)
        {
            if (amount != 0)
            {
                return amount.ToString();
            }
            return "";
        }
        public static string GetStringValueForNonDefaultDate(DateTime dte)
        {
            if (dte.ToShortDateString() != HTBUtils.DefaultDate.ToShortDateString())
            {
                return dte.ToShortDateString();
            }
            return "";
        }
        public static bool IsTablet()
        {
            try
            {
                return HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad");
            }
            catch
            {
                return false;
            }
        }

        public static string GetInterventionAktStatusText(int aktStatus)
        {
            switch (aktStatus)
            {
                case 0:
                    return "0 - Neu Erfasst";
                case 1:
                    return "1 - In Bearbeitung";
                case 2:
                    return "2 - Abgegeben";
                case 3:
                    return "3 - Fertig";
                case 4:
                    return "4 - Abgeschlossen";
                case 5:
                    return "5 - Abgeschlossene Altakte";
                case 8:
                    return "8 - Unter Schwellwert";
                case 9:
                    return "9 - Wartet auf Bonitätsprüfung";
                case 10:
                    return "10 - Storno aufgrund Bonität";
                case 11:
                    return "11 - Sofortklage";
                default:
                    return "unbekannt";
            }
        }

        public static string GetXmlData(HttpRequest request)
        {
            HttpFileCollection uploadFiles = request.Files;
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile postedFile = uploadFiles[i];

                // Access the uploaded file's content in-memory:
                Stream inStream = postedFile.InputStream;
                var fileData = new byte[postedFile.ContentLength];
                inStream.Read(fileData, 0, postedFile.ContentLength);

                if (postedFile.FileName.ToLower().EndsWith(".xml"))
                {
                    return Encoding.UTF8.GetString(fileData);
                }
            }
            return null;
        }

        public static string GetDayOfWeekName(DateTime dte)
        {
            switch (dte.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Montag";
                case DayOfWeek.Tuesday:
                    return "Dienstag";
                case DayOfWeek.Wednesday:
                    return "Mitwoch";
                case DayOfWeek.Thursday:
                    return "Donerstag";
                case DayOfWeek.Friday:
                    return "Freitag";
                case DayOfWeek.Saturday:
                    return "Samstag";
                case DayOfWeek.Sunday:
                    return "Sontag";
            }
            return "";
        }

        public static string GetSqlInClause(IEnumerable<string> list, bool isVarchar = true)
        {
            var sb = new StringBuilder("(  ");
            foreach (string s in list)
            {
                if (isVarchar)
                    sb.Append("'");
                sb.Append(s);
                if (isVarchar)
                    sb.Append("'");
                sb.Append(", ");
                
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            return sb.ToString();
        }

        public static double GetAktBalance(tblAktenInt akt)
        {
            double balance = 0;
            if (akt != null)
            {
                if (akt.IsInkasso())
                {
                    var aktUtils = new AktUtils(akt.AktIntCustInkAktID);
                    balance = aktUtils.GetAktBalance();
                }
                else
                {
                    ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WITH (NOLOCK) WHERE AktIntPosAkt = " + akt.AktIntID, typeof(tblAktenIntPos));
                    balance += posList.Cast<tblAktenIntPos>().Sum(aktIntPos => aktIntPos.AktIntPosBetrag);
                    balance += akt.AKTIntZinsenBetrag;
                    balance += akt.AKTIntKosten;
                    balance += akt.AktIntWeggebuehr;
                }
            }
            return balance;
        }

        public static string GetDocAttachmentLink(string attachment)
        {
            var sb = new StringBuilder();
            if (attachment.ToLower().StartsWith("http://"))
            {
                sb.Append("<a href=\"javascript:void(window.open('");
                sb.Append(attachment);
                sb.Append("','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10'))\">");
                sb.Append(attachment.Substring(attachment.LastIndexOf("/") + 1));
                sb.Append("</a>");
                return sb.ToString();
            }
            sb.Append("<a href=\"javascript:void(window.open('../../intranet/documents/files/");
            sb.Append(attachment);
            sb.Append("','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10'))\">");
            sb.Append(attachment);
            sb.Append("</a>");
            return sb.ToString();
        }
        /*
        public static ArrayList GetCombineActions(ArrayList inkassoAktions, ArrayList interventionAktions, ArrayList meldeResults)
        {

            var aktionsList = new ArrayList();
            
            if(inkassoAktions != null)
                foreach (qryCustInkAktAktionen inkAction in inkassoAktions)
                    aktionsList.Add(new InkassoActionRecord(inkAction, intAkt));

            if(interventionAktions != null)
                foreach (qryInktAktAction intAction in interventionAktions)
                {
                    aktionsList.Add(new InkassoActionRecord(intAction, intAkt));
                }
            if(meldeResults != null)
                foreach (qryMeldeResult melde in meldeResults)
                    aktionsList.Add(new InkassoActionRecord(melde, intAkt));

            aktionsList.Sort(new InkassoActionRecordComparer());

            var list = new ArrayList();
            foreach (InkassoActionRecord rec in aktionsList)
                list.Add(new Aktion(rec));

            return list;
        }
         */ 

        #region Login
        public static bool Login(string userName, string password, HttpSessionState session, HttpResponse response)
        {
            
            string wrongPage = "/v2/intranet/login/wrong.asp";
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                response.Redirect(wrongPage);
                return false;
            }
            var user = (qryUsers)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryUsers WITH (NOLOCK) WHERE UserUsername = '" + userName + "' AND UserPasswort='" + password + "'", typeof(qryUsers));
            if (user == null)
            {
                response.Redirect(wrongPage);
                return false;
            }
            if (user.UserStatus == 0)
            {
                response.Redirect(wrongPage);
                return false;
            }
            session["MM_Username"] = user.UserUsername;
            session["MM_UserAuthorization"] = user.UserLevel;
            session["MM_Vorname"] = user.UserVorname;
            session["MM_Nachname"] = user.UserNachname;
            session["MM_Levelname"] = user.LevelCaption;
            session["MM_DataRows"] = user.UserRowCount;
            session["MM_UserID"] = user.UserID;
            session["MM_UserEMail"] = user.UserEMailOffice;
            session["MM_Klient"] = user.UserKlient;
            session["MM_AG"] = user.UserAG;
            session["MM_RoleALBonitaet"] = user.UserRoleALBonitaet;
            session["MM_RoleIntAkt"] = user.UserRoleIntAkt;
            session["MM_Picture"] = user.UserPic;
            session["MM_Filter"] = "0";
            session["MM_Sort"] = "0";
            session["MM_Action"] = "0";
            session["MM_DestPage"] = "../intranet/intranet.asp";
            session["MM_UserLevelLevel"] = user.LevelLevel;
            return true;
        }
        #endregion
        
        #region Load Dropdown Lists
        /*
         * this method is slow but cuts down the development time substantially
         */

        public static void LoadDropdownList(DropDownList ddl, string query, Type clsName, string captionField, bool addBitteAuswahlen)
        {
            LoadDropdownList(ddl, query, clsName, captionField, captionField, addBitteAuswahlen);
        }

        public static void LoadDropdownList(DropDownList ddl, string query, Type clsName, string idField, string captionField, bool addBitteAuswahlen)
        {
            LoadDropdownList(ddl, HTBUtils.GetSqlRecords(query, clsName), idField, captionField, addBitteAuswahlen);
        }

        public static void LoadDropdownList(DropDownList ddl, ArrayList list, string idField, string captionField, bool addBitteAuswahlen)
        {
            ddl.Items.Clear();
            if (addBitteAuswahlen)
            {
                ddl.Items.Add(new ListItem("*** bitte auswählen ***", DefaultDropdownValue));
            }
            if (list.Count > 0)
            {
                string wid = "";
                string wcaption = "";

                foreach (object wrecord in list)
                {
                    var wisIdset = false;
                    var wisCaptionSet = false;
                    foreach (var info in wrecord.GetType().GetProperties())
                    {
                        if (info.Name.ToLower().Equals(idField.ToLower()))
                        {
                            wid = info.GetValue(wrecord, null).ToString();
                            wisIdset = true;
                        }
                        if (info.Name.ToLower().Equals(captionField.ToLower()))
                        {
                            wcaption = info.GetValue(wrecord, null).ToString();
                            wisCaptionSet = true;
                        }
                        if (wisIdset && wisCaptionSet)
                            break;
                    }
                    ddl.Items.Add(new ListItem(wcaption, wid));
                }
            }
        }

        public static void LoadUserDropdownList(DropDownList ddl, string query, bool addBitteAuswahlen = false)
        {
            LoadUserDropdownList(ddl, HTBUtils.GetSqlRecords(query, typeof (tblUser)));
        }

        public static void LoadUserDropdownList(DropDownList ddl, ArrayList usersList, bool addBitteAuswahlen = false)
        {
            ddl.Items.Clear();
            if (addBitteAuswahlen)
            {
                ddl.Items.Add(new ListItem("*** bitte auswählen ***", DefaultDropdownValue));
            }
            foreach (tblUser user in usersList)
            {
                var item = new ListItem(user.UserNachname + ", " + user.UserVorname, user.UserID.ToString());
                if (user.UserStatus == 0)
                {
                    item.Attributes.Add("class", "dis");
                }
                ddl.Items.Add(item);

            }
        }
        public static List<VisitRecord> GetVisitedDates(int intAktId)
        {
            var actions = HTBUtils.GetSqlRecords(
                $"SELECT * FROM qryAktenIntActionWithType WITH (NOLOCK) WHERE AktIntActionAkt = {intAktId} AND AktIntActionIsExtensionRequest <> 1 ORDER BY AktIntActionTime", typeof(qryAktenIntActionWithType));

            return (from qryAktenIntActionWithType action in actions
                select new VisitRecord
                {
                    VisitTime = action.AktIntActionTime,
                    VisitMemo = action.AktIntActionMemo,
                    VisitAction = action.AktIntActionTypeCaption,
                    VisitPerson = action.UserVorname + " " + action.UserNachname
                }).ToList();
        }
        public static List<VisitRecord> GetVisitedDates_OLD(int intAktId)
        {
            var list = new List<VisitRecord>();

            ArrayList actions = HTBUtils.GetSqlRecords("SELECT * FROM qryAktenIntActionWithType WITH (NOLOCK) WHERE AktIntActionAkt = " + intAktId + " AND AktIntActionIsExtensionRequest <> 1 AND AktIntActionIsThroughPhone <> 1  AND AktIntActionIsInternal = 0 ORDER BY AktIntActionTime", typeof(qryAktenIntActionWithType));
            foreach (qryAktenIntActionWithType action in actions)
            {
                bool ok = true;
                foreach (var visit in list)
                {
                    TimeSpan ts = visit.VisitTime.Subtract(action.AktIntActionTime);
                    if (Math.Abs(ts.TotalHours) < 1)
                    {
                        if (!string.IsNullOrEmpty(action.AktIntActionMemo))
                        {
                            if (!visit.VisitMemo.ToLower().Contains(action.AktIntActionMemo.Trim().ToLower()))
                            {
                                visit.VisitMemo = visit.VisitMemo.Trim() + " " + action.AktIntActionMemo.Trim();
                            }
                        }
                        ok = false;
                    }
                }
                if (ok)
                {
                    list.Add(new VisitRecord
                    {
                        VisitTime = action.AktIntActionTime,
                        VisitMemo = action.AktIntActionMemo,
                        VisitAction = action.AktIntActionTypeCaption,
                        VisitPerson = action.UserVorname + " " + action.UserNachname
                    });
                }
            }
            return list;
        }
        public static List<tblAktenIntPos> GetPosList(int intAktId)
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WITH (NOLOCK) WHERE AktIntPosAkt = " + intAktId + " ORDER BY AktIntPosTransferredDate ", typeof(tblAktenIntPos)).Cast<tblAktenIntPos>().ToList();
        }
        public static bool InsertPosRecord(int aktid, string posCaption, double posAmount, int posType, DateTime posTransferredDate)
        {
            return InsertPosRecord(aktid, "Zahlung", posCaption, posAmount, posType, posTransferredDate);
        }
        public static bool InsertPosRecord(int aktid, string posNr, string posCaption, double posAmount, int posType, DateTime posTransferredDate)
        {
            if (!HTBUtils.IsZero(posAmount))
            {
                return RecordSet.Insert(new tblAktenIntPos
                {
                    AktIntPosAkt = aktid,
                    AktIntPosBetrag = posAmount,
                    AktIntPosCaption = posCaption,
                    AktIntPosDatum = DateTime.Now,
                    AktIntPosDueDate = DateTime.Now,
                    AktIntPosNr = posNr,
                    AktIntPosTypeCode = posType,
                    AktIntPosTransferredDate = posTransferredDate
                });
            }
            return true;
        }
        #endregion

        public static ArrayList GetUsers(HttpSessionState session)
        {
            object obj = session["MM_Users"];
            if (obj != null && obj is ArrayList)
            {
                return (ArrayList) obj;
            }
            var usersList = HTBUtils.GetSqlRecords("SELECT * FROM tblUser WITH (NOLOCK) WHERE UserAbteilung = 1 ORDER BY UserStatus DESC, UserNachname ASC", typeof(tblUser));
            session["MM_Users"] = usersList;
            return usersList;
        }

        #region Permissions

        public static void ValidateSession(HttpSessionState session, HttpResponse response)
        {
            if (GetUserId(session) < 0 && PerformSessionValidation)
            {
                response.Redirect(LoginExpiredLink);
            }
        }

        #endregion

        #region StringBufferUtilities

        public static void AppentWindowOpenerValue(HttpRequest req, StringBuilder sb, string fname, string value)
        {
            if (req.QueryString[fname] != null && req.QueryString[fname].Trim() != "")
            {
                sb.Append("window.opener.document.forms[0].item(\"");
                sb.Append(req.QueryString[fname]);
                sb.Append("\").value = \"");
                sb.Append(value);
                sb.Append("\"; ");
            }
        }

        public static void AppentWindowOpenerInnerHTML(HttpRequest req, StringBuilder sb, string fname, string value)
        {
            if (req.QueryString[fname] != null && req.QueryString[fname] != "")
            {
                sb.Append("window.opener.document.getElementById(\"");
                sb.Append(req.QueryString[fname]);
                sb.Append("\").innerHTML = \"");
                sb.Append(value);
                sb.Append("\"; ");
            }
        }

        public static void AddNonEmptyHtmlLine(StringBuilder sb, string line)
        {

            if (line != null && line.Trim().Length > 0)
            {
                sb.Append(line);
                sb.Append("<br/>");
            }
        }

        #endregion

        #region Encode / Decode URL

        public static string EncodeURL(string url)
        {
            return url.Replace("&", "`").Replace("=", "~").Replace(" ", "^").Replace("?", "%*");
        }

        public static string DecodeUrl(string url)
        {
            return url.Replace("~", "=").Replace("`", "&").Replace("^", " ").Replace("%*", "?");
        }

        #endregion

        public static void AddSortArrow(GridView gv, HTBUtilities.SortDirection direction, string sortField)
        {
            if (!string.IsNullOrEmpty(sortField) && gv.ShowHeader)
            {
                GridViewRow row = gv.HeaderRow;
                if (row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tc in row.Cells)
                    {
                        if (!tc.HasControls()) continue;
                        // search for the header link
                        var lnk = (LinkButton) tc.Controls[0];

                        // inizialize a new image
                        var img = new Image
                                      {
                                          ImageUrl =
                                              "~/v2/intranet/images/sort_" +
                                              (direction == HTBUtilities.SortDirection.Asc ? "asc" : "desc") + ".gif",
                                          Width = 15,
                                          Height = 15
                                      };
                        // setting the dynamically URL of the image
                        // checking if the header link is the user's choice
                        if (sortField == lnk.CommandArgument)
                        {
                            // adding a space and the image to the header link
                            tc.Controls.Add(new System.Web.UI.LiteralControl(" "));
                            tc.Controls.Add(img);
                        }
                    }
                }
            }
        }

        private static string FormatHtmlParam(string name, string value)
        {
            if (value != null && !value.Trim().Equals(string.Empty))
            {
                return name + "=" + FormatUmlautsForHTML(value);
            }
            return null;
        }

        public static void AppendHtmlParamIfNotEmpty(StringBuilder sb, string name, string value, bool addAmp = true, string ampString = "&")
        {
            string param = FormatHtmlParam(name, value);
            if (param != null)
            {
                if (addAmp)
                    sb.Append(ampString);
                sb.Append(HttpUtility.UrlPathEncode(param));
            }
        }

        public static string FormatUmlautsForHTML(string str)
        {
            if (IsTablet())
            {
                return str.Replace("ß", "%DF")
                    .Replace("Ä", "%C4")
                    .Replace("Ö", "%D6")
                    .Replace("Ü", "%DC")
                    .Replace("ä", "%E4")
                    .Replace("ö", "%F6")
                    .Replace("ü", "%FC");
            }
            return str;
        }
        
        public static bool IsPopUp(HttpRequest req)
        {
            return GetEmptyIfNull(req.QueryString[GlobalHtmlParams.IS_POPUP]).Trim().Equals(GlobalHtmlParams.YES);
        }

        public static bool IsZero(double val)
        {
            return AlmostEqual(val, 0);
        }

        #region Almost Equal

        public static bool AlmostEqual(double val1, double val2, double epsilon = Epsilon)
        {

            return Math.Abs(val1 - val2) < epsilon;
        }
        #endregion

        #region HTML Controls
        public static HtmlTableRow GetTableRow(List<HtmlControl> controls)
        {
            var row = new HtmlTableRow();
            foreach (var control in controls)
            {
                var cell = new HtmlTableCell();
                cell.Controls.Add(control);
                row.Cells.Add(cell);
            }
            return row;
        }
        #endregion

        #region Get Intervention Actions
        
        public static ArrayList GetInterventionActions(int agId, int aktType, HttpSessionState session)
        {
            /* Try AG + Type First */
            string sqlQuery = "SELECT * FROM qryAuftraggeberAktTypeAction " +
                                " WITH (NOLOCK) WHERE AGAktionTypeAuftraggeberID = " + agId +
                                " AND AGAktionTypeAktTypeIntID = " + aktType +
                                " ORDER BY AktIntActionTypeCaption";
            ArrayList list = GetActions(sqlQuery, typeof(qryAuftraggeberAktTypeAction), session);
            if (list.Count > 0)
            {
                return list;
            }

            /* Than Try Just Type */
            sqlQuery = "SELECT * FROM qryAktTypeAction " +
                        " WITH (NOLOCK) WHERE AktTypeActionAktTypeIntID = " + aktType +
                        " ORDER BY AktIntActionTypeCaption";
            list = GetActions(sqlQuery, typeof(qryAktTypeAction), session);
            if (list.Count > 0)
            {
                return list;
            }

            /* Than Try Just AG */
            sqlQuery = "SELECT * FROM qryAuftraggeberAction " +
                        " WITH (NOLOCK) WHERE AGAktionAuftraggeberID = " + agId +
                        " ORDER BY AktIntActionTypeCaption";
            list = GetActions(sqlQuery, typeof(qryAuftraggeberAction), session);
            if (list.Count > 0)
            {
                return list;
            }

            /*If All fails show default actions */
            sqlQuery = "SELECT * FROM tblAktenIntActionType WITH (NOLOCK) WHERE AktIntActionIsDefault = 1 ORDER BY AktIntActionTypeCaption";
            return GetActions(sqlQuery, typeof(tblAktenIntActionType), session);
        }
        private static ArrayList GetActions(string slqQuery, Type classType, HttpSessionState session)
        {
            var resultsList = HTBUtils.GetSqlRecords(slqQuery, classType);
            var list = new ArrayList();

            if (resultsList.Count > 0)
            {
                foreach (Record action in resultsList)
                {
                    list.Add(new ActionRecord(action));
                }
                return GetUserActions(list, session);
            }
            return list;
        }
        private static ArrayList GetUserActions(ArrayList actionsList, HttpSessionState session)
        {
            ArrayList userActionsList = HTBUtils.GetSqlRecords("SELECT * FROM qryUserAktionen WITH (NOLOCK) WHERE UserAktionUserID = " + GetUserId(session), typeof(qryUserAktionen));
            if (userActionsList.Count > 0)
            {
                var resultsList = new ArrayList();
                foreach (ActionRecord agAction in actionsList)
                {
                    foreach (qryUserAktionen userAction in userActionsList)
                    {
                        if (agAction.ActionID == userAction.UserAktionAktIntActionTypeID)
                        {
                            resultsList.Add(agAction);
                        }
                    }
                }
                return resultsList;
            }
            return actionsList;
        }

        #endregion

        #region Address Lookup [from Lat, Lgn]
        public static string GetAddressFromLatitudeAndLongitude(double latitude, double longitude)
        {
            var reverseGeocodeRequest = new ReverseGeocodeRequest {Credentials = new Credentials {ApplicationId = RoutePlanerManager.BingMapsKey}};

            // Set the point to use to find a matching address
            var point = new Location {Latitude = latitude, Longitude = longitude};

            reverseGeocodeRequest.Location = point;

            // Make the reverse geocode request
            var geocodeService = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            GeocodeResponse geocodeResponse = geocodeService.ReverseGeocode(reverseGeocodeRequest);

            return geocodeResponse.Results.Length > 0 ? geocodeResponse.Results[0].DisplayName : "";
        }
        /*
        private void LoadGeocodeAddressFromGoogleMaps(object state)
        {

            var stateInfo = (AddressLookupState)state;
            var request = new GeocodingRequest
            {
                Address = stateInfo.Address.Address,
                Sensor = "false"
            };
            var response = GeocodingService.GetResponse(request);
            if (response.Status == ServiceResponseStatus.Ok)
            {
                if (response.Results == null || response.Results.Length <= 0)
                {
                    AddressLookupStateStaticsAccess.GetAddressStateStatic(stateInfo.UserId).BadAddresses.Add(stateInfo.Address);
                }
                else
                {
                    var location = new GeocodeLocation
                    {
                        Latitude = Convert.ToDouble(response.Results[0].Geometry.Location.Latitude),
                        Longitude = Convert.ToDouble(response.Results[0].Geometry.Location.Longitude)
                    };
                    AddressLookupStateStaticsAccess.GetAddressStateStatic(stateInfo.UserId).Addresses.Add(new City(new AddressLocation(stateInfo.Address, new GeocodeLocation[] { location }), null));
                    SaveGegnerLocation(stateInfo.Address.ID, location.Latitude, location.Longitude);
                }
            }
            else
            {
                AddressLookupStateStaticsAccess.GetAddressStateStatic(stateInfo.UserId).BadAddresses.Add(stateInfo.Address);
            }
        }
         * */
        #endregion

        #region Debug
        public static void PrintFormParameters(HttpRequest request, HttpResponse response)
        {
            // If there are any form variables, get them here:
            response.Write("\nForm Variables:\n");

            //Load Form variables into NameValueCollection variable.
            NameValueCollection coll = request.Form;

            // Get names of all forms into a string array.
            String[] arr1 = coll.AllKeys;
            foreach (string t in arr1)
            {
                response.Write(t);
                response.Write(":    ");
                response.Write(request.Params[t]);
                response.Write("\n");
            }
            response.Write("\n\r");
        }
        #endregion
    }
    
}