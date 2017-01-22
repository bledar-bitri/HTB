/*
 * ClassName:       tblMahnungSet
 * Author:          Blade
 * Date Created:    02/22/2011
 * Description:     Contains a list of tblMahnung records
 */

using System;

namespace HTB.Database
{
    public class tblMahnungSet : RecordSet
    {
        #region Load
        public tblMahnung GetNewMahnung()
        {
            tblMahnung record = new tblMahnung();
            Random random = new Random();
            record.MahnungNr = random.Next(1000, 10000);
            record.MahnungDate = DateTime.Now;
            InsertRecord(record); // insert record into database
            
            // read the inserted record to get the new ID
            Load("MahnungNr = " + record.MahnungNr + " and MahnungDate = '" + record.MahnungDate + "'", null);
            if (RecordsList.Count > 0)
            {
                record = (tblMahnung)RecordsList[0];
                RecordsList.Clear();
                return record;
            }
            else
            {
                return null; // could either create or read the new record
            }
        }

        public void Load(String where, String order)
        {
            LoadRecords("select * from tblMahnung "+ (where != null ? "where " + where : "") + (order != null ? " order by " + order : ""), typeof(tblMahnung));
        }
        public void LoadMahnungById(int MahnungId)
        {
            LoadRecords("select * from tblMahnung where MahnungID = " + MahnungId, typeof(tblMahnung));
            foreach (tblMahnung record in RecordsList)
                LoadMahnungItems(record);
        }
        private void LoadMahnungItems(tblMahnung record)
        {
            //LoadRecords(record.KostenList, "select * from tblMahnungItems where MahnungID = " + record.MahnungID, typeof(tblMahnungItem));
        }
        #endregion


        #region write
        public bool InsertMahnung(tblMahnung record)
        {
            bool okk = true;
            if (InsertRecord(record))
            {
                foreach (tblMahnungKosten kost in record.KostenList)
                {
                    okk &= InsertRecord(kost);
                }
            }
            return okk;
        }
        public bool UpdateMahnung(tblMahnung record)
        {
            bool okk = true;
            if (UpdateRecord(record))
            {
                DeleteRecord(new tblMahnungKosten(), "MahnungId = " + record.MahnungID);
                foreach (tblMahnungKosten kost in record.KostenList)
                {
                    okk &= InsertRecord(kost);
                }
            }
            return okk;
        }
        public bool InsertKosten(tblMahnung record)
        {
            bool okk = true;
            foreach (tblMahnungKosten kost in record.KostenList)
            {
                okk &= InsertRecord(kost);
            }
            return okk;
        }
        #endregion
    }
}