using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using HTB.Database;
using HTB.Database.Views;
using HTBExtras;
using HTBExtras.KingBill;
using HTBUtilities;

namespace HTBReports
{
    public static class ReportFactory
    {
        public static Stream GetZwischenbericht(int aktId)
        {
            var akt = (qryCustInkAkt) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + aktId, typeof (qryCustInkAkt));
            var aktionsList = new ArrayList();

            #region Load Records

            var interventionAktions = new ArrayList();
            var inkassoAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktAktionen WHERE CustInkAktAktionAktID = " + aktId + " ORDER BY CustInkAktAktionDate", typeof (qryCustInkAktAktionen));
            var intAktList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + aktId + " ORDER BY AktIntID DESC", typeof(tblAktenInt));
            if (!string.IsNullOrEmpty(akt.CustInkAktMemo))
                aktionsList.Add(new InkassoActionRecord { ActionID = 9999, ActionDate = DateTime.Now, ActionCaption = "Inkassomemo:", ActionMemo = akt.CustInkAktMemo, IsOnlyMemo = true });

            if (intAktList != null && intAktList.Count > 0)
            {
                foreach (tblAktenInt interventionsAkt in intAktList)
                {
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + interventionsAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof(qryInktAktAction)), interventionAktions);
                    if (interventionsAkt.AKTIntMemo.Trim() != string.Empty)
                    {
                        // add 2 seconds to make sure it shows after the Inkassomemo
                        aktionsList.Add(new InkassoActionRecord { ActionID = 8888, ActionDate = DateTime.Now.AddSeconds(2), ActionCaption = "Interventionsmemo:", ActionMemo = interventionsAkt.AKTIntMemo, IsOnlyMemo = true });
                    }
                }
            }

            ArrayList meldeResults = HTBUtils.GetSqlRecords("SELECT * FROM qryMeldeResult WHERE AMNr = '" + akt.CustInkAktID + "' ORDER BY AMID", typeof(qryMeldeResult));

            #endregion

            #region CombineActions
            tblAktenInt intAkt = (intAktList == null || intAktList.Count == 0) ? null : (tblAktenInt)intAktList[0];
            foreach (qryCustInkAktAktionen inkAction in inkassoAktions)
                if (!inkAction.CustInkAktAktionCaption.Equals("Kosten Änderung"))
                    aktionsList.Add(new InkassoActionRecord(inkAction, intAkt));

            foreach (qryInktAktAction intAction in interventionAktions)
                aktionsList.Add(new InkassoActionRecord(intAction, intAkt));

            foreach (qryMeldeResult melde in meldeResults)
                aktionsList.Add(new InkassoActionRecord(melde, intAkt, false));

            aktionsList.Sort(new InkassoActionRecordComparer());
            #endregion

            var ms = new MemoryStream();
            new Zwischenbericht().GenerateZwischenbericht(akt, aktionsList, "", ms);
            return new MemoryStream(ms.ToArray());
        }
    }
}
