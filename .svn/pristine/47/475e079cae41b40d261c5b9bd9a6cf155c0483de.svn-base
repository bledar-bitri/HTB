using System;
using HTB.Database;

namespace HTBExtras.KingBill
{
    public class Aktion : Record
    {
        public int AktionID { get; set; }
        public string AktionBeschreibung { get; set; }
        public DateTime AktionDatum { get; set; }
        public string AktionTyp { get; set; }
        public string AktionMemo { get; set; }
        public double AktionLatitude { get; set; }
        public double AktionLongitude { get; set; }
        public string AktionAdress { get; set; }

        public Aktion()
        {
            AktionID = 0;
            AktionBeschreibung = string.Empty;
            AktionTyp = string.Empty;
            AktionMemo = string.Empty;

            AktionLatitude = 0;
            AktionLongitude = 0;
            AktionAdress = string.Empty;
        }

        public Aktion(InkassoActionRecord action)
            : base()
        {
            Assign(action);
        }


        public void Assign(InkassoActionRecord action)
        {
            AktionID = action.ActionID;
            AktionDatum = action.ActionDate;
            AktionTyp = action.ActionType;
            AktionBeschreibung = action.ActionCaption;
            AktionMemo = action.ActionMemo;
            AktionLatitude = action.AktIntActionLatitude;
            AktionLongitude = action.AktIntActionLongitude;
            AktionAdress = action.ActionAddress;
        }
    }
}
