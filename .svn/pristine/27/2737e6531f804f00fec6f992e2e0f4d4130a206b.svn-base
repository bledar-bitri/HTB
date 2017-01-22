using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;


namespace HTB.Database
{
    public static class DatabasePool
    {
        public const int ConnectToHTB = 0;
        public const int ConnectionToHTBRoads = 1;
        public const int ConnectionToHTBRoadsDB2 = 2;

        private static readonly ArrayList DB2RoadsConnections = new ArrayList();
        private static readonly ArrayList HTBConnections = new ArrayList();
        private static readonly ArrayList HTBRoadsConnections = new ArrayList();

        private const int MaxConnections = 2000;
        private const int MaxDB2Connections = 1000;

        private static readonly string DB2RoadsConnString = System.Configuration.ConfigurationManager.AppSettings["DB2RoadsConnectionString"];
        private static readonly string HTBConnString = System.Configuration.ConfigurationManager.AppSettings["HTBConnectionString"];
        private static readonly string HTBRoadsConnString = System.Configuration.ConfigurationManager.AppSettings["HTBRoadsConnectionString"];

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static DbConnection GetConnection(string cmd, int connectToDatabase, bool debugMode = false)
        {
            if (connectToDatabase == ConnectionToHTBRoadsDB2)
                return GetDB2Connection(cmd, connectToDatabase);
 
            return GetSqlServerConnection(cmd, connectToDatabase, debugMode);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static DbConnection GetSqlServerConnection(string cmd, int connectToDatabase, bool debugMode = false)
        {
            if (connectToDatabase == ConnectionToHTBRoads)
            {
                while (true)
                {
                    int i;
                    for (i = 0; i < HTBRoadsConnections.Count; i++)
                    {
                        var con = (DbConnection) HTBRoadsConnections[i];
                        if (!con.IsInUse)
                        {
                            con.LastCommand = cmd;
                            con.IsInUse = true;
                            return con;
                        }
                    }
                    if (HTBRoadsConnections.Count < MaxConnections)
                    {
                        var con = new DbConnection();
                        con.Connection.ConnectionString = HTBRoadsConnString;
                        con.ID = i;
                        con.LastCommand = cmd;
                        con.ConnectionType = DbConnection.ConnectionType_SqlServer;
                        con.Connection.Open();
                        HTBRoadsConnections.Add(con);
                        con.IsInUse = true;
                        return con;
                    }
                    try
                    {
                        Thread.Sleep(100); // allow time for the connection to be put to use
                    }
                    catch
                    {
                        try
                        {
                            Thread.Sleep(100); // retry sleeping
                        }
                        catch
                        {
                        }
                    }
                }
            }
            
            var sw = new Stopwatch();
            sw.Start();
            LogIfDebug(debugMode, "HTBConnections Count: "+HTBConnections.Count+" Max Connections: "+MaxConnections);
            bool gotConnection = false;
            DbConnection connToReturn = null;
            while (!gotConnection)
            {
                int i;
                LogIfDebug(debugMode, "HTBConnections Count: "+HTBConnections.Count);
                for (i = 0; i < HTBConnections.Count; i++)
                {
                    LogIfDebug(debugMode, "Analyzing [next] HTBConnection");
                    var con = (DbConnection)HTBConnections[i];
                    if (con.NeedsToBeDestroyed)
                    {
                        LogIfDebug(debugMode, "Destroying connection");
                        con.Connection.Close();
                        con.Connection.Dispose();
                        HTBConnections.Remove(con);
                        LogIfDebug(debugMode, "Connection destroyed");
                    }
                    if (!con.IsInUse)
                    {
                        con.LastCommand = cmd;
                        con.IsInUse = true;
                        LogIfDebug(debugMode, "Reusing Old connection");
                        connToReturn = con;
                        gotConnection = true;
                    }
                    LogIfDebug(debugMode, "Done Analyzing");
                }
                    
                LogIfDebug(debugMode, "Done for loop");

                if (!gotConnection && HTBConnections.Count < MaxConnections)
                {
                    LogIfDebug(debugMode, "Creating New Connection");
                    var con = new DbConnection();
                    con.Connection.ConnectionString = HTBConnString;
                    con.ID = i;
                    con.LastCommand = cmd;
                    //Console.WriteLine("CONNECTION: [ID: " + con.ID + "]  {NEW} "+(cmd.Length > 20 ? cmd.Substring(0,20) : cmd));
                    con.Connection.Open();
                    HTBConnections.Add(con);
                    con.IsInUse = true;
                    connToReturn = con;
                    gotConnection = true;
                }
                if(sw.Elapsed.TotalMinutes > 1)
                {
                    Log.Error("More than a minute to get HTB Connection [SQL: "+cmd+"]");
                    sw.Restart();
                }
                try
                {
                    Thread.Sleep(100); // allow time for the connection to be put to use
                }
                catch
                {
                    try
                    {
                        Thread.Sleep(100); // retry sleeping
                    }
                    catch
                    {
                    }
                }
            }
            return connToReturn;
            
        }

        private static DbConnection GetDB2Connection(string cmd, int connectToDatabase)
        {
            lock (DB2RoadsConnString)
            {
                while (true)
                {
                    int i;
                    for (i = 0; i < DB2RoadsConnections.Count; i++)
                    {
                        var con = (DbConnection)DB2RoadsConnections[i];
                        if (!con.IsInUse)
                        {
                            con.LastCommand = cmd;
                            con.IsInUse = true;
                            return con;
                        }
                    }
                    if (DB2RoadsConnections.Count < MaxDB2Connections)
                    {
                        var con = new DbConnection();
                        con.DB2Connection.ConnectionString = DB2RoadsConnString;
                        con.ID = i;
                        con.LastCommand = cmd;
                        con.ConnectionType = DbConnection.ConnectionType_DB2;
                        con.DB2Connection.Open();

                        DB2RoadsConnections.Add(con);
                        con.IsInUse = true;
                        try
                        {
                            Thread.Sleep(100); // allow time for the connection to be put to use
                        }
                        catch
                        {
                            try
                            {
                                Thread.Sleep(100); // retry sleeping
                            }
                            catch
                            {
                            }
                        }
                        return con;
                    }
                    // try to close idle connections
                    for (i = 0; i < DB2RoadsConnections.Count; i++)
                    {
                        var con = (DbConnection)DB2RoadsConnections[i];
                        if (con.GetAliveTime() > 60) // 1 minute
                        {
                            con.Connection.Close();
                            con.IsInUse = false;
                        }
                    }
                }
            }
        }
    
        public static DbConnection GetHTBConnection2()
        {

            lock (HTBConnections)
            {
                while (true)
                {
                    int i = 0;
                    for (i = 0; i < HTBConnections.Count; i++)
                    {
                        var con = (DbConnection)HTBConnections[i];
                        if (!con.IsInUse)
                        {
                            con.IsInUse = true;
                            return con;
                        }
                    }
                    if (HTBConnections.Count < MaxConnections)
                    {
                        var con = new DbConnection();
                        con.MySqlConnection.ConnectionString = HTBConnString;
                        con.MySqlConnection.Open();
                        HTBConnections.Add(con);
                        con.IsInUse = true;
                        return con;
                    }
                    try
                    {
                        Thread.Sleep(100); // allow time for the connection to be put to use
                    }
                    catch
                    {
                        try
                        {
                            Thread.Sleep(100); // retry sleeping
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private static void LogIfDebug(bool debug, string message)
        {
            if (debug)
            {
                Log.Info(message);
                try
                {
                    Thread.Sleep(100);
                }
                catch
                {
                }
            }
        }
    }
}


