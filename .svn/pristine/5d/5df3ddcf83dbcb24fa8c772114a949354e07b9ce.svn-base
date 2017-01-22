using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HTB.Database;
using System.Collections;
using HTBExtras.KingBill;
using HTBUtilities;
using System.Data;
using HTB.v2.intranetx.util;
using HTB.v2.intranetx.permissions;
using HTB.Database.Views;
using HTBExtras;
using System.ComponentModel;
using HTB.Database.LookupRecords;
using System.Web;
using System.IO;

namespace HTB.v2.intranetx.customer
{
    public partial class ShowAktInk : System.Web.UI.Page
    {
        protected qryCustAktEdit _akt = new qryCustAktEdit();
        private ArrayList _intAktList = new ArrayList();
        private ArrayList _invoiceList = new ArrayList();
        private ArrayList _inkassoAktions = new ArrayList();
        private readonly ArrayList _interventionAktions = new ArrayList();
        private readonly ArrayList _aktionsList = new ArrayList();
        private tblUser _user;

        private double _initialAmount;
        private double _initialAmountPay;

        private double _initialClientCosts;
        private double _initialClientCostsPay;

        private static string _aktId;
        private static int _klientId = -1;
        private double _totalAmountTransferred;

        private readonly PermissionsEditAktInk _permissions = new PermissionsEditAktInk();
        private ArrayList _docsList = new ArrayList();

        private static readonly string lawyerPdfLetterDescription = HTBUtils.GetConfigValue("Lawyer_Pdf_Description");
        protected void Page_Load(object sender, EventArgs e)
        {
//            Session["MM_UserID"] = "545";
            _permissions.LoadPermissions(GlobalUtilArea.GetUserId(Session));
            _aktId = GlobalUtilArea.GetEmptyIfNull(Request[GlobalHtmlParams.ID]);
            //_klientId = GlobalUtilArea.GetZeroIfConvertToIntError((string)Session["MM_Klient"]);
            _user = HTBUtils.GetUser(GlobalUtilArea.GetUserId(Session));
            if (_user == null) return;
            _klientId = _user.UserKlient;

            if (!IsPostBack)
            {
                if (_aktId.Trim().Equals(string.Empty)) return;
                LoadRecords();
                if (!IsClientAllowedToViewAkt()) return;
                 if(_user.UserIsSbAdmin)
                    LoadClientSbDdl(_user.UserKlient);
                else
                    trClientSB.Visible = false;
                CombineActions();
                SetTotalInvoiceAmounts();
                SetValues();
            }
            Session["tmpInkAktID"] = Request.Params[GlobalHtmlParams.ID];
        }

        private void LoadRecords()
        {
            _interventionAktions.Clear();

            ArrayList[] results = HTBUtils.GetMultipleListsFromStoredProcedure("spGetInkassoAktData", new ArrayList
                                                                                                 {
                                                                                                     new StoredProcedureParameter("aktId", SqlDbType.Int, GlobalUtilArea.GetZeroIfConvertToIntError(_aktId))
                                                                                                 },
                                                                                                 new[]
                                                                                                 {
                                                                                                     typeof(qryCustAktEdit), 
                                                                                                     typeof(tblCustInkAktInvoice), 
                                                                                                     typeof(qryCustInkAktAktionen), 
                                                                                                     typeof(qryDoksInkAkten), 
                                                                                                     typeof(tblAktenInt), 
                                                                                                     typeof(qryMeldeResult),
                                                                                                     typeof(InvoiceIdAndAmountRecord),
                                                                                                     typeof(qryCustInkRate)
                                                                                                 }
                                                                                                 );
            _akt = (qryCustAktEdit)results[0][0];
            _invoiceList = results[1];
            _inkassoAktions = results[2];
            _docsList = results[3];
            _intAktList = results[4];
            
            if (_intAktList != null && _intAktList.Count > 0)
                foreach (tblAktenInt intAkt in _intAktList)
                {
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryInktAktAction WHERE AktIntActionAkt = " + intAkt.AktIntID + " ORDER BY AktIntActionDate DESC", typeof(qryInktAktAction)), _interventionAktions);
                    HTBUtils.AddListToList(HTBUtils.GetSqlRecords("SELECT * FROM qryDoksIntAkten WHERE AktIntID = " + intAkt.AktIntID, typeof(qryDoksIntAkten)), _docsList);
                }
            //QryInkassoakt.CustInkAktGothiaNr = (int)sw.ElapsedMilliseconds;

        }

        private void SetValues()
        {
            lblAktNumber.Text = _akt.CustInkAktID.ToString();
            if (_akt.CustInkAktOldID.Trim() != string.Empty)
                lblAktNumber.Text +=  "[" + _akt.CustInkAktOldID + "]";
            if (!string.IsNullOrEmpty(_akt.CustInkAktGothiaNr))
                lblGothiaNr.Text = _akt.CustInkAktGothiaNr;

            lblAktEnteredDate.Text = _akt.CustInkAktEnterDate.ToShortDateString();

            lblKlient.Text = "<strong>" + _akt.KlientName1 + "</strong>&nbsp;" + _akt.KlientName2 + "<br/>" +
                                        _akt.KlientStrasse + "<br/>" +
                                        _akt.KlientLKZ + "&nbsp;" + _akt.KlientPLZ + "&nbsp;" + _akt.KlientOrt;

            lblGegner.Text = "<strong>" + _akt.GegnerLastName1 + ", " + _akt.GegnerLastName2 + "</strong><br/>";
            lblGegner.Text += _akt.GegnerLastStrasse + "<br/>";
            lblGegner.Text += _akt.GegnerLastZipPrefix + "&nbsp;" + _akt.GegnerLastZip + "&nbsp;" + _akt.GegnerLastOrt;


            lblCustInkAktKunde.Text = !_akt.CustInkAktKunde.Trim().Equals("") ? _akt.CustInkAktKunde : "Keine Rechnungsnummer vorhanden!";

            lblInvoiceDate.Text = _akt.CustInkAktInvoiceDate.ToShortDateString();
            
            if(trClientSB.Visible)
                GlobalUtilArea.SetSelectedValue(ddlClientSB, _akt.CustInkAkKlientSB.ToString());

            btnSave.Visible = trClientSB.Visible;

            lblForderung.Text = HTBUtils.FormatCurrency(_initialAmount);
            lblZahlungen.Text = HTBUtils.FormatCurrency(_initialAmountPay);
            lblKostenKlientSumme.Text = HTBUtils.FormatCurrency(_initialClientCosts);
            lblKostenKlientZahlungen.Text = HTBUtils.FormatCurrency(_initialClientCostsPay);

            
            PopulateInvoicesGrid();
            PopulateDocumentsGrid();
        }

        private bool IsClientAllowedToViewAkt()
        {
            return _akt != null && _akt.KlientID == _klientId;
            
        }

        private void LoadClientSbDdl(int klientId)
        {
            if (klientId > 0)
            {
                ArrayList klientSbList = HTBUtils.GetSqlRecords("SELECT UserId, UserVorname + ' ' + UserNachname [UserName] FROM tblUser WHERE UserKlient = " + klientId + " ORDER BY UserVorname", typeof(UserLookup));
                if (klientSbList.Count == 0)
                    trClientSB.Visible = false;
                else
                {
                    GlobalUtilArea.LoadDropdownList(ddlClientSB, klientSbList, "UserId", "UserName", false);
                    GlobalUtilArea.SetSelectedValue(ddlClientSB, _user.UserID.ToString());
                }
            }
        }
        

        #region Event Handlers
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveAkt();
            LoadRecords();
            PopulateDocumentsGrid();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/v2/intranetx/customer/AktenInk.aspx");
        }
        #endregion
        private void SaveAkt()
        {
            var akt = HTBUtils.GetInkassoAkt(Convert.ToInt32(_aktId));
            akt.CustInkAkKlientSB = GlobalUtilArea.GetZeroIfConvertToIntError(ddlClientSB.SelectedValue);
            if (RecordSet.Update(akt))
            {
                var msg = "Akt Gespeichert! [" + akt.CustInkAktID + "]";
                if (!UploadFiles(akt))
                {
                    msg += "<br/> <strong>***Aber nicht alle Dokumente wurden upgeloaded***</strong>";
                }
                ctlMessage.ShowSuccess(msg);
                ctlMessageBottom.ShowSuccess(msg);
            }
            else
            {
                ctlMessage.ShowSuccess("Fehler: Akt wurde nicht gespeichert!");
                ctlMessageBottom.ShowSuccess("Fehler: Akt wurde nicht gespeichert!");
            }
        }

        private void SetTotalInvoiceAmounts()
        {
            foreach (tblCustInkAktInvoice inv in _invoiceList)
            {
                if (!inv.IsVoid())
                {
                    if (inv.IsPayment())
                    {
                    }
                    else
                    {
                        switch (inv.InvoiceType)
                        {
                            case tblCustInkAktInvoice.INVOICE_TYPE_ORIGINAL:
                                _initialAmount += inv.InvoiceAmount;
                                _initialAmountPay += inv.InvoiceAmount - inv.InvoiceBalance;
                                break;
                            case tblCustInkAktInvoice.INVOICE_TYPE_CLIENT_COST:
                                _initialClientCosts += inv.InvoiceAmount;
                                _initialClientCostsPay += inv.InvoiceAmount - inv.InvoiceBalance;
                                break;
                        }
                    }
                }
            }
        }

        private void CombineActions()
        {

            tblAktenInt intAkt = _intAktList.Count == 0 ? null : (tblAktenInt)_intAktList[0];
            foreach (qryCustInkAktAktionen inkAction in _inkassoAktions)
                _aktionsList.Add(new InkassoActionRecord(inkAction, intAkt));

            foreach (qryInktAktAction intAction in _interventionAktions)
            {
                var action = new InkassoActionRecord(intAction, intAkt);
                if (!string.IsNullOrEmpty(intAction.AktIntActionAddress))
                {
                    action.ActionCaption += "<BR/><I>" + intAction.AktIntActionAddress + "</I>";
                }
                else if (action.AktIntActionLatitude > 0 && action.AktIntActionLongitude > 0)
                {
                    string address = GlobalUtilArea.GetAddressFromLatitudeAndLongitude(action.AktIntActionLatitude, action.AktIntActionLongitude);
                    if (!string.IsNullOrEmpty(address))
                    {
                        action.ActionCaption += "<BR/><I>" + address + "</I>";
                        // Update Action [set address based on coordinates]
                        try
                        {
                            var set = new RecordSet();
                            set.ExecuteNonQuery("UPDATE tblAktenIntAction SET AktIntActionAddress = " + address + " WHERE AktIntActionID = " + intAction.AktIntActionID);
                        }
                        catch
                        {
                        }
                    }
                }
                _aktionsList.Add(action);
            }
            foreach (InkassoActionRecord rec in _aktionsList)
            {
                if (rec.ActionCaption == null || rec.ActionCaption.Trim() == string.Empty)
                    rec.ActionCaption = "&nbsp;";
                if (rec.ActionMemo == null || rec.ActionMemo.Trim() == string.Empty)
                    rec.ActionMemo = "&nbsp;";
            }
            _aktionsList.Sort(new InkassoActionRecordComparer());
        }
       
        #region Invoices
        private static ArrayList GetInvoicesList()
        {
            return HTBUtils.GetSqlRecords("SELECT * FROM tblCustInkAktInvoice WHERE InvoiceStatus <> -1 AND InvoicePaymentTransferToClientAmount > 0  AND InvoiceCustInkAktId = " + _aktId + " ORDER BY InvoiceDate", typeof(tblCustInkAktInvoice));
        }
        private void PopulateInvoicesGrid()
        {
            ArrayList invList = GetInvoicesList();
            _totalAmountTransferred = 0;
            DataTable dt = GetInvoicesDataTableStructure();
            foreach (tblCustInkAktInvoice inv in invList)
            {
                DataRow dr = dt.NewRow();
                
                dr["InvoiceID"] = inv.InvoiceID;
                
                dr["DueDate"] = "";
                dr["InvoiceDate"] = inv.InvoicePaymentReceivedDate.ToShortDateString();
                if (inv.IsBankTransferrable() && (inv.InvoicePaymentTransferToClientDate.ToShortDateString() != HTBUtils.DefaultShortDate))
                {
                    if (inv.InvoicePaymentTransferToClientDate.ToShortDateString() != HTBUtils.DefaultShortDate)
                    {
                        dr["TransferDate"] = inv.InvoicePaymentTransferToClientDate.ToShortDateString();
                        dr["TransferAmount"] = HTBUtils.FormatCurrency(inv.InvoicePaymentTransferToClientAmount);
                        _totalAmountTransferred += inv.InvoicePaymentTransferToClientAmount;
                    }
                    else
                    {
                        dr["TransferDate"] = "Noch nicht";
                        dr["TransferAmount"] = "";
                    }
                }
                dt.Rows.Add(dr);
            }
            gvInvoices.DataSource = dt;
            gvInvoices.DataBind();
        }

        private static DataTable GetInvoicesDataTableStructure()
        {
            var dt = new DataTable();

            dt.Columns.Add(new DataColumn("DeleteUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("EditPopupUrl", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceID", typeof(int)));
            dt.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("AppliedAmount", typeof(double)));
            dt.Columns.Add(new DataColumn("DueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferDate", typeof(string)));
            dt.Columns.Add(new DataColumn("TransferAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("InvoiceAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("UnappliedPay", typeof(string)));
            return dt;
        }

        protected double GetTotalTransferred()
        {
            return _totalAmountTransferred;
        }
        
        #endregion

        #region Actions
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<InkassoActionRecord> GetActions()
        {
            LoadRecords();
            CombineActions();    
            return _aktionsList.Cast<InkassoActionRecord>().ToList();
        }
        #endregion

        #region Documents
        private void PopulateDocumentsGrid()
        {
            DataTable dt = GetDocumentsDataTableStructure();
            foreach (Record rec in _docsList)
            {

                if (rec is qryDoksInkAkten)
                {
                    var doc = (qryDoksInkAkten)rec;
                    if (IsDocumentVisibleToClient(doc.DokCaption))
                    {
                        DataRow dr = dt.NewRow();

                        dr["DokAttachment"] = GetDocAttachmentLink(doc.DokAttachment);

                        dr["DokChangeDate"] = doc.DokChangeDate.ToShortDateString();
                        dr["DokTypeCaption"] = doc.DokTypeCaption;
                        dr["DokCaption"] = doc.DokCaption;

                        dt.Rows.Add(dr);
                    }
                }
                else if (rec is qryDoksIntAkten)
                {

                    var doc = (qryDoksIntAkten) rec;
                    if (IsDocumentVisibleToClient(doc.DokCaption))
                    {
                        DataRow dr = dt.NewRow();
                        dr["DokAttachment"] = GetDocAttachmentLink(doc.DokAttachment);
                        dr["DokChangeDate"] = doc.DokCreationTimeStamp.ToShortDateString();
                        dr["DokTypeCaption"] = doc.DokTypeCaption;
                        dr["DokCaption"] = doc.DokCaption;

                        dt.Rows.Add(dr);
                    }
                }
            }
            gvDocs.DataSource = dt;
            gvDocs.DataBind();
        }
        private DataTable GetDocumentsDataTableStructure()
        {
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("DokChangeDate", typeof(string)));
            dt.Columns.Add(new DataColumn("DokTypeCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DokCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DokAttachment", typeof(string)));
            return dt;
        }
        private bool IsDocumentVisibleToClient(string docCaption)
        {
            string str = docCaption.ToLower();
            return !str.StartsWith("mahnung") && 
                    !str.StartsWith("terminverlust") &&
                    !str.StartsWith("rechtsanwalt kosten") &&
                    !str.Equals(lawyerPdfLetterDescription.ToLower())
                    ;
        }
        private string GetDocAttachmentLink(string attachment)
        {
            var sb = new StringBuilder("<a href=\"javascript:void(window.open('/v2/intranet/documents/files/");
            sb.Append(attachment);
            sb.Append("','popSetAction','menubar=yes,scrollbars=yes,resizable=yes,width=800,height=800,top=10'))\">");
            sb.Append(attachment);
            sb.Append("</a>");
            return sb.ToString();
        }
        private bool UploadFiles(tblCustInkAkt akt)
        {
            try
            {
                string folderPath = HTBUtils.GetConfigValue("DocumentsFolder");
                // Get the HttpFileCollection
                HttpFileCollection hfc = Request.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFile hpf = hfc[i];
                    if (hpf.ContentLength > 0)
                    {
                        string fileName = akt.CustInkAktID + "_" + Path.GetFileName(hpf.FileName);

                        hpf.SaveAs(folderPath + fileName);
                        RecordSet.Insert(new tblDokument
                        {
                            // CollectionInvoice
                            DokDokType = 25,
                            DokCaption = HTBUtils.GetJustFileName(hpf.FileName),
                            DokInkAkt = akt.CustInkAktID,
                            DokCreator = akt.CustInkAktSB,
                            DokAttachment = fileName,
                            DokCreationTimeStamp = DateTime.Now,
                            DokChangeDate = DateTime.Now
                        });
                        var doc = (tblDokument)HTBUtils.GetSqlSingleRecord("SELECT TOP 1 * FROM tblDokument ORDER BY DokID DESC", typeof(tblDokument));
                        if (doc != null)
                        {
                            RecordSet.Insert(new tblAktenDokumente { ADAkt = akt.CustInkAktID, ADDok = doc.DokID, ADAkttyp = 1 });
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}