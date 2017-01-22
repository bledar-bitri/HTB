using System;
using HTB.Database.Views;
using HTB.Database;

namespace HTBExtras
{
    public class ActionRecord : Record
    {

        public int ActionID { get; set; }
        public int ActionTypeId { get; set; }
        public int AktIntId { get; set; }
        public string ActionNumber { get; set; }
        public string ActionCaption { get; set; }
        public string ActionMemo { get; set; }
        public DateTime ActionDate { get; set; }
        public double ActionLatitude { get; set; }
        public double ActionLongitude { get; set; }

        public int RecordStatus { get; set; }
        
        public ActionRecord()
        {
            ActionCaption = string.Empty;
        }

        public ActionRecord(Record action)
        {
            ActionCaption = string.Empty;
            if (action is qryAuftraggeberAktTypeAction) Assign((qryAuftraggeberAktTypeAction)action);
            else if (action is qryAktTypeAction) Assign((qryAktTypeAction)action);
            else if (action is qryAuftraggeberAction) Assign((qryAuftraggeberAction)action);
            else if (action is tblAktenIntActionType) Assign((tblAktenIntActionType)action);
            else if (action is tblAktenIntAction) Assign((tblAktenIntAction)action);
            else throw new Exception("New ActionRecord: Invalid Record Class. Valid Types: [qryAuftraggeberAktTypeAction] [qryAktTypeAction] [qryAuftraggeberAction] [tblAktenIntActionType]");
        }

        public void Assign(qryAuftraggeberAktTypeAction action)
        {
            ActionID = action.AGAktionTypeAktIntActionTypeID;
            ActionCaption = action.AktIntActionTypeCaption;
        }
        public void Assign(qryAktTypeAction action)
        {
            ActionID = action.AktTypeActionAktIntActionTypeID;
            ActionCaption = action.AktIntActionTypeCaption;
        }
        public void Assign(qryAuftraggeberAction action)
        {
            ActionID = action.AGAktionAktIntActionTypeID;
            ActionCaption = action.AktIntActionTypeCaption;
        }
        public void Assign(tblAktenIntActionType action)
        {
            ActionID = action.AktIntActionTypeID;
            ActionCaption = action.AktIntActionTypeCaption;
        }
        public void Assign(tblAktenIntAction action)
        {
            ActionID = action.AktIntActionType;
            ActionMemo = action.AktIntActionMemo;
        }
    }
}