/*
 * ClassName:       TableDefList
 * Author:          Blade
 * Date Created:    08/12/2008
 * Description:     Contains a list of table definition rows
 */

using System.Data.SqlClient;
using System.Data;
using System.Collections;
namespace RecordBuilder
{
    public class TableDefList
    {
        public static string DB_SERVER_RHEA = "rhea";
        public static string DB_SERVER_ARES = "ares";
        
        #region Property Declaration
        private ArrayList _lstDef = new ArrayList();
        public ArrayList DefinitionList
        {
            get { return _lstDef; }
            set { _lstDef = value; }
        }
        #endregion

        #region CallManagement And Mep Production
        public SqlDataReader GetDataReader(string storedProcName, string tableName)
        {
            SqlDataReader _Results;
            SqlCommand _cmd = new SqlCommand(storedProcName, HTBDatabase.Instance.Connection);
            _cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter _param = new SqlParameter("@TableName", tableName);
            _cmd.Parameters.Add(_param);
            _Results = _cmd.ExecuteReader();
            return _Results;
        }
        
        public void LoadCmTableDef(string tableName)
        {
            LoadListFromDataReader(GetDataReader("GetTableDef", tableName));
        }
        public void LoadMepTableDef(string tableName)
        {
            LoadListFromDataReader(GetDataReader("GetMEPTableDef", tableName));
        }
        public void LoadTableDef(string storedProcName, string tableName)
        {
            LoadListFromDataReader(GetDataReader(storedProcName, tableName));
        }
        #endregion

        #region Rhea
        public SqlDataReader GetRheaDataReader(string storedProcName, string tableName)
        {
            SqlDataReader _Results;
            SqlCommand _cmd = new SqlCommand(storedProcName, RheaDatabase.Instance.Connection);
            _cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter _param = new SqlParameter("@TableName", tableName);
            _cmd.Parameters.Add(_param);
            _Results = _cmd.ExecuteReader();
            return _Results;
        }
        public void LoadRheaTableDef(string tableName)
        {
            LoadListFromDataReader(GetRheaDataReader("GetTableDef", tableName));
        }
        #endregion

        #region Using Connection String
        public SqlDataReader GetGenericDataReader(string connString, string storedProcName, string tableName)
        {
            SqlDataReader _Results;
            SqlCommand _cmd = new SqlCommand(storedProcName, new GenericDatabase(connString).Connection);
            _cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter _param = new SqlParameter("@TableName", tableName);
            _cmd.Parameters.Add(_param);
            _Results = _cmd.ExecuteReader();
            return _Results;
        }
        public void LoadGenericTableDef(string connString, string tableName)
        {
            LoadListFromDataReader(GetGenericDataReader(connString, "GetTableDef", tableName));
        }
        #endregion

        private void LoadListFromDataReader(SqlDataReader dr)
        {
            _lstDef.Clear();
            RecordLoader.LoadRecordsFromDataReader(
                dr,
                DefinitionList,
                "RecordBuilder.TableDef");
        }
    }
}
