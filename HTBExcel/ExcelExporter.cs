using System.Collections;
using System.Diagnostics;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using HTB.Database;

namespace HTBExcel
{
    public class ExcelExporter : HTBExcelGenerator
    {
        
        public void WriteExcelFile(string fileName, ArrayList list)
        {
            Workbook book = GetWorkbook();
            GenerateWorksheetSheet1("Sheet 1", book.Worksheets, list);
            book.Save(fileName);
            Process.Start(fileName);
        }

        public void WriteExcelFile(Stream stream, ArrayList list)
        {
            Workbook book = GetWorkbook();
            GenerateWorksheetSheet1("Sheet 1", book.Worksheets, list);
            book.Save(stream);
        }

        private void GenerateWorksheetSheet1(string name, WorksheetCollection sheets, ArrayList list)
        {
            Worksheet sheet = sheets.Add(name);
            sheet.Table.DefaultRowHeight = 15F;
//            sheet.Table.ExpandedColumnCount = 26;
//            sheet.Table.ExpandedRowCount = 2;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.Columns.Add(106);
            
            // -----------------------------------------------
            foreach (Record rec in list)
                WriteRow(sheet.Table.Rows.Add(), rec);
            
            
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

        private void WriteRow(WorksheetRow row, Record rec)
        {
            foreach (object val in rec.ToArrayList())
                row.Cells.Add(val.ToString(), DataType.String, StyleDefault);

        }
    }
}
