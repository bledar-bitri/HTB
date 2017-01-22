using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryCustAktStatus : tblCustInkAkt
    {
        #region Property Declaration tblKZ

        public string KZKZ { get; set; }
        public string KZCaption { get; set; }

        #endregion

        #region Property Declaration

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int CustInkAktStatusId { get; set; }
        public int CustInkAktStatusCode { get; set; }
        public string CustInkAktStatusCaption { get; set; }

        #endregion
    }
}
