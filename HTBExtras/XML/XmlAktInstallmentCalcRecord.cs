using System;
using System.Collections;
using System.Collections.Generic;
using HTB.Database;

namespace HTBExtras.XML
{
    public class XmlAktInstallmentCalcRecord : Record
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double InstallmentAmount { get; set; }
        public double LastInstallmentAmount { get; set; }
        public double TotalInterest { get; set; }
        public int NumberOfInstallments { get; set; }
        public double CollectedAmount { get; set; }

        // old installment (intervention only) fields
        public int PaymentType { get; set; }
        public int PaymentDay { get; set; }
        public string PaymentPeriod { get; set; }

        public ArrayList InstallmentsList { get; set; }
        public XmlAktInstallmentCalcRecord()
        {
            if(InstallmentsList == null)
                InstallmentsList = new ArrayList();
        }

    }
}
