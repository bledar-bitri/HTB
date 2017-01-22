using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using HTB.Database;
using HTBUtilities;

namespace DeltaVistaTransfer
{
    public class DvTransfer
    {
        private static string EmailText = "Sehr geehrte Damen und Herren,<br/><br/>" +
                                          "im Anhang finden Sie eine CVS Datei mit unser eigene Inkasso Akte. <br/><br/>" +
                                          "Mit freundlichen Grüßen,<br/>" +
                                          "Ihr ECP Team <br/><br/>" +
                                          "ECP European Car Protect OG<br/>"+
                                          "Schwarzparkstraße 15, A-5020 Salzburg <br/>"+
                                          "[p] 	+43 662 203410 <br/>"+
                                          "[f] 	+43 662 203410-90 <br/>"+
                                          "[@] 	office@ecp.or.at <br/>"+
                                          "[http] 	www.ecp.or.at";


        private static ArrayList RecordsList = new ArrayList();
        private static DbConnection Con;
        private static readonly string CommandTimeout = System.Configuration.ConfigurationManager.AppSettings["CommandTimeout"];

        private static void Main(string[] args)
        {
            LoadFromStoredProcedure("spGetDeltaVistaTransferData", null, typeof (SingleValue));

            using (var stream = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream, Encoding.UTF8))
                {
                    foreach (SingleValue s in RecordsList)
                    {
                        sw.WriteLine(s.StringValue);
                    }
                    sw.Flush();
                    sw.Close();
                    stream.Flush();
                    stream.Close();

                    var to = new List<string> {HTBUtils.GetConfigValue("DV_Transfer_EMail")};

                    var attachment = new HTBEmailAttachment(HTBUtils.ReopenMemoryStream(stream), "ECP_Inkasso.csv", "text/plain");
                    new HTBEmail().SendGenericEmail(to, "ECP Inkasso Daten", EmailText, true, new List<HTBEmailAttachment> {attachment});
                }
            }
        }

        private static void LoadFromStoredProcedure(string spName, ArrayList parameters, Type pclassName, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            int timeout;
            try
            {
                timeout = Convert.ToInt32(CommandTimeout);
            }
            catch
            {
                timeout = 360;
            }
            DbConnection con = DatabasePool.GetConnection(spName, connectToDatabase);
            var cmd = new SqlCommand(spName, con.Connection) {CommandType = CommandType.StoredProcedure, CommandTimeout = timeout};
            bool containsOutput = false;
            if (parameters != null)
            {
                foreach (StoredProcedureParameter p in parameters)
                {
                    var sqlParam = new SqlParameter(p.Name, p.Value) { Direction = p.Direction };
                    cmd.Parameters.Add(sqlParam);

                    if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.ReturnValue)
                        containsOutput = true;
                }
            }
            SqlDataReader results = cmd.ExecuteReader();
            LoadListFromDataReader(results, pclassName);
            if (containsOutput)
            {
                var returnValues = new ArrayList();
                foreach (StoredProcedureParameter p in parameters)
                {
                    if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.ReturnValue)
                    {
                        returnValues.Add(new StoredProcedureParameter(p.Name, p.SqlType, cmd.Parameters[p.Name].Value));
                    }
                }
                parameters.Add(returnValues);
            }
            con.IsInUse = false;
        }

        private static void LoadListFromDataReader(SqlDataReader dr, Type ptype)
        {
            RecordsList.Clear();
            RecordLoader.LoadRecordsFromDataReader(
                dr,
                RecordsList,
                ptype, Con);
        }

    }
}
