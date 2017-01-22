using System;
using CarlosAg.ExcelXmlWriter;

namespace HTBExcel
{
    public class HTBExcelGenerator
    {
        public const string StyleDefault = "sdef";
        public const string StyleHeader = "shdr";
        public const string StyleHeaderGreen = "shdrgr";
        public const string StyleInteger = "sint";
        public const string StyleNumber = "snum";
        public const string StyleCurrency = "scrr";
        public const string StyleDate = "sdte";
        public const string StyleBordered = "sborder";

        protected Workbook GetWorkbook()
        {
            Workbook book = new Workbook();
            // -----------------------------------------------
            //  Properties
            // -----------------------------------------------
            book.Properties.Author = "HTB";
            book.Properties.LastAuthor = "HTB";
            book.Properties.Created = DateTime.Now;
            book.Properties.Version = "12.00";
            book.ExcelWorkbook.WindowHeight = 9525;
            book.ExcelWorkbook.WindowWidth = 16275;
            book.ExcelWorkbook.WindowTopX = 240;
            book.ExcelWorkbook.WindowTopY = 75;
            book.ExcelWorkbook.ProtectWindows = false;
            book.ExcelWorkbook.ProtectStructure = false;

            GenerateStyles(book.Styles);

            return book;
        }
        
        private void GenerateStyles(WorksheetStyleCollection styles) {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle s = styles.Add(StyleDefault);
            s.Font.FontName = "Calibri";
            s.Font.Size = 11;
            s.Font.Color = "#000000";
            s.Alignment.Vertical = StyleVerticalAlignment.Bottom;

            // -----------------------------------------------
            //  Header Style
            // -----------------------------------------------
            s = styles.Add(StyleHeader);
            s.Font.FontName = "Calibri";
            s.Font.Size = 11;
            s.Font.Color = "#000000";
            s.Font.Bold = true;
            s.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            
            // -----------------------------------------------
            //  Header Style Green
            // -----------------------------------------------
            s = styles.Add(StyleHeaderGreen);
            s.Font.FontName = "Calibri";
            s.Font.Size = 11;
            s.Font.Color = "#000000";
            s.Font.Bold = true;

            s.Interior.Color = "#9BBB59";
            s.Interior.Pattern = StyleInteriorPattern.Solid;

            s.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            // -----------------------------------------------
            //  StyleBordered
            // -----------------------------------------------
            s = styles.Add(StyleBordered);
            s.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            
            // -----------------------------------------------
            //  StyleDate
            // -----------------------------------------------
            s = styles.Add(StyleDate);
            s.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s.NumberFormat = "Short Date";
            
            // -----------------------------------------------
            //  StyleInteger
            // -----------------------------------------------
            s = styles.Add(StyleInteger);
            s.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s.NumberFormat = "0";

            // -----------------------------------------------
            //  StyleCurrency
            // -----------------------------------------------
            s = styles.Add(StyleCurrency);
            s.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s.NumberFormat = "\"€\" #,##0.00";
//            s.NumberFormat = "\"€\"\\ #.##0,00;[Red]\"€\"\\ #.##0,00";
            
        }
        
        private void GenerateWorksheetSheet2(WorksheetCollection sheets) {
            Worksheet sheet = sheets.Add("Sheet2");
            sheet.Table.DefaultRowHeight = 15F;
            sheet.Table.ExpandedColumnCount = 1;
            sheet.Table.ExpandedRowCount = 1;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Header.Margin = 0.3F;
            sheet.Options.PageSetup.Footer.Margin = 0.3F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
            sheet.Options.PageSetup.PageMargins.Left = 0.7F;
            sheet.Options.PageSetup.PageMargins.Right = 0.7F;
            sheet.Options.PageSetup.PageMargins.Top = 0.75F;
        }
        
        private void GenerateWorksheetSheet3(WorksheetCollection sheets) {
            Worksheet sheet = sheets.Add("Sheet3");
            sheet.Table.DefaultRowHeight = 15F;
            sheet.Table.ExpandedColumnCount = 1;
            sheet.Table.ExpandedRowCount = 1;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Header.Margin = 0.3F;
            sheet.Options.PageSetup.Footer.Margin = 0.3F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
            sheet.Options.PageSetup.PageMargins.Left = 0.7F;
            sheet.Options.PageSetup.PageMargins.Right = 0.7F;
            sheet.Options.PageSetup.PageMargins.Top = 0.75F;
        }

        protected static string GetExcelNumber(double num)
        {
//            return num.ToString();
            return num.ToString("N2").Replace(".", "").Replace(",", ".");
        }
    }
}
