using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using HTB.Database.Views;
using HTBPdf;
using HTBUtilities;
using iTextSharp.text;

namespace HTBReports
{
    public class MahnungPdfReportGenerator
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private int _col;
        private int _col1;
        private int _col2;

        public MahnungPdfReportGenerator(String filePath)
        {
            string testxml = File.ReadAllText(filePath);
            var ds = new DataSet();
            ds.ReadXml(new StringReader(testxml));
            var mahListRec = new MahnungsListRecord();
            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == "MAHNUNG")
                {

                    foreach (DataRow dr in tbl.Rows)
                    {
                        var mahnung = new Mahnung();
                        mahnung.LoadFromDataRow(dr);
                        mahListRec.MahnungsList.Add(mahnung);
                    }
                }
                else if (tbl.TableName.ToUpper().Trim() == "TERMINVERLUSTREC")
                {

                    foreach (DataRow dr in tbl.Rows)
                    {
                        var tvl = new TerminverlustRec();
                        tvl.LoadFromDataRow(dr);
                        mahListRec.MahnungsList.Add(tvl);
                    }
                }
            }
            GenerateMahnungOrTerminverlust(mahListRec);
        }
        
        public MahnungPdfReportGenerator(){}
       
        public void GenerateForderungsaufstellung(EcpMahnung mah, Stream os, string info, bool includeRatenversuchen = false, bool includeRatenplan = false, bool isForderungsauftellung = true)
        {
            var lin = 400;
            const int gap = 37;
            _col = 390;
            _col1 = _col + 33;
            _col2 = _col1 + 1100;

            var writer = new ECPPdfWriter();
            var logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");

            writer.setFormName("A4");
            writer.open(os);

            writer.PrintLeftEcpInfo();

            writer.drawBitmap(350, _col, Image.GetInstance(logoPath), 40);
            for (var i = 0; i < 3; i++)
            {
                writer.setFont("Arial", 22, true, false, false);
                writer.print(50, _col + 1600 + i, "EUROPEAN CAR PROTECT", 'R', BaseColor.BLUE); // give it a bolder look
                writer.setFont("Arial", 16, true, false, false);
                writer.print(120, _col + 1600 + i, "INKASSO-SERVICE", 'R', BaseColor.BLUE); // give it a bolder look
            }

            writer.setFont("Calibri", 8);
            writer.print((lin += gap), _col1, "E.C.P. European Car Protect KG");
            writer.print((lin += gap), _col1, "Schwarzparkstrasse 15   A 5020 Salzburg");
            writer.drawLine(lin + 30, _col1, lin + 30, _col1 + 465);
            lin += gap;
            writer.setFont("Calibri", 11);
            writer.print((lin += gap), _col1, mah.SchuldnerAnrede);
            if (mah.SchuldnerTyp == 0)
            {
                writer.print((lin += gap), _col1, mah.NameFirma);
                if (mah.AnsprechpartnerNachname.Trim() != "")
                {
                    writer.print((lin += gap), _col1, mah.AnsprechpartnerAnrede + " " + mah.AnsprechpartnerVorename + " " + mah.AnsprechpartnerNachname);
                }
            }
            else
            {
                writer.print((lin += gap), _col1, mah.SchuldnerVorname + " " + mah.SchuldnerNachname);
            }
            writer.print((lin += gap), _col1, mah.Strasse);
            writer.print((lin += gap), _col1, mah.LKZ + " - " + mah.PLZ + " " + mah.Ort);
            writer.print((lin += gap), _col + 1600, "Salzburg, am " + mah.Date.ToShortDateString(), 'R');
            lin += gap * 5;
            writer.setFont("Calibri", 11, true, false, false);
            if (isForderungsauftellung)
            {
                writer.print((lin += gap), _col1, "FORDERUNGSAUFSTELLUNG");
                lin += gap;
            }
            writer.print((lin += gap), _col1, "AKTENZAHL: " + mah.Aktenzahl + (mah.CustomerAktenzahl.Trim() != string.Empty ? " [" + mah.CustomerAktenzahl + "]" : ""));
            writer.print((lin += gap), _col1, "Klient:            " + mah.KlientName);
            lin += gap * 2;
            writer.setFont("Calibri", 11);
            if (mah.SchuldnerTyp == 0) // firma
            {
                if (mah.AnsprechpartnerNachname != null && mah.AnsprechpartnerNachname.Trim() != "")
                {
                    if (mah.AnsprechpartnerAnrede.ToLower() == "frau")
                    {
                        writer.print((lin += gap), _col1, "Sehr geehrte Frau " + mah.AnsprechpartnerNachname + ",");
                    }
                    else
                    {
                        writer.print((lin += gap), _col1, "Sehr geehrter " + mah.AnsprechpartnerAnrede + " " + mah.AnsprechpartnerNachname + ",");
                    }
                }
                else
                {
                    writer.print((lin += gap), _col1, "Sehr geehrte Damen und Herren,");
                }
            }
            else if (mah.SchuldnerTyp == 2) // Frau
            {
                writer.print((lin += gap), _col1, "Sehr geehrte Frau " + mah.SchuldnerNachname + ",");
            }
            else
            {
                writer.print((lin += gap), _col1, "Sehr geehrter Herr " + mah.SchuldnerNachname + ",");
            }
            lin += gap;
            
                string[] lines = info.Split('\n');
                foreach (var line in lines)
                {
                    IEnumerable<string> lins = HTBUtils.SplitStringInPdfLines(line, 90);
                    foreach (var l in lins)
                    {
                        writer.print((lin += gap), _col1, l);
                    }
                }
            lin += gap;
            if (isForderungsauftellung)
            {
                writer.setFont("Calibri", 11, true, false, false);
                writer.print((lin += gap), _col1, "Zahlungen mit schuldbefreiender Wirkung sind ausschliesslich an European Car Protect zu entrichten.");
                writer.setFont("Calibri", 11);
                writer.print((lin += gap*2), _col1, "FORDERUNGSAUFSTELLUNG der Rechnungsnummer [" + mah.RechnungsNummer + "] vom [" + mah.RechnungsDatum.ToShortDateString() + "]:");
                lin += gap*3;
                lin = PrintKosten(writer, lin, gap, mah);
                lin += gap*2;
                switch (mah.Number)
                {
                    case 1:
                        writer.print((lin += gap), _col1, "Sollten Sie die angeführte Forderung schon bezahlt haben, betrachten Sie bitte unser");
                        writer.print((lin += gap), _col1, "Schreiben als gegenstandslos.");
                        break;
                    case 2:
                        writer.print((lin += gap), _col1, "Bedenken Sie die hohen Gerichts- und Anwaltskosten sowie die Unannehmlichkeiten");
                        writer.print((lin += gap), _col1, "(Exekution), die eine gerichtliche Betreibung mit sich bringt.");
                        break;
                    case 3:
                        writer.print((lin += gap), _col1, "Sollten Sie wieder nicht reagieren, müsste Klage und Exekution gegen Sie eingebracht");
                        writer.print((lin += gap), _col1, "werden. Sollten Sie die angeführte Forderung schon bezahlt haben, betrachten Sie bitte");
                        writer.print((lin += gap), _col1, "unser Schreiben als gegenstandslos.");
                        break;
                }
                lin += gap*2;
            }
            writer.print((lin += gap), _col1, "Mit freundlichem Gruß");
            lin += gap;
            writer.print((lin + gap), _col1, "E.C.P. European Car Protect");

            if(includeRatenversuchen)
            {
                writer.newPage();
                var ratenRpt = new Ratenansuchen
                                   {
                                       Writer = writer,
                                       Akt = (qryCustInkAkt) HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + mah.Aktenzahl, typeof (qryCustInkAkt))
                                   };
                ratenRpt.Init();
                ratenRpt.PrintPageHeader();
                ratenRpt.PrintRatenansuchen();
            }
            if (includeRatenplan)
            {
                writer.newPage();
                var ratenRpt = new RatenPlan()
                {
                    Writer = writer,
                    Akt = (qryCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryCustInkAkt WHERE CustInkAktID = " + mah.Aktenzahl, typeof(qryCustInkAkt))
                };
                ratenRpt.Init();
                ratenRpt.PrintPageHeader();
                ratenRpt.PrintRatenplan();
            }
            writer.Close();
        }

        public void GenerateMahnungOrTerminverlust(MahnungsListRecord recordList)
        {
            string mahnungsDir = HTBUtils.GetConfigValue("DocumentsFolder");
            string emailTo = HTBUtils.GetConfigValue("Default_EMail_Addr");
            var email = new HTBEmail();
            foreach (object obj in recordList.MahnungsList)
            {
                if (obj is EcpTerminverlustRec)
                {
                    string fileName = mahnungsDir + "TVL_" + ((EcpTerminverlustRec)obj).Aktenzahl + "_" + ((EcpTerminverlustRec)obj).RateID + ".pdf";
                    GenerateTerminverlust((EcpTerminverlustRec)obj, new FileStream(fileName, FileMode.OpenOrCreate));
                    Log.Info("SENDING TVL EMAIL TO :" + emailTo);
                    email.SendMahnung(emailTo, fileName, "Terminverlust zum Senden: [Akt: " + ((EcpTerminverlustRec)obj).Aktenzahl + " ] [Rate vom: " + ((EcpTerminverlustRec)obj).RateDate.ToShortDateString() + " ]", "Terminverlust.pdf");
                }
                else if (obj is EcpMahnung)
                {
                    string fileName = mahnungsDir + "Mahnung_" + ((EcpMahnung)obj).Aktenzahl + "_" + ((EcpMahnung)obj).Number + ".pdf";
                    GenerateMahnung((EcpMahnung)obj, new FileStream(fileName, FileMode.OpenOrCreate));
                    Log.Info("SENDING MAH EMAIL TO :" + emailTo);
                    email.SendMahnung(emailTo, fileName, "Mahnung zum Senden: [Akt: " + ((EcpMahnung)obj).Aktenzahl + "] [Mahnung: " + ((EcpMahnung)obj).Number + "]", "Mahnung.pdf");
                }
            }
        }

        #region Mahnung
        private void GenerateMahnung(EcpMahnung mah, Stream os)
        {
            int lin = 400;
            int gap = 37;
            _col = 390;
            _col1 = _col + 33;
            _col2 = _col1 + 1100;

            var writer = new ECPPdfWriter();
            string logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");

            writer.setFormName("A4");
            writer.open(os);

            writer.PrintLeftEcpInfo();

            writer.drawBitmap(350, _col, Image.GetInstance(logoPath), 40);
            for (int i = 0; i < 3; i++)
            {
                writer.setFont("Arial", 22, true, false, false);
                writer.print(50, _col + 1600 + i, "EUROPEAN CAR PROTECT", 'R', BaseColor.BLUE); // give it a bolder look
                writer.setFont("Arial", 16, true, false, false);
                writer.print(120, _col + 1600 + i, "INKASSO-SERVICE", 'R', BaseColor.BLUE); // give it a bolder look
            }

            writer.setFont("Calibri", 8);
            writer.print((lin += gap), _col1, "E.C.P. European Car Protect KG");
            writer.print((lin += gap), _col1, "Schwarzparkstrasse 15   A 5020 Salzburg");
            writer.drawLine(lin + 30, _col1, lin + 30, _col1 + 465);
            lin += gap;
            writer.setFont("Calibri", 11);
            writer.print((lin += gap), _col1, mah.SchuldnerAnrede);
            if (mah.SchuldnerTyp == 0)
            {
                writer.print((lin += gap), _col1, mah.NameFirma);
                if (mah.AnsprechpartnerNachname != null && mah.AnsprechpartnerNachname.Trim() != "")
                {
                    writer.print((lin += gap), _col1, mah.AnsprechpartnerAnrede + " " + mah.AnsprechpartnerVorename + " " + mah.AnsprechpartnerNachname);
                }
            }
            else
            {
                writer.print((lin += gap), _col1, mah.SchuldnerVorname + " " + mah.SchuldnerNachname);
            }
            if (!string.IsNullOrEmpty(mah.SchuldnerPostInfo))
            {
                writer.print((lin += gap), _col1, mah.SchuldnerPostInfo);
            }
            writer.print((lin += gap), _col1, mah.Strasse);
            writer.print((lin += gap), _col1, mah.LKZ + " - " + mah.PLZ + " " + mah.Ort);
            writer.print((lin += gap), _col + 1600, "Salzburg, am " + mah.Date.ToShortDateString(), 'R');
            lin += gap * 5;
            writer.setFont("Calibri", 11, true, false, false);
            writer.print((lin += gap), _col1, mah.Number + ". MAHNUNG");
            lin += gap;
            writer.print((lin += gap), _col1, "AKTENZAHL: " + mah.Aktenzahl);
            writer.print((lin += gap), _col1, "Klient:            " + mah.KlientName);
            lin += gap * 2;
            writer.setFont("Calibri", 11);
            if (mah.SchuldnerTyp == 0) // firma
            {
                if (mah.AnsprechpartnerNachname != null && mah.AnsprechpartnerNachname.Trim() != "")
                {
                    if (mah.AnsprechpartnerAnrede.ToLower() == "frau")
                    {
                        writer.print((lin += gap), _col1, "Sehr geehrte Frau " + mah.AnsprechpartnerNachname + ",");
                    }
                    else
                    {
                        writer.print((lin += gap), _col1, "Sehr geehrter  " + mah.AnsprechpartnerAnrede + " " + mah.AnsprechpartnerNachname + ",");
                    }
                }
                else
                {
                    writer.print((lin += gap), _col1, "Sehr geehrte Damen und Herren,");
                }
            }
            else if (mah.SchuldnerTyp == 2) // Frau
            {
                writer.print((lin += gap), _col1, "Sehr geehrte Frau " + mah.SchuldnerNachname + ",");
            }
            else
            {
                writer.print((lin += gap), _col1, "Sehr geehrter Herr " + mah.SchuldnerNachname + ",");
            }
            lin += gap;
            switch (mah.Number)
            {
                case 1:
                    lin = PrintMahnung1(writer, lin, _col1, gap, mah);
                    break;
                case 2:
                    lin = PrintMahnung2(writer, lin, _col1, gap, mah);
                    break;
                case 3:
                    lin = PrintMahnung3(writer, lin, _col1, gap, mah);
                    break;
                case 4:
                    lin = PrintMahnung4(writer, lin, _col1, gap, mah);
                    break;
            }
            lin += gap;
            writer.setFont("Calibri", 11, true, false, false);
            writer.print((lin += gap), _col1, "Zahlungen mit schuldbefreiender Wirkung sind ausschliesslich an European Car Protect zu entrichten.");
            writer.setFont("Calibri", 11);
            writer.print((lin += gap * 2), _col1, "FORDERUNGSAUFSTELLUNG der Rechnungsnummer [" + mah.RechnungsNummer + "] vom [" + mah.RechnungsDatum.ToShortDateString() + "]:");
            lin += gap * 3;
            lin = PrintKosten(writer, lin, gap, mah);
            lin += gap * 2;
            switch (mah.Number)
            {
                case 1:
                    writer.print((lin += gap), _col1, "Sollten Sie die angeführte Forderung schon bezahlt haben, betrachten Sie bitte unser");
                    writer.print((lin += gap), _col1, "Schreiben als gegenstandslos.");
                    break;
                case 2:
                    writer.print((lin += gap), _col1, "Bedenken Sie die hohen Gerichts- und Anwaltskosten sowie die Unannehmlichkeiten");
                    writer.print((lin += gap), _col1, "(Exekution), die eine gerichtliche Betreibung mit sich bringt.");
                    break;
                case 3:
                    writer.print((lin += gap), _col1, "Sollten Sie wieder nicht reagieren, müsste Klage und Exekution gegen Sie eingebracht");
                    writer.print((lin += gap), _col1, "werden. Sollten Sie die angeführte Forderung schon bezahlt haben, betrachten Sie bitte");
                    writer.print((lin += gap), _col1, "unser Schreiben als gegenstandslos.");
                    break;
                case 4:
                    writer.print((lin += gap), _col1, "Bedenken Sie die hohen Gerichts- und Anwaltskosten sowie die Unannehmlichkeiten, die eine");
                    writer.print((lin += gap), _col1, "gerichtliche Betreibung mit sich bringt.");
                    break;
            }

            writer.setFont("Calibri", 11);
            lin += gap * 2;
            writer.print((lin += gap), _col1, "Mit freundlichem Gruß");
            lin += gap;
            writer.print((lin + gap), _col1, "E.C.P. European Car Protect");
            
            writer.setFont("Calibri", 6);
            lin += gap * 2;
            writer.print((lin += gap), _col1, "Für den Fall, dass dieser Zahlungsaufforderung nicht vollständig und fristgerecht entsprochen wird und das Bestehen der Forderung unbestritten bleibt, werden Ihre Daten an die ");
            writer.print((lin += gap - 5), _col1, "Deltavista GmbH, Diefenbachg. 35, 1150 Wien übermittelt, welche diese Daten als datenschutzrechtlicher Auftraggeber zum Zweck der Erteilung von Bonitätsauskünften ermittelt.");
            
            writer.Close();
        }

        private static int PrintMahnung1(ECPPdfWriter writer, int lin, int col, int gap, EcpMahnung mah)
        {
            writer.print((lin += gap), col, "unser vorgenannter Klient teilt uns mit, dass Sie trotz mehrmaliger Aufforderung seine Forderung");
            writer.print((lin += gap), col, "nicht bezahlt haben. Trotzdem ist unser Klient bereit, es noch ein letztes Mal über uns zu versuchen.");
            lin += gap;
            //writer.print((lin += gap), col, "Zahlen Sie bitte bis spätestens " + mah.Faelligkeitsdatum.ToShortDateString() + " den unten aufgeführten Betrag an unser Büro mit dem");
            writer.print((lin += gap), col, "Zahlen Sie bitte bis spätestens");
            writer.setFont("Calibri", 11, true, false, false);
            writer.print(lin, col + 490, mah.Faelligkeitsdatum.ToShortDateString());
            writer.setFont("Calibri", 11);
            writer.print(lin, col + 680, "den unten aufgeführten Betrag an unser Büro mit dem");
            writer.print((lin += gap), col, "beiliegenden Zahlschein. Sollte die Zahlung bis zu diesem Termin nicht erfolgen, dann werden Sie von");
            writer.print((lin += gap), col, "unserem Außendienstpartner persönlich besucht, was wiederum mit erheblichen Mehrkosten");
            writer.print((lin += gap), col, "verbunden ist.");
            return lin;
        }

        private static int PrintMahnung2(ECPPdfWriter writer, int lin, int col, int gap, EcpMahnung mah)
        {
            writer.print((lin += gap), col, "leider haben Sie unser Mahnschreiben nicht beachtet.");
            lin += gap;
            writer.print((lin += gap), col, "Wir können nicht verstehen, dass Sie Ihre Zahlungsangelegenheiten nicht regeln wollen. Dabei wäre");
            writer.print((lin += gap), col, "es so einfach! Rufen Sie uns an, wir finden garantiert eine Lösung!");
            lin += gap;
            writer.print((lin += gap), col, "Sollten Sie auf unser Schreiben wieder nicht reagieren, bleibt uns leider nur mehr die Möglichkeit, die");
            writer.print((lin += gap), col, "Klage bei Gericht gegen Sie einzubringen. Wir hoffen jedoch, dass Sie es nicht soweit kommen lassen,");
            writer.print((lin += gap), col, "denn die gerichtliche Beitreibung kostet Sie um Vieles mehr!");
            lin += gap;
            // writer.print((lin += gap), col, "Zahlen Sie bitte nunmehr bis spätestens " + mah.Faelligkeitsdatum.ToShortDateString() + " den unten aufgeführten Betrag an unser Büro mit");
            writer.print((lin += gap), col, "Zahlen Sie bitte bis spätestens");
            writer.setFont("Calibri", 11, true, false, false);
            writer.print(lin, col + 490, mah.Faelligkeitsdatum.ToShortDateString());
            writer.setFont("Calibri", 11);
            writer.print(lin, col + 680, "den unten aufgeführten Betrag an unser Büro mit dem");
            writer.print((lin += gap), col, "beiliegenden Zahlschein.");
            return lin;
        }

        private static int PrintMahnung3(ECPPdfWriter writer, int lin, int col, int gap, EcpMahnung mah)
        {
            writer.print((lin += gap), col, "trotz zweimaliger Aufforderung haben Sie bisher weder eine Zahlung geleistet, noch mit unserem");
            writer.print((lin += gap), col, "Büro Kontakt aufgenommen.");
            lin += gap;
            writer.print((lin += gap), col, "Unsere Geduld ist nun erschöpft! Wir fordern Sie heute letztmalig auf, den unten aufgeführten Betrag");
            //writer.print((lin += gap), col, "nunmehr bis spätestens  " + mah.Faelligkeitsdatum.ToShortDateString() + " an unser Büro mit dem beiliegenden Zahlschein zu überweisen.");
            writer.print((lin += gap), col, "nunmehr bis spätestens");
            writer.setFont("Calibri", 11, true, false, false);
            writer.print(lin, col + 390, mah.Faelligkeitsdatum.ToShortDateString());
            writer.setFont("Calibri", 11);
            writer.print(lin, col + 580, "an unser Büro mit dem beiliegenden Zahlschein zu überweisen.");
            lin += gap;
            return lin;
        }

        private static int PrintMahnung4(ECPPdfWriter writer, int lin, int col, int gap, EcpMahnung mah)
        {
            writer.print((lin += gap), col, "nachdem Sie die bisherigen Mahnungen ignoriert haben und sich auch nicht mit uns wegen einer");
            writer.print((lin += gap), col, "Zahlungsmodalität in Verbindung gesetzt haben, sahen wir uns gezwungen, den Akt zur");
            writer.print((lin += gap), col, "GERICHTLICHEN EINTREIBUNG weiterzuleiten. Der beauftragte Rechtsanwalt wird gegen Sie die");
            writer.print((lin += gap), col, "Klage einbringen.");
            lin += gap;
            writer.print((lin += gap), col, "Wir haben jedoch die Möglichkeit, den Akt bis zum ");
            writer.setFont("Calibri", 11, true, false, false);
            writer.print(lin, col + 810, mah.Faelligkeitsdatum.ToShortDateString());
            writer.setFont("Calibri", 11);
            writer.print(lin, col + 1000, "zurückzurufen. Dies geschieht jedoch");
            writer.print((lin += gap), col, "nur, wenn Sie sich innerhalb der genannten Frist mit uns in Verbindung setzen.");
            lin += gap;
            return lin;
        }
        #endregion

        #region Terminverlust
        private void GenerateTerminverlust(EcpTerminverlustRec mah, Stream os)
        {
            int lin = 400;
            int gap = 37;
            _col = 390;
            _col1 = _col + 33;
            _col2 = _col1 + 1100;

            var writer = new ECPPdfWriter();
            string logoPath = HTBUtils.GetConfigValue("LogoPath_Mahnung");

            writer.setFormName("A4");
            writer.open(os);

            writer.PrintLeftEcpInfo();

            writer.drawBitmap(350, _col, Image.GetInstance(logoPath), 40);
            for (int i = 0; i < 3; i++)
            {
                writer.setFont("Arial", 22, true, false, false);
                writer.print(50, _col + 1600 + i, "EUROPEAN CAR PROTECT", 'R', BaseColor.BLUE); // give it a bolder look
                writer.setFont("Arial", 16, true, false, false);
                writer.print(120, _col + 1600 + i, "INKASSO-SERVICE", 'R', BaseColor.BLUE); // give it a bolder look
            }

            writer.setFont("Calibri", 8);
            writer.print((lin += gap), _col1, "E.C.P. European Car Protect KG");
            writer.print((lin += gap), _col1, "Schwarzparkstrasse 15   A 5020 Salzburg");
            writer.drawLine(lin + 30, _col1, lin + 30, _col1 + 465);
            lin += gap;
            writer.setFont("Calibri", 11);
            writer.print((lin += gap), _col1, mah.SchuldnerAnrede);
            if (mah.SchuldnerTyp == 0)
            {
                writer.print((lin += gap), _col1, mah.NameFirma);
                if (mah.AnsprechpartnerNachname != null && mah.AnsprechpartnerNachname.Trim() != "")
                {
                    writer.print((lin += gap), _col1, mah.AnsprechpartnerAnrede + " " + mah.AnsprechpartnerVorename + " " + mah.AnsprechpartnerNachname);
                }
            }
            else
            {
                writer.print((lin += gap), _col1, mah.SchuldnerVorname + " " + mah.SchuldnerNachname);
            }
            writer.print((lin += gap), _col1, mah.Strasse);
            writer.print((lin += gap), _col1, mah.LKZ + " - " + mah.PLZ + " " + mah.Ort);
            writer.print((lin += gap), _col + 1600, "Salzburg, am " + mah.Date.ToShortDateString(), 'R');
            lin += gap * 5;
            writer.setFont("Calibri", 11, true, false, false);
            writer.print((lin += gap), _col1, "MAHNUNG wegen Terminverlust");
            lin += gap;
            writer.print((lin += gap), _col1, "AKTENZAHL: " + mah.Aktenzahl);
            writer.print((lin += gap), _col1, "Klient:            " + mah.KlientName);
            lin += gap * 2;
            writer.setFont("Calibri", 11);
            if (mah.SchuldnerTyp == 0) // firma
            {
                if (mah.AnsprechpartnerNachname != null && mah.AnsprechpartnerNachname.Trim() != "")
                {
                    if (mah.AnsprechpartnerAnrede.ToLower() == "frau")
                    {
                        writer.print((lin += gap), _col1, "Sehr geehrte Frau " + mah.AnsprechpartnerNachname + ",");
                    }
                    else
                    {
                        writer.print((lin += gap), _col1, "Sehr geehrter " + mah.AnsprechpartnerAnrede + " " + mah.AnsprechpartnerNachname + ",");
                    }
                }
                else
                {
                    writer.print((lin += gap), _col1, "Sehr geehrte Damen und Herren,");
                }
            }
            else if (mah.SchuldnerTyp == 2) // Frau
            {
                writer.print((lin += gap), _col1, "Sehr geehrte Frau " + mah.SchuldnerNachname + ",");
            }
            else
            {
                writer.print((lin += gap), _col1, "Sehr geehrter Herr " + mah.SchuldnerNachname + ",");
            }
            lin += gap;
            writer.print((lin += gap), _col1, "Sie haben sich nicht an Ihre Ratenvereinbarung gehalten, dadurch ist der Terminverlust eingetreten");
            writer.print((lin += gap), _col1, "und somit ist diese Vereinbarung aufgelöst!");
            lin += gap;
            writer.print((lin += gap), _col1, "Sollten Sie sich jedoch in den nächsten zwei Tagen bei uns melden, so besteht letztmalig");
            writer.print((lin += gap), _col1, "die Möglichkeit Ihre Ratenvereinbarung noch fortzuführen.");
            writer.print((lin += gap), _col1, "Wird auch diese Möglichkeit nicht von Ihnen genützt so wird diese Forderung dem Anwalt übergeben");
            writer.print((lin += gap), _col1, "und Klage gegen Sie eingebracht,  was wiederum mit erheblichen Mehrkosten verbunden ist.");
            
            lin += gap;
            writer.setFont("Calibri", 11, true, false, false);
            writer.print((lin += gap), _col1, "Zahlungen mit schuldbefreiender Wirkung sind ausschliesslich an European Car Protect zu entrichten.");
            writer.setFont("Calibri", 11);
            writer.print((lin += gap * 2), _col1, "FORDERUNGSAUFSTELLUNG der Rechnungsnummer [" + mah.RechnungsNummer + "] vom [" + mah.RechnungsDatum.ToShortDateString() + "]:");
            lin += gap * 3;
            lin = PrintKosten(writer, lin, gap, mah);
            lin += gap * 2;
            writer.print((lin += gap), _col1, "Sollten Sie die angeführte Forderung schon bezahlt haben, betrachten Sie bitte unser");
            writer.print((lin += gap), _col1, "Schreiben als gegenstandslos.");
            lin += gap * 2;
            writer.print((lin += gap), _col1, "Mit freundlichem Gruß");
            lin += gap;
            writer.print((lin + gap), _col1, "E.C.P. European Car Protect");

            lin += gap * 2;
            writer.print((lin += gap), _col1, "Für den Fall, dass dieser Zahlungsaufforderung nicht vollständig und fristgerecht entsprochen wird und das Bestehen der Forderung unbestritten bleibt, werden Ihre Daten an die ");
            writer.print((lin += gap - 5), _col1, "Deltavista GmbH, Diefenbachg. 35, 1150 Wien übermittelt, welche diese Daten als datenschutzrechtlicher Auftraggeber zum Zweck der Erteilung von Bonitätsauskünften ermittelt.");
            
            writer.Close();
            os.Close();
            os.Dispose();
        }
        #endregion

        private int PrintKosten(ECPPdfWriter writer, int lin, int gap, EcpMahnung mah)
        {
            var gap1 = gap + 20;
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Hauptforderung", mah.Forderung);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Mahnspesen/Zinsen", mah.MahnspesenZinsen);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Bearbeitungsgebühren Kosten", mah.Bearbeitung);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Gebühr 1. Mahnung", mah.Mahnung1);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Gebühr 2. Mahnung", mah.Mahnung2);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Gebühr 3. Mahnung", mah.Mahnung3);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Gebühr 4. Mahnung", mah.Mahnung4);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Gebühr 5. Mahnung", mah.Mahnung5);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Telefoninkasso", mah.Telefonincasso);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Meldeerhebung", mah.Meldeergebung);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Evidenzhaltungsgebühr", mah.Evidenzhaltung);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Persönliche Intervention/Weggebühr", mah.InterventionWeg);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Ratenangebot/-ansuchen", mah.Ratenangebot);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Terminverlust", mah.Terminverlust);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " Porto", mah.Porto);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " -Kostenreduktion", mah.KostenReduktion);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " -Forderung/ Kosten bereits bezahlt", mah.Bezahlt);
            lin = PrintKostLine(writer, lin, _col1, _col2, gap1, " 20% Mehrwertsteuer Kosten", mah.Steuer);
            lin += gap;
            writer.setFont("Calibri", 11, true, false, false);
            writer.print(lin, _col2 - 33, "Gesamtforderung:                             " + HTBUtils.FormatCurrency(mah.Gesamtforderung), 'R');
            writer.setFont("Calibri", 11);
            return lin;
        }
        
        private static int PrintKostLine(ECPPdfWriter writer, int lin, int col1, int col2, int gap, string text, double amount)
        {
            if (amount > 0)
            {
                writer.drawRectangle(lin, col1, lin + gap, col2, BaseColor.WHITE);
                writer.print(lin + 5, col1, text);
                writer.print(lin + 5, col2 - 33, HTBUtils.FormatCurrency(amount), 'R');
                writer.drawLine(lin, col2 - 300, lin + gap, col2 - 300);
                lin += gap;
            }
            return lin;
        }
    }
}
