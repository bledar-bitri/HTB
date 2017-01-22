using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HTB.Database;
using HTBUtilities;

namespace HTBIncentroNotification
{
    public class InCentroNotifier
    {
        private readonly List<ProcessedPathRecord> _processedPathList = new List<ProcessedPathRecord>();

        private InCentroNotifier()
        {
            _processedPathList.Clear();
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblMahnung WHERE MahnungStatus = 1 ORDER BY MahnungRateID", typeof(tblMahnung));
            if (list.Count > 0)
            {
                foreach (tblMahnung mahnung in list)
                {
                    if (!string.IsNullOrEmpty(mahnung.MahnungXMLPath))
                    {
                        int count = GetPathCount(mahnung.MahnungXMLPath);
                        if(count == 0)
                        {
                            _processedPathList.Add(new ProcessedPathRecord(count+1, mahnung.MahnungXMLPath));
                        }
                        else
                        {
                            SetPathCount(count + 1, mahnung.MahnungXMLPath);
                        }
                    }
                }
                if (SendNotificationEmail(list.Count))
                {
                    foreach (tblMahnung mahnung in list)
                    {
                        mahnung.MahnungStatus = 2;
                        RecordSet.Update(mahnung);
                    }
                }
            }
            list.Clear();
            _processedPathList.Clear();
        }

        private bool SendNotificationEmail(int totalCount)
        {
            return new HTBEmail().SendGenericEmail(
                    new string[] { HTBUtils.GetConfigValue("Mahnung_FTP_Email"), HTBUtils.GetConfigValue("Mahnung_FTP_CC") },
                    string.Format("Mahnung(en)  [ {0} ] [ {1} ]", DateTime.Now.ToShortDateString(), totalCount),
                    GetEmailBody(totalCount),
                    true);
        }

        private int GetPathCount(string path)
        {
            return (from rec in _processedPathList where rec.Path == path select rec.Count).FirstOrDefault();
        }

        private void SetPathCount(int count, string path)
        {
            foreach (ProcessedPathRecord rec in _processedPathList.Where(rec => rec.Path == path))
                rec.Count = count;
        }

        private string GetEmailBody(int totalCount)
        {
            string body = HTBUtils.GetFileText(HTBUtils.GetConfigValue("Incentro_Text"));
            var sb = new StringBuilder();
            foreach (ProcessedPathRecord rec in _processedPathList)
            {
                sb.Append("<tr><td>&nbsp;&nbsp;&nbsp;");
                sb.Append(Path.GetFileName(rec.Path));
                sb.Append("</td><td align=\"right\">");
                sb.Append(rec.Count);
                sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");
            }
            sb.Append("<tr><td align=\"right\"><strong>Total:</strong><td align=\"right\"><strong>");
            sb.Append(totalCount);
            sb.Append("</strong>&nbsp;&nbsp;&nbsp;&nbsp;</td></tr>");

            body = body.Replace("[DATE]", DateTime.Now.ToShortDateString());
            body = body.Replace("[TABLE_DATA]", sb.ToString());
            return body;
        }

        static void Main(string[] args)
        {
            new InCentroNotifier();
        }
    }

    class ProcessedPathRecord
    {
        public string Path;
        public int Count;
        public ProcessedPathRecord(int count, string path)
        {
            Count = count;
            Path = path;
        }
    }
}
