using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database.HTB.StoredProcs
{
    public class spGetWeeklyValidationResults : Record
    {
        public int ID { get; set; }
        public int Akt { get; set; }
        public string ErrorDescription { get; set; }
    }
}
