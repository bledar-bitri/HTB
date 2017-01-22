using System;
using System.Data.SqlClient;
using IBM.Data.DB2;
using MySql.Data.MySqlClient;



namespace HTB.Database
{

    public sealed class DbConnection
    {
        public const int ConnectionType_SqlServer = 0;
        public const int ConnectionType_MySql = 1;
        public const int ConnectionType_DB2 = 3;

        private readonly SqlConnection _conn = new SqlConnection();
        private readonly MySqlConnection _mySqlConn = new MySqlConnection();
        private readonly DB2Connection _db2Conn = new DB2Connection();

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
        public DB2Connection DB2Connection
        {
            get { return _db2Conn; }
        }
        
        public double GetAliveTime()
        {
            TimeSpan ts = DateTime.Now.Subtract(lastUsedTime);
            return ts.TotalSeconds;
        }
    }

}


