using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBAktLayer;
using HTBExtras.KingBill;
using HTBUtilities;
using HTB.Database;
using System.Collections;
using System.ComponentModel;
using System.IO;
using HTBExtras;

namespace HTB.v2.intranetx.global_forms
{
    public partial class Zwischenbericht : System.Web.UI.Page
    {
        private static string _aktId = string.Empty;
        private qryCustInkAkt _akt;
        private ArrayList _intAktList = new ArrayList();
        private ArrayList _inkassoAktions = new ArrayList();
        private readonly ArrayList _interventionAktions = new ArrayList();
        private readonly ArrayList _aktionsList = new ArrayList();
        private ArrayList _meldeResults = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            _aktId = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ID]);
            if (_aktId != string.Empty)
            {
                _akt = (qryCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + _aktId, typeof(qryCustInkAkt));
                if (_akt != null)
                {
                    LoadRecords();
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

        private void LoadRecords()
        {
            _interventionAktions.Clear();
            _inkassoAktions = HTBUtils.GetSqlRecords("SELECT * FROM qryCustInkAktAktionen WHERE CustInkAktAktionAktID = " + _aktId + " ORDER BY CustInkAktAktionDate", typeof(qryCustInkAktAktionen));
            _meldeResults = HTBUtils.GetSqlRecords("SELECT * FROM qryMeldeResult WHERE AMNr = '" + _akt.CustInkAktID + "' ORDER BY AMID", typeof(qryMeldeResult));
            _intAktList = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenInt WHERE AktIntCustInkAktID = " + _aktId + " ORDER BY AktIntID DESC", typeof(tblAktenInt));
            
            if (!string.IsNullOrEmpty(_akt.CustInkAktMemo))
                _aktionsList.Add(new InkassoActionRecord { ActionID = 9999, ActionDate = DateTime.Now, ActionCaption = "Inkassomemo:", ActionMemo = _akt.CustInkAktMemo, IsOnlyMemo = true });

            if (_intAktList != null && _intAktList.Count > 0)
            {
                foreach (tblAktenInt intAkt in _intAktList)
                {
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + intAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof(qryInktAktAction)), _interventionAktions);
                    if (intAkt.AKTIntMemo.Trim() != string.Empty)
                    {
                        // add 2 seconds to make sure it shows after the Inkassomemo
                        _aktionsList.Add(new InkassoActionRecord {ActionID = 8888, ActionDate = DateTime.Now.AddSeconds(2), ActionCaption = "Interventionsmemo:", ActionMemo = intAkt.AKTIntMemo, IsOnlyMemo = true});
                    }
                }
            }
        }

        private void SetValues()
        {
            lblAktNr.Text = _akt.CustInkAktID.ToString();
            lblAZ.Text = _akt.CustInkAktAZ.Trim() == "" ? "&nbsp;" : _akt.CustInkAktAZ;
            lblAuftraggeber.Text = _akt.CustInkAktAuftraggeber.ToString();
            lblAuftraggeberName.Text = _akt.AuftraggeberName1 + _akt.AuftraggeberName2;
            lblKlient.Text = _akt.CustInkAktKlient.ToString();
            lblKlientName.Text = _akt.KlientName1 + _akt.KlientName2;
            lblGegner.Text = _akt.CustInkAktGegner.ToString();
            lblGegnerName.Text = _akt.GegnerLastName1 + _akt.GegnerLastName2;
        }

        private void CombineActions()
        {
            tblAktenInt intAkt = _intAktList.Count == 0 ? null : (tblAktenInt)_intAktList[0];
            foreach (qryCustInkAktAktionen inkAction in _inkassoAktions)
                if (!inkAction.CustInkAktAktionCaption.Equals("Kosten Änderung"))
                    _aktionsList.Add(new InkassoActionRecord(inkAction, intAkt));

            foreach (qryInktAktAction intAction in _interventionAktions)
                _aktionsList.Add(new InkassoActionRecord(intAction, intAkt));

            foreach (qryMeldeResult melde in _meldeResults)
                _aktionsList.Add(new InkassoActionRecord(melde, intAkt, false));

            _aktionsList.Sort(new InkassoActionRecordComparer());
        }

        public void LoadActions()
        {
            if (_aktId != string.Empty)
            {
                _akt = (qryCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + _aktId, typeof(qryCustInkAkt));
                if (_akt != null)
                {
                    LoadRecords();
                    CombineActions();
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InkassoActionRecord> GetActions()
        {
            LoadActions();
            return _aktionsList.Cast<InkassoActionRecord>().ToList();
        }

        private void PrintZwischenbericht()
        {
            var ms = new MemoryStream();
            var rpt = new HTBReports.Zwischenbericht();
            
            rpt.GenerateZwischenbericht(_akt, GetSelectedActions(), txtMemo.Text, ms);

            Response.Clear();
            Response.ContentType = "Application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        public ArrayList GetSelectedActions()
        {

            LoadActions();
            var list = new ArrayList();

            for (int i = 0; i < gvActions.Rows.Count; i++)
            {
                GridViewRow row = gvActions.Rows[i];
                var chkSelected = (CheckBox)row.FindControl("chkSelected");

                if (chkSelected.Checked)
                {
                    Label lblActionId = (Label)row.FindControl("lblActionID");

                    foreach (InkassoActionRecord action in _aktionsList)
                    {
                        if (action.ActionID.ToString() == lblActionId.Text)
                        {
                            list.Add(action);
                            break;
                        }
                    }
                }
            }
            return list;
        }

        #region Event Handler
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Create action
            new AktUtils(_akt.CustInkAktID).CreateAktAktion(69, GlobalUtilArea.GetUserId(Session), txtMemo.Text.Replace("'", ""));
            
            PrintZwischenbericht();
        }
        #endregion

    }
}