using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;



namespace RecordBuilder
{

    public sealed class GenericDatabase
    {

        private SqlConnection _Conn = new SqlConnection();

        public GenericDatabase(string connString)
        {
            if (_Conn.State != ConnectionState.Open)
            {
                _Conn.ConnectionString = connString;
                _Conn.Open();
            }
        }

        public SqlConnection Connection
        {
            get { return _Conn; }
        }

    }

}


