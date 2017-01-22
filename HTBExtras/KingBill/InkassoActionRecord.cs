using System;
using System.Text;
using HTB.Database;
using HTB.Database.Views;
using HTBUtilities;

namespace HTBExtras.KingBill
{
    public class InkassoActionRecord : Record
    {
        public int ActionID { get; set; }
        public string ActionCaption { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; }
        public string ActionBeleg { get; set; }
        public string ActionHonorar { get; set; }
        public string ActionProvision { get; set; }
        public string ActionBetrag { get; set; }
        public string ActionMemo { get; set; }
        public string ActionUser { get; set; }
        public string DeleteActionURL { get; set; }
        public string EditActionURL { get; set; }
        public string InterventionMemo { get; set; }
        public string InverventionURL { get; set; }
        public string MeldeInfo { get; set; }
        public string MeldeURL { get; set; }
        public bool IsInkassoAction { get; set; }
        public bool IsOnlyMemo { get; set; }
        public double AktIntActionLatitude { get; set; }
        public double AktIntActionLongitude { get; set; }
        public string ActionAddress { get; set; }
        
        public InkassoActionRecord()
        {
            ActionID = 0;
            ActionCaption = string.Empty;
            ActionType = string.Empty;
            ActionBeleg = string.Empty;
            ActionHonorar = string.Empty;
            ActionProvision = string.Empty;
            ActionBetrag = string.Empty;
            ActionMemo = string.Empty;
            ActionUser = string.Empty;
            DeleteActionURL = string.Empty;
            EditActionURL = string.Empty;
            InterventionMemo = string.Empty;
            InverventionURL = string.Empty;
            MeldeInfo = string.Empty;
            MeldeURL = string.Empty;
            AktIntActionLatitude = 0;
            AktIntActionLongitude = 0;
            ActionAddress = string.Empty;
        }

        public InkassoActionRecord(qryCustInkAktAktionen action, tblAktenInt intAkt)
        {
            ActionID = 0;
            ActionCaption = string.Empty;
            ActionType = string.Empty;
            ActionBeleg = string.Empty;
            ActionHonorar = string.Empty;
            ActionProvision = string.Empty;
            ActionBetrag = string.Empty;
            ActionMemo = string.Empty;
            ActionUser = string.Empty;
            DeleteActionURL = string.Empty;
            EditActionURL = string.Empty;
            InterventionMemo = string.Empty;
            InverventionURL = string.Empty;
            MeldeInfo = string.Empty;
            MeldeURL = string.Empty;
            Assign(action);
            Assign(intAkt);
            IsInkassoAction = true;
        }
        
        public InkassoActionRecord(qryInktAktAction action, tblAktenInt intAkt)
        {
            ActionID = 0;
            ActionCaption = string.Empty;
            ActionType = string.Empty;
            ActionBeleg = string.Empty;
            ActionHonorar = string.Empty;
            ActionProvision = string.Empty;
            ActionBetrag = string.Empty;
            ActionMemo = string.Empty;
            ActionUser = string.Empty;
            DeleteActionURL = string.Empty;
            EditActionURL = string.Empty;
            InterventionMemo = string.Empty;
            InverventionURL = string.Empty;
            MeldeInfo = string.Empty;
            MeldeURL = string.Empty;
            Assign(action);
            Assign(intAkt);
            IsInkassoAction = false;
            ActionAddress = action.AktIntActionAddress;
        }

        public InkassoActionRecord(qryMeldeResult melde, tblAktenInt intAkt, bool showLink = true)
        {
            ActionID = 0;
            ActionCaption = string.Empty;
            ActionType = string.Empty;
            ActionBeleg = string.Empty;
            ActionHonorar = string.Empty;
            ActionProvision = string.Empty;
            ActionBetrag = string.Empty;
            ActionMemo = string.Empty;
            ActionUser = string.Empty;
            DeleteActionURL = string.Empty;
            EditActionURL = string.Empty;
            InterventionMemo = string.Empty;
            InverventionURL = string.Empty;
            MeldeInfo = string.Empty;
            MeldeURL = string.Empty;
            Assign(melde, showLink);
            Assign(intAkt);
            IsInkassoAction = false;
        }

        public void Assign(qryCustInkAktAktionen action)
        {
            ActionID = action.CustInkAktAktionID;
            ActionDate = action.CustInkAktAktionDate;
            ActionType = action.KZKZ;
            ActionCaption = action.CustInkAktAktionCaption;
            ActionMemo = action.CustInkAktAktionMemo.Replace(Environment.NewLine, "<BR/>");
            ActionUser = string.IsNullOrEmpty(action.UserVorname) ? "" : action.UserVorname + " " + action.UserNachname;
            EditActionURL = "EditInkAction.aspx?ID=" + action.CustInkAktAktionID;

            #region Delete action url
            StringBuilder sb = new StringBuilder("../global_forms/GlobalDelete.aspx?titel=Position%20löschen&amp;frage=Sie%20sind%20dabei%20diese%20Position%20zu%20löschen:&amp;strTable=tblCustInkAktAktion&amp;strTextField=");
            if (action.CustInkAktAktionCaption != null && action.CustInkAktAktionCaption.Trim() != string.Empty)
                sb.Append("CustInkAktAktionCaption");
            else
                sb.Append("CustInkAktAktionMemo");

            sb.Append("&amp;strColumn=CustInkAktAktionID&amp;ID=");
            sb.Append(action.CustInkAktAktionID);
            DeleteActionURL = sb.ToString();
            #endregion
        }

        private void Assign(qryInktAktAction action)
        {
            ActionID = action.AktIntActionID;
            ActionDate = action.AktIntActionTime;
            ActionCaption = action.AktIntActionTypeCaption;
            ActionBeleg = action.AktIntActionBeleg;
            ActionHonorar = HTBUtils.FormatCurrency(action.AktIntActionHonorar);
            ActionProvision = HTBUtils.FormatCurrency(action.AktIntActionProvision);
            ActionBetrag = HTBUtils.FormatCurrency(action.AktIntActionBetrag);
            AktIntActionLongitude = action.AktIntActionLongitude;
            AktIntActionLatitude = action.AktIntActionLatitude;
        }

        private void Assign(tblAktenInt intAkt)
        {
            if (intAkt != null)
            {
                InterventionMemo = intAkt.AKTIntMemo.Replace(Environment.NewLine, "<BR/>");
                InverventionURL = "<a href=\"../aktenint/workaktint.aspx?ID=" + intAkt.AktIntID + "\">" + intAkt.AktIntID + "</a>";
            }
        }

        private void Assign(qryMeldeResult melde, bool showLink)
        {
            if (melde != null)
            {
                MeldeInfo = melde.AMBericht.Replace(Environment.NewLine, "<BR/>");
                MeldeURL = "<a href=\"/v2/intranet/melde/editmelde.asp?ID=" + melde.AMID + "\">" + melde.AMID + "</a>";

                ActionID = 0;
                ActionDate = melde.AMUebergabeDatum;
                ActionCaption = "Meldeergebnis: " + melde.MeldeInfoAuswahlCaption + (showLink ? ":  " + MeldeURL : "");
                ActionType = "MEL_RET";
            }
        }
    }
}