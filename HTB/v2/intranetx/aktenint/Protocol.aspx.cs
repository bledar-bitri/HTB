using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTB.v2.intranetx.glocalcode;
using System.Collections;
using HTB.Database;
using HTBReports;
using HTBUtilities;

namespace HTB.intranetx.aktenint
{
    public partial class Protocol : System.Web.UI.Page
    {

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public IncControls controls;

        private readonly ArrayList _inkassoRows = new ArrayList();
        private readonly ArrayList _sicherstellungRows = new ArrayList();
        private readonly ArrayList _negativRows = new ArrayList();

        private tblProtokol _protokol;
        private tblProtokolUbername _protokolUbername;
        private qryAktenInt _akt;
        private qryAktenIntActionWithType _action;
        private ArrayList _protocolVisits;

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
                    _action = (qryAktenIntActionWithType)HTBUtils.GetSqlSingleRecord($"SELECT * FROM qryAktenIntActionWithType WHERE AktIntActionIsInternal = 0 AND AktIntActionAkt = {_protokol.ProtokolAkt } ORDER BY AktIntActionTime DESC", typeof(qryAktenIntActionWithType));
                    _protocolVisits = HTBUtils.GetSqlRecords($"SELECT * FROM tblProtokolBesuch WHERE ProtokolID = {_protokol.ProtokolID}", typeof(tblProtokolBesuch));
                    _protokolUbername = (tblProtokolUbername)HTBUtils.GetSqlSingleRecord($"SELECT * FROM tblProtokolUbername WHERE UbernameAktIntID = { _protokol.ProtokolAkt} ORDER BY UbernameDatum DESC" , typeof(tblProtokolUbername));

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
            SetVisitDateValues();

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

            txtVersicherungBarKassiert.Text = HTBUtils.FormatCurrencyNumber(_protokol.VersicherungBarKassiert);
            txtVersicherungUberwiesenAm.Text = GlobalUtilArea.GetStringValueForNonDefaultDate(_protokol.VersicherungUberwiesen);
            txtForderungBarKassiert.Text = HTBUtils.FormatCurrencyNumber(_protokol.ForderungBarKassiert);
            txtForderungUberwiesenAm.Text = GlobalUtilArea.GetStringValueForNonDefaultDate(_protokol.ForderungUberwiesen);
            txtKostenBarKassiert.Text = HTBUtils.FormatCurrencyNumber(_protokol.KostenBarKassiert);
            txtKostenUberwiesenAm.Text = GlobalUtilArea.GetStringValueForNonDefaultDate(_protokol.KostenUberwiesen);
            txtDirektzahlung.Text = HTBUtils.FormatCurrencyNumber(_protokol.Direktzahlung);
            txtDirektzahlungAm.Text = GlobalUtilArea.GetStringValueForNonDefaultDate(_protokol.DirektzahlungAm);
            txtDirektzahlungVersicherung.Text = HTBUtils.FormatCurrencyNumber(_protokol.DirektzahlungVersicherung);
            txtDirektzahlungVersicherungAm.Text = GlobalUtilArea.GetStringValueForNonDefaultDate(_protokol.DirektzahlungVersicherungAm);

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
            try
            {
                _protokol.VersicherungBarKassiert = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtVersicherungBarKassiert.Text);
                _protokol.VersicherungUberwiesen = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtVersicherungUberwiesenAm.Text);
            }catch
            {
                _protokol.VersicherungBarKassiert = 0;
                _protokol.VersicherungUberwiesen = HTBUtils.DefaultDate;
            }
            try
            {
                _protokol.ForderungBarKassiert = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtForderungBarKassiert.Text);
                _protokol.ForderungUberwiesen = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtForderungUberwiesenAm.Text);
            }
            catch
            {
                _protokol.ForderungBarKassiert = 0;
                _protokol.ForderungUberwiesen = HTBUtils.DefaultDate;
            }
            try
            {
                _protokol.KostenBarKassiert = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtKostenBarKassiert.Text);
                _protokol.KostenUberwiesen = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtKostenUberwiesenAm.Text);
            }
            catch
            {
                _protokol.KostenBarKassiert = 0;
                _protokol.KostenUberwiesen = HTBUtils.DefaultDate;
            }
            try
            {
                _protokol.Direktzahlung = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtDirektzahlung.Text);
                _protokol.DirektzahlungAm = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDirektzahlungAm.Text);
            }
            catch
            {
                _protokol.Direktzahlung = 0;
                _protokol.DirektzahlungAm = HTBUtils.DefaultDate;
            }
            try
            {
                _protokol.DirektzahlungVersicherung = GlobalUtilArea.GetZeroIfConvertToDoubleError(txtDirektzahlungVersicherung.Text);
                _protokol.DirektzahlungVersicherungAm = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDirektzahlungVersicherungAm.Text);
            }
            catch
            {
                _protokol.DirektzahlungVersicherung = 0;
                _protokol.DirektzahlungVersicherungAm = HTBUtils.DefaultDate;
            }

            if(_protocolVisits == null)
                _protocolVisits = new ArrayList();

            _protocolVisits.Clear();
            _protocolVisits.Add(new tblProtokolBesuch{BesuchAm = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBesuch1.Text)});
            _protocolVisits.Add(new tblProtokolBesuch{BesuchAm = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBesuch2.Text)});
            _protocolVisits.Add(new tblProtokolBesuch{BesuchAm = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBesuch3.Text)});
            _protocolVisits.Add(new tblProtokolBesuch{BesuchAm = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBesuch4.Text)});
            _protocolVisits.Add(new tblProtokolBesuch{BesuchAm = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtBesuch5.Text)});
            
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
                if (!SaveProtocol()) 
                    return;

                bool ok = true;

                var akt = (qryAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + _protokol.ProtokolAkt, typeof(qryAktenInt));
                var action = (qryAktenIntActionWithType)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntActionWithType WHERE AktIntActionIsInternal = 0 AND AktIntActionAkt = " + _protokol.ProtokolAkt + " ORDER BY AktIntActionTime DESC", typeof(qryAktenIntActionWithType));
                _protokolUbername = (tblProtokolUbername)HTBUtils.GetSqlSingleRecord($"SELECT * FROM tblProtokolUbername WHERE UbernameAktIntID = { _protokol.ProtokolAkt} ORDER BY UbernameDatum DESC", typeof(tblProtokolUbername));

                ArrayList docsList = HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + _protokol.ProtokolAkt, typeof(qryDoksIntAkten));
                if (akt.IsInkasso())
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksInkAkten WHERE CustInkAktID = " + akt.AktIntCustInkAktID, typeof(qryDoksInkAkten)), docsList);


                var fileName = "Protokoll_" + akt.AktIntAZ + ".pdf";
                var filepath = HTBUtils.GetConfigValue("DocumentsFolder") + fileName;
                var ms = File.Exists(filepath) ? new FileStream(filepath, FileMode.Truncate) : new FileStream(filepath, FileMode.Create);

                var rpt = new ProtokolTablet();
                try
                {
                    var visitedDates = GlobalUtilArea.GetVisitedDates(akt.AktIntID);
                    var posList = GlobalUtilArea.GetPosList(akt.AktIntID);

                    if (action == null)
                    {
                        Log.Error("Could not find Akt Actions... action is null");
                        ctlMessage.ShowError("Keine Protokollaktionen wurden gefunden. Bitte einen Aktion erstellen und noch mal probieren.");
                        ok = false;
                    }
                    if (visitedDates == null)
                        Log.Error("Could not find Akt Visited Dates... visitedDates is null");
                    if (posList == null)
                        Log.Error("Could not find Akt Pos List... posList is null");
                    if (docsList == null)
                        Log.Error("Could not find Akt Documents... docsList is null");

                    if (ok)
                        rpt.GenerateProtokol(akt, _protokol, _protokolUbername, action, ms, GlobalUtilArea.GetVisitedDates(akt.AktIntID), GlobalUtilArea.GetPosList(akt.AktIntID), docsList.Cast<Record>().ToList());
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
                
                if (!ok) 
                    return;
                SaveDocumentRecord(_akt.AktIntID, fileName, GlobalUtilArea.GetUserId(Session));
                Response.Redirect("/v2/intranet/documents/files/" + fileName);
            }
            catch (Exception ex)
            {
                ctlMessage.ShowException(ex);
            }
        }
       
        private bool SaveProtocol()
        {
            var set = new RecordSet();
            LoadProtokolFromScreen();
            bool ok = true;
            if(_protokol.ProtokolID <= 0)
                ok = RecordSet.Insert(_protokol);
            else
                ok = RecordSet.Update(_protokol);

            var sb = new StringBuilder("DELETE tblProtokolBesuch WHERE ProtokolID = ");
            sb.Append(_protokol.ProtokolID);
            sb.Append("; ");
            
            foreach (tblProtokolBesuch visit in _protocolVisits)
            {
                if (HTBUtils.IsDateValid(visit.BesuchAm))
                {
                    sb.Append("INSERT INTO tblProtokolBesuch VALUES (");
                    sb.Append(_protokol.ProtokolID);
                    sb.Append(", '");
                    sb.Append(visit.BesuchAm.ToShortDateString());
                    sb.Append("'); ");
                }
                try
                {
                    set.ExecuteNonQuery(sb.ToString());
                }
                catch (Exception ex)
                {
                    ctlMessage.ShowException(ex);
                    ok = false;
                }
            }
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
            _inkassoRows.Add(trBesuch1);
            _inkassoRows.Add(trBesuch2);
            _inkassoRows.Add(trBesuch3);
            _inkassoRows.Add(trBesuch4);
            _inkassoRows.Add(trBesuch5);
            //inkassoRows.Add(trSchaden);
            _inkassoRows.Add(trVersicherungBarKassiert);
            _inkassoRows.Add(trForderungBarKassiert);
            _inkassoRows.Add(trKostenBarKassiert);
            _inkassoRows.Add(trDirektzahlung);
            _inkassoRows.Add(trDirektzahlungVersicherung);
            _inkassoRows.Add(trErweiterterBericht);

            _sicherstellungRows.Add(trVerrechnungsart);
            _sicherstellungRows.Add(trRechnungNr);
            _sicherstellungRows.Add(trDatum);
            _sicherstellungRows.Add(trBesuch1);
            _sicherstellungRows.Add(trBesuch2);
            _sicherstellungRows.Add(trBesuch3);
            _sicherstellungRows.Add(trBesuch4);
            _sicherstellungRows.Add(trBesuch5);
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
            _negativRows.Add(trBesuch1);
            _negativRows.Add(trBesuch2);
            _negativRows.Add(trBesuch3);
            _negativRows.Add(trBesuch4);
            _negativRows.Add(trBesuch5);
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

        private void SetVisitDateValues()
        {
            try
            {
                int idx = 0;
                SetVisitDate(txtBesuch1, idx++);
                SetVisitDate(txtBesuch2, idx++);
                SetVisitDate(txtBesuch3, idx++);
                SetVisitDate(txtBesuch4, idx++);
                SetVisitDate(txtBesuch5, idx);
            }
            catch
            {
            }
        }

        private void SetVisitDate(TextBox textBox, int idx)
        {
            if(_protocolVisits.Count > idx)
            {
                var visitRec = (tblProtokolBesuch) _protocolVisits[idx];
                if(visitRec != null)
                {
                    textBox.Text = visitRec.BesuchAm.ToShortDateString();
                }
            }
        }
    }
}