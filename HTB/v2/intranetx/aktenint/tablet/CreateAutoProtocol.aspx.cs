using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using HTB.Database;
using HTB.Database.Views;
using HTB.v2.intranetx.util;
using HTBReports;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class CreateAutoProtocol : System.Web.UI.Page
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.PROTOCOL_TYPE]) == GlobalHtmlParams.PROTOCOL_TYPE_UBERNAME)
                    CreateProtocolUbername();
                else
                    CreateProtocol();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }

        
        #region Protocol
        private void CreateProtocol()
        {

            HttpFileCollection uploadFiles = Request.Files;
            // Loop over the uploaded files and save to disk.
            tblProtokol protocol = null;
            int i;
            Response.Write("file count  " + uploadFiles.Count + "\n");
            byte[] signature = null;
            string signatureFileName = "";
            for (i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile postedFile = uploadFiles[i];

                // Access the uploaded file's content in-memory:
                Stream inStream = postedFile.InputStream;
                var fileData = new byte[postedFile.ContentLength];
                inStream.Read(fileData, 0, postedFile.ContentLength);

                Response.Write("FILE_" + i + ": " + postedFile.FileName);

                if (postedFile.FileName.ToLower().EndsWith(".xml"))
                {
                    var xmlData = Encoding.UTF8.GetString(fileData);
                    try
                    {
                        postedFile.SaveAs("c:\\temp\\Protokol_xml_" + DateTime.Now.ToFileTime() + "_" +
                                          new Random().Next(1000) + ".xml");
                    }
                    catch 
                    {
                    }
                    protocol = GetProtocol(xmlData);
                    
                }
                else if (postedFile.FileName.ToLower().StartsWith("image"))
                {
                    signature = fileData;
                    signatureFileName = HTBUtils.GetConfigValue("SignaturePath") + DateTime.Now.ToFileTime() + "_" + new Random().Next(1000) + ".jpg";
                    postedFile.SaveAs(signatureFileName);
                }
            }
            if (protocol != null)
            {
                UpdatePosRecords(protocol);
                protocol.SignaturePath = signatureFileName;
                protocol.SicherstellungDatum = DateTime.Now;
                var set = new RecordSet();
                if (protocol.ProtokolID == 0)
                {
                    set.InsertRecord(protocol);
                    var saveProtocol = (tblProtokol)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 ProtokolID FROM tblProtokol ORDER BY ProtokolID DESC", typeof(tblProtokol));
                    if(saveProtocol != null)
                    {
                        protocol.ProtokolID = saveProtocol.ProtokolID;
                    }
                }
                else
                {
                    set.UpdateRecord(protocol);
                }
                if (protocol.ProtokolID > 0)
                {
                    #region Visits

                    var sb = new StringBuilder("DELETE tblProtokolBesuch WHERE ProtokolID = ");
                    sb.Append(protocol.ProtokolID);
                    if (!string.IsNullOrEmpty(protocol.VisitsList))
                    {
                        if (protocol.VisitsList.Length > 0)
                        {
                            string[] visits = protocol.VisitsList.Split();
                            foreach (var visit in visits)
                            {
                                sb.Append("INSERT INTO tblProtokolBesuch VALUES (");
                                sb.Append(protocol.ProtokolID);
                                sb.Append(", '");
                                sb.Append(visit);
                                sb.Append("'); ");
                            }
                            try
                            {
                                set.ExecuteNonQuery(sb.ToString());
                            }
                            catch (Exception e)
                            {
                                Log.Error(e);
                            }
                        }
                    }

                    #endregion

                    #region Generate Protocol Document
                    Log.Info($"Akt [{protocol.ProtokolAkt}]  Checking if protocol is needed");
                    //var action = (qryAktenIntActionWithType)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenIntActionWithType WHERE AktIntActionAkt = " + protocol.ProtokolAkt + " AND AktIntActionIsInternal = 0 ORDER BY AktIntActionTime DESC", typeof(qryAktenIntActionWithType));
                    //if (action.AktIntActionIsAutoRepossessed)
                    var akt = (qryAktenInt)HTBUtils.GetSqlSingleRecord("SELECT * FROM qryAktenInt WHERE AktIntID = " + protocol.ProtokolAkt, typeof(qryAktenInt));

                    var validEmails = HTBUtils.GetValidEmailAddressesFromStrings(new[] { protocol.HandlerEMail, akt.UserEMailOffice });
                    if (validEmails.Count > 0 && protocol.UbernommenVon.Trim() != "")
                    {
                        try
                        {
                            bool ok = true;
                            Log.Info($"Akt [{protocol.ProtokolAkt}]  Generating Protocol");
                    

                            ArrayList docsList = HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + protocol.ProtokolAkt, typeof (qryDoksIntAkten));
                            if (akt.IsInkasso())
                                HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksInkAkten WHERE CustInkAktID = " + akt.AktIntCustInkAktID, typeof (qryDoksInkAkten)), docsList);

                            var fileName = "Uebergabe_Protokoll_" + akt.AktIntAZ + ".pdf";
                            var filepath = HTBUtils.GetConfigValue("DocumentsFolder") + fileName;
                            var ms = File.Exists(filepath) ? new FileStream(filepath, FileMode.Truncate) : new FileStream(filepath, FileMode.Create);

                            try
                            {
                                Log.Info($"Akt [{protocol.ProtokolAkt}]  Generating Dealer Protocol");
                                new ProtokolTablet().GenerateDealerProtokol(akt, protocol, ms, validEmails);
                                SaveDocumentRecord(akt.AktIntID, fileName, akt.AktIntSB);
                            }
                            catch (Exception ex)
                            {
                                ok = false;
                                Log.Error(ex);
                            }
                            finally
                            {
                                ms.Close();
                                ms.Dispose();
                            }
                            Log.Info($"Akt [{protocol.ProtokolAkt}]  OK: [{ok}] Dealer EMail: [{protocol.HandlerEMail}] ");
                            if (ok)
                            {
                                
                                Log.Info($"Akt [{protocol.ProtokolAkt}] Sending email");

                                #region Send confirmation EMail to Dealer

                                    
                                using (var fileStream = File.OpenRead(filepath))
                                {
                                    var attachment = new HTBEmailAttachment(fileStream, "Uebergabe_Protokoll_" + akt.AktIntAZ +".pdf", "application/pdf");
                                    new HTBEmail().SendGenericEmail(validEmails, "ECP Übergabe Protokoll", "Siehe Anhang", true, new List<HTBEmailAttachment> {attachment}, 0, akt.AktIntID);
                                    Log.Info($"Akt [{protocol.ProtokolAkt}]  Email Sent TO: [{string.Join(" ", validEmails)}]");

                                }

                                #endregion

                                #region Save Dealer's Email Address

                                if (HTBUtils.IsValidEmail(protocol.HandlerEMail))
                                {
                                    var list = new ArrayList
                                    {
                                        new StoredProcedureParameter("autoDealerId", SqlDbType.Int,
                                            akt.AktIntAutoDealerId),
                                        new StoredProcedureParameter("autoDealerEmail", SqlDbType.VarChar,
                                            protocol.HandlerEMail),
                                    };
                                    HTBUtils.ExecuteStoredProcedure("spUpdateAutoDealerEMail", list);
                                }

                                #endregion
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }

                    #endregion
                }
                Response.Write("OKK");
            }
            else
            {
                Response.Write("ERROR: Could not load protokol !");
            }
        }

        private tblProtokol GetProtocol(string xmlData)
        {
            var ds = new DataSet();
            ds.ReadXml(new StringReader(xmlData));
            var rec = new tblProtokol();
            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == "TBLPROTOKOL")
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        rec.LoadFromDataRow(dr);
                    }
                }
            }
            return rec;
        }

        private void UpdatePosRecords(tblProtokol protocol)
        {
            UpdatePosRecord(protocol.ProtokolAkt, "Zahlung Versicherung", protocol.VersicherungBarKassiert * -1, tblAktenIntPosType.INVOICE_TYPE_PAYMENT_CASH_INSURANCE, protocol.VersicherungUberwiesen);
            UpdatePosRecord(protocol.ProtokolAkt, "Zahlung Forderung", protocol.ForderungBarKassiert * -1, tblAktenIntPosType.INVOICE_TYPE_PAYMENT_CASH_ORIGINAL, protocol.ForderungUberwiesen);
            UpdatePosRecord(protocol.ProtokolAkt, "Zahlung Kosten", protocol.KostenBarKassiert * -1, tblAktenIntPosType.INVOICE_TYPE_PAYMENT_CASH_COLLECTION, protocol.KostenUberwiesen);
            UpdatePosRecord(protocol.ProtokolAkt, "Zahlung Direkt an Versicherung", protocol.DirektzahlungVersicherung * -1, tblAktenIntPosType.INVOICE_TYPE_PAYMENT_DIRECT_INSURANCE, protocol.DirektzahlungVersicherungAm);
            UpdatePosRecord(protocol.ProtokolAkt, "Zahlung Direkt an AG", protocol.Direktzahlung * -1, tblAktenIntPosType.INVOICE_TYPE_PAYMENT_DIRECT_AG_ORIGINAL, protocol.DirektzahlungAm);
        }

        private void UpdatePosRecord(int aktid, string posCaption, double posAmount, int posType, DateTime posTransferredDate)
        {
            /*
            var pos = (tblAktenIntPos)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblAktenIntPos WHERE AktIntPosAkt = " + aktid + " AND AktIntPosNr = 'Zahlung' AND AktIntPosCaption = '"+posCaption+"'", typeof(tblAktenIntPos));
            if(pos != null)
            {
                if (HTBUtils.IsZero(posAmount))
                {
                    RecordSet.Delete(pos);
                }
                else
                {
                    pos.AktIntPosBetrag = posAmount;
                    RecordSet.Update(pos);
                }
            }
            else 
             */ // keep inserting new payments... no update
                if (!HTBUtils.IsZero(posAmount))
                    GlobalUtilArea.InsertPosRecord(aktid, posCaption, posAmount, posType, posTransferredDate);
        }

        #endregion

        #region Protocol Ubername
        private void CreateProtocolUbername()
        {
            HttpFileCollection uploadFiles = Request.Files;
            // Loop over the uploaded files and save to disk.
            tblProtokolUbername protocol = null;
            int i;
            Response.Write("file count  " + uploadFiles.Count + "\n");
            byte[] signature = null;
            string signatureFileName = "";
            for (i = 0; i < uploadFiles.Count; i++)
            {
                HttpPostedFile postedFile = uploadFiles[i];

                // Access the uploaded file's content in-memory:
                Stream inStream = postedFile.InputStream;
                var fileData = new byte[postedFile.ContentLength];
                inStream.Read(fileData, 0, postedFile.ContentLength);

                Response.Write("FILE_" + i + ": " + postedFile.FileName);

                if (postedFile.FileName.ToLower().EndsWith(".xml"))
                {
                    string xmlData = Encoding.UTF8.GetString(fileData);
                    protocol = GetProtocolUbername(xmlData);
                }
                else if (postedFile.FileName.ToLower().StartsWith("image"))
                {
                    signature = fileData;
                    signatureFileName = HTBUtils.GetConfigValue("SignaturePath") + DateTime.Now.ToFileTime() + "_" + new Random().Next(1000) + ".jpg";
                    postedFile.SaveAs(signatureFileName);
                }
            }
            if (protocol != null)
            {
                protocol.UbernameSignaturePath = signatureFileName;
                protocol.UbernameDatum = DateTime.Now;
                try
                {
                    RecordSet.Insert(protocol);
                }
                catch
                {
                    RecordSet.Update(protocol);
                }
                Response.Write("OKK");
            }
            else
            {
                Response.Write("ERROR: Could not load Ubername Protokol  !");
            }
        }
        private tblProtokolUbername GetProtocolUbername(string xmlData)
        {
            var ds = new DataSet();
            ds.ReadXml(new StringReader(xmlData));
            var rec = new tblProtokolUbername();
            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == "TBLPROTOKOLUBERNAME")
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        rec.LoadFromDataRow(dr);
                    }
                }
            }
            return rec;
        }
        #endregion

        private void SaveDocumentRecord(int aktNumber, string fileName, int userId, string caption = "2 Übergabe Protokoll")
        {
            var doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT * FROM tblDokument WHERE DokInkAkt = " + aktNumber + " AND DokAttachment = '" + fileName + "'", typeof(tblDokument));
            if (doc == null)
            {
                doc = new tblDokument
                {
                    // CollectionInvoice
                    DokDokType = 25,
                    DokCaption = caption,
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
    }
}