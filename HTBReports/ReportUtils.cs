using System.Collections.Generic;
using HTBUtilities;
using iTextSharp.text;

namespace HTBReports
{
    internal static class ReportUtils
    {
        public static int PrintTextInMultipleLines(IReport report, int lin, int col, int linGap, string text, int maxChars)
        {
            return PrintTextInMultipleLines(report, lin, col,linGap, text, maxChars, BaseColor.BLACK);
        }
        public static int PrintTextInMultipleLines(IReport report, int lin, int col, int linGap, string text, int maxChars, BaseColor fontColor)
        {
            string[] lines = text.Split('\n');
            foreach (string line in lines)
            {
                IEnumerable<string> lins = HTBUtils.SplitStringInPdfLines(line, maxChars);
                foreach (string l in lins)
                {
                    report.GetWriter().print(lin, col, l, fontColor);
                    lin += linGap;
                    if (report.CheckOverflow(lin))
                    {
                        report.GetWriter().newPage();
                        lin = report.PrintPageHeader();
                        report.SetLineFont();
                    }
                }
            }
            return lin;
        }
    }
}
