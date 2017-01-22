using System;

namespace HTB.Database.HTB.StoredProcs
{
    public class spGetActionCoordinates : Record
    {
        #region Property Declaration tblAktenInt

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int AktIntID { get; set; }
        #endregion

        #region Property Declaration tblAktenIntAction

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int AktIntActionID { get; set; }
        public DateTime AktIntActionDate { get; set; }
        public double AktIntActionBetrag { get; set; }
        public double AktIntActionLatitude { get; set; }
        public double AktIntActionLongitude { get; set; }
        public string AktIntActionAddress { get; set; }
        #endregion

        #region Property Declaration tblAktenIntActionType

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int AktIntActionTypeID { get; set; }
        public string AktIntActionTypeCaption { get; set; }
        public bool AktIntActionIsPositive { get; set; }
        #endregion

        #region Property Declaration tblGegner
        public string GegnerLastName1 { get; set; }
        public string GegnerLastName2 { get; set; }
        
        public string GegnerLastZip { get; set; }
        public string GegnerLastOrt { get; set; }
        public string GegnerLastStrasse { get; set; }

        public double GegnerLatitude { get; set; }
        public double GegnerLongitude { get; set; }

        #endregion
    }
}
