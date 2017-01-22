using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint
{
    public partial class ImportGeneral : System.Web.UI.Page
    {
        protected System.Web.UI.HtmlControls.HtmlInputFile fileUpload;
        private string _lastAktImported = "";
        private const string TmpOldId = "sssss";
        int _coutner;

        
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["MM_UserID"] = "99";
            Server.ScriptTimeout = 6200;
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadUserDropdownList(ddlSB, GlobalUtilArea.GetUsers(Session));
                ddlSB.Items.Insert(0, new ListItem("*** bitte auswählen ***", GlobalUtilArea.DefaultDropdownValue));
            }
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var fileName = UploadFile();
            
            if (fileName == null) return;

            var sheetNames = GetExcelSheetNames(fileName);
            
            var ok = true;
            if (sheetNames == null) 
                ok = false;
            else
            {
                foreach (var sheetName in sheetNames)
                {
                    DataTable dt = LoadXls(fileName, sheetName);
                    if (dt == null || dt.Rows == null) ok = false;
                    else
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (ok)
                                    ok &= CreateAkt(row);
                                _coutner++;
                                if (_coutner%50 == 0)
                                    Thread.Sleep(5000);
                            }
                        }
                    }
                }
            }
            if (ok)
                ctlMessage.ShowSuccess(_coutner + " Akte importiert!");

            File.Delete(fileName);
            
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/v2/intranet/aktenint/aktenint.asp");
        }
        #endregion

        private string UploadFile()
        {
            if (fileUpload.PostedFile == null || fileUpload.PostedFile.FileName == "")
            {
                ShowError("No file specified.");
                return null;
            }
            try
            {
                var fileName = fileUpload.PostedFile.FileName.Replace(" ", "_");
                var idx = fileName.LastIndexOf("\\", StringComparison.Ordinal);
                if(idx >= 0)
                {
                    fileName = fileName.Substring(idx+1);
                }
                else
                {
                    idx = fileName.LastIndexOf("/", StringComparison.Ordinal);
                    if (idx >= 0)
                    {
                        fileName = fileName.Substring(idx + 1);
                    }
                }

                var serverFileName = HTBUtils.GetConfigValue("AktenImportFolder") + fileName;
                //FileInput.PostedFile.SaveAs(@"c:\" + serverFileName);
                fileUpload.PostedFile.SaveAs(serverFileName);
                ctlMessage.ShowSuccess(" uploaded successfully.");
                return serverFileName;
            }
            catch (Exception ex)
            {
                ShowException(ex);
                return null;
            }
        }
        #region XL
        private String[] GetExcelSheetNames(string strFile)
        {
            DataTable dt = null;

            try
            {
                // Connection String. Change the excel file to the file you
                // will search.
                string connString = "";

                if (strFile.Trim().EndsWith(".xlsx"))
                {

                    connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", strFile);

                }
                else if (strFile.Trim().EndsWith(".xls"))
                {

                    connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", strFile);

                }
                // Create connection object by using the preceding connection string.
                using (var conn = new OleDbConnection(connString))
                {
                    // Open connection with the database.
                    conn.Open();
                    // Get the data table containg the schema guid.
                    dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    conn.Close();
                }

                if (dt == null)
                {
                    return null;
                }

                var excelSheets = new String[dt.Rows.Count];
                var i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                dt.Dispose();
                return excelSheets;
            }
            catch (Exception ex)
            {
                ShowException(ex);
                return null;
            }
            finally
            {
                dt.Dispose();
            }
        }
        private DataTable LoadXls(string strFile, String sheetName)
        {
            var dataTable = new DataTable(sheetName);

            try
            {
                var strConnectionString = "";

                if (strFile.Trim().EndsWith(".xlsx"))
                {

                    strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", strFile);

                }
                else if (strFile.Trim().EndsWith(".xls"))
                {

                    strConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", strFile);

                }

                using (var conn = new OleDbConnection(strConnectionString))
                {

                    conn.Open();

                    using (var adapter = new OleDbDataAdapter())
                    {
                        //                string sql = "SELECT * FROM [" + sheetName + "$] WHERE " + column + " = " + value;
                        //                string sql = "SELECT * FROM [" + sheetName + "$] ";
                        string sql = "SELECT * FROM [" + sheetName + "] ";

                        var cmd = new OleDbCommand(sql, conn);

                        adapter.SelectCommand = cmd;

                        adapter.Fill(dataTable);
                    }
                    conn.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
//            finally
//            {
//                // Clean up.
//                if (objConn != null)
//                {
//                    objConn.Close();
//                    objConn.Dispose();
//                }
//                if (dt != null)
//                {
//                    dt.Dispose();
//                }
//            }
            return dataTable;

        }
        #endregion

        private bool CreateAkt(DataRow row)
        {
            string ksvAuftragsnummer = "";
            try
            {
                int i = 0;
                string kundennummerSDbeiAG = GetSqlEncodeString(row[i++]);
                ksvAuftragsnummer = GetSqlEncodeString(row[i++]);
                if (string.IsNullOrEmpty(ksvAuftragsnummer))
                    return true;

                string sdName1 = GetSqlEncodeString(row[i++]);
                string sdName2 = GetSqlEncodeString(row[i++]);
                string aliasName = GetSqlEncodeString(row[i++]);
                
                string sdplz = GetSqlEncodeString(row[i++]);
                string sdOrt = GetSqlEncodeString(row[i++]);
                string sdStrasse = GetSqlEncodeString(row[i++]);
                string sdlkz = GetSqlEncodeString(row[i++]);
                string sdTel = GetSqlEncodeString(row[i++]);

                string ueberKapital = GetSqlEncodeString(row[i++]);
                string bezKapital = GetSqlEncodeString(row[i++]);

                string agName = GetSqlEncodeString(row[i++]);
                string agplz = GetSqlEncodeString(row[i++]);
                string agOrt = GetSqlEncodeString(row[i++]);
                string agStrasse = GetSqlEncodeString(row[i++]);
                string aglkz = GetSqlEncodeString(row[i++]);
                string sdGebDat = GetSqlEncodeString(row[i++]);
                string sb = GetSqlEncodeString(row[i++]);
                string sbMail = GetSqlEncodeString(row[i++]);
                
                string offeneZinsen = GetSqlEncodeString(row[i++]);
                string offeneAGSpesen = GetSqlEncodeString(row[i++]);
                string offeneAGKosten = GetSqlEncodeString(row[i++]);
                i++;
                string fordText = GetSqlEncodeString(row[i]);

                if (sdlkz == "AT")
                    sdlkz = "A";
                if (aglkz == "AT")
                    aglkz = "A";

                DateTime gegnerDob = GlobalUtilArea.GetDefaultDateIfConvertToDateError(sdGebDat);
                if (gegnerDob == HTBUtils.DefaultDate)
                    GetDateFromString(sdGebDat);

                DateTime aktDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtEnterDate);
                if (aktDate == HTBUtils.DefaultDate)
                    aktDate = DateTime.Now;
                DateTime aktDueDate = GlobalUtilArea.GetDefaultDateIfConvertToDateError(txtDueDate);
                if (aktDueDate == HTBUtils.DefaultDate)
                    aktDueDate = DateTime.Now.AddDays(21);

                tblGegner gegner = GetGegner(sdName1, sdName2, sdStrasse, sdplz, gegnerDob) ?? CreateGegner(new tblGegner
                                                                                                                {
                                                                                                                    GegnerName1 = sdName1,
                                                                                                                    GegnerName2 = sdName2,
                                                                                                                    GegnerName3 = aliasName,
                                                                                                                    GegnerStrasse = sdStrasse,
                                                                                                                    GegnerZipPrefix = sdlkz,
                                                                                                                    GegnerZip = sdplz,
                                                                                                                    GegnerOrt = sdOrt,

                                                                                                                    GegnerLastStrasse = sdStrasse,
                                                                                                                    GegnerLastZipPrefix = sdlkz,
                                                                                                                    GegnerLastZip = sdplz,
                                                                                                                    GegnerLastOrt = sdOrt,

                                                                                                                    GegnerGebDat = gegnerDob,
                                                                                                                    GegnerOldID = TmpOldId,
                                                                                                                    GegnerCreateDate = DateTime.Now,
                                                                                                                    GegnerPhone = sdTel,
                                                                                                                    GegnerCreateSB = HTBUtils.GetControlRecord().AutoUserId
                                                                                                                });

                tblKlient klient = GetKlient(agName, agStrasse, agplz) ?? CreateKlient(new tblKlient
                                                                                           {
                                                                                               KlientName1 = agName,
                                                                                               KlientStrasse = agStrasse,
                                                                                               KlientLKZ = aglkz,
                                                                                               KlientPLZ = agplz,
                                                                                               KlientOrt = agOrt,
                                                                                               KlientType = 5,
                                                                                               KlientOldID = TmpOldId
                                                                                           });
                if (gegner == null || klient == null)
                    return false;
                
                int aktSb = ddlSB.SelectedValue != GlobalUtilArea.DefaultDropdownValue ? Convert.ToInt32(ddlSB.SelectedValue) : HTBUtils.GetGegnerSB(gegner);
                var user = (tblUser) HTBUtils.GetSqlSingleRecord("SELECT * FROM tblUser WHERE UserId = " + aktSb, typeof (tblUser));

                int distance = 0;
                if (user != null)
                    distance = (int) HTBUtils.GetDistance(user.UserZIP, gegner.GegnerLastZip);
                

                var akt = CreateAkt(new tblAktenInt
                                        {
                                            AktIntAZ = ksvAuftragsnummer,
                                            AktIntAuftraggeber = 6,
                                            AktIntKlient = klient.KlientOldID,
                                            AktIntGegner = gegner.GegnerOldID,
                                            AktIntDatum = aktDate,
                                            AktIntStatus = 1,
                                            AktIntTermin = aktDueDate,
                                            AktIntTerminAD = aktDueDate,
                                            AktIntWeggebuehr = HTBUtils.GetWeggebuehr(distance),
                                            AktIntIZ = ksvAuftragsnummer,
                                            AktIntSB = aktSb,
                                            AKTIntDistance = distance,
                                            AKTIntKosten = GlobalUtilArea.GetZeroIfConvertToDoubleError(offeneAGKosten),
                                            AKTIntZinsen = 0,
                                            AKTIntZinsenBetrag = GlobalUtilArea.GetZeroIfConvertToDoubleError(offeneZinsen),
                                            AKTIntKSVEMail = sbMail,
                                            AKTIntAGSB = sb,
                                            AktIntOriginalMemo = fordText,
                                            AKTIntDub = chkIsDub.Checked ? 1 : 0,
                                            AktIntAktType = chkIsDub.Checked ? 4 : 1,
                                        });

                if (akt == null)
                {
                    ShowError("Akt [" + ksvAuftragsnummer + "] konte nicht gespeichert werden!");
                    return false;
                }

                CreatePos(akt.AktIntID, "Buchung lt. " + kundennummerSDbeiAG, ueberKapital, "Ursprüngl. Forderungen");
                CreatePos(akt.AktIntID, "Buchung lt. " + kundennummerSDbeiAG, offeneAGSpesen, "Spesen Auftraggeber");
                CreatePos(akt.AktIntID, "Buchung lt. " + kundennummerSDbeiAG, bezKapital, "Bereits bezahlt", true);
                _lastAktImported = akt.AktIntAZ;
            }
            catch(Exception ex)
            {
                _coutner--;
                if (!string.IsNullOrEmpty(ksvAuftragsnummer))
                {
                    ShowException(ex);
                    return false;
                }
            }
            return true;
        }

        private tblAktenInt CreateAkt(tblAktenInt akt)
        {
            try
            {
                if (RecordSet.Insert(akt))
                {
                    return (tblAktenInt)HTBUtils.GetSqlSingleRecord("SELECT  TOP 1 * FROM tblAktenInt ORDER BY AktIntID DESC", typeof(tblAktenInt));
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
            return null;
        }
        

        #region Klient
        private tblKlient GetKlient(string name1, string street, string zip)
        {
            var sb = new StringBuilder("SELECT TOP 1 KlientID, KlientOldID FROM tblKlient WHERE RTRIM(LTRIM(UPPER(KlientName1))) = '");
            sb.Append(name1.ToUpper());
            sb.Append("' AND RTRIM(LTRIM(UPPER(KlientStrasse))) = '");
            sb.Append(street.ToUpper());
            sb.Append("' AND KlientPLZ = '");
            sb.Append(zip);
            sb.Append("'");
            return (tblKlient)HTBUtils.GetSqlSingleRecord(sb.ToString(), typeof(tblKlient));
        }
        private tblKlient CreateKlient(tblKlient klient, int counter = 0)
        {
            try
            {
                if (counter > 0)
                {
                    var set = new RecordSet();
                    set.ExecuteNonQuery("DELETE tblKlient WHERE KlientOldID = '" + TmpOldId + "'");
                }
                if (RecordSet.Insert(klient))
                {
                    klient = (tblKlient) HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblKlient ORDER BY KlientID DESC", typeof (tblKlient));
                    klient.KlientOldID = "1000" + klient.KlientID + ".002";
                    if (RecordSet.Update(klient))
                    {
                        return klient;
                    }
                }
            }
            catch(Exception ex)
            {
                if (counter == 0) 
                    return CreateKlient(klient, 1);
                ShowException(ex);
            }
            return null;
        }
        #endregion
        #region Gegner
        private tblGegner GetGegner(string name1, string name2, string street, string zip, DateTime dob)
        {
            var sb = new StringBuilder("SELECT TOP 1 * FROM tblGegner WHERE RTRIM(LTRIM(UPPER(GegnerLastName1))) = '");
            sb.Append(name1.ToUpper());
            sb.Append("' AND RTRIM(LTRIM(UPPER(GegnerLastName2))) = '");
            sb.Append(name2.ToUpper());
            sb.Append("' AND RTRIM(LTRIM(UPPER(GegnerLastStrasse))) = '");
            sb.Append(street.ToUpper());
            sb.Append("' AND GegnerLastZIP = '");
            sb.Append(zip);
            sb.Append("'");
            if(dob != HTBUtils.DefaultDate)
            {
                sb.Append(" AND GegnerGebdat = '");
                sb.Append(dob.ToShortDateString());
                sb.Append("'");
            }
            return (tblGegner)HTBUtils.GetSqlSingleRecord(sb.ToString(), typeof (tblGegner));
        }
        private tblGegner CreateGegner(tblGegner gegner, int counter = 0)
        {
            try
            {
                if (counter > 0)
                {
                    var set = new RecordSet();
                    set.ExecuteNonQuery("DELETE tblGegner WHERE GegnerOldID = '" + TmpOldId + "'");
                }
                if (RecordSet.Insert(gegner))
                {
                    gegner = (tblGegner)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblGegner ORDER BY GegnerID DESC", typeof(tblGegner));
                    gegner.GegnerOldID = "1000" + gegner.GegnerID+ ".002";
                    if (RecordSet.Update(gegner))
                    {
                        return gegner;
                    }
                }
            }
            catch (Exception ex)
            {
                if (counter == 0)
                    return CreateGegner(gegner, 1);
                ShowException(ex);
            }
            return null;
        }
        #endregion
        #region Pos
        private bool CreatePos(int aktId, string posNr, string amount, string caption, bool makeNegative = false)
        {
            double amt = GlobalUtilArea.GetZeroIfConvertToDoubleError(amount);
            if (!HTBUtils.IsZero(amt))
            {
                if (makeNegative) amt *= -1;
                return RecordSet.Insert(new tblAktenIntPos
                                            {
                                                AktIntPosAkt = aktId,
                                                AktIntPosNr = posNr,
                                                AktIntPosDatum = DateTime.Now,
                                                AktIntPosDueDate = DateTime.Now,
                                                AktIntPosBetrag = amt,
                                                AktIntPosCaption = caption
                                            });
            }
            return true;

        }
        #endregion

        private DateTime GetDateFromString(string str)
        {
            if(str == null || str.Length != 8)
                return HTBUtils.DefaultDate;

            int y, m, d;
            try
            {
                y = Convert.ToInt32(str.Substring(0, 4));
                m = Convert.ToInt32(str.Substring(5, 2));
                d = Convert.ToInt32(str.Substring(7, 2));
                return new DateTime(y, m, d);
            }
            catch
            {
                return HTBUtils.DefaultDate;
            }
        }
        private string GetSqlEncodeString(object obj)
        {
            if(obj == null)
                return "";
            return obj.ToString().Replace("\"", "").Replace("'", "");
        }

        private void ShowException(Exception ex)
        {
            ctlMessage.ShowException(ex);
            ctlMessage.AppendError("<br/><br><strong> L&auml;tzte Akt importiert: " + _lastAktImported + "</strong><br/>");
        }

        private void ShowError(string error)
        {
            ctlMessage.ShowError(error);
            ctlMessage.AppendError("<br/><br><strong> L&auml;tzte Akt importiert: " + _lastAktImported + "</strong><br/>");
        }
    }
}