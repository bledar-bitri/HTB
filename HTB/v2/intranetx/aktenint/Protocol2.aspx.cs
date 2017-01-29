using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HTB.Database.HTB.Views;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTB.v2.intranetx.glocalcode;
using System.Collections;
using HTB.Database;
using HTBReports;
using HTBUtilities;
using Microsoft.VisualBasic.Logging;

namespace HTB.intranetx.aktenint
{
    public partial class Protocol2 : System.Web.UI.Page
    {

        public IncControls controls;

        private readonly ArrayList _inkassoRows = new ArrayList();
        private readonly ArrayList _sicherstellungRows = new ArrayList();
        private readonly ArrayList _negativRows = new ArrayList();

        private tblProtokol _protokol;
        private qryAktenInt _akt;
        private qryAktenIntActionWithType _action;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.ID]);
            Initialize();
            if (aktId > 0)
            {
                _protokol = (tblProtokol)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblProtokol WHERE AktIntID = " + aktId + " ORDER BY ProtokolID DESC", typeof(tblProtokol));
                if(_protokol == null)
                {
                    _protokol = new tblProtokol { ProtokolAkt = aktId };
                    RecordSet.Insert(_protokol);
                     _protokol = (tblProtokol)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 ProtokolID FROM tblProtokol ORDER BY ProtokolID DESC", typeof(tblProtokol));
                }
                if (_protokol != null)
                {
                    _akt = (qryAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + _protokol.ProtokolAkt, typeof(qryAktenInt));
                    _action = (qryAktenIntActionWithType)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntActionWithType WHERE AktIntActionIsInternal = 0 AND AktIntActionAkt = " + _protokol.ProtokolAkt + " ORDER BY AktIntActionTime DESC", typeof(qryAktenIntActionWithType));
                    if (_action != null)
                    {
                        SetRowsVisible();
                        if (!IsPostBack)
                        {
                            GlobalUtilArea.LoadDropdownList(ddlServiceheft,
                                                            "SELECT ProtokolServiceheftID, ProtokolServiceheftText FROM dbo.tblProtokolServiceheft ORDER BY ProtokolServiceheftID",
                                                            typeof (tblProtokolServiceheft),
                                                            "ProtokolServiceheftText",
                                                            "ProtokolServiceheftText", true);
                            SetValues();
                        }
                    }
                    else
                    {
                        ctlMessage.ShowError("Kein Aktion!!!");
                        SetRowsVisible();
                    }
                }
            }
        }

        private void Initialize()
        {
            Response.Cache.SetExpires(DateTime.Parse(DateTime.Now.ToString()));
            Response.Cache.SetCacheability(HttpCacheability.Private);
            controls = new IncControls(Request);

            LoadProtokolRowList();
            SetAllRowsVisible(false);
        }
        
        private void SetValues()
        {

            lblVerrechnungsart.Text = _action.AktIntActionTypeCaption;
            txtRechnungsNr.Text = _protokol.RechnungNr;
            if(HTBUtils.IsDateValid(_protokol.SicherstellungDatum))
            {
                txtDatum.Text = _protokol.SicherstellungDatum.ToShortDateString();
                txtUhrzeit.Text = _protokol.SicherstellungDatum.ToShortTimeString();
            }
            txtOrtDerUbernahme.Text = _protokol.UbernahmeOrt;
            ddlKZ.SelectedValue = _protokol.KZ;
            ddlZulassung.SelectedValue = _protokol.UbernommentMitZulassung ? "Ja" : "Nein";
            ddlServiceheft.SelectedValue = _protokol.Serviceheft;
            txtAnzahlSchlussel.Text = _protokol.AnzahlSchlussel.ToString();
            txtKMStand.Text = _protokol.Tachometer.ToString();
            txtSchaden.Text = _protokol.SchadenComment;
            txtErweiterterBericht.Text = _protokol.Memo;
            ddlAbschleppdienst.SelectedValue = _protokol.Abschleppdienst ? "Ja" : "Nein";
            txtAbschleppdienstName.Text = _protokol.AbschleppdienstName;
            txtZusatzkostenTreibstoff.Text = _protokol.ZusatzkostenTreibstoff > 0 ? HTBUtils.FormatCurrencyNumber(_protokol.ZusatzkostenTreibstoff) : "";
            txtZusatzkostenVignette.Text = _protokol.ZusatzkostenVignette > 0 ? HTBUtils.FormatCurrencyNumber(_protokol.ZusatzkostenVignette) : "";
            txtZusatzkostenSonstige.Text = _protokol.ZusatzkostenSostige > 0 ? HTBUtils.FormatCurrencyNumber(_protokol.ZusatzkostenSostige) : "";
            txtHandler.Text = _protokol.HandlerName;
            ddlPolizeiInformiert.SelectedValue = _protokol.PolizieInformiert ? "Ja" : "Nein";
            txtBeifahrer.Text = _protokol.Beifahrer;
            txtUbernommenVon.Text = _protokol.UbernommenVon;
        }

        private void LoadProtokolFromScreen()
        {
            _protokol.SicherstellungDatum = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDatum);
            if(HTBUtils.IsDateValid(_protokol.SicherstellungDatum))
            {
                _protokol.SicherstellungDatum = GlobalUtilArea.GetDefaultDateIfConvertToDateError(_protokol.SicherstellungDatum.ToShortDateString() + " " + txtUhrzeit.Text);
            }
            _protokol.RechnungNr = txtRechnungsNr.Text;
            _protokol.UbernahmeOrt = txtOrtDerUbernahme.Text;
            _protokol.KZ = ddlKZ.SelectedValue;
            _protokol.UbernommentMitZulassung = ddlZulassung.SelectedValue == "Ja";
            _protokol.Serviceheft = ddlServiceheft.SelectedValue;
            _protokol.AnzahlSchlussel = GlobalUtilArea.GetZeroIfConvertToIntError(txtAnzahlSchlussel.Text);
            _protokol.Tachometer = GlobalUtilArea.GetZeroIfConvertToIntError(txtKMStand.Text);
            _protokol.SchadenComment = txtSchaden.Text;
            _protokol.Memo = txtErweiterterBericht.Text;
            _protokol.Abschleppdienst = ddlAbschleppdienst.SelectedValue  == "Ja";
            _protokol.AbschleppdienstName = txtAbschleppdienstName.Text;
            _protokol.ZusatzkostenTreibstoff = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtZusatzkostenTreibstoff);
            _protokol.ZusatzkostenVignette = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtZusatzkostenVignette);
            _protokol.ZusatzkostenSostige = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtZusatzkostenSonstige);
            _protokol.HandlerName = txtHandler.Text;
            _protokol.PolizieInformiert = ddlPolizeiInformiert.SelectedValue == "Ja";
            _protokol.Beifahrer = txtBeifahrer.Text;
            _protokol.UbernommenVon = txtUbernommenVon.Text;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if(SaveProtocol())
                bdy.Attributes.Add("onload", "window.close();");
        }

        protected void btnSaveAndGeneratePDF_Click(object sender, EventArgs e)
        {
            try
            {
                if(SaveProtocol())
                {
                    bool ok = true;

                    var akt = (qryAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + _protokol.ProtokolAkt, typeof(qryAktenInt));
                    var action = (qryAktenIntActionWithType)HTBUtils.GetSqlSingleRecord($"SELECT * FROM qryAktenIntActionWithType WHERE AktIntActionIsInternal = 0 AND AktIntActionAkt = " + _protokol.ProtokolAkt + " ORDER BY AktIntActionTime DESC", typeof(qryAktenIntActionWithType));
                    var protokolUbername = (tblProtokolUbername)HTBUtils.GetSqlSingleRecord($"SELECT * FROM tblProtokolUbername WHERE UbernameAktIntID = { akt.AktIntID } ORDER BY UbernameDatum DESC", typeof(tblProtokolUbername));

                    //                    var protocolVisits = HTBUtils.GetSqlRecords("SELECT * FROM tblProtokolBesuch WHERE ProtokolID = " + _protokol.ProtokolID, typeof(tblProtokolBesuch));

                    ArrayList docsList = HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + _protokol.ProtokolAkt, typeof(qryDoksIntAkten));
                    if (akt.IsInkasso())
                        HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksInkAkten WHERE CustInkAktID = " + akt.AktIntCustInkAktID, typeof(qryDoksInkAkten)), docsList);


                    var fileName = "Protokoll_" + akt.AktIntAZ + ".pdf";
                    string filepath = HTBUtils.GetConfigValue("DocumentsFolder") + fileName;
                    FileStream ms = File.Exists(filepath) ? new FileStream(filepath, FileMode.Truncate) : new FileStream(filepath, FileMode.Create);

                    var rpt = new ProtokolTablet();
                    try
                    {
//                        rpt.GenerateProtokol(akt, _protokol, action, ms, protocolVisits.Cast<tblProtokolBesuch>().ToList(), docsList.Cast<Record>().ToList());
                        rpt.GenerateProtokol(akt, _protokol, protokolUbername, action, ms, GlobalUtilArea.GetVisitedDates(akt.AktIntID), GlobalUtilArea.GetPosList(akt.AktIntID), docsList.Cast<Record>().ToList());
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        ctlMessage.ShowException(ex);
                    }
                    finally
                    {
                        ms.Close();
                        ms.Dispose();
                    }
                    if (ok)
                    {
                        SaveDocumentRecord(_akt.AktIntID, fileName, GlobalUtilArea.GetUserId(Session));
                        Response.Redirect("/v2/intranet/documents/files/" + fileName);
                    }
                }

            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
       
        private bool SaveProtocol()
        {
            LoadProtokolFromScreen();
            bool ok = true;
            ok = _protokol.ProtokolID <= 0 ? RecordSet.Insert(_protokol) : RecordSet.Update(_protokol);
            return ok;
        }

        private void SetRowsVisible()
        {
            if(_action != null)
            {
                if (_action.AktIntActionIsAutoRepossessed)
                    SetRowsVisible(_sicherstellungRows, true);
                else if (_action.AktIntActionIsAutoMoneyCollected || _action.AktIntActionIsAutoPayment)
                    SetRowsVisible(_inkassoRows, true);
                else
                    SetRowsVisible(_negativRows, true);
            }
        }

        private void SetAllRowsVisible(bool visible)
        {
            SetRowsVisible(_inkassoRows, visible);
            SetRowsVisible(_sicherstellungRows, visible);
            SetRowsVisible(_negativRows, visible);
        }

        private void SetRowsVisible(ArrayList plist, bool visible)
        {
            foreach (HtmlTableRow row in plist)
            {
                row.Visible = visible;
            }
        }

        private void LoadProtokolRowList()
        {
            _inkassoRows.Add(trVerrechnungsart);
            _inkassoRows.Add(trRechnungNr);
            _inkassoRows.Add(trDatum);
            //inkassoRows.Add(trSchaden);
            _inkassoRows.Add(trErweiterterBericht);

            _sicherstellungRows.Add(trVerrechnungsart);
            _sicherstellungRows.Add(trRechnungNr);
            _sicherstellungRows.Add(trDatum);
            _sicherstellungRows.Add(trOrt);
            _sicherstellungRows.Add(trKZ);
            _sicherstellungRows.Add(trZulassung);
            _sicherstellungRows.Add(trServiceheft);
            _sicherstellungRows.Add(trAnzahlSchlussel);
            _sicherstellungRows.Add(trKMStand);
            _sicherstellungRows.Add(trSchaden);
            _sicherstellungRows.Add(trErweiterterBericht);
            _sicherstellungRows.Add(trAbschleppdienst);
            _sicherstellungRows.Add(trAbschleppdienstName);
            _sicherstellungRows.Add(trZusatzkostenTreibstoff);
            _sicherstellungRows.Add(trZusatzkostenVignette);
            _sicherstellungRows.Add(trZusatzkostenSonstige);
            _sicherstellungRows.Add(trUberstellungsdistanz);
            _sicherstellungRows.Add(trHandler);
            _sicherstellungRows.Add(trPolizeiInformiert);
            _sicherstellungRows.Add(trBeifahrer);
            _sicherstellungRows.Add(trUbernommenVon);

            _negativRows.Add(trVerrechnungsart);
            _negativRows.Add(trRechnungNr);
            _negativRows.Add(trDatum);
            //negativRows.Add(trSchaden);
            _negativRows.Add(trErweiterterBericht);
        }

        private void SaveDocumentRecord(int aktNumber, string fileName, int userId)
        {
            var doc = (tblDokument) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblDokument WHERE DokInkAkt = " + aktNumber + " AND DokAttachment = '" + fileName + "'", typeof (tblDokument));
            if (doc == null)
            {
                doc = new tblDokument
                          {
                              // CollectionInvoice
                              DokDokType = 25,
                              DokCaption = "Protokol",
                              DokInkAkt = aktNumber,
                              DokCreator = userId,
                              DokAttachment = fileName,
                              DokCreationTimeStamp = DateTime.Now,
                              DokChangeDate = DateTime.Now
                          };
                RecordSet.Insert(doc);

                doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof(tblDokument));
                if (doc != null)
                {
                    RecordSet.Insert(new tblAktenDokumente { ADAkt = aktNumber, ADDok = doc.DokID, ADAkttyp = 3 });
                }
            }
            else
            {
                doc.DokChangeDate = DateTime.Now;
                doc.DokChangeUser = userId;
                doc.DokCreator = userId;
                RecordSet.Update(doc);
            }
        }
    }
}