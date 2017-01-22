/*
 * ClassName:       tblAktenIntPosSet
 * Author:          Blade
 * Date Created:    02/22/2011
 * Description:     Contains a list of tblAktenIntPos records
 */

using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Text;
using MySql.Data.MySqlClient;
using System;
using System.Reflection;

namespace HTB.Database
{
    public class tblAktenIntPosSet
    {

        #region Property Declaration
        private ArrayList _lsttblAktenIntPos = new ArrayList();
        public ArrayList tblAktenIntPosList
        {
            get { return _lsttblAktenIntPos; }
            set { _lsttblAktenIntPos = value; }
        }
        #endregion
        private DbConnection _con;
        
        #region Load Data
        public SqlDataReader GettblAktenIntPosDataReader(string psqlCommand, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            SqlDataReader _Results;
            _con = DatabasePool.GetConnection(psqlCommand, connectToDatabase);
            SqlCommand _cmd = new SqlCommand(psqlCommand, _con.Connection);
            _Results = _cmd.ExecuteReader();
            return _Results;
        }
        public void LoadtblAktenIntPos(string psqlCommand)
        {
            LoadListFromDataReader(GettblAktenIntPosDataReader(psqlCommand));
        }
        public void Load(String where, String order)
        {
            LoadListFromDataReader(GettblAktenIntPosDataReader("SELECT * FROM tblAktenIntPos " + (where != null ? " WHERE " + where : "") + (order != null ? " order by " + order : "")));
        }
        public void LoadAktenIntPosByAktIntPosAkt(int aktIntPosAkt)
        {
            LoadListFromDataReader(GettblAktenIntPosDataReader("select * from tblAktenIntPos where AktIntPosAkt = " + aktIntPosAkt));
        }
        private void LoadListFromDataReader(SqlDataReader dr)
        {
            _lsttblAktenIntPos.Clear();
            RecordLoader.LoadRecordsFromDataReader(
                dr,
                tblAktenIntPosList,
                typeof(tblAktenIntPos), _con);
        }
        #endregion
    }
}