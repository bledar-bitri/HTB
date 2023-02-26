﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using HTB.Database;
using System.Reflection;
using HTB.Database.Views;
using System.Text.RegularExpressions;
using System.Data;

using log4net;
using Microsoft.VisualBasic;

namespace HTBUtilities
{
    public static class HTBUtils
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        private const double Epsilon = 0.001;
        public static DateTime DefaultDate = new DateTime(1900, 1, 1);
        public const string DefaultShortDate = "01.01.1900";
        public const int EarthRadius = 6380;
        public static readonly bool IsTestEnvironment = GetBoolValue(GetConfigValue("IsTest"));
        public const string DateFormat = "dd.MM.yyyy";

        public static ArrayList GetSqlRecords(string sql, Type recordClass, int connType = DbConnection.ConnectionType_SqlServer)
        {
            var set = new GenericRecordset(connType);
            Log.DebugFormat("SQL: {0}" , sql);
            set.LoadFromSqlQuery(sql, recordClass);
            return set.RecordsList;
        }

        public static object GetSqlSingleRecord(string sql, Type recordClass, int connType = DbConnection.ConnectionType_SqlServer, bool debugMode = false)
        {
            Log.DebugFormat("{0}", sql);
            var set = new GenericRecordset(connType);
            set.LoadFromSqlQuery(sql, recordClass, debugMode);
            if (set.RecordsList.Count > 0)
                return set.RecordsList[0];

            return null;
        }

        public static ArrayList[] GetMultipleListsFromStoredProcedure(string spName, ArrayList parameters, Type[] types)
        {
            var set = new GenericRecordset();
            Log.DebugFormat("SP: [Multiple Lists] {0}" , spName);
            return set.GetMultipleListsFromStoredProcedure(spName, parameters, types);
        }

        public static ArrayList GetStoredProcedureRecords(string spName, ArrayList parameters, Type recordClass)
        {
            var set = new GenericRecordset();
            Log.DebugFormat("SP: {0}" , spName);
            set.LoadFromStoredProcedure(spName, parameters, recordClass);
            return set.RecordsList;
        }

        public static object GetStoredProcedureSingleRecord(string spName, ArrayList parameters, Type recordClass)
        {
            ArrayList list = GetStoredProcedureRecords(spName, parameters, recordClass);
            if (list.Count > 0)
                return list[0];

            return null;
        }

        public static void ExecuteStoredProcedure(string spName, ArrayList parameters)
        {
            new RecordSet().ExecuteStoredProcedure(spName, parameters);
        }
        public static tblControl GetControlRecord()
        {
            return (tblControl)GetSqlSingleRecord("SELECT * FROM tblControl WITH (NOLOCK) WHERE ControlCode = 'HTB'", typeof(tblControl));
        }

        public static qryCustInkAkt GetInkassoAktQry(int aktId)
        {
            return (qryCustInkAkt)GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WITH (NOLOCK) WHERE CustInkAktID = " + aktId, typeof(qryCustInkAkt));
        }

        public static qryAktenInt GetInterventionAktQry(int aktId)
        {
            return (qryAktenInt)GetSqlSingleRecord("SELECT * FROM qryAktenInt WITH (NOLOCK) WHERE AktIntID = " + aktId, typeof(qryAktenInt));
        }

        public static tblCustInkAkt GetInkassoAkt(int aktId)
        {
            return (tblCustInkAkt)GetSqlSingleRecord("SELECT * FROM tblCustInkAkt WITH (NOLOCK) WHERE CustInkAktID = " + aktId, typeof(tblCustInkAkt));
        }

        public static tblAktenInt GetInterventionAkt(int aktId)
        {
            return (tblAktenInt)GetSqlSingleRecord("SELECT * FROM tblAktenInt WITH (NOLOCK) WHERE AktIntID = " + aktId, typeof(tblAktenInt));
        }

        public static ArrayList GetPosList(int intAktId)
        {
            return GetSqlRecords("SELECT * FROM tblAktenIntPos WITH (NOLOCK) WHERE AktIntPosAkt = " + intAktId, typeof(tblAktenIntPos));
        }

        public static int GetIntFromQuery(String query, Type recordClass)
        {
            int wrett = -1;
            var wset = new GenericRecordset();
            try
            {
                wset.LoadFromSqlQuery(query, recordClass);
                if (wset.RecordsList.Count > 0)
                {
                    wrett = ((SingleValue) wset.RecordsList[0]).IntValue;
                }
            }
            catch 
            {
            }
            return wrett;
        }

        public static string GetConfigValue(string name)
        {
            var rec = (tblConfig)GetSqlSingleRecord("SELECT * FROM tblConfig WITH (NOLOCK) WHERE ConfigName = '" + name + "'", typeof(tblConfig));
            return rec?.ConfigValue;
        }

        public static int GetIntConfigValue(string name)
        {
            try
            {
                return int.Parse(GetConfigValue(name));
            }
            catch
            {
                return 0;
            }
        }

        public static List<string> GetKlientEmailAddresses(int klientId, string additionalEmailAddress = null)
        {
            ArrayList contactsList = GetSqlRecords("SELECT * FROM tblAnsprechpartner WITH (NOLOCK) WHERE AnsprechKlient = " + klientId, typeof(tblAnsprechpartner));
            var toList = new List<string>();
            if (!string.IsNullOrEmpty(additionalEmailAddress) && IsValidEmail(additionalEmailAddress))
                toList.Add(additionalEmailAddress);

            if (contactsList.Count > 0)
            {
                var found = false;
                foreach (tblAnsprechpartner contact in contactsList)
                {
                    foreach (var email in toList)
                    {
                        if(contact.AnsprechEMail == email)
                        {
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        toList.Add(contact.AnsprechEMail);
                    }
                }
            }
            return toList;
        }

        public static string GetADEmailAddress(int aktId)
        {
            var akt = (qryAktenInt)GetSqlSingleRecord("SELECT UserEMailOffice FROM qryAktenInt WITH (NOLOCK) WHERE AktIntID = " + aktId, typeof(qryAktenInt));
            if (akt != null && !string.IsNullOrEmpty(akt.UserEMailOffice) && IsValidEmail(akt.UserEMailOffice))
            {
                return akt.UserEMailOffice;
            }
            return null;
        }

        public static tblUser GetUser(int userId)
        {
            return (tblUser)GetSqlSingleRecord("SELECT * FROM tblUser WITH (NOLOCK) WHERE UserID = " + userId, typeof(tblUser));
        }

        public static tblKlient GetKlientById(int klientId)
        {
            return (tblKlient)GetSqlSingleRecord(string.Format("SELECT * FROM tblKlient WITH (NOLOCK) WHERE KlientID = {0}", klientId), typeof(tblKlient));
        }
        public static tblKlient GetKlient(string name1, string street, string zip)
        {
            var sb = new StringBuilder("SELECT TOP 1 KlientID, KlientOldID, KlientLawyerId FROM tblKlient WITH (NOLOCK) WHERE RTRIM(LTRIM(UPPER(KlientName1))) = '");
            sb.Append(name1.ToUpper());
            sb.Append("' AND RTRIM(LTRIM(UPPER(KlientStrasse))) = '");
            sb.Append(street.ToUpper());
            sb.Append("' AND KlientPLZ = '");
            sb.Append(zip);
            sb.Append("'");
            return (tblKlient)GetSqlSingleRecord(sb.ToString(), typeof(tblKlient));
        }
        public static tblKlient GetEntireKlientRecord(string name1, string street, string zip)
        {
            var sb = new StringBuilder("SELECT TOP 1 * FROM tblKlient WITH (NOLOCK) WHERE RTRIM(LTRIM(UPPER(KlientName1))) = '");
            sb.Append(name1.ToUpper());
            sb.Append("' AND RTRIM(LTRIM(UPPER(KlientStrasse))) = '");
            sb.Append(street.ToUpper());
            sb.Append("' AND KlientPLZ = '");
            sb.Append(zip);
            sb.Append("'");
            return (tblKlient)HTBUtils.GetSqlSingleRecord(sb.ToString(), typeof(tblKlient));
        }
        public static tblKlient CreateKlient(tblKlient klient, int counter = 0)
        {

            const string tmpOldId = "sssss";
            try
            {
                if (counter > 0)
                {
                    var set = new RecordSet();
                    set.ExecuteNonQuery("DELETE tblKlient WITH (NOLOCK) WHERE KlientOldID = '" + tmpOldId + "'");
                }
                
                klient.KlientOldID = tmpOldId;

                if (RecordSet.Insert(klient))
                {
                    klient = (tblKlient)GetSqlSingleRecord("SELECT TOP 1 * FROM tblKlient ORDER BY KlientID DESC", typeof(tblKlient));
                    klient.KlientOldID = "1000" + klient.KlientID + ".002";
                    if (RecordSet.Update(klient))
                    {
                        return klient;
                    }
                }
            }
            catch 
            {
                if (counter == 0)
                    return CreateKlient(klient, 1);
            }
            return null;
        }

        public static tblGegner GetGegner(int gegnerId)
        {
            return (tblGegner)GetSqlSingleRecord("SELECT * FROM tblGegner WITH (NOLOCK) WHERE GegnerID = " + gegnerId, typeof(tblGegner));
        }
        public static tblGegner GetGegner(string name1, string name2, string street, string zip, DateTime dob)
        {
            var sb = new StringBuilder("SELECT TOP 1 * FROM tblGegner WITH (NOLOCK) WHERE RTRIM(LTRIM(UPPER(GegnerLastName1))) = '");
            sb.Append(name1.ToUpper());
            sb.Append("' AND RTRIM(LTRIM(UPPER(GegnerLastName2))) = '");
            sb.Append(name2.ToUpper());
            sb.Append("' AND RTRIM(LTRIM(UPPER(GegnerLastStrasse))) = '");
            sb.Append(street.ToUpper());
            sb.Append("' AND GegnerLastZIP = '");
            sb.Append(zip);
            sb.Append("'");
            if (dob != DefaultDate)
            {
                sb.Append(" AND GegnerGebdat = '");
                sb.Append(dob.ToShortDateString());
                sb.Append("'");
            }
            return (tblGegner)GetSqlSingleRecord(sb.ToString(), typeof(tblGegner));
        }
        public static tblGegner CreateGegner(tblGegner gegner, int counter = 0)
        {
            const string tmpOldId = "sssss";
            try
            {
                if (counter > 0)
                {
                    var set = new RecordSet();
                    set.ExecuteNonQuery("DELETE tblGegner WITH (NOLOCK) WHERE GegnerOldID = '" + tmpOldId + "'");
                }
                gegner.GegnerOldID = tmpOldId;
                if (RecordSet.Insert(gegner))
                {
                    gegner = (tblGegner)GetSqlSingleRecord("SELECT TOP 1 * FROM tblGegner ORDER BY GegnerID DESC", typeof(tblGegner));
                    gegner.GegnerOldID = "1000" + gegner.GegnerID + ".002";
                    if (RecordSet.Update(gegner))
                    {
                        return gegner;
                    }
                }
            }
            catch 
            {
                if (counter == 0)
                    return CreateGegner(gegner, 1);
            }
            return null;
        }

        public static int GetGegnerSB(qryCustInkAkt akt)
        {
            return GetGegnerSB(akt.GegnerOldID);
        }

        public static int GetGegnerSB(string gegnerOldId)
        {
            return GetGegnerSB((tblGegner)GetSqlSingleRecord("SELECT * FROM tblGegner WITH (NOLOCK) WHERE GegnerOldID = " + gegnerOldId, typeof(tblGegner)));
        }

        public static int GetGegnerSB(int gegnerId)
        {
            return GetGegnerSB((tblGegner)GetSqlSingleRecord("SELECT * FROM dbo.tblGegner WITH (NOLOCK) WHERE GegnerID = " + gegnerId, typeof(tblGegner)));
        }

        public static int GetGegnerSB(tblGegner gegner)
        {
            int sb = GetControlRecord().AutoUserId;
            try
            {
                if (gegner != null)
                {
                    var gebiet = (tblADGebiete)GetSqlSingleRecord("SELECT * FROM tblADGebiete WITH (NOLOCK) WHERE ADGEBIETSTARTZIP <= '" + gegner.GegnerLastZip + "' AND ADGEBIETENDZIP >= '" + gegner.GegnerLastZip + "'", typeof(tblADGebiete));
                    if (gebiet != null)
                    {
                        sb = gebiet.ADGebietUser;
                    }
                }
            }
            catch
            {
                sb = GetControlRecord().AutoUserId;
            }
            return sb;
        }
        public static string GetSBEmailAddress(int aktId)
        {
            var akt = (qryAktenInt)GetSqlSingleRecord("SELECT AKTIntKSVEMail FROM qryAktenInt WITH (NOLOCK) WHERE AktIntID = " + aktId, typeof(qryAktenInt));
            if (akt != null && !string.IsNullOrEmpty(akt.AKTIntKSVEMail) && IsValidEmail(akt.AKTIntKSVEMail))
            {
                return akt.AKTIntKSVEMail;
            }
            return null;
        }

        public static string GetCityForZip(string zip)
        {
            int zipInt;
            try
            {
                zipInt = Convert.ToInt32(zip);
            }
            catch { zipInt = 9999999; }
            var ort = (tblOrte)GetSqlSingleRecord("SELECT * FROM tblOrte WITH (NOLOCK) WHERE " + GetZipWhere(zipInt), typeof(tblOrte));
            return ort != null ? ort.Ort : "unbekannt";
        }

        public static string GetADSalutationAndName(int aktId, bool isGeherter = true, bool includeFirstName = false)
        {
            var akt = (qryAktenInt)GetSqlSingleRecord("SELECT * FROM qryAktenInt WITH (NOLOCK) WHERE AktIntID = " + aktId, typeof(qryAktenInt));
            if (akt != null)
            {
                string geherter = "";
                string name = includeFirstName ? akt.UserVorname + " " + akt.UserNachname : akt.UserNachname;
                switch (akt.UserSex)
                {
                    case 1:
                        if (isGeherter) geherter = "r ";
                        return geherter+"Herr " + name;
                    default:
                        if (isGeherter) geherter = " ";
                        return geherter + "Frau " + name;
                }
            }
            return null;
        }
        public static string GetEmailSalutationAndName(int gender, bool isGeherter = true)
        {
            string geherter = "";
            switch (gender)
            {
                case 1:
                    if (isGeherter) geherter = "r ";
                        return geherter + "Herr ";
                default:
                    if (isGeherter) geherter = " ";
                        return geherter + "Frau ";
            }
        }
        
        public static string GetAktGegnerNameAndAddress(int aktId, bool isHtml = false, int indentSpaces = 0)
        {
            var akt = (qryAktenInt)GetSqlSingleRecord("SELECT * FROM qryAktenInt WITH (NOLOCK) WHERE AktIntID = " + aktId, typeof(qryAktenInt));
            var sbIndent = new StringBuilder();
            
            for (int i = 0; i < indentSpaces; i++)
                sbIndent.Append(isHtml ? "&nbsp;" : " ");

            var newline = isHtml ? "<br/>" : Environment.NewLine;
            newline += sbIndent.ToString();

            if (akt != null)
            {
                return sbIndent + akt.GegnerLastName1 + " " + akt.GegnerLastName2 + newline + akt.GegnerLastStrasse + newline + akt.GegnerLastZip + " " + akt.GegnerLastOrt;
            }
            return null;
        }

        public static void DeleteInstallmentPlan(int aktId)
        {
            var list = new ArrayList
                           {
                               new StoredProcedureParameter("inkAktId", SqlDbType.Int, aktId)
                           };

            ExecuteStoredProcedure("spDeleteInstallmentPlan", list);

            //new RecordSet().ExecuteNonQuery("DELETE FROM tblCustInkAktRate WITH (NOLOCK) WHERE CustInkAktRateAktID = " + inkAktId);
        }

        public static void DeleteInkassoAkt(int aktId)
        {
            DeleteAllDocumentsOfInkassoAkt(aktId);

            var list = new ArrayList
                           {
                               new StoredProcedureParameter("inkAktId", SqlDbType.Int, aktId)
                           };
            ExecuteStoredProcedure("spDeleteInkassoAkt", list);
        }


        public static bool IsPaymentTransferrable(int invoiceId)
        {
            int result = 0;
            var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("invoiceId", SqlDbType.Int, invoiceId),
                                     new StoredProcedureParameter("result", SqlDbType.Bit, result, ParameterDirection.Output)
                                 };

            ExecuteStoredProcedure("spIsPaymentTransferrable", parameters);
            foreach (object o in parameters)
            {
                if (o is ArrayList)
                {
                    var outputList = (ArrayList)o;
                    foreach (StoredProcedureParameter p in outputList)
                    {
                        if (p.Name.IndexOf("result") >= 0)
                        {
                            result = Convert.ToInt32(p.Value);
                        }
                    }
                }
            }
            return result > 0;
        }


        public static List<qryDoksInkAkten> GetDocumentsForInkassoAkt(int aktId)
        {
            return GetSqlRecords(string.Format("SELECT * FROM qryDoksInkAkten WITH (NOLOCK) WHERE CustInkAktID = {0}", aktId), typeof(qryDoksInkAkten)).Cast<qryDoksInkAkten>().ToList();
        }

        #region Calculations

        public static decimal GetCalculatedKost(decimal amount, qryKosten kostRecord, DateTime aktDate)
        {
            if (kostRecord.KostenAmount > 0)
            {
                return kostRecord.KostenAmount;
            }
            if (kostRecord.IsZinsen)
            {
                return GetCalculatedInterest(amount, kostRecord.KostenPct, aktDate);
            }
            return amount*(kostRecord.KostenPct/100);
        }

        public static decimal GetCalculatedInterest(decimal kapital, decimal rate, DateTime startDate)
        {
            TimeSpan ts = DateTime.Now.Subtract(startDate);
            int days = 1;
            if (ts.Days > 1)
            {
                days = ts.Days;
            }
            //return (decimal)((kapital * days * rate / 100) / 360);    // calculate interest for the same day
            return (decimal) ((kapital*ts.Days*rate/100)/360); // do not calculate interest for the same day
        }

        public static bool IsZero(double val)
        {
            return AlmostEqual(val, 0);
        }

        public static bool AlmostEqual(double val1, double val2, double epsilon = Epsilon)
        {

            return Math.Abs(val1 - val2) < epsilon;
        }

        public static double GetDistance(tblUser user, string toZip)
        {
            if (user != null)
                return GetDistance(user.UserZIP, toZip);

            return 0;
        }
        public static double GetDistance(string fromZip, string toZip)
        {
            double distance = 0;
           
            Log.DebugFormat("Calculating Distance for PLZ: {0}" , toZip);

            var ortFrom = (tblOrte)GetSqlSingleRecord("SELECT * FROM dbo.tblOrte WITH (NOLOCK) WHERE " + GetZipWhere(fromZip), typeof(tblOrte));
            var ortTo = (tblOrte)GetSqlSingleRecord("SELECT * FROM dbo.tblOrte WITH (NOLOCK) WHERE " + GetZipWhere(toZip), typeof(tblOrte));
            if (ortTo != null && ortFrom != null)
            {
                if (!IsZero(ortFrom.B) && !IsZero(ortTo.B) && !IsZero(ortFrom.L) && !IsZero(ortTo.L))
                {
                    Log.DebugFormat(" From Place: {0}\n Latitute: {1}\n Longitude: {2}", ortFrom.Ort , ortFrom.L, ortFrom.B);
                    Log.DebugFormat(" To Place: {0}\n Latitute: {1}\n Longitude: {2}", ortTo.Ort, ortTo.L, ortTo.B);
                    var aLon = ortFrom.B * (3.1415925 / 180);
                    var bLon = ortTo.B * (3.1415925 / 180);
                    var aLat = ortFrom.L * (3.1415925 / 180);
                    var bLat = ortTo.L * (3.1415925 / 180);
                    Log.DebugFormat(" A_Lon: {0}\n B_Lon: {1}\n A_Lat: {2}\n B_Lat: {3}", aLon, bLon, aLat, bLat);
                    distance = ArcCos(Math.Sin(bLat) * Math.Sin(aLat) + Math.Cos(bLat) * Math.Cos(aLat) * Math.Cos(bLon - aLon)) * EarthRadius;
                }
            }
            return distance;
        }

        /*
         * This method calculates correctly but the business rules mandate we do away with 'weggebühr'
         */
        public static double GetWeggebuehr_ORG(double distance)
        {

            var wege = (tblWege)GetSqlSingleRecord("SELECT * FROM tblWege WITH (NOLOCK) WHERE WegVon <= " + distance + " AND WegBis >= " + distance, typeof(tblWege));
            if (wege != null)
            {
                return wege.Preis;
            }
            return 0;
        }

        /*
         * See GetWeggebuehr_ORG for the real calculation
         */
        public static double GetWeggebuehr(double distance)
        {
            return 0;
        }

        public static double ArcCos(double x)
        {
            try
            {
                return Math.Atan(-x / Math.Sqrt(-x * x + 1)) + 2 * Math.Atan(1);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region Formating

        public static string FormatCurrency(double amount, bool isHtml = false)
        {
            //return "€ " + amount.ToString("N.2");
            if (isHtml)
                return "€&nbsp;" + amount.ToString("N2");
            return "€ " + amount.ToString("N2");
        }

        public static string FormatCurrency(decimal amount, bool isHtml = false)
        {
            //return "€ " + amount.ToString("N.2");
            if (isHtml)
                return "€&nbsp;" + amount.ToString("N2");
            return "€ " + amount.ToString("N2");
        }

        public static string FormatPercent(double amount, bool isHtml = true)
        {
            if (isHtml)
                return (amount*100).ToString("N2") + "&nbsp;&#37;";
            
            return (amount*100).ToString("N2") + " %";
        }
        
        public static string FormatCurrencyNumber(decimal amount)
        {
            return amount.ToString("N2");
        }

        public static string FormatCurrencyNumber(double amount)
        {
            return amount.ToString("N2");
        }

        public static string GetHtmlSpaceForEmpty(string str)
        {
            return str == null || str.Trim().Equals("") ? "&nbsp;" : str;
        }

        public static string RemoveAllSpecialChars(string str, bool removeSpaces = false)
        {
            string ret = str.
                Replace("!", "").
                Replace("@", "").
                Replace("$", "").
                Replace("%", "").
                Replace("^", "").
                Replace("&", "").
                Replace("*", "").
                Replace("(", "").
                Replace(")", "").
                Replace("_", "").
                Replace("-", "").
                Replace("+", "").
                Replace("=", "").
                Replace("\\", "").
                Replace("|", "").
                Replace("{", "").
                Replace("}", "").
                Replace("[", "").
                Replace("]", "").
                Replace("?", "").
                Replace("/", "").
                Replace(".", "").
                Replace(",", "").
                Replace(">", "").
                Replace("<", "").
                Replace("~", "").
                Replace("`", "");
            if (removeSpaces)
                ret = ret.Replace(" ", "");
            return ret;
        }

        public static IEnumerable<string> SplitStringInPdfLines(string lines, int maxLineLength)
        {
            var list = new ArrayList();
            if (lines.Length <= maxLineLength)
            {
                list.Add(lines);
            }
            else
            {
                var sb = new StringBuilder();
                string[] words = lines.Replace(" ", " `").Split();
                foreach (string word in words)
                {
                    if ((sb.Length + word.Length) > maxLineLength)
                    {
                        list.Add(sb.ToString().Trim());
                        sb.Clear();
                    }
                    sb.Append(word.Replace("`", " "));
                }
                list.Add(sb.ToString().Trim());
            }
            return list.Cast<string>().ToArray();
        }

        public static string ReplaceHtmlBreakWithNewLine(string str)
        {
            return str.Replace("<br>", "\n")
                .Replace("<br/>", "\n")
                .Replace("<BR>", "\n")
                .Replace("<BR/>", "\n");
        }

        public static string ReplaceUmlautsWithHtmlCodes(string str)
        {
            str = str.Replace("ß", "&szlig;");
            str = str.Replace("Ä", "&Auml;");
            str = str.Replace("Ö", "&Ouml;");
            str = str.Replace("Ü", "&Uuml;");
            str = str.Replace("ä", "&auml;");
            str = str.Replace("ö", "&ouml;");
            return str.Replace("ü", "&uuml;");
        }
        
        public static string FormatTimeSpan(TimeSpan ts)
        {
            return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
        }
        #endregion

        #region Miscelaneous Utilities
        public static string GetZipWhere(int zip)
        {
            return GetZipWhere(zip.ToString());
        }
        public static string GetZipWhere(string zip)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < 21; i++)
            {
                sb.Append(" ZIP");
                if (i != 1)
                    sb.Append(i.ToString());
                sb.Append(" = '");
                sb.Append(zip);
                sb.Append("'");

                if (i != 20)
                    sb.Append(" OR ");
            }
            return sb.ToString();
        }
        public static bool IsDateValid(DateTime dte)
        {
            if (dte.Equals(DateTime.MinValue) || dte.Equals(DefaultDate))
                return false;

            return dte.ToString(DateFormat) != "01.01.1900" && dte.ToString(DateFormat) != "01.01.0001" && dte.ToString(DateFormat) != "01.01.1970";
        }
        public static bool IsNumber(string str)
        {
            try
            {
                Convert.ToInt64(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static List<string> GetValidEmailAddressesFromStrings(string[] strsIn)
        {
            var ret = new List<string>();
            if (strsIn == null || strsIn.Length == 0)
                return ret;
            foreach (var s in strsIn)
            {
                ret.AddRange(GetValidEmailAddressesFromString(s));
            }
            return ret;
        }
        public static List<string> GetValidEmailAddressesFromString(string strIn)
        {
            var ret = new List<string>();
            if (string.IsNullOrEmpty(strIn))
                return ret;
            var splits = strIn.Split();
            ret.AddRange(splits.Where(IsValidEmail));
            return ret;
        }

        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            
            return !string.IsNullOrEmpty(strIn) && Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }
        public static void AddListToList(ArrayList from, ArrayList to)
        {
            foreach (var obj in @from)
            {
                to.Add(obj);
            }
        }

        public static string GetJustFileName(String filePath)
        {
            int idx = filePath.LastIndexOf("/");
            if (idx < 0)
                idx = filePath.LastIndexOf("\\");

            if (idx >= 0)
            {
                return filePath.Substring(idx+1);
            }
            return filePath;
        }

        public static string GetFileExtenssion(String fileName)
        {
            var idx = fileName.LastIndexOf(".", StringComparison.Ordinal);
            
            return idx >= 0 ? fileName.Substring(idx + 1) : fileName;
        }
        public static string ReplaceStringBetween(string data, string startToken, string endToken, string replaceWith, int startIndex = 0, bool matchCase = false)
        {
            int idx = matchCase ? data.IndexOf(startToken, startIndex) : data.ToLower().IndexOf(startToken.ToLower(), startIndex);

            if (idx < 0)
                return data;

            int idx2 = matchCase ? data.IndexOf(endToken, idx + startToken.Length, idx) : data.ToLower().IndexOf(endToken.ToLower(), idx + startToken.Length, idx);
            
            if (idx2 < 0)
                return data;

            return data.Substring(0, idx) + replaceWith + data.Substring(idx2 + 1);
        }

        public static string ReplaceStringAfter(string data, string token, string replaceWith, int startIndex = 0, bool matchCase = false)
        {
            int idx = matchCase ? data.IndexOf(token, startIndex) : data.ToLower().IndexOf(token.ToLower(), startIndex);

            if (idx < 0)
                return data;

            return data.Substring(0, idx) + replaceWith;
        }
        
        public static bool GetBoolValue(string str)
        {
            return str != null && (
                                      str.ToLower() == "y" ||
                                      str.ToLower() == "1" ||
                                      str.ToLower() == "true");
        }

        public static string GetKlientNotifiationEmailSubject(tblKlient klient, StringBuilder body, List<string> receipients)
        {
            var sb = new StringBuilder();
            switch (klient.KlientNachricht)
            {
                case 1: // Fax
                    sb.Append("Bitte an Klient per Fax schicken: <strong>");
                    sb.Append(klient.KlientName1);
                    sb.Append(" ");
                    sb.Append(klient.KlientName2 ?? klient.KlientName2);
                    sb.Append("</strong> <br/>Fax: ");
                    sb.Append(klient.KlientFaxCountry);
                    sb.Append(" ");
                    sb.Append(klient.KlientFaxCity);
                    sb.Append(" ");
                    sb.Append(klient.KlientFax);
                    body.Insert(0, sb.ToString());
                    return "[An Klient Faxen] [" + klient.KlientFaxCountry + " " + klient.KlientFaxCity + " " + klient.KlientFax + "]!!!";
                case 3: // Post
                    sb.Append("Bitte an Klient per Post schicken: <strong>");
                    sb.Append(klient.KlientName1);
                    sb.Append(" ");
                    sb.Append(klient.KlientName2 ?? klient.KlientName2);
                    sb.Append("</strong> <br/>Adresse: ");
                    sb.Append(klient.KlientStrasse);
                    sb.Append(", ");
                    sb.Append(klient.KlientPLZ);
                    sb.Append(" ");
                    sb.Append(klient.KlientOrt);
                    body.Insert(0, sb.ToString());
                    return "[An Klient Per Post Schicken] [" + klient.KlientStrasse + " " + klient.KlientPLZ + " " + klient.KlientOrt + "]!!!";
                default: // Email
                    if (receipients.Count == 0)
                    {
                        sb.Append("Keine Emailadresse f&uuml;r Klient eingestellt: <strong>");
                        sb.Append(klient.KlientName1);
                        sb.Append(" ");
                        sb.Append(klient.KlientName2 ?? klient.KlientName2);
                        sb.Append("</strong>");
                        body.Insert(0, sb.ToString());
                        return "[An Klient NICHT Geschickt]!!!";
                    }
                    return "";
            }
        }

        public static bool IsAktSentToLawyer(int aktId)
        {
            try
            {
                var sb = new StringBuilder("select count(*) IntValue from tblCustInkAktAktion WITH (NOLOCK) WHERE CustInkAktAktionAktID = ");
                sb.Append(aktId.ToString());
                sb.Append("and CustInkAktAktionTyp in (select KostenCustInkAktCurStatus from tblKostenArt WITH (NOLOCK) WHERE KostenArtID in (select top 1 RechtsanwaldKostenArtId from tblControl))");
                var sv = (SingleValue) GetSqlSingleRecord(sb.ToString(), typeof (SingleValue));
                if (sv != null)
                    return sv.IntValue > 0;
            }
            catch
            {
                
            }
            return false;
        }

        public static DateTime GetFirstDayOfMonth()
        {
            return GetFirstDayOfMonth(DateTime.Now);
        }
        public static DateTime GetFirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime GetLastDayOfMonth()
        {
            return GetLastDayOfMonth(DateTime.Now);
        }
        public static DateTime GetLastDayOfMonth(DateTime dateTime)
        {
            var firstDayOfTheMonth = GetFirstDayOfMonth(dateTime);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
        public static bool IsLastDayOfMonthForDate(DateTime dateToTest)
        {
            return IsLastDayOfMonthForDate(DateTime.Now, dateToTest);
        }
        public static bool IsLastDayOfMonthForDate(DateTime mainDate, DateTime dateToTest)
        {
            var lastDayOfMonth = GetLastDayOfMonth(mainDate);
            return lastDayOfMonth.ToShortDateString() == dateToTest.ToShortDateString();
        }
        
        public static void AddInkassoAction(int aktId, string caption, string memo, int userId = 0)
        {
            // Create action
            RecordSet.Insert(
                new tblCustInkAktAktion
                    {
                        CustInkAktAktionAktID = aktId,
                        CustInkAktAktionDate = DateTime.Now,
                        CustInkAktAktionCaption = caption,
                        CustInkAktAktionMemo = memo.Replace("'", ""),
                        CustInkAktAktionUserId = userId,
                        CustInkAktAktionEditDate = DateTime.Now
                    });

        }

        public static void AddInterventionAction(int aktId, int actionType, string memo, int userId = 0)
        {
            // Create action
            RecordSet.Insert(
                new tblAktenIntAction
                {
                    AktIntActionAkt = aktId,
                    AktIntActionType =  actionType,
                    AktIntActionDate = DateTime.Now,
                    AktIntActionTime = DateTime.Now,
                    AktIntActionMemo = memo.Replace("'", ""),
                    AktIntActionSB = userId
                });

        }
        public static void AddInterventionAction(int aktId, string caption, string memo, int userId = 0)
        {
            // Create action
            RecordSet.Insert(
                new tblCustInkAktAktion
                {
                    CustInkAktAktionAktID = aktId,
                    CustInkAktAktionDate = DateTime.Now,
                    CustInkAktAktionCaption = caption,
                    CustInkAktAktionMemo = memo.Replace("'", ""),
                    CustInkAktAktionUserId = userId,
                    CustInkAktAktionEditDate = DateTime.Now
                });

        }
        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
            {
//                throw new ArgumentException("Incorrect last day " + lastDay);
                return 0;
            }

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
        
        public static string GetCountryName(string countryCode)
        {
            var country =
                (tblCountry)
                GetSqlSingleRecord("select * from tblCountry WITH (NOLOCK) WHERE CountryCode = '" + countryCode + "'",
                                   typeof (tblCountry));
            if (country != null)
            {
                return country.CountryName;
            }
            return "Austria";
        }
        public static string GetCountryName(int countryNumber)
        {
            var country =
                (tblCountry)
                GetSqlSingleRecord("select * from tblCountry WITH (NOLOCK) WHERE CountryNumber = '" + countryNumber + "'",
                                   typeof(tblCountry));
            if (country != null)
            {
                return country.CountryName;
            }
            return "Austria";
        }

        public static void CreateInkassoDocumentRecord(int aktId, string description, string fileName, int userId)
        {
            var doc = (tblDokument)GetSqlSingleRecord(string.Format("SELECT * FROM tblDokument WITH (NOLOCK) WHERE DokInkAkt = {0} AND (DokCaption = '{1}' OR  DokAttachment = '{2}')", aktId, description, fileName), typeof(tblDokument));
            
            if (doc != null)
            {
                doc.DokCaption = description;
                doc.DokAttachment = fileName;
                RecordSet.Update(doc);
            }
            else
            {
                doc = new tblDokument
                {
                    DokDokType = 25,
                    DokInkAkt = aktId,
                    DokCreator = userId,
                    DokCaption = description,
                    DokAttachment = fileName,
                    DokCreationTimeStamp = DateTime.Now,
                    DokChangeDate = DateTime.Now
                };

                RecordSet.Insert(doc);

                doc =
                    (tblDokument)GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC",
                            typeof (tblDokument));
                if (doc != null)
                {
                    RecordSet.Insert(new tblAktenDokumente {ADAkt = aktId, ADDok = doc.DokID, ADAkttyp = 1});
                }
            }
        }
        public static void DeleteAllDocumentsOfInkassoAkt(int aktId)
        {
            var documentsDirectory = HTBUtils.GetConfigValue("DocumentsFolder");
            var list = GetDocumentsForInkassoAkt(aktId);
            DeleteFiles(list.Select(doc => string.Format("{0}/{1}", documentsDirectory, doc.DokAttachment)).ToList());
        }
        #endregion

        #region File Utilities
        public static string GetFileText(string fileName)
        {
            string text = "";

            if (File.Exists(fileName))
            {
                var myFile = new StreamReader(fileName, Encoding.Default);
                text = myFile.ReadToEnd();
                myFile.Close();
                myFile.Dispose();
            }
            return text;
        }

        public static byte[] GetFileData(string fileName)
        {
            try
            {
                return File.ReadAllBytes(fileName);
            }
            catch
            {
                return null;
            }
        }
        public static byte[] GetUrlData(string url)
        {
            try
            {
                url = url.Replace("http://ecp.kingbill.com/", "http://www.kingbill.com/ecp/");
                return new WebClient().DownloadData(url);
            }
            catch
            {
                return null;
            }
        }
        public static string GetPathTimestamp ()
        {
            return DateTime.Now.Year + "." + DateTime.Now.Month + "." + DateTime.Now.Day + "." + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second;
        }

        public static void DeleteFiles(List<string> paths)
        {
            paths.ForEach(DeleteFile);
        }
        public static void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch(Exception)
            {
                var user = GetConfigValue("ImpersonatorUser");
                var domain = GetConfigValue("ImpersonatorDomain");
                var password = GetConfigValue("ImpersonatorPassword");

                using (new Impersonator(user, domain, password))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
        }

        public static void SaveTextFile(string fileName, string text)
        {
            try
            {
                File.WriteAllText(fileName, text);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public static string SanitizeFileName(string filename)
        {
            string regex = $@"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]+";
            return Regex.Replace(filename, regex, "_");
        }
        #endregion

        #region Base64 Encode / Decode
        public static string EncodeTo64(string toEncode)
        {
            return  Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncode));
        }
        public static string DecodeFrom64(string encoded)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(encoded));
        }
        public static Hashtable GetParamsMapFromBase64(string encoded)
        {
            if (encoded == null || encoded.Trim() == String.Empty)
            {
                return null;
            }
            else
            {
                string normalized = DecodeFrom64(encoded);
                if (normalized == null || normalized.Trim() == String.Empty)
                {
                    return null;
                }
                else
                {
                    var map = new Hashtable();
                    string[] paramArray = normalized.Split('&');
                    foreach (string param in paramArray)
                    {
                        string[] pair = param.Split('=');
                        if (pair.Length == 2)
                        {
                            map.Add(pair[0], pair[1]);
                        }
                    }
                    return map;
                }
            }
        }

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        public static String UTF8ByteArrayToString(Byte[] characters)
        {
            var encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        public static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            var encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        #endregion

        #region Serialization
        public static String SerializeObject(Object pObject, Type tp)
        {
            try
            {
                var memoryStream = new MemoryStream();
                var xs = new XmlSerializer(tp);
                var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                return UTF8ByteArrayToString(memoryStream.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static Object DeserializeObject(String pXmlizedString, Type tp)
        {
            var xs = new XmlSerializer(tp);
            var memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
            new XmlTextWriter(memoryStream, Encoding.UTF8);

            return xs.Deserialize(memoryStream);
        }
        #endregion

        #region Memory Stream
        public static MemoryStream ReopenMemoryStream(MemoryStream ms)
        {
            var newStream = new MemoryStream();
            var writer = new BinaryWriter(newStream);
            writer.Write(ms.ToArray());
            writer.Flush();
            newStream.Seek(0, SeekOrigin.Begin);
            return newStream;
        }

        public static void SaveMemoryStream(MemoryStream ms, string fileName)
        {
            FileStream outStream = File.OpenWrite(fileName);
            ms.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
        }
        #endregion

        #region Financial

        public static void CalculateInstallmentPlanBasedOnPaymentAmount(double totalAmount,  double installmentAmount, double annualInterestRate, int interestPeriod, out int numberOfInstallments, out double latestInstallment, out double totalInterest)
        {
            var numOfInstallments = Financial.NPer(annualInterestRate / interestPeriod, installmentAmount, -totalAmount);
            var totalAmountToPay = numOfInstallments * installmentAmount;
            numberOfInstallments = (int) numOfInstallments + 1;
            latestInstallment = totalAmountToPay - (installmentAmount * (int)numOfInstallments);
            totalInterest = totalAmountToPay - totalAmount;
        }
        #endregion
    }
}
