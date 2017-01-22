using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using HTB.Database;

namespace TransferTables
{
    class Program
    {
        static string[] PrecedingZeros = {"0000000000000", "000000000000", "00000000000", "0000000000", "000000000","00000000", "0000000", "000000", "00000", "0000", "000", "00", "0", ""};
        static string tableName  = ConfigurationManager.AppSettings["Table"];

        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            var watch = new Stopwatch();
            Console.WriteLine("Loading Records");
            watch.Start();
            ArrayList roads = HTBUtilities.HTBUtils.GetSqlRecords("SELECT * FROM " + tableName, typeof(tblRoadInfo), DbConnection.ConnectionType_DB2);

            Console.WriteLine(string.Format("Transferring Records [ total read time {0} ]",watch.Elapsed));
            watch.Restart();
//            var set = new RecordSet(DbConnection.ConnectionType_DB2);
            int counter = 0;
            var sb = new StringBuilder();
            TextWriter tw = new StreamWriter("c:/temp/db2data.txt");
            foreach (tblRoadInfo road in roads)
            {
                if(++counter % 10000 == 0)
                {
                    Console.WriteLine(string.Format("Exported: {0} records in {1}",counter, watch.Elapsed));
                }
                sb.Append("insert into tblroad values (");
                if (road.FromLatitude >= road.ToLatitude)
                {
                    sb.Append(GetIntFromCoordinate(road.FromLatitude));
                    sb.Append(", ");
                    sb.Append(GetIntFromCoordinate(road.FromLongitude));
                    sb.Append(", ");
                    sb.Append(GetIntFromCoordinate(road.ToLatitude));
                    sb.Append(", ");
                    sb.Append(GetIntFromCoordinate(road.ToLongitude));
                }
                else
                {
                    sb.Append(GetIntFromCoordinate(road.ToLatitude));
                    sb.Append(", ");
                    sb.Append(GetIntFromCoordinate(road.ToLongitude));
                    sb.Append(", ");
                    sb.Append(GetIntFromCoordinate(road.FromLatitude));
                    sb.Append(", ");
                    sb.Append(GetIntFromCoordinate(road.FromLongitude));
                }
                sb.Append(", ");
                sb.Append(road.Distance);
                sb.Append(", ");
                sb.Append(road.TimeInSeconds);
                sb.Append(", '");
                sb.Append(road.LookupDate.Year);
                sb.Append("-");
                sb.Append(road.LookupDate.Month > 9 ? ""+road.LookupDate.Month : "0" + road.LookupDate.Month);
                sb.Append("-");
                sb.Append(road.LookupDate.Day > 9 ? "" + road.LookupDate.Day : "0" + road.LookupDate.Day);
                sb.Append("');\n");
                tw.WriteLine(sb.ToString());
                
                //Console.WriteLine(sb.ToString());
                sb.Clear();

            }
            tw.Flush();
            tw.Close();
            tw.Dispose();
            Console.WriteLine("DONE {0} {1}",counter, watch.Elapsed);
            watch.Stop();
            Console.ReadLine();
        }

        static void Main_Old(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            var watch = new Stopwatch();
            Console.WriteLine("Loading Records");
            watch.Start();
            ArrayList roads = HTBUtilities.HTBUtils.GetSqlRecords("SELECT * FROM tblRoadInfo", typeof(tblRoadInfo));

            Console.WriteLine(string.Format("Transferring Records [ total read time {0} ]", watch.Elapsed));
            watch.Restart();
            //            var set = new RecordSet(DbConnection.ConnectionType_DB2);
            int counter = 0;
            var sb = new StringBuilder();
            TextWriter tw = new StreamWriter("c:/temp/db2data.txt");
            foreach (tblRoadInfo road in roads)
            {
                if (++counter % 10000 == 0)
                {
                    Console.WriteLine(string.Format("Exported: {0} records in {1}", counter, watch.Elapsed));
                }
                if (road.FromLatitude >= road.ToLatitude)
                {
                    sb.Append(GetString(GetIntFromCoordinate(road.FromLatitude)));
                    sb.Append(GetString(GetIntFromCoordinate(road.FromLongitude)));
                    sb.Append(GetString(GetIntFromCoordinate(road.ToLatitude)));
                    sb.Append(GetString(GetIntFromCoordinate(road.ToLongitude)));
                }
                else
                {
                    sb.Append(GetString(GetIntFromCoordinate(road.ToLatitude)));
                    sb.Append(GetString(GetIntFromCoordinate(road.ToLongitude)));
                    sb.Append(GetString(GetIntFromCoordinate(road.FromLatitude)));
                    sb.Append(GetString(GetIntFromCoordinate(road.FromLongitude)));
                }
                sb.Append(GetIntFromCoordinate(road.Distance));
                sb.Append(road.TimeInSeconds);
                sb.Append(",");
                sb.Append(road.LookupDate.Year);
                sb.Append(road.LookupDate.Month > 9 ? "" + road.LookupDate.Month : "0" + road.LookupDate.Month);
                sb.Append(road.LookupDate.Day > 9 ? "" + road.LookupDate.Day : "0" + road.LookupDate.Day);
                tw.WriteLine(sb.ToString());

                //Console.WriteLine(sb.ToString());
                sb.Clear();

            }
            tw.Flush();
            tw.Close();
            tw.Dispose();
            Console.WriteLine("DONE {0} {1}", counter, watch.Elapsed);
            watch.Stop();
            Console.ReadLine();
        }

        private static string GetString(double val)
        {
            string str = val.ToString();
            int idx = str.IndexOf(".");
            var sb = new StringBuilder("+");
            if(idx >= 0)
                sb.Append(PrecedingZeros[idx]);
            sb.Append(str);
            sb.Append(",");
            return sb.ToString();
        }
        private static string GetString(int val)
        {
            string str = val.ToString();
            var sb = new StringBuilder();
            sb.Append(str);
            sb.Append(",");
            return sb.ToString();
        }

        public static int GetIntFromCoordinate(double coordinate)
        {
            int ret = 0;
            try
            {
                string corString = coordinate.ToString().Replace(",", "").Replace(".", "");
                if (corString.Length > 7)
                    corString = corString.Substring(0, 7);

                ret = Convert.ToInt32(corString);
            }
            catch
            {
            }
            return ret;
        }
    }
}
