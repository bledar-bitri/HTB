using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTB.Database;
using System.Collections;
using HTBUtilities;
using System.Data;
using HTBAktLayer;
using System.Text;
using HTB.v2.intranetx.util;
using System.IO;
using HTB.Database.Views;
using HTBServices;
using HTBServices.Mail;

namespace HTB.v2.intranetx.auftraggeber
{
    public partial class AuftraggeberExtension : System.Web.UI.Page
    {
        ArrayList extList = new ArrayList();
        ArrayList sbExtList = new ArrayList();
        tblAuftraggeber auftraggeber;

        string agId = "";
        string agSB = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            string base64Params = Request["params"];
            if (base64Params != null && base64Params.Trim() != string.Empty)
            {
                Hashtable paramsMap = HTBUtils.GetParamsMapFromBase64(base64Params);
                if (paramsMap != null)
                {
                    //ctlMessage.ShowInfo("PARAMS: <BR/>");
                    //foreach (string key in paramsMap.Keys)
                    //{
                    //    ctlMessage.AppendInfo(key + " --> " + paramsMap[key] + "<BR/>");
                    //}
                    agId = (string)paramsMap["AG"];
                    agSB = (string)paramsMap["AGSB"];
                    if (agId != null && agId.Trim() != string.Empty && agSB != null && agSB.Trim() != string.Empty)
                    {
                        if (!IsPostBack)
                        {
                            LoadExtensionList();
                            auftraggeber = (tblAuftraggeber)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAuftraggeber WHERE AuftraggeberID = " + agId, typeof(tblAuftraggeber));
                            SetValues();
                        }
                    }
                }
                //else
                //{
                //    ctlMessage.ShowInfo("NO PARAMS: <BR/>");
                //}
            }
        }

        private void SetValues()
        {
            if (auftraggeber != null)
            {
                lblAuftraggeber.Text = auftraggeber.AuftraggeberAnrede + " " + auftraggeber.AuftraggeberName1 + " " + auftraggeber.AuftraggeberName2;
                lblAGSB.Text = agSB;
                PopulateExtensionGrid();
            }
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                NotificatySB();
                LoadExtensionList();
                PopulateExtensionGrid();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            
        }
        #endregion
        
        private bool IsEntryValid()
        {
            bool actionPerfomed = false;
            for (int i = 0; i < gvExtension.Rows.Count; i++)
            {
                GridViewRow row = gvExtension.Rows[i];
                CheckBox chkApprove = (CheckBox)row.FindControl("chkApprove");
                CheckBox chkDeny = (CheckBox)row.FindControl("chkDeny");
                if (chkApprove.Checked && chkDeny.Checked)
                {
                    ctlMessage.ShowError("Sie d&uuml;rfen nicht beide [Genehmigen und Nicht Genehmigen] den gleichen Akt");
                    return false;
                }
                else if (chkApprove.Checked || chkDeny.Checked)
                {
                    actionPerfomed = true;
                }
            }
            if (!actionPerfomed)
            {
                ctlMessage.ShowError("Kein Aktion wurde eingegeben.");
                return false;
            }
            return true;
        }
        private bool Save()
        {
            if (!IsEntryValid())
            {
                return false;
            }
            else 
            {
                try
                {
                    var set = new RecordSet();
                    for (int i = 0; i < gvExtension.Rows.Count; i++)
                    {
                        GridViewRow row = gvExtension.Rows[i];
                        CheckBox chkApprove = (CheckBox)row.FindControl("chkApprove");
                        CheckBox chkDeny = (CheckBox)row.FindControl("chkDeny");
                        if (chkApprove.Checked && chkDeny.Checked)
                        {
                            ctlMessage.ShowError("Sie d&uuml;rfen nicht beide [Genehmigen und Nicht Genehmigen] den gleichen Akt");
                        }
                        else if (chkApprove.Checked || chkDeny.Checked)
                        {
                            Label lblExtId = (Label)row.FindControl("lblExtId");
                            TextBox txtMemo = (TextBox)row.FindControl("txtMemo");

                            tblAktenIntExtension ext = (tblAktenIntExtension)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntExtension WHERE AktIntExtID = " + lblExtId.Text, typeof(tblAktenIntExtension));
                            if (ext != null)
                            {
                                var aktInt = (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenInt WHERE AktIntID = " + ext.AktIntExtAkt, typeof(tblAktenInt));
                                if (aktInt != null)
                                {
                                    GetSBExtension(aktInt.AktIntSB).Add(ext.AktIntExtID);
                                    if (chkApprove.Checked)
                                    {
                                        ext.AktIntExtApprovedDate = DateTime.Now;
                                        aktInt.AktIntTerminAD = aktInt.AktIntTerminAD.AddDays(ext.AktIntExtRequestDays);
                                        aktInt.AktIntTermin = aktInt.AktIntTermin.AddDays(ext.AktIntExtRequestDays);
                                    }
                                    else if (chkDeny.Checked)
                                    {
                                        ext.AktIntExtDeniedDate = DateTime.Now;
                                    }
                                    ext.AktIntExtMemo = txtMemo.Text;

                                    set.UpdateRecord(ext);
                                    set.UpdateRecord(aktInt);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ctlMessage.ShowError(e.Message);
                    ctlMessage.AppendError("<BR>" + e.StackTrace);
                    return false;
                }
                return true;
            }
        }
        private double GetClientBalance(qryAktenIntExtension ext)
        {
            if (ext.IsInkassoAkt())
            {
                AktUtils utils = new AktUtils(ext.AktIntCustInkAktID);
                return utils.GetAktKlientTotalBalance();
            }
            else
            {
                double ret = 0;
                ArrayList posList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + ext.AktIntCustInkAktID, typeof(tblAktenIntPos));
                foreach (tblAktenIntPos AktIntPos in posList)
                {
                    ret += AktIntPos.AktIntPosBetrag;
                }
                ret += ext.AKTIntZinsenBetrag;
                ret += ext.AKTIntKosten;
                return ret;
            }
        }

        private void LoadExtensionList()
        {
            string where = " WHERE AuftraggeberID = " + agId + " and AKTIntAGSB = '" + agSB + "'" + 
                " AND AktIntExtApprovedDate = '01.01.1900' AND AktIntExtDeniedDate = '01.01.1900'";
            extList = HTBUtils.GetSqlRecords("SELECT * FROM qryAktenIntExtension " + where + "  ORDER BY AktIntExtRequestDate ", typeof(qryAktenIntExtension));
        }
        private SBExtensionRecord GetSBExtension(int sbId)
        {
            foreach(SBExtensionRecord rec in sbExtList)
                if (sbId == rec.sbId)
                    return rec;
            
            SBExtensionRecord newRec = new SBExtensionRecord(sbId);
            sbExtList.Add(newRec);
            return newRec;
        }

        private void NotificatySB()
        {
            foreach (SBExtensionRecord rec in sbExtList)
            {
                string inStr = rec.GetInString();
                if (inStr != null)
                {
                    var user = (tblUser)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserID = " + rec.sbId, typeof(tblUser));
                    if (user != null)
                    {
                        string emailBody = GetSBEmailBody(rec, user);
                        if (emailBody != null)
                        {
                            var toList =
                                HTBUtils.GetValidEmailAddressesFromStrings(new[]
                                    {
                                        user.UserEMailOffice,
                                        user.UserEMailPrivate,
                                        HTBUtils.GetConfigValue("AG_Extenssion_CC_EMail_Addr")
                                    });
                            var email = ServiceFactory.Instance.GetService<IHTBEmail>();
                            email.SendGenericEmail(toList.ToArray(), HTBUtils.GetConfigValue("SB_Extension_Email_Subject"), emailBody, true);
                        }
                    }
                }
            }
        }

        private string GetSBEmailBody(SBExtensionRecord rec, tblUser user)
        {
            if (user == null)
                return null;

            StringBuilder sbText = new StringBuilder();
            StreamReader re = File.OpenText(HTBUtils.GetConfigValue("SB_Extension_Text"));
            string input = null;
            while ((input = re.ReadLine()) != null)
            {
                sbText.Append(input);
            }
            re.Close();
            re.Dispose();

            StringBuilder sb = new StringBuilder();
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM qryAktenIntExtension WHERE AktIntExtID IN (" + rec.GetInString() + ") ORDER BY AktIntExtDeniedDate", typeof(qryAktenIntExtension));
            foreach (qryAktenIntExtension ext in list)
            {
                sb.Append("<TR><TD class=\"tblData2\" align=\"center\">");
                sb.Append(ext.AktIntAZ.ToString());
                sb.Append("</TD><TD class=\"tblData2\">");
                sb.Append(GetGegnerInfo(ext));
                sb.Append("</TD><TD class=\"tblData2\" align=\"center\">");
                if (HTBUtils.IsDateValid(ext.AktIntExtApprovedDate))
                {
                    sb.Append("JA");
                    sb.Append("<BR/> ehem. Termin: ");
                    sb.Append(ext.AktIntTermin.AddDays(-ext.AktIntExtRequestDays).ToShortDateString());
                    sb.Append("<BR/> neuer. Termin: ");
                    sb.Append(ext.AktIntTermin.ToShortDateString());
                }
                else
                {
                    sb.Append("NEIN");
                }
                sb.Append("</TD><TD class=\"tblData2\">");
                sb.Append(ext.AktIntExtMemo);
                sb.Append("</TD></TR>");
            }
            string sbName = user.UserVorname + " " + user.UserNachname;
            if (user.UserSex == 1)
            {
                sbName = "Herr " + user.UserNachname;
            }
            else if (user.UserSex == 2)
            {
                sbName = "Frau " + user.UserNachname;
            }

            return sbText.ToString().Replace("[NAME]", sbName).Replace("[TABLE_DATA]", sb.ToString());
        }


        private string GetGegnerInfo(qryAktenIntExtension extension)
        {
            return extension.GegnerAnrede + " " + extension.GegnerLastName1 + " " + extension.GegnerLastName2 + "<BR/>" +
                       extension.GegnerLastStrasse + "<BR/>" +
                       extension.GegnerLastZipPrefix + " - " + extension.GegnerLastZip + " " + extension.GegnerLastOrt;
        }

        #region Extension Grid
        private void PopulateExtensionGrid()
        {
            DataTable dt = GetKostenDataTableStructure();
            
            foreach (qryAktenIntExtension extension in extList)
            {
                DataRow dr = dt.NewRow();
                {
                    dr["ExtId"] = extension.AktIntExtID;
                    dr["AktAZ"] = extension.AktIntAZ.ToString();
                    dr["CrrDueDate"] = extension.AktIntTerminAD.ToShortDateString();
                    dr["ExtenssionDays"] = extension.AktIntExtRequestDays.ToString();
                    dr["NextDueDate"] = extension.AktIntTerminAD.AddDays(extension.AktIntExtRequestDays).ToShortDateString();
                    dr["Aprove"] = HTBUtils.IsDateValid(extension.AktIntExtApprovedDate);
                    dr["Deny"] = HTBUtils.IsDateValid(extension.AktIntExtDeniedDate);
                    dr["Memo"] = extension.AktIntExtMemo;
                    dr["Gegner"] = GetGegnerInfo(extension);
                    dr["TotalDue"] = HTBUtils.FormatCurrency(GetClientBalance(extension));
                    dr["Reason"] = extension.AktIntExtRequestReason;
                    dt.Rows.Add(dr);
                }
            }
            gvExtension.DataSource = dt;
            gvExtension.DataBind();
        }
        private DataTable GetKostenDataTableStructure()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("ExtId", typeof(int)));
            dt.Columns.Add(new DataColumn("AktAZ", typeof(string)));
            dt.Columns.Add(new DataColumn("Gegner", typeof(string))); 
            dt.Columns.Add(new DataColumn("CrrDueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("ExtenssionDays", typeof(string)));
            dt.Columns.Add(new DataColumn("NextDueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Aprove", typeof(bool)));
            dt.Columns.Add(new DataColumn("Deny", typeof(bool)));
            dt.Columns.Add(new DataColumn("Memo", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalDue", typeof(string)));
            dt.Columns.Add(new DataColumn("Reason", typeof(string)));
            return dt;
        }
        #endregion
    }

    class SBExtensionRecord
    {
        public int sbId = 0;
        public ArrayList extensionList = new ArrayList();
        
        public SBExtensionRecord(int sb)
        {
            sbId = sb;
        }

        public SBExtensionRecord()
        {}

        public void Clear()
        {
            extensionList.Clear();
            sbId = 0;
        }

        public void Add(int extensionId)
        {
            extensionList.Add(extensionId);
        }

        public string GetInString()
        {
            if (extensionList.Count == 0) 
                return null;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < extensionList.Count; i++)
            {
                sb.Append(extensionList[i]);
                if (i < extensionList.Count - 1)
                    sb.Append(",");
            }
            return sb.ToString();
        }
    }
}