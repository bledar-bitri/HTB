using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Threading;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBInvoiceManager;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenink
{
    public partial class ImportFromXL : System.Web.UI.Page
    {
        protected System.Web.UI.HtmlControls.HtmlInputFile fileUpload;
        private string _lastAktImported = "";
        private const string TmpOldId = "sssss";
        int _coutner;
        tblKlient klient;
        private tblControl control = HTBUtils.GetControlRecord();

        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 6200;
            if (!IsPostBack)
            {
                GlobalUtilArea.LoadDropdownList(ddlKlient,
                      "SELECT KlientID, (CASE WHEN KlientName1 IS NULL THEN '' ELSE KlientName1 END) + ' ' + (CASE WHEN KlientName2 IS NULL THEN '' ELSE KlientName2 END) + ' ' + (CASE WHEN KlientName3 IS NULL THEN '' ELSE KlientName3 END) [KlientName1] FROM tblKlient WHERE KlientExcelInterfaceCode > 0",
                      typeof(tblKlient),
                      "KlientID",
                      "KlientName1", true);
            }
        }

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int klientId = GlobalUtilArea.GetZeroIfConvertToIntError(ddlKlient.SelectedValue);
            if (klientId <= 0)
            {
                ShowError("Bitte einen Klient ausw&auml;hlen!");
                return;
            }
            klient = HTBUtils.GetKlientById(GlobalUtilArea.GetZeroIfConvertToIntError(ddlKlient.SelectedValue));
            if (klient == null)
            {
                ShowError("Klient nicht gefunden!");
                return;
            }
            
            string fileName = UploadFile();
            if (fileName != null)
            {
                string[] sheetNames = GetExcelSheetNames(fileName);
                bool ok = true;
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
                                int startIdx = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (ok)
                                    {
                                        ok &= CreateAkt(row, startIdx);
                                        if(ok)_coutner++;
                                    }
                                    if (_coutner%50 == 0)
                                        Thread.Sleep(5000);
                                }
                            }
                        }
                    }
                }
                ctlMessage.ShowSuccess(_coutner + " Akt" + (_coutner == 1 ? " wurde" : "en wurden") + "  importiert!");
                File.Delete(fileName);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        protected void btnLoadKlientSB_Click(object sender, EventArgs e)
        {
            GlobalUtilArea.LoadDropdownList(ddlKlientSB,
                      "SELECT UserID, (CASE WHEN UserVorname IS NULL THEN '' ELSE UserVorname END) + ' ' + (CASE WHEN UserNachname IS NULL THEN '' ELSE UserNachname END) [UserVorname] FROM tblUser where UserKlient = " + GlobalUtilArea.GetZeroIfConvertToIntError(ddlKlient.SelectedValue),
                      typeof(tblUser),
                      "UserID",
                      "UserVorname", true);
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
                string fileName = fileUpload.PostedFile.FileName.Replace(" ", "_");
                int idx = fileName.LastIndexOf("\\");
                if(idx >= 0)
                {
                    fileName = fileName.Substring(idx+1);
                }
                else
                {
                    idx = fileName.LastIndexOf("/");
                    if (idx >= 0)
                    {
                        fileName = fileName.Substring(idx + 1);
                    }
                }

                string serverFileName = HTBUtils.GetConfigValue("AktenImportFolder") + fileName;
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
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

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
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                var excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                return excelSheets;
            }
            catch (Exception ex)
            {
                ShowException(ex);
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }
        private DataTable LoadXls(string strFile, String sheetName)
        {
            var dataTable = new DataTable(sheetName);

            try
            {
                string strConnectionString = "";

                if (strFile.Trim().EndsWith(".xlsx"))
                {

                    strConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", strFile);

                }
                else if (strFile.Trim().EndsWith(".xls"))
                {

                    strConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", strFile);

                }

                var conn = new OleDbConnection(strConnectionString);

                conn.Open();

                var adapter = new OleDbDataAdapter();

//                string sql = "SELECT * FROM [" + sheetName + "$] WHERE " + column + " = " + value;
//                string sql = "SELECT * FROM [" + sheetName + "$] ";
                string sql = "SELECT * FROM [" + sheetName + "] ";

                var cmd = new OleDbCommand(sql, conn);

                adapter.SelectCommand = cmd;

                adapter.Fill(dataTable);

                conn.Close();
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

        private bool CreateAkt(DataRow row, int startIdx)
        {
            string reference = "";
            try
            {
                int i = startIdx;
#region read data
                reference = GetSqlEncodeString(row[i++]);
                // debtor information (gegner)
                string salutation = GetSqlEncodeString(row[i++]).Trim();
                string title = GetSqlEncodeString(row[i++]).Trim();
                
                string firstName = GetSqlEncodeString(row[i++]);    // name 2
                string lastName = GetSqlEncodeString(row[i++]);     // name 1
                string companyName = GetSqlEncodeString(row[i++]);  // name 1 if not empty

                string birthday = GetSqlEncodeString(row[i++]);
                string streetAddress = GetSqlEncodeString(row[i++]);
                string country = GetSqlEncodeString(row[i++]);
                string zip = GetSqlEncodeString(row[i++]);
                string city = GetSqlEncodeString(row[i++]);
                string phone = GetSqlEncodeString(row[i++]);
                double originalAmount = GlobalUtilArea.GetZeroIfConvertToDoubleError(GetSqlEncodeString(row[i++]));
                double extraCosts = GlobalUtilArea.GetZeroIfConvertToDoubleError(GetSqlEncodeString(row[i++]));
                double totalOpened = GlobalUtilArea.GetZeroIfConvertToDoubleError(GetSqlEncodeString(row[i++]));
                double payments = GlobalUtilArea.GetZeroIfConvertToDoubleError(GetSqlEncodeString(row[i++]));
                string invoiceDate = GetSqlEncodeString(row[i++]);

                if (string.IsNullOrEmpty(country))
                    country = "A";
                if (country == "AT")
                    country = "A";
                string name3 = "";
                if (!string.IsNullOrEmpty(companyName))
                {
                    name3 = lastName;
                    lastName = companyName;
                }
#endregion
                // do not process empty line
                if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(companyName))
                    return false;

                DateTime gegnerDob = GlobalUtilArea.GetDefaultDateIfConvertToDateError(birthday);
                DateTime invoiceDte = GlobalUtilArea.GetDefaultDateIfConvertToDateError(invoiceDate);
                int gegnerType = 0;
                if (salutation.Equals("herr", StringComparison.CurrentCultureIgnoreCase))
                    gegnerType = 1;
                else if (salutation.Equals("frau", StringComparison.CurrentCultureIgnoreCase))
                    gegnerType = 2;




#region gegner
                tblGegner gegner = GetGegner(lastName, firstName, streetAddress, zip, gegnerDob) ?? CreateGegner(new tblGegner
                                                                                                                {
                                                                                                                    GegnerType = gegnerType,
                                                                                                                    GegnerAnrede =  title,
                                                                                                                    GegnerName1 = lastName,
                                                                                                                    GegnerName2 = firstName,
                                                                                                                    GegnerName3 = name3,
                                                                                                                    GegnerStrasse = streetAddress,
                                                                                                                    GegnerZipPrefix = country,
                                                                                                                    GegnerZip = zip,
                                                                                                                    GegnerOrt = city,

                                                                                                                    GegnerLastStrasse = streetAddress,
                                                                                                                    GegnerLastZipPrefix = country,
                                                                                                                    GegnerLastZip = zip,
                                                                                                                    GegnerLastOrt = city,

                                                                                                                    GegnerGebDat = gegnerDob,
                                                                                                                    GegnerOldID = TmpOldId,
                                                                                                                    GegnerCreateDate = DateTime.Now,
                                                                                                                    GegnerPhone = phone,
                                                                                                                    GegnerCreateSB = HTBUtils.GetControlRecord().AutoUserId
                                                                                                                });
                if (gegner == null)
                    return false;
#endregion

#region akt
                var akt = new tblCustInkAkt
                {
                    CustInkAktAuftraggeber = 41,
                    // ECP
                    CustInkAktKlient = klient.KlientID,
                    CustInkAktGegner = gegner.GegnerID,
                    CustInkAktAZ = reference,
                    CustInkAktKunde = reference,
                    CustInkAktEnterDate = DateTime.Now,
                    CustInkAktLastChange = DateTime.Now,
                    CustInkAktEnterUser = control.AutoUserId,
                    CustInkAktGothiaNr = reference,
                    CustInkAktInvoiceDate = invoiceDte,
                    CustInkAktForderung = originalAmount,
                    CustInkAktBetragOffen = totalOpened,
                    CustInkAktSB = HTBUtils.GetGegnerSB(gegner),
                    CustInkAktNextWFLStep = DateTime.Now.AddMinutes(5),
                    CustInkAktStatus = 1,
                    CustInkAktCurStatus = 1,
                    CustInkAktIsPartial = false,
                    CustInkAktSendBericht = true,
                    CustInkAktLawyerId = klient.KlientLawyerId,
                    CustInkAkKlientSB = GlobalUtilArea.GetZeroIfConvertToIntError(ddlKlientSB.SelectedValue)
                };

                akt = CreateAkt(akt);
                CreateInvoices(akt, originalAmount, extraCosts, payments);
#endregion

                _lastAktImported = string.Format("[{0}] [{1}] [{2}]", reference, lastName, firstName);
            }
            catch(Exception ex)
            {
                _coutner--;
                if (!string.IsNullOrEmpty(reference))
                {
                    ShowException(ex);
                    return false;
                }
            }
            return true;
        }

        private tblCustInkAkt CreateAkt(tblCustInkAkt akt)
        {
            try
            {
                if (RecordSet.Insert(akt))
                {
                    return (tblCustInkAkt)HTBUtils.GetSqlSingleRecord("SELECT  TOP 1 * FROM tblCustInkAkt ORDER BY CustInkAktID DESC", typeof(tblCustInkAkt));
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
                ShowError("Akt konte nicht gespeichert werden!");
            }
            return null;
        }
        
        private void CreateInvoices(tblCustInkAkt akt, double originalAmount, double extraCosts, double paid)
        {
            var invMgr = new InvoiceManager();
            if(originalAmount > 0) 
            invMgr.CreateAndSaveInvoice(akt.CustInkAktID, tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL, akt.CustInkAktForderung, "Kapital - Forderung", false);
            if (extraCosts > 0)
            {
                invMgr.CreateAndSaveInvoice(akt.CustInkAktID, tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST, extraCosts, "Klient - Kosten", false);
            }
            if (paid > 0)
            {
                int paymentId = invMgr.CreateAndSavePayment(akt.CustInkAktID, tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_INITIAL_PAYMENT, paid, DateTime.Now);
                if(paymentId > 0) 
                    invMgr.ApplyPayment(paymentId);
            }
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