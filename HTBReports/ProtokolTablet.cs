using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using HTB.Database;
using HTB.Database.Views;
using HTBExtras;
using HTBPdf;
using HTBUtilities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace HTBReports
{
    public class ProtokolTablet : IReport
    {


        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const int MiddleCol = 1050;
        private const int RightCol = 2000;
        private const int StartLine = 30;
        private const int NormalGap = 45;
        private const int MaxLines = 2600;

        private const string DateFormat = "dd.MM.yyyy";
        private const string TimeFormat = "HH:mm";
        private const string ja = "ja";
        private const string nein = "nein";

        private int _col1;
        private int _col2;
        private int _col3;
        private int _lin;
        private int _gap;

        private ECPPdfWriter Writer { get; set; }
   
        
        private readonly string _logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");

        private tblProtokol _protokol;
        private tblProtokolUbername _ubernameProtokol;
        private qryAktenInt _akt;
        private qryAktenIntActionWithType _action;
        private List<tblProtokolBesuch> _visits;
        private List<VisitRecord> _visitRecordList;
        private List<tblAktenIntPos> _invoices;
        private List<string> _emailAddresses;
        private List<Record> _documents;

        private void Init()
        {
            _lin = StartLine;
            _gap = NormalGap;
            _col1 = 150;
            _col2 = _col1 + 700;
            _col3 = _col2 + 1000;
            if(_action != null && _action.AktIntActionTypeID == 90) // Direktzahlung
            {
                _action.AktIntActionTypeCaption = "Abschluss ohne Berechnung";
            }
        }
        
        public void GenerateProtokol(qryAktenInt akt, tblProtokol protokol, tblProtokolUbername ubernameProtokol, qryAktenIntActionWithType action, Stream os, List<VisitRecord> visits, List<tblAktenIntPos> invoices, List<Record> documents = null, List<string> emailAddresses = null)
        {
            _akt = akt;
            _protokol = protokol;
            _ubernameProtokol = ubernameProtokol;
            _action = action;
            _visitRecordList = visits;
            _invoices = invoices;
            _documents = documents;
            _emailAddresses = emailAddresses;
            
            Init();
            Open(os);
            if (_akt.AuftraggeberUseMerzedesProtocol && _action.AktIntActionIsAutoRepossessed)
            {
                PrintRepossessionForMerzedes();
            }
            else
            {
                PrintPageHeader();
                PrintProtokol();
                PrintDocuments(documents);
            }
            Close();
        }

        public void GenerateOfficeProtokol(qryAktenInt akt, tblProtokol protokol, qryAktenIntActionWithType action, Stream os, List<VisitRecord> visits, List<tblAktenIntPos> invoices, List<Record> documents = null)
        {
            _akt = akt;
            _protokol = protokol;
            _action = action;
            _visitRecordList = visits;
            _invoices = invoices;
            Init();
            Open(os);
            PrintPageHeader();
            PrintProtokol();
            //PrintOfficeCharges();
            Close();
        }

        public void GenerateDealerProtokol(qryAktenInt akt, tblProtokol protokol, Stream os, List<string> emailAddresses = null)
        {
            _akt = akt;
            _protokol = protokol;

            _action = new qryAktenIntActionWithType {AktIntActionTime = DateTime.Now};
            _emailAddresses = emailAddresses;

            Init();
            Open(os);
            if (_akt.AuftraggeberUseMerzedesProtocol)
            {
                PrintRepossessionForMerzedes();
            }
            else
            {
                PrintPageHeader();
                PrintRepossession(true);
            }
            Writer.print(2900, 1800, DateTime.Now.ToShortDateString(), 'R');

            Close();
        }

        private void PrintProtokol()
        {
            if (_action.AktIntActionIsAutoRepossessed)
                PrintRepossession(false);
            else if (_action.AktIntActionIsAutoMoneyCollected)
                PrintMoneyCollected();
            else //if (_action.AktIntActionIsAutoNegative)
                PrintBericht();
        }

        private void PrintRepossession(bool isDealerProtocol)
        {
            #region declare and set variables
            var col2 = _col2 + 350;
            DateTime dte = _protokol.SicherstellungDatum;
            if (!HTBUtils.IsDateValid(dte))
            {
                dte = _action.AktIntActionTime;
            }

            string formattedSicherstellungsDatum = dte.ToString(DateFormat);
            string formattedSicherstellungsZeit = "";
            if (dte.ToShortTimeString() == "00:00")
                WriteInfo(dte.ToShortDateString());
            else
                formattedSicherstellungsZeit = dte.ToString(TimeFormat);

            dte = _ubernameProtokol?.UbernameAusloesenDatum ?? DateTime.MinValue;
            string formattedAusloseDatum = HTBUtils.IsDateValid(dte) ? dte.ToString(DateFormat) : "";
            string formattedAusloseZeit = HTBUtils.IsDateValid(dte) && dte.ToShortTimeString() != "00:00" ? dte.ToString(TimeFormat) : "";

            var ubernameTachometer = _ubernameProtokol?.UbernameTachometer.ToString() ?? "";
            
            var kfzDurchBehordeAbgemeldet = _ubernameProtokol != null && _ubernameProtokol.UbernameKzDurchBehoerdeEingezogen ? ja : nein;
            var typenschein = _ubernameProtokol != null && _ubernameProtokol.UbernameTypenschein ? ja : nein;
            var mochteAuslosen = _ubernameProtokol != null && _ubernameProtokol.UbernameMoechteAusloesen ? ja : nein;

            #endregion

            SetHeadingFont();
            _lin += _gap*2;

            
            WriteUnderlinedSubheader("Auftragsdaten:", _col1);
            _lin += _gap+20;
            
            PrintHeaderAndInfo("Kunde: ", _akt.GegnerLastName1 + " " + _akt.Gegner2LastName2, _col1);
            PrintHeaderAndInfo("Vertragsart: ", _akt.AktIntAutoVertragArt, col2);
            _lin += _gap;

            PrintHeaderAndInfo("Object: ", _akt.AktIntAutoName, _col1);
            PrintHeaderAndInfo("Kennzeichen: ", _akt.AktIntAutoKZ, col2);
            _lin += _gap * 2;

            WriteUnderlinedSubheader("Vollbrachte Dienstleistung durch ECP:", _col1);
            _lin += _gap + 20;
            if(!isDealerProtocol)
                PrintHeaderAndInfo("Dienstleistung: ", _action.AktIntActionTypeCaption.Replace("Auto - ", ""), _col1, BaseColor.RED);
            var hdr = "Sicherstellung am: ";
            var info = formattedSicherstellungsDatum;
            PrintHeaderAndInfo(hdr, info, col2, BaseColor.RED);
            var newCol = col2 + GetHeaderAndInfoWidth(hdr, info);
            hdr = " um ";
            info = formattedSicherstellungsZeit;
            PrintInfoAndInfo(hdr, info, newCol, BaseColor.RED);
            newCol += GetHeaderAndInfoWidth(hdr, info);
            WriteInfo(newCol, " Uhr");
            _lin += _gap;
            int distance = 0;
            if (_protokol.Uberstellungsdistanz > 0)
                distance = (int) _protokol.Uberstellungsdistanz;
            PrintHeaderAndInfo("Ort der Übernahme: ", _protokol.UbernahmeOrt, _col1, BaseColor.RED);
            PrintHeaderAndInfo("Überstellungsdistanz in KM: ", distance > 0 ? distance.ToString() : "", col2, BaseColor.RED);
            _lin += _gap;
            PrintHeaderAndInfo("Sichersteller: ", _akt.UserVorname + " " + _akt.UserNachname, _col1, BaseColor.RED);
            PrintHeaderAndInfo("Beifahrer: ", _protokol.Beifahrer, col2, BaseColor.RED);
            _lin += _gap;
            PrintHeaderAndInfo("KM-Stand bei Übernahme: ", ubernameTachometer, _col1, BaseColor.RED);
            PrintHeaderAndInfo("KM-Stand nach Überstellung: ", _protokol.Tachometer.ToString(), col2, BaseColor.RED);
            _lin += _gap;
            var picCount = GetPicCount();
            PrintHeaderAndInfo("Bilder vom KFZ: ", picCount > 0 ? ja : nein, _col1, BaseColor.RED);
            if(picCount > 0)
                PrintHeaderAndInfo("Anzahl der Bilder: ", picCount.ToString(), col2, BaseColor.RED);
            _lin += _gap;

            hdr = "Sichtbare Schaden: ";
            info = _protokol.SchadenComment.Trim();
            PrintHeaderAndInfo(hdr, info == "" ? "nein" : "", _col1, BaseColor.RED);
            if(info != "")
            {

                WriteMultilineInfo(info, _col1 + GetHeaderWidth(hdr), BaseColor.RED);
            }
            if (!string.IsNullOrEmpty(formattedAusloseDatum))
            {
                _lin += _gap;

                hdr = "Kunde möchte das KFZ bis zum ";
                info = formattedAusloseDatum;
                PrintHeaderAndInfo(hdr, info, _col1, BaseColor.RED);
                newCol = _col1 + GetHeaderAndInfoWidth(hdr, info);
                hdr = " wider auslösen: ";
                info = formattedAusloseZeit;
                PrintHeaderAndInfo(hdr, info, newCol, BaseColor.RED);
                newCol += GetHeaderAndInfoWidth(hdr, info);
                PrintHeaderAndInfo("", mochteAuslosen, newCol, BaseColor.RED);
            }
            _lin += _gap*2;
            PrintHeaderAndInfo("Abschleppdienst: ", _protokol.Abschleppdienst ? ja : nein, _col1, BaseColor.RED);
            if(_protokol.Abschleppdienst)
                PrintHeaderAndInfo("Warum wurde dieser benötigt: ", _protokol.AbschleppdienstGrund, col2, BaseColor.RED);
            _lin += _gap;

            PrintHeaderAndInfo("Polizei wurde informiert: ", _protokol.PolizieInformiert ? ja : nein, _col1, BaseColor.RED);
            if(_protokol.PolizieInformiert)
                PrintHeaderAndInfo("Polizei Dienststelle: ", _protokol.PolizeiDienststelle, col2, BaseColor.RED);
            _lin += _gap * 2;

            WriteUnderlinedSubheader("Mit dem KFZ wurden sichergestellt/ zurückgegeben:", _col1);
            _lin += _gap + 20;
            PrintHeaderAndInfo("Zulassungsschein: ", _protokol.UbernommentMitZulassung ? ja : nein, _col1, BaseColor.RED);
            PrintHeaderAndInfo("Serviceheft: ", _protokol.Serviceheft.ToLower().Trim() == ja ? ja : nein, col2, BaseColor.RED);
            _lin += _gap;
            PrintHeaderAndInfo("Masterkey / Codekarte: ", _protokol.MasterKey ? ja : nein, _col1, BaseColor.RED);
            PrintHeaderAndInfo("Anzahl der Schlüssel: ", _protokol.AnzahlSchlussel.ToString(), col2, BaseColor.RED);
            _lin += _gap;
            PrintHeaderAndInfo("Kennzeichen am KFZ: ", _protokol.KZ.ToLower() != "abgemeldet" ? ja : nein, _col1, BaseColor.RED);
            PrintHeaderAndInfo("Kennzeichen durch Behörde eingezogen: ", kfzDurchBehordeAbgemeldet, col2, BaseColor.RED);
            _lin += _gap;
            PrintHeaderAndInfo("Typenschein: ",  typenschein, _col1, BaseColor.RED);
            PrintHeaderAndInfo("Kennzeichen von ECP abgenommen und dem AG übergeben: ", _protokol.KzVonEcpAnAg ? ja : nein, col2-300, BaseColor.RED);
            _lin += _gap * 2;
            if (!isDealerProtocol)
            {
                WriteUnderlinedSubheader("Kosten im Rahmen der Sicherstellung:", _col1);
                _lin += _gap + 20;
                PrintHeaderAndInfo("Kosten Abschleppdienst: ",
                    HTBUtils.FormatCurrency(_protokol.ZusatzkostenAbschleppdienst), _col1, BaseColor.RED);
                PrintHeaderAndInfo("Pannendienst: ", HTBUtils.FormatCurrency(_protokol.ZusatzkostenPannendienst), col2,
                    BaseColor.RED);
                _lin += _gap;
                PrintHeaderAndInfo("Vignette: ", HTBUtils.FormatCurrency(_protokol.ZusatzkostenVignette), _col1,
                    BaseColor.RED);
                PrintHeaderAndInfo("Mautgebühren: ", HTBUtils.FormatCurrency(_protokol.ZusatzkostenMaut), col2,
                    BaseColor.RED);
                _lin += _gap;
                PrintHeaderAndInfo("Treibstoff: ", HTBUtils.FormatCurrency(_protokol.ZusatzkostenTreibstoff), _col1,
                    BaseColor.RED);
                PrintHeaderAndInfo("Reparaturrechnung von ECP ausgelegt: ",
                    HTBUtils.FormatCurrency(_protokol.ZusatzkostenEcp), col2, BaseColor.RED);
                _lin += _gap;
                PrintHeaderAndInfo("Standgebühren: ", HTBUtils.FormatCurrency(_protokol.ZusatzkostenStandgebuhren),
                    _col1, BaseColor.RED);
                PrintHeaderAndInfo("Rechnungsnr. E.C.P: ", _protokol.RechnungNr, col2, BaseColor.RED);
                _lin += _gap * 2;
            }
            WriteUnderlinedSubheader("Übernahmebestätigung durch:", _col1);
            _lin += _gap + 20;

            WriteInfo(_col1, _protokol.HandlerName);
            _lin += _gap;
            WriteInfo(_col1, _protokol.HandlerStrasse);
            _lin += _gap;
            WriteInfo(_col1, "A -" +_protokol.HandlerPLZ + " " + _protokol.UbernahmeOrt);
            _lin += _gap * 2;
            
            if (_emailAddresses != null)
            {
                WriteUnderlinedSubheader( "EMail gesendet an:", _col1);
                _lin += _gap;
                _emailAddresses.ForEach(address =>
                {
                    WriteMercedesInfo(address);
                });
            }

            var signatureLine = _lin - _gap * 4;
            var signatureCol = 1200;
            PrintHeaderAndInfo("Übernommen von: ", _protokol.UbernommenVon, signatureCol, BaseColor.RED);
            _lin += _gap;
            newCol = signatureCol + 50;
            PrintHeaderAndInfo("", formattedSicherstellungsDatum, newCol, BaseColor.RED);
            newCol += GetHeaderAndInfoWidth("", formattedSicherstellungsDatum);
            hdr = " um ";
            info = formattedSicherstellungsZeit;
            PrintInfoAndInfo(" um ", formattedSicherstellungsZeit, newCol, BaseColor.RED);
            newCol += GetHeaderAndInfoWidth(hdr, info);
            WriteInfo(newCol, " Uhr ");

            
            Writer.drawBitmapFromPath(signatureLine+_gap*4, signatureCol, _protokol.SignaturePath, 40);
            if (!isDealerProtocol)
            {
                _lin += _gap * 2;
                var multilineText =
                    $"Sollten wir eigene Forderungen gegen den Kunden/Leasingnehmer haben, bestätigen wir hiermit gegenüber {_akt.AuftraggeberName1} weder ein allfälliges Retentionsrecht geltend zu machen, noch Standgebühren für die Lagerung zu verrechnen.";
                WriteMultilineInfo(multilineText, _col1, BaseColor.BLACK, 100);
            }
        }
        
        private void PrintRepossessionForMerzedes()
        {

            WriteMercedesPageHeader();
            
            var dte = _protokol.SicherstellungDatum;
            var text = "KFZ Rückgabe am ";
            if (!HTBUtils.IsDateValid(dte))
            {
                dte = _action.AktIntActionTime;
            }
            if (dte.ToShortTimeString() == "00:00")
                text += dte.ToString(DateFormat);
            else
                text += dte.ToString(DateFormat) + " um " + dte.ToString(TimeFormat) + " Uhr";

            var orgLine = _lin;

            WriteMercedesInfo(text);
            WriteMercedesInfo("Fabrikat: Mercedes-Benz" );
            WriteMercedesInfo("Kennzeichen: "+_akt.AktIntAutoKZ);
            WriteMercedesInfo("Km-Stand bei Übernahme "+_protokol.Tachometer);
            _lin = orgLine - _gap;

            WriteMercedesInfo3("Vertragsnummer: "+_akt.AktIntAZ);
            WriteMercedesInfo3("Kundenname: "+ _akt.GegnerLastName1 + " " + _akt.GegnerLastName2);
            WriteMercedesInfo3("Fahrgestellnummer: "+_akt.AktIntAutoIdNr);
            WriteMercedesInfo3("Erstzulassung: " + _akt.AktIntAutoFirstRegistrationDate.ToString(DateFormat));
            WriteMercedesInfo3("Fahrzeug übernommen von " + _protokol.UbernommenVon);
            _lin += _gap * 2;

            Writer.drawLine(_lin, _col1, _lin, _col3);
            
            WriteMercedesHeader("Mit dem Fahrzeug wurde sichergestellt/zurückgegeben");

            _lin += _gap * 2;

            orgLine = _lin;
            WriteMercedesInfo("Zulassungschein", 0);
            WriteMercedesInfo2(_protokol.UbernommentMitZulassung ? ja : nein, 0);
            _lin += _gap;
            WriteMercedesInfo("KFZ - Schlüssel", 0);
            WriteMercedesInfo2(_protokol.AnzahlSchlussel > 0 ? ja : nein, 0);
            _lin += _gap;
            WriteMercedesInfo("Masterkey/Codekarte", 0);
            WriteMercedesInfo2(_protokol.MasterKey ? ja : nein, 0);

            _lin = orgLine;
            WriteMercedesInfo3("Kundendienstheft", 0);
            WriteMercedesInfo4(_protokol.Serviceheft.ToLower().Trim() == ja ? ja : nein, 0);
            _lin += _gap;
            WriteMercedesInfo3("Anzahl der Schlüssel", 0);
            WriteMercedesInfo4(_protokol.AnzahlSchlussel.ToString(), 0);
            _lin += _gap;
            WriteMercedesInfo3("Kennzeichen "+_protokol.AnzahlKZ+" Stück", 0);
            WriteMercedesInfo4(_protokol.KZ.ToLower() != "abgemeldet" ? ja : nein, 0);
            _lin += _gap * 2;

            WriteMercedesInfo("Solten nicht alle Schlüssel, Code- Karte, Kennzeichentafeln, und Zulassungspapiere ausgehändigt werden, entstehen dem Kunden");
            WriteMercedesInfo("zusätzliche Kosten.");
            _lin += _gap * 2;

            Writer.drawLine(_lin, _col1, _lin, _col3);
            
            WriteMercedesHeader("Anmerkungen:");

            _lin += (int)(_gap * 1.5);
            WriteMercedesMultilineInfo(_protokol.Memo.Trim());

            _lin += _gap * 20;

            Writer.drawLine(_lin, _col1, _lin, _col3);

            WriteMercedesHeader("Händlerbestätigung:");
            _lin += _gap / 2;
            WriteMercedesInfo("Sollten wir eigene  Forderungen gegen den  Kunden/Leasingnehmer  haben, bestätigen wir hiermit gegenüber Mercedes-Benz");
            WriteMercedesInfo("Financial Services Austria GmbH weder ein allfälliges Retentionsrecht geltend zu machen, noch Standgebühren fur die Lagerung");
            WriteMercedesInfo("zu verrechnen. Für alle im Fahrzeug allenfalls noch verbliebenen Gegenstände wird jegliche Haftung unsererseits als auch der");
            WriteMercedesInfo("Mercedes-Benz Financial Services Austria GmbH ausgeschlossen.");

            _lin += _gap * 4;
            _col2 += 200;
            var signatureLine = _lin + _gap * 4;

            WriteMercedesInfo("Datum: "+ _protokol.SicherstellungDatum.ToString(DateFormat), 0);
            WriteMercedesInfo2("Unterschrift: ", 0);
            WriteMercedesInfo2(_protokol.UbernommenVon);
            _lin += _gap;
            WriteMercedesInfo2(_protokol.HandlerName);
            WriteMercedesInfo2(_protokol.HandlerStrasse);
            WriteMercedesInfo2(_protokol.HandlerPLZ + "," + _protokol.HandlerOrt);
            _lin -= _gap * 4;
            if (_emailAddresses != null)
            {
                WriteMercedesInfo("EMail gesendet an:");
                _emailAddresses.ForEach(address =>
                {
                    WriteMercedesInfo(address);
                });
            }


            Writer.drawBitmapFromPath(signatureLine, _col2+100, _protokol.SignaturePath, 40);
        }

        private void PrintVisitDates()
        {
            if (_visitRecordList != null && _visitRecordList.Count > 0)
            {
                int visitNumber = 1;
                foreach (var visitRecord in _visitRecordList)
                {
                    DateTime visit = visitRecord.VisitTime;
                    WriteHeader(visitNumber + ". Besuch");
                    WriteInfo(visit.ToShortDateString() + " " + (visit.ToShortTimeString().Equals("00:00") ? "" : visit.ToShortTimeString()) +" "+visitRecord.VisitPerson+" ["+visitRecord.VisitAction+"]");
                    if(!string.IsNullOrEmpty(visitRecord.VisitMemo))
                    {
                        _lin += _gap;
                        WriteMultilineInfo(visitRecord.VisitMemo);
                    }
                    visitNumber++;
                }
            }
        }

        private void PrintMoneyCollected()
        {

            if (_invoices == null)
                return;

            #region declare and set variables
            var col2 = _col2 + 350;
            var dte = _action.AktIntActionTime;
            
            string formattedDate = dte.ToString(DateFormat);
            string formattedTime = "";
            if (dte.ToShortTimeString() == "00:00")
                WriteInfo(dte.ToShortDateString());
            else
                formattedTime = dte.ToString(TimeFormat);



            #endregion
            // start reporting


            SetHeadingFont();
            _lin += _gap * 2;


            WriteUnderlinedSubheader("Auftragsdaten:", _col1);
            _lin += _gap + 20;

            PrintHeaderAndInfo("Kunde: ", _akt.GegnerLastName1 + " " + _akt.Gegner2LastName2, _col1);
            PrintHeaderAndInfo("Vertragsart: ", _akt.AktIntAutoVertragArt, col2);
            _lin += _gap;
            PrintHeaderAndInfo("Object: ", _akt.AktIntAutoName, _col1);
            PrintHeaderAndInfo("Kennzeichen: ", _akt.AktIntAutoKZ, col2);
            _lin += _gap;
            PrintHeaderAndInfo("Offene Forderung AG: ", HTBUtils.FormatCurrency(GetOriginalOpenedAgAmount()), _col1);
            PrintHeaderAndInfo("Offene Versicherung: ", HTBUtils.FormatCurrency(GetOriginalOpenedInsuranceAmount()), col2);

            _lin += _gap * 2;
            
            PrintHeaderAndInfo("Akt wurde abgeschlossen durch: ", _action.AktIntActionTypeCaption.Replace("Auto - ", ""), _col1, BaseColor.RED);
            var hdr = "Abschluss am: ";
            var info = formattedDate;
            PrintHeaderAndInfo(hdr, info, col2, BaseColor.RED);
            var newCol = col2 + GetHeaderAndInfoWidth(hdr, info);
            hdr = " um ";
            info = formattedTime;
            PrintInfoAndInfo(hdr, info, newCol, BaseColor.RED);
            newCol += GetHeaderAndInfoWidth(hdr, info);
            WriteInfo(newCol, " Uhr");

            _lin += _gap;
            PrintHeaderAndInfo("Anzahl der Betreibungsversuche: ", _visitRecordList?.Count.ToString(), _col1, BaseColor.RED);
            PrintHeaderAndInfo("ZMR durch E.C.P.: ", HasZmrAction() ? ja : nein, col2, BaseColor.RED);

            _lin += _gap;
            PrintHeaderAndInfo("Sachbearbeiter E.C.P.: ", _akt.UserVorname + " " + _akt.UserNachname, _col1, BaseColor.RED);
            PrintHeaderAndInfo("Anschrift Erhebung durch E.C.P.: ", HasAnschriftErhebungAction() ? ja : nein, col2, BaseColor.RED);

            _lin += _gap * 2;

            WriteUnderlinedSubheader("Zahlungsaufstellung durch E.C.P.:", _col1);
            _lin += _gap + 40;
            
            PrintPaymantTable();
            
            _lin += _gap;

            if (CheckOverflow(_lin + (_gap * 5)))
            {
                Writer.newPage();
                PrintPageHeader();
                _lin += _gap;
            }
            WriteUnderlinedSubheader("Erbrachte Dienstleistungen der E.C.P. European Car Protect KG:", _col1);
            _lin += _gap + 40;
            var multiLineWidth = 0;
            var orgLin = 0;
            var visitNbr = 1;
            _visitRecordList.ForEach(visit =>
            {
                multiLineWidth = ReportUtils.GetMultipleLinesHeight(_gap, visit.VisitMemo, 110);

                if (CheckOverflow(_lin + (_gap * 3) + multiLineWidth))
                {
                    Writer.newPage();
                    PrintPageHeader();
                    _lin += _gap;
                }

                hdr = visitNbr+". Betreibungsversuch am: ";
                info = visit.VisitTime.ToString(DateFormat);
                PrintHeaderAndInfo(hdr, info, _col1, BaseColor.RED);
                newCol = _col1 + GetHeaderAndInfoWidth(hdr, info);
                hdr = " um ";
                info = visit.VisitTime.ToString(TimeFormat);
                PrintHeaderAndInfo(hdr, info, newCol, BaseColor.RED);
                newCol += GetHeaderAndInfoWidth(hdr, info);
                hdr = " Uhr";
                info = "";
                PrintHeaderAndInfo(hdr, info, newCol, BaseColor.RED);
                newCol += GetHeaderAndInfoWidth(hdr, info);
                hdr = " durch ";
                info = visit.VisitPerson;
                PrintHeaderAndInfo(hdr, info, newCol, BaseColor.RED);
                _lin += _gap;
                PrintHeaderAndInfo("Aktion: ", visit.VisitAction, _col1, BaseColor.RED);
                _lin += _gap;
                if (visit.VisitMemo.Trim() != "")
                {
                    PrintHeaderAndInfo("Memo: ", "", _col1, BaseColor.RED);
                    _lin += _gap;
                    orgLin = _lin;

                    WriteMultilineInfo(visit.VisitMemo, _col1 + 50, BaseColor.BLACK, 110);

                    Writer.drawRect(orgLin, _col1, _lin, 1850);
                }
                _lin += _gap;
                visitNbr++;
            });

            multiLineWidth = ReportUtils.GetMultipleLinesHeight(_gap, _akt.AKTIntMemo, 110);
            if (CheckOverflow(_lin + (_gap * 3) + multiLineWidth))
            {
                Writer.newPage();
                PrintPageHeader();
                _lin += _gap;
            }
            if (_akt.AKTIntMemo.Trim() != "")
            {
                _lin += _gap * 2;
                WriteUnderlinedSubheader("Erweiterter Bericht:", _col1);
                _lin += _gap + 40;
                orgLin = _lin;
                WriteMultilineInfo(_akt.AKTIntMemo, _col1 + 50, BaseColor.BLACK, 110);
                Writer.drawRect(orgLin, _col1, _lin, 1850);
            }
        }

        private void PrintMoneyCollectedLine(string header, double amount, string headerDate, DateTime date, bool isKosten = false)
        {
            if(amount > 0)
            {
                WriteHeader(header);
                if(isKosten)
                {
                    WriteInfo("JA");
                }
                else
                {
                    WriteInfo(HTBUtils.FormatCurrency(amount));
                    if (date != HTBUtils.DefaultDate)
                    {
                        WriteHeader(headerDate);
                        WriteInfo(date.ToShortDateString());
                    }     
                }
                _lin += _gap/3;
            }
        }

        private void PrintOfficeCharges()
        {
            _lin += _gap*2;

            Writer.setFont("Arial", 14, true, false, false);
            Writer.print(_lin, MiddleCol, "Für Office zur Verrechnung!", 'C');
            _lin += _gap * 2;
            WriteHeader("Aussendienst:");
            WriteInfo(_akt.UserVorname + " " + _akt.UserNachname);
            _lin += _gap;
            WriteHeader("Dauer:");
            WriteInfo(HTBUtils.BusinessDaysUntil(_akt.AktIntDatum, _action.AktIntActionTime, new DateTime[] { }) + " Arbeitstage");
            _lin += _gap;

            if (!_action.AktIntActionIsReceivable)
            {
                _lin += _gap;
                Writer.print(_lin, MiddleCol, "Akt abgeschlossen ohne Verrechnung!", 'C');
            }
            else
            {
                double total = 0;

                WriteHeader("Sicherstellungsgebühr:");
                WriteInfo(HTBUtils.FormatCurrency(_akt.AKTIntKosten));
                total += _akt.AKTIntKosten;
                WriteHeader("Zusatzkosten Treibstoff:");
                WriteInfo(HTBUtils.FormatCurrency(_protokol.ZusatzkostenTreibstoff));
                total += _protokol.ZusatzkostenTreibstoff;
                WriteHeader("Zusatzkosten Vignette:");
                WriteInfo(HTBUtils.FormatCurrency(_protokol.ZusatzkostenVignette));
                total += _protokol.ZusatzkostenVignette;
                WriteHeader("Sonstige Zusatzkosten:");
                WriteInfo(HTBUtils.FormatCurrency(_protokol.ZusatzkostenSostige));
                total += _protokol.ZusatzkostenSostige;
                _lin += _gap;

                WriteHeader("Summe:");
                WriteInfo(HTBUtils.FormatCurrency(total));
                _lin += _gap;
            }
        }

        private void PrintBericht()
        {
            WriteHeader("Erweiterter Bericht:");
            WriteMultilineInfo(_protokol.Memo.Trim());
        }

        private void PrintDocuments(IEnumerable<Record> documents)
        {

            if(documents == null || !documents.Any())
                return;
            
            bool addedNewPage = false;
            Writer.StartPdfTable(2);
            bool isLeftSide = true;

            if (documents != null)
            {
                foreach (Record rec in documents)
                {
                    string description = "";
                    string fileName = "";
                    if (rec is qryDoksInkAkten)
                    {
                        var doc = (qryDoksInkAkten) rec;
                        description = string.IsNullOrEmpty(doc.DokText) ? doc.DokCaption : doc.DokText;
                        fileName = doc.DokAttachment;

                    }
                    else if (rec is qryDoksIntAkten)
                    {
                        var doc = (qryDoksIntAkten) rec;
                        description = string.IsNullOrEmpty(doc.DokText) ? doc.DokCaption : doc.DokText;
                        fileName = doc.DokAttachment;
                    }
                    if (fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".jpeg") || fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".gif"))
                    {
                        if (!addedNewPage)
                        {
                            Writer.newPage();
                            //PrintDocumentsPageHeader();
                            addedNewPage = true;
                        }
                        //description += "\n ";
                        //SetLineFont();
                        //_lin += _gap * 2 + Writer.AddImageToDocument(_lin, _col1, fileName, description);
                       //break;
                       Writer.AddImageToPdfTable(fileName, description);
                    }
                }
                Writer.FinishPdfTable();
            }
        }

        private int GetPicCount()
        {
            int cnt = 0;
            if (_documents != null)
            {
                foreach (Record rec in _documents)
                {
                    string description = "";
                    string fileName = "";
                    if (rec is qryDoksInkAkten)
                    {
                        var doc = (qryDoksInkAkten)rec;
                        fileName = doc.DokAttachment;

                    }
                    else if (rec is qryDoksIntAkten)
                    {
                        var doc = (qryDoksIntAkten)rec;
                        fileName = doc.DokAttachment;
                    }
                    if (fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".jpeg") || fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".gif"))
                    {
                        cnt++;
                    }
                }
            }
            return cnt;
        }

        private double GetOriginalOpenedAgAmount()
        {
            if (_invoices == null) return 0;

            var invoices =
                _invoices.Where(
                    i =>
                        i.AktIntPosTypeCode == tblAktenIntPosType.INVOICE_TYPE_ORIGINAL ||
                        i.AktIntPosTypeCode == tblAktenIntPosType.INVOICE_TYPE_CLIENT_COST);

            return invoices.Sum(i => i.AktIntPosBetrag);
        }

        private double GetOriginalOpenedInsuranceAmount()
        {
            if (_invoices == null) return 0;

            var invoices = _invoices.Where(i => i.AktIntPosTypeCode == tblAktenIntPosType.INVOICE_TYPE_INSURANCE);

            return invoices.Sum(i => i.AktIntPosBetrag);
        }

        private void PrintPaymantTable()
        {
            int col1 = _col1;
            int col2 = _col2 + 200;
            int col3 = col2 + 500;
            int col4 = col3 + 300;
            int rowH = 50;

            Writer.drawRect(_lin, col1, _lin + rowH, col4);
            Writer.drawLine(_lin, col2, _lin + rowH, col2);
            Writer.drawLine(_lin, col3, _lin + rowH, col3);

            SetHeadingFont();
            Writer.print(_lin, col1 + (col2 - col1)/2, "Art der Zahlung", 'C');
            Writer.print(_lin, col2 + (col3 - col2)/2, "Betrag", 'C');
            Writer.print(_lin, col3 + (col4 - col3)/2, "Üw.  Datum", 'C');
            
            _lin += rowH;
            SetLineFont();
            _invoices?.ForEach(invoice =>
            {
                if (invoice.IsPayment())
                {
                    Writer.drawRect(_lin, col1, _lin + rowH, col4);
                    Writer.drawLine(_lin, col2, _lin + rowH, col2);
                    Writer.drawLine(_lin, col3, _lin + rowH, col3);

                    Writer.print(_lin, col1 + 50, invoice.AktIntPosCaption.Replace("Zahlung", ""));
                    Writer.print(_lin, col3 - 50, HTBUtils.FormatCurrency(invoice.AktIntPosBetrag * -1), 'R');
                    if(HTBUtils.IsDateValid(invoice.AktIntPosTransferredDate))
                        Writer.print(_lin, col3 + (col4 - col3) / 2, invoice.AktIntPosTransferredDate.ToString(DateFormat), 'C');
                    _lin += rowH;
                }
            });

            _lin += rowH;
        }

        private void Open(Stream os)
        {
            Writer = new ECPPdfWriter();
            Writer.setFormName("A4");
            Writer.setImagePath(HTBUtils.GetConfigValue("DocumentsFolder"));
            Writer.open(os);
        }

        private void Close()
        {
            Writer.Close();
        }

        public bool CheckOverflow(int plin)
        {
            return plin >= MaxLines;
        }
        
        private void PrintDocumentsPageHeader()
        {
            _lin = StartLine;
            Writer.drawBitmap(230, _col1, iTextSharp.text.Image.GetInstance(_logoPath), 40);
            for (int i = 0; i < 3; i++)
            {
                Writer.setFont("Calibri", 20, true, false, false);
                Writer.print(50, MiddleCol + i, "EUROPEAN CAR PROTECT", 'C', BaseColor.BLUE); // give it a bolder look
            }
            _lin += 100;
            Writer.setFont("Calibri", 11, true, false, false);
            Writer.print(_lin, MiddleCol, "Inh. Thomas Jaky & Helmut Ammer", 'C');
            _lin += _gap * 2;
            WriteUnderlinedSubheader("Bilder:", _col1);
            _lin += _gap * 4;
        }

        public int PrintPageHeader()
        {
            if (_akt.AuftraggeberUseMerzedesProtocol && _action.AktIntActionIsAutoRepossessed) // no page header for mercedes protocol
                return _lin;
            
            _lin = StartLine;
            Writer.drawBitmap(230, _col1, iTextSharp.text.Image.GetInstance(_logoPath), 40);
            for (int i = 0; i < 3; i++)
            {
                Writer.setFont("Calibri", 20, true, false, false);
                Writer.print(50, MiddleCol + i, "EUROPEAN CAR PROTECT", 'C', BaseColor.BLUE); // give it a bolder look
            }
            _lin += 100;
            Writer.setFont("Calibri", 11, true, false, false);
            Writer.print(_lin, MiddleCol, "Inh. Thomas Jaky & Helmut Ammer", 'C');
            _lin += _gap*2;
            Writer.setFont("Calibri", 20, true, false, false);
            Writer.print(_lin, MiddleCol,
                _action.AktIntActionIsAutoRepossessed
                    ? "Protokoll Fahrzeugsicherstellung"
                    : "Protokoll zur Intervention", 'C');
            _lin += _gap*2+20;
            Writer.setFont("Calibri", 14, true, false, false);
            Writer.print(_lin, MiddleCol-320, "Auftraggeber: ");
            Writer.setFont("Calibri", 14, false, false, false);
            Writer.print(_lin, MiddleCol-20, _akt.AuftraggeberName1);
            _lin += _gap+15;
            Writer.setFont("Calibri", 14, true, false, false);
            Writer.print(_lin, MiddleCol-250, "Vertragsnummer: ");
            Writer.setFont("Calibri", 14, false, false, false);
            Writer.print(_lin, MiddleCol+125, _akt.AktIntAZ, BaseColor.RED);
            _lin += _gap;
            return _lin;
        }

        private void PrintHeaderAndInfo(string hdr, string info)
        {
            PrintHeaderAndInfo(hdr, info, _col1);
        }

        private void PrintHeaderAndInfo(string hdr, string info, int col)
        {
            PrintHeaderAndInfo(hdr, info, col, BaseColor.BLACK);
        }
        private void PrintHeaderAndInfo(string hdr, string info, int col, BaseColor lineColor)
        {
            SetHeadingFont();
            Writer.print(_lin, col, hdr);
            var width = Writer.GetTextWidth(hdr);
            SetLineFont();
            Writer.print(_lin, (int)(col + width), info, lineColor);
        }
        private int GetHeaderAndInfoWidth(string hdr, string info)
        {
            SetHeadingFont();
            
            var width = Writer.GetTextWidth(hdr);
            SetLineFont();
            width += Writer.GetTextWidth(info);
            return (int)width;
        }
        private int GetHeaderWidth(string hdr)
        {
            SetHeadingFont();
            var width = Writer.GetTextWidth(hdr);
            return (int)width;
        }
        private void PrintInfoAndInfo(string hdr, string info, int col, BaseColor lineColor)
        {
            SetLineFont();
            Writer.print(_lin, col, hdr);
            var width = Writer.GetTextWidth(hdr);
            SetLineFont();
            Writer.print(_lin, (int)(col + width), info, lineColor);
        }
        private void WriteUnderlinedSubheader(string text, int col)
        {
            Writer.setFont("Calibri", 14, true, false, true);
            var width = Writer.GetTextWidth(text);
            Writer.print(_lin, col, text);
            
            Writer.drawLine(_lin+53, col, _lin+53, col + (int)width);
        }
        public ECPPdfWriter GetWriter()
        {
            return Writer;
        }
        private void WriteHeader(int col, string str, int gap)
        {
            if (CheckOverflow(_lin))
            {
                Writer.newPage();
                PrintPageHeader();
            }

            SetHeadingFont();
            Writer.print(_lin += gap, col, str);
        }

        private void WriteHeader(string str, int gap)
        {
            WriteHeader(_col1, str, gap);
        }


        private void WriteHeader(string str)
        {
            WriteHeader(str, _gap);
        }
        private void WriteInfo(int col, string str, int gap = 0)
        {
            SetLineFont();
            Writer.print(_lin += gap, col, str);
        }

        private void WriteInfo(string str, int gap = 0)
        {
            WriteInfo(_col1, str, gap);
        }

        private void WriteMercedesPageHeader()
        {
            _lin += _gap * 5;
            Writer.setFont("Calibri", 13, false, false, false);
            Writer.print(_lin, 1050, "Mercedes-Benz Financial Services Austria GmbH", 'C');
            _lin += _gap * 3;

            Writer.setFont("Calibri", 13, true, false, false);

            Writer.print(_lin, 1050, "Protokoll Fahrzeugsicherstellung", 'C');
            _lin += _gap * 3;

            var dte = _akt.AktIntAutoSecuredDate;
            if (!HTBUtils.IsDateValid(dte))
                dte = _protokol.SicherstellungDatum;

            var text = "Sicherstellung am ";
            if (!HTBUtils.IsDateValid(dte))
            {
                dte = _action.AktIntActionTime;
            }
            if (dte.ToShortTimeString() == "00:00")
                text += dte.ToString(DateFormat);
            else
                text += dte.ToString(DateFormat) + " um " + dte.ToString(TimeFormat) + " Uhr";

            Writer.print(_lin, _col1, text);
            _lin += _gap * 2;

        }
        private void WriteMercedesHeader(string str, int gap)
        {
            if (CheckOverflow(_lin))
            {
                Writer.newPage();
                PrintPageHeader();
            }

            SetHeadingFont();
            Writer.print(_lin += gap, _col1, str, 'L');
        }
        private void WriteMercedesHeader(string str)
        {
            WriteMercedesHeader(str, _gap);
        }
        private void WriteMercedesInfo(string str, int gap = 39)
        {
            SetMercedesLineFont();
            Writer.print(_lin += gap, _col1, str);
        }

        private void WriteMercedesInfo2(string str, int gap = 39)
        {
            SetMercedesLineFont();
            Writer.print(_lin += gap, _col2-200, str);
        }

        private void WriteMercedesInfo3(string str, int gap = 39)
        {
            SetMercedesLineFont();
            Writer.print(_lin += gap, _col2+300, str);
        }

        private void WriteMercedesInfo4(string str, int gap = 39)
        {
            SetMercedesLineFont();
            Writer.print(_lin += gap, _col2+800, str);
        }

        private void WriteMultilineInfo(string str, int gap = 0)
        {
            SetLineFont();
            //const int maxLines = 20;
            const int maxCharsPerLine = 65;
            //if (str.Length > maxLines * maxCharsPerLine)
            //    str = str.Substring(0, (maxLines * maxCharsPerLine) - 3)+"...";
            _lin = ReportUtils.PrintTextInMultipleLines(this, _lin += gap, _col2, _gap, str, maxCharsPerLine);
        }
        private void WriteMultilineInfo(string str, int col, BaseColor color, int maxCharsPerLine = 80, int gap = 0)
        {
            SetLineFont();
            _lin = ReportUtils.PrintTextInMultipleLines(this, _lin += gap, col, _gap, str, maxCharsPerLine, color);
        }

        private void WriteMercedesMultilineInfo(string str, int gap = 0)
        {
            SetMercedesLineFont();
            const int maxCharsPerLine = 120;
            ReportUtils.PrintTextInMultipleLines(this, _lin += gap, _col1, _gap, str, maxCharsPerLine);
        }

        
        public void SetLineFont()
        {
            Writer.setFont("Calibri", 11);
        }
        public void SetMercedesLineFont()
        {
            Writer.setFont("Calibri", 9);
        }
        public void SetSmallHeaderFont()
        {
            Writer.setFont("Calibri", 9, false, true, false);
        }
        public void SetLineFont(bool bold, bool italics, bool underline)
        {
            Writer.setFont("Calibri", 11, bold, italics, underline);
        }

        private void SetHeadingFont()
        {
           Writer.setFont("Calibri", 11, true, false, true);
        }

        private void SetHeadingFont(bool italics, bool underline)
        {
            Writer.setFont("Calibri", 13, true, italics, underline);
        }

        private bool HasZmrAction()
        {
            return HasActionType("zmr");
        }

        private bool HasAnschriftErhebungAction()
        {
            return HasActionType("anschrift");
        }

        private bool HasActionType(string actionType)
        {
            var sql = $"SELECT * FROM qryAktenIntActionWithType WHERE AktIntActionAkt = {_akt.AktIntID} AND AktIntActionTypeCaption LIKE '%{actionType}%'";
            return HTBUtils.GetSqlSingleRecord(sql, typeof(qryAktenIntActionWithType)) != null;
        }


        #region Old Methods


        public int PrintPageHeaderOld()
        {
            if (_akt.AuftraggeberUseMerzedesProtocol && _action.AktIntActionIsAutoRepossessed) // no page header for mercedes protocol
                return _lin;

            Writer.PrintLeftEcpInfo();
            _lin = StartLine;
            Writer.drawBitmap(350, _col1, iTextSharp.text.Image.GetInstance(_logoPath), 40);
            for (int i = 0; i < 3; i++)
            {
                Writer.setFont("Arial", 22, true, false, false);
                Writer.print(50, MiddleCol + i, "EUROPEAN CAR PROTECT", 'C', BaseColor.BLUE); // give it a bolder look
            }
            _lin += 150;
            Writer.setFont("Calibri", 19, true, false, false);
            Writer.print(_lin, MiddleCol, "Protokoll Für KFZ Einzug", 'C');
            _lin += 200;
            return _lin;
        }



        private void PrintProtokolOld()
        {
            SetHeadingFont();

            WriteHeader("Rechnungs Nr. E.C.P.:");
            WriteInfo(_protokol.RechnungNr);
            WriteHeader("Vertragsnummer:");
            WriteInfo(_akt.AktIntAZ);
            _lin += _gap;
            WriteHeader("Verrechnungsart:");
            WriteInfo(_action.AktIntActionTypeCaption);
            WriteHeader("Auftraggeber:");
            WriteInfo(_akt.AuftraggeberName1 + " " + _akt.AuftraggeberName2);
            WriteHeader("Vertragsart:");
            WriteInfo(_akt.AktIntAutoVertragArt);
            WriteHeader("Kunde:");
            WriteInfo(_akt.GegnerLastName1 + " " + _akt.GegnerLastName2);
            WriteHeader("Objekt:");
            WriteInfo(_akt.AktIntAutoName);
            WriteHeader("KZ:");
            WriteInfo(_akt.AktIntAutoKZ);
            _lin += _gap;
            PrintVisitDates();
            _lin += _gap;
            if (_action.AktIntActionIsAutoRepossessed)
                PrintRepossessionOld();
            else if (_action.AktIntActionIsAutoMoneyCollected)
                PrintMoneyCollected();
            else //if (_action.AktIntActionIsAutoNegative)
                PrintBericht();

            Writer.print(2900, 1800, DateTime.Now.ToShortDateString(), 'R');
        }



        private void PrintRepossessionOld()
        {

            WriteHeader("Sichergestellt am:");
            DateTime dte = _protokol.SicherstellungDatum;
            if (!HTBUtils.IsDateValid(dte))
            {
                dte = _action.AktIntActionTime;
            }
            if (dte.ToShortTimeString() == "00:00")
                WriteInfo(dte.ToShortDateString());
            else
                WriteInfo(dte.ToShortDateString() + " " + dte.ToShortTimeString());

            WriteHeader("Ort der Übernahme:");
            WriteInfo(_protokol.UbernahmeOrt);
            WriteHeader("Übernommen mit Zulassung:");
            WriteInfo(_protokol.UbernommentMitZulassung ? "Ja" : "Nein");
            WriteHeader("KZ vorhanden:");
            WriteInfo(_protokol.KZ);
            WriteHeader("Serviceheft:");
            WriteInfo(_protokol.Serviceheft);
            WriteHeader("Anzahl Schlüssel:");
            WriteInfo(_protokol.AnzahlSchlussel.ToString());
            WriteHeader("KM - Stand laut Tacho:");
            WriteInfo(_protokol.Tachometer.ToString());
            _lin += _gap;
            WriteHeader("Abschleppdienst:");
            WriteInfo(_protokol.Abschleppdienst ? "Ja" : "Nein");
            WriteHeader("Abschleppdienst Name:");
            WriteInfo(_protokol.AbschleppdienstName);
            _lin += _gap;

            WriteHeader("Sichtbare Schäden:");
            WriteMultilineInfo(_protokol.SchadenComment);
            WriteHeader("Erweiterter Bericht:");
            WriteMultilineInfo(_protokol.Memo.Trim() + " " + _action.AktIntActionMemo);

            WriteHeader("Zusatzkosten Treibstoff:");
            WriteInfo(HTBUtils.FormatCurrency(_protokol.ZusatzkostenTreibstoff));
            WriteHeader("Zusatzkosten Vignette:");
            WriteInfo(HTBUtils.FormatCurrency(_protokol.ZusatzkostenVignette));
            WriteHeader("Sonstige Zusatzkosten:");
            WriteInfo(HTBUtils.FormatCurrency(_protokol.ZusatzkostenSostige));
            _lin += _gap;

            WriteHeader("Polizei informiert:");
            WriteInfo(_protokol.PolizieInformiert ? "Ja" : "Nein");

            WriteHeader("Sichersteller:");
            WriteInfo(_akt.UserVorname + " " + _akt.UserNachname);
            WriteHeader("Name vom Beifahrer:");
            WriteInfo(_protokol.Beifahrer);
            _lin += _gap;
            WriteHeader("Händler:");
            WriteInfo(_protokol.HandlerName);
            WriteInfo(_protokol.HandlerStrasse, _gap);
            WriteInfo(_protokol.HandlerLKZ + " - " + _protokol.HandlerPLZ + " " + _protokol.HandlerOrt, _gap);

            WriteHeader("Übernommen von:");
            WriteInfo(_protokol.UbernommenVon);
            Writer.drawBitmapFromPath(_lin += _gap * 6, _col1 + (_col2 - _col1) / 2, _protokol.SignaturePath, 40);
        }

        private void PrintMoneyCollectedOld()
        {

            if (_invoices == null)
                return;
            bool kostenShown = false;
            foreach (var invoice in _invoices)
            {
                double amount = invoice.AktIntPosBetrag * -1;
                DateTime transferDate = invoice.AktIntPosTransferredDate;

                if (invoice.AktIntPosTypeCode == tblAktenIntPosType.INVOICE_TYPE_PAYMENT_CASH_COLLECTION && !kostenShown)
                {
                    PrintMoneyCollectedLine("Kosten bar kassiert:", amount, "Kosten überwiesen am:", HTBUtils.DefaultDate, true); // do not show Kosten uberwiesen am
                    kostenShown = true;
                }
                else switch (invoice.AktIntPosTypeCode)
                    {
                        case tblAktenIntPosType.INVOICE_TYPE_PAYMENT_CASH_ORIGINAL:
                            PrintMoneyCollectedLine("Forderung bar kassiert:", amount, "Forderung überwiesen am:", transferDate);
                            break;
                        case tblAktenIntPosType.INVOICE_TYPE_PAYMENT_CASH_INSURANCE:
                            PrintMoneyCollectedLine("Versicherung bar kassiert:", amount, "Versicherung überwiesen am:", transferDate);
                            break;
                        case tblAktenIntPosType.INVOICE_TYPE_PAYMENT_DIRECT_AG_ORIGINAL:
                            PrintMoneyCollectedLine("Direktzahlung an AG:", amount, "Direktzahlung am:", transferDate);
                            break;
                        case tblAktenIntPosType.INVOICE_TYPE_PAYMENT_DIRECT_INSURANCE:
                            PrintMoneyCollectedLine("Direktzahlung an Versicherung:", amount, "Direktzahlung an Versicherung am:", transferDate);
                            break;
                    }
            }
            WriteHeader("Erweiterter Bericht:");
            WriteMultilineInfo(_protokol.Memo.Trim());
        }

        private void PrintMoneyCollected_Old()
        {

            PrintMoneyCollectedLine("Forderung bar kassiert:", _protokol.ForderungBarKassiert, "Forderung überwiesen am:", _protokol.ForderungUberwiesen);
            PrintMoneyCollectedLine("Versicherung bar kassiert:", _protokol.VersicherungBarKassiert, "Versicherung überwiesen am:", _protokol.VersicherungUberwiesen);
            //            PrintMoneyCollectedLine("Kosten bar kassiert", _protokol.KostenBarKassiert, "Kosten überwiesen am", _protokol.KostenUberwiesen);
            PrintMoneyCollectedLine("Direktzahlung an AG:", _protokol.Direktzahlung, "Direktzahlung am:", _protokol.DirektzahlungAm);
            PrintMoneyCollectedLine("Direktzahlung an Versicherung:", _protokol.DirektzahlungVersicherung, "Direktzahlung an Versicherung am:", _protokol.DirektzahlungVersicherungAm);

            PrintMoneyCollectedLine("Kosten bar kassiert:", _protokol.KostenBarKassiert, "Kosten überwiesen am:", HTBUtils.DefaultDate, true); // do not show Kosten uberwiesen am

            WriteHeader("Erweiterter Bericht:");
            WriteMultilineInfo(_protokol.Memo.Trim());
        }


        private void PrintVisitDates_Old()
        {
            if (_visits != null && _visits.Count > 0)
            {
                int visitNumber = 1;
                foreach (var visit in _visits)
                {
                    WriteHeader(visitNumber + ". Besuch");
                    WriteInfo(visit.BesuchAm.ToShortDateString());
                    visitNumber++;
                }
            }
        }

        /*
        private void AddPhrase(string leftText, string leftFont, string rightText, string rightFont)
        {
            var cleft = new Chunk(leftText);
            cleft.Font = new

            var p = new Phrase()
            {
                new Chunk(text),
                new Chunk(_akt.GegnerLastName1 + " " + _akt.Gegner2LastName2)
            };
        }
        */

        #endregion

    }
}
