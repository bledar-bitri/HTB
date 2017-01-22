using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.Views
{
    public class qryCustInkAktInvoiceApply : tblCustInkAktInvoice
    {

        #region Property Declaration tblCustInkAktInvoiceApply

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int ApplyId { get; set; }

        public DateTime ApplyDate { get; set; }

        [MappingAttribute(FieldFormat = MappingAttribute.FIELD_FORMAT_CURRENCY)]
        public double ApplyAmount { get; set; }

        public int ApplyFromInvoiceId { get; set; }

        public int ApplyToInvoiceId { get; set; }

        #endregion
    }
}
