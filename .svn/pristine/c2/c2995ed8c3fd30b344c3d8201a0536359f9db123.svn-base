/*
 * ClassName:       qryAktenIntSet
 * Author:          Blade
 * Date Created:    02/22/2011
 * Description:     Contains a list of qryAktenInt records
 */

using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Text;
using MySql.Data.MySqlClient;
using System;
using System.Reflection;

namespace HTB.Database.Views
{
    public class qryAktenIntSet
    {

        #region Property Declaration
        private ArrayList _lstQryAktenInt = new ArrayList();
        public ArrayList qryAktenIntList
        {
            get { return _lstQryAktenInt; }
            set { _lstQryAktenInt = value; }
        }
        #endregion
        private DbConnection _con;

        public SqlDataReader GetQryAktenIntDataReader(string psqlCommand, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            SqlDataReader _Results;
            _con = DatabasePool.GetConnection(psqlCommand, connectToDatabase);
            SqlCommand _cmd = new SqlCommand(psqlCommand, _con.Connection);
            _Results = _cmd.ExecuteReader();
            return _Results;
        }
        public void LoadQryAktenInt(string psqlCommand)
        {
            LoadListFromDataReader(GetQryAktenIntDataReader(psqlCommand));
        }
        public void Load(String where, String order)
        {
            LoadListFromDataReader(GetQryAktenIntDataReader("select * from qryAktenInt where " + where + " order by " + order));
        }
        public void LoadAktenIntById(int aktId)
        {
            LoadListFromDataReader(GetQryAktenIntDataReader("select * from qryAktenInt where AktIntID = " + aktId));
        }
        private void LoadListFromDataReader(SqlDataReader dr)
        {
            _lstQryAktenInt.Clear();
            RecordLoader.LoadRecordsFromDataReader(
                dr,
                qryAktenIntList,
                typeof(qryAktenInt), _con);
        }
    }
}