using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBExtras.XML;
using HTBUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadChangedAktsTablet : System.Web.UI.Page
    {
       private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            var sp = new Stopwatch();
            sp.Start();
            var retIds = new List<StringValueRec>();
            try
            {
                string xmlData = GlobalUtilArea.GetXmlData(Request);
                if (xmlData != null)
                {
                    var records = GetRecords(xmlData);
                    var ids = records.Select(rec => rec.AktIntId.ToString()).ToList();
                    var tsList = HTBUtils.GetSqlRecords("SELECT AktIntId[IntValue], AktIntTimestamp [LongValue] FROM tblAktenInt WHERE AktIntID in " + GlobalUtilArea.GetSqlInClause(ids, false), typeof (SingleValue));
                    
                    foreach (SingleValue sv in tsList)
                    {
                        XmlChangedAktsRequestRecord foundRec = records.FirstOrDefault(changedRec => changedRec.AktIntId == sv.IntValue);
                        if (foundRec != null)
                        {
                            if (!foundRec.AktIntTimestamp.Equals(sv.LongValue.ToString()))
                            {
                                retIds.Add(new StringValueRec("", foundRec.AktIntId.ToString()));
                                records.Remove(foundRec);
                            }
                        }
                    }
                    if (records.Count > 0)
                    {
                        ids = records.Select(rec => rec.AktIntId.ToString()).ToList();
                        ArrayList qryAktList = HTBUtils.GetSqlRecords("SELECT * FROM qryAktenInt WHERE AktIntID in " + GlobalUtilArea.GetSqlInClause(ids, false), typeof (qryAktenInt));
                        foreach (qryAktenInt qryAkt in qryAktList)
                        {
                            if (qryAkt != null)
                            {
                                XmlChangedAktsRequestRecord foundRec = records.FirstOrDefault(changedRec => changedRec.AktIntId == qryAkt.AktIntID);
                                if (foundRec != null)
                                {
                                    if (!HTBUtils.AlmostEqual(AktInterventionUtils.GetAktAmounts(qryAkt).GetTotal(), foundRec.AktIntBalance))
                                    {
                                        retIds.Add(new StringValueRec("", qryAkt.AktIntID.ToString(CultureInfo.InvariantCulture)));
                                        records.Remove(foundRec);
                                    }
                                    else if (!AreDocTimeStampsEqual(qryAkt, foundRec.DocTimeStampsList.Cast<StringValueRec>().ToList()))
                                    {
                                        retIds.Add(new StringValueRec("", qryAkt.AktIntID.ToString(CultureInfo.InvariantCulture)));
                                        records.Remove(foundRec);
                                    }
                                    else if (!AreActionsTimeStampCountsEqual(qryAkt, foundRec.ActionsTimeStampsList.Cast<StringValueRec>().ToList()))
                                    {
                                        retIds.Add(new StringValueRec("", qryAkt.AktIntID.ToString()));
                                        records.Remove(foundRec);
                                    }
                                    else if (!AreGegnerPhoneTimeStampsEqual(qryAkt, foundRec.PhoneTimeStampsList.Cast<StringValueRec>().ToList()))
                                    {
//                                        retIds.Add(new StringValueRec("", qryAkt.AktIntID.ToString()));
//                                        records.Remove(foundRec);
                                    }
                                }
                            }
                        }
                    }
                    //                    Log.Info(xmlData);
                }
            }
            catch(Exception ex)
            {
                Response.Write(GlobalHtmlParams.ResponseError+" "+ex.Message);
            }
            var response = new XmlChangedAktsResponseRecord();
            response.SetAktIntIds(retIds);
            Response.Write(response.ToXmlString());

//            Log.Warn("Done: [" + sp.ElapsedMilliseconds + " mils] [" + response.ToXmlString()+"]");
            Log.Info("Done: [" + sp.ElapsedMilliseconds + " mils]");

        }

        private bool AreDocTimeStampsEqual(qryAktenInt qryAkt, List<StringValueRec> timeStamps)
        {
            if (timeStamps == null)
                return true;
            var docTimestampsList = HTBUtils.GetSqlRecords("SELECT DokTimestamp [LongValue] FROM qryDoksIntAkten WHERE AktIntID = " + qryAkt.AktIntID, typeof(SingleValue));
            if (qryAkt.IsInkasso())
            {
                HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT DokTimestamp [LongValue] FROM qryDoksInkAkten WHERE CustInkAktID = " + qryAkt.AktIntCustInkAktID, typeof(SingleValue)), docTimestampsList);
            }

            return AreTimestampsEqual(docTimestampsList, timeStamps);
        }

        private bool AreActionsTimeStampCountsEqual(qryAktenInt qryAkt, List<StringValueRec> timeStamps)
        {
            if (timeStamps == null)
                return true;
            var actions = GlobalUtilArea.GetInterventionActions(qryAkt.AktIntAuftraggeber, qryAkt.AktIntAktType, Session);
            return actions.Count == timeStamps.Count;
        }

        private bool AreGegnerPhoneTimeStampsEqual(qryAktenInt qryAkt, List<StringValueRec> timeStamps)
        {
            if (timeStamps == null)
                return true;
            var gegnerPhoneTimestampsList = HTBUtils.GetSqlRecords("SELECT GPhoneTimestamp [LongValue] FROM tblGegnerPhone where GPhoneGegnerID =  " + qryAkt.GegnerID, typeof(SingleValue));


            return AreTimestampsEqual(gegnerPhoneTimestampsList, timeStamps);
        }

        private static bool AreTimestampsEqual(ArrayList list, List<StringValueRec> timestamps)
        {
            return list.Count == timestamps.Count && (from SingleValue sv in list select timestamps.Any(timestamp => sv.LongValue.ToString(CultureInfo.InvariantCulture) == timestamp.Value)).All(found => found);
        }


        private List<XmlChangedAktsRequestRecord> GetRecords(string xmlData)
        {
            var ds = new DataSet();
            ds.ReadXml(new StringReader(xmlData));
            var records = new List<XmlChangedAktsRequestRecord>();

            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName != null)
                {
                    tbl.TableName = tbl.TableName.Trim();
                    foreach (DataRow dr in tbl.Rows)
                    {
                        if (tbl.TableName.Equals("XmlChangedAktsRequestRecord", StringComparison.InvariantCultureIgnoreCase)) {
                            var rec = new XmlChangedAktsRequestRecord();
                            rec.LoadFromDataRow(dr);
                            records.Add(rec);
                        }
                        else if (tbl.TableName.Equals("StringValueRec", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var rec = new StringValueRec("","");
                            rec.LoadFromDataRow(dr);
                            if(rec.Name != null)
                            {
                                rec.Name = rec.Name.Trim();
                                string[] id_type = rec.Name.Split();
                                if (id_type.Length == 2)
                                {
                                    string id = id_type[0];
                                    string type = id_type[1];
                                    XmlChangedAktsRequestRecord foundRec = records.FirstOrDefault(changedRec => changedRec.AktIntId.ToString().Equals(id));
                                    if(foundRec != null)
                                    {
                                        switch (type)
                                        {
                                            case "DT":
                                                foundRec.DocTimeStampsList.Add(rec);
                                                break;
                                            case "PT":
                                                foundRec.PhoneTimeStampsList.Add(rec);
                                                break;
                                            case "AT":
                                                foundRec.AddressTimeStampsList.Add(rec);
                                                break;
                                            case "ACT":
                                                foundRec.ActionsTimeStampsList.Add(rec);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return records;
        }


    }
}
