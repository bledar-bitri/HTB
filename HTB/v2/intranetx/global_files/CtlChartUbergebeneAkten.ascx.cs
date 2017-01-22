using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI.DataVisualization.Charting;
using HTB.Database;
using HTBUtilities;

namespace HTB.v2.intranetx.global_files
{
    public partial class CtlChartUbergebeneAkten : System.Web.UI.UserControl
    {
        public int AgId { get; set; }
        public tblUser User { get; set; }

        private readonly ArrayList _list = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Generate()
        {
            if (AgId > 0 && User != null && User.UserID > 0)
            {
                LoadChart();
            }
        }
        private void LoadChart()
        {
            _list.Clear();
            for (int i = 0; i < 12; i++)
            {
                var parameters = new ArrayList
                                 {
                                     new StoredProcedureParameter("agId", SqlDbType.Int, AgId),
                                     new StoredProcedureParameter("monthsToGoBack", SqlDbType.Int, i)
                                 };

                if (User != null)
                {
                    if (!User.UserIsSbAdmin)
                        parameters.Add(new StoredProcedureParameter("agSbEmail", SqlDbType.VarChar, User.UserEMailOffice));
                }
                else
                {
                    parameters.Add(new StoredProcedureParameter("agSbEmail", SqlDbType.VarChar, "NOT_VALID_USER")); // the database should not return anything
                } 
                
                var aktsList = HTBUtils.GetStoredProcedureRecords("spAGUbergebeneForPreviousMonth", parameters, typeof(ChartRecord));
                var rec = (ChartRecord) aktsList[0];
                rec.Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.AddMonths(-i).Month) + " " + DateTime.Now.AddMonths(-i).Year;

                _list.Add(rec);
            }
            BindChartData(chrtChart, _list);
        }


        private static void BindChartData(Chart chrt, ArrayList list)
        {
            chrt.Visible = true;

            if (list == null || list.Count == 0)
                chrt.Visible = false;

            if (list != null)
            {
                var xval1 = new string[list.Count];
                var yval1 = new int[list.Count];

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var rec = (ChartRecord)list[i];
                    xval1[list.Count - i - 1] = rec.Month;
                    yval1[list.Count - i - 1] = rec.Akts;
                }
                chrt.Series["Series1"].Points.DataBindXY(xval1, yval1);
            }
        }

        private class ChartRecord
        {
            public string Month { get; set; }
            public int Akts { get; set; }
            public double Amount { get; set; }
        }
    }
}