using System.Text;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace HTB.v2.intranetx.progress.atlas
{
    public partial class ExecContext : System.Web.UI.Page
    {
        private static string cacheKey;

        protected static string UpdateStatusInternal(string taskID)
        {
            cacheKey = taskID;
//            object o = Cache[taskID];
//            if (o == null)
//            {
                return string.Empty;
//            }
//            return Cache[taskID].ToString();
        }

        protected static string GetSalesEmployeesInternal(string taskID, int year)
        {
            FakeDataService service = new FakeDataService(taskID);
            SalesDataCollection table = service.GetSalesEmployees(year);

            StringBuilder builder = new StringBuilder();
            builder.Append("<div id=grid>");
            builder.Append("<table>");
            foreach (SalesData row in table)
            {
                builder.Append("<tr>");
                builder.AppendFormat("<td><b>{0}</b></td><td>{1}</td>", row.Employee, row.Amount);
                builder.Append("</tr>");
            }

            builder.Append("</table>");
            builder.Append("</div>");
            return builder.ToString();
        }

    }

}