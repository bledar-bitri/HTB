/*
 * ClassName:       TableDefList
 * Author:          Blade
 * Date Created:    08/12/2008
 * Description:     Contains a list of table definition rows
 */

using System.Data.SqlClient;
using System.Data;
using System.Collections;
using MySql.Data.MySqlClient;
namespace RecordBuilder
{
    public class MySqlTableDefList
    {
        
        #region Property Declaration
        private ArrayList _lstDef = new ArrayList();
        public ArrayList DefinitionList
        {
            get { return _lstDef; }
            set { _lstDef = value; }
        }
        #endregion

        #region Using Connection String
        public MySqlDataReader GetGenericDataReader(string connString, string sqlCommand)
        {
            MySqlDataReader _Results;
            MySqlConnection _conn = new MySqlConnection(connString);
            _conn.Open();
            MySqlCommand _cmd = new MySqlCommand(sqlCommand, _conn);
            _Results = _cmd.ExecuteReader();
            return _Results;
        }
        public void LoadGenericTableDef(string connString, string tableName)
        {
            LoadListFromDataReader(GetGenericDataReader(connString, "show columns FROM " + tableName));
        }
        #endregion

        private void LoadListFromDataReader(MySqlDataReader dr)
        {
            _lstDef.Clear();
            RecordLoader.LoadRecordsFromMySqlDataReader(
                dr,
                DefinitionList,
                "RecordBuilder.MySqlTableDef");
        }
    }
}
