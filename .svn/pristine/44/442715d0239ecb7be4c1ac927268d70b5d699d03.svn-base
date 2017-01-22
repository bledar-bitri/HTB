/*
 * ClassName:       GenericRecordset
 * Author:          Blade
 * Date Created:    12/21/2010
 * Description:     Contains a list of records
 */

using System.Data.SqlClient;
using System.Data;
using System.Collections;
using IBM.Data.DB2;
using System;

namespace HTB.Database
{
    public class GenericRecordset : RecordSet
    {
        public GenericRecordset(int connType = DbConnection.ConnectionType_SqlServer) : base (connType)
        {
        }

        public void LoadFromSqlQuery(string psqlCommand, Type pclassName, bool debugMode = false)
        {
            if (ConnectionType == DbConnection.ConnectionType_DB2)
                LoadListFromDB2DataReader(GetDB2DataReader(psqlCommand), pclassName);
            else
                LoadListFromDataReader(GetDataReader(psqlCommand, debugMode), pclassName, debugMode);
        }

        public void LoadFromStoredProcedure(string spName, ArrayList parameters, Type pclassName, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            //LoadListFromDataReader(GetSoredProcedureDataReader(spName, parameters), pclassName);

            Con = DatabasePool.GetConnection(spName, connectToDatabase);
            var cmd = new SqlCommand(spName, Con.Connection) {CommandType = CommandType.StoredProcedure};
            cmd.CommandTimeout = 600;
            bool containsOutput = false;
            if (parameters != null)
            {
                foreach (StoredProcedureParameter p in parameters)
                {
                    var sqlParam = new SqlParameter(p.Name, p.Value) {Direction = p.Direction};
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
            Con.IsInUse = false;
        }

        public ArrayList[] GetMultipleListsFromStoredProcedure(string spName, ArrayList parameters, Type[] pclassName, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            Con = DatabasePool.GetConnection(spName, connectToDatabase);
            var cmd = new SqlCommand(spName, Con.Connection) {CommandType = CommandType.StoredProcedure};
            cmd.CommandTimeout = 600;
            bool containsOutput = false;
            if (parameters != null)
            {
                foreach (StoredProcedureParameter p in parameters)
                {
                    var sqlParam = new SqlParameter(p.Name, p.Value) {Direction = p.Direction};
                    cmd.Parameters.Add(sqlParam);

                    if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.ReturnValue)
                        containsOutput = true;
                }
            }
            SqlDataReader results = cmd.ExecuteReader();
            var list = new ArrayList[pclassName.Length];
            for (int i = 0; i < list.Length; i++)
                list[i] = new ArrayList();
            LoadMultipleListsFromDataReader(results, list, pclassName);
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
            Con.IsInUse = false;
            return list;
        }

        private void LoadListFromDataReader(SqlDataReader dr, Type ptype, bool debugMode = false)
        {
            RecordsList.Clear();
            RecordLoader.LoadRecordsFromDataReader(
                dr,
                RecordsList,
                ptype, Con);
        }

        private void LoadMultipleListsFromDataReader(SqlDataReader dr, ArrayList[] presults, Type[] ptypes)
        {
            RecordsList.Clear();
            RecordLoader.LoadRecordsFromMultipleResultsets(
                dr,
                presults,
                ptypes, 
                Con);
        }

        private void LoadListFromDB2DataReader(DB2DataReader dr, Type ptype)
        {
            RecordsList.Clear();
            RecordLoader.LoadRecordsFromDB2DataReader(
                dr,
                RecordsList,
                ptype, Con);
        }
    }
}