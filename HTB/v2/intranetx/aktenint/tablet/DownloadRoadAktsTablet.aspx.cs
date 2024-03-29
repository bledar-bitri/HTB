﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.routeplanter;
using HTB.v2.intranetx.util;
using HTBExtras.XML;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadRoadAktsTablet : System.Web.UI.Page
    {

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            string routeName = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.ROAD_NAME]);
            int routeUser = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.INKASSANT_ID]);
            bool reverseRoute = GlobalUtilArea.StringToBool(GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.REVERSE_ROUTE]));
            var aktIntIds = new List<string>();
            try
            {
                RoutePlanerManager rpManager = FileSerializer<RoutePlanerManager>.DeSerialize(RoutePlanerManager.GetRouteFilePath(routeUser, routeName));
                rpManager.IsReverse = reverseRoute;
                if (rpManager != null)
                {
                    var aktList = new List<XmlRoadAktRecord>();
                    string addr1 = string.Empty;
                    double totalDistance = 0;
                    long totalTimeInSeconds = 0;
                    DateTime firstAppointmentTime = rpManager.FirstAppointmentTime;
                    if (firstAppointmentTime == DateTime.MinValue)
                    {
//                        var ts = new TimeSpan(8, 0, 0);
//                        firstAppointmentTime = firstAppointmentTime.Date + ts;
                        firstAppointmentTime = DateTime.Now;
                    }
                    else
                    {
                        firstAppointmentTime = firstAppointmentTime.Subtract(new TimeSpan(0, 0, 0, GetInitialTravelTime(rpManager)));
                    }
                    int idx = 1;
                    foreach (var address in rpManager.GetTourAddresses())
                    {

                        var addr2 = address.Address;
                        var rec = new XmlRoadAktRecord();
                        var akt = HTBUtils.GetInterventionAktQry(address.ID);
                        rec.Index = address.ID == -1 ? "" : idx.ToString(CultureInfo.InvariantCulture);
                        rec.Address = address.Address;
                        rec.Akt = address.ID == -1 ? "" : address.ID.ToString(CultureInfo.InvariantCulture);
                        rec.Klient = GetKlientInfo(akt);
                        rec.Gegner = GetGegnerInfo(akt);
                        aktIntIds.Add(address.ID.ToString(CultureInfo.InvariantCulture));

                        if (addr1 != string.Empty && addr2 != string.Empty)
                        {
                            var road = rpManager.GetRoadBetweenAddresses(addr1, addr2);
                            totalDistance += road.Distance;
                            totalTimeInSeconds += road.TravelTimeInSeconds;
                            if (road != null)
                            {
                                firstAppointmentTime = firstAppointmentTime.AddSeconds(road.TravelTimeInSeconds);
                                rec.Distance = road.Distance.ToString("N2") + " km";
                                rec.Time = GetTimeString(road.TravelTimeInSeconds);
                                rec.TotalDistance = totalDistance.ToString("N2") + " km";
                                rec.TotalTime = GetTimeString(totalTimeInSeconds);
                                rec.ExampleTime = firstAppointmentTime.ToShortTimeString();
                                rec.AktStatus = akt.AktIntStatus.ToString();
                                rec.Latitude = akt.GegnerLatitude.ToString().Replace(",", ".");
                                rec.Longitude = akt.GegnerLongitude.ToString().Replace(",", ".");
                                if (!string.IsNullOrEmpty(akt.AktIntAutoName))
                                    rec.AktCarInfo = akt.AktIntAutoName.Length > 15 ? akt.AktIntAutoName.Substring(0, 15) : akt.AktIntAutoName;
                                if (!string.IsNullOrEmpty(akt.AktIntAutoKZ))
                                    rec.AktCarLicensePlate = akt.AktIntAutoKZ;
                                if (!string.IsNullOrEmpty(akt.AktIntAZ))
                                    rec.AktAZ = akt.AktIntAZ;
                                

                                firstAppointmentTime = firstAppointmentTime.AddSeconds(RoutePlanerManager.ADGegnerStopMinutes * 60);
                                totalTimeInSeconds += RoutePlanerManager.ADGegnerStopMinutes * 60;
                            }
                            aktList.Add(rec);
                            if (address.ID != -1) idx++;
                        }
                        else
                        {
                            rec.ExampleTime = firstAppointmentTime.ToShortTimeString();
                            aktList.Add(rec);
                            if (address.ID != -1) idx++;
                        }
                        foreach (var othAktId in address.OtherIds)
                        {
                            if (othAktId > 0)
                            {
                                rec = new XmlRoadAktRecord();
                                akt = HTBUtils.GetInterventionAktQry(othAktId);
                                aktIntIds.Add(othAktId.ToString());

                                rec.AktStatus = akt.AktIntStatus.ToString();
                                rec.Klient = GetKlientInfo(akt);
                                SetGegnerInfo(idx, akt, rec);
                                
                                if (!string.IsNullOrEmpty(akt.AktIntAutoName))
                                    rec.AktCarInfo = akt.AktIntAutoName.Length > 15 ? akt.AktIntAutoName.Substring(0, 15) : akt.AktIntAutoName;
                                if (!string.IsNullOrEmpty(akt.AktIntAutoKZ))
                                    rec.AktCarLicensePlate = akt.AktIntAutoKZ;
                                if (!string.IsNullOrEmpty(akt.AktIntAZ))
                                    rec.AktAZ = akt.AktIntAZ;

                                aktList.Add(rec);
                                if (address.ID != -1) idx++;
                            }
                        }
                        addr1 = address.Address;
                    }
                    // see if there are actions
                    var sql = new StringBuilder("SELECT convert(varchar, AktIntActionAkt) StringValue , COUNT(*) IntValue FROM tblAktenIntAction WHERE AktIntActionAkt in ");
                    sql.Append(GlobalUtilArea.GetSqlInClause(aktIntIds, false));
                    sql.Append(" GROUP BY AktIntActionAkt ");

                    ArrayList aktsWithActions = HTBUtils.GetSqlRecords(sql.ToString(), typeof (SingleValue));
                    foreach (SingleValue aktsWithAction in aktsWithActions)
                    {
                        foreach (var xmlRoadAktRecord in aktList)
                        {
                            if(aktsWithAction.StringValue.Equals(xmlRoadAktRecord.Akt))
                            {
                                if(aktsWithAction.IntValue > 0) // not necessary... just a sanity check
                                {
                                    xmlRoadAktRecord.AktHasActions = true;
                                }
                                break;
                            }
                        }
                    }


                    // Send Records
                    foreach (XmlRoadAktRecord rec in aktList)
                    {
                        Response.Write(rec.ToXmlString());
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
                Log.Error(ex);
            }
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

        private string GetKlientInfo(qryAktenInt akt)
        {
            if(akt != null)
            {
                var sb = new StringBuilder();
                if (akt.KlientName1.StartsWith("Kein Klient"))  // autoeinzug klient
                {
                    sb.Append(akt.AuftraggeberName1);
                    sb.Append(" ");
                    sb.Append(akt.AuftraggeberName2);   
                }
                else
                {
                    sb.Append(akt.KlientName1);
                    sb.Append(" ");
                    sb.Append(akt.KlientName2);
                }
                return sb.ToString();
            }
            return "";
        }


        private void SetGegnerInfo(int idx, qryAktenInt akt, XmlRoadAktRecord rec)
        {
            if(akt != null)
            {
                rec.Index = idx.ToString();
                rec.Address = akt.GegnerLastStrasse + ", " + akt.GegnerLastZip + " " + akt.GegnerLastOrt;
                rec.Akt = akt.AktIntID.ToString();
                rec.Gegner = GetGegnerInfo(akt);   
            }
        }

        private string GetGegnerInfo(qryAktenInt akt)
        {
            if (akt != null)
            {
                var sb = new StringBuilder();
                sb.Append(akt.GegnerLastName1);
                sb.Append(" ");
                sb.Append(akt.GegnerLastName2);
                sb.Append("\n");
                sb.Append(akt.GegnerPhoneCity);
                sb.Append(" ");
                sb.Append(akt.GegnerPhone);

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
            var sb = new StringBuilder(" ");
            int h = GetHours(sec);
            int m = GetMins(sec - (h*3600));
            if (h > 0)
                sb.Append(h + " Std. ");
            if (m > 0)
                sb.Append(m + " Min.");
            if (h == 0 && m == 0)
                sb.Append(sec + "Sec.");
            return sb.ToString();
        }
    }
}