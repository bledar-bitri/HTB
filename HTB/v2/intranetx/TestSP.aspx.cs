using System;
using System.Collections;
using System.IO;
using HTB.Database;
using HTB.Database.LookupRecords;
using HTB.Database.Views;
using HTBExtras;
using HTBExtras.XML;

namespace HTB.v2.intranetx
{
    public partial class TestSP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtOutput.Text = Record.GetObjectiveCString(typeof(tblProtokolUbername));
            Type t = typeof(tblProtokolUbername);
            ArrayList list = Record.GetObjectiveCFiles(t);

            WriteFile("c:\\temp\\ObjC2Record\\" + t.Name + ".h", list[0].ToString());
            WriteFile("c:\\temp\\ObjC2Record\\" + t.Name + ".m", list[1].ToString());

            txtOutput.Text = list[0].ToString() + "\n\n" + list[1].ToString();

        }

        private void WriteFile (string fileName, string text)
        {
            TextWriter tw = new StreamWriter(fileName);
            tw.Write(text);
            tw.Flush();
            tw.Close();
            tw.Dispose();
        }
    }
}