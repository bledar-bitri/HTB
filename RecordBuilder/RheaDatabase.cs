using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;



namespace RecordBuilder
{

    public sealed class RheaDatabase
    {

        private SqlConnection _Conn = new SqlConnection();

        private string ConnString = @"Data Source=192.168.22.81\dev;Initial Catalog=MEP;User Id=sa;Password=Meineke1;";

        static readonly RheaDatabase instance = new RheaDatabase();

        public static RheaDatabase Instance
        {
            get
            {
                return instance;
            }
        }


        RheaDatabase()
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


