using System;
using System.Collections.Specialized;
using System.Web;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.upload
{
    public partial class ReceiveUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int aktId = GlobalUtilArea.GetZeroIfConvertToIntError(Request.Params[GlobalHtmlParams.ID]);
            if (aktId <= 0)
            {
                Response.Write("Kein Aktenzahl!");
                return;
            }
            var akt = HTBUtils.GetInterventionAkt(aktId);
            if (akt == null)
            {
                Response.Write("Akt ["+aktId+"] Nicht Gefunden");
                return;
            }
            try
            {
                HttpFileCollection uploadFiles = Request.Files;
                // Loop over the uploaded files and save to disk.
                int i;
                for (i = 0; i < uploadFiles.Count; i++)
                {
                    HttpPostedFile postedFile = uploadFiles[i];

                    // Access the uploaded file's content in-memory:
                    System.IO.Stream inStream = postedFile.InputStream;
                    byte[] fileData = new byte[postedFile.ContentLength];
                    inStream.Read(fileData, 0, postedFile.ContentLength);
                    
                    string fileName = aktId.ToString() + "_" + DateTime.Now.ToString().Replace(" ", "-").Replace(":", "") + "_" + postedFile.FileName;
                    string index = GetIndex(postedFile.FileName);
                    /*
                    Response.Write(HTBUtils.GetConfigValue("DocumentsFolder") + fileName);
                    Response.Write("\n\r");
                    Response.Write("index:  "+index);
                    Response.Write("\n\r");

                    PrintFormParameters();
                     */ 

                    string description = Request.Params["description_" + index];
                    if(string.IsNullOrEmpty(description))
                    {
                        description = "IPad_" + postedFile.FileName;
                    }
                    // Save the posted file in our "data" virtual directory.
                    postedFile.SaveAs(HTBUtils.GetConfigValue("DocumentsFolder") + fileName);
                    SaveDocumentRecord(akt.AktIntID, akt.AktIntSB, description, fileName);
                }
                Response.Write("OKK");
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }

        private void SaveDocumentRecord(int aktNumber, int creator, string description, string fileName)
        {
            var doc = new tblDokument
            {
                // CollectionInvoice
                DokDokType = 25,
                DokCaption = description,
                DokInkAkt = aktNumber,
                DokCreator = creator,
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

        private string GetIndex(string fileName)
        {
            int idx = fileName.IndexOf("_");
            if(idx > 0)
            {
                string fname = fileName.Substring(idx+1);
                idx = fname.IndexOf(".");
                if (idx > 0)
                    return fname.Substring(0, idx);
            }
            return "";
        }
    }
}