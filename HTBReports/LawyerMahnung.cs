using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using HTB.Database.Views;
using HTBUtilities;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace HTBReports
{
    public class LawyerMahnung
    {
        private const string fin = "c:/temp/test.html";
        private const string fout = "c:/temp/test.pdf";

        public void GenerateLawyerMahnungPdf(String text, Stream os)
        {
            string html = HTBUtils.GetFileText(fin);
            //ConvertHtmltoPdf(html);
            //Text2PDF(html);
            HtmlToPdf(html);
            new Process { StartInfo = { FileName = fout } }.Start();
        }

        private static void ConvertHtmltoPdf(string htmlCode)
        {
            
            //Create PDF document
            var doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream(fout, FileMode.Create));
            doc.Open();

            /********************************************************************************/

            foreach (var element in HTMLWorker.ParseToList(new StringReader(htmlCode), null))
            {
                doc.Add(element);
            }
            doc.Close();
            
            /********************************************************************************/

        }

        protected static void Text2PDF(string PDFText)
        {
            //HttpContext context = HttpContext.Current;
            StringReader reader = new StringReader(PDFText);

            //Create PDF document 
            Document document = new Document(PageSize.A4);
            HTMLWorker parser = new HTMLWorker(document);

            PdfWriter.GetInstance(document, new FileStream(fout, FileMode.Create));
            document.Open();

            try
            {
                parser.Parse(reader);
            }
            catch (Exception ex)
            {
                //Display parser errors in PDF. 
                Paragraph paragraph = new Paragraph("Error!" + ex.Message);
                Chunk text = paragraph.Chunks[0] as Chunk;
                if (text != null)
                {
                    text.Font.Color = BaseColor.RED;
                }
                document.Add(paragraph);
            }
            finally
            {
                document.Close();
            }
        }

        protected static void HtmlToPdf(string contents)
        {
            // Create a Document object
            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a new PdfWrite object, writing the output to a MemoryStream

            var writer = PdfWriter.GetInstance(document, new FileStream(fout, FileMode.Create));

            // Open the Document for writing
            document.Open();


            //// Replace the placeholders with the user-specified text
            //contents = contents.Replace("[ORDERID]", txtOrderID.Text);
            //contents = contents.Replace("[TOTALPRICE]", Convert.ToDecimal(txtTotalPrice.Text).ToString("c"));
            //contents = contents.Replace("[ORDERDATE]", DateTime.Now.ToShortDateString());

            //var itemsTable = @"<table><tr><th style=""font-weight: bold"">Item #</th><th style=""font-weight: bold"">Item Name</th><th style=""font-weight: bold"">Qty</th></tr>";
            //foreach (System.Web.UI.WebControls.ListItem item in cblItemsPurchased.Items)
            //    if (item.Selected)
            //    {
            //        // Each CheckBoxList item has a value of ITEMNAME|ITEM#|QTY, so we split on | and pull these values out...
            //        var pieces = item.Value.Split("|".ToCharArray());
            //        itemsTable += string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>",
            //                                    pieces[1], pieces[0], pieces[2]);
            //    }
            //itemsTable += "</table>";

            //contents = contents.Replace("[ITEMS]", itemsTable);


            var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(contents), null);
            foreach (var htmlElement in parsedHtmlElements)
                document.Add(htmlElement as IElement);



            // You can add additional elements to the document. Let's add an image in the upper right corner
            //var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/4guysfromrolla.gif"));
            //logo.SetAbsolutePosition(440, 800);
            //document.Add(logo);

            document.Close();


        }
    }
}
