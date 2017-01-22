using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace HTB.v2.intranetx.progress.atlas
{
    
    partial class SyncExec : System.Web.UI.Page
    {

        public static decimal GetSalesByEmployeeInternal(int id, int year)
        {
            FakeDataService helper = new FakeDataService("");
            return helper.GetSalesByEmployee(id, year);
        }


    }
}