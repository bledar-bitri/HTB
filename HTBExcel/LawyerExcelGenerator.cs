using System.Collections;
using System.Diagnostics;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using HTB.Database;
using HTB.Database.StoredProcs;

namespace HTBExcel
{
    public class LawyerExcelGenerator : HTBExcelGenerator
    {
        
        public void WriteExcelFile(string fileName, ArrayList[] list)
        {
            var fs = new FileStream(fileName, FileMode.OpenOrCreate);
            WriteExcelFile(fs, list);
            fs.Close();
            fs.Dispose();
            Process.Start(fileName);
        }

        public void WriteExcelFile(Stream stream, ArrayList[] list)
        {
            Workbook book = GetWorkbook();
            GenerateWorksheetSheet1("ECP Buchungen", book.Worksheets, list);
            book.Save(stream);
        }

        private void GenerateWorksheetSheet1(string name, WorksheetCollection sheets, ArrayList[] list)
        {
            Worksheet sheet = sheets.Add(name);
            sheet.Table.DefaultRowHeight = 15F;
//            sheet.Table.ExpandedColumnCount = 26;
//            sheet.Table.ExpandedRowCount = 2;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.Columns.Add(106);
            
            // -----------------------------------------------
            WorksheetRow headerRow = sheet.Table.Rows.Add();
            headerRow.Cells.Add("ECP Akt Anlagedatum", DataType.String, StyleHeader);
            headerRow.Cells.Add("ECP AZ", DataType.String, StyleHeader);
            headerRow.Cells.Add("Rg. Datum", DataType.String, StyleHeader);
            headerRow.Cells.Add("Rg. Nummer", DataType.String, StyleHeader);
            headerRow.Cells.Add("Kundennummer", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Kapital", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Mahnspesen", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Zinsen", DataType.String, StyleHeader);
            headerRow.Cells.Add("ECP Zinsen", DataType.String, StyleHeader);
            headerRow.Cells.Add("ECP Kosten", DataType.String, StyleHeader);
            headerRow.Cells.Add("Zahlungen bis Dato", DataType.String, StyleHeader);
            headerRow.Cells.Add("Saldo bis Dato", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner Typ", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner Nachname/Firma", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner Vorname", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner Alias", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner Straﬂe", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner PLZ/Ort", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner Geb.Dat.", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner E-Mail", DataType.String, StyleHeader);
            headerRow.Cells.Add("Gegner Telefonnummer", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Anrede", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Titel", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Name1", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Name2", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Name3", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Straﬂe ", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient PLZ/Ort", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient E-Mail", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Telefonnummer", DataType.String, StyleHeader);

            headerRow.Cells.Add("Klient Ansprechpartner", DataType.String, StyleHeader);
            headerRow.Cells.Add("Klient Firmenbuchnummer", DataType.String, StyleHeader);

            headerRow.Cells.Add("Auftraggeber", DataType.String, StyleHeader);
            headerRow.Cells.Add("Auftraggeber Straﬂe ", DataType.String, StyleHeader);
            headerRow.Cells.Add("Auftraggeber PLZ/Ort", DataType.String, StyleHeader);
            
            // -----------------------------------------------
            WorksheetRow row;
            foreach (spGetInfoForLawyerAkt layerAkt in list[0])
            {
                row = sheet.Table.Rows.Add();
                row.Cells.Add(layerAkt.AktDate.ToShortDateString(), DataType.String, StyleDate);
                row.Cells.Add(layerAkt.AktID.ToString(), DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.RechnungsDate.ToShortDateString(), DataType.String, StyleDate);
                row.Cells.Add(layerAkt.RechnungsNummer, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KundenNummer, DataType.String, StyleBordered);
                row.Cells.Add(GetExcelNumber(layerAkt.KlientKapital), DataType.Number, StyleBordered);
                row.Cells.Add(GetExcelNumber(layerAkt.KlientMahnspesen), DataType.Number, StyleBordered);
                row.Cells.Add(GetExcelNumber(layerAkt.KlientZinsen), DataType.Number, StyleBordered);
                row.Cells.Add(GetExcelNumber(layerAkt.ECPZinsen), DataType.Number, StyleBordered);
                row.Cells.Add(GetExcelNumber(layerAkt.ECPKosten), DataType.Number, StyleBordered);
                row.Cells.Add(GetExcelNumber(layerAkt.Payments), DataType.Number, StyleBordered);
                row.Cells.Add(GetExcelNumber(layerAkt.Balance), DataType.Number, StyleBordered);
                row.Cells.Add(tblGegner.GetGegnerTypeText(layerAkt.GegnerTyp), DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.GegnerNachname, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.GegnerVorname, DataType.String, StyleBordered);
                
                if (layerAkt.GegnerNachname != layerAkt.GegnerAliasNachname || layerAkt.GegnerVorname != layerAkt.GegnerAliasVorname)
                    row.Cells.Add(layerAkt.GegnerAliasNachname + " " + layerAkt.GegnerAliasVorname, DataType.String, StyleBordered);
                else 
                    row.Cells.Add("", DataType.String, StyleBordered);
                
                row.Cells.Add(layerAkt.GegnerStrasse, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.GegnerCountry + "-" + layerAkt.GegnerPLZ + " " + layerAkt.GegnerOrt, DataType.String, StyleBordered);
                
                if (layerAkt.GegnerGeburtsdatum.ToShortDateString() != "" && layerAkt.GegnerGeburtsdatum.ToShortDateString() != "01.01.1900")
                    row.Cells.Add(layerAkt.GegnerGeburtsdatum.ToShortDateString(), DataType.String, StyleDate);
                else
                    row.Cells.Add("", DataType.String, StyleBordered);

                row.Cells.Add(layerAkt.GegnerEmail, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.GegnerPhoneCountry + " " + layerAkt.GegnerPhoneCity + " " + layerAkt.GegnerPhone, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientAnrede, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientTitel, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientName1, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientName2, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientName3, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientStrasse, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientCountry + "-" + layerAkt.KlientPLZ + " " + layerAkt.KlientOrt, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientEMail, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientPhoneCountry + " " + layerAkt.KlientPhoneCity + " " + layerAkt.KlientPhone, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientAnsprechpartner, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.KlientFirmenbuchnummer, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.Auftraggeber, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.AuftraggeberStrasse, DataType.String, StyleBordered);
                row.Cells.Add(layerAkt.AuftraggeberCountry + "-" + layerAkt.AuftraggeberPLZ + " " + layerAkt.AuftraggeberOrt, DataType.String, StyleBordered);
            }

            sheet.Table.Rows.Add(); 
            sheet.Table.Rows.Add();
            row = sheet.Table.Rows.Add();
            row.Cells.Add("Inkassokosten Details", DataType.String, StyleHeader);
            row.Cells.Add("", DataType.String, StyleHeader);
            row.Cells.Add("", DataType.String, StyleHeader);
            sheet.Table.Rows.Add();
            foreach (tblCustInkAktInvoice rec in list[1])
            {
                row = sheet.Table.Rows.Add();
                row.Cells.Add(rec.InvoiceDate.ToShortDateString(), DataType.String, StyleBordered);
                row.Cells.Add(rec.InvoiceDescription, DataType.String, StyleBordered);
                row.Cells.Add(GetExcelNumber(rec.InvoiceAmount), DataType.Number, StyleBordered);
                
            }
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.Selected = true;
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Header.Margin = 0.3F;
            sheet.Options.PageSetup.Footer.Margin = 0.3F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
            sheet.Options.PageSetup.PageMargins.Left = 0.7F;
            sheet.Options.PageSetup.PageMargins.Right = 0.7F;
            sheet.Options.PageSetup.PageMargins.Top = 0.75F;
        }
    }
}
