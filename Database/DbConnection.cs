using System;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;



namespace HTB.Database
{

    public sealed class DbConnection
    {
        public const int ConnectionType_SqlServer = 0;
        public const int ConnectionType_MySql = 1;

        private readonly SqlConnection _conn = new SqlConnection();
        private readonly MySqlConnection _mySqlConn = new MySqlConnection();

        public int ConnectionType = ConnectionType_SqlServer;

        public bool IsInUse { get; set; }
        public bool NeedsToBeDestroyed { get; set; }
        public int ID { get; set; }
        public string LastCommand { get; set; }
        public DateTime lastUsedTime { get; set; }

        public DbConnection()
        {
            LastCommand = "";
            ID = -1;
            IsInUse = false;
            NeedsToBeDestroyed = false;
            lastUsedTime = DateTime.Now;
        }
        
        public SqlConnection Connection
        {
            get { return _conn; }
        }
        public MySqlConnection MySqlConnection
        {
            get { return _mySqlConn; }
        }
        public double GetAliveTime()
        {
            TimeSpan ts = DateTime.Now.Subtract(lastUsedTime);
            return ts.TotalSeconds;
        }
    }

}


