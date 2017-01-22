using System;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBExtras.KingBill;
using HTBUtilities;
using HTB.Database;
using System.Collections;
using System.IO;
using HTBExtras;
using System.Text;

namespace HTB.v2.intranetx.klienten
{
    public partial class KlientBericht : System.Web.UI.Page
    {
        private string _klientId = string.Empty;
        private tblKlient _klient; 
        protected void Page_Load(object sender, EventArgs e)
        {
            _klientId = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ID]);
            if (_klientId != string.Empty)
            {
                _klient = (tblKlient)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblKlient WHERE KlientId = " + _klientId, typeof(tblKlient));
                if (_klient != null)
                {
                    if (!IsPostBack)
                    {
                        SetValues();
                    }
                    else
                    {
                        btnSubmit.Visible = false;
                    }
                }
            }
        }

        private void SetValues()
        {
            lblKlient.Text = _klient.KlientID.ToString();
            lblKlientName.Text = _klient.KlientName1 + _klient.KlientName2;
        }

        private string GetQuery()
        {
            _klientId = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ID]);

            var enteredDteStart = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtEnteredDateStart);
            var enteredDteEnd = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtEnteredDateEnd);
            var updDteStart = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtUpdatedDateStart);
            var updDteEnd = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtUpdatedDateEnd);
            var sb = new StringBuilder("SELECT * FROM qryCustInkAkt WHERE KlientId = ");
            sb.Append(_klientId);

            if (enteredDteStart != HTBUtils.DefaultDate)
            {
                sb.Append(" AND CustInkAktEnterDate >= '");
                sb.Append(enteredDteStart.ToShortDateString());
                sb.Append("'");
            }
            if (enteredDteEnd != HTBUtils.DefaultDate)
            {
                sb.Append(" AND CustInkAktEnterDate <= '");
                sb.Append(enteredDteEnd.ToShortDateString());
                sb.Append("'");
            }

            if (updDteStart != HTBUtils.DefaultDate)
            {
                sb.Append(" AND CustInkAktLastChange >= '");
                sb.Append(enteredDteEnd.ToShortDateString());
                sb.Append("'");
            }
            if (updDteEnd != HTBUtils.DefaultDate)
            {
                sb.Append(" AND CustInkAktLastChange <= '");
                sb.Append(enteredDteEnd.ToShortDateString());
                sb.Append("'");
            }
            if(chkSkipStorno.Checked)
            {
                sb.Append(" AND CustInkAktStatus <> 5 ");
            }
            sb.Append(" ORDER BY CustInkAktEnterDate desc");
            return sb.ToString();
        }

        private void PrintKlientbericht()
        {
            try
            {
                var aktList = HTBUtils.GetSqlRecords(GetQuery(), typeof(qryCustInkAkt));
                var rpt = new HTBReports.Zwischenbericht();
                var ms = new MemoryStream();
                rpt.Open(ms);
                var firstAkt = true;
                foreach (qryCustInkAkt akt in aktList)
                {
                    if (akt != null)
                    {
                        var aktionsList = new ArrayList();

                        #region Load Records
                        
                        var interventionAktions = new ArrayList();
                        var inkassoAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktAktionen WHERE CustInkAktAktionAktID = " + akt.CustInkAktID + " ORDER BY CustInkAktAktionDate", typeof(qryCustInkAktAktionen));
                        var intAktList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + akt.CustInkAktID + " ORDER BY AktIntID DESC", typeof(tblAktenInt));
                        
                        if (!string.IsNullOrEmpty(akt.CustInkAktMemo))
                            aktionsList.Add(new InkassoActionRecord { ActionID = 9999, ActionDate = DateTime.Now, ActionCaption = "Inkassomemo:", ActionMemo = akt.CustInkAktMemo, IsOnlyMemo = true });

                        if (intAktList != null && intAktList.Count > 0)
                        {
                            foreach (tblAktenInt interventionsAkt in intAktList)
                            {
                                HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + interventionsAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof (qryInktAktAction)), interventionAktions);
                                if (interventionsAkt.AKTIntMemo.Trim() != string.Empty)
                                {
                                    // add 2 seconds to make sure it shows after the Inkassomemo
                                    aktionsList.Add(new InkassoActionRecord {ActionID = 8888, ActionDate = DateTime.Now.AddSeconds(2), ActionCaption = "Interventionsmemo:", ActionMemo = interventionsAkt.AKTIntMemo, IsOnlyMemo = true});
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

                        #region Generate KLientbericht
                        if (!firstAkt)
                        {
                            rpt.NewPage();
                        }
                        else
                        {
                            firstAkt = false;
                        }
                        rpt.GenerateZwischenbericht(akt, aktionsList, "", null, false);

                        #endregion
                    }
                }
                rpt.PrintFooter();
                rpt.CloseReport();
                
                Response.Clear();
                Response.ContentType = "Application/pdf";
                Response.BinaryWrite(ms.ToArray());
                Response.Flush();
                Response.Close();
                Response.End();
            }
            catch (Exception ex)
            {
                btnSubmit.Visible = true;
                ctlMessage.ShowException(ex);
            }
        }


        #region Event Handler
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            PrintKlientbericht();
        }
        #endregion

    }
}