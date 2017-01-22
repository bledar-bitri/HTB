using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace RecordBuilder
{
    class PrimaryKeyList
    {
        #region Property Declaration
        private string _tableName;

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        private ArrayList _lstKey = new ArrayList();
        public ArrayList KeysList
        {
            get { return _lstKey; }
            set { _lstKey = value; }
        }
        #endregion

        public PrimaryKeyList(string tableName)
        {
            TableName = tableName;
        }
        #region Rhea
        public SqlDataReader GetDataReader(SqlConnection connection, string storedProcName)
        {
            SqlDataReader _Results;
            SqlCommand _cmd = new SqlCommand(storedProcName, connection);
            _cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter _param = new SqlParameter("@TableName", TableName);
            _cmd.Parameters.Add(_param);
            _Results = _cmd.ExecuteReader();
            return _Results;
        }
        public void LoadPrimaryKeys(SqlConnection connection)
        {
            LoadListFromDataReader(GetDataReader(connection, "GetPrimaryKyesForTable"));
        }
        #endregion

        private void LoadListFromDataReader(SqlDataReader dr)
        {
            KeysList.Clear();
            RecordLoader.LoadRecordsFromDataReader(
                dr,
                KeysList,
                "RecordBuilder.PrimaryKey");
        }
    }
}
