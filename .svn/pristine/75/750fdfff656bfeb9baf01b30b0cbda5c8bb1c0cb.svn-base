using System;
using System.Text;
using System.Collections;
using HTBUtilities;
using HTB.Database;

namespace HTB.v2.intranetx.aktenint
{
    public partial class WeeklyValidationOrg : System.Web.UI.Page
    {
        DateTime _startDate = DateTime.Now;
        DateTime _endDate = DateTime.Now;
        readonly DateTime _today = DateTime.Now;      // Normal
//        readonly DateTime _today = DateTime.Now.AddDays(-1);    // Tuesday
        readonly ArrayList _statusAktList = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 3600 * 3; // hours
            DayOfWeek day = _today.DayOfWeek;
            if (day == DayOfWeek.Monday)
            {
                _startDate = _today.AddDays(-6);
            }
            else
            {
                int days = day - DayOfWeek.Tuesday;
                _startDate = _today.AddDays(-days);
            }
            _endDate = _today;
            ArrayList aktList = GetAkts();
            ShowMessage(GetMessage(aktList));
        }
        private bool IsAktProcessible(StringBuilder sb, tblAktenInt akt)
        {
            string sqlQuery = "SELECT * FROM tblAktenIntAction A inner join tblAktenIntActionType t on A.AktIntActionType = t.AktIntActionTypeID WHERE t.AktIntActionIsExtensionRequest = 0 "+
                " And AktIntActionAkt = " + akt.AktIntID + " AND AktIntActionDate between '" + _startDate.ToShortDateString() + "' AND '" + _endDate.ToShortDateString() + " 23:59:59'";

            ArrayList aktionsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(tblAktenIntAction));
            if (akt.AktIntStatus == 2 || akt.AktIntStatus == 3)
            {
                if (aktionsList.Count == 0)
                {
                    sb.Append("<BR>");
                    sb.Append(akt.AktIntID);
                    sb.Append("&nbsp;&nbsp;&nbsp;&nbsp; ");
                    if (akt.AktIntStatus == 2) 
                        sb.Append("Abgegeben ");
                    else
                        sb.Append("Fertig ");
                    
                    sb.Append("ohne Aktion!");
                }
            }
            return aktionsList.Count > 0;
        }
        private string IsAktValid(tblAktenInt akt)
        {
            string sqlQuery = "SELECT * FROM tblAktenIntAction WHERE AktIntActionType <> 77 AND  AktIntActionAkt = " + akt.AktIntID +
                " AND AktIntActionDate between '" + _startDate.ToShortDateString() + "' AND '" + _endDate.ToShortDateString()+"'";

            ArrayList aktionsList = HTBUtils.GetSqlRecords(sqlQuery, typeof(tblAktenIntAction));
            if (aktionsList.Count > 1)
            {
                return "Mehr als 1 Aktionen";
            }
            return null;
        }
        
        private void ProcessAkt(StringBuilder sb, tblAktenInt akt)
        {
            bool ok = true;
            string sqlQuery = "SELECT * FROM tblAktenIntAction WHERE AktIntActionType <> 77 AND AktIntActionAkt = " + akt.AktIntID +
                " AND AktIntActionDate between '" + _startDate.ToShortDateString() + "' AND '" + _endDate.ToShortDateString() + "'";

            var aktion = (tblAktenIntAction)HTBUtils.GetSqlSingleRecord(sqlQuery, typeof(tblAktenIntAction));
            if (aktion != null)
            {
                if (!HTBUtils.IsZero(aktion.AktIntActionBetrag) || aktion.AktIntActionBeleg.Trim() != "")
                {
                    if (HTBUtils.IsZero(aktion.AktIntActionBetrag) && aktion.AktIntActionBeleg.Trim() != "")
                    {
                        sb.Append("<BR>");
                        sb.Append(akt.AktIntID);
                        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;Aktion [Betrag = 0 aber Beleg ist nicht leer!]");
                        ok = false;
                    }
                    else if (aktion.AktIntActionBetrag > 0 && aktion.AktIntActionBeleg.Trim() == "")
                    {
                        sb.Append("<BR>");
                        sb.Append(akt.AktIntID);
                        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;Aktion [Betrag > 0 aber Beleg ist leer!]");
                        ok = false;
                    }
                    else
                    {
                        double num;
                        bool isNum = double.TryParse(aktion.AktIntActionBeleg.Replace("/", "").Replace("\\", ""), out num);
                        if (!isNum)
                        {
                            string beleg = HTBUtils.RemoveAllSpecialChars(aktion.AktIntActionBeleg).Replace(" ", "").Trim().ToLower();

                            if (beleg != "zhlganikb" && beleg != "zahlunganikb")
                            {
                                sb.Append("<BR>");
                                sb.Append(akt.AktIntID);
                                sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;Aktion Beleg (falsch)");
                                ok = false;
                            }
                            else if (akt.AKTIntMemo.IndexOf("Zhlg an IKB") < 0)
                            {
                                akt.AKTIntMemo += "\nZhlg an IKB";
                            }
                        }
                    }
                }

                // Check Installments 
                if (aktion.AktIntActionType == 48 ||
                    aktion.AktIntActionType == 50 ||
                    aktion.AktIntActionType == 58 ||
                    aktion.AktIntActionType == 61 ||
                    aktion.AktIntActionType == 66 ||
                    aktion.AktIntActionType == 70 ||
                    aktion.AktIntActionType == 71)
                {
                    #region Installment Selected

                    if (akt.AKTIntRVStartDate == new DateTime(1900, 1, 1))
                    {
                        if (ok)
                        {
                            sb.Append("<BR>");
                            sb.Append(akt.AktIntID);
                        }
                        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;RV Start Date Falsch!");
                        ok = false;
                    }
                    if (akt.AKTIntRVAmmount == 0)
                    {
                        if (ok)
                        {
                            sb.Append("<BR>");
                            sb.Append(akt.AktIntID);
                        }
                        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;RV Betrag/Monat Falsch!");
                        ok = false;
                    }
                    //if (akt.AKTIntRVNoMonth == 0)
                    //{
                    //    if (ok)
                    //    {
                    //        sb.Append("<BR>");
                    //        sb.Append(akt.AktIntID);
                    //    }
                    //    sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;RV Laufzeit Falsch!");
                    //    ok = false;
                    //}
                    //if (akt.AKTIntRVIntervallDay == 0)
                    //{
                    //    if (ok)
                    //    {
                    //        sb.Append("<BR>");
                    //        sb.Append(akt.AktIntID);
                    //    }
                    //    sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;RV immer zum Falsch!");
                    //    ok = false;
                    //}
                    if (ok)
                    {
                        if (akt.AKTIntRVInkassoType == 1)
                        {
                            if (akt.AKTIntMemo.IndexOf("LFI") < 0)
                                akt.AKTIntMemo += "\nLFI";
                        }
                    }

                    #endregion
                }
                else
                {
                    #region No Installment Selected

                    if (akt.AKTIntRVStartDate != new DateTime(1900, 1, 1))
                    {
                        sb.Append("<BR>");
                        sb.Append(akt.AktIntID);
                        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;Kein RV Aktion: [RV Start Date: " + akt.AKTIntRVStartDate.ToShortDateString() + "]!");
                        ok = false;
                    }
                    if (akt.AKTIntRVAmmount != 0)
                    {
                        if (ok)
                        {
                            sb.Append("<BR>");
                            sb.Append(akt.AktIntID);
                        }
                        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;Kein RV Aktion: [RV Betrag/Monat: " + akt.AKTIntRVAmmount + "]");
                        ok = false;
                    }
                    //if (akt.AKTIntRVNoMonth != 0)
                    //{
                    //    if (ok)
                    //    {
                    //        sb.Append("<BR>");
                    //        sb.Append(akt.AktIntID);
                    //    }
                    //    sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;Kein RV Aktion: [RV Laufzeit [" + akt.AKTIntRVNoMonth + "]");
                    //    ok = false;
                    //}
                    //if (akt.AKTIntRVIntervallDay != 0)
                    //{
                    //    if (ok)
                    //    {
                    //        sb.Append("<BR>");
                    //        sb.Append(akt.AktIntID);
                    //    }
                    //    sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;Kein RV Aktion: [RV immer zum [" + akt.AKTIntRVIntervallDay + "]");
                    //    ok = false;
                    //}

                    #endregion
                }
            }
        }
        private ArrayList GetAkts()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntStatus in(1, 2, 3)", typeof(tblAktenInt));
            return list;
        }
        private string GetMessage(ArrayList aktList)
        {
            var sb = new StringBuilder();
            sb.Append("Start Date: " + _startDate.ToShortDateString());
            sb.Append("<BR>End Date: " + _endDate.ToShortDateString());
            sb.Append("<BR><BR><b>AKTEN</b><BR>");
            foreach (tblAktenInt akt in aktList)
            {
                if (IsAktProcessible(sb, akt))
                {
                    string message = IsAktValid(akt);
                    if (message != null)
                    {
                        sb.Append("<BR>");
                        sb.Append(akt.AktIntID);
                        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
                        sb.Append(message);
                    }
                    else
                    {
                        ProcessAkt(sb, akt);
                    }
                    if (akt.AktIntStatus == 1)
                    {
                        string sqlQuery = "SELECT * FROM tblAktenIntAction WHERE AktIntActionAkt = " + akt.AktIntID + " AND AktIntActionDate between '" + _startDate.ToShortDateString() + "' AND '" + _endDate.ToShortDateString() + "'";
                        string sqlOrderBy = " Order By AktIntActionDate DESC";
                        tblAktenIntAction action = (tblAktenIntAction) HTBUtils.GetSqlSingleRecord(sqlQuery + sqlOrderBy, typeof(tblAktenIntAction));
                        if (action != null)
                        {
                            tblAktenIntActionType actionType = (tblAktenIntActionType)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntActionType WHERE AktIntActionTypeID = " + action.AktIntActionType, typeof(tblAktenIntActionType));
                            if (!actionType.AktIntActionIsExtensionRequest)
                            {
                                akt.AktIntStatus = 2;
                                RecordSet.Update(akt);
                                _statusAktList.Add(akt);
                            }
                        }
                    }
                }
            }
            if (_statusAktList.Count > 0)
            {
                sb.Append("<BR><BR>Folgende Akten wurden von 'In Bearbeitung' auf 'Abgegeben'");
            }
            foreach (tblAktenInt akt in _statusAktList)
            {
                sb.Append("<BR>");
                sb.Append(akt.AktIntID);
            }
            return sb.ToString();
        }
        private void ShowMessage(string message)
        {
            lblMsg.Text = message;
        }
    }
}