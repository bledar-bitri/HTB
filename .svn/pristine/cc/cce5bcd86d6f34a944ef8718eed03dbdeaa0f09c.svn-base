using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using HTB.Database;
using HTB.Database.StoredProcs;
using HTBExtras;

namespace HTBExcel
{
    public class TransferExcelGenerator : HTBExcelGenerator
    {

        public void WriteExcelFile(string fileName, ArrayList list, DateTime from, DateTime to)
        {
            var fs = new FileStream(fileName, FileMode.OpenOrCreate);
            WriteExcelFile(fs, list, from, to);
            fs.Close();
            fs.Dispose();
            Process.Start(fileName);
        }

        public void WriteExcelFile(Stream stream, ArrayList list, DateTime from, DateTime to)
        {
            Workbook book = GetWorkbook();
            GenerateWorksheetSheet("Überweisungsliste", book.Worksheets, list, from, to);
            book.Save(stream);
        }

        private void GenerateWorksheetSheet(string name, WorksheetCollection sheets, ArrayList list, DateTime from, DateTime to)
        {
            Worksheet sheet = sheets.Add(name);
            sheet.Table.DefaultRowHeight = 15F;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.Columns.Add(106);

            // -----------------------------------------------
            WorksheetRow peirodRow = sheet.Table.Rows.Add();
            peirodRow.Cells.Add(from.ToShortDateString() + " - " + to.ToShortDateString(), DataType.String, StyleHeader);
            
            sheet.Table.Rows.Add(); // empty row
            // -----------------------------------------------
            WorksheetRow headerRow = sheet.Table.Rows.Add();
            headerRow.Cells.Add("Aktenzeichen", DataType.String, StyleHeader);
            headerRow.Cells.Add("Name Schuldner", DataType.String, StyleHeader);
            headerRow.Cells.Add("Rg. Nummer", DataType.String, StyleHeader);
            headerRow.Cells.Add("Kundennummer", DataType.String, StyleHeader);
            headerRow.Cells.Add("Datum", DataType.String, StyleHeader);
            headerRow.Cells.Add("Betrag (Ausgang)", DataType.String, StyleHeader);
            headerRow.Cells.Add("Buchung Kapital", DataType.String, StyleHeaderGreen);
            headerRow.Cells.Add("Buchung Zinsen", DataType.String, StyleHeaderGreen);
            
            // -----------------------------------------------
            foreach (KlientTransferRecord invoice in list)
            {
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Cells.Add(string.IsNullOrEmpty(invoice.AktAZ) ? invoice.AktId.ToString() : invoice.AktAZ, DataType.String, StyleDate);
                row.Cells.Add(invoice.GegnerName, DataType.String, StyleBordered);
                row.Cells.Add(invoice.KlientInvoiceNumber, DataType.String, StyleBordered);
                row.Cells.Add(invoice.KlientCustomerNumber, DataType.String, StyleBordered);
                row.Cells.Add(invoice.TransferDate.ToShortDateString(), DataType.String, StyleBordered);
                row.Cells.Add(GetExcelNumber(invoice.TransferAmount), DataType.Number, StyleCurrency);
                row.Cells.Add(GetExcelNumber(invoice.AppliedToInvoice), DataType.Number, StyleCurrency);
                row.Cells.Add(GetExcelNumber(invoice.AppliedToInterest), DataType.Number, StyleCurrency);
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
