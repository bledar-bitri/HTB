/*
 * ClassName:       qryKostenSet
 * Author:          Blade
 * Date Created:    02/21/2011
 * Description:     Contains a list of qryKosten records
 */

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Text;
using MySql.Data.MySqlClient;
using System;
using System.Reflection;

namespace HTB.Database.Views
{
    public class qryKostenSet
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Property Declaration
        private ArrayList _lstQryKosten = new ArrayList();
        public ArrayList qryKostenList
        {
            get { return _lstQryKosten; }
            set { _lstQryKosten = value; }
        }
        #endregion
        private DbConnection _con;

        public SqlDataReader GetQryKostenDataReader(string psqlCommand, int connectToDatabase = DatabasePool.ConnectToHTB)
        {
            log.Info("SQL: " + psqlCommand);
            SqlDataReader _Results;
            _con = DatabasePool.GetConnection(psqlCommand, connectToDatabase);
            SqlCommand _cmd = new SqlCommand(psqlCommand, _con.Connection);
            _Results = _cmd.ExecuteReader();
            return _Results;
        }
        public void LoadQryKosten(string psqlCommand)
        {
            LoadListFromDataReader(GetQryKostenDataReader(psqlCommand));
        }
        public void LoadAll()
        {
            LoadListFromDataReader(GetQryKostenDataReader("SELECT * FROM qryKosten"));
        }
        public void LoadKostenArt(int artId)
        {
            LoadListFromDataReader(GetQryKostenDataReader("SELECT * FROM qryKosten WHERE KostenArtId = " + artId));
        }
        public void LoadKostenBasedOnForderung(decimal forderung)
        {
            LoadListFromDataReader(GetQryKostenDataReader("SELECT * FROM qryKosten WHERE Von <= " + GetNormalizedDbAmount(forderung) + " and bis >= " + GetNormalizedDbAmount(forderung)));
        }
        public void LoadInitialKostenBasedOnForderung(decimal forderung)
        {
            LoadListFromDataReader(GetQryKostenDataReader("SELECT * FROM qryKosten WHERE IsImErstenSchritt = 1 AND IsZinsen = 0 AND Von <= " + GetNormalizedDbAmount(forderung) + " and bis >= " + GetNormalizedDbAmount(forderung)));
        }
        public void LoadZinsen()
        {
            LoadListFromDataReader(GetQryKostenDataReader("SELECT * FROM qryKosten WHERE IsZinsen = 1"));
        }
        public void LoadTerminverlust()
        {
            LoadListFromDataReader(GetQryKostenDataReader("SELECT * FROM qryKosten WHERE IsTerminverlust = 1"));
        }
        public void LoadKostenBasedOnForderungAndArtId(decimal forderung, List<int> kostenArtId)
        {
            if (kostenArtId.Count > 0)
            {
                var sb = new StringBuilder("KostenArtId in (");
                foreach (int i in kostenArtId)
                {
                    sb.Append(i.ToString());
                    sb.Append(", ");
                }
                sb.Remove(sb.Length - 2, 2); // remove last comma
                sb.Append(")");
                LoadListFromDataReader(GetQryKostenDataReader("SELECT * FROM qryKosten WHERE " + sb.ToString() + " AND Von <= " + GetNormalizedDbAmount(forderung) + " and bis >= " + GetNormalizedDbAmount(forderung)));
            }
        }
        private string GetNormalizedDbAmount(decimal amt)
        {
            return amt.ToString().Replace(",", ".");
        }
        private void LoadListFromDataReader(SqlDataReader dr)
        {
            _lstQryKosten.Clear();
            RecordLoader.LoadRecordsFromDataReader(
                dr,
                qryKostenList,
                typeof(qryKosten), _con);
        }
    }
}