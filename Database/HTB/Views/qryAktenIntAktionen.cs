using System;

namespace HTB.Database.Views
{
    public class qryAktenIntAktionen : tblAktenIntAction
    {
        #region Property Declaration tblAktenInt

        public int AktIntAuftraggeber { get; set; }

        public DateTime AktIntDatum { get; set; }

        public int AktIntSB { get; set; }

        public int AktIntAktType { get; set; }

        public int AktIntCustInkAktID { get; set; }

        #endregion


        #region Property Declaration tblAktenIntActionTypeNextStep

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int ActionTypeNextStepID { get; set; }

        public string ActionTypeNextStepCaption { get; set; }
        
        public int ActionTypeNextStepCode { get; set; }

        #endregion

        #region Property Declaration tblAktenIntActionType

        public bool AktIntActionIsPersonalCollection { get; set; }
        public string AktIntActionTypeCaption { get; set; }

        #endregion
    }
}
