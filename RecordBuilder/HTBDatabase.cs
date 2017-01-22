using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;



namespace RecordBuilder
{

    public sealed class HTBDatabase
    {

        private SqlConnection _Conn = new SqlConnection();

        //private string ConnString = @"Data Source=192.168.22.73;Initial Catalog=CM;User Id=sa;Password=Meineke1;";
        private string ConnString = @"Data Source=localhost\sqlexpress;Initial Catalog=HTB;User Id=sa;Password=garant;";

        static readonly HTBDatabase instance = new HTBDatabase();

        public static HTBDatabase Instance
        {
            get
            {
                return instance;
            }
        }


        HTBDatabase()
        {
            if (_Conn.State != ConnectionState.Open)
            {
                _Conn.ConnectionString = ConnString;
                _Conn.Open();
            }
        }

        public SqlConnection Connection
        {
            get { return _Conn; }
        }

    }

}


