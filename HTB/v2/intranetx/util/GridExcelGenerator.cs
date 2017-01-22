using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarlosAg.ExcelXmlWriter;
using HTBExcel;


namespace HTB.v2.intranetx.util
{
    public class GridExcelGenerator : HTBExcelGenerator
    {
        
        public void WriteExcelFile(Stream stream , GridView gv)
        {
            Workbook book = GetWorkbook();
            GenerateWorksheetSheet1("Sheet 1", book.Worksheets, gv);
            book.Save(stream);
        }

        private void GenerateWorksheetSheet1(string name, WorksheetCollection sheets, GridView gv)
        {
            Worksheet sheet = sheets.Add(name);
            sheet.Table.DefaultRowHeight = 15F;
            sheet.Table.ExpandedColumnCount = 26;
            sheet.Table.ExpandedRowCount = gv.Rows.Count;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.Columns.Add(106);

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow gvRow = gv.Rows[i];
                WorksheetRow row = sheet.Table.Rows.Add();
                PrepareControlForExport(gvRow);
                for (int j = 0; j < gv.Columns.Count; j++)
                {
                    row.Cells.Add(gv.Rows[i].Cells[j].Text, DataType.String, StyleHeader);
                }
            }
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

        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }

    }
}
