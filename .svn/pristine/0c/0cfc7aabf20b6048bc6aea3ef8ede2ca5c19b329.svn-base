using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Diagnostics;

namespace ManagedUtilities
{
    public class Logging
    {

        public DateTime CreatedDate
        {
            get { try { FileInfo fi = new FileInfo(Path.Combine(LocationPath, fileName)); return fi.CreationTime; } catch { return DateTime.MinValue; } }
        }

        public string LocationPath
        {
            get { return ManagedUtilities.Local._strLogDir; }
            set { ManagedUtilities.Local._strLogDir = value; }
        }

        public string fileName
        {
            get { return ManagedUtilities.Local._strFileName; }
            set { ManagedUtilities.Local._strFileName = value; }
        }

        public long Size
        {
            get { try { FileInfo fi = new FileInfo(Path.Combine(LocationPath, fileName)); return (fi.Length /1024); } catch { return 0; }; }
        }

        private void getInfo()
        {
            if (string.IsNullOrEmpty(LocationPath))
                LocationPath = Environment.CurrentDirectory;
           
            try
            {
                if (!Directory.Exists(LocationPath))
                {
                    Directory.CreateDirectory(LocationPath);
                }
                if (!File.Exists(Path.Combine(LocationPath, fileName)))
                {
                    StreamWriter sw;
                    sw = File.AppendText(Path.Combine(LocationPath, fileName));
                    try
                    {
                        sw.WriteLine(string.Concat(FormatMessage("Date/Time".PadLeft(10, ' '), "Type".PadLeft(5, ' '), "CallingMethod".PadLeft(10, ' '), "LineNumber".PadLeft(5, ' '), "Function".PadLeft(10, ' '), "Message".PadLeft(25, ' ')), Environment.NewLine));
                    }
                    finally
                    {
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch(Exception e)
            {
              
            }
        }

        public void TrimFile()
        {
            string strReadall = string.Empty;
            int iCount = 0;
            int icounter = 0;
            string strLine = string.Empty;
            StringBuilder sbReWrite = new StringBuilder();
            if (File.Exists(Path.Combine(LocationPath, fileName)))
            {

                StreamReader fs = new StreamReader(Path.Combine(LocationPath, fileName));

                try
                {
                    iCount = File.ReadAllText(Path.Combine(LocationPath, fileName)).Trim().Split(new string[] { "STARTING"}, StringSplitOptions.RemoveEmptyEntries).Length;
                    using (fs)
                    {
                        sbReWrite.Append((string.Concat(FormatMessage("Date/Time".PadLeft(10, ' '), "Type".PadLeft(10, ' '), "CallingMethod".PadLeft(10, ' '), "LineNumber".PadLeft(5, ' '), "Function".PadLeft(10, ' '), "Message".PadLeft(10, ' ')), Environment.NewLine)));
                        while (!fs.EndOfStream)
                        {
                            strLine = fs.ReadLine();

                            if (strLine.ToUpper().Contains("STARTING"))
                            {
                                icounter++;
                            }
                            if (icounter >= (iCount - 4))
                            {
                                sbReWrite.Append(string.Concat(strLine, Environment.NewLine));
                            }
                        }
                    }

                    fs.Close();
                    StreamWriter sw;
                    sw = File.CreateText(Path.Combine(LocationPath, fileName));

                    using (sw)
                    {
                        sw.Write(sbReWrite.ToString());
                        sw.Flush();
                        sw.Close();
                    }
                }
                catch
                {

                }
            }

        }

        public bool AppendToFile(string description)
        {
            bool blnReturn = false;
            StreamWriter sw;
            try
            {
                getInfo();

                FileInfo fi = new FileInfo(Path.Combine(LocationPath, fileName));

                sw = File.AppendText(fi.FullName);
                try
                {
                    if (fi.Length == 0)
                    {
                        sw.WriteLine(string.Concat(FormatMessage("Date/Time".PadLeft(10, ' '), "Type".PadLeft(10, ' '), "CallingMethod".PadLeft(10, ' '), "LineNumber".PadLeft(5,' '), "Function".PadLeft(10, ' '), "Message".PadLeft(10, ' ')), Environment.NewLine));
                    }
                    sw.WriteLine(description);
                    blnReturn = true;
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (IOException ex)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return blnReturn;
        }

        public string FormatMessage(string strTime, string strType, string strMethod, string LineNumber, string strFunction, string strMessage)
        {
            string strReturn = string.Empty;
            try
            {
                strReturn += strTime.PadRight(30, ' ').ToString();
                strReturn += strType.PadRight(20, ' ').ToString();
                strReturn += strMethod.PadRight(30, ' ').ToString();
                strReturn += LineNumber.PadRight(15, ' ').ToString();
                strReturn += strFunction.PadRight(80, ' ').ToString();
                strReturn += strMessage.PadRight(500, ' ').ToString();
                return strReturn;
            }
            catch
            {
                return "ERROR FORMATING STRING";
            }
        }
    }
}
