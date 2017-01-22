using System;

namespace HTB.Database.HTB.Views
{
    public class qryAktIntProvabrechnung : Record
    {

        #region Property Declaration tblAktenIntAction
        public int AktIntActionID { get; set; }

        public int AktIntActionType { get; set; }

        public DateTime AktIntActionDate { get; set; }

        public string AktIntActionMemo { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double AktIntActionBetrag { get; set; }

        public int AktIntActionAkt { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double AktIntActionProvision { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double AktIntActionHonorar { get; set; }

        public string AktIntActionBeleg { get; set; }

        public int AktIntActionUeberwiesen { get; set; }

        public int AktIntActionProvAbzug { get; set; }

        #endregion

        #region Property Declaration tblAktenIntActionType
        public string AktIntActionTypeCaption { get; set; }

        public bool AktIntActionIsExtensionRequest { get; set; }
        #endregion

        #region Property Declaration tblAktenInt
        public string AktIntAZ { get; set; }

        public int AktIntSB { get; set; }

        public int AktIntAktType { get; set; }

        public string AktIntUserEdit { get; set; }
        #endregion

        #region Property Declaration tblAuftraggeber
        public string AuftraggeberName1 { get; set; }
        #endregion

        #region Property Declaration tblGegner
        public string GegnerLastZip { get; set; }

        public string GegnerLastOrt { get; set; }

        public string GegnerLastName1 { get; set; }

        public string GegnerLastName2 { get; set; }
        #endregion

        #region Property Declaration tblUser
        public string UserVorname { get; set; }
        public string UserNachname { get; set; }
        #endregion
    }
}
